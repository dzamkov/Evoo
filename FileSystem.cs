using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Evoo
{
    class FileSystem
    {
        public static string GetModelPath(string name)
        {
            string file = Application.StartupPath + "\\models\\" + name;
            file = file.Replace("\\bin\\Debug\\", "\\");
            file = file.Replace("\\bin\\Release\\", "\\");
            if (!File.Exists(file))
                throw new Exception("File \"" + file + "\" Not Found");
            return file;
        }
    }
}
