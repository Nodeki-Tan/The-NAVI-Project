extends NinePatchRect

export var windowName = "name"
onready var windowNameText = get_node("WindowNameText")

# Called when the node enters the scene tree for the first time.
func _ready():
	windowNameText.text = windowName
	pass # Replace with function body.

func _on_Button_pressed():
	visible = false
	pass # Replace with function body.
