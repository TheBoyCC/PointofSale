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
    public partial class add_product : System.Web.UI.Page
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
                readCategory();
            }
        }
        //GET AND SET SESSION
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

        //SAVE METHOD
        private void saveDetails()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT product_name FROM product WHERE product_name = '" + txtProductName.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    alertErrorPanel.Visible = true;
                    alertErrorTitle.Text = "FAILED";
                    alertErrorMessage.Text = txtProductName.Text + " Already Exists.";
                }
                else
                {
                    dr.Close();
                    cmd = new MySqlCommand("INSERT INTO product (product_name, category_id, quantity_available, cost_price, selling_price, min_stock_level) VALUES (@product_name, @category_id, @quantity_available,@cost_price, @selling_price, @min_stock_level)", con);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@product_name", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@category_id", ddlCategory.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@quantity_available", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@cost_price", txtCostPrice.Text);
                    cmd.Parameters.AddWithValue("@selling_price", txtSellingPrice.Text);
                    cmd.Parameters.AddWithValue("@min_stock_level", txtMinLevel.Text);

                    cmd.ExecuteNonQuery();

                    alertSuccessPanel.Visible = true;
                    alertSuccessTitle.Text = "SUCCESS";
                    alertSuccessMessage.Text = txtProductName.Text + " Created Succesfully.";
                }
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }         
            
        }

        //READ CATEGORY FROM DB 
        private void readCategory()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM category", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["category"].ToString();
                    item.Value = dr["category_id"].ToString();

                    ddlCategory.Items.Add(item);
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

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/add-product.aspx");
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