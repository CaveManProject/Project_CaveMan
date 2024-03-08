using Caveman.Enums;
using Godot;
using Godot.Collections;

namespace Caveman.World
{
	public partial class MapGenerator : GodotObject
	{
		private readonly int MAP_SIZE;
		private readonly int CHUNK_SIZE;
		private readonly int SAFE_RADIUS;
		private readonly int ITERATIONS;
		private readonly double GENERATOR_SPAWN_CHANCE;
		private readonly double GENERATOR_DESTROY_CHANCE;
		private readonly int MAX_GENERATORS;
		private readonly int RENDER_RADIUS;
		private Array<Array<MapChunk>> _chunks = new();

		public MapGenerator(int _mapSize, int _safeRadius, int _chunkSize, int _iterations, double _generatorSpawnChance, double _generatorDestroyChance, int _maxGenerators, int _renderRadius)
		{
			this.MAP_SIZE = _mapSize;
			this.SAFE_RADIUS = _safeRadius;
			this.CHUNK_SIZE = _chunkSize;
			this.ITERATIONS = _iterations;
			this.GENERATOR_SPAWN_CHANCE = _generatorSpawnChance;
			this.GENERATOR_DESTROY_CHANCE = _generatorDestroyChance;
			this.MAX_GENERATORS = _maxGenerators;
			this.RENDER_RADIUS = _renderRadius;
			InitMap();
			UpdateMap(this.CENTER, true);
		}

		public Vector2I CENTER => new(this.MAP_SIZE / 2, this.MAP_SIZE / 2);

		public Vector2I CHUNK_CENTER => this.CENTER / this.CHUNK_SIZE;

		private void InitMap()
		{
			var nChunks = MAP_SIZE / CHUNK_SIZE;
			for (var x = 0; x < nChunks; x++)
			{
				var row = new Array<MapChunk>();
				for (var y = 0; y < nChunks; y++)
				{
					row.Add(null);
				}
				this._chunks.Add(row);
			}
		}


		/// <summary>
		/// Update the map with the new target position.
		/// </summary>
		/// <param name="target">Player position</param>
		/// <returns>True if the chunk was created, false otherwise</returns>
		public Array<Vector2I> UpdateMap(Vector2I target, bool safeZone = false)
		{
			var newChunks = new Array<Vector2I>();
			for (int x = -RENDER_RADIUS; x <= RENDER_RADIUS; x++)
			{
				for (int y = -RENDER_RADIUS; y <= RENDER_RADIUS; y++)
				{
					var chunkTarget = target + new Vector2I(x * CHUNK_SIZE, y * CHUNK_SIZE);
					if (!IsInWorld(chunkTarget))
					{
						return null;
					}
					var chunkX = chunkTarget.X / CHUNK_SIZE;
					var chunkY = chunkTarget.Y / CHUNK_SIZE;
					var chunk = this._chunks[chunkX][chunkY];
					if (chunk == null)
					{
						chunk = new MapChunk(CHUNK_SIZE, SAFE_RADIUS, ITERATIONS, GENERATOR_SPAWN_CHANCE, GENERATOR_DESTROY_CHANCE, MAX_GENERATORS, safeZone && x == 0 && y == 0);
						this._chunks[chunkX][chunkY] = chunk;
						newChunks.Add(new Vector2I(chunkX, chunkY));
					}
				}
			}

			return newChunks;
		}

		/// <summary>
		/// Get the tile at the given position.
		/// </summary>
		/// <param name="position">Position</param>
		/// <returns>Tile at the position</returns>
		public Tile GetTile(Vector2I position)
		{
			if (!IsInWorld(position))
			{
				return null;
			}
			var chunkX = position.X / CHUNK_SIZE;
			var chunkY = position.Y / CHUNK_SIZE;
			var chunk = this._chunks[chunkX][chunkY];
			if (chunk == null)
			{
				return null;
			}
			var tPos = new Vector2I(position.X % CHUNK_SIZE, position.Y % CHUNK_SIZE);
			return chunk.GetTile(tPos);
		}

		private int ChunkDim => MAP_SIZE / CHUNK_SIZE;

		private bool IsInWorld(Vector2I position)
		{
			return 0 <= position.X && position.X / CHUNK_SIZE < ChunkDim && 0 <= position.Y && position.Y / CHUNK_SIZE < ChunkDim;
		}


		public byte GetObservation(Vector2I target)
		{
			byte observation = 0b0000;
			if (IsInWorld(target + new Vector2I(1, 0)))
			{
				var tile = this.GetTile(target + new Vector2I(1, 0));
				if (tile != null)
				{
					observation |= tile.IsAir()
						? (byte)ObservationMask.RIGHT
						: (byte)0;
				}
			}
			if (IsInWorld(target + new Vector2I(-1, 0)))
			{
				var tile = this.GetTile(target + new Vector2I(-1, 0));
				if (tile != null)
				{
					observation |= tile.IsAir()
						? (byte)ObservationMask.LEFT
						: (byte)0;
				}
			}
			if (IsInWorld(target + new Vector2I(0, 1)))
			{
				var tile = this.GetTile(target + new Vector2I(0, 1));
				if (tile != null)
				{
					observation |= tile.IsAir()
						? (byte)ObservationMask.DOWN
						: (byte)0;
				}
			}
			if (IsInWorld(target + new Vector2I(0, -1)))
			{
				var tile = this.GetTile(target + new Vector2I(0, -1));
				if (tile != null)
				{
					observation |= tile.IsAir()
						? (byte)ObservationMask.UP
						: (byte)0;
				}
			}
			return observation;
		}
	}
}
