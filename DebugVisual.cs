using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Evoo
{
    /// <summary>
    /// A happy little pyrimad.
    /// </summary>
    public class DebugVisual : VisualEntity
    {
        public override void Render()
        {
            Vector top = new Vector(0.0, 0.0, 4.0);
            Vector a = new Vector(1.0, 1.0, 0.0);
            Vector b = new Vector(1.0, -1.0, 0.0);
            Vector c = new Vector(-1.0, -1.0, 0.0);
            Vector d = new Vector(-1.0, 1.0, 0.0);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Begin(BeginMode.Triangles);
            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3((Vector3)top); GL.Vertex3((Vector3)a); GL.Vertex3((Vector3)b);
            GL.Vertex3((Vector3)top); GL.Vertex3((Vector3)b); GL.Vertex3((Vector3)c);
            GL.Vertex3((Vector3)top); GL.Vertex3((Vector3)c); GL.Vertex3((Vector3)d);
            GL.Vertex3((Vector3)top); GL.Vertex3((Vector3)d); GL.Vertex3((Vector3)a);
            GL.End();
            GL.Begin(BeginMode.Quads);
            GL.Vertex3((Vector3)a); GL.Vertex3((Vector3)b); GL.Vertex3((Vector3)c); GL.Vertex3((Vector3)d);
            GL.End();
        }
    }
}