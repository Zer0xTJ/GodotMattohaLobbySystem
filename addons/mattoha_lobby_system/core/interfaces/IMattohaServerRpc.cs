namespace MattohaLobbySystem.Core.Interfaces
{
	/// <summary>
	/// An interface for Server RPCs that should be exists on both, client and server.
	/// </summary>
	public interface IMattohaServerRpc
	{

		/// <summary>
		/// Server Rpc sent by client with method name to execute on server.
		/// </summary>
		/// <param name="method">method name, one of MattohaServerRpcMethods enum.</param>
		/// <param name="payload">the payload required to execute the logic.</param>
		public void ServerRpc(string method, string payload);
	}
}
