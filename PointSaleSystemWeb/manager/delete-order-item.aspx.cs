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
    public partial class delete_order_item : System.Web.UI.Page
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
                getOrderItem();
                loadCart();
            }
            showModal();
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
                lblOldQty.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN QUANTITY SOLD TO LABEL


            }
            else
            {
                Response.Redirect("~/manager/add-order.aspx");
            }
        }

        //READ ORDER ITEM DETAILS FROM DB
        private void getOrderItem()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT product_name FROM product WHERE product_id = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lblProductName.Text = dr["product_name"].ToString();
            }
            con.Close();
        }
     
        //LOAD CART METHOD
        private void loadCart()
        {
            ListBox totalPrice = new ListBox();
            string item;
            double sum = 0;

            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT list_item_id, product_id, product_name, quantity_sold, price FROM order_view WHERE order_id = 2 AND order_number = 8980826585 ", con);
            //cmd = new MySqlCommand("SELECT product_id, product_name, quantity_sold, price FROM order_view WHERE order_id = '" + orderID + "' AND order_number = '" + orderNumber + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            //CHECK DATA READER FOR ROWS
            if (dr.HasRows)
            {
                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    totalPrice.Items.Add(dr["price"].ToString());

                    //POPULATE TABLE WITH DATA FROM DB
                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_sold"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["price"].ToString() + "</td>";

                    //ENCRYPT DATA IN URL
                    string strURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductID={3}&OrderQuantity={4}", dr["list_item_id"], "2", "8980826585",
                       dr["product_id"].ToString(), dr["quantity_sold"].ToString()));
                    string delURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductID={3}&OrderQuantity={4}", dr["list_item_id"], "2", "8980826585",
                       dr["product_id"].ToString(), dr["quantity_sold"].ToString()));
                    //string strURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductID={3}&OrderQuantity={4}", dr["list_item_id"], lblOrderID.Text, lblSubOrderNum.Text,
                    //    dr["product_id"].ToString(), dr["quantity_sold"].ToString()));
                    //string delURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductID={3}&OrderQuantity={4}", dr["list_item_id"], lblOrderID.Text, lblSubOrderNum.Text,
                    //    dr["product_id"].ToString(), dr["product_name"].ToString()));

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=edit-order-item.aspx?" + strURLData + " title=" + "'Edit Order Item'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "          </a>";
                    tableData += "          <a class=" + "'btn btn-danger'" + " href=delete-order-item.aspx?" + delURLData + " title=" + "'Delete Order Item'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-trash-o'" + "></i>";
                    tableData += "          </a>";
                    tableData += "      </td>";

                    tableData += "</tr>";
                    tblOrderItems.InnerHtml = tableData;
                    i += 1;
                }
                con.Close();
            }
            //LOOP THROUGH LISTBOX ITEMS AND CALCULATE THE TOTAL COST OF ORDER
            for (int j = 0; j < totalPrice.Items.Count; j++)
            {
                item = totalPrice.Items[j].ToString();
                sum += Convert.ToDouble(item);

                lblTotalCost.Text = "TOTAL: GHȻ " + sum.ToString("#,0.00");
            }

            //CHECK IF TOTAL COST LABEL CONTAINS TEXT AND SHOW DIV
            if (lblTotalCost.Text != String.Empty)
            {
                divCheckOut.Visible = true;
            }
        }

        //SHOW MODAL METHOD
        private void showModal()
        {
            divModal.Attributes["class"] = "modal-header bg-danger";
            btnModYes.Attributes["class"] = "btn btn-danger";
            lblModTitle.Text = "DELETE PRODUCT";
            lblModMessage.Text = "Are you sure you want to Delete " + lblProductName.Text + " from Cart?";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
        }

        //DELETE PRODUCT
        private void deleteProduct()
        {
            connection();
            con.Open();
            cmd = new MySqlCommand("DELETE FROM list_item WHERE list_item_id = '" + lblItemID.Text + "'", con);
            cmd.Connection = con;
            
            cmd.ExecuteNonQuery();

            con.Close();
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            //ENCRYPT DATA IN URL
            string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}", lblOrderID.Text, lblOrderNumber.Text));

            Response.Redirect("~/manager/add-order.aspx?" + strURLData);
        }


        protected void btnModYes_ServerClick(object sender, EventArgs e)
        {
            deleteProduct();
            //ENCRYPT DATA IN URL
            string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}", lblOrderID.Text, lblOrderNumber.Text));

            Response.Redirect("~/manager/add-order.aspx?" + strURLData);
        }

        protected void btnModClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/add-order.aspx");
        }
        
    }
}