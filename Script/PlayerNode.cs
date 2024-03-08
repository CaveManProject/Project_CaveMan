using System.Diagnostics;
using Caveman.Enums;
using Caveman.Resources;
using Godot;

namespace Caveman.Player
{
	public partial class PlayerNode : CharacterBody2D
	{

		[Export]
		private int _speed = 100;

		[Export]
		private int _sprintSpeed = 200;

		[Export]
		private float _animationSpeedScale = 2.5f;

		[Export]
		private PlayerState _playerState = PlayerState.IDLE;

		[Export]
		private const double MAX_HEALTH = 100;
		[Export]
		private const double HEALTH_REGEN = 1;
		private double _health = 100;

		[Export]
		private const double MAX_STAMINA = 100;
		[Export]
		private const double STAMINA_REGEN = 1;
		[Export]
		private const double STAMINA_SPRINT_DRAIN = 0.5;
		private double _stamina = 100;

		[Signal]
		public delegate void OnStaminaUpdateEventHandler(double amount);
		[Signal]
		public delegate void OnHealthUpdateEventHandler(double amount);

		private AnimatedSprite2D _animation;

		private EntityRotation? _rotation = null;

		private InventoryResource _inventory = GD.Load<InventoryResource>("res://Data/inventory.tres");

		public override void _Ready()
		{
			this._animation = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}

		public override void _PhysicsProcess(double delta)
		{
			var direction = Input.GetVector("left", "right", "up", "down");
			this._rotation = EntityRotationExtensions.GetRotation(direction);
			this._playerState = this.GetPlayerState();

			if (this._playerState == PlayerState.SPRINTING)
			{
				this._stamina -= STAMINA_SPRINT_DRAIN;
				if (this._stamina < 0)
				{
					this._stamina = 0;
				}
				this.EmitSignal(SignalName.OnStaminaUpdate, this._stamina);
			}
			else
			{
				this._stamina += STAMINA_REGEN;
				if (this._stamina > MAX_STAMINA)
				{
					this._stamina = MAX_STAMINA;
				}
				this.EmitSignal(SignalName.OnStaminaUpdate, this._stamina);
			}

			this._animation.SpeedScale = this._playerState == PlayerState.SPRINTING && this._stamina > 0 ? this._animationSpeedScale : 1.0f;
			this.Velocity = (this._playerState == PlayerState.SPRINTING && this._stamina > 0 ? _sprintSpeed : _speed) * direction;
			this.MoveAndSlide();
			this.AnimateMovement();
		}

		public PlayerState GetPlayerState()
		{
			if (this._rotation == null)
			{
				return PlayerState.IDLE;
			}
			return Input.IsActionPressed("sprint") && this._rotation != null ? PlayerState.SPRINTING : PlayerState.WALKING;

		}

		public void Collect(InventoryItem item)
		{
			this._inventory.Insert(item);
		}

		private void AnimateMovement()
		{
			if (
				this._animation.IsPlaying()
				&& this._animation.Animation.ToString().StartsWith("Att_")
			)
			{
				return;
			}
			switch (this._rotation)
			{
				case EntityRotation.RIGHT:
					this._animation.Play("Mv_right");
					break;
				case EntityRotation.LEFT:
					this._animation.Play("Mv_left");
					break;
				case EntityRotation.UP:
					this._animation.Play("Mv_up");
					break;
				case EntityRotation.DOWN:
					this._animation.Play("Mv_down");
					break;
				case EntityRotation.UP_RIGHT:
					this._animation.Play("Mv_upR");
					break;
				case EntityRotation.UP_LEFT:
					this._animation.Play("Mv_upL");
					break;
				case EntityRotation.DOWN_RIGHT:
					this._animation.Play("Mv_downR");
					break;
				case EntityRotation.DOWN_LEFT:
					this._animation.Play("Mv_downL");
					break;
				default:
					this._animation.Play("Idle");
					break;
			}
		}

		public void AnimateBreaking()
		{
			switch (this._rotation)
			{
				case EntityRotation.RIGHT:
					this._animation.Play("Att_right");
					break;
				case EntityRotation.LEFT:
					this._animation.Play("Att_left");
					break;
				case EntityRotation.UP:
					this._animation.Play("Att_up");
					break;
				case EntityRotation.DOWN:
					this._animation.Play("Att_down");
					break;
			}
		}

		public Vector2I GetDirection()
		{
			return this._rotation?.ToVector2I() ?? new Vector2I(0, 0);
		}
	}
}
