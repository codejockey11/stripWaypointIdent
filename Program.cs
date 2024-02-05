using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace stripWaypointIdent
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] files = Directory.GetFiles(@"bgls/", "*.xml", SearchOption.AllDirectories);
            StreamWriter sw = new StreamWriter("waypoints.dat");

            foreach (String file in files)
            {
                StreamReader sr = new StreamReader(file);
                String srf = sr.ReadToEnd();
                sr.Close();
                
                String f = srf.Replace("&", "&amp;");
                String f2 = f.Replace("\"Pvt\"", "");
                String f3 = f2.Replace(" \"N\"", "");
                StreamWriter fsr = new StreamWriter("scratch.xml");
                fsr.Write(f3);
                fsr.Close();

                XmlDocument xdcDocument = new XmlDocument();

                xdcDocument.Load("scratch.xml");
                XmlElement xmlRoot = xdcDocument.DocumentElement;
                XmlNodeList xmlNodes;
                Char[] c = file.ToCharArray();
                if (c[6] == 'P')
                {
                    xmlNodes = xmlRoot.SelectNodes("Airport/Waypoint");
                }
                else
                {
                    xmlNodes = xmlRoot.SelectNodes("Waypoint");
                }

                if (xmlNodes.Count > 0)
                {
                    foreach (XmlNode xndNode in xmlNodes)
                    {
                        foreach (XmlAttribute attr in xndNode.Attributes)
                        {
                            if (String.Compare(attr.Name, "waypointIdent") == 0)
                            {
                                if (attr.Value.Length == 5)
                                {
                                    Console.WriteLine(file + "\t" + attr.Value);
                                    sw.WriteLine(attr.Value);
                                }
                            }
                        }
                    }
                }
            }

            sw.Close();
        }
    }
}
