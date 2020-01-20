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
    public partial class view_order : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID;
        private static string orderID, orderNumber, orderStatus, orderTotal, orderDate, userName, customerName, selectedFilter, selectedCondition;
        ListBox user = new ListBox();
        ListBox order_total = new ListBox();

        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                getQueryString();
                filterOptions();
                filterOrder("");
            }
            user_name();
            
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
                orderID = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[1].Split('='); //GET Order Number
                orderNumber = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[2].Split('='); //GET Order Status
                orderStatus = arrIndMsg[1].ToString().Trim();
                //READ CURRENT ORDER DETAILS
                orderDetails(orderID, orderNumber, orderStatus);
                //SHOW ORDER DETAILS IN MODAL
                showModal(orderStatus);

                //divOrder.Visible = true;
            }
        }

        //READ ORDER DETAILS FROM DB
        private void orderDetails(string order_id, string order_number, string order_status)
        {
            string query;

            //CHECK ORDER STATUS
            switch (order_status)
            {
                case "CANCELLED":
                    query = "SELECT a.order_date, a.order_total, a.customer_name, a.user_name, b.product_name, b.quantity_sold, b.price " +
                            " FROM order_view a, deleted_list_item_view b" +
                            " WHERE a.order_id = '" + order_id + "' AND b.order_number = '" + order_number + "'";
                    break;
                default:
                    query = "SELECT a.order_date, a.order_total, a.customer_name, a.user_name, b.product_name, b.quantity_sold, b.price " +
                            " FROM order_view a, list_item_view b" +
                            " WHERE a.order_id = '" + order_id + "' AND b.order_number = '" + order_number + "'";
                    break;
            }

            try
            {
                string tableData = "", rowData = "", userData = "", spanData = "";
                ListBox totalPrice = new ListBox();
                string item;
                double sum = 0;
                int i = 0;

                connection();

                cmd = new MySqlCommand(query, con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        orderDate = Convert.ToDateTime(dr["order_date"]).ToShortDateString();
                        customerName = dr["customer_name"].ToString();
                        userName = dr["user_name"].ToString();
                        totalPrice.Items.Add(dr["price"].ToString());
                        orderTotal = dr["order_total"].ToString();

                        //POPULATE TABLE BODY WITH DATA FROM DB
                        tableData += "<tr>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_sold"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["price"].ToString() + "</td>";
                        tableData += "</tr>";
                        tblOrderItems.InnerHtml = tableData;

                        ////POPULATE TABLE FOOTER
                        //tableFoot += "<tr>";
                        //tableFoot += "<td></td>";
                        //tableFoot += "<td></td>";
                        //tableFoot += "<td class=" + "'text-center font-bold'" + ">" + orderTotal + "</td>";
                        //tableFoot += "</tr>";
                        //tblOrderFoot.InnerHtml = tableFoot;

                        i += 1;
                    }
                    //CHECK ORDER TOTAL FROM DB
                    switch (orderTotal)
                    {
                        case "":
                            //LOOP THROUGH LISTBOX ITEMS AND CALCULATE THE TOTAL COST OF ORDER
                            for (int j = 0; j < totalPrice.Items.Count; j++)
                            {
                                item = totalPrice.Items[j].ToString();
                                sum += Convert.ToDouble(item);
                            }

                            orderTotal = "TOTAL: GHȻ " + sum.ToString("#,0.00");
                            break;
                        default:
                            orderTotal = "TOTAL: GHȻ " + orderTotal;
                            break;
                    }

                    #region ORDER DETAILS DIV
                    //SET CSS CLASS AND INNER HTML OF USER & CUSTOMER DETAILS
                    if (customerName != "")
                    {
                        //CUSTOMER DIV
                        rowData += "<div class=" + "'col-sm-6 text-left'" + ">";
                        rowData += "       <i class=" + "'glyph-icon icon-users pad5R'" + "></i>";
                        rowData += "CUSTOMER:";
                        rowData += "   <span class=" + "'bs-label label-default font-bold font-size-16 pad0L'" + ">";
                        rowData += customerName;
                        rowData += "   </span>";
                        rowData += "</div>";
                    }
                    else
                    {
                        rowData += "<div class=" + "'col-sm-6 text-left'" + "></div>";
                    }
                    //TOTAL DIV
                    rowData += "<div class=" + "'col-sm-6 text-right'" + ">";
                    rowData += "   <span class=" + "'bs-label label-default font-bold font-size-16 pad0L'" + ">";
                    rowData += orderTotal;
                    rowData += "   </span>";
                    rowData += "</div>";

                    divRow.InnerHtml = rowData;

                    //USER DIV
                    userData += "<div class=" + "'col-sm-12 text-center'" + ">";
                    userData += "       <i class=" + "'glyph-icon icon-elusive-user pad5R'" + "></i>";
                    userData += "USER:";
                    userData += "   <span class=" + "'bs-label label-default font-bold font-size-16 pad0L '" + ">";
                    userData += userName;
                    userData += "   </span>";
                    userData += "</div>";

                    divUserDetails.InnerHtml = userData;

                    if (order_status == "PENDING")
                    {
                        //SET CSS CLASS AND INNER HTML OF ORDER DETAILS LABEL

                        //ORDER NUM DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB'" + ">";
                        spanData += "   <span class=" + "'bs-label label-yellow'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-shopping-cart pad5R'" + "></i>";
                        spanData += order_number;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //DATE DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-center'" + ">";
                        spanData += "   <span class=" + "'bs-label label-yellow'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-calendar pad5R'" + "></i>";
                        spanData += orderDate;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //ORDER STATUS DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-right'" + ">";
                        spanData += "   <span class=" + "'bs-label label-yellow'" + ">";
                        spanData += order_status;
                        spanData += "       <i class=" + "'glyph-icon icon-database pad5L'" + "></i>";
                        spanData += "   </span>";
                        spanData += "</div>";

                        divOrderDetails.InnerHtml = spanData;

                    }
                    else if (order_status == "COMPLETED")
                    {
                        //SET CSS CLASS AND INNER HTML OF ORDER DETAILS LABEL

                        //ORDER NUM DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB'" + ">";
                        spanData += "   <span class=" + "'bs-label label-success'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-shopping-cart pad5R'" + "></i>";
                        spanData += order_number;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //DATE DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-center'" + ">";
                        spanData += "   <span class=" + "'bs-label label-success'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-calendar pad5R'" + "></i>";
                        spanData += orderDate;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //ORDER STATUS DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-right'" + ">";
                        spanData += "   <span class=" + "'bs-label label-success'" + ">";
                        spanData += order_status;
                        spanData += "   <i class=" + "'glyph-icon icon-check pad5L'" + "></i>";
                        spanData += "</span>";
                        spanData += "</div>";

                        divOrderDetails.InnerHtml = spanData;

                    }
                    else if (order_status == "CANCELLED")
                    {
                        //SET CSS CLASS AND INNER HTML OF ORDER DETAILS LABEL

                        //ORDER NUM DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB'" + ">";
                        spanData += "   <span class=" + "'bs-label label-danger'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-shopping-cart pad5R'" + "></i>";
                        spanData += order_number;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //DATE DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-center'" + ">";
                        spanData += "   <span class=" + "'bs-label label-danger'" + ">";
                        spanData += "       <i class=" + "'glyph-icon icon-calendar pad5R'" + "></i>";
                        spanData += orderDate;
                        spanData += "   </span>";
                        spanData += "</div>";
                        //ORDER STATUS DIV
                        spanData += "<div class=" + "'col-sm-4 pad25TB text-right'" + ">";
                        spanData += "   <span class=" + "'bs-label label-danger'" + ">";
                        spanData += order_status;
                        spanData += "   <i class=" + "'glyph-icon icon-times pad5L'" + "></i>";
                        spanData += "</span>";
                        spanData += "</div>";

                        divOrderDetails.InnerHtml = spanData;
                    }
                    #endregion

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
                dr.Close();
            }
        }

        //ADD TO FILTER LISTITEMS
        private void filterOptions()
        {
            List<ListItem> filterOption = new List<ListItem>();
            filterOption.Add(new ListItem("Order Date", "1"));
            filterOption.Add(new ListItem("Order Status", "2"));
            filterOption.Add(new ListItem("User", "3"));
            ddlFilter.Items.AddRange(filterOption.ToArray());

        }
        private void user_name()
        {
            connection();

            cmd = new MySqlCommand("SELECT DISTINCT (full_name) FROM user_view", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                user.Items.Add(dr["full_name"].ToString());
            }
        }

        //SHOW MODAL METHOD
        private void showModal(string modalStatus)
        {
            switch (modalStatus)
            {
                case "COMPLETED":
                    divModal.Attributes["class"] = "modal-header bg-green";
                    break;

                case "PENDING":
                    divModal.Attributes["class"] = "modal-header bg-yellow";
                    break;

                case "CANCELLED":
                    divModal.Attributes["class"] = "modal-header bg-danger";
                    break;
                default:
                    break;
            }
            btnModClose.Attributes["class"] = "btn btn-alt btn-hover btn-blue";
            lblModTitle.Text = "ORDER DETAILS";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);

        }

        protected void btnFilter_ServerClick(object sender, EventArgs e)
        {
            filterOrder(selectedFilter);

            int orderCount = order_total.Items.Count;
            string item;
            decimal totalCost = 0;

            //CHECK IF FILTERED ORDERS != 0
            if (orderCount != 0)
            {
                //LOOP THROUGH FILTERED ORDERS AND SUM TOTAL COST
                for (int i = 0; i < order_total.Items.Count; i++)
                {
                    item = order_total.Items[i].ToString();
                    if (item == "")
                    {
                        item = "0";
                    }
                    totalCost += Convert.ToDecimal(item);
                }
                //DISPLAY DIV 
                divFilterDetails.Visible = true;

                spnTotalNumOrders.InnerHtml = "Total Orders: " + orderCount;
                spnTotalCostOrders.InnerHtml = "Total Cost: GHȻ " + totalCost.ToString("#,0.00");
            }
        }

        protected void btnShowFilters_ServerClick(object sender, EventArgs e)
        {
            divFilterButton.Visible = false;
            divFilter.Visible = true;
        }

        //LOAD TABLE METHOD
        private void filterOrder(string filter)
        {
            try
            {
                if (filter == "Order Date")
                {
                    switch (selectedCondition)
                    {
                        case "Is":
                            connection();

                            cmd = new MySqlCommand("SELECT * FROM order_view WHERE order_date = '" + txtStartDate.Text + "'", con);
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            break;

                        case "Is Between":
                            connection();
                            cmd = new MySqlCommand("SELECT * FROM order_view WHERE order_date BETWEEN '" + txtStartDate.Text + "' AND '" + txtEndDate.Text + "'", con);
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            break;
                        default:
                            break;
                    }
                }
                else if (filter == "Order Status")
                {
                    connection();

                    cmd = new MySqlCommand("SELECT * FROM order_view WHERE order_status = '" + ddlUserStatus.SelectedItem.Text + "'", con);
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();
                }
                else if (filter == "User")
                {
                    connection();

                    cmd = new MySqlCommand("SELECT * FROM order_view WHERE user_name = '" + ddlUserStatus.SelectedItem.Text + "'", con);
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();
                }
                else
                {
                    connection();

                    cmd = new MySqlCommand("SELECT * FROM order_view", con);
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();
                }

                string tableData = "";
                int i = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //ENCRYPT DATA IN URL
                        string strURLData = EncryptQueryString(string.Format("OrderID={0}&OrderNumber={1}&OrderStatus={2}", dr["order_id"].ToString(), dr["order_number"].ToString(), dr["order_status"]));

                        order_total.Items.Add(dr["order_total"].ToString());

                        //POPULATE TABLE WITH DATA FROM DB
                        tableData += "<tr>";
                        tableData += "<td class=" + "'text-center'" + ">" + Convert.ToDateTime(dr["order_date"]).ToShortDateString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["order_number"].ToString() + "</td>";

                        if (dr["order_total"].ToString() != "")
                        {
                            tableData += "<td class=" + "'text-center'" + ">" + "Ȼ " + dr["order_total"].ToString() + "</td>";
                        }
                        else
                        {
                            tableData += "<td class=" + "'text-center'" + ">" + dr["order_total"].ToString() + "</td>";
                        }

                        tableData += "<td class=" + "'text-center'" + ">" + dr["user_name"].ToString() + "</td>";

                        if (dr["order_status"].ToString() == "PENDING")
                        {
                            tableData += "      <td class=" + "'text-center'" + ">";
                            tableData += "          <span class=" + "'bs-label label-yellow'" + ">PENDING</span>";
                            tableData += "      </td>";
                        }
                        else if (dr["order_status"].ToString() == "COMPLETED")
                        {
                            tableData += "      <td class=" + "'text-center'" + ">";
                            tableData += "          <span class=" + "'bs-label label-success'" + ">COMPLETED</span>";
                            tableData += "      </td>";
                        }
                        else
                        {
                            tableData += "      <td class=" + "'text-center'" + ">";
                            tableData += "          <span class=" + "'bs-label label-danger'" + ">CANCELLED</span>";
                            tableData += "      </td>";
                        }

                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <a class=" + "'btn btn-alt btn-hover btn-primary tooltip-button'" + " href=view-order.aspx?" + strURLData + " data-placement=" + "top" + " title=" + "'View Order Details'" + ">";
                        tableData += "              <span>View Order Details</span>";
                        tableData += "              <i class=" + "'glyph-icon icon-eye'" + "></i>";
                        tableData += "          </a>";
                        tableData += "      </td>";

                        tableData += "</tr>";
                        tblOrder.InnerHtml = tableData;
                        i += 1;
                    }
                }
                else
                {
                    tblOrder.InnerHtml = tableData;
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

        protected void ddlCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCondition = ddlCondition.SelectedItem.Text;
            if (selectedFilter == "Order Date" && selectedCondition == "Is")
            {
                txtStartDate.Text = String.Empty;
                txtEndDate.Text = String.Empty;
                divActions.Visible = true;
                divStartDate.Visible = true;
                divDateAnd.Visible = false;
                divEndDate.Visible = false;
                divUserStatus.Visible = false;
                txtStartDate.Attributes["placeholder"] = "Select Date";
            }
            else if (selectedFilter == "Order Date" && selectedCondition == "Is Between")
            {
                txtStartDate.Text = String.Empty;
                txtEndDate.Text = String.Empty;
                divActions.Visible = true;
                divUserStatus.Visible = false;
                divStartDate.Visible = true;
                divDateAnd.Visible = true;
                divEndDate.Visible = true;
                txtStartDate.Attributes["placeholder"] = "Start Date";
                txtEndDate.Attributes["placeholder"] = "End Date";
            }
            else if (selectedFilter == "User" && selectedCondition == "Is")
            {
                divActions.Visible = true;
                divStartDate.Visible = false;
                divDateAnd.Visible = false;
                divEndDate.Visible = false;
                divUserStatus.Visible = true;
                ddlUserStatus.Items.Clear();
                ddlUserStatus.Items.Add(new ListItem("- Select Option -", "0"));

                for (int i = 0; i < user.Items.Count; i++)
                {
                    ddlUserStatus.Items.Add(user.Items[i]);
                }
            }
            else if (selectedFilter == "Order Status" && selectedCondition == "Is")
            {
                divActions.Visible = true;
                divStartDate.Visible = false;
                divDateAnd.Visible = false;
                divEndDate.Visible = false;
                divUserStatus.Visible = true;

                ddlUserStatus.Items.Clear();
                List<ListItem> filterStatus = new List<ListItem>();
                ddlUserStatus.Items.Clear();
                filterStatus.Add(new ListItem("- Select Option -", "0"));
                filterStatus.Add(new ListItem("Cancelled", "1"));
                filterStatus.Add(new ListItem("Completed", "2"));
                filterStatus.Add(new ListItem("Pending", "3"));
                ddlUserStatus.Items.AddRange(filterStatus.ToArray());
            }
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFilter = ddlFilter.SelectedItem.Text;
            divCondition.Visible = false;
            divActions.Visible = false;
            if (selectedFilter == "Order Date")
            {
                divCondition.Visible = true;
                ddlCondition.Items.Clear();
                List<ListItem> filterCondition = new List<ListItem>();
                filterCondition.Add(new ListItem("- Select Option -", "0"));
                filterCondition.Add(new ListItem("Is", "1"));
                filterCondition.Add(new ListItem("Is Between", "2"));
                ddlCondition.Items.AddRange(filterCondition.ToArray());
            }
            else if (selectedFilter == "Order Status" || selectedFilter == "User")
            {
                ddlCondition.Items.Clear();
                divCondition.Visible = true;
                List<ListItem> filterCondition = new List<ListItem>();
                filterCondition.Add(new ListItem("- Select Option -", "0"));
                filterCondition.Add(new ListItem("Is", "1"));
                ddlCondition.Items.AddRange(filterCondition.ToArray());
            }
        }

        protected void btnModClose_ServerClick(object sender, EventArgs e)
        {

        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                filterOrder("SEARCH");
            }
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/view-order.aspx");
        }


    }
}