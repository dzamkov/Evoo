using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Collections;
namespace Evoo
{
    class Model
    {
        public unsafe Model(Vector3[] VertArray)
        {
            this.VertexArray = VertArray;
            //this.NormalArray = ;

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
        
        public unsafe static LinkedList<Model> FromFile(string model)
        {
            Console.WriteLine("Loading Model " + model);
            StreamReader obj = new StreamReader(FileSystem.GetModelPath(model));
           
            LinkedList<Vector3> vertList = new LinkedList<Vector3>();
            LinkedList<Vector3> normList = new LinkedList<Vector3>();

            LinkedList<Model> ret = new LinkedList<Model>();

            // Declerations
            Vector3[] verts;
            Vector3[] norms;
            long count = 0;

            string[] obj_contents = obj.ReadToEnd().Split('\n');
            obj.Close(); obj.Dispose();

            foreach (string line in obj_contents)
            {
                string[] args = line.Split(' '); 
                if(line.StartsWith("v "))
                {
                    float x = float.Parse(args[1]);
                    float y = float.Parse(args[2]);
                    float z = float.Parse(args[3]);
                    vertList.AddLast(new Vector3(x, y, z));
                }
                else if (line.StartsWith("vn "))
                {
                    float x = float.Parse(args[1]);
                    float y = float.Parse(args[2]);
                    float z = float.Parse(args[3]);
                    normList.AddLast(new Vector3(x, y, z));
                }
                else if (line.StartsWith("g "))
                {
                    if (vertList.Count > 0) // Only add ones with verts
                    {
                        Console.WriteLine("Adding Group with " + vertList.Count.ToString() + " verts");
                        verts = new Vector3[vertList.Count];
                        norms = new Vector3[normList.Count];

                        count = 0;
                        foreach (Vector3 vec in vertList)
                        {
                            verts[count] = vec;
                            count++;
                        }
                        count = 0;
                        foreach (Vector3 vec in normList)
                        {
                            norms[count] = vec;
                            count++;
                        }
                        ret.AddLast(new Model(verts));
                        vertList.Clear();
                        normList.Clear();
                    }
                }
            }
            Console.WriteLine("Adding Group with " + vertList.Count.ToString() + " verts");
            verts = new Vector3[vertList.Count];
            norms = new Vector3[normList.Count];

            count = 0;
            foreach(Vector3 vec in vertList)
            {
                verts[count] = vec;
                count++;
            }
            count = 0;
            foreach (Vector3 vec in normList)
            {
                norms[count] = vec;
                count++;
            }
            ret.AddLast(new Model(verts));
            Console.WriteLine("Model Loaded");
            return ret;
        }
        ~Model()
        {
            //GL.DeleteBuffers(1, ref VBOid);
        }

        public void DrawModel()
        {
            GL.InterleavedArrays(InterleavedArrayFormat.V3f, sizeof(float) * 3, IntPtr.Zero);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.DrawArrays(BeginMode.Polygon, 0, VertexArray.Length);
            return;
        }

        public Vector3[] VertexArray = null;
        public Vector3[] NormalArray = null;
        uint VBOid;
        
    }
}
