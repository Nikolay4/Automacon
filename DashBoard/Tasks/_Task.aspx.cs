using DashBoard.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using DashBoard.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace DashBoard.Tasks
{
    public partial class _Task : Page
    {


        public TaskModel Task
        {
            get
            {
                if (ViewState["Task"] == null)
                    ViewState["Task"] = new TaskModel();
                return (TaskModel)ViewState["Task"];
            }
            set
            {
                ViewState["Task"] = value;
            }
        }

        List<Types> types
        {
            get
            {
                if (ViewState["Types"] == null)
                    ViewState["Types"] = GetTypes();
                return (List<Types>)ViewState["Types"];
            }
            set
            {
                ViewState["Types"] = value;
            }
        }

        List<Contacts> contacts
        {
            get
            {
                if (ViewState["Contacts"] == null || ((List<Contacts>)ViewState["Contacts"]).Count < 1)
                    ViewState["Contacts"] = GetContacts();
                return (List<Contacts>)ViewState["Contacts"];
            }
            set
            {
                ViewState["Contacts"] = value;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            UpdatePanel2.Visible = false;
            ErrorMessage.Visible = false;
            if (!IsPostBack)
            {
                var Id = Request.QueryString["id"];
                if (Id == null)
                    Id = RouteData.Values["id"] as string;
                if (Id != null)
                {
                    Id = HttpUtility.UrlDecode(Id).Replace("_", ".");
                    if (Id.ToUpper() == "ADD")
                    {
                        Page.Title = "Новое задание";
                        AddTask();
                    }
                    else
                    {
                        // спрячем/отключим не нужное
                        //Button_Save.Visible = false;
                        //DropDownList_Type.Enabled = false;
                        txtCkEditor.ReadOnly = true;
                        //DropDownList_Contact.Enabled = false;
                        SetTask(Id);
                        ShowTask();
                        Page.Title = "Задание " + Task.Number + " от" + Task.Date;
                    }
                }
                else
                    Response.Redirect("~/Tasks/Tasks.aspx");
            }
            if (Request.Form["__EVENTTARGET"] == "SetMessageRead")
            {
                string id = Request.Form["__EVENTARGUMENT"]; //id_isread
                SetMessageRead(id);
            }
            UpdatePanel2.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //UpdatePanel2.Visible = false;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //UpdatePanel2.Visible = true;
        }

        private void SetMessageRead(string id)
        {
            var mess = Task.Messages.Where(m => m.Id == id).FirstOrDefault();
            mess.IsRead = !mess.IsRead;
        }

        #region Get Task
        private void ShowTask()
        {
            if(Task.Number != null)
            {
                SetMessages1();
                if (PlaceHolder_Newtask.Visible)
                    PlaceHolder_Newtask.Visible = false;



                
                txtCkEditor.Text = Task.Content;

                if (Task.Status == "Не обработано") 
                {
                    DropDownList_Type.DataSource = types.Select(t => t.TypePresent).ToList();
                    Types currType = types.Where(k => k.TypeName == Task.Type || k.TypePresent == Task.Type).First();
                    DropDownList_Type.DataBind();
                    DropDownList_Type.SelectedIndex = types.IndexOf(currType);
                    DropDownList_Type.Enabled = true;

                    TextBox_Title.Enabled = true;
                    txtCkEditor.ReadOnly = false;

                }
                else
                {
                    ///переделай, пёс
                    DropDownList_Type.DataSource = new List<string>() { Task.Type };
                    DropDownList_Type.DataBind();
                    //ответственный
                    DropDownList_Contact.DataSource = new List<string>() { Task.Contact };
                    DropDownList_Contact.DataBind();

                    Button_Back.Text = "Вернуться";
                    DropDownList_Type.Enabled = false;
                }

                DropDownList_Contact.DataSource = contacts.Select(c => c.Name).ToList();
                DropDownList_Contact.DataBind();
                try
                {
                    DropDownList_Contact.SelectedIndex = contacts.IndexOf(contacts.Where(k => k.Name == Task.Contact).First());
                }
                catch (Exception ex)
                {

                }
                if (Task.Status.ToUpper() == "ЗАКРЫТО")
                {
                    Button_Save.Visible = false;
                    DropDownList_Contact.Enabled = false;
                }

                if (Task.Status.ToUpper() == "НА ПРОВЕРКЕ")
                {
                    PlaceHolder_AcceptTask.Visible = true;
                }
                else
                {
                    PlaceHolder_AcceptTask.Visible = false;
                }
                if(Task.AttachedFiles != null && Task.AttachedFiles.Count > 0)
                {
                    PlaceHolder_AttachedFiles.Visible = true;
                }


                
                Literal_2.Text = Task.Result;

                try
                {
                    Task.Date = DateTime.ParseExact(Task.Date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy HH:mm:ss");
                }
                catch
                {
                    try
                    {
                        Task.Date = DateTime.ParseExact(Task.Date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy HH:mm:ss");
                    }
                    catch
                    {
                        Task.Date = Task.Date;
                    }
                }
                //TextBox_Date.Text = Task.Date;
                //Прикрепленные файлы
                AttachedFiles.DataSource = Task.AttachedFiles;
                AttachedFiles.DataBind();
                //Статус и номер
                //TextBox_Number.Text = Task.Number;
                TextBox_Status.Text = Task.Status;
                TextBox_ExpectedTime.Text = Task.ExpectedTime;
                Label_Descr.Text = "Задание " + Task.Number + " от " + Task.Date;
                TextBox_Title.Text = Task.Name;
            }
            else
            {
                FailureText.Text = "Нет такого задания или произошла ошибка";
                ErrorMessage.Visible = true;
            }
        }

        private void SetTask(string id)
        {
            HttpWebResponse responce;
            responce = API.Requests.Get(API.Links.P_USER_TASK_GET_TASK,id);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                Task = Serializer.DeserializeJSon<TaskModel>(result.ToString());
                //Task.AttachedFiles = new List<Models.AttachedFiles>();
                //Task.AttachedFiles.Add(new AttachedFiles() { FileName = "firefox.gif", Url = "http://www.w3schools.com/images/compatible_firefox.gif" });
                //Task.AttachedFiles.Add(new AttachedFiles() { FileName = "chrome.gif", Url = "http://www.w3schools.com/images/compatible_chrome.gif" });
                //Task.AttachedFiles.Add(new AttachedFiles() { FileName = "file1", Url = "http://automacon.ru:8080/pictures/fece29b5-440b-4fc1-bb9e-50ba47288291" });

            }
            else
            {
                FailureText.Text = "Нет такого задания или произошла ошибка";
                ErrorMessage.Visible = true;
            }
        }
        #endregion

        #region Add Task
        private void AddTask()
        {
            PlaceHolder_Newtask.Visible = true;
            //TextBox_Date.Text = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");

            DropDownList_Type.DataSource = types.Select(t => t.TypePresent).ToList();
            DropDownList_Type.SelectedIndex = 0;
            DropDownList_Type.DataBind();

            DropDownList_Contact.DataSource = contacts.Select(t => t.Name).ToList();
            //DropDownList_Contact.SelectedValue = User.Identity.Name.Split('|')[2];
            try
            {
                string uname = User.Identity.Name.Split('|')[2];
                DropDownList_Contact.SelectedIndex = contacts.IndexOf(contacts.Where(k => k.Name == uname).First());
            }
            catch (Exception ex)
            {
            }
                DropDownList_Contact.DataBind();
            DropDownList_Contact.Enabled = true;

            TextBox_Title.Enabled = true;
        }

        
        protected void Button_SaveClick(object sender, EventArgs e)
        {
            if (IsValidete())
            {
                if (Task.Number == null) Task.Number = "";
                Task.BasicNumber = "";
                Task.Contact = contacts[DropDownList_Contact.SelectedIndex].Name;
                //Task.Contact = "";
                Task.Content = txtCkEditor.Text;
                Task.Date = DateTime.Now.ToString();
                //Task.Date = TextBox_Date.Text;
                Task.DateAccept = "";
                Task.Name = TextBox_Title.Text;
                
                //Task.Number = TextBox_Number.Text ?? "";
                Task.Result = "";
                Task.Status = "";
                Task.Type = types.Where(t => t.TypePresent == DropDownList_Type.SelectedItem.Text).First().TypeName;
                HttpWebResponse responce;
                responce = API.Requests.Post(API.Links.P_NEWTASK_POST_TASK, Serializer.SerializeJSon(Task));
                if (responce.StatusCode == HttpStatusCode.OK)
                {
                    string result = "";
                    using (Stream stream = responce.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        result = reader.ReadToEnd();
                    }
                    TaskModel tsk = Serializer.DeserializeJSon<TaskModel>(result.ToString());
                    if (tsk == null) tsk = new TaskModel();
                    if (tsk.Number != "" && tsk.Error == "")
                    {
                        Task = tsk;
                        ShowTask();
                        FailureText.Text = "Сохранено";
                        ErrorMessage.Visible = true;
                    }
                    else
                    {
                        FailureText.Text = Task.Error;
                        ErrorMessage.Visible = true;
                    }
                    
                }
                else
                {
                    FailureText.Text = "Ошибка добавления задания, задание не сохранено";
                    ErrorMessage.Visible = true;
                }
            }
        }

        private bool IsValidete()
        {
            //if(DropDownList_Type.SelectedIndex != 0)
            //{
                if(TextBox_Title.Text != "" && !String.IsNullOrWhiteSpace(TextBox_Title.Text))
                {
                    if (txtCkEditor.Text != "" && !String.IsNullOrWhiteSpace(txtCkEditor.Text))
                    {
                        return true;
                    }
                    else FailureText.Text = "заполните Описание";
                }
                else FailureText.Text = "заполните Название";
            //}
            //else FailureText.Text = "выберите Тип";
            ErrorMessage.Visible = true;
            return false;
        }

        protected List<Types> GetTypes()
        {
            List<Types> model = new List<Types>();
            HttpWebResponse responce;
            responce = API.Requests.Get(API.Links.N_GETTASKTYPES_GET_LIST_STRING);

            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                model = Serializer.DeserializeJSon<List<Types>>(result.ToString());
            }

            return model;
        }

        protected List<Contacts> GetContacts()
        {
            List<Contacts> model = new List<Contacts>();
            HttpWebResponse responce;
            responce = API.Requests.Get(API.Links.N_CONTACTLIST);

            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                model = Serializer.DeserializeJSon<List<Contacts>>(result.ToString());
            }
            List<Contacts> contacts = new List<Contacts>();
            contacts.AddRange(model.Where(str => model.Count(s => s == str) >= 1).Distinct().ToList());
            //model.Add(User.Identity.Name.Split('|')[0]);
            //model.Add("Чурбаков Николай");
            //model.Add("Ответственный №1");
            //model.Add("responsible #2");
            return contacts;
        }
        #endregion

        #region Messages
  

        protected void Button_SendMessage_Click(object sender, EventArgs e)
        {
            string text = TextBox1.Text;
            string ParentId = HiddenField1.Value;
            string UserName = User.Identity.Name.Split('|')[0];
            int type = RadioButtonList1.SelectedIndex;
            string Date = DateTime.Now.ToString();
            string Id = DateTime.Now.Ticks.ToString();
            Task.Messages.Add(new MessageModel() { Id = Id, Question = text, ParentId = ParentId, UserName = UserName, DateTime = Date, Type = type.ToString() });
            //HiddenField2.Value = getNode();
        }
        
        //public void SetMessages()
        //{
            
        //    Random rnd = new Random();
        //    Task.Messages = new List<MessageModel>();
        //    for (int i = 1; i < 2; i++)
        //    {
        //        Task.Messages.Add(new MessageModel
        //        {
        //            //Id = i,
        //            ParentId = 0,
        //            IsProcessed = (i % 2 == 0 ? true : false),
        //            Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков. />",
        //            //IsRead = (i % 4 == 1 ? true : false),
        //            IsRead = true,
        //            Answer = (i % 2 == 0) ? "<br /> bla-bla-bla" : "",
        //            //Type = (i % 3 == 2 ? true : false),
        //            UserName = "Имя пользователя#" + i,
        //            DateTime = "14.07.16 22:33:44",
        //            children = "{ }",
        //        });
        //        for (int j = 10; j < 14; j++)
        //        {
        //            Task.Messages.Add(new MessageModel
        //            {
        //                //Id = j,
        //                ParentId = i,
        //                IsProcessed = (j % 2 == 0 ? true : false),
        //                Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<img src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
                        
        //                Answer = (j % 2 == 0) ? "<br /> bla-bla-bla-bla" : "",
        //                //IsRead = (j % 5 == 1 ? true : false),
        //                IsRead = rnd.Next(0, 5) > 3,
        //                //Type = (j % 3 == 2 ? true : false),
        //                UserName = "Имя пользователя#" + i + j,
        //                DateTime = "15.07.16 16:25:36",
        //                children = "{ }",
                        
        //            });
        //            for (int k = 20; k < 24; k++)
        //            {
        //                Task.Messages.Add(new MessageModel
        //                {
        //                //Id = k,
        //                ParentId = j,
        //                IsProcessed = (k % 2 == 0 ? true : false),
        //                Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<img src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
        //                //IsRead = (k % 6 == 1 ? true : false),
        //                IsRead = rnd.Next(0, 4) > 3,
        //                Answer = (k % 2 == 0) ? "<br /> bla-bla-bla-bla-bla" : "",
        //                //Type = (k % 3 == 2 ? true : false),
        //                UserName = "Имя пользователя#" + i + j + k,
        //                DateTime = "16.07.16 9:04:01",
        //                children = "{ }",
        //                });
        //                for (int l = 30; l < 33; l++)
        //                {
        //                    Task.Messages.Add(new MessageModel
        //                    {
        //                        //Id = l,
        //                        ParentId = k,
        //                        IsProcessed = (l % 2 == 0 ? true : false),
        //                        Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<br /><image src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
        //                        IsRead = rnd.Next(0,4) < 3,
        //                        Answer = (l % 2 == 0) ? "<br /> bla-bla-bla-bla-bla" : "",
        //                        //Type = (l % 3 == 2 ? true : false),
        //                        UserName = "Имя пользователя#" + i + j + k + l,
        //                        DateTime = "14.07.16 13:04:22",
        //                        children = "{ }",
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    //sampleTree.Nodes.Clear();
        //    //TreeNode root = new TreeNode();
        //    //root.Value = "<h4>Общение по заданию</h4>";
        //    //root.SelectAction = TreeNodeSelectAction.None;
        //    //sampleTree.Nodes.Add(root);

        //    //PopulateTreeView(Task.Messages.Where(m => m.ParentId == 0).ToList(), 0, null);
        //}

        public void SetMessages1()
        {
            HttpWebResponse responce;
            //responce = API.Requests.Get(API.Links.P_GETTASKHISTORY_LIST_MESSAGES, Task.Number);//"ИП-00013339.01"
            //if (responce.StatusCode == HttpStatusCode.OK)
            //{
            //    string result = "";
            //    using (Stream stream = responce.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            //        result = reader.ReadToEnd();
            //    }
            //    Task.Messages = Serializer.DeserializeJSon<List<MessageModel>>(result.ToString());
            //    HiddenField2.Value = getNode();
            //}
            //else
            //{
            //    FailureText.Text = "Произошла ошибка! Не удалось получить историю по заданию";
            //    ErrorMessage.Visible = true;
            //}
        }

        #endregion
       

        protected void Button_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Tasks/Tasks.aspx");
        }

        public string getNode()
        {
            return Serializer.SerializeJSon(Task.Messages);
        }

        protected void Download(object sender, EventArgs e)
        {
            LinkButton lBtn = (LinkButton)sender;
            string Url = lBtn.ToolTip;
            string FileName = lBtn.Text;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            
            Response.Write(Url);
            Response.End();
        }

        protected void ButtonAcceptTask_Click(object sender, EventArgs e)
        {
            AcceptTaskModel model = new AcceptTaskModel();
            model.Reason = (RadioButtonList2.SelectedIndex + 1).ToString();
            model.Number = Task.Number.Replace(" ", string.Empty);
            HttpWebResponse responce;
            responce = API.Requests.Post(API.Links.P_ACCEPTTASK_POST, Serializer.SerializeJSon(model));
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                
                SetTask(Task.Number);
                ShowTask();
            }
            else
            {
                FailureText.Text = "Произошла ошибка";
                ErrorMessage.Visible = true;
            }
        }

        protected void Button_RejectTask_Click(object sender, EventArgs e)
        {
            AcceptTaskModel model = new AcceptTaskModel();
            model.Reason = CKEditor_Reject.Text;
            model.Number = Task.Number.Replace(" ", string.Empty);
            HttpWebResponse responce;
            responce = API.Requests.Post(API.Links.P_REJECTTASK_POST, Serializer.SerializeJSon(model));
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                SetTask(Task.Number);
                ShowTask();
            }
            else
            {
                FailureText.Text = "Произошла ошибка";
                ErrorMessage.Visible = true;
            }
        }
    }
}