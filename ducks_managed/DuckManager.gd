extends Node2D

signal added_ducks(count)

export var starting_ducks = 10

export (PackedScene) var duck_scene
export (Array, Vector2) var spawn_points

export var grid_size = 16
export var move_delay = 1

var time_since_move = 0
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


func _physics_process(delta):
	time_since_move += delta
	
	if time_since_move > move_delay:
		time_since_move = 0

		for duck in get_children():
			move_duck(duck);


func move_duck(instance):
	instance.raycast_right.force_raycast_update()
	instance.raycast_left.force_raycast_update()
	instance.raycast_up.force_raycast_update()
	instance.raycast_down.force_raycast_update()

	if instance.next_direction >= 0 and instance.next_direction < 25:
		if not instance.raycast_right.is_colliding():
			instance.transform.origin.x += grid_size
			instance.length_moved += 1
		else:
			instance.next_direction = choose_direction()
	if instance.next_direction >= 25 and instance.next_direction < 50:
		if not instance.raycast_left.is_colliding():
			instance.transform.origin.x -= grid_size
			instance.length_moved += 1
		else:
			instance.next_direction = choose_direction()
	if instance.next_direction >= 50 and instance.next_direction < 75:
		if not instance.raycast_up.is_colliding():
			instance.transform.origin.y -= grid_size
			instance.length_moved += 1
		else:
			instance.next_direction = choose_direction()	
	if instance.next_direction >= 75 and instance.next_direction < 100:
		if not instance.raycast_down.is_colliding():
			instance.transform.origin.y += grid_size
			instance.length_moved += 1
		else:
			instance.next_direction = choose_direction()	

	if instance.length_moved > instance.next_length:
		instance.next_direction = choose_direction()
		instance.next_length = choose_length()


func choose_direction():
	return randi() % 99


func choose_length():
	return randi() % 40
