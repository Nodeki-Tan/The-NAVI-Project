extends Node2D

onready var LoginWindow = get_node("LoginWindow")
onready var RegisterWindow = get_node("RegisterWindow")
onready var WelcomeWindow = get_node("WelcomeWindow")

# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _on_ToLogin_pressed():
	WelcomeWindow.visible = false
	
	LoginWindow.visible = true
	pass # Replace with function body.


func _on_ToRegister_pressed():
	WelcomeWindow.visible = false
	
	RegisterWindow.visible = true
	pass # Replace with function body.


func _on_CloseButton_pressed():
	WelcomeWindow.visible = true
	
	LoginWindow.visible = false
	RegisterWindow.visible = false
	pass # Replace with function body.
