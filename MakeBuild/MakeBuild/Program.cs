using System;
using System.IO;
using System.Windows.Forms;
using BuildLibrary;

namespace MakeBuild
{
    class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("=============================================================");
            Console.WriteLine("============== AUTO BUILD UTILITY TOOL ======================");
            Console.WriteLine("============== @Author: Cam Thang Duong =====================");
            Console.WriteLine("============== @Date: November 18, 2016 =====================");
            Console.WriteLine("=============================================================");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Initalize environment");
            Console.WriteLine();
            ///
            /// Get the current path where the application launched
            /// Our template re-defined path is C:\GIT
            /// In order to builds the audit patch, the folder named BUILDS is required where the source folder is            
            /// 
            string appPath = Application.ExecutablePath;
            string appFolder = Path.GetDirectoryName(appPath);
            string makeBuildDocPath = Path.Combine(appFolder, "Make Build.cvw");
            string buildVersionXMLPath = Path.Combine(appFolder, "Version.xml");
            string iniFilePath = Path.Combine(appFolder, "Library", "silentcwp.ini");
            XMLLibrary xml;
            string [] buildVersion;
            string [] newVersion = new string[4];
            IOLibrary ioUtil = new IOLibrary();

            // Create a BUILDS Folder if it does not exist
            // Clean up everything from previous build process
            Console.WriteLine("Cleaning up the BUILDS folder");
            Console.WriteLine();
            string buildFolder = Path.Combine(appFolder, "BUILDS");
            try
            {
                if (Directory.Exists(buildFolder))
                {
                    ioUtil.DeleteFolder(buildFolder);
                }                
                Directory.CreateDirectory(buildFolder);
            } 
            catch (IOException e)
            {
                // Could not delete folder or unable to create folder                
            }
            if (Directory.Exists(buildFolder) && File.Exists(makeBuildDocPath) && File.Exists(buildVersionXMLPath) && File.Exists(iniFilePath))
            {
                ///
                /// Only continue if the folder exist, we might not able to delete folder from previous build but it is not prevent the build process
                /// Because the template packager not ready for C# yet
                /// Will implement by calling CVConvert to open and run the script from Make Build document
                /// 
                Console.WriteLine("Reading build version");
                Console.WriteLine();
                xml = new XMLLibrary(buildVersionXMLPath);
                // Read version from Version.xml
                buildVersion = xml.ReadVersionFromXML();
                newVersion[0] = buildVersion[0];
                newVersion[1] = buildVersion[1];
                newVersion[2] = (Convert.ToInt32(buildVersion[2]) + 1).ToString(); // Increase the build to 1 for next build
                newVersion[3] = "A";
                // Write it back to Vesion.xml
                xml.WriteVersionToXML(newVersion);

                BuildProcess buildProcess = new BuildProcess(makeBuildDocPath, buildVersion);
                // Build PCUTIL
                string cwpPCUTIL = buildProcess.BuildPCUTIL();
                buildProcess.CleanUpAfterBuild(ioUtil.GetParentFolderFromFilePath(cwpPCUTIL));
                // Build Audit CWI
                string cwpCWIUpdate = buildProcess.BuildAuditCWI();
                buildProcess.CleanUpAfterBuild(ioUtil.GetParentFolderFromFilePath(cwpCWIUpdate));
                buildProcess.CopyIniFile(ioUtil.GetParentFolderFromFilePath(cwpCWIUpdate), iniFilePath);
                // Build Audit INT
                string cwpINTUpdate = buildProcess.BuildAuditINT();
                buildProcess.CleanUpAfterBuild(ioUtil.GetParentFolderFromFilePath(cwpINTUpdate));
                buildProcess.CopyIniFile(ioUtil.GetParentFolderFromFilePath(cwpINTUpdate), iniFilePath);
                // Build Audit US
                string cwpUSUpdate = buildProcess.BuildAuditUS();
                buildProcess.CleanUpAfterBuild(ioUtil.GetParentFolderFromFilePath(cwpUSUpdate));
                buildProcess.CopyIniFile(ioUtil.GetParentFolderFromFilePath(cwpUSUpdate), iniFilePath);

                PatchTemplate patchProcess = new PatchTemplate();
                // Patch Audit CWI
                patchProcess.PatchAuditCWI(cwpCWIUpdate);
                // Patch Audit INT
                patchProcess.PatchAuditINT(cwpINTUpdate);
                // Patch Audit US
                patchProcess.PatchAuditUS(cwpUSUpdate);

            }
        }       
    }
}
