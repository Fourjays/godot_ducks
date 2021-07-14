extends Sprite

export var grid_size = 16
export var move_delay = 1

var next_direction = 0
var next_length = 0

var length_moved = 0
var time_since_move = 0

onready var raycast_right = $RayCastRight
onready var raycast_left = $RayCastLeft
onready var raycast_up = $RayCastUp
onready var raycast_down = $RayCastDown


func _ready():
	choose_direction()
	choose_length()


func _physics_process(delta):
	time_since_move += delta
	
	if time_since_move > move_delay:
		time_since_move = 0
		
		raycast_right.force_raycast_update()
		raycast_left.force_raycast_update()
		raycast_up.force_raycast_update()
		raycast_down.force_raycast_update()
		
		if next_direction >= 0 and next_direction < 25:
			if not raycast_right.is_colliding():
				transform.origin.x += grid_size
				length_moved += 1
			else:
				choose_direction()
		if next_direction >= 25 and next_direction < 50:
			if not raycast_left.is_colliding():
				transform.origin.x -= grid_size
				length_moved += 1
			else:
				choose_direction()
		if next_direction >= 50 and next_direction < 75:
			if not raycast_up.is_colliding():
				transform.origin.y -= grid_size
				length_moved += 1
			else:
				choose_direction()
		if next_direction >= 75 and next_direction < 100:
			if not raycast_down.is_colliding():
				transform.origin.y += grid_size
				length_moved += 1
			else:
				choose_direction()
		
	if length_moved > next_length:
		choose_direction()
		choose_length()


func choose_direction():
	next_direction = randi() % 99


func choose_length():
	next_length = randi() % 40
