using CaseViewGateway;
using CaseWare;
using System;
using System.Diagnostics;

namespace BuildLibrary
{
    public class CWLibrary
    {
        private CWApplication cwApp;
        private string cwPrgPath;
        private CvGtwyApp gtApp;

        /// <summary>
        /// Constructor
        /// </summary>
        public CWLibrary ()
        {
            cwApp = new CWApplication();
            gtApp = new CvGtwyApp();
            cwPrgPath = cwApp.ApplicationInfo["ProgramPath"];
        }

        public string CaseWareFolder
        {
            get { return cwPrgPath; }
        }

        public string DocumentLibFolder
        {
            get { return System.IO.Path.Combine(cwPrgPath, "Document Library"); }
        }

        /// <summary>
        /// Install template cwp file
        /// </summary>
        /// <param name="cwpFile"></param>
        /// <returns></returns>
        public bool InstallTemplate(string cwpFile)
        {
            //Create a new process info structure.
            Process pInfo = new Process();
            pInfo.StartInfo.FileName = System.IO.Path.Combine(cwPrgPath, "cwpackager.exe");
            pInfo.StartInfo.Arguments = "-u \"" + cwpFile + "\"";
            pInfo.StartInfo.UseShellExecute = false;
            //Start the process.
            pInfo.Start();
            try
            {
                //Wait for the window to finish loading.
                pInfo.WaitForInputIdle();
                //Wait for the process to end.
                pInfo.WaitForExit();
            }
            catch { }

            return true;
        }

        /// <summary>
        /// Convert the ac file to current Working Papers version
        /// </summary>
        /// <param name="sACFile"></param>
        /// <returns></returns>
        public bool ConvertAC(string sACFile)
        {
            try
            {
                cwApp.Clients.Convert(sACFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Run script from CaseView Document
        /// </summary>
        /// <param name="sDocPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="scriptPath"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool RunScriptOnDocument(string sDocPath, string scriptName, string type, string funcName, string [] args)
        {
            try
            {
                GC.Collect();
                /*Open the document */
                CvGtwyDocument oCurDoc = gtApp.Documents.Open(sDocPath, 0);
                // Set the cell value for the build based on type
                RunUpdateCell(oCurDoc, type, args);                
                // Run the script                                                
                try { oCurDoc.RunScriptVariant(scriptName, "", funcName, type); }
                catch (Exception) { }
                // Close the document                
                oCurDoc.Save();
                oCurDoc.Close(1);
                oCurDoc = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Run repkacger for the ac file
        /// </summary>
        /// <param name="sDocPath"></param>
        /// <param name="scriptName"></param>
        /// <param name="type"></param>
        /// <param name="funcName"></param>
        public void RunRepackger (string sDocPath, string scriptName, string acFilePath, string funcName)
        {
            try
            {
                GC.Collect();
                /*Open the document */
                CvGtwyDocument oCurDoc = gtApp.Documents.Open(sDocPath, 0);                
                // Run the script                                                
                try { oCurDoc.RunScriptVariant(scriptName, "", funcName, acFilePath); }
                catch (Exception) { }
                // Close the document                
                oCurDoc.Save();
                oCurDoc.Close(1);
                oCurDoc = null;                
            }
            catch (Exception)
            {                
            }
        }

        /// <summary>
        /// Update the cell in document for version build
        /// </summary>
        /// <param name="sDocPath"></param>
        /// <param name="aVer"></param>
        public void SetCellBuildVersion (string sDocPath, string [] aVer)
        {
            /*Open the document */
            CvGtwyDocument oCurDoc = gtApp.Documents.Open(sDocPath, 0);
            CvGtwyCell oCell = oCurDoc.Cells.get_ItemByName("VERMAJ");
            oCell.Value = aVer[0];
            oCell = oCurDoc.Cells.get_ItemByName("VERMIN");
            oCell.Value = aVer[1];
            oCell = oCurDoc.Cells.get_ItemByName("VERBUL");
            oCell.Value = aVer[2];
            oCurDoc.Save();
            oCurDoc.Close(1);
        }

        /// <summary>
        /// Update cells in makebuild.cvw document
        /// </summary>
        /// <param name="i_args"></param>
        /// <param name="sDocPath"></param>
        private void RunUpdateCell(CvGtwyDocument i_oCVDoc, string sBuild, string [] i_args)
        {
            // Update cell values in document
            string[] aVer = i_args;
            CvGtwyCell oCell = i_oCVDoc.Cells.get_ItemByName("VERMAJ");
            oCell.Value = aVer[0];
            oCell = i_oCVDoc.Cells.get_ItemByName("VERMIN");
            oCell.Value = aVer[1];
            oCell = i_oCVDoc.Cells.get_ItemByName("VERBUL");
            oCell.Value = aVer[2];
            oCell = i_oCVDoc.Cells.get_ItemByName("VERREV");
            oCell.Value = aVer[3];            
            // Set the popup cell
            oCell = i_oCVDoc.Cells.get_ItemByName("WPGTEMPDIR");            
            switch (sBuild)
            {
                case "AUDITUS":
                    oCell.PopupSelection = 3;
                    break;
                case "AUDITINT":
                    oCell.PopupSelection = 2;
                    break;
                case "AUDITCWI":
                    oCell.PopupSelection = 1;
                    break;
                case "PCUTIL":
                    oCell.PopupSelection = 0;
                    break;
                default:
                    oCell.PopupSelection = 0;
                    break;
            }
            i_oCVDoc.Save();            
        }
    }
}
