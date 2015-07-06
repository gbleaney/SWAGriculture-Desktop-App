using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace SWAGriculture_Base_Station
{
    class Program
    {
        private static readonly string dir = @"C:\Users\Graham\AppData\Roaming\EnOcean\DolphinView\Logs\";
        private static readonly string file = @"EventLog_2015-07-06_124714.xml";

        static void Main(string[] args)
        {

            Tail tail = new Tail(dir+file, 1);

            tail.Changed += EventHandler;

            tail.Run();

            Console.Read();

            tail.Stop();

        }

        static void EventHandler(object obj, Tail.TailEventArgs args)
        {
            string line = args.Line;
            if (line.Contains("<Telegram"))
            {
                string id = line.Substring(line.IndexOf("ID=") + 4, 8);
                string data = line.Substring(line.IndexOf("Data=") + 6, 2);

                if (data == "10")
                {
                    Task.Run(()=>ResetTrap(id));
                }
                else if(data=="00")
                {
                    Task.Run(()=>TriggerTrap(id));
                }
                else
                {
                    Debug.Print("Error, Unknown data. ID: {0}, Data: {1}", id, data);
                }

                Debug.Print("ID: {0}, Data: {1}", id, data);
                
            }
        }

        static void ResetTrap(string id)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://swagriculture.me/reset");

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

            Debug.Print("Reset Sent. Response: {0}", responseString);
        }

        static void TriggerTrap(string id)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://swagriculture.me/trigger");

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

            Debug.Print("Trigger Sent. Response: {0}", responseString);

        }
    }
}
