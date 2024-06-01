using Godot;
using Godot.Collections;
using Mattoha.Core;
using Mattoha.Core.Demo;
using Mattoha.Core.Utils;
using System;

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
	[ExportGroup("Server Configuration"), Export] public bool DespawnPlayerNodesOnLeave { get; set; } = true;

	[ExportGroup("System Nodes"), Export] public MattohaServer Server { get; private set; }
	[ExportGroup("System Nodes"), Export] public MattohaClient Client { get; private set; }

	[ExportGroup("Lobby Configuration"), Export] public long LobbySize { get; set; } = 2500;


	public static MattohaSystem Instance { get; set; }


	public override void _Ready()
	{
		Client.SpawnNodeSucceed += OnSpawnNode;
		Client.DespawnNodeSucceed += OnDespawnNode;
		Instance = this;
		base._Ready();
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
		var instance = scene.Instantiate();
		instance.Name = instance.Name.ToString().Replace("@", "_");
		instance.Name += $"_{Multiplayer.GetUniqueId()}_{Client.CurrentLobby[MattohaLobbyKeys.Id]}_{Time.GetTicksMsec()}";
		instance.SetMultiplayerAuthority(Multiplayer.GetUniqueId());
		return instance;
	}

	public Node CreateInstance(string sceneFile)
	{
		return CreateInstance(GD.Load<PackedScene>(sceneFile));
	}


	public void SpawnNode(Node node, bool spawnByServer = false)
	{
		Dictionary<string, Variant> payload = new()
		{
			{ MattohaSpawnKeys.SceneFile, node.SceneFilePath },
			{ MattohaSpawnKeys.ParentPath, node.GetParent().GetPath().ToString() },
			{ MattohaSpawnKeys.NodeName, node.Name.ToString() },
			{ MattohaSpawnKeys.Owner, node.GetMultiplayerAuthority() },
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
		if(lobbyId == 0)
		{
			payload[MattohaSpawnKeys.LobbyId] = Client.CurrentPlayer[MattohaPlayerKeys.JoinedLobbyId].AsInt32();
		}

		if (!Multiplayer.IsServer())
		{
			SendReliableServerRpc(nameof(ServerRpc.SpawnNode), payload);
		}
	}


	public void OnSpawnNode(Dictionary<string, Variant> payload)
	{
		GD.Print("Im gonna spawn: ", payload);
		var parentPath = payload[MattohaSpawnKeys.ParentPath].AsString();
		var scene = GD.Load<PackedScene>(payload[MattohaSpawnKeys.SceneFile].AsString());
		var instanceName = payload[MattohaSpawnKeys.NodeName].AsString(); ;

		if (!GetTree().Root.HasNode(parentPath))
			return;
		if (GetNode(parentPath).HasNode(instanceName))
			return;

		var instance = scene.Instantiate();
		instance.Name = instanceName;
		instance.SetMultiplayerAuthority(payload[MattohaSpawnKeys.Owner].AsInt32());
		if (instance is Node2D)
		{
			(instance as Node2D).Position = payload[MattohaSpawnKeys.Position].AsVector2();
			(instance as Node2D).Rotation = payload[MattohaSpawnKeys.Rotation].As<float>();
		}
		if (instance is Node3D)
		{
			(instance as Node3D).Position = payload[MattohaSpawnKeys.Position].AsVector3();
			(instance as Node3D).Rotation = payload[MattohaSpawnKeys.Rotation].AsVector3();
		}

		GetNode(parentPath).AddChild(instance);
	}


	public void DespawnNode(Node node)
	{
		Dictionary<string, Variant> despawnPayload = new() {
			{ MattohaSpawnKeys.NodeName, node.Name },
			{ MattohaSpawnKeys.ParentPath, node.GetParent().GetPath().ToString() }
		};
		SendReliableServerRpc(nameof(ServerRpc.DespawnNode), despawnPayload);
	}

	private void OnDespawnNode(string nodePath)
	{
		if (GetTree().Root.HasNode(nodePath))
		{
			GetNode(nodePath).QueueFree();
		}
	}


	public void SendReliableServerRpc(string methodName, Dictionary<string, Variant> payload)
	{
		RpcId(1, nameof(ServerReliableRpc), methodName, payload);
	}


	public void SendReliableClientRpc(long peer, string methodName, Dictionary<string, Variant> payload)
	{
		RpcId(peer, nameof(ClientReliableRpc), methodName, payload);
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