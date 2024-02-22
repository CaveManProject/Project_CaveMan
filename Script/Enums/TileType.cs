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
                { TileType.AIR, GD.Load<Texture2D>("res://Assets/Textures/Items/air.png") },
                { TileType.STONE, GD.Load<Texture2D>("res://Assets/Textures/Items/stone.png") },
                { TileType.COAL, GD.Load<Texture2D>("res://Assets/Textures/Items/coal.png") },
                { TileType.IRON, GD.Load<Texture2D>("res://Assets/Textures/Items/iron.png") },
                { TileType.COPPER, GD.Load<Texture2D>("res://Assets/Textures/Items/copper.png") },
                { TileType.TIN, GD.Load<Texture2D>("res://Assets/Textures/Items/tin.png") },
                {
                    TileType.ALUMINUM,
                    GD.Load<Texture2D>("res://Assets/Textures/Items/aluminum.png")
                },
                { TileType.GOLD, GD.Load<Texture2D>("res://Assets/Textures/Items/gold.png") },
                { TileType.DIAMOND, GD.Load<Texture2D>("res://Assets/Textures/Items/diamond.png") },
                { TileType.EMERALD, GD.Load<Texture2D>("res://Assets/Textures/Items/emerald.png") },
                {
                    TileType.AMETHYST,
                    GD.Load<Texture2D>("res://Assets/Textures/Items/amethyst.png")
                },
                { TileType.BEDROCK, GD.Load<Texture2D>("res://Assets/Textures/Items/bedrock.png") }
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
