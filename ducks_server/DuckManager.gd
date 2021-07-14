extends Node2D

# Struct to hold data.
class Duck:
	var ci_rid
	var position = Vector2.ZERO
	var next_direction = 0
	var next_length = 0
	var length_moved = 0

signal added_ducks(count)

export var starting_ducks = 10

export (Array, Vector2) var spawn_points
export (Texture) var duck_texture
export (Vector2) var duck_source_grid

export var grid_size = 16
export var move_delay = 1

var time_since_move = 0
var all_ducks = []

onready var space_state = get_world_2d().direct_space_state


func _ready():
	for _i in range (starting_ducks):
		add_duck()


func find_spawn_position():
	var spawn_index = randi() % (spawn_points.size() - 1)
	return spawn_points[spawn_index]


func add_duck():
	var spawn_position = find_spawn_position()

	var ci_rid = VisualServer.canvas_item_create()
	var destination_rect = Rect2(Vector2.ZERO, Vector2(grid_size, grid_size));
	var source_rect = Rect2(duck_source_grid * grid_size, Vector2(grid_size, grid_size));
	var xform = Transform2D().translated(spawn_position)

	VisualServer.canvas_item_set_parent(ci_rid, get_canvas_item())
	VisualServer.canvas_item_add_texture_rect_region(ci_rid, destination_rect, duck_texture, source_rect)
	VisualServer.canvas_item_set_transform(ci_rid, xform)

	var duck = Duck.new()
	duck.ci_rid = ci_rid
	duck.position = xform.origin
	duck.next_direction = choose_direction()
	duck.next_length = choose_length()

	all_ducks.append(duck)
	emit_signal ("added_ducks", all_ducks.size())


func _physics_process(delta):
	time_since_move += delta
	
	if time_since_move > move_delay:
		time_since_move = 0

		for duck in all_ducks:
			move_duck(duck);


func move_duck(instance):
	var position_center = instance.position + Vector2(8,8)

	if instance.next_direction >= 0 and instance.next_direction < 25:
		if space_state.intersect_ray(position_center, position_center + Vector2(grid_size, 0)):
			instance.next_direction = choose_direction()
		else:
			instance.position.x += grid_size
			instance.length_moved += 1
	if instance.next_direction >= 25 and instance.next_direction < 50:
		if space_state.intersect_ray(position_center, position_center + Vector2(-grid_size, 0)):
			instance.next_direction = choose_direction()
		else:
			instance.position.x -= grid_size
			instance.length_moved += 1
	if instance.next_direction >= 50 and instance.next_direction < 75:
		if space_state.intersect_ray(position_center, position_center + Vector2(0, -grid_size)):
			instance.next_direction = choose_direction()
		else:
			instance.position.y -= grid_size
			instance.length_moved += 1
	if instance.next_direction >= 75 and instance.next_direction < 100:
		if space_state.intersect_ray(position_center, position_center + Vector2(0, grid_size)):
			instance.next_direction = choose_direction()
		else:
			instance.position.y += grid_size
			instance.length_moved += 1

	if instance.length_moved > instance.next_length:
		instance.next_direction = choose_direction()
		instance.next_length = choose_length()

	var xform = Transform2D().translated(instance.position)
	VisualServer.canvas_item_set_transform(instance.ci_rid, xform)


func choose_direction():
	return randi() % 99


func choose_length():
	return randi() % 40
