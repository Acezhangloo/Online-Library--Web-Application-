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
    public partial class Search : System.Web.UI.Page
    {
        private const string SESSION_SEARCH_LIST = "SearchList";
        private const string CONST_DOWNLOAD_STRING = "download";
        private const string CONST_LOGON_USER = "LOGON_USER";
        private const string CONST_DATE_MODIFIED = "date_modified";

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string logonUser = Request.ServerVariables[CONST_LOGON_USER].ToString();
            List<string> domainUsername = Global.GetUserName(logonUser);
            string domain = domainUsername[0].ToLower();
            string user = domainUsername[1].ToLower();
            if (!IsPostBack)
            {
                DataTable dtGenre = DataLayer.PopulateGenre();
                Global.PopulateDropDownList(ddlGenre, dtGenre);
                DataTable dtDomain = DataLayer.PopulateDomain();
                Global.PopulateDropDownList(ddlDomain, dtDomain);
                ddlDomain.SelectedValue = "mercer";
            }
            if (DataLayer.Authenticate(domain, user) == WeBSARole.User)
            {
                ddlDomain.SelectedValue = domain;
                ddlDomain.Enabled = false;
                tbOwner.Text = user;
                btnOwnerSearch.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateTable();
        }

        protected void PopulateTable()
        {
            DataTable dt = DataLayer.GetBookList(tbBookTitle.Text, tbAuthor.Text, tbPublicationYear.Text, ddlGenre.SelectedValue, ddlDomain.SelectedValue, tbOwner.Text);
            Session[SESSION_SEARCH_LIST] = new DataView(dt);
            gvSearchList.DataSource = Session[SESSION_SEARCH_LIST];
            gvSearchList.DataBind();
        }

        protected void btnOwnerSearch_Click(object sender, EventArgs e)
        {
            ucOwnerSearch.Show();
        }

        protected void ucOwnerSearch_OwnerSelected(string domain, string username)
        {
            ddlDomain.SelectedValue = domain;
            tbOwner.Text = username;
            this.updpnlEverything.Update();
        }

        protected void gvSearchList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView data = Session[SESSION_SEARCH_LIST] as DataView;
            gvSearchList.DataSource = data;
            gvSearchList.PageIndex = e.NewPageIndex;
            gvSearchList.SelectedIndex = -1;
            gvSearchList.DataBind();
        }
        #endregion

        #region Sorting
        protected void gvSearchList_OnSorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortList = Session[SESSION_SEARCH_LIST] as DataView;
            this.gvSearchList.EditIndex = -1;
            if (sortList != null)
            {
                sortList.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvSearchList.DataSource = sortList;
                gvSearchList.DataBind();
                ShowSortDirection();
            }

            //Session[SESSION_SEARCH_LIST] = sortList;
        }

        private void ShowSortDirection()
        {
            for (int i = 0; i < gvSearchList.Columns.Count - 3; i++)
            {
                string columnName = ((LinkButton)gvSearchList.HeaderRow.Cells[i].Controls[0]).CommandArgument;
                if (columnName == GridViewSortExpression)
                {
                    TableCell tableCell = gvSearchList.HeaderRow.Cells[i];
                    Label lbl = new Label();
                    lbl.Text = " ";
                    Image img = new Image();
                    img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/Images/ArrowUp.jpg" : "~/Images/ArrowDown.jpg";
                    tableCell.Controls.Add(lbl);
                    tableCell.Controls.Add(img);
                }
            }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? "book_title"; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirection(string columnName)
        {
            string sortDirection = "ASC";

            string sortExpression = GridViewSortExpression;

            if (sortExpression != null)
            {
                if (sortExpression == columnName)
                {
                    string lastDirection = GridViewSortDirection;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            GridViewSortDirection = sortDirection;
            GridViewSortExpression = columnName;

            return sortDirection;
        }
        #endregion

        #region Edit/Delete
        protected void gvSearchList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSearchList.EditIndex = e.NewEditIndex;
            gvSearchList.DataSource = Session[SESSION_SEARCH_LIST];
            gvSearchList.DataBind();
        }

        protected void gvSearchList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSearchList.EditIndex = -1;
            gvSearchList.DataSource = Session[SESSION_SEARCH_LIST];
            gvSearchList.DataBind();
        }

        protected void gvSearchList_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            LinkButton btn = (LinkButton)e.Row.FindControl("btnDownload");
            if (btn != null)
            {
                ScriptManager scriptManager1 = (ScriptManager)((WeBSA)this.Master).ReturnScriptManager();
                scriptManager1.RegisterPostBackControl(btn);
            }
            if (gvSearchList.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlEditBookGenre = (DropDownList)e.Row.FindControl("ddlEditBookGenre");
                if (ddlEditBookGenre == null)
                    return;
                DataTable data = DataLayer.PopulateGenre();
                Global.PopulateDropDownList(ddlEditBookGenre, data);
                ddlEditBookGenre.Items.FindByText((e.Row.FindControl("lblGenre_Description") as Label).Text).Selected = true;
            }
        }

        protected void gvSearchList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvSearchList.Rows[e.RowIndex];
            Label lblBookID = (Label)gvrow.FindControl("lblBookID");

            ucDeleteConfirmation.Show(lblBookID.Text);
        }

        protected void DeleteConfirmed(bool delete, string deleteItem)
        {
            this.ucDeleteConfirmation.Hide();

            //if delete is confirmed
            if (delete)
            {
                DataLayer.DeleteBook(Convert.ToInt32(deleteItem));

                gvSearchList.EditIndex = -1;
                PopulateTable();
            }
        }

        protected void gvSearchList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvSearchList.Rows[e.RowIndex];
            TextBox tbEditBookTitle = (TextBox)gvrow.FindControl("tbEditBookTitle");
            TextBox tbEditBookAuthor = (TextBox)gvrow.FindControl("tbEditBookAuthor");
            TextBox tbEditPublicationYear = (TextBox)gvrow.FindControl("tbEditPublicationYear");
            DropDownList ddlEditBookGenre = (DropDownList)gvrow.FindControl("ddlEditBookGenre");
            Label lblBookID = (Label)gvrow.FindControl("lblBookID");

            int ID = Convert.ToInt32(lblBookID.Text);
            string title = tbEditBookTitle.Text;
            string author = tbEditBookAuthor.Text;
            int pubyear = Convert.ToInt32(tbEditPublicationYear.Text);
            int genreID = Convert.ToInt32(ddlEditBookGenre.SelectedValue);
            DataLayer.EditBook(ID, title, author, pubyear, genreID);

            gvSearchList.EditIndex = -1;
            DataTable dt = DataLayer.GetBookList(tbBookTitle.Text, tbAuthor.Text, tbPublicationYear.Text, ddlGenre.SelectedValue, ddlDomain.SelectedValue, tbOwner.Text);
            Session[SESSION_SEARCH_LIST] = new DataView(dt);
            DataView sortList = Session[SESSION_SEARCH_LIST] as DataView;
            sortList.Sort = CONST_DATE_MODIFIED + " " + "DESC";
            gvSearchList.DataSource = sortList;
            gvSearchList.DataBind();
        }
        #endregion

        #region Download
        protected void gvSearchList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == CONST_DOWNLOAD_STRING)
            {
                GridViewRow gvrow = (GridViewRow)gvSearchList.Rows[Convert.ToInt32(e.CommandArgument)];
                Label lblBookId = (Label)gvrow.FindControl("lblBookID");
                Label lblBookTitle = (Label)gvrow.FindControl("lblBookTitle");
                int ID = Convert.ToInt32(lblBookId.Text);
                string title = lblBookTitle.Text;
                string fileType = string.Empty;
                byte[] buffer = DataLayer.DownloadBook(ID, ref fileType);
                string fileName = title + "." + fileType;
                Response.ClearContent();
                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ";");
                Response.ContentType = "." + fileType;
                Response.BinaryWrite(buffer);
                Response.Flush();
                Response.End();
            }
        }
        #endregion
    }
}