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
    public partial class category : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, expire = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            loadTable();
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

        //ENCRYPT QUERY STRING FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
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
                    //POPULATE TABLE WITH DATA FROM DB
                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["category"].ToString() + "</td>";

                    string strURLData = EncryptQueryString(dr["category_id"].ToString()); //ENCRYPT DATA IN URL

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=edit-category.aspx?" + strURLData + " title=" + "'Edit'" + ">";
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

                cmd = new MySqlCommand("INSERT INTO category (category, expire) VALUES (@category, @expire)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@category", txtCategory.Text);
                cmd.Parameters.AddWithValue("@expire", expire);

                cmd.ExecuteNonQuery();

                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = txtCategory.Text + " Created Succesfully.";
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

        protected void chkExpire_ServerChange(object sender, EventArgs e)
        {
            if (chkExpire.Checked)
            {
                expire = "1";
            }
            else
            {
                expire = "0";
            }
        }

    }
}