using SFML.Graphics;

namespace Paint.GLSL.Brushes
{

    abstract class BrushBase
    {
        public string Name;

        public RenderStates RenderStates;

        public abstract void Update();
    }
}
