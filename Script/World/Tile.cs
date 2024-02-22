using Caveman.Enums;
using Godot;

namespace Caveman.World
{
    public partial class Tile : GodotObject
    {
        public TileType _tileType;
        public double _durability;

        public Tile(TileType tileType)
        {
            _tileType = tileType;
            _durability = 1;
        }

        public (int, EntityRotation?) GetVariant(byte observation)
        {
            var wall_count = 0;
            EntityRotation? rotation = null;
            var gap = false;
            for (var offset = 0; offset < 4; offset++)
            {
                if ((observation >> offset & 0b1) == 0b1)
                {
                    wall_count += 1;
                    if (rotation == null || gap)
                    {
                        rotation = (EntityRotation)offset;
                    }
                    gap = false;
                }
                else if (wall_count > 0)
                {
                    gap = true;
                }
            }

            var is_tunnel =
                (observation & (byte)ObservationMask.TUNNEL_H) == (byte)ObservationMask.TUNNEL_H
                || (observation & (byte)ObservationMask.TUNNEL_V) == (byte)ObservationMask.TUNNEL_V;

            var x = 0;
            switch (wall_count)
            {
                case 0:
                    x = 4;
                    break;
                case 1:
                    x = 0;
                    break;
                case 2:
                    x = is_tunnel ? 5 : 1;
                    if (x == 5)
                    {
                        rotation =
                            (observation & (byte)ObservationMask.TUNNEL_H)
                            == (byte)ObservationMask.TUNNEL_H
                                ? EntityRotation.RIGHT
                                : EntityRotation.UP;
                    }
                    break;
                case 3:
                    x = 2;
                    break;
                case 4:
                    x = 3;
                    break;
            }

            return (x, rotation);
        }

        public bool IsBreakable()
        {
            return _tileType != TileType.BEDROCK;
        }

        public void ClearTile()
        {
            _tileType = TileType.AIR;
            _durability = 0;
        }

        public bool IsAir()
        {
            return _tileType == TileType.AIR;
        }
    }
}
