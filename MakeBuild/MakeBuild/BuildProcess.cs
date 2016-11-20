using System;
using System.IO;
using BuildLibrary;

namespace MakeBuild
{
    public class BuildProcess
    {
        private CWLibrary cwUtil;
        private IOLibrary ioUtil;
        private string buildFolder;
        private string buildVersion;
        private string[] Version;
        private string buildDoc;

        public BuildProcess (string buildDocPath, string[] version)
        {
            cwUtil = new CWLibrary();
            ioUtil = new IOLibrary();
            this.Version = version;
            this.buildDoc = buildDocPath;
            this.buildVersion = version[0] + "." + version[1] + "." + version[2] + " Rev " + version[3];
            this.buildFolder = Path.Combine(ioUtil.GetParentFolderFromFilePath(buildDocPath), "BUILDS", this.buildVersion);
            cwUtil.SetCellBuildVersion(buildDocPath, version); 
        }

        public string BuildPCUTIL ()
        {
            string cwpFilePath = "";            
            // Now we start the build process
            Console.WriteLine("Build PCUTIL");
            cwUtil.RunScriptOnDocument(this.buildDoc, "On Opening", "PCUTIL", "init", this.Version);
            while (ioUtil.IsCaseWareProcessRunning())
            {
                // Wait for build to finish
            }
            // Need to find the cwp file after build
            cwpFilePath = ioUtil.FindTheFileByExtension(Path.Combine(buildFolder, "PCUTIL"), ".cwp");
            Console.WriteLine();
            return cwpFilePath;
        }

        public string BuildAuditCWI()
        {
            string cwpFilePath = "";
            // Now we start the build process
            Console.WriteLine("Build Audit CWI");
            cwUtil.RunScriptOnDocument(this.buildDoc, "On Opening", "AUDITCWI", "init", this.Version);
            while (ioUtil.IsCaseWareProcessRunning())
            {
                // Wait for build to finish
            }
            cwpFilePath = ioUtil.FindTheFileByExtension(Path.Combine(buildFolder, "AUDITCWI"), ".cwp");
            Console.WriteLine();
            return cwpFilePath;
        }

        public string BuildAuditINT()
        {
            string cwpFilePath = "";
            // Now we start the build process
            Console.WriteLine("Build Audit INT");
            cwUtil.RunScriptOnDocument(this.buildDoc, "On Opening", "AUDITINT", "init", this.Version);
            while (ioUtil.IsCaseWareProcessRunning())
            {
                // Wait for build to finish
            }
            cwpFilePath = ioUtil.FindTheFileByExtension(Path.Combine(buildFolder, "AUDITINT"), ".cwp");
            Console.WriteLine();
            return cwpFilePath;
        }

        public string BuildAuditUS()
        {
            string cwpFilePath = "";
            // Now we start the build process
            Console.WriteLine("Build Audit US");
            cwUtil.RunScriptOnDocument(this.buildDoc, "On Opening", "AUDITUS", "init", this.Version);
            while (ioUtil.IsCaseWareProcessRunning())
            {
                // Wait for build to finish
            }
            cwpFilePath = ioUtil.FindTheFileByExtension(Path.Combine(buildFolder, "AUDITUS"), ".cwp");
            Console.WriteLine();
            return cwpFilePath;
        }

        public void CleanUpAfterBuild (string buildFolder)
        {
            if (Directory.Exists(buildFolder))
            {
                DirectoryInfo dirs = new DirectoryInfo(buildFolder);                
                // Set the Attributes for it
                dirs.Attributes = FileAttributes.Normal;
                FileInfo[] files = dirs.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Attributes = FileAttributes.Normal;
                    if (file.Extension != ".cwp" && file.Extension != ".CWP" && file.Extension != ".ini")
                    {
                        file.Delete();
                    }
                }
                // Looping through sub-folder
                DirectoryInfo[] dir = dirs.GetDirectories();
                foreach (DirectoryInfo subfolder in dir)
                {
                    if ((subfolder.Name.Contains("AUDIT") == false && subfolder.Name.Contains("PCUTIL") == false) || subfolder.Name.Contains("CWUPDATE") == true)
                    {
                        Directory.Delete(subfolder.FullName, true);
                        continue;
                    }
                    CleanUpAfterBuild(subfolder.FullName);
                }
            }
        }

        public void CopyIniFile (string buildFolder, string iniFilePath)
        {
            // Copy ini file
            File.Copy(iniFilePath, Path.Combine(buildFolder, "silentcwp.ini"), true);
        }
    }
}
