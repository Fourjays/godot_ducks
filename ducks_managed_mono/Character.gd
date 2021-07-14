extends Sprite

export var grid_size = 16

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	
	
	if Input.is_action_just_released("ui_right"):
		if not $RayCastRight.is_colliding():
			transform.origin.x += grid_size
	if Input.is_action_just_released("ui_left"):
		if not $RayCastLeft.is_colliding():
			transform.origin.x -= grid_size
	if Input.is_action_just_released("ui_up"):
		if not $RayCastUp.is_colliding():
			transform.origin.y -= grid_size
	if Input.is_action_just_released("ui_down"):
		if not $RayCastDown.is_colliding():
			transform.origin.y += grid_size

