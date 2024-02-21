using Caveman.Enums;
using Caveman.Resources;
using Godot;
using System;


namespace Caveman.Player;
public partial class PlayerNode : CharacterBody2D
{

    private AnimatedSprite2D _animation;

    private PlayerRotation _rotation = PlayerRotation.IDLE;

    [Export]
    private int _speed = 100;

    [Export]
    private PlayerState _playerState = PlayerState.IDLE;
    [Export]
    private InventoryResource _inventory;

    public override void _Ready()
    {
        this._animation = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Input.GetVector("left", "right", "up", "down");
        if(direction.X == 0 && direction.Y == 0){
            this._playerState = PlayerState.IDLE;
        }
        else{
            this._playerState = PlayerState.WALKING;
        }
        this.Velocity = direction * this._speed;
        this.MoveAndSlide();
        this.AnimateMovement(direction);
    }

    public void Collect(InventoryItem item)
    {
        this._inventory.Insert(item);
    }

    private void AnimateMovement(Vector2 direction){
        if(this._animation.IsPlaying() && this._animation.Animation.ToString().StartsWith("Att_")){
            return;
        }
        this._rotation = PlayerRotation.IDLE;
        if(this._playerState == PlayerState.IDLE){
            this._animation.Play("Idle");
        }
        if(this._playerState == PlayerState.WALKING){
            if(direction.X > 0.5 && direction.Y < -0.5){
                this._animation.Play("Mv_upR");
            }
            else if(direction.X > 0.5 && direction.Y > 0.5){
                this._animation.Play("Mv_downR");
            }
            else if(direction.X < -.05 && direction.Y > 0.5){
                this._animation.Play("Mv_downL");
            }
            else if(direction.X < -.05 && direction.Y < -.05){
                this._animation.Play("Mv_upL");
            }
            else if(direction.Y < -.05){
                this._rotation = PlayerRotation.UP;
                this._animation.Play("Mv_up");
            }
            else if(direction.X == 1){
                this._rotation = PlayerRotation.RIGHT;
                this._animation.Play("Mv_right");
            }
            else if(direction.Y == 1){
                this._rotation = PlayerRotation.DOWN;
                this._animation.Play("Mv_down");
            }
            else if(direction.X == -1){
                this._rotation = PlayerRotation.LEFT;
                this._animation.Play("Mv_left");
            }
        }
    }

    public void AnimateBreaking(){
        switch(this._rotation){
            case PlayerRotation.RIGHT:
                this._animation.Play("Att_right");
                break;
            case PlayerRotation.LEFT:
                this._animation.Play("Att_left");
                break;
            case PlayerRotation.UP:
                this._animation.Play("Att_up");
                break;
            case PlayerRotation.DOWN:
                this._animation.Play("Att_down");
                break;
        }
    }

    public Vector2I GetDirection(){
        return this._rotation switch
        {
            PlayerRotation.RIGHT => new Vector2I(1, 0),
            PlayerRotation.LEFT => new Vector2I(-1, 0),
            PlayerRotation.UP => new Vector2I(0, -1),
            PlayerRotation.DOWN => new Vector2I(0, 1),
            _ => new Vector2I(0, 0),
        };
    }

}
   

