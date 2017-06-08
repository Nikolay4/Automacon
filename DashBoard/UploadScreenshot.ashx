<%@ Language="C#" Class="Upload" %>

using System.Web;
using System;
using System.Runtime.Serialization;
using DashBoard.Tools;

public class Upload : IHttpHandler{

    public void ProcessRequest (HttpContext context) {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string fileName = "lk_" + DateTime.Now.Ticks + "_" + uploads.FileName;
        string file = System.IO.Path.GetFileName(uploads.FileName);
        uploads.SaveAs(string.Format("\\\\192.168.100.202\\pictures\\{0}" , fileName));
        //provide direct URL here
        string url = "http://automacon.ru:8080/pictures/"+ fileName;
        result res = new result();
        res.fileName = fileName;
        res.uploaded = "1";
        res.url = url;
        var re =  Serializer.SerializeJSon<result>(res);
        context.Response.Write(re);
        context.Response.End();
    }

    public bool IsReusable {
        get { return false; }
    }

    [DataContract]
    private class result
    {
        [DataMember]
        public string uploaded { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public string url      { get; set; }
    }
}