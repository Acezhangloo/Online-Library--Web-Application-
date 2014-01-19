using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace WeBSA
{
    public partial class Help : System.Web.UI.Page
    {
        private const string SESSION_HELP_LIST = "HELPLIST";
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = DataLayer.GetHelpTable();
            Session[SESSION_HELP_LIST] = new DataView(dt);
            gvHelpList.DataSource = Session[SESSION_HELP_LIST];
            gvHelpList.DataBind();
            gvHelpList.Columns[0].Visible = false;
        }

        protected void gvHelpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView dt = Session[SESSION_HELP_LIST] as DataView;
            gvHelpList.DataSource = dt;
            gvHelpList.PageIndex = e.NewPageIndex;
            gvHelpList.SelectedIndex = -1;
            gvHelpList.DataBind();
        }
    }
}