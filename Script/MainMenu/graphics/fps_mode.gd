extends Control

@onready var option_button = $HBoxContainer/OptionButton as OptionButton

const FPS_MODE_ARRAY: Array[String] = [
	"30 FPS",
	"60 FPS",
	"120 FPS",
	"144 FPS",
	"240 FPS",
	"unlimited"
]

func _ready():
	add_fps_modes_items()
	option_button.item_selected.connect(on_fps_mode_selected)

func add_fps_modes_items() -> void:
	for fps_mode in FPS_MODE_ARRAY:
		option_button.add_item(fps_mode)

func on_fps_mode_selected(index : int) -> void:
	match index:
		0: #30 FPS
			Engine.max_fps = 30
		1: #60 FPS
			Engine.max_fps = 60
		2: #120 FPS
			Engine.max_fps = 120
		3: #144 FPS
			Engine.max_fps = 144
		4: #240 FPS
			Engine.max_fps = 240
		5: #unlimited
			Engine.max_fps = 0
