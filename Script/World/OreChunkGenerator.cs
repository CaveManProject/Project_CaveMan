using Caveman.Enums;
using Godot;

namespace Caveman.World
{
    public partial class OreChunkGenerator : GodotObject
    {
        private Vector2I _position;
        private EntityRotation _rotation;
        public TileType _tileType;
        public int _chunkSize;

        public OreChunkGenerator(Vector2I position)
        {
            _position = position;
            _rotation = EntityRotationExtensions.GetRandomRotation();
            _tileType = TileTypeExtensions.GetRandomTileType(true);
            _chunkSize = 0;
        }

        public Vector2I RotatePosition()
        {
            this._rotation = EntityRotationExtensions.GetRandomRotation();
            return this._position + this._rotation.ToVector2I();
        }

        public void GenerateOre()
        {
            this._position += this._rotation.ToVector2I();
            this._chunkSize++;
        }
    }
}
