using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;
using System.Linq;

namespace Mattoha.Nodes;
public partial class MattohaSystem : Node
{

	[Signal] public delegate void ServerRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);
	[Signal] public delegate void ClientRpcRecievedEventHandler(string methodName, Dictionary<string, Variant> payload, long sender);

	[ExportGroup("Server Configuration"), Export] public string Address { get; set; } = "127.0.0.1";
	[ExportGroup("Server Configuration"), Export] public int Port { get; set; } = 7001;
	[ExportGroup("Server Configuration"), Export] public int MaxChannels { get; set; } = 5;
	[ExportGroup("Server Configuration"), Export] public int MaxPlayers { get; set; } = 4000;
	[ExportGroup("Server Configuration"), Export] public int MaxPlayersPerLobby { get; set; } = 10;
	public int MaxLobbies => MaxPlayers / MaxPlayersPerLobby;

	[ExportGroup("Server Configuration"), Export] public bool AutoLoadAvailableLobbies { get; set; } = true;

	[ExportGroup("System Nodes"), Export] public MattohaServer Server { get; private set; }
	[ExportGroup("System Nodes"), Export] public MattohaClient Client { get; private set; }

	[ExportGroup("Lobby Configuration"), Export] public long LobbySize { get; set; } = 2500;


	public static MattohaSystem Instance { get; set; }


	public override void _Ready()
	{
		Instance = this;
		Client.SpawnNodeRequested += OnSpawnNodeRequested;
		Client.DespawnNodeRequested += OnDespawnNodeRequested;
		base._Ready();
	}


	public static int ExtractLobbyId(string nodePath)
	{
		string lobbyName = nodePath.Split("/")[3];
		string lobbyId = lobbyName.Split("Lobby")[1];
		return int.Parse(lobbyId);
	}


	public void StartServer()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port, MaxPlayers, MaxChannels);
		Multiplayer.MultiplayerPeer = peer;
	}


	public void StartClient()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(Address, Port);
		Multiplayer.MultiplayerPeer = peer;
	}


	public Node CreateInstance(PackedScene scene)
	{
		var owner = Multiplayer.GetUniqueId();
		var instance = scene.Instantiate();
		instance.Name = instance.Name.ToString().Replace("@", "_");
		instance.Name += $"_{owner}_{Time.GetTicksMsec()}";
		instance.SetMultiplayerAuthority(owner);
		return instance;
	}

	public void SpawnNodeFromPayload(Dictionary<string, Variant> payload)
	{
		var sceneFile = payload[MattohaSpawnKeys.SceneFile].ToString();
		var node = GD.Load<PackedScene>(sceneFile).Instantiate();
		node.Name = payload[MattohaSpawnKeys.NodeName].ToString();
		node.SetMultiplayerAuthority(payload[MattohaSpawnKeys.Owner].AsInt32());
		var parentPath = payload[MattohaSpawnKeys.ParentPath].ToString();
		if (node is Node2D)
		{
			(node as Node2D).Position = payload[MattohaSpawnKeys.Position].AsVector2();
			(node as Node2D).Rotation = payload[MattohaSpawnKeys.Rotation].As<float>();
		}
		if (node is Node3D)
		{
			(node as Node3D).Position = payload[MattohaSpawnKeys.Position].AsVector3();
			(node as Node3D).Rotation = payload[MattohaSpawnKeys.Rotation].As<Vector3>();
		}
		if (GetTree().Root.HasNode(parentPath))
		{
			var parent = GetNode(parentPath);
			if (!parent.HasNode(node.Name.ToString()))
			{
				parent.AddChild(node);
			}
		}
	}


	public void DespawnNodeFromPayload(Dictionary<string, Variant> payload)
	{
		if (GetTree().Root.HasNode(payload[MattohaSpawnKeys.ParentPath].ToString()))
		{
			var parent = GetNode(payload[MattohaSpawnKeys.ParentPath].ToString());
			if (parent.HasNode(payload[MattohaSpawnKeys.NodeName].ToString()))
			{
				parent.GetNode(payload[MattohaSpawnKeys.NodeName].ToString()).QueueFree();
			}
		}
	}

	private void OnSpawnNodeRequested(Dictionary<string, Variant> lobbyData)
	{
		SpawnNodeFromPayload(lobbyData);
	}


	private void OnDespawnNodeRequested(Dictionary<string, Variant> nodeData)
	{
		DespawnNodeFromPayload(nodeData);
	}

	public Node CreateInstance(string sceneFile)
	{
		return CreateInstance(GD.Load<PackedScene>(sceneFile));
	}


	public Dictionary<string, Variant> GenerateNodePayloadData(Node node)
	{
		var payload = new Dictionary<string, Variant>()
		{
			{ MattohaSpawnKeys.Owner, node.GetMultiplayerAuthority() },
			{ MattohaSpawnKeys.SceneFile, node.SceneFilePath },
			{ MattohaSpawnKeys.NodeName, node.Name },
			{ MattohaSpawnKeys.ParentPath, node.GetParent().GetPath().ToString() },
		};
		if (node is Node2D)
		{
			payload[MattohaSpawnKeys.Position] = (node as Node2D).Position;
			payload[MattohaSpawnKeys.Rotation] = (node as Node2D).Rotation;
		}

		if (node is Node3D)
		{
			payload[MattohaSpawnKeys.Position] = (node as Node3D).Position;
			payload[MattohaSpawnKeys.Rotation] = (node as Node3D).Rotation;
		}

		return payload;

	}


	public void SendReliableServerRpc(string methodName, Dictionary<string, Variant> payload)
	{
		RpcId(1, nameof(ServerReliableRpc), methodName, payload);
	}


	public void SendReliableClientRpc(long peer, string methodName, Dictionary<string, Variant> payload)
	{
		if (peer == 0)
		{
			Rpc(nameof(ClientReliableRpc), methodName, payload);
		}
		else
		{
			if (Multiplayer.GetPeers().ToList().Contains((int)peer))
			{
				RpcId(peer, nameof(ClientReliableRpc), methodName, payload);
			}
		}
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 2)]
	private void ServerReliableRpc(string methodName, Dictionary<string, Variant> payload)
	{
#if MATTOHA_SERVER
		EmitSignal(SignalName.ServerRpcRecieved, methodName, payload, Multiplayer.GetRemoteSenderId());
#endif
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 2)]
	private void ClientReliableRpc(string methodName, Dictionary<string, Variant> payload)
	{
#if MATTOHA_CLIENT
		EmitSignal(SignalName.ClientRpcRecieved, methodName, payload, Multiplayer.GetRemoteSenderId());
#endif
	}

}