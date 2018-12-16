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
    public partial class settings : System.Web.UI.Page
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
                loadTable();
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

        //ENCRYPT QUERY STRING FUNCTION
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
            lblUrlString.Text = strReq;

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                lblSettingID.Text = DecryptQueryString(strReq);

                readSetting(lblSettingID.Text);
            }
                               
        }

        //READ SETTING
        private void readSetting(string settingID)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT shop_name, contact_number, address FROM setting WHERE setting_id = '" + settingID + "'", con);
                cmd.Connection = con;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    txtShopName.ReadOnly = false;
                    txtPhone.ReadOnly = false;
                    txtAddress.ReadOnly =false;

                    divBtn.Visible = true;
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
                while (dr.Read())
                {
                    txtShopName.Text = dr["shop_name"].ToString();
                    txtPhone.Text = dr["contact_number"].ToString();
                    txtAddress.Text = dr["address"].ToString();
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

        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM setting", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                if (dr.HasRows)
                {
                    txtShopName.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                    txtAddress.ReadOnly = true;

                    divBtn.Visible = false;
                }
                else
                {
                    divBtn.Visible = true;
                    btnUpdate.Visible = false;
                }
                while (dr.Read())
                {
                    //POPULATE TABLE WITH DATA FROM DB
                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["shop_name"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["contact_number"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["address"].ToString() + "</td>";

                    string strURLData = EncryptQueryString(dr["setting_id"].ToString()); //ENCRYPT DATA IN URL

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=settings.aspx?" + strURLData + " title=" + "'Edit'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "              Edit";
                    tableData += "          </a>";
                    tableData += "      </td>";

                    tableData += "</tr>";
                    tblSettings.InnerHtml = tableData;
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

        //UPDATE SETTING
        private void updateSetting(string settingID)
        {
            try
            {
                connection();
                cmd = new MySqlCommand("UPDATE setting SET shop_name = @shop_name, contact_number = @contact_number, address = @address WHERE setting_id = '" + settingID + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@shop_name", txtShopName.Text);
                cmd.Parameters.AddWithValue("@contact_number", txtPhone.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);

                cmd.ExecuteNonQuery();

                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = " Settings Updated Succesfully.";

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

                cmd = new MySqlCommand("INSERT INTO setting (shop_name, contact_number, address) VALUES (@shop_name, @contact_number, @address)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@shop_name", txtShopName.Text);
                cmd.Parameters.AddWithValue("@contact_number", txtPhone.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);

                cmd.ExecuteNonQuery();

                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = " Settings Created Succesfully.";
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
            Response.Redirect("~/manager/settings.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                updateSetting(lblSettingID.Text);
            }
        }

    }
}