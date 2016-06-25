using SFML.System;

namespace Paint.GLSL
{
    public static class Helper
    {
        public static Vector2f ConvertToVector2f(this Vector2i input)
        {
            return new Vector2f(input.X, input.Y);
        }
    }
}
