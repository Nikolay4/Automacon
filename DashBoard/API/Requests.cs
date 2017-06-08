using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;

namespace DashBoard.API
{
    public class Requests
    {
        internal static HttpWebResponse Post(string link, string model = "")
        {
            string Url = Links.GetLink(link);

            //StringContent content = new StringContent(model, Encoding.UTF8, "application/json");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Proxy = null;
            req.KeepAlive = true;
            string authInfo = HttpContext.Current.User.Identity.Name.Split('|')[0] + ":" + HttpContext.Current.User.Identity.Name.Split('|')[1];
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
            req.Method = "POST";
           
            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                streamWriter.Write(model);
                streamWriter.Flush();
                streamWriter.Close();
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            HttpWebResponse resp = HttpWebResponseExt.GetResponseNoException(req);

            
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            WriteTime(ts, Url);
            return resp;

        }

        internal static HttpWebResponse Get(string link, string param = "")
        {
            string Url = Links.GetLink(link) + param;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Proxy = null;
            req.KeepAlive = true;
            string authInfo = HttpContext.Current.User.Identity.Name.Split('|')[0] + ":" + HttpContext.Current.User.Identity.Name.Split('|')[1];
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
            req.Method = "GET";
         
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            WriteTime(ts, Url);
            return resp;
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization =
            //                            new AuthenticationHeaderValue(
            //                                "Basic",
            //                                Convert.ToBase64String(
            //                                    Encoding.UTF8.GetBytes(
            //                                        string.Format("{0}:{1}", HttpContext.Current.User.Identity.Name.Split('|')[0], HttpContext.Current.User.Identity.Name.Split('|')[1]))));

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //HttpResponseMessage result = client.GetAsync(Url).Result;
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //WriteTime(ts, Url);            
            //return result;

        }

        private static void WriteTime(TimeSpan ts, string url)
        {
            try
            {
                string text = string.Format(@"{0,-25:D}   ||   {1,-70:D}  ||  {4, -40:D}   ||   {2,-3} sec. {3,-4:D} milli-sec", DateTime.Now.ToString(), url, ts.Seconds,ts.Milliseconds, HttpContext.Current.User.Identity.Name.Split('|')[2]);
            
                using (StreamWriter file = new StreamWriter(@"C:\inetpub\wwwroot\asp.automacon.ru\WriteLines2.txt", true))
                {
                    file.WriteLine(text);
                }
            }
            catch(Exception ex)
            { }
        }
    }
}