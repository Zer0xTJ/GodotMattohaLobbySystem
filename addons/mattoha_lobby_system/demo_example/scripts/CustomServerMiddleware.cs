using Godot;
using MattohaLobbySystem.Core;
using MattohaLobbySystem.Core.Models;
using MattohaLobbySystem.Core.Nodes;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Demo;
public partial class CustomServerMiddleware : MattohaServerMiddleware
{
#if MATTOHA_SERVER
	public override MattohaMiddlewareResponse BeforeSetPlayerData(MattohaServerBase server, JsonObject player)
	{
		GD.Print("Doing some logic on custom middleware");
		return new MattohaMiddlewareResponse
		{
			Status = true,
		};
	}
#endif
}
