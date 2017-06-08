using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DashBoard.API
{
    public class Links
    {
        // P_ - добавить параметр N_ - без параметров
        internal static readonly string N_ACTIVE_USER_TASKS_POST_LIST_TASK = "N_ACTIVE_USER_TASKS_POST_LIST_TASK";      // активные задачи
        internal static readonly string N_ALL_USER_TASKS_POST_LIST_TASK = "N_ALL_USER_TASKS_POST_LIST_TASK";         // все задачи
        internal static readonly string P_USER_TASK_GET_TASK = "P_USER_TASK_GET_TASK";                 // задача + ID
        internal static readonly string P_NEWTASK_POST_TASK = "P_NEWTASK_POST_TASK";              // новая задача
        internal static readonly string N_GETTASKTYPES_GET_LIST_STRING = "N_GETTASKTYPES_GET_LIST_STRING";             // список типов для новых задач

        public static string GetLink(string link)
        {
            return WebConfigurationManager.AppSettings[link] ?? "";
        }
    }
    
}