extends Node2D

onready var AudioPlayer = get_node("AudioPlayer")

# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	AudioPlayer.play(0)
	pass # Replace with function body.



func _on_AudioPlayer_finished():
	print("changed!")
	get_tree().change_scene("res://Scenes/Menus/LoginMenu.tscn") 
	pass
