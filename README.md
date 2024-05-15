( !! Pray for Gaza / Israeli occupation is doing genocide against Innocent Gaza People and children !!)


# Multi-Lobby System for Godot Engine (C#)

Mattoha Lobby System is an addon designed to simplify multiplayer game development in Godot using C#! This addon offers a robust framework for managing multiple lobbies with a single server instance, allowing for dynamic and engaging multiplayer experiences.

## Features:

- **Multi-Lobby Management:** Easily create and manage multiple lobbies within your game.
- **Centralized Server:** Streamline server management with a single server instance for all lobbies.
- **Auto Replication:** Ensure consistency between clients with automatic data replication.
- **Dynamic Spawning:** Seamlessly spawn and manage game entities within each lobby.
- **Easy Integration:** Designed for C# developers with a user-friendly API.
- **Extensibility:** Customize and extend functionality to suit your game's needs.

Get started with multiplayer game development in Godot today using the Multi-Lobby System addon!


## Have any ideas ?
DM me on X-platform - https://x.com/zer0xtj , It would be appreciated if you write in Arabic.

## Donation
Buy me a cofee - https://www.buymeacoffee.com/zer0xtj

## Requirements
- Godot Engine .NET 4.2+
- Visual Studio Community 2022


## Installation
- You can download the source code and copy/paste the addon to your godot addons folder, then enable it from plugin tab in your project settings
- Or you can download it from asset lib in godot engine

## Usage
There is an example that comes with MattohaLobbySystem Addon , you can discover it on your own, or you can watch a youtube video .
- Youtube [ English ] :
- Youtube [ Arabic ] :

## Step By Step Explination
before we start, configure your .csproject to be similar to this:
```
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
When you want to distrubite your game for players, disable MATTOHA_SERVER constant and keep MATTOHA_CLIENT, this will protect your server side code.
This configuration will help you to debug your code easly.

After Enabling The addon on your project, Create a new node and call it whatever you want.
then attach a C# script to it and call it `MyLobbyManager` - as name of the class, class snippet :
```Csharp
public partial class MyLobbyManager : Node
{
	public static MattohaSystem? System { get; private set; }

	public override void _EnterTree()
	{
#if MATTOHA_SERVER
		GD.Print("#Is MATTOHA_SERVER : true");
#endif
#if MATTOHA_CLIENT
		GD.Print("#Is MATTOHA_CLIENT : true");
#endif
		System = (MattohaSystem)GetNode("MattohaSystem");
		base._EnterTree();
	}
}
```

Now compile your code, and add a child node of type `MattohaSystem`, you will find this node when you enable MattohaLobbySystem addon, we didn't finished yet, 
now we want 2 children for our `MattohaSystem` node, we need `MattohaServer` & `MattohaClient` nodes as sibling nodes under `MattohaSystem` go ahead and add them.
Your tree would be something like this;
```
- MyLobbyManager
  - MattohaSystem
    - MattohaClient
    - MattohaServer
```
Now, from inspector, edit MattohaSystem proeprties and assign `Server` & `Client` nodes to it.
You can also configure the system proeprties like `IP`, `PORT`, `MaxPlayersPerLobby` etc .

Last but not least, go ahead and add this scene to autoload in your project settings.
