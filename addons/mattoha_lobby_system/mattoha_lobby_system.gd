@tool
extends EditorPlugin

func _enter_tree():
	add_autoload_singleton("MattohaSystem", "res://addons/mattoha_lobby_system/core/autoload/MattohaSystem.tscn")

func _exit_tree():
	# Clean-up of the plugin goes here.
	pass
