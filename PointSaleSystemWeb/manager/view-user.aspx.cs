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
    public partial class view_user : System.Web.UI.Page
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
            }
            loadTable();
        }
        //SET SESSION
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
        
        //ENCRYPT QUERY STRING FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }
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

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET User ID
                lblUserID.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[1].Split('='); //GET Full Name
                lblUserName.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[2].Split('='); //GET Status
                lblStatus.Text = arrIndMsg[1].ToString().Trim();

                showModal(lblStatus.Text);
            }
        }

        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM user_view", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    //POPULATE TABLE WITH DATA FROM DB
                    if (dr["Picture"] == DBNull.Value)
                    {
                        tableData += "<tr>";
                        tableData += "  <td class=" + "'center'" + ">";
                        tableData += "	    <div class=" + "'image text-center'" + ">";
                        tableData += "		    <img height=" + "'45px'" + " width=" + "'45px'" + " class=" + "'img-circle'" + " runat=" + "server" + " src=" + "'../uploads/no-image.jpg'" + "/>";
                        tableData += "	    </div>";
                        tableData += "	</td>";
                    }
                    else
                    {
                        byte[] img = (byte[])dr["Picture"];
                        string strbase64 = Convert.ToBase64String(img, 0, img.Length);

                        tableData += "<tr>";
                        tableData += "  <td class=" + "'text-center'" + ">";
                        tableData += "	    <div class=" + "'image text-center'" + ">";
                        tableData += "		    <img height=" + "'45px'" + " width=" + "'45px'" + " class=" + "'img-circle'" + " runat=" + "server" + " src=" + "data:Img/png;base64," + strbase64 + " " + "/>";
                        tableData += "	    </div>";
                        tableData += "	</td>";
                    }
                    string strURLData = EncryptQueryString(dr["user_id"].ToString());
                    string delURLData = EncryptQueryString(string.Format("ID={0}&Name={1}&Status={2}", dr["user_id"].ToString(), dr["full_name"], dr["Status"].ToString()));
                    
                    tableData += "      <td class=" + "'text-center'" + ">" + dr["full_name"].ToString() + "</td>";
                    tableData += "      <td class=" + "'text-center'" + ">" + dr["phone_number"].ToString() + "</td>";
                    tableData += "      <td class=" + "'text-center'" + ">" + dr["role"].ToString() + "</td>";
                    tableData += "      <td class=" + "'text-center'" + ">" + dr["username"].ToString() + "</td>";
                                   
                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=edit-user.aspx?" + strURLData + " title=" + "'Edit'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "          </a>";

                    if (dr["Status"].ToString() == "1")
                    {

                        tableData += "          <a class=" + "'btn btn-danger'" + " href=view-user.aspx?" + delURLData + " title=" + "'Deactivate'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-close'" + "></i>";
                        tableData += "          </a>";
                    }
                    else
                    {
                        tableData += "          <a class=" + "'btn btn-success'" + " href=view-user.aspx?" + delURLData + " title=" + "'Activate'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-check'" + "></i>";
                        tableData += "          </a>";
                    }
                    tableData += "      </td>";

                    if (dr["Status"].ToString() == "1")
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-success'" + ">ACTIVE</span>";
                        tableData += "      </td>";
                    }
                    else
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-danger'" + ">INACTIVE</span>";
                        tableData += "      </td>";
                    }
                    
                    tableData += "</tr>";
                    tblUser.InnerHtml = tableData;
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

        //SHOW MODAL METHOD
        private void showModal(string orderStatus)
        {
            if (orderStatus == "0")
            {
                divModal.Attributes["class"] = "modal-header bg-green";
                btnModYes.Attributes["class"] = "btn btn-success";
                lblModTitle.Text = "ACTIVATE USER";
                lblModMessage.Text = "Are you sure you want to Activate " + lblUserName.Text + " ?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
                lblStatus.Text = "1";
            }
            else if (orderStatus == "1")
            {
                divModal.Attributes["class"] = "modal-header bg-danger";
                btnModYes.Attributes["class"] = "btn btn-danger";
                lblModTitle.Text = "DEACTIVATE USER";
                lblModMessage.Text = "Are you sure you want to Deactivate " + lblUserName.Text + " ?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
                lblStatus.Text = "0";
            }

        }

        //UPDATE USER STATUS
        private void updateUser(string status)
        {
            int userStatus = Convert.ToInt32(status);
            try
            {
                connection();

                cmd = new MySqlCommand("UPDATE user SET status = @status WHERE user_id = '" + lblUserID.Text + "'" , con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@status", userStatus);

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

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/view-user.aspx");
        }

        protected void btnModYes_ServerClick(object sender, EventArgs e)
        {
            updateUser(lblStatus.Text);
            Response.Redirect("~/manager/view-user.aspx");
        }


    }
}