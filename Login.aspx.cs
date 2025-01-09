using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CRUD_ASP
{
    public partial class Login : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string emp_email = txt_loginid.Text;
            string emp_pass = txt_passwd.Text; 
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            string query = "select count(*) from Tbl_Emp where Emp_email= @email and Emp_pass = @pass";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("email", emp_email);
            cmd.Parameters.AddWithValue("pass", emp_pass);
            int i = Convert.ToInt16(cmd.ExecuteScalar());

            if (i == 0)
            {
                Response.Write("Login not valid");
            }
            else
            {
              Session["a"] = txt_loginid.Text;
              Response.Redirect("Display.aspx?id=" + txt_loginid.Text + "&ps=" + txt_passwd.Text);
              // Response.Redirect("Display.aspx");

                              
                //Session["b"] = i;
                //  ViewState["temp"] = Txt_id.Text;
              

            }
            con.Close();

        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_passwd.Text = "";
        }
    }
}