using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;

namespace WeBSA
{
    public enum WeBSARole
    {
        Unauthorized = -1,
        User = 0,
        Administrator = 1,
        SuperAdministrator = 2
    }

    public class Global
    {
        public static void PopulateDropDownList(DropDownList dropDownControl, DataTable data)
        {
            dropDownControl.Items.Insert(0, new ListItem("---Select---", "0"));
            dropDownControl.DataSource = data;
            if (data.Columns.Count > 1)
            {
                dropDownControl.DataValueField = data.Columns[0].ColumnName;
                dropDownControl.DataTextField = data.Columns[1].ColumnName;
            }
            else
            {
                dropDownControl.DataValueField = data.Columns[0].ColumnName;
                dropDownControl.DataTextField = data.Columns[0].ColumnName;
            }
            dropDownControl.DataBind();
        }

        public static List<string> GetUserName(string logonUser)
        {
            List<string> domainUsername = new List<string>();
            char[] charactersUsedToSplit = new char[] { '\\' };
            string[] domainUser = logonUser.Split(charactersUsedToSplit);
            domainUsername.Add(domainUser[0].ToString().ToLower());
            domainUsername.Add(domainUser[1].ToString().ToLower());
            return domainUsername;
        }





    } 
}