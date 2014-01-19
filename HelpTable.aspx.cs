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
    public partial class HelpTable : System.Web.UI.Page
    {

        private const string SESSION_HELP_TABLE = "HelpTable";
        protected void HelpDataBinding()
        {
            DataTable dataTable = DataLayer.GetHelpTable();
            Session[SESSION_HELP_TABLE] = new DataView(dataTable);
            gvHelpTable.DataSource = Session[SESSION_HELP_TABLE];
            gvHelpTable.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) //Bind GridView only when IsPostBack is false
            {
                 HelpDataBinding();
            }

        }

        protected void gvHelpTable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView data = Session[SESSION_HELP_TABLE] as DataView;
            gvHelpTable.DataSource = data;
            gvHelpTable.PageIndex = e.NewPageIndex;
            gvHelpTable.SelectedIndex = -1;
            gvHelpTable.DataBind();
        }


        #region Delete
        protected void gvHelpTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvHelpTable.Rows[e.RowIndex];
            Label lblHelpID = (Label)gvrow.FindControl("lblHelpID");

            ucDeleteConfirmation.Show(lblHelpID.Text);
        }

        protected void DeleteConfirmed(bool delete, string deleteItem)
        {
            this.ucDeleteConfirmation.Hide();

            //if delete is confirmed
            if (delete)
            {
                DataLayer.DeleteHelp(Convert.ToInt32(deleteItem));

                gvHelpTable.EditIndex = -1;
                HelpDataBinding();
            }
            this.updPnlHelp.Update();
            
        }
        #endregion Delete

        #region Sorting
        protected void gvHelpTable_OnSorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortList = Session[SESSION_HELP_TABLE] as DataView;
            this.gvHelpTable.EditIndex = -1;
            if (sortList != null)
            {
                sortList.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvHelpTable.DataSource = sortList;
                gvHelpTable.DataBind();
                ShowSortDirectionHelp();
            }


        }

        private void ShowSortDirectionHelp()
        {
            for (int i = 0; i < gvHelpTable.Columns.Count - 1; i++)
            {
                string columnName = ((LinkButton)gvHelpTable.HeaderRow.Cells[i].Controls[0]).CommandArgument;
                if (columnName == GridViewSortExpression)
                {
                    TableCell tableCell = gvHelpTable.HeaderRow.Cells[i];
                    Label lbl = new Label();
                    lbl.Text = " ";
                    Image img = new Image();
                    img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/Images/ArrowUp.jpg" : "~/Images/ArrowDown.jpg";
                    tableCell.Controls.Add(lbl);
                    tableCell.Controls.Add(img);
                }
            }
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

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? ""; }
            set { ViewState["SortExpression"] = value; }
        }

        #endregion

        #region Edit
        protected void gvHelpTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblInvalidInput.Visible = false;
            lblHelpAdded.Visible = false;

            gvHelpTable.EditIndex = e.NewEditIndex;
            gvHelpTable.DataSource = Session[SESSION_HELP_TABLE];
            gvHelpTable.DataBind();
        }

        protected void gvHelpTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHelpTable.EditIndex = -1;
            gvHelpTable.DataSource = Session[SESSION_HELP_TABLE];
            gvHelpTable.DataBind();
        }


        protected void gvHelpTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow gvrow = (GridViewRow)gvHelpTable.Rows[e.RowIndex];
            TextBox tbEditHelpTitle = (TextBox)gvrow.FindControl("tbEditHelpTitle");
            //TextBox tbEditDescription = (TextBox)gvrow.FindControl("tbEditDescription");
            Label lblHelpID = (Label)gvrow.FindControl("lblHelpID");
            //string description = tbEditDescription.Text;
            string text = Server.HtmlEncode(((TextBox)gvrow.FindControl("tbEditDescription")).Text);

            
            int ID = Convert.ToInt32(lblHelpID.Text);
            string title = tbEditHelpTitle.Text;
            
            DataLayer.EditHelpInfo(ID, title, text);
            gvHelpTable.EditIndex = -1;

            HelpDataBinding();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(tbHelpID.Text);

            if (tbHelpID.Text != "" && tbHelpTitle.Text != "" && tbDescription.Text != "" && !DataLayer.IsValidHelpID(ID))
            {
                string text = Server.HtmlEncode(tbDescription.Text);
                DataLayer.AddHelpInfo(ID, tbHelpTitle.Text, text);

                HelpDataBinding();
                lblHelpAdded.Visible = true;
                lblInvalidInput.Visible = false;
                pnlAddHelp.Visible = false;

            } 
            else
            {
                lblInvalidInput.Visible = true;
                lblHelpAdded.Visible = false;
            }


        }

        protected void btnAddHelp_Click(object sender, EventArgs e)
        {
            lblInvalidInput.Visible = false;
            lblHelpAdded.Visible = false;
            if (pnlAddHelp.Visible == false)
            {
                pnlAddHelp.Visible = true;
                tbDescription.Text = "";
                tbHelpID.Text = "";
                tbHelpTitle.Text = "";
            }
            else
            {
                pnlAddHelp.Visible = false;

            }
        }

        
    }
}
        #endregion
