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

namespace PointSaleSystemWeb.sales
{
    public partial class edit_order_item : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID;
        int minStockLevel;

        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                getQueryString();
                getOrderItem();
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
                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET Item ID
                lblItemID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ITEM ID TO LABEL
                arrIndMsg = arrMsgs[1].Split('='); //GET Order ID
                lblOrderID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER ID LABEL TO LABEL
                arrIndMsg = arrMsgs[2].Split('='); //GET Order Number
                lblOrderNumber.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER NUMBER TO LABEL 
                arrIndMsg = arrMsgs[3].Split('='); //GET Product ID
                lblProductID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN PRODUCT ID TO LABEL
                arrIndMsg = arrMsgs[4].Split('='); //GET Order Quantity
                txtOrderQty.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN QUANTITY SOLD TO TEXTBOX

                lblOldQty.Text = txtOrderQty.Text; //ASSIGN OLD QUANTITY SOLD TO LABEL

            }
            else
            {
                Response.Redirect("~/sales/add-order.aspx");
            }
        }

        //READ ORDER ITEM DETAILS FROM DB
        private void getOrderItem()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT product_name, quantity_available, selling_price, min_stock_level FROM product WHERE product_id = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txtProductName.Text = dr["product_name"].ToString();
                lblQtyAvailable.Text = dr["quantity_available"].ToString();
                txtUnitPrice.Text = dr["selling_price"].ToString();
                minStockLevel = Convert.ToInt32(dr["min_stock_level"].ToString());
            }
            con.Close();

            double unitPrice = Convert.ToDouble(txtUnitPrice.Text);
            double orderQuantity = Convert.ToDouble(txtOrderQty.Text);

            double cost = unitPrice * orderQuantity;
            txtCost.Text = cost.ToString("#,0.00");
        }

        //CALCULATE COST OF PRODUCT
        private void calculateCost()
        {
            double unitPrice = Convert.ToDouble(txtUnitPrice.Text);
            double orderQuantity = Convert.ToDouble(txtOrderQty.Text);

            double cost = unitPrice * orderQuantity;
            txtCost.Text = cost.ToString("#,0.00");
        }

        //SAVE METHOD
        private void saveOrder()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("UPDATE list_item SET quantity_sold = @quantity_sold, price = @price, modified_date = @modified_date WHERE list_item_id = '" + lblItemID.Text + "'", con);
            cmd.Connection = con;

            cmd.Parameters.AddWithValue("@quantity_sold", txtOrderQty.Text);
            cmd.Parameters.AddWithValue("@price", txtCost.Text);
            cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

            cmd.ExecuteNonQuery();

            con.Close();

            //UPDATE PRODUCT QUANTITY
            updateProduct();

            alertSuccessPanel.Visible = true;
            alertSuccessTitle.Text = "SUCCESS";
            alertSuccessMessage.Text = txtProductName.Text + " Updated Succesfully.";
        }

        //UPDATE PRODUCT
        private void updateProduct()
        {
            int diffQty;
            int oldQty = Convert.ToInt32(lblOldQty.Text); //CONVERT OLD QUANTITY TO INT
            int newQty = Convert.ToInt32(txtOrderQty.Text); //CONVERT NEW QUANTITY TO INT
            int qtyAvailable = Convert.ToInt32(lblQtyAvailable.Text); //CONVERT PRODUCT QUANTITY AVAILABLE TO INT

            //CHECK  DIFFERENCES BETWEEN QUANTITY
            if (oldQty < newQty)
            {
                diffQty = newQty - oldQty; //SUBTRACT OLD QUANTITY FROM NEW QUANTITY
                qtyAvailable = qtyAvailable - diffQty; //SUBTRACT DIFF(NEW & OLD QUANTITY) FROM QUANTITY AVAILABLE

                connection();
                con.Open();

                cmd = new MySqlCommand("UPDATE product SET quantity_available = @quantity_available, modified_date = @modified_date WHERE product_id = '" + lblProductID.Text + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@quantity_available", qtyAvailable);
                cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

                cmd.ExecuteNonQuery();

                con.Close();

            }
            else if (oldQty > newQty)
            {
                diffQty = oldQty - newQty; //SUBTRACT NEW QUANTITY FROM OLD QUANTITY
                qtyAvailable = qtyAvailable + diffQty; //ADD DIFF(OLD & NEW QUANTITY) TO QUANTITY AVAILABLE

                connection();
                con.Open();

                cmd = new MySqlCommand("UPDATE product SET quantity_available = @quantity_available, modified_date = @modified_date WHERE product_id = '" + lblProductID.Text + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@quantity_available", qtyAvailable);
                cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            //ENCRYPT DATA IN URL
            string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}", lblOrderID.Text, lblOrderNumber.Text));

            Response.Redirect("~/sales/add-order.aspx?" + strURLData);
        }
        protected void txtOrderQty_TextChanged(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                calculateCost();
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                saveOrder();
            }
        }

        protected void valOrderQty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int orderQuantity = Convert.ToInt32(txtOrderQty.Text); //CONVERT ORDER QUANTITY TO INT
            int qtyAvailable = Convert.ToInt32(lblQtyAvailable.Text); //CONVERT PRODUCT QUANTITY AVAILABLE TO INT

            if (orderQuantity > qtyAvailable)
            {
                args.IsValid = false;
                valOrderQty.ErrorMessage = "Cannot order more than " + qtyAvailable;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}