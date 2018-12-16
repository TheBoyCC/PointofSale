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
    public partial class edit_user : System.Web.UI.Page
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
                readRole();
                readUser(lblUserID.Text);
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

            con.Open();
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
            lblUrlString.Text = strReq;

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                lblUserID.Text = DecryptQueryString(strReq);
            }
            else
            {
                Response.Redirect("~/manager/view-user.aspx");
            }
        }

        //READ ROLE FROM DB
        private void readRole()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM role", con);
                cmd.Connection = con;

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["role"].ToString();
                    item.Value = dr["role_id"].ToString();

                    ddlRole.Items.Add(item);
                }
            }
            catch
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

        //READ SELECTED USER
        private void readUser(string user)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT first_name, last_name, role_id FROM user WHERE user_id = '" + user + "'", con);
                cmd.Connection = con;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txtFirstName.Text = dr["first_name"].ToString();
                    txtLastName.Text = dr["last_name"].ToString();
                    ddlRole.SelectedValue = dr["role_id"].ToString();
                }
            }
            catch
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
        private void saveDetails(string user)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("UPDATE user SET first_name = @first_name, last_name = @last_name, role_id = @role_id WHERE user_id = '" + user + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                cmd.Parameters.AddWithValue("@role_id", ddlRole.SelectedItem.Value);

                cmd.ExecuteNonQuery();

                //UPDATE ROLE IN ACCOUNT
                cmd = new MySqlCommand("UPDATE account SET role_id = @role_id WHERE user_id = '" + user + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@role_id", ddlRole.SelectedItem.Value);

                cmd.ExecuteNonQuery();

                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = "User " + txtFirstName.Text + " Updated Successfully.";
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

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~" + lblUrlString.Text);
        }

        protected void btnSaveuser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails(lblUserID.Text);
            }
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/view-user.aspx");
        }
    }
}