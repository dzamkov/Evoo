using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
namespace Evoo
{
    class Model
    {
        public unsafe Model(string model)
        {
            StreamReader obj = new StreamReader(FileSystem.GetModelPath(model)); // messy and probbly slow, TODO: Improve & Move to seperate class
            string[] obj_contents = obj.ReadToEnd().Split('\n');
            obj.Close(); obj.Dispose();
            #region VERTS
            bool done = false;
            long cur_line = 0;
            long start = 0;
            long end = 0;

            // lets find where we start and end for our array size
            while (!done)
            {
                string line = obj_contents[cur_line];
                if (start == 0)
                {
                    if (line.StartsWith("v "))
                        start = cur_line;
                }
                else
                {
                    if (!line.StartsWith("v "))
                    {
                        end = cur_line;
                        done = true;
                    }
                }
                cur_line++;
            }
            // now lets parse them and add them to our array (we can create our array now we know our size)
            VertexArray = new Vector3[(end - start) + 1];
            for (long i = start; i < end; i++)
            {
                string[] args = obj_contents[i].Split(' '); // args[0] should be v, i geuss you could do some error checking
                if (args[0] != "v")
                    throw new Exception("Error parsing model");
                float x = float.Parse(args[1]);
                float y = float.Parse(args[2]);
                float z = float.Parse(args[3]);

                VertexArray[i - start] = new Vector3(x, y, z);
            }
            #endregion
            // TODO: Normals

            GL.GenBuffers(1, out VBOid);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexArray.Length * sizeof(float) * 3), IntPtr.Zero, BufferUsageHint.StaticDraw);
            float* buf = (float*)(GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly).ToPointer());
            foreach (Vector3 vec in VertexArray)
            {
                buf[0] = vec.X;
                buf[1] = vec.Y;
                buf[2] = vec.Z;
                buf += 3;
            }
            GL.UnmapBuffer(BufferTarget.ArrayBuffer);
        }
        ~Model()
        {
            //GL.DeleteBuffers(1, ref VBOid);
        }

        public void DrawModel()
        {
            GL.InterleavedArrays(InterleavedArrayFormat.V3f, sizeof(float) * 3, IntPtr.Zero);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.DrawArrays(BeginMode.Triangles, 0, VertexArray.Length);

            //GL.Begin(BeginMode.Triangles);
            //GL.Color3(1.0, 1.0, 1.0);

            //foreach (Vector3 vec in VertexArray)
            //    GL.Vertex3(vec + (Vector3)Pos);
            //GL.End();
            return;
        }
        
        public Vector3[] VertexArray = null;
        uint VBOid;
        
    }
}
