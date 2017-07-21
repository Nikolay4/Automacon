
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Windows.Forms;
using DashBoard.Tools;
using System.Web;
using DashBoard.Models;
using System.IO;
using System.Text;
using System.Web.Services;

namespace DashBoard.Tasks
{
    public partial class Tasks : Page
    {
        string SessionFilterContactName = "FilterContact";
        string SessionFilterTypeName =    "FilterType";
        string SessionFilterStatusName =  "FilterStatus";
        string SessionFilterIsActiveName = "FilterIsActive";

        #region parameters
        protected string FilterContact
        {
            get
            {
                if (Session[SessionFilterContactName] == null)
                    Session[SessionFilterContactName] = "Все";
                return Session[SessionFilterContactName].ToString();
            }
            set
            {
                Session[SessionFilterContactName] = value;
            }
        }

        protected string FilterType
        {
            get
            {
                if (Session[SessionFilterTypeName] == null)
                    Session[SessionFilterTypeName] = "Все";
                return Session[SessionFilterTypeName].ToString();
            }
            set
            {
                Session[SessionFilterTypeName] = value;
            }
        }

        protected string FilterStatus
        {
            get
            {
                if (Session[SessionFilterStatusName] == null)
                    Session[SessionFilterStatusName] = "Все";
                return Session[SessionFilterStatusName].ToString();
            }
            set
            {
                Session[SessionFilterStatusName] = value;
            }
        }

        protected bool FilterIsActive
        {
            get
            {
                if (Session[SessionFilterIsActiveName] == null)
                    Session[SessionFilterIsActiveName] = true;
                return (bool)Session[SessionFilterIsActiveName];
            }
            set
            {
                Session[SessionFilterIsActiveName] = value; 
            }
        }

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Получаем задачи
                GetTasks(FilterIsActive);
                // Применяем фильтры
                AttemptFilters();
            }
        }

        protected void SetSelected()
        {
            try
            {
                DropDownList_Types.SelectedValue = FilterType;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                FilterType = "Все";
                AttemptFilters(); SetDrops();
            }
            try
            {
                DropDownList_Statuses.SelectedValue = FilterStatus;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                FilterStatus = "Все";
                AttemptFilters();SetDrops();
            }
            try
            {
                DropDownList_Contacts.SelectedValue = FilterContact;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                FilterContact = "Все";
                AttemptFilters(); SetDrops();
            }
            DropDownList_IsActive.SelectedValue = FilterIsActive ? "Только активные" : "Все";
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
            HttpWebResponse responce;
            if (isActive)
                responce = API.Requests.Post(API.Links.N_ACTIVE_USER_TASKS_POST_LIST_TASK);
            else
                responce = API.Requests.Post(API.Links.N_ALL_USER_TASKS_POST_LIST_TASK);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                string result = "";
                using (Stream stream = responce.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                };
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
            if (!string.IsNullOrWhiteSpace(Text) && !string.IsNullOrEmpty(Text) && Text != "")
                { AttemptFilters(); }
        }

        protected void FilterByText()
        {
            string Text = TextBox8.Text;
            if (!string.IsNullOrWhiteSpace(Text) && !string.IsNullOrEmpty(Text) && Text != "")
            {
                currTasks = currTasks.FindAll(task => task.Name.Contains(Text) || task.Number.Contains(Text)).ToList();
                BindData();
            }

        }

        // Сброс всех фильтров
        protected void ClearFilters_Click(object sender, EventArgs e)
        {
            currTasks = allTasks;
            TextBox8.Text = "";
            FilterContact = "Все";
            FilterStatus= "Все";
            FilterType = "Все";
            SetSelected();
            BindData();
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
            contacts.AddRange(Contacts.Where(str => Contacts.Count(s => s == str) >= 1).Distinct().OrderBy(q => q).ToList());
            DropDownList_Contacts.DataSource = contacts;
            DropDownList_Contacts.DataBind();
            SetSelected();
        }
        
        // фильтры
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterContact = DropDownList_Contacts.SelectedValue.ToString();
            FilterType    = DropDownList_Types.SelectedValue.ToString();
            FilterStatus  = DropDownList_Statuses.SelectedValue.ToString();
            AttemptFilters();
        }

        // Применить фильтры
        protected void AttemptFilters()
        {
            currTasks = allTasks;
            FilterByText();
            if (FilterContact != "Все")
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
            string url = "/Tasks/Task/" + HttpUtility.UrlEncode(task.Number.Replace(" ", string.Empty).Replace(".","_"));
            Response.Redirect(url);
        }
        
        // эффекты для GridView
        protected void GridView_TaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#e1eefc'; this.style.cursor='pointer';");
                string val = Page.ClientScript.GetPostBackEventReference(GridView_TaskList,""/*, "Select$" + e.Row.RowIndex.ToString()*/);
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(GridView_TaskList, "Select$" + e.Row.RowIndex.ToString()));

                
                TaskModel task = currTasks[e.Row.RowIndex + GridView_TaskList.PageIndex * GridView_TaskList.PageSize];
                string url = "/Tasks/Task/" + HttpUtility.UrlEncode(task.Number.Replace(" ", string.Empty).Replace(".", "_"));

                e.Row.Attributes.Add("oncontextmenu", "OpenNewTab('" + url + "');return false;");
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='#FFFFFF';";
            }
        }

        protected void DropDownList_IsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterIsActive = DropDownList_IsActive.SelectedValue.ToString() != "Все";
            GetTasks(FilterIsActive);
            AttemptFilters();
        }

        //Перелистывание страницы
        protected void GridView_TaskList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_TaskList.PageIndex = e.NewPageIndex;
            GridView_TaskList.DataSource = currTasks;
            GridView_TaskList.DataBind();
        }
        
        // Кнопка поиска
        protected void ButtonClearSearch_Click(object sender, EventArgs e)
        {
            TextBox8.Text = "";
            AttemptFilters();
        }
    }
    
}
