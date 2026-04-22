extends CharacterBody2D


const SPEED = 125.0
const JUMP_VELOCITY = -400.0

@onready var sprite = $AnimatedSprite2D
func _physics_process(delta: float) -> void:
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	# Obtener la dirección en X y en Y
	var direction = Input.get_vector("Move_LEFT", "Move_RIGHT", "Move_UP", "Move_DOWN")

	velocity = direction * SPEED

	# Animaciones
	if direction != Vector2.ZERO:
		if direction.y < 0:
			sprite.play("idleUP")
		elif direction.y > 0:
			sprite.play("idleDOWN")
		elif direction.x > 0:
			sprite.play("vertical_DOWN")
		elif direction.x < 0:
			sprite.play("vertical_UP")
	else:
		sprite.stop()

	# Aplicar movimiento
	move_and_slide()
