using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace WorkAssistant
{
    public class XmlConfig
    {
        public List<ProjectConfig> projects = new List<ProjectConfig>();

        private XmlDocument appXml = new XmlDocument();
        private XmlDocument workXml = new XmlDocument();

        private static Object s_lock = new Object();
        private static XmlConfig _instance = null;
        public static XmlConfig Instance
        {
            get
            {
                if (null != _instance)
                    return _instance;

                lock(s_lock)
                {
                    if (null == _instance)
                    {
                        XmlConfig temp = new XmlConfig();
                        //Volatile保证了XmlConfig构造器执行结束后，才把引用发布到_instance
                        Volatile.Write(ref _instance, temp);
                    }
                }
                
                return _instance;
            }
        }

        public void LoadAppConfig()
        {
            try
            {
                string filePath = "AppConfig.xml";
                if (!File.Exists(filePath))
                    return;
                /*appXml.Load(filePath);
                //http://www.w3school.com.cn/xmldom/dom_element.asp
                XmlElement root = appXml.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    Console.WriteLine(nodeList[i].InnerText);
                }*/
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void LoadWorkConfig()
        {
            try
            {
                projects.Clear();
                string filePath = string.Format("{0}/WorkConfig.xml", Directory.GetCurrentDirectory());
                if (!File.Exists(filePath))
                    return;
                workXml.Load(filePath);
                XmlElement root = workXml.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    switch(nodeList[i].Name)
                    {
                        case "project":
                            XmlNodeList prjNodeList = nodeList[i].ChildNodes;
                            ProjectConfig project_cfg = new ProjectConfig();
                            for (int j = 0; j < prjNodeList.Count; j++)
                            {
                                string fieldName = prjNodeList[j].Name;
                                string fieldValue = prjNodeList[j].InnerText;
                                Type type = project_cfg.GetType();
                                FieldInfo fi = type.GetField(fieldName);
                                if (null == fi)
                                    continue;
                                fi.SetValue(project_cfg, fieldValue);
                            }
                            projects.Add(project_cfg);
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    

    public sealed class ProjectConfig
    {
        public string name;
        public string client_pc;
        public string client_android;
        public string client_build_pc;
        public string client_build_android;
        public string client_svn_url;
    }
}
