using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Paint.GLSL
{
    interface IDrawing
    {

        void Initialize();

        void Update();

        void Render();

        Texture GetBackTexture();

        RenderTexture GetBackRenderTexture();

        void SetBackRenderTexture(RenderTexture renderTexture);
    }
}
