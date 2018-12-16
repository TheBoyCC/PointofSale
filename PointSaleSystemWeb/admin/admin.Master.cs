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
using PointSaleSystemWeb.manager;


namespace PointSaleSystemWeb.admin
{
    public partial class admin : System.Web.UI.MasterPage
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, roleID, username, userLogID;

        protected void Page_Load(object sender, EventArgs e)
        {
            session();
            user();
            checkProductLevel();
        }

        //GET AND SETS SESSION
        public void session()
        {
            if (Session["UserID"] != null)
            {
                userID = Session["UserID"].ToString();
            }
            else
            {
                Response.Redirect("~/admin/default.aspx");
            }
        }

        //READ USER DETAILS FROM DB
        private void user()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM administrator WHERE admin_id = '" + userID + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblUser.Text = dr["full_name"].ToString();
                    lblUser1.Text = dr["full_name"].ToString();
                    username = dr["username"].ToString();

                    if (dr["picture"] == DBNull.Value)
                    {
                        imgUser.ImageUrl = "~/uploads/no-image.jpg";
                        imgUser1.ImageUrl = "~/uploads/no-image.jpg";
                    }
                    else
                    {
                        byte[] img = (byte[])dr["picture"];
                        string strbase64 = Convert.ToBase64String(img, 0, img.Length);
                        imgUser.ImageUrl = "data:Image/png;base64," + strbase64;
                        imgUser1.ImageUrl = "data:Image/png;base64," + strbase64;
                    }
                }
                dr.Close();

                cmd = new MySqlCommand("SELECT password FROM administrator WHERE admin_id = '" + userID + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lblPass.Text = dr["password"].ToString();
                }
            }
            catch (Exception)
            {
                alertPanel.Visible = true;
                alertTitle.Text = Utils.errorTitle;
                alertMessage.Text = Utils.errorMessage;
            }

        }

        //MYSQL CONNCETION
        public void connection()
        {
            sqlcon = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            con = new MySqlConnection(sqlcon);
            con.Open();
        }

        //UPDATE PICTURE METHOD
        private void updatePicture()
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    string filePath = fileUpload.PostedFile.FileName;
                    string filename1 = Path.GetFileName(filePath);
                    string ext = Path.GetExtension(filename1);

                    int length = fileUpload.PostedFile.ContentLength;
                    byte[] imgbyte = new byte[length];
                    HttpPostedFile img = fileUpload.PostedFile;
                    img.InputStream.Read(imgbyte, 0, length);

                    connection();
                    cmd = new MySqlCommand("UPDATE administrator SET picture = @picture WHERE user_id = '" + userID + "'", con);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@picture", imgbyte);

                    cmd.ExecuteNonQuery();

                    alertPanel.Visible = true;
                    alertTitle.Text = "UPLOAD SUCCESSFUL";
                    alertMessage.Text = "Profile Picture Changed.";

                    Response.Redirect("~/manager/default.aspx");

                }
                else
                {
                    alertPanel.Visible = true;
                    alertTitle.Text = "UPLOAD FAILED";
                    alertMessage.Text = "Please Select a File.";
                }
            }
            catch (Exception)
            {
                alertPanel.Visible = true;
                alertTitle.Text = Utils.errorTitle;
                alertMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }

        }

        //CHECK PRODUCT LEVEL
        private void checkProductLevel()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM product_view WHERE quantity_available <= min_stock_level", con);
                cmd.Connection = con;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    notBadge.Visible = true;
                    divNotID.InnerHtml = "New Notifications";
                    divNotification.Visible = true;
                }
            }
            catch (Exception)
            {
                alertPanel.Visible = true;
                alertTitle.Text = Utils.errorTitle;
                alertMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }
        }

        //ENCRYPT QUERY STRING FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }

        //USERLOG
        private void userLog()
        {
            try
            {
                connection();

                DateTime logout = DateTime.UtcNow;

                cmd = new MySqlCommand("SELECT user_log_id FROM user_log ORDER BY user_log_id DESC LIMIT 1", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    userLogID = dr["user_log_id"].ToString();
                }
                dr.Close();

                cmd = new MySqlCommand("UPDATE user_log SET logout_time = @logout_time WHERE user_id = '" + userID +
                    "' AND user_log_id = '" + userLogID + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@logout_time", Convert.ToDateTime(logout));

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                alertPanel.Visible = true;
                alertTitle.Text = "LOGIN FAILED";
                alertMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }

        }

        protected void btnSavePicture_ServerClick(object sender, EventArgs e)
        {
            updatePicture();
        }

        protected void clsAlert_ServerClick(object sender, EventArgs e)
        {
            alertPanel.Visible = false;
        }

        protected void btnSignOut_ServerClick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            //userLog();


            Response.Redirect("~/admin/default.aspx");
        }

        protected void changePass_ServerClick(object sender, EventArgs e)
        {
            string strURLData = EncryptQueryString(string.Format("ID={0}", lblPass.Text));

            Response.Redirect("~/admin/change-password.aspx?" + strURLData);
        }

        protected void editProfile_ServerClick(object sender, EventArgs e)
        {
            string strURLData = EncryptQueryString(userID);

           // Response.Redirect("~/edit-profile.aspx?" + strURLData);
        }

    }
}