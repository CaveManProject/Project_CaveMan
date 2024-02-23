
using System;
using Caveman.Enums;
using Godot;
using Godot.Collections;

namespace Caveman.World
{
    public partial class MapChunk : GodotObject
    {
        private readonly int CHUNK_SIZE;
        private readonly int SAFE_RADIUS;
        private readonly int ITERATIONS;
        private readonly double GENERATOR_SPAWN_CHANCE;
        private readonly double GENERATOR_DESTROY_CHANCE;
        private readonly int MAX_GENERATORS;
        private Array<OreChunkGenerator> _generators = new();
        private Array<Array<Tile>> _grid = new();

        public MapChunk(int _chunkSize, int _safeRadius, int _iterations, double _generatorSpawnChance, double _generatorDestroyChance, int _maxGenerators, bool safeZone = false)
        {
            this.CHUNK_SIZE = _chunkSize;
            this.SAFE_RADIUS = _safeRadius;
            this.ITERATIONS = _iterations;
            this.GENERATOR_SPAWN_CHANCE = _generatorSpawnChance;
            this.GENERATOR_DESTROY_CHANCE = _generatorDestroyChance;
            this.MAX_GENERATORS = _maxGenerators;
            GenerateChunk(safeZone);
            CreateOreChunks();
        }

        private Vector2I CENTER => new(CHUNK_SIZE / 2, CHUNK_SIZE / 2);


        private bool IsInSafeZone(Vector2I position)
        {
            var distance = Math.Sqrt(
                Math.Pow(position.X - CENTER.X, 2) + Math.Pow(position.Y - CENTER.Y, 2)
            );
            return distance < this.SAFE_RADIUS;
        }

        private Vector2I GetRandomSafePosition()
        {
            Vector2I position;
            do
            {
                position = new Vector2I(GD.RandRange(0, CHUNK_SIZE), GD.RandRange(0, CHUNK_SIZE));
            } while (IsInSafeZone(position));
            return position;
        }

        private void CreateOreChunks()
        {
            var iteration = 0;
            while (iteration < this.ITERATIONS)
            {
                // Random: Maybe destroy generator?
                for (var i = 0; i < _generators.Count; i++)
                {
                    if (GD.Randf() < this.GENERATOR_DESTROY_CHANCE)
                    {
                        _generators.RemoveAt(i);
                        break; // Destroy only one generator per iteration
                    }
                }

                // Spawn new generator, with chance
                if (GD.Randf() < this.GENERATOR_SPAWN_CHANCE && _generators.Count < this.MAX_GENERATORS)
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
                        && target.X < CHUNK_SIZE - 1
                        && target.Y >= 1
                        && target.Y < CHUNK_SIZE - 1
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

        private void GenerateChunk(bool safeZone = false)
        {
            for (var x = 0; x < CHUNK_SIZE; x++)
            {
                var row = new Array<Tile>();
                for (var y = 0; y < CHUNK_SIZE; y++)
                {
                    if (safeZone && IsInSafeZone(new Vector2I(x, y)))
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

        public Tile GetTile(Vector2I position)
        {
            return this._grid[position.X][position.Y];
        }
    }
}