extends Sprite

var next_direction = 0
var next_length = 0
var length_moved = 0

onready var raycast_right = $RayCastRight
onready var raycast_left = $RayCastLeft
onready var raycast_up = $RayCastUp
onready var raycast_down = $RayCastDown