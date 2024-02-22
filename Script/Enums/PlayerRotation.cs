using Godot;

namespace Caveman.Enums
{
    public enum EntityRotation
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    public static class EntityRotationExtensions
    {
        public static Vector2I ToVector2I(this EntityRotation rotation)
        {
            return rotation switch
            {
                EntityRotation.RIGHT => new Vector2I(1, 0),
                EntityRotation.LEFT => new Vector2I(-1, 0),
                EntityRotation.UP => new Vector2I(0, -1),
                EntityRotation.DOWN => new Vector2I(0, 1),
                _ => new Vector2I(0, 0),
            };
        }

        public static EntityRotation GetRandomRotation()
        {
            return (EntityRotation)GD.RandRange(0, 4);
        }
    }
}
