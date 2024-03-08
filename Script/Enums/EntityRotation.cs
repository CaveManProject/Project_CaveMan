using Godot;

namespace Caveman.Enums
{
    public enum EntityRotation
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
        // Diagonals for the player
        UP_RIGHT,
        UP_LEFT,
        DOWN_RIGHT,
        DOWN_LEFT
    }

    public static class EntityRotationExtensions
    {
        public static Vector2I ToVector2I(this EntityRotation rotation)
        {
            return rotation switch
            {
                EntityRotation.UP => new Vector2I(0, -1),
                EntityRotation.RIGHT => new Vector2I(1, 0),
                EntityRotation.DOWN => new Vector2I(0, 1),
                EntityRotation.LEFT => new Vector2I(-1, 0),
                EntityRotation.UP_RIGHT => new Vector2I(1, -1),
                EntityRotation.UP_LEFT => new Vector2I(-1, -1),
                EntityRotation.DOWN_RIGHT => new Vector2I(1, 1),
                EntityRotation.DOWN_LEFT => new Vector2I(-1, 1),
                _ => new Vector2I(0, 0)

            };
        }

        public static EntityRotation GetRandomBasicRotation()
        {
            return (EntityRotation)GD.RandRange(0, 4);
        }

        public static EntityRotation? GetRotation(Vector2 direction)
        {
            if (direction.X > 0.5 && direction.Y < -0.5)
            {
                return EntityRotation.UP_RIGHT;
            }
            else if (direction.X > 0.5 && direction.Y > 0.5)
            {
                return EntityRotation.DOWN_RIGHT;
            }
            else if (direction.X < -.05 && direction.Y > 0.5)
            {
                return EntityRotation.DOWN_LEFT;
            }
            else if (direction.X < -.05 && direction.Y < -.05)
            {
                return EntityRotation.UP_LEFT;
            }
            else if (direction.Y < -.05)
            {
                return EntityRotation.UP;
            }
            else if (direction.X == 1)
            {
                return EntityRotation.RIGHT;
            }
            else if (direction.Y == 1)
            {
                return EntityRotation.DOWN;
            }
            else if (direction.X == -1)
            {
                return EntityRotation.LEFT;
            }
            return null;
        }
    }
}
