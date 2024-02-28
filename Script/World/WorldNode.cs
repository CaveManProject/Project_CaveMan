using Caveman.Enums;
using Caveman.Item;
using Caveman.Player;
using Caveman.Resources;
using Godot;
using Godot.Collections;

namespace Caveman.World
{
	public partial class WorldNode : Node
	{
		/// <summary>
		/// The width of the map.
		/// </summary>
		[Export]
		private int _mapSize;

		/// <summary>
		/// The size of the chunk
		/// 
		[Export]
		private int _chunkSize;

		/// <summary>
		/// The radius of the safe zone around spawn without ores.
		/// </summary>
		[Export]
		private int _safeZoneRadius = 4;

		/// <summary>
		/// The chance that a generator will be destroyed.
		/// </summary>
		[Export]
		private double _generatorDestroyChance = 0.1;

		/// <summary>
		///  The chance that a generator will spawn.
		/// </summary>
		[Export]
		private double _generatorSpawnChance = 0.1;

		/// <summary>
		///  The maximum number of iterations that can be performed.
		/// </summary>
		[Export]
		private int _maxIterations = 10;

		/// <summary>
		///   The maximum number of generators that can be spawned at once.
		/// </summary>
		[Export]
		private int _maxGenerators = 10;

		/// <summary>
		/// The size of the cell.
		/// </summary>
		[Export]
		private int _cellSize = 16;

		/// <summary>
		/// Render radius.
		/// </summary>
		[Export]
		private int _renderRadius = 2;

		private TileMap _tileMap;
		private PlayerNode _player;
		private Array<OreChunkGenerator> _generators = new();
		private MapGenerator _map;

		public override void _Ready()
		{
			this._tileMap = this.GetNode<TileMap>("StoneTileMap");
			this._map = new MapGenerator(_mapSize, _safeZoneRadius, _chunkSize, _maxIterations, _generatorSpawnChance, _generatorDestroyChance, _maxGenerators, _renderRadius);
			this._player = this.GetNode<PlayerNode>("Player");
			this._player.GlobalPosition += this._tileMap.MapToLocal(this._map.CENTER + new Vector2I(_chunkSize / 2, _chunkSize / 2));
			InitialRender();
		}

		private void InitialRender()
		{
			for (var x = -_renderRadius; x <= _renderRadius; x++)
			{
				for (var y = -_renderRadius; y <= _renderRadius; y++)
				{
					this.RenderTiles(this._map.CHUNK_CENTER + new Vector2I(x, y));
				}
			}
		}


		/// <summary>
		/// Render the tiles in the map.
		/// </summary>
		/// <param name="target">Chunk position</param>
		private void RenderTiles(Vector2I target)
		{
			for (var x = 0; x < _chunkSize; x++)
			{
				for (var y = 0; y < _chunkSize; y++)
				{
					var position = target * _chunkSize + new Vector2I(x, y);
					var tile = _map.GetTile(position);
					if (tile is null || tile.IsAir())
					{
						this._tileMap.SetCell(
							0,
							position,
							(int)TileType.AIR,
							new Vector2I(0, 0));
						continue;
					}
					var observation = _map.GetObservation(position);
					var (tileX, rotation) = tile.GetVariant(observation);
					this._tileMap.SetCell(
						0,
						position,
						(int)tile._tileType,
						new Vector2I(tileX, rotation != null ? (int)rotation : 0)
					);
				}
			}
		}


		private void UpdateTile(Vector2I target)
		{
			if (target.X <= 0 || target.X > _mapSize)
			{
				return;
			}
			if (target.Y <= 0 || target.Y > _mapSize)
			{
				return;
			}
			var tile = _map.GetTile(target);
			if (tile is null || tile.IsAir())
			{
				return;
			}
			var observation = _map.GetObservation(target);
			var (tileX, rotation) = tile.GetVariant(observation);
			this._tileMap.SetCell(
				0,
				target,
				(int)tile._tileType,
				new Vector2I(tileX, rotation != null ? (int)rotation : 0)
			);
		}

		private void UpdateSurroundingTiles(Vector2I target)
		{
			UpdateTile(target + new Vector2I(1, 0));
			UpdateTile(target + new Vector2I(-1, 0));
			UpdateTile(target + new Vector2I(0, 1));
			UpdateTile(target + new Vector2I(0, -1));
		}

		private void MineBlock(Vector2I target, Vector2I direction)
		{
			this._player.AnimateBreaking();
			this._tileMap.SetCell(0, target, 0, new Vector2I(0, 0));
			var tile = _map.GetTile(target);
			if (tile is null)
			{
				GD.PrintErr("Tile is null?!!!");
				return;
			}
			var itemScene = ResourceLoader.Load<PackedScene>("res://Scenes/Items/item.tscn");
			var itemNode = itemScene.Instantiate<ItemNode>();
			itemNode.item = new InventoryItem(tile._tileType);
			itemNode.GlobalPosition = this._player.GlobalPosition;
			this.GetParent().AddChild(itemNode);
			itemNode.Drop(direction);
			tile.ClearTile();
			UpdateSurroundingTiles(target);
		}


		public override void _Process(double delta)
		{
			var playerCoords = this._tileMap.LocalToMap(this._player.GlobalPosition);
			var target = playerCoords + this._player.GetDirection();
			if (Input.IsActionJustPressed("e") && this._map.GetTile(target).IsBreakable())
			{
				this.MineBlock(target, this._player.GetDirection());
			}
			var newChunks = this._map.UpdateMap(playerCoords);
			if (newChunks.Count > 0)
			{
				foreach (var newChunk in newChunks)
				{
					this.RenderTiles(newChunk);
				}
			}

		}
	}
}
