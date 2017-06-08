using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace DashBoard.API
{
    public class Requests
    {
        internal static HttpResponseMessage Post(string link, string model = "")
        {
            string Url = Links.GetLink(link);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                                        new AuthenticationHeaderValue(
                                            "Basic",
                                            Convert.ToBase64String(
                                                Encoding.UTF8.GetBytes(
                                                    string.Format("{0}:{1}", HttpContext.Current.User.Identity.Name.Split('|')[0], HttpContext.Current.User.Identity.Name.Split('|')[1]))));
            StringContent content = new StringContent(
                                                model,
                                                    Encoding.UTF8,
                                                        "application/json");
            return client.PostAsync(Url, content).Result;
        }

        internal static HttpResponseMessage Get(string link, string param = "")
        {
            string Url = Links.GetLink(link) + param;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                                        new AuthenticationHeaderValue(
                                            "Basic",
                                            Convert.ToBase64String(
                                                Encoding.UTF8.GetBytes(
                                                    string.Format("{0}:{1}", HttpContext.Current.User.Identity.Name.Split('|')[0], HttpContext.Current.User.Identity.Name.Split('|')[1]))));

            return client.GetAsync(Url).Result;
        }
    }
}