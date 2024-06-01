@tool
extends EditorPlugin

func _enter_tree():
	add_autoload_singleton("MattohaSystem", "res://addons/mattoha_lobby_system/core/autoload/MattohaSystem.tscn")
	add_custom_type("MattohaGameHolder", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaGameHolder.cs"), null)
	add_custom_type("MattohaSynchronizerModifer", "Node2D", preload ("res://addons/mattoha_lobby_system/core/nodes/MattohaSynchronizerModifier.cs"), null)

func _exit_tree():
	# Clean-up of the plugin goes here.
	remove_custom_type("MattohaGameHolder")
	remove_custom_type("MattohaSynchronizerModifer")
	pass
