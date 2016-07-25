using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class DrawingData
    {
        public string NameOfBrush;
        public float SizeOfBrush;
        public Vector2f Position;
        public Color Color;

        public DrawingData()
        {
        }

        public DrawingData(string nameOfBrush, float sizeOfBrush, Vector2f position, Color color)
        {
            NameOfBrush = nameOfBrush;
            SizeOfBrush = sizeOfBrush;
            Position = position;
            Color = color;
        }
    }
}
