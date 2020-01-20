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
    public partial class edit_product : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, categoryID;

        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                getQueryString();
                getProduct();
                readCategory();
            }
        }
        //GETS AND SETS SESSION ID
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
                lblProductID.Text = DecryptQueryString(strReq);
            }
            else
            {
                Response.Redirect("~/manager/manage-product.aspx");
            }
        }

        //READ PRODUCT FROM DB
        private void getProduct()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("SELECT * FROM product WHERE product_id = '" + lblProductID.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txtProductName.Text = dr["product_name"].ToString();
                    categoryID = dr["category_id"].ToString();
                    txtQuantity.Text = dr["quantity_available"].ToString();
                    txtSellingPrice.Text = dr["selling_price"].ToString();
                    txtCostPrice.Text = dr["cost_price"].ToString();
                    txtMinLevel.Text = dr["min_stock_level"].ToString();
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
        private void saveDetails()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("UPDATE product SET selling_price = @selling_price, cost_price = @cost_price, min_stock_level = @min_stock_level, modified_date = @modified_date WHERE product_id = '" + lblProductID.Text + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@selling_price", txtSellingPrice.Text);
                cmd.Parameters.AddWithValue("@cost_price", txtCostPrice.Text);
                cmd.Parameters.AddWithValue("@min_stock_level", txtMinLevel.Text);
                               cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

                cmd.ExecuteNonQuery();
                               
                alertSuccessPanel.Visible = true;
                alertSuccessTitle.Text = "SUCCESS";
                alertSuccessMessage.Text = txtProductName.Text + " Updated Succesfully.";
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

        //READ CATEGORY FROM DB
        private void readCategory()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("SELECT category FROM category WHERE category_id = '" + categoryID + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txtCategory.Text = dr["category"].ToString();
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

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails();
            }
        }
    }
}