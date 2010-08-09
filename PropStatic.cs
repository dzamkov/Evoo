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
            Mdl = new Model(filename);
        }
        public override void Render()
        {
            Mdl.DrawModel();
        }
        private Model Mdl;
    }
}

