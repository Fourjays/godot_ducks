extends Node2D

signal added_ducks(count)

export var starting_ducks = 10

export (PackedScene) var duck_scene
export (Array, Vector2) var spawn_points

var duck_count = 0


func _ready():
	for _i in range (starting_ducks):
		add_duck()


func find_spawn_position():
	var spawn_index = randi() % (spawn_points.size() - 1)
	return spawn_points[spawn_index]


func add_duck():
	var duck_instance = duck_scene.instance()
	duck_instance.transform.origin = find_spawn_position()
	add_child(duck_instance)

	duck_count += 1
	emit_signal ("added_ducks", duck_count)