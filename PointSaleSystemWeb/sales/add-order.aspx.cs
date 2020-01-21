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
    public partial class add_order : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID;

        private static int minStockLevel, quantityAvailable;
        private static decimal unitPrice;
        private static string delOrderURL;

        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                getQueryString();

                readProducts();
            }
            
            loadCart(lblOrderID.Text, lblSubOrderNum.Text);
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
            string strReq, orderNumber;
            strReq = Request.RawUrl;

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET Order ID
                lblOrderID.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[1].Split('='); //GET Order Number
                orderNumber = arrIndMsg[1].ToString().Trim();

                //ASSIGN ORDER NUMBER TO LABEL AND SHOW ORDER STATUS DIV
                lblOrderNumber.Text = "ORDER NO: " + orderNumber;

                //SUBSTRING ORDER NUMBER LABEL, GET NUMBER PART AND ASSIGN TO NEW LABEL
                lblSubOrderNum.Text = lblOrderNumber.Text.Substring(10, 10);

                //READ CURRENT CUSTOMER
                readCustomer(lblOrderID.Text);

                //SET CSS CLASS AND INNER HTML OF ORDER STATUS LABEL
                string spanData = "";

                spanData += "<span class=" + "'bs-label label-yellow float-right tooltip-button'" + " data-placement=" + "top" + " title=" + "'Order Status'" + ">";
                spanData += "PENDING";
                spanData += "<i class=" + "'glyph-icon icon-database pad5L'" + "></i>";
                spanData += "</span>";

                divOrderStatus.InnerHtml = spanData;

                divOrder.Visible = true;
            }
        }
   
        #region ADD ORDER DETAILS
        //READ PRODUCTS FROM DB
        private void readProducts()
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

        //READ DETAILS OF SELECTED PRODUCT FROM DB
        private void readProductDetail()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT quantity_available, selling_price, min_stock_level FROM product WHERE product_id = '" + ddlProduct.SelectedItem.Value + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    quantityAvailable = Convert.ToInt32(dr["quantity_available"].ToString());
                    unitPrice = Convert.ToDecimal(dr["selling_price"].ToString());
                    minStockLevel = Convert.ToInt32(dr["min_stock_level"].ToString());
                }
            }
            catch (Exception ex)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }

            lblExists.Visible = false;

            //SET CSS AND INNER HTML FOR PRODUCT DETAIL DIV
            string spanData = "";

            spanData += "<div class=" + "'tile-header font-bold'" + ">";
            spanData += ddlProduct.SelectedItem.Text;
            spanData += "</div>";
            spanData += "<div class=" + "'tile-content-wrapper pad20A'" + ">";
            spanData += "<i class=" + "'glyph-icon icon-database'" + "></i>";
            spanData += "<div class=" + "'tile-content'" + ">";
            spanData += "<span>" + "Ȼ " + "</span>";
            spanData += unitPrice.ToString("#,0.00");
            spanData += "</div>";
            spanData += "<small class=" + "'pad15T font-bold'" + ">";
            spanData += quantityAvailable + " Available in Stock";
            spanData += "</small>";
            spanData += "</div>";

            divProduct.InnerHtml = spanData;

            //SHOW ORDER STATUS DIV
            divProduct.Visible = true;


        }

        //CALCULATE COST OF PRODUCT
        private void calculateCost()
        {
            if (quantityAvailable.ToString() != String.Empty)
            {
                double productPrice = Convert.ToDouble(unitPrice);
                double orderQuantity = Convert.ToDouble(txtOrderQty.Text);

                double cost = productPrice * orderQuantity;

                txtCost.Text = cost.ToString("#,0.00");
            }
            else
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = "FAILED";
                alertErrorMessage.Text = "Select a Product.";

            }
        }

        //GENERATE ORDER NUMBER
        private string genOrderNumber(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder(); //CREATE A STRING BUILDER OBJECT
            Random random = new Random(); //CREATE A RANDOM OBJECT
            int ch; //INT VARIABLE ch

            for (int i = 0; i < size; i++)
            {
                //1.RETURNS THE RANDOM NUMBER
                //2.MULTIPLY THE RANDOM NUMBER BY 26 AND ADD 65
                //3.RETURN THE LARGEST INT <= 2.
                //4.CONVERT TO INT AND ASSIGN ch
                ch = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                builder.Append(ch); //ADD THE CREATED INT TO STRING BUILDER OBJECT
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        //CREATE ORDER
        private void generateOrder(string customer)
        {
            //CHECK IF CUSTOMER WAS CREATED
            if (customer != String.Empty)
            {
                try
                {
                    connection();

                    cmd = new MySqlCommand("INSERT INTO `order` (order_number, order_date, customer_id, user_id, modified_date) VALUES (@order_number, @order_date, @customer_id, @user_id, @modified_date)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@order_number", lblSubOrderNum.Text);
                    cmd.Parameters.AddWithValue("@order_date", DateTime.Now.ToShortDateString());
                    cmd.Parameters.AddWithValue("@customer_id", customer);
                    cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(userID));
                    cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand("SELECT order_id FROM `order` ORDER BY order_id DESC LIMIT 1", con);
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lblOrderID.Text = dr["order_id"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    alertErrorPanel.Visible = true;
                    alertErrorTitle.Text = Utils.errorTitle;
                    alertErrorMessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }
            else if (customer == String.Empty)
            {
                try
                {
                    connection();

                    cmd = new MySqlCommand("INSERT INTO `order` (order_number, order_date, user_id) VALUES (@order_number, @order_date, @user_id)", con);
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@order_number", lblSubOrderNum.Text);
                    cmd.Parameters.AddWithValue("@order_date", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(userID));

                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand("SELECT order_id FROM `order` ORDER BY order_id DESC LIMIT 1", con);
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lblOrderID.Text = dr["order_id"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    alertErrorPanel.Visible = true;
                    alertErrorTitle.Text = Utils.errorTitle;
                    alertErrorMessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }

        }

        //SAVE ORDER ITEMS
        private void orderItem(string order)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("INSERT INTO list_item (order_id, order_number, product_id, quantity_sold, price, modified_date) VALUES (@order_id, @order_number, @product_id, @quantity_sold, @price, @modified_date)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@order_id", order);
                cmd.Parameters.AddWithValue("@order_number", lblSubOrderNum.Text);
                cmd.Parameters.AddWithValue("@product_id", ddlProduct.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@quantity_sold", txtOrderQty.Text);
                cmd.Parameters.AddWithValue("@price", txtCost.Text);
                cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

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

        //CREATE CUSTOMER
        private void createCustomer(string name, string phone)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("INSERT INTO customer (customer_name, customer_phone) VALUES (@customer_name, @customer_phone)", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@customer_name", name);
                cmd.Parameters.AddWithValue("@customer_phone", phone);

                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand("SELECT * FROM customer ORDER BY customer_id DESC LIMIT 1", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblCustomerID.Text = dr["customer_id"].ToString();
                    txtCustomerName.Text = dr["customer_name"].ToString();
                    txtCustomerPhone.Text = dr["customer_phone"].ToString();
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

        //CHECK EXISTING CUSTOMER
        private void existingCustomer(string phone)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM customer WHERE customer_phone = '" + phone + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    txtCustomerName.ReadOnly = true;
                    txtCustomerPhone.ReadOnly = true;

                    lblExists.Visible = true;
                    lblExists.Text = "Customer Exists";

                    while (dr.Read())
                    {
                        lblCustomerID.Text = dr["customer_id"].ToString();
                        lblCustomerName.Text += txtCustomerName.Text = dr["customer_name"].ToString();
                        lblCustomerPhone.Text += txtCustomerPhone.Text = dr["customer_phone"].ToString();
                    }
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

        //READ CUSTOMER METHOD
        private void readCustomer(string orderID)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT customer_id FROM `order` WHERE order_id = '" + orderID + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblCustomerID.Text = dr["customer_id"].ToString();
                }
                dr.Close();

                cmd = new MySqlCommand("SELECT * FROM customer WHERE customer_id = '" + lblCustomerID.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblCustomerID.Text = dr["customer_id"].ToString();
                    lblCustomerName.Text += txtCustomerName.Text = dr["customer_name"].ToString();
                    lblCustomerPhone.Text += txtCustomerPhone.Text = dr["customer_phone"].ToString();
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

            txtCustomerName.ReadOnly = true;
            txtCustomerPhone.ReadOnly = true;
        }

        //CHECKOUT METHOD
        private void checkOut(string completed)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("UPDATE `order` SET total = @total, order_status = @order_status, modified_date = @modified_date WHERE order_id = '" + lblOrderID.Text + "' AND order_number = '" + lblSubOrderNum + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@total", lblSum.Text);
                cmd.Parameters.AddWithValue("@order_status", completed);
                cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "CloseModal();", true);

            //SET CSS CLASS AND INNER HTML OF ORDER STATUS DIV
            string spanData = "";

            spanData += "<span class=" + "'bs-label label-success float-right'" + ">";
            spanData += completed;
            spanData += "<i class=" + "'glyph-icon icon-check-circle pad5L'" + "></i>";
            spanData += "</span>";

            divOrderStatus.InnerHtml = spanData;

            alertSuccessPanel.Visible = true;
            alertSuccessTitle.Text = "SUCCESS";
            alertSuccessMessage.Text = "Order " + lblSubOrderNum.Text + " Checked out Succesfully.";

        }

        //CANCEL METHOD
        private void cancel(string cancelled)
        {
            try
            {
                connection();

                cmd = new MySqlCommand("DELETE FROM list_item WHERE order_id = '" + lblOrderID.Text + "'", con);
                cmd.Connection = con;

                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand("UPDATE `order` SET total = @total, order_status = @order_status, modified_date = @modified_date WHERE order_id = '" + lblOrderID.Text + "' AND order_number = '" + lblSubOrderNum.Text + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@total", lblSum.Text);
                cmd.Parameters.AddWithValue("@order_status", cancelled);
                cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "CloseModal();", true);

            //SET CSS CLASS AND INNER HTML OF ORDER STATUS LABEL
            string spanData = "";

            spanData += "<span class=" + "'bs-label label-danger float-right'" + ">";
            spanData += cancelled;
            spanData += "<i class=" + "'glyph-icon icon-times pad5L'" + "></i>";
            spanData += "</span>";

            divOrderStatus.InnerHtml = spanData;


            alertErrorPanel.Visible = true;
            alertErrorTitle.Text = "CANCELLED";
            alertErrorMessage.Text = "Order " + lblSubOrderNum.Text + " Cancelled Succesfully.";
        }

        //ADD TO CART METHOD
        private void addToCart()
        {
            //CHECK IF THERE'S A CUSTOMER
            if (txtCustomerName.Text != lblCustomerName.Text && txtCustomerPhone.Text != lblCustomerPhone.Text)
            {
                createCustomer(txtCustomerName.Text, txtCustomerPhone.Text);

                txtCustomerName.ReadOnly = true;
                txtCustomerPhone.ReadOnly = true;
            }
            else if (txtCustomerName.Text == lblCustomerName.Text && txtCustomerPhone.Text == lblCustomerPhone.Text)
            {
                txtCustomerName.ReadOnly = true;
                txtCustomerPhone.ReadOnly = true;
            }

            //CHECK IF THERE'S AN ORDER NUMBER
            if (lblOrderNumber.Text == String.Empty)
            {
                //PASS 5 & TRUE THROUGH THE genOrderNumber METHOD TO CREATE ORDER NNUMBER AND ASSIGN TO LABEL
                lblOrderNumber.Text = "ORDER NO: " + genOrderNumber(5, true);

                //SUBSTRING ORDER NUMBER LABEL, GET NUMBER PART AND ASSIGN TO NEW LABEL
                lblSubOrderNum.Text = lblOrderNumber.Text.Substring(10, 10);

                //SET CSS CLASS AND INNER HTML OF ORDER STATUS LABEL
                string spanData = "";

                spanData += "<span class=" + "'bs-label label-yellow float-right'" + ">";
                spanData += "PENDING";
                spanData += "<i class=" + "'glyph-icon icon-database pad5L'" + "></i>";
                spanData += "</span>";

                divOrderStatus.InnerHtml = spanData;

                //SHOW ORDER STATUS DIV
                divOrder.Visible = true;

                //CREATE AN ORDER
                generateOrder(lblCustomerID.Text);

                //INSERT ORDER ITEMS
                orderItem(lblOrderID.Text);
            }
            else if (lblOrderNumber.Text != String.Empty)//THERE'S AN EXISTING ORDER NUMBER
            {
                //INSERT ORDER ITEMS
                orderItem(lblOrderID.Text);
            }

            //SHOW ORDER ITEMS IN CART
            loadCart(lblOrderID.Text, lblSubOrderNum.Text);

            //CLEAR TEXTBOX CONTENTS
            clear();
        }

        //LOAD CART METHOD
        private void loadCart(string orderID, string orderNumber)
        {
            ListBox totalPrice = new ListBox();
            string item;
            double sum = 0;

            try
            {
                connection();

                cmd = new MySqlCommand("SELECT list_item_id, product_id, product_name, quantity_sold, price FROM list_item_view WHERE order_id = '" + orderID + "' AND order_number = '" + orderNumber + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                //CHECK DATA READER FOR ROWS

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
                    string strURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductID={3}&OrderQuantity={4}", dr["list_item_id"], lblOrderID.Text, lblSubOrderNum.Text,
                        dr["product_id"].ToString(), dr["quantity_sold"].ToString()));
                    string delURLData = EncryptQueryString(string.Format("itemID={0}&OrderID={1}&OrderNumber={2}&ProductName={3}", dr["list_item_id"], lblOrderID.Text, lblSubOrderNum.Text,
                        dr["product_name"].ToString()));

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=edit-order-item.aspx?" + strURLData + " title=" + "'Edit Order Item'" + " > ";
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

                //COUNT NUMBER OF ITEMS IN CART
                int orderCount = totalPrice.Items.Count;

                //SET CSS CLASS AND INNER HTML OF CART BOX HEADER & ORDER COUNT BADGE
                string spanData = "";
                spanData += "<i class=" + "'glyph-icon icon-shopping-cart'" + "></i>";
                spanData += "Cart";

                if (orderCount != 0)
                {
                    spanData += "<span class=" + "'bs-badge badge-info float-right'" + ">";
                    spanData += orderCount.ToString();
                    spanData += "</span>";
                }

                boxHeader.InnerHtml = spanData;

                //LOOP THROUGH LISTBOX ITEMS AND CALCULATE THE TOTAL COST OF ORDER
                for (int j = 0; j < totalPrice.Items.Count; j++)
                {
                    item = totalPrice.Items[j].ToString();
                    sum += Convert.ToDouble(item);
                }

                lblSum.Text = sum.ToString("#,0.00");

                lblTotalCost.Text = "TOTAL: GHȻ " + sum.ToString("#,0.00");


                //CHECK IF TOTAL COST LABEL CONTAINS TEXT AND SHOW DIV
                if (lblTotalCost.Text != "TOTAL: GHȻ 0.00")
                {
                    divCheckOut.Visible = true;
                }
                else
                {
                    divCheckOut.Visible = false;
                }
            }
            catch (Exception ex)
            {
                alertErrorPanel.Visible = true;
                alertErrorTitle.Text = Utils.errorTitle;
                alertErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        //CLEAR METHOD
        private void clear()
        {
            divProduct.Visible = false;
            ddlProduct.SelectedValue = "0";
            txtOrderQty.Text = String.Empty;
            txtCost.Text = String.Empty;
        }

        //SHOW MODAL METHOD
        private void showModal(string orderStatus)
        {
            lblModal.Text = orderStatus;

            if (lblModal.Text == "CHECKOUT")
            {
                divModal.Attributes["class"] = "modal-header bg-green";
                btnModYes.Attributes["class"] = "btn btn-success";
                lblModTitle.Text = "CONFIRM ORDER";
                lblModMessage.Text = "Checkout Order " + lblSubOrderNum.Text + " " + lblTotalCost.Text;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
            }
            else if (lblModal.Text == "CANCEL")
            {
                divModal.Attributes["class"] = "modal-header bg-danger";
                btnModYes.Attributes["class"] = "btn btn-danger";
                lblModTitle.Text = "CANCEL ORDER";
                lblModMessage.Text = "Are you sure you want to Cancel Order " + lblSubOrderNum.Text + " ?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
            }
            else if (lblModal.Text == "DELETE ORDER ITEM")
            {
                divModal.Attributes["class"] = "modal-header bg-danger";
                btnModYes.Attributes["class"] = "btn btn-danger";
                lblModTitle.Text = "DELETE PRODUCT";
                lblModMessage.Text = "Are you sure you want to Delete " + lblDelProductName.Text + " from Cart?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);
            }
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/sales/add-order.aspx");
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}", lblOrderID.Text, lblSubOrderNum.Text));

            Response.Redirect("~/sales/receipt.aspx?" + strURLData);
        }

        protected void btnAddToCart_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                addToCart();
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            readProductDetail();
        }

        protected void txtOrderQty_TextChanged(object sender, EventArgs e)
        {
            calculateCost();
        }

        protected void valOrderQty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //CONVERT ENTERED QUANTITY TO INT
            int orderQuantity = Convert.ToInt32(txtOrderQty.Text);

            //CHECK IF ENTERED QUANTITY IS <= 0
            if (orderQuantity <= 0)
            {
                args.IsValid = false;
                valOrderQty.ErrorMessage = "Order Quantity must be more than 0";
            }
            //CHECK IF ENTERED QUANTITY IS >< QUANTITY AVAILABLE
            else if (orderQuantity > quantityAvailable)
            {
                args.IsValid = false;
                valOrderQty.ErrorMessage = "Cannot order more than " + quantityAvailable;
            }            
            else
            {
                args.IsValid = true;
            }          

        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            showModal("CANCEL");
        }

        protected void btnCheckOut_ServerClick(object sender, EventArgs e)
        {
            showModal("CHECKOUT");
        }

        protected void btnModYes_ServerClick(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            btnCheckOut.Visible = false;

            if (lblModal.Text == "CHECKOUT")
            {
                checkOut("COMPLETED");
            }
            else if (lblModal.Text == "CANCEL")
            {
                cancel("CANCELLED");
            }
            else if (lblModal.Text == "DELETE ORDER ITEM")
            {
                deleteProduct();
            }
        }

        protected void txtCustomerPhone_TextChanged(object sender, EventArgs e)
        {
            existingCustomer(txtCustomerPhone.Text);
        }
        #endregion

        #region DELETE ORDER ITEM
        //GET URL DATA FOR DEL ORDER ITEM
        private void getDelQueryString()
        {
            string strReq;
            strReq = Request.RawUrl;
            int urlcount = strReq.Count();
            delOrderURL = strReq.Substring(0, 23);

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET Item ID
                lblDelItemID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ITEM ID TO LABEL
                arrIndMsg = arrMsgs[1].Split('='); //GET Order ID
                lblDelOrderID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER ID LABEL TO LABEL
                arrIndMsg = arrMsgs[2].Split('='); //GET Order Number
                lblDelOrderNum.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER NUMBER TO LABEL 
                arrIndMsg = arrMsgs[3].Split('='); //GET Product ID
                lblDelProductName.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN PRODUCT ID TO LABEL

                showModal("DELETE ORDER ITEM");

                //ASSIGN ORDER NUMBER TO LABEL AND SHOW ORDER STATUS DIV
                lblOrderNumber.Text = "ORDER NO: " + lblDelOrderNum.Text;

                //READ CURRENT CUSTOMER
                readCustomer(lblOrderID.Text);

                //SET CSS CLASS AND INNER HTML OF ORDER STATUS LABEL
                string spanData = "";

                spanData += "<span class=" + "'bs-label label-yellow float-right'" + ">";
                spanData += "PENDING";
                spanData += "<i class=" + "'glyph-icon icon-database pad5L'" + "></i>";
                spanData += "</span>";

                divOrderStatus.InnerHtml = spanData;

                divOrder.Visible = true;
            }
        }

        //DELETE PRODUCT
        private void deleteProduct()
        {
            connection();

            cmd = new MySqlCommand("DELETE FROM list_item WHERE list_item_id = '" + lblDelItemID.Text + "'", con);
            cmd.Connection = con;

            cmd.ExecuteNonQuery();

            con.Close();

            string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}", lblDelOrderID.Text, lblDelOrderNum.Text));

            Response.Redirect("~/" + delOrderURL + "?" + strURLData);

        }

        #endregion
    }
}