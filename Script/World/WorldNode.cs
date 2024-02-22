using System;
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
        private int _mapWidth = 100;

        /// <summary>
        /// The height of the map.
        /// </summary>
        [Export]
        private int _mapHeight = 100;

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

        private TileMap _tileMap;
        private PlayerNode _player;
        private Array<Array<Tile>> _grid = new();
        private Array<OreChunkGenerator> _generators = new();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            this._tileMap = this.GetNode<TileMap>("StoneTileMap");
            this._player = this.GetNode<PlayerNode>("Player");
            this._player.GlobalPosition += new Vector2I(_mapWidth / 2, _mapHeight / 2) * _cellSize;
            this.InitGrid();
            this.CreateChunks();
            this.RenderTiles();
        }

        private bool IsInSafeZone(Vector2I position)
        {
            var distance = Math.Sqrt(
                Math.Pow(position.X - _mapWidth / 2, 2) + Math.Pow(position.Y - _mapHeight / 2, 2)
            );
            return distance < _safeZoneRadius;
        }

        private Vector2I GetRandomSafePosition()
        {
            Vector2I position;
            do
            {
                position = new Vector2I(GD.RandRange(0, _mapWidth), GD.RandRange(0, _mapHeight));
            } while (!IsInSafeZone(position));
            return position;
        }

        private void InitGrid()
        {
            for (var x = 0; x < _mapWidth; x++)
            {
                var row = new Array<Tile>();
                for (var y = 0; y < _mapHeight; y++)
                {
                    if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight - 1)
                    {
                        row.Add(new Tile(TileType.BEDROCK));
                    }
                    else if (IsInSafeZone(new Vector2I(x, y)))
                    {
                        row.Add(new Tile(TileType.AIR));
                    }
                    else
                    {
                        row.Add(new Tile(TileType.STONE));
                    }
                }
                this._grid.Add(row);
            }
        }

        private void CreateChunks()
        {
            var iteration = 0;
            while (iteration < _maxIterations)
            {
                // Random: Maybe destroy generator?
                for (var i = 0; i < _generators.Count; i++)
                {
                    if (GD.Randf() < _generatorDestroyChance)
                    {
                        _generators.RemoveAt(i);
                        break; // Destroy only one generator per iteration
                    }
                }

                // Spawn new generator, with chance
                if (GD.Randf() < _generatorSpawnChance && _generators.Count < _maxGenerators)
                {
                    var generator = new OreChunkGenerator(GetRandomSafePosition());
                    _generators.Add(generator);
                }

                // Move generators
                for (var i = 0; i < _generators.Count; i++)
                {
                    var generator = _generators[i];
                    var target = generator.RotatePosition();
                    if (
                        target.X >= 1
                        && target.X < _mapWidth - 1
                        && target.Y >= 1
                        && target.Y < _mapHeight - 1
                        && _grid[target.X][target.Y].IsBreakable()
                    )
                    {
                        generator.GenerateOre();
                        _grid[target.X][target.Y]._tileType = generator._tileType;
                        if (generator._chunkSize == 10)
                        {
                            _generators.RemoveAt(i);
                        }
                    }
                }
                iteration += 1;
            }
        }

        private void RenderTiles()
        {
            for (var x = 0; x < _mapWidth; x++)
            {
                for (var y = 0; y < _mapHeight; y++)
                {
                    var tile = _grid[x][y];
                    var observation = GetObservation(new Vector2I(x, y));
                    var (tileX, rotation) = tile.GetVariant(observation);
                    GD.Print($"Tile: {tile._tileType}, TileX: {tileX}, Rotation: {rotation}");
                    this._tileMap.SetCell(
                        0,
                        new Vector2I(x, y),
                        (int)tile._tileType,
                        new Vector2I(tileX, rotation != null ? (int)rotation : 0)
                    );
                }
            }
        }

        private byte GetObservation(Vector2I target)
        {
            byte observation = 0b0000;
            if (_mapWidth > target.X + 1)
            {
                observation |= _grid[target.X + 1][target.Y].IsAir()
                    ? (byte)ObservationMask.RIGHT
                    : (byte)0;
            }
            if (target.X - 1 > 0)
            {
                observation |= _grid[target.X - 1][target.Y].IsAir()
                    ? (byte)ObservationMask.LEFT
                    : (byte)0;
            }
            if (_mapHeight > target.Y + 1)
            {
                observation |= _grid[target.X][target.Y + 1].IsAir()
                    ? (byte)ObservationMask.DOWN
                    : (byte)0;
            }
            if (target.Y - 1 > 0)
            {
                observation |= _grid[target.X][target.Y - 1].IsAir()
                    ? (byte)ObservationMask.UP
                    : (byte)0;
            }
            return observation;
        }

        private void UpdateTile(Vector2I target)
        {
            if (target.X <= 0 || target.X > _grid.Count)
            {
                return;
            }
            if (target.Y <= 0 || target.Y > _grid[target.X].Count)
            {
                return;
            }
            var tile = _grid[target.X][target.Y];
            if (tile.IsAir())
            {
                return;
            }
            var observation = GetObservation(target);
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
            var tile = _grid[target.X][target.Y];
            var itemScene = ResourceLoader.Load<PackedScene>("res://Items/ItemNode.tscn");
            var itemNode = itemScene.Instantiate<ItemNode>();
            itemNode._item = new InventoryItem(tile._tileType);
            itemNode.GlobalPosition = this._player.GlobalPosition;
            this.GetParent().AddChild(itemNode);
            itemNode.Drop(direction);
            _grid[target.X][target.Y].ClearTile();
            UpdateSurroundingTiles(target);
        }

        public override void _Process(double delta)
        {
            var playerCoords = this._tileMap.LocalToMap(this._player.GlobalPosition);
            var target = playerCoords + this._player.GetDirection();
            if (Input.IsActionJustPressed("e") && _grid[target.X][target.Y].IsBreakable())
            {
                MineBlock(target, this._player.GetDirection());
            }

            if (Input.IsActionJustPressed("space"))
            {
                InitGrid();
                CreateChunks();
                RenderTiles();
            }
        }
    }
}
