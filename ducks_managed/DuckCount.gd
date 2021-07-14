extends Label


func _on_DuckManager_added_ducks(count):
	set_text(str(count, " Ducks"))
