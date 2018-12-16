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
    public partial class manage_product : System.Web.UI.Page
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
        }
        //ENCRYPT QUERY FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }


        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();
                con.Open();

                cmd = new MySqlCommand("SELECT * FROM product_view", con);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                string tableData = "";
                int i = 0;

                while (dr.Read())
                {
                    //POPULATE TABLE WITH DATA FROM DB

                    int minStock = Convert.ToInt32(dr["min_stock_level"]);
                    int qty = Convert.ToInt32(dr["quantity_available"]);

                    tableData += "<tr>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["category"].ToString() + "</td>";
                    
                    if (qty <= minStock)
                    {
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["quantity_available"].ToString() + "</td>";
                    }
                    else
                    {
                        tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_available"].ToString() + "</td>";
                    }
                    tableData += "<td class=" + "'text-center'" + ">" + dr["cost_price"].ToString() + "</td>";
                    tableData += "<td class=" + "'text-center'" + ">" + dr["selling_price"].ToString() + "</td>";

                    string strURLData = EncryptQueryString(dr["product_id"].ToString());
                    string delURLData = EncryptQueryString(string.Format("ID={0}&Name={1}&Status={2}", dr["product_id"].ToString(), dr["product_name"].ToString(), dr["status"].ToString()));

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary'" + " href=edit-product.aspx?" + strURLData + " title=" + "'Edit'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "          </a>";

                    if (dr["status"].ToString() == "1")
                    {

                        tableData += "          <a class=" + "'btn btn-danger'" + " href=product-status.aspx?" + delURLData + " title=" + "'Deactivate'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-close'" + "></i>";
                        tableData += "          </a>";
                    }
                    else
                    {
                        tableData += "          <a class=" + "'btn btn-success'" + " href=product-status.aspx?" + delURLData + " title=" + "'Activate'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-check'" + "></i>";
                        tableData += "          </a>";
                    }
                    tableData += "      </td>";

                    if (qty <= minStock)
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-danger'" + ">LOW STOCK</span>";
                        tableData += "      </td>";
                    }
                    else if (dr["status"].ToString() == "1")
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-success'" + ">ACTIVE</span>";
                        tableData += "      </td>";
                    }
                    else
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-danger'" + ">INACTIVE</span>";
                        tableData += "      </td>";
                    }
                    
                    tableData += "      </td>";
                    tableData += "</tr>";

                    tblProduct.InnerHtml = tableData;
                    i += 1;
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

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }

    }
}