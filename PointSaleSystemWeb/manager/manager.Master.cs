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
    public partial class manager : System.Web.UI.MasterPage
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, roleID, username, userLogID, duration;

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
                roleID = Session["RoleID"].ToString();
            }
            else
            {
                Response.Redirect("~/default.aspx");
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
                    lblRole.Text = dr["role"].ToString();

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

                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        alertPanel.Visible = true;
                        alertTitle.Text = "UPLOAD FAILED";
                        alertMessage.Text = "Please Select a Picture.";
                    }
                    else
                    {
                        int length = fileUpload.PostedFile.ContentLength;
                        byte[] imgbyte = new byte[length];
                        HttpPostedFile img = fileUpload.PostedFile;
                        img.InputStream.Read(imgbyte, 0, length);

                        connection();
                        cmd = new MySqlCommand("UPDATE user SET picture = @picture, modified_date = @modified_date WHERE user_id = '" + userID + "'", con);
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@picture", imgbyte);
                        cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

                        cmd.ExecuteNonQuery();

                        alertPanel.Visible = true;
                        alertTitle.Text = "UPLOAD SUCCESSFUL";
                        alertMessage.Text = "Profile Picture Changed.";

                        Response.Redirect("~/manager/default.aspx");
                    }
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
            ListBox notifications = new ListBox();
            ListBox notifID = new ListBox();
            ListBox notifTime = new ListBox();
            string productName, product_id;
            DateTime modified_date;
            DateTime today = DateTime.Now;

            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM product_view WHERE quantity_available <= min_stock_level", con);
                cmd.Connection = con;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        notifications.Items.Add(dr["product_name"].ToString());
                        notifID.Items.Add(dr["product_id"].ToString());
                        notifTime.Items.Add(dr["modified_date"].ToString());

                    }
                }

                #region NOTIFICATION
                //COUNT NUMBER OF NOTIFICATIONS
                int countNotif = notifications.Items.Count;

                //SET INNER HTML OF SPAN TO SHOW NUMBER NOTIFICATIONS
                string spanData = "";
                spanData += countNotif.ToString();
                notBadge.InnerHtml = spanData;
                notBadge.Visible = true;

                divNotID.InnerHtml = "New Notifications";

                //LOOP THROUGH LISTBOX ITEMS AND DISPLAY PRODUCTS
                string divData = "";

                for (int i = 0; i < notifications.Items.Count; i++)
                {
                    //ASSIGN NAME OF LISTBOX ITEM
                    productName = notifications.Items[i].ToString();
                    product_id = notifID.Items[i].ToString();

                    #region CALCULATE NOTIFICATION TIME
                    string diffDay, diffHour, diffMin, diffSec;
                    modified_date = Convert.ToDateTime(notifTime.Items[i].ToString());

                    TimeSpan dateDiff = today - modified_date;
                    diffDay = dateDiff.Days.ToString();
                    diffHour = dateDiff.Hours.ToString();
                    diffMin = dateDiff.Minutes.ToString();
                    diffSec = dateDiff.Seconds.ToString();

                    if (diffDay != "0")
                    {
                        int numDay = Convert.ToInt32(diffDay);
                        switch (numDay)
                        {
                            case 1:
                                duration = numDay.ToString() + " day";
                                break;
                            default:
                                duration = numDay.ToString() + " days";
                                break;
                        }
                    }
                    else if (diffHour != "0")
                    {
                        int numHour = Convert.ToInt32(diffHour);
                        switch (numHour)
                        {
                            case 1:
                                duration = numHour.ToString() + " hour";
                                break;
                            case 24:
                                duration = "1 day";
                                break;
                            default:
                                duration = numHour.ToString() + " hours";
                                break;
                        }
                    }
                    else if (diffMin != "0")
                    {
                        int numMin = Convert.ToInt32(diffMin);
                        switch (numMin)
                        {
                            case 1:
                                duration = numMin.ToString() + " minute";
                                break;
                            case 60:
                                duration = "1 hour";
                                break;
                            default:
                                duration = numMin.ToString() + " minutes";
                                break;
                        }
                    }
                    else if (diffSec != "0")
                    {
                        int numSec = Convert.ToInt32(diffSec);
                        switch (numSec)
                        {
                            case 1:
                                duration = numSec.ToString() + " second";
                                break;
                            case 60:
                                duration = "1 minute";
                                break;
                            default:
                                duration = numSec.ToString() + " seconds";
                                break;
                        }
                    }
                    #endregion

                    //ENCRYPT LISTBOX ITEM IN URL
                    string strUrl = EncryptQueryString(product_id);

                    divData += "    <li>";
                    divData += "        <span class=" + "'bg-danger icon-notification glyph-icon icon-database'" + "></span>";
                    divData += "        <span class=" + "'notification-text font-red'" + ">";
                    divData += "            <a class=" + "font-red" + " href =" + "manage-product.aspx?" + strUrl + ">" + "<b>" + productName + "</b>" + " Stock Level Low</a>";
                    divData += "        </span>";
                    divData += "        <div class=" + "'notification-time'" + ">";
                    divData += "            <b>" + duration + "</b>" + " ago";
                    divData += "            <span class=" + "'glyph-icon icon-clock-o'" + "></span>";
                    divData += "        </div>";
                    divData += "   </li>";

                    ulNotification.InnerHtml = divData;
                }

                #endregion

                divNotification.Visible = true;
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
            userLog();

            Response.Redirect("~/default.aspx");
        }

        protected void changePass_ServerClick(object sender, EventArgs e)
        {

            Response.Redirect("~/change-password.aspx");
        }

        protected void editProfile_ServerClick(object sender, EventArgs e)
        {
            string strURLData = EncryptQueryString(userID);

            // Response.Redirect("~/edit-profile.aspx?" + strURLData);
        }

    }
}