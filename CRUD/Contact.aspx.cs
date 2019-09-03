using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace CRUD
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-B22QP1E\SACHITHSQL;Initial Catalog=ASPCRUD;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("ContactCreateorUpdate", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ContactID", (hfContactID.Value == "" ? 0 : Convert.ToInt32(hfContactID.Value)));
                sqlcmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
                string contactID = hfContactID.Value;
                clear();
                
                if (contactID == "")
                {
                    lblSuccessMessage.Text = "Saved Successfully";
                    
                }
                else
                {
                    lblSuccessMessage.Text = "Update Successfully";
                    
                }
                FillGridView();
            }
        }
        void FillGridView()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll", sqlcon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtb1 = new DataTable();
                sqlDa.Fill(dtb1);
                sqlcon.Close();
                gvContact.DataSource = dtb1;
                gvContact.DataBind();
            }
        }
        protected void link_onClick(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewByID", sqlcon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@ContactID", contactID);
                DataTable dtb1 = new DataTable();
                sqlDa.Fill(dtb1);
                sqlcon.Close();
                hfContactID.Value = contactID.ToString();
                txtName.Text = dtb1.Rows[0]["Name"].ToString();
                txtMobile.Text = dtb1.Rows[0]["Mobile"].ToString();
                txtAddress.Text = dtb1.Rows[0]["Address"].ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("ContactDletedByID", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
                clear();
                FillGridView();
                lblSuccessMessage.Text = "Deleted Successfully";
            }
        }
    }
}