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
after installing and enable MattohaLobbySystem, a new auto load will be exists in your project with name `MattohaSystem`, you can configure it as you want.
In addition you can access the system methods as following:

- For c#
```csharp
// starting server
MattohaSystem.Instance.Server.StartServer();

// starting client (connect to server)
MattohaSystem.Instance.Client.StartClient();

// Creating a lobby, lobbyDictionary is a GodotDictionary<string, Variant>
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
MattohaSystem.Client.CreateLobby(lobbyDictionary)

# listining for a creating lobby signals.
MattohaSystem.Client.CreateLobbySucceed.connect(_on_create_lobby)         # on success
MattohaSystem.Client.CreateLobbyFailed.connect(_on_create_lobby_failed_)  # on fail

```

There is a lot of signals and methods you can use , check the API documentation:
https://zer0xtj.github.io/GodotMattohaLobbySystem/


## Export Notes

When you export your game for the server, ensure to remove MATTOHA_CLIENT from csproject constants values for conditional compilation, same for client version, you should remove MATTOHA_SERVER for security reasons.


## API Documentation
https://zer0xtj.github.io/GodotMattohaLobbySystem/
