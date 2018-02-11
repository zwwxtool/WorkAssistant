using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace WorkAssistant
{
    public class XmlConfig
    {
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
                appXml.Load(filePath);
                XmlElement root = appXml.DocumentElement;
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
                string filePath = "WorkConfig.xml";
                if (!File.Exists(filePath))
                    return;
                workXml.Load(filePath);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
