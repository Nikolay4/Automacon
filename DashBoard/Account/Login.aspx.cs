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
using System.IO;
using Newtonsoft.Json.Linq;
using DashBoard.API;

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
                HttpWebResponse responce = testRequest();
                SignInStatus status = SignInStatus.Failure;
                string UserName = "";
                if (responce.StatusCode == HttpStatusCode.OK)
                {
                    status = SignInStatus.Success;
                    using (Stream stream = responce.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        UserName = reader.ReadToEnd();
                    }
                }

                switch (status)
                {
                    case SignInStatus.Success:
                        ApplicationUser usr = new ApplicationUser();
                        usr.UserName = Email.Text + "|" + Password.Text + "|" + UserName;
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

        protected HttpWebResponse testRequest()
        {
            string Url = Links.GetLink(Links.AUTHORIZATION);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Proxy = null;
            req.KeepAlive = true;
            string authInfo = Email.Text + ":" + Password.Text;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
            req.Method = "GET";
            req.Timeout = 100000;
            return HttpWebResponseExt.GetResponseNoException(req);
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