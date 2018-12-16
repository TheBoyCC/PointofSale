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

namespace PointSaleSystemWeb
{
    public partial class change_password : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, userLogID, hashOldPass, hashNewPass, username;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            user();

            if (!IsPostBack)
            {
                getQueryString();
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

        //HASH FUNCTION
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
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

                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET Old Password
                lblOldPass.Text = arrIndMsg[1].ToString().Trim();
            }
        }

        //READ USER DETAILS FROM DB
        private void user()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM user_view WHERE user_id = '" + userID + "'", con);
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
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }

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
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }
            finally
            {
                con.Close();
            }

        }

        //UPDATE PASSWORD METHOD
        private void updatePassword(string oldPassword)
        {
            try
            {
                hashOldPass = GetHashString(oldPassword);

                if (lblOldPass.Text != hashOldPass)
                {
                    alertErrorPanel.Visible = true;
                    alertErrorTitle.Text = "CHANGE PASSWORD FAILED";
                    alertErrorMessage.Text = "Old Password is Incorrect.";
                }
                else
                {
                    hashNewPass = GetHashString(txtRepeatPassword.Text);

                    if (hashNewPass == lblOldPass.Text)
                    {
                        alertErrorPanel.Visible = true;
                        alertErrorTitle.Text = "CHANGE PASSWORD FAILED";
                        alertErrorMessage.Text = "New Password shouldn't be the same as Old Password.";
                    }
                    else
                    {
                        connection();

                        cmd = new MySqlCommand("UPDATE account SET password = @password WHERE user_id = '" + userID + "'", con);
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@password", hashNewPass);

                        cmd.ExecuteNonQuery();

                        alertSuccessPanel.Visible = true;
                        alertSuccessTitle.Text = "CHANGE PASSWORD SUCCESSFULL";
                        alertSuccessMessage.Text = "Password Changed Succesfully.";
                    }
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

        protected void btnSignOut_ServerClick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            userLog();


            Response.Redirect("~/default.aspx");
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                updatePassword(txtOldPassword.Text);
            }
        }

        protected void CustomValidatorNewPass_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int passwordLenght = txtNewPassword.Text.Length;

            if (passwordLenght > 6)
            {
                
                if (txtNewPassword.Text.Any(char.IsUpper))
                {
                    if (txtNewPassword.Text.Any(char.IsNumber))
                    {
                        args.IsValid = true;  
                    }
                    else
                    {
                        args.IsValid = false;

                        CustomValidatorNewPass.ErrorMessage = "Password must contain at least 1 number";
                    }
                      
                }
                else
                {
                    args.IsValid = false;

                    CustomValidatorNewPass.ErrorMessage = "Password must contain at least 1 uppercase letter";
                }
            }
            else
            {
                args.IsValid = false;

                CustomValidatorNewPass.ErrorMessage = "Password must be more than 6 characters";
            }
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            userLog();
            Response.Redirect("~/default.aspx");
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~" + lblUrlString.Text);
        }

    }
}