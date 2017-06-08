using DashBoard.Models;
using DashBoard.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace DashBoard
{
    public class ValuesController : ApiController
    {
        public string GetTaskDescr(string id)
        {
            HttpWebResponse responce;
            responce = API.Requests.Get(API.Links.P_USER_TASK_GET_TASK, id.Trim(' '));
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                TaskModel Task = Serializer.DeserializeJSon<TaskModel>(result.ToString());
                return Task.Content.ToString();
            }
            else
                return "Error get description";
        }

    
    }

}