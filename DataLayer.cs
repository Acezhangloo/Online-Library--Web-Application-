using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Configuration;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.IO;

namespace WeBSA
{
    public class DataLayer
    { 
        private const string CONST_REF_CURSOR = "v_result";

        private static OracleConnection EstablishConnection()
        {
            OracleConnection cnn = new OracleConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            return cnn;
        }

        private static void DisconnectOracle(OracleConnection cnn)
        {
            if (cnn.State != ConnectionState.Closed)
                cnn.Close();
        }

        private static DataTable ExecStoredProcedure(string StoredProc, OracleParameter[] parameters, string cursor_name)
        {
            OracleConnection cnn = EstablishConnection();
            DataTable dt = new DataTable();
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = StoredProc;
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < parameters.Length; i++)
                    cmd.Parameters.Add(parameters[i]);

                cmd.Connection = cnn;
                cnn.Open();
                cmd.ExecuteNonQuery();
                if (!System.String.IsNullOrEmpty(cursor_name))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.Fill(dt, (Oracle.DataAccess.Types.OracleRefCursor)cmd.Parameters[cursor_name].Value);
                }
            }
            catch 
            {
                DisconnectOracle(cnn);
            }
            finally
            {
                DisconnectOracle(cnn);
            }

            return dt;
        }

        #region Book Operations
        public static DataTable GetBookList(string v_title, string v_author, string v_pubyear, string v_genre, string v_domain, string v_owner)
        {
            OracleParameter[] parameters = new OracleParameter[7];
            parameters[0] = new OracleParameter("v_title", OracleDbType.Varchar2);
            parameters[0].Value = v_title;

            parameters[1] = new OracleParameter("v_pubyear", OracleDbType.Varchar2);
            parameters[1].Value = v_pubyear;
            
            parameters[2] = new OracleParameter("v_author", OracleDbType.Varchar2);
            parameters[2].Value = v_author;

            parameters[3] = new OracleParameter("v_genre", OracleDbType.Varchar2);
            parameters[3].Value = v_genre;

            parameters[4] = new OracleParameter("v_owner_domain", OracleDbType.Varchar2);
            parameters[4].Value = v_domain;

            parameters[5] = new OracleParameter("v_owner", OracleDbType.Varchar2);
            parameters[5].Value = v_owner;

            parameters[6] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WeBSA_SEARCH.GET_BOOKS_LIST", parameters, CONST_REF_CURSOR);

        }

        public static DataTable Upload_To_DB(byte[] v_content, byte[] v_coverpage, string v_title, string v_author, Int32 v_pubyear, Int32 v_genre, string v_domain, string v_owner, string v_filetype)
        {
            OracleParameter[] parameters = new OracleParameter[9];

            parameters[0] = new OracleParameter("v_content", OracleDbType.Blob);
            parameters[0].Value = v_content;

            parameters[1] = new OracleParameter("v_coverpage", OracleDbType.Blob);
            parameters[1].Value = v_coverpage;

            parameters[2] = new OracleParameter("v_title", OracleDbType.Varchar2);
            parameters[2].Value = v_title;

            parameters[3] = new OracleParameter("v_author", OracleDbType.Varchar2);
            parameters[3].Value = v_author;

            parameters[4] = new OracleParameter("v_pubyear", OracleDbType.Int32);
            parameters[4].Value = v_pubyear;

            parameters[5] = new OracleParameter("v_genre", OracleDbType.Int32);
            parameters[5].Value = v_genre;

            parameters[6] = new OracleParameter("v_domain", OracleDbType.Varchar2);
            parameters[6].Value = v_domain;

            parameters[7] = new OracleParameter("v_owner", OracleDbType.Varchar2);
            parameters[7].Value = v_owner;

            parameters[8] = new OracleParameter("v_filetype", OracleDbType.Varchar2);
            parameters[8].Value = v_filetype;

            return ExecStoredProcedure("PKG_WeBSA_UPLOAD.BOOK_TO_DB", parameters, string.Empty);
        }

        
 

        public static void EditBook(int ID, string title, string author, int pubyear, int genreID)
        {
            OracleParameter[] parameters = new OracleParameter[5];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_title", OracleDbType.Varchar2);
            parameters[1].Value = title;

            parameters[2] = new OracleParameter("v_pubyear", OracleDbType.Int32);
            parameters[2].Value = pubyear;

            parameters[3] = new OracleParameter("v_author", OracleDbType.Varchar2);
            parameters[3].Value = author;

            parameters[4] = new OracleParameter("v_genre", OracleDbType.Int32);
            parameters[4].Value = genreID;

            ExecStoredProcedure("PKG_WEBSA_SEARCH.BOOK_EDIT", parameters, string.Empty);
        }

        public static void DeleteBook(int ID)
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            ExecStoredProcedure("PKG_WEBSA_SEARCH.BOOK_DELETE", parameters, string.Empty);
        }

        public static byte[] DownloadBook(int ID, ref string fileType)
        {
            byte[] data = null;
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable result = ExecStoredProcedure("PKG_WEBSA_SEARCH.BOOK_DOWNLOAD", parameters, CONST_REF_CURSOR);

            data = (Byte[])result.Rows[0]["book_content"];
            fileType = result.Rows[0]["book_file_type"].ToString();
            return data;
        }

        public static DataTable PopulateDomain()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WEBSA.GET_DOMAIN_LIST", parameters, CONST_REF_CURSOR);
        }
        #endregion

        #region Administrator
        public static DataTable GetAdminList()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WeBSA_ADMIN.ADMINS_GET_LIST", parameters, CONST_REF_CURSOR);

        }
        public static void DeleteAdmin(int ID)
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            ExecStoredProcedure("PKG_WEBSA_ADMIN.ADMIN_DELETE", parameters, string.Empty);
        }
        public static void EditAdminInfo(int ID, int roleID)
        {
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_role", OracleDbType.Int32);
            parameters[1].Value = roleID;

            ExecStoredProcedure("PKG_WEBSA_ADMIN.ADMIN_EDIT", parameters, string.Empty);
        }

        public static DataTable PopulateAdmin()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WeBSA_ADMIN.GET_ROLE_LIST", parameters, CONST_REF_CURSOR);
        }
        #endregion

        #region Genre Operations
        public static DataTable GetGenreList()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WeBSA_ADMIN.GENRE_GET_LIST", parameters, CONST_REF_CURSOR);

        }

        public static void DeleteGenre(int ID)
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            ExecStoredProcedure("PKG_WEBSA_ADMIN.GENRE_DELETE", parameters, string.Empty);
        }

        public static void EditGenreInfo(int ID, string type)
        {
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_name", OracleDbType.Varchar2);
            parameters[1].Value = type;

            ExecStoredProcedure("PKG_WEBSA_ADMIN.GENRE_EDIT", parameters, string.Empty);
        }

        public static void AddGenreInfo(int ID, string type)
        {
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_type", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_name", OracleDbType.Varchar2);
            parameters[1].Value = type;

            ExecStoredProcedure("PKG_WEBSA_ADMIN.GENRE_ADD", parameters, string.Empty);
        }

        public static DataTable PopulateGenre()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WEBSA_SEARCH.GET_GENRE_LIST", parameters, CONST_REF_CURSOR);
        }

        public static DataTable PopulateRole() 
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WEBSA_ADMIN.GET_ROLE_LIST", parameters, CONST_REF_CURSOR);
        }

        public static DataTable AddAdminInfo(string GetDomain, string GetName, int Role_value) 
        {
            OracleParameter[] parameters = new OracleParameter[3];

            parameters[0] = new OracleParameter("v_GetDomain", OracleDbType.Varchar2);
            parameters[0].Value = GetDomain;

            parameters[1] = new OracleParameter("v_GetName", OracleDbType.Varchar2);
            parameters[1].Value = GetName;

            parameters[2] = new OracleParameter("v_Role_value", OracleDbType.Int32);
            parameters[2].Value = Role_value;

            return ExecStoredProcedure("PKG_WEBSA_ADMIN.ADMIN_ADD", parameters, CONST_REF_CURSOR);
        }

        public static bool IsValidAdminAdd(string domain, string name)
        {
            OracleParameter[] parameters = new OracleParameter[3];
            parameters[0] = new OracleParameter("v_domain", OracleDbType.Varchar2);
            parameters[0].Value = domain;

            parameters[1] = new OracleParameter("v_name", OracleDbType.Varchar2);
            parameters[1].Value = name;

            parameters[2] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable results = ExecStoredProcedure("PKG_WEBSA_ADMIN.VALID_ADMINADD", parameters, CONST_REF_CURSOR);

            if (results.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion


        #region Authentication
        public static WeBSARole Authenticate(string domain, string user)
        {
            OracleParameter[] parameters = new OracleParameter[3];
            parameters[0] = new OracleParameter("v_domain_name", OracleDbType.Varchar2);
            parameters[0].Value = domain;

            parameters[1] = new OracleParameter("v_user_name", OracleDbType.Varchar2);
            parameters[1].Value = user;

            parameters[2] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable results = ExecStoredProcedure("PKG_WEBSA.AUTHENTICATE", parameters, CONST_REF_CURSOR);
            if (results.Rows.Count > 0)
            {
                int role = Convert.ToInt16(results.Rows[0]["user_role"].ToString());
                return (WeBSARole)role;
            }
            return WeBSARole.Unauthorized;
        }

        public static string ReturnFullName(string domain, string username)
        {
            string fullname = string.Empty;
            OracleParameter[] parameters = new OracleParameter[3];
            parameters[0] = new OracleParameter("v_domain", OracleDbType.Varchar2);
            parameters[0].Value = domain;

            parameters[1] = new OracleParameter("v_user_name", OracleDbType.Varchar2);
            parameters[1].Value = username;

            parameters[2] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable results = ExecStoredProcedure("PKG_WEBSA.GET_FULL_NAME", parameters, CONST_REF_CURSOR);
            if (results.Rows.Count > 0)
            {
                if (results.Rows[0].ItemArray[0] != null)
                    fullname = results.Rows[0].ItemArray[0].ToString();
                if (results.Rows[0].ItemArray[1] != null)
                {
                    if (!String.IsNullOrEmpty(fullname))
                        fullname = fullname + " ";

                    fullname = fullname + results.Rows[0].ItemArray[1].ToString();                        
                }
            }
            if (string.IsNullOrEmpty(fullname))
                fullname = "Unknown User";
            
            return fullname;
        }

        public static DataTable GetOwnerList(string lastname, string firstname, string domain, string username)
        {
            OracleParameter[] parameters = new OracleParameter[5];
            parameters[0] = new OracleParameter("v_lastname", OracleDbType.Varchar2);
            parameters[0].Value = lastname;

            parameters[1] = new OracleParameter("v_firstname", OracleDbType.Varchar2);
            parameters[1].Value = firstname;

            parameters[2] = new OracleParameter("v_domain", OracleDbType.Varchar2);
            parameters[2].Value = domain;

            parameters[3] = new OracleParameter("v_user_name", OracleDbType.Varchar2);
            parameters[3].Value = username;

            parameters[4] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WEBSA.GET_OWNER_LIST", parameters, CONST_REF_CURSOR);
        }
        #endregion

        public static bool IsValidGenreID(int id)
        {
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_genre_id", OracleDbType.Int32);
            parameters[0].Value = id;

            parameters[1] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable results = ExecStoredProcedure("PKG_WEBSA_ADMIN.VALID_GENREID", parameters, CONST_REF_CURSOR);

            if (results.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        #region Help
        public static DataTable GetHelpTable()
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            return ExecStoredProcedure("PKG_WeBSA_HELP.HELP_GET_LIST", parameters, CONST_REF_CURSOR);

        }

        public static void DeleteHelp(int ID)
        {
            OracleParameter[] parameters = new OracleParameter[1];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            ExecStoredProcedure("PKG_WEBSA_HELP.HELP_DELETE", parameters, string.Empty);
        }

        public static void EditHelpInfo(int ID, string title, string description)
        {
            OracleParameter[] parameters = new OracleParameter[3];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_title", OracleDbType.Varchar2);
            parameters[1].Value = title;

            parameters[2] = new OracleParameter("v_description", OracleDbType.Varchar2);
            parameters[2].Value = description;


            ExecStoredProcedure("PKG_WEBSA_HELP.HELP_EDIT", parameters, string.Empty);
        }

        public static void AddHelpInfo(int ID, string title, string description)
        {
            OracleParameter[] parameters = new OracleParameter[3];
            parameters[0] = new OracleParameter("v_ID", OracleDbType.Int32);
            parameters[0].Value = ID;

            parameters[1] = new OracleParameter("v_title", OracleDbType.Varchar2);
            parameters[1].Value = title;

            parameters[2] = new OracleParameter("v_description", OracleDbType.Varchar2);
            parameters[2].Value = description;

            ExecStoredProcedure("PKG_WEBSA_HELP.HELP_ADD", parameters, string.Empty);
        }

        public static bool IsValidHelpID(int id)
        {
            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter("v_help_id", OracleDbType.Int32);
            parameters[0].Value = id;

            parameters[1] = new OracleParameter(CONST_REF_CURSOR, OracleDbType.RefCursor, ParameterDirection.Output);
            DataTable results = ExecStoredProcedure("PKG_WEBSA_HELP.VALID_HELPID", parameters, CONST_REF_CURSOR);

            if (results.Rows.Count > 0)
            { 
                return true;
            }
            return false;
        }
        #endregion
    }
}
