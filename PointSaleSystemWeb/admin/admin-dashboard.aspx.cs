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

namespace PointSaleSystemWeb.admin
{
    public partial class _default : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        string sqlcon, userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            readSettings();
            readOrders();
            readUsers();
            readProducts();

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
                Response.Redirect("~/admin/default.aspx");
            }
        }

        //MYSQL CONNCETION
        public void connection()
        {
            sqlcon = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            con = new MySqlConnection(sqlcon);
            con.Open();
        }

        //READ SETTINGS
        private void readSettings()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT shop_name FROM setting");
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    title.InnerHtml = dr["shop_name"].ToString();
                   
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

        //READ PRODUCTS
        private void readProducts()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT COUNT(product_id) AS num_products FROM product");
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                   divProduct.InnerHtml = dr["num_products"].ToString();

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

        //READ ORDERS
        private void readOrders()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT COUNT(order_id) AS num_orders FROM `order`");
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    divOrder.InnerHtml = dr["num_orders"].ToString();

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

        //READ USERS
        private void readUsers()
        {
            try
            {
                connection();

                cmd = new MySqlCommand("SELECT COUNT(user_id) AS num_users FROM user");
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    divUser.InnerHtml = dr["num_users"].ToString();

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

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/admin-dashboard.aspx");
        }

    }
}