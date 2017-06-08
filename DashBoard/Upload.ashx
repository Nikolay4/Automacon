<%@ Language="C#" Class="Upload" %>

using System.Web;
using System;

public class Upload : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string fileName = "lk_" + DateTime.Now.Ticks + "_" + uploads.FileName;
        string file = System.IO.Path.GetFileName(uploads.FileName);
        uploads.SaveAs(string.Format("\\\\192.168.100.202\\pictures\\{0}" , fileName));
        //provide direct URL here
        string url = "http://automacon.ru:8080/pictures/"+ fileName;

        context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + (CKEditorFuncNum ?? "1 ")  +  ", \"" + url + "\");</script>");
        context.Response.End();
    }

    public bool IsReusable {
        get { return false; }
    }
}