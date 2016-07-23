using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class DrawingData
    {
        public readonly string NameOfBrush;
        public readonly float SizeOfBrush;
        public readonly Vector2f Position;
        public readonly Color Color;

        public DrawingData(string nameOfBrush, float sizeOfBrush, Vector2f position, Color color)
        {
            NameOfBrush = nameOfBrush;
            SizeOfBrush = sizeOfBrush;
            Position = position;
            Color = color;
        }
    }
}
