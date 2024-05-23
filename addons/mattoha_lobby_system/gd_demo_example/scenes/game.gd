extends Node2D

@export var PlayerScene: PackedScene

func _ready():
	LobbyManager.system.Client.SpawnAvailableNodes()
	LobbyManager.system.Client.DespawnRemovedSceneNodes()
	spawn_player()

func spawn_player():
	var player_instance = LobbyManager.system.CreateInstanceFromPackedScene(PlayerScene) as Node
	add_child(player_instance)
	LobbyManager.system.Client.SpawnNode(player_instance)
