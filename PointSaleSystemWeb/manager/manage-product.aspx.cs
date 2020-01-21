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
            if (!IsPostBack)
            {
                getQueryString();
            }

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

        //ENCRYPT QUERY FUNCTION
        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "r0b1nr0y");
        }
        //DECRYPTION OF QUERY STRING
        private string DecryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Decrypt(strQueryString, "r0b1nr0y");
        }

        //READ DATA FROM URL
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

                arrIndMsg = arrMsgs[0].Split('='); //GET ProductID
                lblProductID.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[1].Split('='); //GET Product Name
                lblProductName.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[2].Split('='); //GET Status
                lblStatus.Text = arrIndMsg[1].ToString().Trim();

                showModal(lblStatus.Text);
            }
        }

        //SHOW MODAL METHOD
        private void showModal(string pStatus)
        {
            if (pStatus == "1")
            {
                lblStatus.Text = Convert.ToInt32("0").ToString();

                divModal.Attributes["class"] = "modal-header bg-danger";
                btnModYes.Attributes["class"] = "btn btn-danger";
                lblModTitle.Text = "DELETE PRODUCT";
                lblModMessage.Text = "Are you sure you want to Delete " + lblProductName.Text + " ?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);

            }
            else if (pStatus == "0")
            {
                lblStatus.Text = Convert.ToInt32("1").ToString();

                divModal.Attributes["class"] = "modal-header bg-green";
                btnModYes.Attributes["class"] = "btn btn-success";
                lblModTitle.Text = "RESTORE PRODUCT";
                lblModMessage.Text = "Are you sure you want to Restore " + lblProductName.Text + " ?";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "ShowModal();", true);

            }
        }

        //LOAD TABLE METHOD
        private void loadTable()
        {
            try
            {
                connection();

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


                    if (qty <= minStock)
                    {
                        tableData += "<tr>";
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["product_name"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["category"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["quantity_available"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["cost_price"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center font-red'" + ">" + dr["selling_price"].ToString() + "</td>";
                    }
                    else
                    {
                        tableData += "<tr>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["category"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_available"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["cost_price"].ToString() + "</td>";
                        tableData += "<td class=" + "'text-center'" + ">" + dr["selling_price"].ToString() + "</td>";
                    }


                    string strURLData = EncryptQueryString(dr["product_id"].ToString());
                    string delURLData = EncryptQueryString(string.Format("ID={0}&Name={1}&Status={2}", dr["product_id"].ToString(), dr["product_name"].ToString(), dr["status"].ToString()));

                    tableData += "      <td class=" + "'text-center'" + ">";
                    tableData += "          <a class=" + "'btn btn-primary tooltip-button'" + " href=edit-product.aspx?" + strURLData + " data-placement=" + "top" + " title=" + "'Edit Product'" + ">";
                    tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                    tableData += "          </a>";

                    if (dr["status"].ToString() == "1")
                    {

                        tableData += "          <a class=" + "'btn btn-danger tooltip-button'" + " href=manage-product.aspx?" + delURLData + " data-placement=" + "top" + " title=" + "'Deactivate Product'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-close'" + "></i>";
                        tableData += "          </a>";
                    }
                    else
                    {
                        tableData += "          <a class=" + "'btn btn-success tooltip-button'" + " href=manage-product.aspx?" + delURLData + " data-placement=" + "top" + " title=" + "'Activate Product'" + ">";
                        tableData += "              <i class=" + "'glyph-icon icon-check'" + "></i>";
                        tableData += "          </a>";
                    }
                    tableData += "      </td>";

                    if (qty == 0)
                    {
                        tableData += "      <td class=" + "'text-center'" + ">";
                        tableData += "          <span class=" + "'bs-label label-danger'" + ">OUT OF STOCK</span>";
                        tableData += "      </td>";
                    }
                    else if (qty <= minStock)
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

        //UPDATE PRODUCT
        private void updateProduct(string productStatus)
        {
            int status = Convert.ToInt32(productStatus);
            connection();

            cmd = new MySqlCommand("UPDATE product SET status = @status, modified_date = @modified_date WHERE product_id = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        protected void btnModClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }

        protected void btnModYes_ServerClick(object sender, EventArgs e)
        {
            
            updateProduct(lblStatus.Text);
            Response.Redirect("~/manager/manage-product.aspx");
        }

        protected void clsAlertError_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }

    }
}