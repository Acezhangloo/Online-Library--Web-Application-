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
    public partial class GenreTable : System.Web.UI.Page
    {
        
        private const string SESSION_GENRE_LIST = "GenreList";
        protected void GenreDataBinding()
        {
            DataTable dt = DataLayer.GetGenreList();
            Session[SESSION_GENRE_LIST] = new DataView(dt);
            gvGenreList.DataSource = Session[SESSION_GENRE_LIST];
            gvGenreList.DataBind();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GenreDataBinding();
                pnlAddGenre.Visible = false;
            };

        }
 


        protected void gvGenreList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView data = Session[SESSION_GENRE_LIST] as DataView;
            gvGenreList.DataSource = data;
            gvGenreList.PageIndex = e.NewPageIndex;
            gvGenreList.SelectedIndex = -1;
            gvGenreList.DataBind();
        }


        #region Delete
        protected void gvGenreList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)gvGenreList.Rows[e.RowIndex];
            Label lblGenreID = (Label)gvrow.FindControl("lblGenreID");

            ucDeleteConfirmation.Show(lblGenreID.Text);
        }

        protected void DeleteConfirmed(bool delete, string deleteItem)
        {
            this.ucDeleteConfirmation.Hide();

            //if delete is confirmed
            if (delete)
            {
                DataLayer.DeleteGenre(Convert.ToInt32(deleteItem));

                gvGenreList.EditIndex = -1;
                GenreDataBinding();
            }

            this.updPnlGenreList.Update();
        }
        #endregion Delete

        #region sort
        protected void gvGenreList_OnSorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortList = Session[SESSION_GENRE_LIST] as DataView;
            this.gvGenreList.EditIndex = -1;
            if (sortList != null)
            {
                sortList.Sort = e.SortExpression + " " + GetSortDirectionGenre(e.SortExpression);
                gvGenreList.DataSource = sortList;
                gvGenreList.DataBind();
                ShowSortDirectionGenre();
            }
        }

        private void ShowSortDirectionGenre()
        {
            for (int i = 0; i < gvGenreList.Columns.Count - 1; i++)
            {
                string columnName = ((LinkButton)gvGenreList.HeaderRow.Cells[i].Controls[0]).CommandArgument;
                if (columnName == GridViewSortExpressionGenre)
                {
                    TableCell tableCell = gvGenreList.HeaderRow.Cells[i];
                    Label lbl = new Label();
                    lbl.Text = " ";
                    Image img = new Image();
                    img.ImageUrl = (GridViewSortDirectionGenre == "ASC") ? "~/Images/ArrowUp.jpg" : "~/Images/ArrowDown.jpg";
                    tableCell.Controls.Add(lbl);
                    tableCell.Controls.Add(img);
                }
            }
        }

        private string GridViewSortDirectionGenre
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string GridViewSortExpressionGenre
        {
            get { return ViewState["SortExpression"] as string ?? ""; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirectionGenre(string columnName)
        {
            string sortDirection = "ASC";

            string sortExpression = GridViewSortExpressionGenre;

            if (sortExpression != null)
            {
                if (sortExpression == columnName)
                {
                    string lastDirection = GridViewSortDirectionGenre;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            GridViewSortDirectionGenre = sortDirection;
            GridViewSortExpressionGenre = columnName;

            return sortDirection;
        }
        #endregion

        #region Edit
        protected void gvGenreList_RowEditing(object sender, GridViewEditEventArgs e)
        {

            lblInvalidInput.Visible = false;
            lblGenreAdded.Visible = false;

            gvGenreList.EditIndex = e.NewEditIndex;
            gvGenreList.DataSource = Session[SESSION_GENRE_LIST];
            gvGenreList.DataBind();

        }

        protected void gvGenreList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvGenreList.EditIndex = -1;
            gvGenreList.DataSource = Session[SESSION_GENRE_LIST];
            gvGenreList.DataBind();
        }

        protected void gvGenreList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow gvrow = (GridViewRow)gvGenreList.Rows[e.RowIndex];
            TextBox tbEditGenreType = (TextBox)gvrow.FindControl("tbEditGenreType");
            Label lblGenreID = (Label)gvrow.FindControl("lblGenreID");

            int ID = Convert.ToInt32(lblGenreID.Text);

            string type = tbEditGenreType.Text;
            DataLayer.EditGenreInfo(ID, type);
            gvGenreList.EditIndex = -1;

            GenreDataBinding();

        }

        protected void btnAddGenre_Click(object sender, EventArgs e)
        {
            lblInvalidInput.Visible = false;
            lblGenreAdded.Visible = false;
            if (pnlAddGenre.Visible == false)
            {
                pnlAddGenre.Visible = true;
                tbGenreType.Text = "";
                tbGenreID.Text = "";
            }
            else
            {
                pnlAddGenre.Visible = false;

            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {

            int ID = Convert.ToInt32(tbGenreID.Text);

            if (tbGenreID.Text != "" && tbGenreType.Text != "" && !DataLayer.IsValidGenreID(ID))
            {

                DataLayer.AddGenreInfo(ID, tbGenreType.Text);

                GenreDataBinding();
                lblGenreAdded.Visible = true;
                lblInvalidInput.Visible = false;
                pnlAddGenre.Visible = false;

            }
            else
            {
                lblInvalidInput.Visible = true;
                lblGenreAdded.Visible = false;
            }


        }
        #endregion
       
    }
}