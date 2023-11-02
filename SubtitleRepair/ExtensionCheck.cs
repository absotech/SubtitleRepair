using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SubtitleRepair
{
    class ExtensionCheck
    {
        public static bool CheckExtension (string filename)
        {
            string ext = Path.GetExtension(filename);
            foreach (string extension in Vars.AllowedExtensions)
            {
                if (ext == extension)
                {
                    return true;
                }
            }
            return false;
        }
           

    }
}
