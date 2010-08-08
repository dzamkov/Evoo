using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Evoo
{
    /// <summary>
    /// Main program window.
    /// </summary>
    public class Window : GameWindow
    {
        public Window()
            : base(640, 480, OpenTK.Graphics.GraphicsMode.Default, "Evoo")
        {

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.2f, 0.8f, 0.2f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            this.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
        }
    }
}