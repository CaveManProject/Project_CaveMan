using Caveman.Player;
using Godot;

public partial class World : Node
{

	[Export]
	private int MAP_WIDTH = 100;
	[Export]
	private int MAP_HEIGHT = 100;
	[Export]
	private int CellSize = 16;

	private TileMap _tileMap;
	private PlayerNode _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._tileMap = this.GetNode<TileMap>("TileMap");
		this._player = this.GetNode<PlayerNode>("Player");
		this._player.GlobalPosition += new Vector2I(MAP_WIDTH / 2, MAP_HEIGHT / 2) * CellSize;

	}


}
