extends "res://Scripts/GDScript/Menus/GenericWindow.gd"

var username = ""
var password = ""

onready var usernameField = get_node("UsernameField")
onready var passwordField = get_node("PasswordField")
onready var errorLabel = get_node("ErrorLabel")

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_LoginButton_pressed():
	# Check username for errors
	username = usernameField.text
	
	if username == "":
		errorLabel.text = "Username is empty"
		return
	
	# Clear error (if any)
	errorLabel.text = ""
	
	# Check password for errors
	password = passwordField.text
	
	if password == "":
		errorLabel.text = "Password is empty"
		return
	
	# Clear error (if any)
	errorLabel.text = ""
	
	# Until now there is no errors so we can continue and load
	# a file to check the data down here and try to login
	
	# Simple print debug test
	print(username + " tried to login with password " + password)
	
	var data = get_node("/root/AssetManager").call("loadDataFile")
	
	print(data[1])
	
	pass # Replace with function body.
