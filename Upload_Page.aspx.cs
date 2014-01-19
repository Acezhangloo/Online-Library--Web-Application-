using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Oracle.DataAccess.Client;

namespace WeBSA
{
    public partial class Upload_Page : System.Web.UI.Page
    {
        private const string CONST_LOGON_USER = "LOGON_USER";
        #region page_load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtGenre = DataLayer.PopulateGenre();
                Global.PopulateDropDownList(GenreList, dtGenre);
            }
            string logonUser = Request.ServerVariables[CONST_LOGON_USER].ToString();
            List<string> domainUserName = Global.GetUserName(logonUser);
            string domain = domainUserName[0].ToLower();
            string user = domainUserName[1].ToLower();

            //If the user if unauthorized, DataLayer.Authenticate(domain, user) will return WeBSARole.Unauthorized which is equal to WeBSARole.User.
            if (DataLayer.Authenticate(domain, user) == WeBSARole.User)
            {
                authenticationInfor.Visible = true;
                BookTitle.Visible = false;
                Author.Visible = false;
                PublicationYear.Visible = false;
                GenreList.Visible = false;
                Upload_Button.Visible = false;

            }
        }
        #endregion
        
        #region Upload
        protected void Upload_Click(object sender, EventArgs e)
        {
            int genValue;
            int Num;
            int.TryParse(GenreList.SelectedValue, out genValue);
            int.TryParse(PublicationYear.Text, out Num);
            string emptyFileType="";
            string fileTypePdf = "pdf";
            string fileTypeDocx = "docx";
            string fileTypeDoc = "doc"; 
            string endWithJpg = ".jpg";
            string endWithPng = ".png";
            string fileType="";
            wordOrPdfOnly.Visible = false;
            jpgPngOnly.Visible = false;
            successfulInfo.Visible = false;
            authenticationInfor.Visible = false;
            rightPath.Text = "";
            rightPath.Visible = false;
            imagePath.Text = "";
            imagePath.Visible = false;
       

            string logonUser = Request.ServerVariables[CONST_LOGON_USER].ToString();
            List<string> domainUserName = Global.GetUserName(logonUser);
            string domain = domainUserName[0].ToLower();
            string user = domainUserName[1].ToLower();
            if (FileUpload.HasFile)
            {
                //if the user doesn't select genre, the genre will be set as others by default whose genre value is 1000;
                if (genValue == 0) {genValue = 1000;}
                byte[] fileData = FileUpload.FileBytes;
                string filePath = FileUpload.PostedFile.FileName.ToString();
                string filePathLower = filePath.ToLower();
                byte[] coverPageData = coverPage.FileBytes;
                string coverPath = coverPage.PostedFile.FileName.ToString().ToLower();
             
                if (!(filePath.EndsWith("."+fileTypePdf) || filePath.EndsWith("."+fileTypeDocx) || filePath.EndsWith("."+fileTypeDoc)))
                {
                    wordOrPdfOnly.Visible=true;
                }
                else if (!(coverPath.EndsWith(endWithJpg) || coverPath.EndsWith(endWithPng)) && coverPath!=emptyFileType) 
                {
                    wordOrPdfOnly.Visible = false;
                    jpgPngOnly.Visible = true;
                }
                else
                {
                    if (filePath.EndsWith("."+fileTypePdf))
                    {
                        fileType = fileTypePdf;
                    }
                    else if (filePath.EndsWith("."+fileTypeDocx))
                    {
                        fileType = fileTypeDocx;
                    }
                    else if (filePath.EndsWith("."+fileTypeDoc))
                    {
                        fileType = fileTypeDoc;
                    }
                    jpgPngOnly.Visible = false;
                    DataTable dt = DataLayer.Upload_To_DB(fileData, coverPageData, this.BookTitle.Text, this.Author.Text, Num, genValue, domain, user, fileType);
                    successfulInfo.Visible = true;
                }
                rightPath.Text = "The Book file path is: " + filePath;
                rightPath.Visible = true;
            }
            if (coverPage.HasFile)
            {
                string imagePathString = coverPage.PostedFile.FileName.ToString();
                imagePath.Text = "The coverpage file path is: " + imagePathString;
                imagePath.Visible = true;
            }
        }
    }
        
        #endregion


    
}
//Close 
             