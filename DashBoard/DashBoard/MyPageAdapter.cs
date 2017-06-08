namespace DashBoard
{
    using System.Web.UI;

    public class MyPageAdapter : System.Web.UI.Adapters.PageAdapter
    {

        public override PageStatePersister GetStatePersister()
        {
            return new SessionPageStatePersister(Page);
        }
    }
}