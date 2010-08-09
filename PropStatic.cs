using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
namespace Evoo
{
    /// <summary>
    /// A happy little pyrimad. // with arms
    /// </summary>
    public class PropStatic : VisualEntity // TODO: Add angel offset and a real place to put the loaded models along with places to staore a precache. EG: ModelLoader.PreCache("ak47.obj") and then you can go PropStaticVarName.SetModel("ak47.obj") without the game halting while it parses the file
    {
        public PropStatic(String filename)
        {
            StreamReader obj = new StreamReader(FileSystem.GetModelPath(filename)); // messy and probbly slow, TODO: Improve & Move to seperate class
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
            _VertexArray = new Vector3[(end - start) + 1];
            for (long i = start; i < end; i++)
            {
                string[] args = obj_contents[i].Split(' '); // args[0] should be v, i geuss you could do some error checking
                if (args[0] != "v")
                    throw new Exception("Error parsing model");
                float x = float.Parse(args[1]);
                float y = float.Parse(args[2]);
                float z = float.Parse(args[3]);

                _VertexArray[i - start] = new Vector3(x, y, z);
            }
            #endregion
            // TODO: Normals
        }
        public override void Render()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Begin(BeginMode.Triangles);
            
            GL.Color3(1.0, 1.0, 1.0);

            foreach (Vector3 vec in this._VertexArray)
                GL.Vertex3(vec);

            GL.End();
        }
        Vector3[] _VertexArray = null;
    }
}

