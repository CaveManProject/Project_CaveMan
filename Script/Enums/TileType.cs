using System.Collections.Generic;
using Godot;

namespace Caveman.Enums
{
    public enum TileType
    {
        AIR,
        STONE,
        COAL,
        IRON,
        COPPER,
        TIN,
        ALUMINUM,
        GOLD,
        DIAMOND,
        EMERALD,
        AMETHYST,
        BEDROCK
    }

    public static class TileTypeExtensions
    {
        private static readonly Dictionary<TileType, Texture2D> _itemTextures =
            new()
            {
                { TileType.STONE, GD.Load<Texture2D>("res://Assets/Items/Stone.png") },
                { TileType.COAL, GD.Load<Texture2D>("res://Assets/Items/Coal.png") },
                { TileType.IRON, GD.Load<Texture2D>("res://Assets/Items/Iron.png") },
                { TileType.COPPER, GD.Load<Texture2D>("res://Assets/Items/Copper.png") },
                { TileType.TIN, GD.Load<Texture2D>("res://Assets/Items/Tin.png") },
                {
                    TileType.ALUMINUM,
                    GD.Load<Texture2D>("res://Assets/Items/Aluminum.png")
                },
                { TileType.GOLD, GD.Load<Texture2D>("res://Assets/Items/Gold.png") },
                { TileType.DIAMOND, GD.Load<Texture2D>("res://Assets/Items/Diamond.png") },
                { TileType.EMERALD, GD.Load<Texture2D>("res://Assets/Items/Emerald.png") },
                {
                    TileType.AMETHYST,
                    GD.Load<Texture2D>("res://Assets/Items/Amethyst.png")
                }
            };

        public static Texture2D GetItemTexture(this TileType tileType)
        {
            return _itemTextures[tileType];
        }

        public static TileType GetRandomTileType(bool onlyOres = false)
        {
            if (onlyOres)
            {
                return (TileType)GD.RandRange(2, 11);
            }

            return (TileType)GD.RandRange(0, 12);
        }
    }
}
