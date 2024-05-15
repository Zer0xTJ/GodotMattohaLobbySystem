namespace MattohaLobbySystem.Core.Interfaces
{
	/// <summary>
	/// An interface for client RPCs that should be exists on both, client and server.
	/// </summary>
	public interface IMattohaClientRpc
	{
		/// <summary>
		/// Client RPC sent by server whe something is fail to notify client.
		/// </summary>
		/// <param name="failCause">Faile Message</param>
		/// <param name="mattohaFailType"></param>
		public void ClientRpcFail(string mattohaFailType, string failCause);


		/// <summary>
		/// Client Rpc sent by server with method name to execute on client device.
		/// </summary>
		/// <param name="method">method name, one of MattohaClientRpcMethods enum.</param>
		/// <param name="jsonPayload">the payload required to execute the logic.</param>
		public void ClientRpc(string method, string jsonPayload);

	}
}
