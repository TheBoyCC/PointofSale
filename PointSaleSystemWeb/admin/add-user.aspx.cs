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
using System.Text.RegularExpressions;


namespace PointSaleSystemWeb.admin
{
    public partial class add_user : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, phoneNumber;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                readRole();
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
        //UPLOAD PICTURE
        private void uploadPicture()
        {
            if (fileUpload.HasFile)
            {
                string filePath = Path.GetFileName(fileUpload.PostedFile.FileName);
                filePath = filePath.Replace(" ", "");
                fileUpload.SaveAs(Server.MapPath("~/uploads/") + filePath);
                filePath = Server.MapPath("~/uploads/") + filePath;

                string fileName = Path.GetFileName(filePath);
                string ext = Path.GetExtension(fileName);


                if (ext != ".jpg" && ext != ".jpeg")
                {
                    alertErrorPanel.Visible = true;
                    alertErrorTitle.Text = "UPLOAD FAILED";
                    alertErrorMessage.Text = "Please Select a Picture.";
                }
                else
                {
                    int length = fileUpload.PostedFile.ContentLength;
                    byte[] imgbyte = new byte[length];
                    HttpPostedFile img = fileUpload.PostedFile;
                    img.InputStream.Read(imgbyte, 0, length);

                    string strBase64 = Convert.ToBase64String(imgbyte, 0, imgbyte.Length);
                    imgUser.ImageUrl = "data:Image/png;base64," + strBase64;

                    btnSelect.Visible = false;
                    btnChange.Visible = true;

                    lblFileName.Text = filePath;
                }


            }
            else
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = "UPLOAD FAILED";
                alertErrorMessage.Text = "Please Select a File.";
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

        //ACCOUNT DETAIL METHOD
        private void userAccountDetail()
        {
            if (txtFirstName.Text == String.Empty || txtLastName.Text == String.Empty || txtPhone.Text == String.Empty)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = "FAILED";
                alertErrorMessage.Text = "Enter First Name, Last Name and Phone Number";
            }
            else
            {
                string lName = Regex.Replace(txtLastName.Text, @"[\s-]", String.Empty);
                txtUserName.Text = txtFirstName.Text.Substring(0, 1).ToLower() + lName.ToLower();
                txtPassword.Text = txtPhone.Text;

                lblPassword.Text = GetHashString(txtPhone.Text);

            }
        }
        
        //USER ACCOUNT METHOD
        private void account(string user_id, string username, string password, string role_id)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("INSERT INTO account (user_id, username, password, role_id) VALUES (@user_id, @username, @password, @role_id)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("user_id", user_id);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("role_id", role_id);
              
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
     
        //CHECK EXISTING USER
        private void checkExisting(string phone)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT phone_number FROM  user WHERE phone_number = '" + phone + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    phoneNumber = dr["phone_number"].ToString();
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
        private void saveDetails(string fileName)
        {
            try
            {
                if (imgUser.ImageUrl != String.Empty)
                {
                    FileInfo finfo = new FileInfo(fileName); //INSTANCE OF FILEINFO CLASS

                    byte[] imgbyte = new byte[finfo.Length]; //CREATE A BYTE ARRAY  OBJECT AND PASS FILEINFO OBJECT
                    FileStream fstream = finfo.OpenRead(); //OPEN FILESTREAM TO READ FILEINFO OBJECT
                    fstream.Read(imgbyte, 0, imgbyte.Length); //READ FILE FROM FILEINFO INTO FILESTREAM
                    fstream.Close(); //CLOSR FILESTREAM

                    connection();

                    cmd = new MySqlCommand("INSERT INTO user (first_name, last_name, gender, date_of_birth, role_id, phone_number, address, picture) VALUES (@first_name, @last_name, @gender, @date_of_birth, @role_id, @phone_number, @address, @picture)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("gender", ddlGender.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("date_of_birth", txtDOB.Text);
                    cmd.Parameters.AddWithValue("role_id", ddlRole.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("phone_number", txtPhone.Text);
                    cmd.Parameters.AddWithValue("address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("picture", imgbyte);

                    cmd.ExecuteNonQuery();


                }
                else if (imgUser.ImageUrl == String.Empty)
                {
                    connection();

                    cmd = new MySqlCommand("INSERT INTO user (first_name, last_name, gender, date_of_birth, role_id, phone_number, address) VALUES (@first_name, @last_name, @gender, @date_of_birth, @role_id, @phone_number, @address)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("gender", ddlGender.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("date_of_birth", txtDOB.Text);
                    cmd.Parameters.AddWithValue("role_id", ddlRole.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("phone_number", txtPhone.Text);
                    cmd.Parameters.AddWithValue("address", txtAddress.Text);

                    cmd.ExecuteNonQuery();
                }

                //READ USER ID OF THE JUST CREATED USER
                cmd = new MySqlCommand("SELECT user_id FROM user ORDER BY user_id DESC LIMIT 1", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblUserID.Text = dr["user_id"].ToString();
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
            
            account(lblUserID.Text, txtUserName.Text, lblPassword.Text, ddlRole.SelectedItem.Value);
            
            alertSuccessPanel.Visible = true;
            alertSuccessTitle.Text = "SUCCESS";
            alertSuccessMessage.Text = "User " + txtFirstName.Text + " Created Successfully.";
            
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/add-user.aspx");
        }

        protected void btnSavePicture_ServerClick(object sender, EventArgs e)
        {
            uploadPicture();
        }

        protected void btnSaveuser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails(lblFileName.Text);
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            userAccountDetail();
        }

        protected void valPhone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            checkExisting(txtPhone.Text);

            if (txtPhone.Text == phoneNumber)
            {
                args.IsValid = false;
                valPhone.ErrorMessage = "User with " + txtPhone.Text + " Already Exists";
                                
                txtPhone.Focus();
            }
            else if (txtPhone.Text != phoneNumber)
            {
                args.IsValid = true;
            }
        }
    }
}