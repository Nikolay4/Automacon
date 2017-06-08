
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Windows.Forms;
using DashBoard.Tools;
using System.Web;
using DashBoard.Models;

namespace DashBoard.Tasks
{
    public partial class Tasks : Page
    {
        protected string FilterContact = "Все";
        protected string FilterType    = "Все";
        protected string FilterStatus  = "Все";

        protected List<TaskModel> allTasks //Все задачи  ViewState["allTasks"] 
        {
            get
            {
                if (ViewState["allTasks"] == null)
                    ViewState["allTasks"] = new List<TaskModel>();
                return (List<TaskModel>)ViewState["allTasks"];
            }
            set
            {
                ViewState["allTasks"] = value;
                SetDrops();
            }
        }
        public List<TaskModel> currTasks // храним текущий список заданий, точнее в ViewState["currTasks"] 
        {
            get
            {
                if (ViewState["currTasks"] == null)
                    ViewState["currTasks"] = new List<TaskModel>();
                return (List<TaskModel>)ViewState["currTasks"];
            }
            set { ViewState["currTasks"] = value; /*SetDrops();*/ }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Получаем задачи
                GetTasks(true);
                
                
                
                if (Session["FilterType"] != null)
                {
                    string aa = Session["FilterType"].ToString();
                    DropDownList_Types.SelectedIndex = int.Parse(Session["FilterType"].ToString());
                }

                if(Session["FilterActive"] != null)
                {
                    string aaa = Session["FilterActive"].ToString();
                    DropDownList_IsActive.SelectedIndex = int.Parse(Session["FilterActive"].ToString());
                }

                if(Session["FilterStatus"] != null)
                {
                    string aaaa = Session["FilterStatus"].ToString();
                    DropDownList_Statuses.SelectedIndex = int.Parse(Session["FilterStatus"].ToString());
                }
                DropDownList_IsActive_SelectedIndexChanged(null, new EventArgs());
            }
        }
        
        protected void GridView_TaskList_Sorted(object sender, EventArgs e)
        {

        }

        
        public void BindData()
        {
            GridView_TaskList.DataSource = null;
            var source = new BindingSource();
            source.DataSource = currTasks;
            GridView_TaskList.DataSource = source;
            GridView_TaskList.DataBind();
        }


        // Получить список заданий     
        public void GetTasks(bool isActive)
        {
            List<TaskModel> model = new List<TaskModel>();
            HttpResponseMessage responce;
            if (isActive)
                responce = API.Requests.Post(API.Links.N_ACTIVE_USER_TASKS_POST_LIST_TASK);
            else
                responce = API.Requests.Post(API.Links.N_ALL_USER_TASKS_POST_LIST_TASK);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var result = responce.Content.ReadAsStringAsync().Result;
                model = Serializer.DeserializeJSon<List<TaskModel>>(result.ToString());
            }
            allTasks = model;
            // Текущие задачи = все задачи
            currTasks = allTasks;
            // Привязываем данные
            BindData();
        }

        //событие сортировки
        protected void GridView_TaskList_Sorting1(object sender, GridViewSortEventArgs e)
        {
            if (currTasks.Count != 0)
            {
                var param = Expression.Parameter(typeof(TaskModel), e.SortExpression);
                // хитрая штука для сортировки
                var sortExpression = Expression.Lambda<Func<TaskModel, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);


                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    currTasks = currTasks.AsQueryable().OrderBy(sortExpression).ToList();
                    GridView_TaskList.DataSource = currTasks;
                    GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    currTasks = currTasks.AsQueryable().OrderByDescending(sortExpression).ToList();
                    GridView_TaskList.DataSource = currTasks;
                    GridViewSortDirection = SortDirection.Ascending;
                }
                GridView_TaskList.DataBind();
                TableCell cell = GridView_TaskList.HeaderRow.Cells[GetColumnIndex(e.SortExpression.ToString())];
                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    cell.CssClass = "sortasc";
                }
                else
                {
                    cell.CssClass = "sortdesc";
                }
            }
        }
        private int GetColumnIndex(string SortExpression)
        {
            int i = 0;
            foreach (DataControlField c in GridView_TaskList.Columns)
            {
                if (c.SortExpression == SortExpression)
                    break;
                i++;
            }
            return i;
        }

        //тип сортировки вперед/назад
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        // кнопка начать поиск по строке
        protected void Button1_Click(object sender, EventArgs e)
        {
            string Text = TextBox8.Text;
            currTasks = allTasks.FindAll(task => task.Name.Contains(Text)).ToList();
            BindData();
        }

        // Сброс всех фильтров
        protected void ClearFilters_Click(object sender, EventArgs e)
        {
            ClearFilrter();
        }
        protected void ClearFilrter()
        {
            currTasks = allTasks;
            BindData();
            TextBox8.Text = "";
        }
        
        // редактируем выпадающие списки
        // выберем из столбца уникальные записи и поместим в выпадающий список + 
        protected void SetDrops()
        {
            List<string> statuses = new List<string> { "Все" };
            List<string> Statuses = allTasks.Select(s => s.Status).ToList();
            statuses.AddRange(Statuses.Where(str => Statuses.Count(s => s == str) >= 1).Distinct().ToList());
            DropDownList_Statuses.DataSource = statuses;
            DropDownList_Statuses.DataBind();

            List<string> types = new List<string> { "Все" };
            List<string> Types = allTasks.Select(s => s.Type).ToList();
            types.AddRange(Types.Where(str => Types.Count(s => s == str) >= 1).Distinct().ToList());
            DropDownList_Types.DataSource = types;
            DropDownList_Types.DataBind();


            List<string> contacts = new List<string> { "Все" };
            List<string> Contacts = allTasks.Select(s => s.Contact).ToList();
            contacts.AddRange(Contacts.Where(str => Contacts.Count(s => s == str) >= 1).Distinct().ToList());
            DropDownList_Contacts.DataSource = contacts;
            DropDownList_Contacts.DataBind();
        }
        
        // фильтры
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterContact = DropDownList_Contacts.SelectedItem.ToString();
            FilterType    = DropDownList_Types.SelectedItem.ToString();
            FilterStatus  = DropDownList_Statuses.SelectedItem.ToString();

            Session["FilterType"] = DropDownList_Types.SelectedIndex.ToString();
            Session["FilterActive"] = DropDownList_IsActive.SelectedIndex.ToString();
            Session["FilterStatus"] = DropDownList_Statuses.SelectedIndex.ToString();

            currTasks = allTasks;
            if(FilterContact != "Все")
                currTasks = currTasks.Where(t => t.Contact == FilterContact).ToList();

            if (FilterType != "Все")
                currTasks = currTasks.Where(t => t.Type == FilterType).ToList();

            if (FilterStatus != "Все")
                currTasks = currTasks.Where(t => t.Status == FilterStatus).ToList();
            BindData();
        }
        
        // клик по строке с заданием - переход к заданию
        protected void GridView_TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskModel task = currTasks[GridView_TaskList.SelectedIndex + GridView_TaskList.PageIndex* GridView_TaskList.PageSize];
            //string id = System.Text.RegularExpressions.Regex.Replace(task.Number, @"\s+", ""); // убрать пробелы
            string url = "/Tasks/Task/" + HttpUtility.UrlEncode(task.Nomber.Replace(" ", string.Empty).Replace(".","_"));
            Response.Redirect(url);
        }
        
        // эффекты для GridView
        protected void GridView_TaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#e1eefc'; this.style.cursor='pointer'");
                string val = Page.ClientScript.GetPostBackEventReference(GridView_TaskList,""/*, "Select$" + e.Row.RowIndex.ToString()*/);
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(GridView_TaskList, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#FFFFFF';";
                e.Row.ToolTip = currTasks[e.Row.RowIndex].Name;
            }
        }

        protected void DropDownList_IsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_IsActive.SelectedItem.ToString() == "Все")
                GetTasks(false);
            else
                GetTasks(true);
            ClearFilrter();
        }

        protected void GridView_TaskList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_TaskList.PageIndex = e.NewPageIndex;
            GridView_TaskList.DataSource = currTasks;
            GridView_TaskList.DataBind();
        }

        protected void Button_AddTask_Click(object sender, EventArgs e)
        {

        }
    }
    
}