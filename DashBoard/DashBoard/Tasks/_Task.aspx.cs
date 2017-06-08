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
using System.Web.Services;
using System.Web.Script.Services;

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
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            if (!IsPostBack)
            {
                var Id = Request.QueryString["id"];
                if (Id == null)
                    Id = RouteData.Values["id"] as string;
                if (Id != null) {
                    Id = HttpUtility.UrlDecode(Id).Replace("_", ".");
                    if (Id.ToUpper() == "ADD")
                    {
                        Page.Title = "Новое задание";
                        AddTask();
                    }
                    else
                    {
                        // спрячем/отключим не нужное
                        Button_Save.Visible = false;
                        DropDownList_Type.Enabled = false;
                        txtCkEditor.ReadOnly = true;

                        SetTask(Id);
                        ShowTask();
                        Page.Title = "Задание " + Task.Nomber + " от" + Task.Date;
                    }
                }
                else
                    Response.Redirect("~/Tasks/Tasks.aspx");
            }
        }

        #region Get Task
        private void ShowTask()
        {
            if(Task.Nomber != null)
            {
                SetMessages();
                DropDownList_Type.DataSource = types.Select(t => t.TypePresent).ToList();
                DropDownList_Type.SelectedIndex = types.Select(t => t.TypePresent).ToList().IndexOf(Task.Type);
                DropDownList_Type.DataBind();
                DropDownList_Type.Enabled = false;
                txtCkEditor.Text = Task.Content;
                if (Task.Status == "Не обработано") 
                {
                    TextBox_Title.Enabled = true;
                    Button_Save.Visible = true;
                    DropDownList_Type.Enabled = true;
                    txtCkEditor.ReadOnly = false;
                }
                else
                {
                    Button_Back.Text = "Вернуться";
                }
                
                Literal_2.Text = Task.Result;

                try
                {
                    TextBox_Date.Text = DateTime.ParseExact(Task.Date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy HH:mm:ss");
                }
                catch
                {
                    TextBox_Date.Text = Task.Date;
                }
                TextBox_Number.Text = Task.Nomber;
                TextBox_Status.Text = Task.Status;
                TextBox_Contact.Text = Task.Contact;
                Label_Descr.Text = "Задание " + Task.Nomber + " от " + Task.Date;
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
            HttpResponseMessage responce;
            responce = API.Requests.Get(API.Links.P_USER_TASK_GET_TASK,id);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var result = responce.Content.ReadAsStringAsync().Result;
                Task = Serializer.DeserializeJSon<TaskModel>(result.ToString());
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
            PlaceHolder_Newtask.Visible = true;;
            TextBox_Date.Text = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            DropDownList_Type.DataSource = types.Select(t => t.TypePresent).ToList();
            DropDownList_Type.SelectedIndex = 0;
            DropDownList_Type.DataBind();
            TextBox_Contact.Text = User.Identity.Name.Split('|')[0];
            TextBox_Title.Enabled = true;
        }

        
        protected void Button_SaveClick(object sender, EventArgs e)
        {
            if (IsValidete())
            {
                Task.BasicNomber = "";
                Task.Contact = "";
                Task.Content = txtCkEditor.Text;
                //Task.Date = DateTime.Now;
                Task.Date = TextBox_Date.Text;
                Task.DateAccept = "";
                Task.Name = TextBox_Title.Text;
                Task.Nomber = TextBox_Number.Text ?? "";
                Task.Result = "";
                Task.Status = "";
                Task.Type = types.Where(t => t.TypePresent == DropDownList_Type.SelectedItem.Text).First().TypeName;
                HttpResponseMessage responce;
                responce = API.Requests.Post(API.Links.P_NEWTASK_POST_TASK, Serializer.SerializeJSon(Task));
                if (responce.StatusCode == HttpStatusCode.OK)
                {
                    var result = responce.Content.ReadAsStringAsync().Result;
                    TaskModel tsk = Serializer.DeserializeJSon<TaskModel>(result.ToString());
                    if (tsk.Nomber != "" && tsk.Error == "")
                        SetTask(tsk.Nomber);
                    else
                    {
                        FailureText.Text = "Ошибка добавления задания, задание не сохранено";
                        ErrorMessage.Visible = true;
                    }
                    ShowTask();
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
            HttpResponseMessage responce;
            responce = API.Requests.Get(API.Links.N_GETTASKTYPES_GET_LIST_STRING);

            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var result = responce.Content.ReadAsStringAsync().Result;
                model = Serializer.DeserializeJSon<List<Types>>(result.ToString());
            }

            return model;
        }
        #endregion

        #region Messages
        //private void PopulateTreeView(List<MessageModel> subMess, int parentId, TreeNode treeNode)
        //{
        //    foreach (MessageModel mess in subMess)
        //    {
        //        TreeNode child = new TreeNode();
        //        child.Text = CreateNode(mess);
        //        child.SelectAction = TreeNodeSelectAction.None;
        //        //child.Value = CreateNode(mess);
        //        //child.ChildNodes.Add(child1);
        //        if (parentId == 0)
        //        {
        //            sampleTree.Nodes.Add(child);
        //        }
        //        else
        //        {
        //            treeNode.ChildNodes.Add(child);
        //        }
        //        List<MessageModel> m = Task.Messages.Where(mm => mm.ParentId == mess.Id).ToList();
        //        PopulateTreeView(m, mess.Id, child);

        //    }
        //}


        //private string CreateNode(MessageModel message)
        //{
        //    string result = "";
        //    if (message.Type == false)
        //        result += "<img src=\"/Content/Images/error.png\" />";
        //    else
        //        result += "<img src=\"/Content/Images/question.png\" />";
        //    result += message.DateTime;
        //    result += "<b>   От: " + message.UserName + "</b>";
        //    result += (message.IsProcessed ? " <i style =\"color:green; padding-left:10px;\">Обработано" : "<i style =\"color:red; padding-left:10px;\">Не обработано") + "</i>";
        //    result += "<a onclick =\"showhide(this)\" style=\"cursor: pointer;padding-left: 10px;\" >Развернуть</a>";
        //        //+ "<br />";
        //    result += "<div class=\"question\">" + message.Question + "</div>";
        //    //if() // завершена ли задача ?
        //    //string buttonClick = Page.ClientScript.GetPostBackEventReference(sampleTree, "s" + message.Id);
        //    //result += "<br /><a href=\"javascript:" + buttonClick + "\" onclick=\"TreeView_SelectNode(MainContent_sampleTree_Data, this.parent,this.parent.id');\" >Ответить</a>";
        //    if (message.Answer != null && message.Answer != "")
        //    {
        //        result += "<br /><div class=\"answer\"><b>Ответ</b>" + (message.IsRead ? " <a style =\"color:green; padding-left:10px;cursor:pointer;\">Прочитано" : "<a style =\"color:red; padding-left:10px;cursor:pointer\">Пометить прочитанным") + "</a><br />";
        //        if (message.IsRead)
        //            result += message.Answer;
        //        else
        //            result += "<b>" + message.Answer + "</b>";
        //        result += "</div>";
        //    }
        //    result += "<div id=\"button_newmess\"><button id=\"\" class=\"Button_ok\" onclick=\"show('block', '1'); return false;\" ><img src = \"/Content/Images/new_message.png\" /> Написать </button ></div> ";
        //    return result;
        //}

        protected void Button_SendMessage_Click(object sender, EventArgs e)
        {

        }

        protected void IsRead(object sender, EventArgs e)
        {
            string ktoMolodec = "kol9-kol9-kol9";
        }
        
        public void SetMessages()
        {
            
            Random rnd = new Random();
            Task.Messages = new List<MessageModel>();
            for (int i = 1; i < 4; i++)
            {
                Task.Messages.Add(new MessageModel
                {
                    Id = i,
                    ParentId = 0,
                    IsProcessed = (i % 2 == 0 ? true : false),
                    Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<img src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
                    //IsRead = (i % 4 == 1 ? true : false),
                    IsRead = true,
                    Answer = (i % 2 == 0) ? "<br /> bla-bla-bla" : "",
                    Type = (i % 3 == 2 ? true : false),
                    UserName = "Имя пользователя#" + i,
                    DateTime = "14.07.16",
                    children = "{ }",
                });
                for (int j = 10; j < 14; j++)
                {
                    Task.Messages.Add(new MessageModel
                    {
                        Id = j,
                        ParentId = i,
                        IsProcessed = (j % 2 == 0 ? true : false),
                        Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<img src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
                        
                        Answer = (j % 2 == 0) ? "<br /> bla-bla-bla-bla" : "",
                        //IsRead = (j % 5 == 1 ? true : false),
                        IsRead = true,
                        Type = (j % 3 == 2 ? true : false),
                        UserName = "Имя пользователя#" + i + j,
                        DateTime = "14.07.16",
                        children = "{ }",
                        
                    });
                    for (int k = 20; k < 24; k++)
                    {
                        Task.Messages.Add(new MessageModel
                        {
                        Id = k,
                        ParentId = j,
                        IsProcessed = (k % 2 == 0 ? true : false),
                        Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<img src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
                        //IsRead = (k % 6 == 1 ? true : false),
                        IsRead = true,
                        Answer = (k % 2 == 0) ? "<br /> bla-bla-bla-bla-bla" : "",
                        Type = (k % 3 == 2 ? true : false),
                        UserName = "Имя пользователя#" + i + j + k,
                        DateTime = "14.07.16",
                        children = "{ }",
                        });
                        for (int l = 30; l < 33; l++)
                        {
                            Task.Messages.Add(new MessageModel
                            {
                                Id = l,
                                ParentId = k,
                                IsProcessed = (l % 2 == 0 ? true : false),
                                Question = "В приложении скан с печатью ИП Широков.прошу вставить во франче в печатные формы счета на оплату и реализаций, во все формы только где в конце стоит ПП по организации ИП Широков.<br /><image src =\"http://d585tldpucybw.cloudfront.net/images/AvatarImages/e0bbba7a-3785-4dce-8b0b-1dce6d5e9901tw_avatar_v2.png\" />",
                                IsRead = rnd.Next(0,4) < 3,
                                Answer = (l % 2 == 0) ? "<br /> bla-bla-bla-bla-bla" : "",
                                Type = (l % 3 == 2 ? true : false),
                                UserName = "Имя пользователя#" + i + j + k + l,
                                DateTime = "14.07.16",
                                children = "{ }",
                            });
                        }
                    }
                }
            }
            //sampleTree.Nodes.Clear();
            //TreeNode root = new TreeNode();
            //root.Value = "<h4>Общение по заданию</h4>";
            //root.SelectAction = TreeNodeSelectAction.None;
            //sampleTree.Nodes.Add(root);

            //PopulateTreeView(Task.Messages.Where(m => m.ParentId == 0).ToList(), 0, null);
        }

        #endregion
        [DataContract]
        public class Types
        {
            [DataMember]
            public string TypeName { get; set; }
            [DataMember]
            public string TypePresent { get; set; }
        }

        protected void Button_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Tasks/Tasks.aspx");
        }

        public string getNode()
        {
            return Serializer.SerializeJSon(Task.Messages);
        }
    }
}