extends Label


func _on_DuckManager_added_ducks(count):
	set_text(str(count, " Ducks"))


func _on_DuckManager_AddedDucks(count):
	set_text(str(count, " Ducks"))
