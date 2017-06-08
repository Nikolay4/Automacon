using System;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DashBoard.Models;
using System.Net.Http;
using Microsoft.Owin.Security;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Web.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Account
{
    
    public partial class Login : Page
    {
        private CustomUserManager CustomUserManager { get; set; }

        public Login()
            : this(new CustomUserManager())
        {
        }

        public Login(CustomUserManager customUserManager)
        {
            CustomUserManager = customUserManager;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                var result = testRequest();
                switch (result)
                {
                    case SignInStatus.Success:
                        ApplicationUser usr = new ApplicationUser();
                        usr.UserName = Email.Text + "|" + Password.Text;
                        As(usr);
                        FormsAuthentication.SetAuthCookie(usr.UserName, true);
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Неудачная попытка входа";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        public async void As(ApplicationUser usr)
        {
            await SignInAsync(usr, RememberMe.Checked);
        }

        protected SignInStatus testRequest()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                                        new AuthenticationHeaderValue(
                                            "Basic",
                                            Convert.ToBase64String(
                                                Encoding.UTF8.GetBytes(
                                                    string.Format("{0}:{1}", Email.Text, Password.Text))));

            HttpResponseMessage responce = client.PostAsync(API.Links.GetLink(API.Links.N_ACTIVE_USER_TASKS_POST_LIST_TASK), new StringContent("")).Result;
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                return SignInStatus.Success;
            }
            else
            {
                var r = responce.Content;
                var rr = responce.RequestMessage.Content;
                return SignInStatus.Failure;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Context.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var identity = await CustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}