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
    public partial class add_inventory : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, categoryID, expire;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();
            
            if (!IsPostBack)
            {
                readProduct();
            }
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

        //SAVE METHOD
        private void saveDetails()
        {
            try
            {
                if (txtExpiryDate.Text != String.Empty)
                {
                    connection();

                    cmd = new MySqlCommand("INSERT INTO product_inventory (inventory_date, product_id, quantity_delivered, batch_number, product_expiry_date, user_id) VALUES (@inventory_date, @product_id, @quantity_delivered, @batch_number, @product_expiry_date, @user_id)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@inventory_date", txtInvDate.Text);
                    cmd.Parameters.AddWithValue("@product_id", ddlProduct.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@quantity_delivered", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@batch_number", txtBatchNumber.Text);
                    cmd.Parameters.AddWithValue("@product_expiry_date", txtExpiryDate.Text);
                    cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(userID));

                    cmd.ExecuteNonQuery();

                    alertSuccessPanel.Visible = true;
                    alertSuccessTitle.Text = "SUCCESS";
                    alertSuccessMessage.Text = "Inventory Created Succesfully.";

                }
                else
                {
                    cmd = new MySqlCommand("INSERT INTO product_inventory (inventory_date, product_id, quantity_delivered, batch_number, user_id) VALUES (@inventory_date, @product_id, @quantity_delivered, @batch_number, @user_id)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@inventory_date", txtInvDate.Text);
                    cmd.Parameters.AddWithValue("@product_id", ddlProduct.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@quantity_delivered", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@batch_number", txtBatchNumber.Text);
                    cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(userID));

                    cmd.ExecuteNonQuery();

                    alertSuccessPanel.Visible = true;
                    alertSuccessTitle.Text = "SUCCESS";
                    alertSuccessMessage.Text = "Inventory Created Succesfully.";
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

        //READ PRODUCTS FROM DB
        private void readProduct()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT product_id, product_name FROM product WHERE status = 1", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["product_name"].ToString();
                    item.Value = dr["product_id"].ToString();

                    ddlProduct.Items.Add(item);
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
        
        //READ EXPIRE STATUS OF PRODUCT CATEGORY
        private void readCategoryExp()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT expire FROM category WHERE category_id= '" + categoryID + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    expire = dr["expire"].ToString();
                }

                if (expire == "1")
                {
                    divExpiry.Visible = true;
                }
                else
                {
                    divExpiry.Visible = false;
                }
            }
            catch (Exception)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = Utils.errorMessage;
            }
            
        }

        //GET QUANTITY OF SELECTED PRODUCT
        private void productQuantity()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT quantity_available, category_id FROM product WHERE product_name = '" + ddlProduct.SelectedItem.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    categoryID = dr["category_id"].ToString();
                    txtQuantity.Text = dr["quantity_available"].ToString();
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
            Response.Redirect("~/manager/add-inventory.aspx");

        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveDetails();
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            productQuantity();
            readCategoryExp();
        }

        protected void valInvDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime maxDate = DateTime.Now;
            DateTime dt;

            args.IsValid = (DateTime.TryParse(args.Value, out dt) && dt <= maxDate);
        }

        protected void valExpiryDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime minDate = DateTime.Now;
            DateTime dt;

            args.IsValid = (DateTime.TryParse(args.Value, out dt) && dt > minDate);
        }

    }
}