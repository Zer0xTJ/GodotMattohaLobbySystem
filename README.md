( !! Pray for Gaza / Israeli occupation is doing genocide against Innocent Gaza People and children !!)

# Multi-Lobby System for Godot Engine

Mattoha Lobby System is an addon designed to simplify multiplayer game development in Godot using C# & GDScript, This addon offers a robust framework for managing multiple lobbies with a single server instance, allowing for dynamic and engaging multiplayer experiences.

## Features:

-   **Multi-Lobby Management:** Easily create and manage multiple lobbies within your game.
-   **Centralized Server:** Streamline server management with a single server instance for all lobbies.
-   **Auto Replication:** Ensure consistency between clients with automatic data replication.
-   **Dynamic Spawning:** Seamlessly spawn and manage game entities within each lobby.
-   **Easy Integration:** Designed with a user-friendly API.
-   **Extensibility:** Customize and extend functionality to suit your game's needs.
-   **ServerMiddleware:** customize your Before & After almost all server events.
-   **UnhandledRpc:** MattohaLobbySystem allows you to send a custom RPC and handle it at your own, for both client & server.

## Development
This addon is still in devlopment stage and new versions may have a breaking changes, it needs some performance optimization too, We welcome any contributions that helps in "documentations, demos, optimizations, improvement and bug fixes" .

## Demo
MattohaLobbySystem comes with demos for c# & gdscript , you can watch the demo on youtube too:
Youtube demo: https://www.youtube.com/watch?v=9CdeYuuKfWo

## Have any ideas ?

DM me on X-platform - https://x.com/zer0xtj , It would be appreciated if you write in Arabic.

## Donation

Buy me a cofee - https://www.buymeacoffee.com/zer0xtj

## Requirements

-   Godot Engine .NET 4.2+

## Installation

-   You can download the source code and copy/paste the addon to your godot addons folder, then enable it from plugin tab in your project settings
-   Or you can download it from asset lib in godot engine

## Usage

There is an example that comes with MattohaLobbySystem Addon , you can discover it on your own, or you can watch a youtube video .

-   Youtube [ English ] : comming soon
-   Youtube [ Arabic ] : comming soon

## Getting Started

before we start, configure your .csproject to be similar to this:

```xml
<Project Sdk="Godot.NET.Sdk/4.2.2">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'ios' ">net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DefineConstants>$(DefineConstants);MATTOHA_SERVER;MATTOHA_CLIENT</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='ExportRelease|AnyCPU'">
    <DefineConstants>$(DefineConstants);MATTOHA_CLIENT,MATTOHA_SERVER</DefineConstants>
  </PropertyGroup>
</Project>
```

When you want to distrubite your game for players, disable `MATTOHA_SERVER` constant and keep `MATTOHA_CLIENT`, this will protect your server side code.
This configuration will help you to debug your code easly.

## Setting Up Nodes
after installing and enable MattohaLobbySystem, a new auto load will be exists in your project with name `MattohaSystem`, you can configure it from `res://addons/mattoha_lobby_system/core/autoload/MattohaSystem.tscn` as you want.
In addition you can access the system methods as following:

- For c#
```csharp
// starting server
MattohaSystem.Instance.Server.StartServer();

// starting client (connect to server)
MattohaSystem.Instance.Client.StartClient();

// Creating a lobby, lobbyDictionary is a GodotDictionary<string, Variant>
var lobbyDictionary = new Godot.Collections.Dictionary<string, Variant>(){
    { "Name", "My Lobby" },
    { "LobbySceneFile", "res://my_game_scene.tscn" }, // IMPORTANT
};
MattohaSystem.Instance.Client.CreateLobby(lobbyDictionary);

// listining for a creating lobby signals.
MattohaSystem.Instance.Client.CreateLobbySucceed += OnCreateLobby; // on success
MattohaSystem.Instance.Client.CreateLobbyFailed += OnCreateLobbyFailed; // on fail

```

- For GDScript

```gdscript
# starting server
MattohaSystem.Server.StartServer()

# starting client (connect to server)
MattohaSystem.Client.StartClient()

# Creating a lobby, lobbyDictionary is a GodotDictionary<string, Variant>
MattohaSystem.Client.CreateLobby({"Name": "My Lobby", "LobbySceneFile": "res://my_game_scene.tscn"})

# listining for a creating lobby signals.
MattohaSystem.Client.CreateLobbySucceed.connect(_on_create_lobby)         # on success
MattohaSystem.Client.CreateLobbyFailed.connect(_on_create_lobby_failed_)  # on fail

```

# GameHolder
When a lobby game is started, we will be listining for a `StartGameSucceed`signal, that comes with the lobby dictionary,
but instead of changing the scene to the game scene, 

we must navigate user to a GameHolder scene, so lets setup our game holder scene.

- Create a new scene (Node2D / Node3D).
- Now attach `MattohaGameHolder` script for it, it can be found on `res://addons/mattoha_lobby_system/core/nodes/MattohaGameHolder.cs`.

Note that `MattohaGameHolder` is responsible for loading the scene and spawning / despawning lobby nodes of the game play.

# Spawning/Depsawning Nodes & Replication
Every node you want to auto spawn/despawn should has a `MattohaSpawner` node child.
`MattohaSpawner` has the following properties:

- `Auto Spawn`
- `Auto Despawn`
- `Spawn For Team Only`
- `Is Scene Node`.

Configuring our nodes to spawn & despawn:
- When a node is a scene node (meaning that it's already exists in scene design and it's able to be despawned) then you must set `Is Scene Node` to true and disable `Auto Spawn` because its already spawned.
- To Enable Replication Add `MultiplayerSynchronizer` node to the node you want to replicate and configure it and add what ever properties you want to replicate, BUT you "MUST" add `MattohaSynchronizerModifier` as a child for `MultiplayerSynchronizer`
- When you set `Spawn For Team Only` to true on `MattohaSpawner`, you must set `Replicate For Team Only` in the `MattohaSynchronizerModifier` too.

After setting up these configurations correctly for nodes you want to spawn, you are ready to move to next step..

# Creating Node instance to spawn.
Replicated nodes must have same name in the scene, and it's hard to create a custom name for every node, this is why MattohaSystem comes with a great method to help you out.

Creating node instance must be as following:

- CSharp:
```csharp
// where `scene` is a PackedScene or a scene file path for example 'res://myscene.tscn'
var instance = MattohaSystem.Instance.CreateInstance(scene);
// after creating the instance, we must add it to tree
AddChild(instance);
```


- GDScript:
```gdscript
# where `scene` is a PackedScene or a scene file path for example 'res://myscene.tscn'
var instance = MattohaSystem.CreateInstance(scene)
# after creating the instance, we must add it to tree
add_child(instance)
```
The instance should now spawn for all players in lobby or (in same team if configured to be spawn for team only), because `MattohaSpawner` will auto spawn and despawn nodes for all players in same lobby or team.



There is a lot of signals and methods you can use , check the API documentation:
https://zer0xtj.github.io/GodotMattohaLobbySystem/annotated.html


## Export Notes

When you export your game for the server, ensure to remove MATTOHA_CLIENT from csproject constants values for conditional compilation, same for client version, you should remove MATTOHA_SERVER for security reasons.


## API Documentation
https://zer0xtj.github.io/GodotMattohaLobbySystem/annotated.html
