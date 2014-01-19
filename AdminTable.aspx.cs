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
    public partial class AdminTable : System.Web.UI.Page
    {
      
        private const string SESSION_ADMIN_LIST = "AdminList";
        protected void AdminDataBinding()
        {
            DataTable data = DataLayer.GetAdminList();
            Session[SESSION_ADMIN_LIST] = new DataView(data);
            gvAdminList.DataSource = Session[SESSION_ADMIN_LIST];
            gvAdminList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                AdminDataBinding();
                pnlAddAdmin.Visible = false;

            }

            string LogonUser = Request.ServerVariables["LOGON_USER"].ToString();
            List<string> domainUsername = Global.GetUserName(LogonUser);
            if (DataLayer.Authenticate(domainUsername[0], domainUsername[1]) == WeBSARole.Administrator)
            {
                gvAdminList.Columns[4].Visible = false;
                btnAddAdmin.Visible = false;
            }
           
        }


        protected void gvAdminList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView data = Session[SESSION_ADMIN_LIST] as DataView;
            gvAdminList.DataSource = data;
            gvAdminList.PageIndex = e.NewPageIndex;
            gvAdminList.SelectedIndex = -1;
            gvAdminList.DataBind();
        }


        #region Delete
        protected void gvAdminList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvAdminList.Rows[e.RowIndex];
            Label lblAdminID = (Label)gvrow.FindControl("lblAdminID");

            ucDeleteConfirmation.Show(lblAdminID.Text);
        }

        protected void DeleteConfirmed(bool delete, string deleteItemID)
        {
            this.ucDeleteConfirmation.Hide();

            
            if (delete)
            {
                DataLayer.DeleteAdmin(Convert.ToInt32(deleteItemID));

                gvAdminList.EditIndex = -1;
                AdminDataBinding();
            }
            this.updPnlAdminList.Update();
        }
        #endregion Delete

        #region Sorting
        protected void gvAdminList_OnSorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortList = Session[SESSION_ADMIN_LIST] as DataView;
            this.gvAdminList.EditIndex = -1;
            if (sortList != null)
            {
                sortList.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvAdminList.DataSource = sortList;
                gvAdminList.DataBind();
                ShowSortDirection();
            }

            
        }

        private void ShowSortDirection()
        {
            for (int i = 0; i < gvAdminList.Columns.Count - 1; i++)
            {
                string columnName = ((LinkButton)gvAdminList.HeaderRow.Cells[i].Controls[0]).CommandArgument;
                if (columnName == GridViewSortExpression)
                {
                    TableCell tableCell = gvAdminList.HeaderRow.Cells[i];
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
            get { return ViewState["SortExpression"] as string ?? ""; }
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

        #region Edit
        protected void gvAdminList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblInvalidInput.Visible = false;
            lblAdminAdded.Visible = false;

            gvAdminList.EditIndex = e.NewEditIndex;
            gvAdminList.DataSource = Session[SESSION_ADMIN_LIST];
            gvAdminList.DataBind();
        }

        protected void gvAdminList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdminList.EditIndex = -1;
            gvAdminList.DataSource = Session[SESSION_ADMIN_LIST];
            gvAdminList.DataBind();
        }

        protected void gvAdminList_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            if (gvAdminList.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlEditAdminRole = (DropDownList)e.Row.FindControl("ddlEditAdminRole");
                if (ddlEditAdminRole == null)
                    return;
                DataTable dt = DataLayer.PopulateRole();
                Global.PopulateDropDownList(ddlEditAdminRole, dt);
                ddlEditAdminRole.Items.FindByText((e.Row.FindControl("lblRole_Description") as Label).Text).Selected = true;
            }
        }

        protected void gvAdminList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvAdminList.Rows[e.RowIndex];
            DropDownList ddlEditAdminRole = (DropDownList)gvrow.FindControl("ddlEditAdminRole");
            Label lblAdminID = (Label)gvrow.FindControl("lblAdminID");

            int ID = Convert.ToInt32(lblAdminID.Text);
            int roleID = Convert.ToInt32(ddlEditAdminRole.SelectedValue);
            DataLayer.EditAdminInfo(ID, roleID);
            gvAdminList.EditIndex = -1;

            AdminDataBinding();
            
            //refresh page
            Response.Redirect(Request.RawUrl);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
         
            if (tbAdminDomain.Text != "" && tbAdminName.Text != "" && ddlAdminRole.SelectedValue != "0")
            {
                int Role_value = Convert.ToInt32(ddlAdminRole.SelectedValue);
                if (DataLayer.IsValidAdminAdd(tbAdminDomain.Text, tbAdminName.Text))
                {
                    lblInvalidInput.Visible = true;
                    lblAdminAdded.Visible = false;
                }
                else
                {
                    DataLayer.AddAdminInfo(tbAdminDomain.Text, tbAdminName.Text, Role_value);
                    AdminDataBinding();
                    lblAdminAdded.Visible = true;
                    lblInvalidInput.Visible = false;
                    pnlAddAdmin.Visible = false;
                   
                }
            }
            else
            {
                lblInvalidInput.Visible = true;
                lblAdminAdded.Visible = false;
            }
        }

        protected void btnAddAdmin_Click(object sender, EventArgs e)
        {
            lblInvalidInput.Visible = false;
            lblAdminAdded.Visible = false;
            if (pnlAddAdmin.Visible == false)
            {
                pnlAddAdmin.Visible = true;
                

                DataTable dt = DataLayer.PopulateRole();
                Global.PopulateDropDownList(ddlAdminRole, dt);
                tbAdminDomain.Text = "";
                tbAdminName.Text = "";
                ddlAdminRole.SelectedValue = "0";

            }
            else
            {
                pnlAddAdmin.Visible = false;
                
            }
        }

        protected void btnAdminSearch_Click(object sender, EventArgs e)
        {
            
            ucOwnerSearch.Show();
            
        }

        

        protected void ucOwnerSearch_OwnerSelected(string domain, string username)
        {
            tbAdminDomain.Text = domain;
            tbAdminName.Text = username;
            this.updPnlAdminList.Update();
        }

       
        #endregion

        
    }
}