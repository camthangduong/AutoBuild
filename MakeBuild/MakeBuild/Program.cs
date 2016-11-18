using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MakeBuild
{
    class Program
    {        
        static void Main(string[] args)
        {
            ///
            /// Get the current path where the application launched
            /// Our template re-defined path is C:\GIT
            /// In order to builds the audit patch, the folder named BUILDS is required where the source folder is            
            /// 
            string appPath = Application.ExecutablePath;
            string appFolder = Path.GetDirectoryName(appPath);

            // Create a BUILDS Folder if it does not exist
            string buildFolder = Path.Combine(appPath, "BUILDS");
            if (Directory.Exists(buildFolder) == false)
            {
                // Folder does not exist, create a folder
                Directory.CreateDirectory(buildFolder);
            }

        }
    }
}
