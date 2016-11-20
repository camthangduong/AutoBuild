using System.IO;
using System.Diagnostics;

namespace BuildLibrary
{
    public class IOLibrary
    {
        private Process[] pname;

        /// <summary>
        /// Constructor
        /// </summary>
        public IOLibrary ()
        {

        }

        public string GetParentFolderFromFilePath(string filePath)
        {            
            string FileName = Path.GetFileName(filePath);
            string ClientFolderPath = Path.GetFullPath(filePath).Replace(FileName, "");
            return ClientFolderPath;            
        }

        /// <summary>
        /// Find the file by its extension from the folder path provided
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public string FindTheFileByExtension (string folderPath, string ext)
        {
            string sFileName = "";
            if (Directory.Exists(folderPath))
            {                
                DirectoryInfo dirs = new DirectoryInfo(folderPath);
                // Set the Attributes for it
                dirs.Attributes = FileAttributes.Normal;
                FileInfo[] files = dirs.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Extension == ext)
                    {
                        sFileName = file.FullName;
                        break;
                    }
                }                                
            }
            return sFileName;
        }

        /// <summary>
        /// Delete the file
        /// </summary>
        /// <param name="delFile"></param>
        public void DeleteFile(string delFile)
        {
            try
            {
                if (File.Exists(delFile))
                {
                    File.SetAttributes(delFile, FileAttributes.Normal);
                    File.Delete(delFile);
                }
            }
            catch (IOException) { }                   
        }

        /// <summary>
        /// Delete all files in folder, except for CWP file
        /// </summary>
        /// <param name="delFolder"></param>
        public void DeleteFileInFolder(string delFolder)
        {
            try
            {
                if (Directory.Exists(delFolder))
                {
                    DirectoryInfo dir = new DirectoryInfo(delFolder);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        if (file.Extension.ToUpper() != "CWP")
                        {
                            DeleteFile(file.FullName);
                        }                        
                    }
                }
            }
            catch (IOException) { }                      
        }

        /// <summary>
        /// Delete folder and its content
        /// </summary>
        /// <param name="delFolder"></param>

        public void DeleteFolder(string delFolder)
        {
            try
            {
                if (Directory.Exists(delFolder))
                {
                    string[] dirs = Directory.GetDirectories(delFolder);
                    foreach (string dir in dirs)
                    {
                        DeleteFileInFolder(dir);
                        DeleteFolder(dir);
                    }
                    Directory.Delete(delFolder, true);
                }
            }
            catch (IOException) { }                     
        }

        /// <summary>
        /// Checking if the CaseWare Packager is running
        /// </summary>
        /// <returns></returns>
        public bool IsCaseWareProcessRunning ()
        {            
            pname = Process.GetProcessesByName("cwpackager");
            if (pname.Length == 0)
            {
                // Double check for process name 
                pname = Process.GetProcessesByName("cwpack~1");
            }
            if (pname.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
