using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildLibrary
{
    public class XMLLibrary
    {
        private string vesionXMLFile;
        private XmlDocument xml;

        /// <summary>
        /// Constructor
        /// </summary>
        public XMLLibrary (string i_vesionXMLFile)
        {
            vesionXMLFile = i_vesionXMLFile;
            xml = new XmlDocument();
            string sText = System.IO.File.ReadAllText(i_vesionXMLFile, Encoding.GetEncoding(65001));
            int bRun = IsValidXmlString(sText);
            while (bRun != -1)
            {
                // Convert string to char array
                char[] myChar = sText.ToCharArray();
                myChar[bRun] = ' ';
                sText = myChar.ToString();
            }
            xml.LoadXml(sText);
        }
       
        /// <summary>
        /// Check if it is a valid XML text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private int IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return -1;
            }
            catch (Exception err)
            {
                string[] findNum = err.Message.Split(' ');
                // Convert the position to number
                int getNum = Convert.ToInt32(findNum[findNum.Length - 1]);
                return getNum;
            }
        }
        
        /// <summary>
        /// get node value
        /// </summary>
        /// <param name="sNode"></param>
        /// <returns></returns>
        private string getNodeValue(string sNode)
        {
            XmlNodeList currentNodelist = xml.SelectNodes(sNode);
            XmlNode currentNode = currentNodelist[0];
            return currentNode.InnerText;
        }

        /// <summary>
        /// set new value to node
        /// </summary>
        /// <param name="sNode"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        private bool setNodeValue(string sNode, string sValue)
        {
            XmlNodeList currentNodelist = this.xml.SelectNodes(sNode);
            if (currentNodelist.Count > 0)
            {
                XmlNode currentNode = currentNodelist[0];
                currentNode.InnerText = sValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Read version value from Version XML
        /// </summary>
        /// <returns></returns>
        public string [] ReadVersionFromXML ()
        {
            string [] ver = new string[9];
            // Get Version.xml file path
            // Get Version.xml file path                        
            ver[0] = getNodeValue("//Major");
            ver[1] = getNodeValue("//Minor");
            ver[2] = getNodeValue("//Build");
            ver[3] = getNodeValue("//Rev");
            ver[4] = getNodeValue("//BuildPath");
            ver[5] = getNodeValue("//MinMajor");
            ver[6] = getNodeValue("//MinMinor");
            ver[7] = getNodeValue("//MinBuild");
            ver[8] = getNodeValue("//MinRev");            
            return ver;
        }

        /// <summary>
        /// Write new value to the Version.xml
        /// </summary>
        /// <param name="newVersion"></param>
        public void WriteVersionToXML (string [] newVersion)
        {            
            // Get Version.xml file path                        
            setNodeValue("//Major", newVersion[0]);
            setNodeValue("//Minor", newVersion[1]);
            setNodeValue("//Build", newVersion[2]);
            setNodeValue("//Rev", newVersion[3]);
            xml.Save(vesionXMLFile);
        }
    }
}
