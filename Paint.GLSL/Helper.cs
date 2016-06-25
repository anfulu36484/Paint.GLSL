using SFML.System;

namespace Paint.GLSL
{
    public static class Helper
    {
        public static Vector2f ConvertToVector2f(this Vector2i input)
        {
            return new Vector2f(input.X, input.Y);
        }

        public static Vector2f ConvertToVector2f(this Vector2u input)
        {
            return new Vector2f(input.X, input.Y);
        }

        public static Vector2f Multiplication(Vector2f first,Vector2f second)
        {
            return new Vector2f(first.X*second.X, first.Y * second.Y);
        }
    }
}
