using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace PointSaleSystemWeb.manager
{
    public partial class edit_category : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();


            if (!IsPostBack)
            {
                getQueryString();
                loadTable();
                getCategory();
            }
            
        }

        //GET SESSION
        public void session()
        {
            if (Session["UserID"] != null)
            {
                userID = Session["UserID"].ToString();
            }
            else
            {
                Response.Redirect("~/default.aspx");
            }
        }

        //MYSQL CONNECTION
        public void connection()
        {
            sqlcon = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            con = new MySqlConnection(sqlcon);
        }

        //ENCRYPT QUERY FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }

        //DECRYPT QUERY FUNCTION
        private string DecryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Decrypt(strQueryString, "r0b1nr0y");
        }

        //GET DATA FROM URL
        private void getQueryString()
        {
            string strReq;
            strReq = Request.RawUrl;
            strReq = strReq.Substring(strReq.IndexOf('?') + 1);

            if (!strReq.Equals(""))
            {
                lblCategoryID.Text = DecryptQueryString(strReq);
            }
            else
            {
                Response.Redirect("~/manager/category.aspx");
            }
        }

        //READ CATEGORY FROM DB
        private void getCategory()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("SELECT category FROM category WHERE category_id= '" + lblCategoryID.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txtCategory.Text = dr["category"].ToString();
                }
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }
           
        }

        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("SELECT * FROM category", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    //POPULATE TABLE
                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["category"].ToString() + "</td>";

                    string strURLData = EncryptQueryString(dr["category_id"].ToString());

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary disabled'" + " href=edit-category.aspx?" + strURLData + " title=" + "'Edit'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "              Edit";
                    tableData += "          </a>";
                    tableData += "      </td>";

                    tableData += "</tr>";
                    tblCategory.InnerHtml = tableData;
                    i += 1;
                }
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }
        }

        //SAVE METHOD
        private void saveDetails()
        {
            try
            {
                connection();
                con.Open();
                cmd = new MySqlCommand("UPDATE category SET category = @category WHERE category_id = '" + lblCategoryID.Text + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@category", txtCategory.Text);

                cmd.ExecuteNonQuery();

                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = txtCategory.Text + " Updated Succesfully.";
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }
        }
        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/category.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails();
            }
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/edit-category.aspx");
        }
    }
}