extends CharacterBody2D

@export var speed = 300.0
@export var Foo: String

func _enter_tree():
	if(MattohaSystem.IsNodeOwner(self)):
		Foo = "Foo_%d" % (randi() % 200)
		$Sprite2D.scale = Vector2(0.25, 0.25)

func _process(_delta):
	if (multiplayer.get_unique_id() != get_multiplayer_authority()):
		return
	move_player()

func move_player():
	var direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	if direction:
		velocity = direction * speed
	else:
		velocity = Vector2.ZERO
	move_and_slide()
