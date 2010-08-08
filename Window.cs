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
            this._Zone = new Zone();
            this._Camera = new CameraEntity();
            this._Zone.AddEntity(this._Camera);
            this._Zone.QueueMessage(this._Camera.Reorient(new CameraInfo
            {
                Pos = new Vector(10.0, 10.0, 10.0),
                Dir = new Vector(-1.0, -1.0, -1.0),
                Up = new Vector(0.0, 0.0, 1.0)
            }));
            this._Zone.FlushMessages();

            this._Zone.AddEntity(new DebugVisual());
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CameraInfo ci = this._Camera.Info;

            Matrix4d proj = Matrix4d.Perspective(0.9, (double)this.Width / (double)this.Height, 0.01, 100.0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4d view = Matrix4d.LookAt(ci.Pos, ci.Pos + ci.Dir, ci.Up);
            GL.LoadMatrix(ref view);

            this._Zone.Render();

            this.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
        }

        public Zone _Zone;
        public CameraEntity _Camera;
    }
}