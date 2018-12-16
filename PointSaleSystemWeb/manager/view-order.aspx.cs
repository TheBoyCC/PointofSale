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
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

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

        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM order_view", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    //POPULATE TABLE WITH DATA FROM DB

                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + Convert.ToDateTime(dr["order_date"]).ToShortDateString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["order_number"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_sold"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["price"].ToString() + "</td>";                  
                    tableData += "<td class=" + "'text-center'" + ">" + dr["user_name"].ToString() + "</td>";
                    tableData += "</tr>";
                    tblOrder.InnerHtml = tableData;
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

        private void searchOrder()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT * FROM order_view WHERE order_date BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "'", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    //POPULATE TABLE WITH DATA FROM DB

                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + Convert.ToDateTime(dr["order_date"]).ToShortDateString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["order_number"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_sold"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["price"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["user_name"].ToString() + "</td>";
                    tableData += "</tr>";
                    tblOrder.InnerHtml = tableData;
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchOrder();
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/view-order.aspx");
        }


    }
}