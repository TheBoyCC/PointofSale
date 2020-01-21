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
    public partial class product_status : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        
        string sqlcon, userID, categoryID, status;
        protected void Page_Load(object sender, EventArgs e)
        {
            session();

            if (!IsPostBack)
            {
                getQueryString();
                getProduct();
                readCategory();
                loadTable();
            }
            showModal(status);
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

        //ENCRYPTION OF QUERY STRING
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
            strReq = strReq.Substring(strReq.IndexOf('?') + 1);

            if (!strReq.Equals(""))
            {
                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;

                arrIndMsg = arrMsgs[0].Split('='); //GET ProductID
                lblProductID.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[1].Split('='); //GET Product Name
                lblProductName.Text = arrIndMsg[1].ToString().Trim();
                arrIndMsg = arrMsgs[2].Split('='); //GET Status
                status = arrIndMsg[1].ToString().Trim();
            }
            else
            {
                Response.Redirect("~/manager/manage-product.aspx");
            }
        }

        //READ PRODUCT FROM DB
        private void getProduct()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT * FROM product WHERE product_id = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txtProductName.Text = dr["product_name"].ToString();
                categoryID = dr["category_id"].ToString();
                txtQuantity.Text = dr["quantity_available"].ToString();
                txtSellingPrice.Text = dr["selling_price"].ToString();
                txtCostPrice.Text = dr["cost_price"].ToString();
                txtMinLevel.Text = dr["min_stock_level"].ToString();
            }
            con.Close();
        }

        //LOAD TABLE METHOD
        private void loadTable()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT * FROM product_view ORDER BY product_id ASC", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            string tableData = "";
            int i = 0;

            while (dr.Read())
            {
                //POPULATE TABLE
                tableData += "<tr>";
                tableData += "<td class=" + "'text-center'" + ">" + dr["product_name"].ToString() + "</td>";
                tableData += "<td class=" + "'text-center'" + ">" + dr["selling_price"].ToString() + "</td>";
                tableData += "<td class=" + "'text-center'" + ">" + dr["quantity_available"].ToString() + "</td>";

                string strURLData = EncryptQueryString(dr["product_id"].ToString());
                string delURLData = EncryptQueryString(string.Format("ID={0}&Name={1}&Status={2}", dr["product_id"].ToString(), dr["product_name"], dr["status"].ToString()));

                tableData += "      <td class=" + "'text-center'" + ">";
                tableData += "          <a class=" + "'btn btn-primary disabled'" + " href=edit-product.aspx?" + strURLData + " title=" + "'Edit'" + ">";
                tableData += "              <i class=" + "'glyph-icon icon-pencil'" + "></i>";
                tableData += "          </a>";

                tableData += "          <a class=" + "'btn btn-danger disabled'" + " href=product-status.aspx?" + delURLData + " title=" + "'Delete'" + ">";
                tableData += "              <i class=" + "'glyph-icon icon-close'" + "></i>";
                tableData += "          </a>";

                tableData += "      </td>";

                tableData += "</tr>";

                tblProduct.InnerHtml = tableData;
                i += 1;
            }
            con.Close();
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

        #region TODO
        //TODO
        private void readUpdateSale()
        {
            ListBox saleID = new ListBox();

            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT SaleID FROM sale WHERE ProductID = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                saleID.Items.Add(dr["SaleID"].ToString());
            }
            dr.Close();

            deleteProduct();

            string sale_ID;

            foreach (var item in saleID.Items)
            {
                sale_ID = item.ToString();

                cmd = new MySqlCommand("UPDATE sale SET ProductID = @ProductID  WHERE SaleID = '" + sale_ID + "'", con);
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@ProductID", lblProductID.Text);

                cmd.ExecuteNonQuery();

            }

            con.Close();
            

            alertSuccessPanel.Visible = true;
            alertSuccessTitle.Text = "SUCCESS";
            alertSuccessMessage.Text = txtProductName.Text + " Deleted Succesfully.";
        }
        private void deleteProduct()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("DELETE FROM stock WHERE ProductID = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;
            
            cmd.ExecuteNonQuery();
            
        }
        #endregion

        //UPDATE PRODUCT
        private void updateProduct()
        {
            int status = Convert.ToInt32(lblStatus.Text);
            connection();
            con.Open();

            cmd = new MySqlCommand("UPDATE product SET status = @status, modified_date = @modified_date WHERE product_id = '" + lblProductID.Text + "'", con);
            cmd.Connection = con;

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@modified_date", DateTime.Now);

            cmd.ExecuteNonQuery();

            con.Close();

        }

        //READ CATEGORY FROM DB
        private void readCategory()
        {
            connection();
            con.Open();

            cmd = new MySqlCommand("SELECT category FROM category WHERE category_id = '" + categoryID + "'", con);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txtCategory.Text = dr["category"].ToString();
            }
            con.Close();
        }

        protected void clsAlertSuccess_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }
        protected void btnModClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/manager/manage-product.aspx");
        }
        protected void btnModYes_ServerClick(object sender, EventArgs e)
        {
            updateProduct();
            Response.Redirect("~/manager/manage-product.aspx");
        }
    }
}