using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeBSA
{
    public partial class _Default : System.Web.UI.Page
    {
        private const string CONST_LOGON_USER = "LOGON_USER";

        protected void Page_Load(object sender, EventArgs e)
        {
            string logonUser = Request.ServerVariables[CONST_LOGON_USER].ToString();
            List<string> domainUsername = Global.GetUserName(logonUser);
            if (!IsPostBack)
            {
                string domain = domainUsername[0];
                string username = domainUsername[1];
                if (DataLayer.Authenticate(domain, username) != WeBSARole.Unauthorized)
                {
                    pnlUnauthorized.Visible = false;
                    pnlAuthorized.Visible = true;
                    string fullname = DataLayer.ReturnFullName(domain, username);
                    lblLogonUser.Text = fullname + "!";
                }
                else
                {
                    pnlUnauthorized.Visible = true;
                    pnlAuthorized.Visible = false;
                }
            }
        }
    }
}
