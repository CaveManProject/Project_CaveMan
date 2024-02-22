using Caveman.Player;
using Caveman.Resources;
using Godot;

namespace Caveman.Item
{
	public partial class ItemNode : Node2D
	{
		[Export]
		public InventoryItem _item;
		private AnimationPlayer _animationPlayer;
		private Sprite2D _sprite;
		private PlayerNode _player = null;
		private bool _collected = false;

		public override void _Ready()
		{
			this._animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
			this._sprite = this.GetNode<Sprite2D>("Sprite2D");
			this._sprite.Texture = this._item.GetTexture();
		}

		private void _on_interactable_body_entered(CharacterBody2D body)
		{
			if (body is PlayerNode)
			{
				this._player = body as PlayerNode;
			}
		}

		private void _on_interactable_body_exited(CharacterBody2D body)
		{
			if (body is PlayerNode)
			{
				this._player = null;
			}
		}

		public override void _Process(double delta)
		{
			if (!this._collected && this._player != null && !this._animationPlayer.IsPlaying())
			{
				this._collected = true;
				this._player.Collect(this._item);
				this.QueueFree();
			}
		}

		public void Drop(Vector2 direction)
		{
			if (direction.X == 1)
			{
				this._animationPlayer.Play("drop_left");
			}
			if (direction.X == -1)
			{
				this._animationPlayer.Play("drop_right");
			}
			if (direction.Y == 1)
			{
				this._animationPlayer.Play("drop_up");
			}
			if (direction.Y == -1)
			{
				this._animationPlayer.Play("drop_down");
			}
		}
	}
}
