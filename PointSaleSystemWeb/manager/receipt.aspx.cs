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
    public partial class receipt : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID, order_number;

        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            getQueryString();

            readSettings();

            loadCart(lblOrderID.Text);
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

            if (strReq.Contains("?"))
            {
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET Order ID
                lblOrderID.Text = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER ID TO LABEL
                arrIndMsg = arrMsgs[1].Split('='); //GET Order Number
                order_number = arrIndMsg[1].ToString().Trim(); //ASSIGN ORDER NUMBER TO LABEL
            }
            else
            {
                Response.Redirect("~/manager/add-order.aspx");
            }
        }

        //READ SETTINGS
        private void readSettings()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM setting");
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lblShopName.Text = dr["shop_name"].ToString();
                    lblAddress.Text = dr["address"].ToString();
                    lblPhone.Text = dr["contact_number"].ToString();
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

        //LOAD CART METHOD
        private void loadCart(string orderID)
        {
            ListBox totalPrice = new ListBox();
            string item, query;
            double sum = 0;
            query = "SELECT a.order_date, a.order_total, a.customer_name, a.user_name, b.product_name, b.quantity_sold, b.price " +
                           " FROM order_view a, list_item_view b" +
                           " WHERE a.order_id = '" + orderID + "' AND b.order_number = '" + order_number + "'";

            try
            {
                connection();

                cmd = new MySqlCommand(query, con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                 string tableData = "";
                 int i = 0;

                 while (dr.Read())
                 {
                     lblDate.Text = Convert.ToDateTime(dr["order_date"]).ToShortDateString();
                     lblOrderNumber.Text = "ORDER NO.: " + dr["order_number"].ToString();
                     lblCustomer.Text = "CUSTOMER: " + dr["customer_name"].ToString();
                     lblUser.Text = "USER: " + dr["user_name"].ToString();
                     totalPrice.Items.Add(dr["price"].ToString());

                     //POPULATE TABLE WITH DATA FROM DB
                     tableData += "<tr>";
                     tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                     tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_sold"].ToString() + "</td>";
                     tableData += "<td class=" + "'text-center'" + ">" + dr["price"].ToString() + "</td>";

                     tableData += "</tr>";

                     tblOrderItems.InnerHtml = tableData;
                     i += 1;
                 }
                
                //LOOP THROUGH LISTBOX ITEMS AND CALCULATE THE TOTAL COST OF ORDER
                for (int j = 0; j < totalPrice.Items.Count; j++)
                {
                    item = totalPrice.Items[j].ToString();
                    sum += Convert.ToDouble(item);
                }

                lblTotal.Text = "TOTAL: GHȻ " + sum.ToString("#,0.00");

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
            Response.Redirect("~/manager/add-order.aspx");
        }     

    }
}