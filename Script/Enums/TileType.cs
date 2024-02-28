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
                { TileType.STONE, GD.Load<Texture2D>("res://Assets/Sprites/Items/Stone.png") },
                { TileType.COAL, GD.Load<Texture2D>("res://Assets/Sprites/Items/Coal.png") },
                { TileType.IRON, GD.Load<Texture2D>("res://Assets/Sprites/Items/Iron.png") },
                { TileType.COPPER, GD.Load<Texture2D>("res://Assets/Sprites/Items/Copper.png") },
                { TileType.TIN, GD.Load<Texture2D>("res://Assets/Sprites/Items/Tin.png") },
                {
                    TileType.ALUMINUM,
                    GD.Load<Texture2D>("res://Assets/Sprites/Items/Aluminum.png")
                },
                { TileType.GOLD, GD.Load<Texture2D>("res://Assets/Sprites/Items/Gold.png") },
                { TileType.DIAMOND, GD.Load<Texture2D>("res://Assets/Sprites/Items/Diamond.png") },
                { TileType.EMERALD, GD.Load<Texture2D>("res://Assets/Sprites/Items/Emerald.png") },
                {
                    TileType.AMETHYST,
                    GD.Load<Texture2D>("res://Assets/Sprites/Items/Amethyst.png")
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
                return (TileType)GD.RandRange(2, 10);
            }

            return (TileType)GD.RandRange(0, 12);
        }
    }
}
