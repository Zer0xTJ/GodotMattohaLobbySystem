@tool
extends EditorPlugin

func _enter_tree():
	add_custom_type("MattohaGameHolder", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaGameHolder.cs"), null)
	add_custom_type("MattohaSpawner", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaSpawner.cs"), null)
	add_custom_type("MattohaServer", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaServer.cs"), null)
	add_custom_type("MattohaClient", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaClient.cs"), null)

	add_autoload_singleton("MattohaSystem", "res://addons/mattoha_lobby_system/core/autoload/MattohaSystem.tscn")
	add_autoload_singleton("MattohaSystemGD", "res://addons/mattoha_lobby_system/gd_bind/nodes/mattoha_system.gd")

func _exit_tree():
	# Clean-up of the plugin goes here.
	remove_custom_type("MattohaGameHolder")
	remove_custom_type("MattohaSpawner")
	remove_custom_type("MattohaServer")
	remove_custom_type("MattohaClient")

	remove_autoload_singleton("MattohaSystem")
	remove_autoload_singleton("MattohaSystemGD")
