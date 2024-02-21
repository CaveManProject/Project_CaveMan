using Caveman.Enums;
using Godot;
using System;


namespace Caveman.Resources;

public partial class InventoryItem : Resource
{
    public int amount = 1;
    public TileType tileType;

    public InventoryItem(TileType tileType)
    {
        this.tileType = tileType;
    }

    public Texture2D GetTexture() {
        return this.tileType.GetItemTexture();
    }

    public static bool operator ==(InventoryItem a, InventoryItem b)
    {
        return a.tileType == b.tileType;
    }

    public static bool operator !=(InventoryItem a, InventoryItem b)
    {
        return a.tileType != b.tileType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        return this.tileType == ((InventoryItem)obj).tileType;
    }

    public override int GetHashCode()
    {
        return this.tileType.GetHashCode() ^ this.amount.GetHashCode();
    }
}
   

