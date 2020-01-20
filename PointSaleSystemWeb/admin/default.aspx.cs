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
using PointSaleSystemWeb.manager;

namespace PointSaleSystemWeb.admin
{
    public partial class _default1 : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, username, password, hashPassword, role;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                checkDB();
            }
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

        //ENCRYPT QUERY STRING FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }
        //MYSQL CONNCETION
        public void connection()
        {
            sqlcon = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            con = new MySqlConnection(sqlcon);

            con.Open();
        }

        //LOGIN METHOD
        public void login()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM administrator WHERE username ='" + txtUsername.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        username = Convert.ToString(dr["username"]).ToLower();
                        password = dr["password"].ToString();

                        Session["UserID"] = dr["admin_id"].ToString();
                    }
                    hashPassword = GetHashString(txtPassword.Text);

                    if (hashPassword == password)
                    {


                        Response.Redirect("~/admin/admin-dashboard.aspx");

                    }
                    else
                    {
                        alertPanel.Visible = true;
                        alertTitle.Text = "LOGIN FAILED";
                        alertMessage.Text = "Invalid Username or Password.";
                    }
                }
                else
                {
                    alertPanel.Visible = true;
                    alertTitle.Text = "LOGIN FAILED";
                    alertMessage.Text = "Username " + "'" + txtUsername.Text + "' Does not exist.";
                }
            }
            catch (Exception ex)
            {
                alertPanel.Visible = true;
                alertTitle.Text = "LOGIN FAILED";
                alertMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }

        }

        //USER LOG METHOD
        private void userLog()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("INSERT INTO user_log (user_id, role_id, date, login_time) VALUES (@user_id, @role_id, @date, @login_time)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@user_id", Session["UserID"].ToString());
                cmd.Parameters.AddWithValue("@role_id", role);
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                cmd.Parameters.AddWithValue("@login_time", Convert.ToDateTime(DateTime.UtcNow));

                cmd.ExecuteNonQuery();
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

        //CHECK DB
        private void checkDB()
        {
            try
            {
                connection();

                if (!con.State.Equals("Open"))
                {

                }
                else
                {

                }
            }
            catch (Exception)
            {
                alertPanel.Visible = true;
                alertTitle.Text = Utils.errorTitle;
                alertMessage.Text = "Server is Offline. Make Sure Server is Connected.";
            }
            finally
            {
                con.Close();
            }
        }
        protected void clsAlert_ServerClick(object sender, EventArgs e)
        {
            alertPanel.Visible = false;

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            login();
        }
    }

}