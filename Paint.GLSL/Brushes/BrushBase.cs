using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL.Brushes
{

    abstract class BrushBase
    {
        public string Name;

        public RenderStates RenderStates;

        public abstract void Update(float sizeOfBrush, Vector2f position, Color color);
    }
}
