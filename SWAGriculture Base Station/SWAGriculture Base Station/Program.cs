using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace SWAGriculture_Base_Station
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = new FileStream(@"C:\Users\Graham\AppData\Roaming\EnOcean\DolphinView\Logs\EventLog_2015-06-28_124047.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string line;
                while (fs.Read())
                {
                    
                }
                //System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Book));
                //System.IO.StreamReader file = new System.IO.StreamReader(@"c:\temp\SerializationOverview.xml");
                //Book overview = new Book();
                //overview = (Book)reader.Deserialize(file);
                
                //Console.WriteLine(overview.title);
            }


        }

        async static void ResetTrap(string id)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://swagriculture.parseapp.com/reset");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "id=" + id;
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        async static void TriggerTrap(string id)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://swagriculture.parseapp.com/trigger");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "id=" + id;
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
