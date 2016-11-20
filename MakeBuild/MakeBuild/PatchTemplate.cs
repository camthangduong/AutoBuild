using System;
using System.IO;
using BuildLibrary;

namespace MakeBuild
{
    public class PatchTemplate
    {
        private CWLibrary cwUtil;
        private IOLibrary ioUtil;
        private string buildDocPath;
        private string AuditCWIFolder = "Audit System";
        private string AuditINTFolder = "Audit International";
        private string AuditUSFolder = "AuditUS";

        public PatchTemplate (string docPath)
        {
            cwUtil = new CWLibrary();
            ioUtil = new IOLibrary();
            this.buildDocPath = docPath;
        }

        public string PatchAuditCWI (string cwpPath)
        {
            string cwpFilePath = "";
            if (File.Exists(cwpPath))
            {
                // Now we start the build process
                Console.WriteLine("Patch Audit CWI");
                cwUtil.InstallTemplate(cwpPath);
                while (ioUtil.IsCaseWareProcessRunning())
                {
                    // Wait for build to finish
                }
                // After the patch finish, will use the new script from Make Build document to repack it
                cwUtil.RunRepackger(this.buildDocPath, "TemplatePackager", Path.Combine(cwUtil.DocumentLibFolder, AuditCWIFolder, "AuditCWI.ac"), "Packager");
                cwpFilePath = ioUtil.FindTheFileByExtension(cwUtil.DocumentLibFolder, ".cwp");
                Console.WriteLine();
            }
            return cwpFilePath;          
        }

        public string PatchAuditINT(string cwpPath)
        {
            string cwpFilePath = "";
            if (File.Exists(cwpPath))
            {
                // Now we start the build process
                Console.WriteLine("Patch Audit CWI");
                cwUtil.InstallTemplate(cwpPath);
                while (ioUtil.IsCaseWareProcessRunning())
                {
                    // Wait for build to finish
                }
                // After the patch finish, will use the new script from Make Build document to repack it
                cwUtil.RunRepackger(this.buildDocPath, "TemplatePackager", Path.Combine(cwUtil.DocumentLibFolder, AuditINTFolder, "AuditINT.ac"), "Packager");
                cwpFilePath = ioUtil.FindTheFileByExtension(cwUtil.DocumentLibFolder, ".cwp");
                Console.WriteLine();
            }
            return cwpFilePath;
        }

        public string PatchAuditUS(string cwpPath)
        {
            string cwpFilePath = "";
            if (File.Exists(cwpPath))
            {
                // Now we start the build process
                Console.WriteLine("Patch Audit CWI");
                cwUtil.InstallTemplate(cwpPath);
                while (ioUtil.IsCaseWareProcessRunning())
                {
                    // Wait for build to finish
                }
                // After the patch finish, will use the new script from Make Build document to repack it
                cwUtil.RunRepackger(this.buildDocPath, "TemplatePackager", Path.Combine(cwUtil.DocumentLibFolder, AuditUSFolder, "AuditUS.ac"), "Packager");
                cwpFilePath = ioUtil.FindTheFileByExtension(cwUtil.DocumentLibFolder, ".cwp");
                Console.WriteLine();
            }
            return cwpFilePath;
        }
    }
}
