using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace CRUD_ASP
{
    public partial class Display : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
               //Response.Write("Welcome..." + Session["a"]);
              string id = Request.QueryString["id"];
               //string ps = Request.QueryString["ps"];
               Response.Write(id + "<br>");
                this.BindGrid();
               this.BindDD();
            }
            
        }

        public void BindGrid()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT c.course_id,c.course_name,d.Dept_name FROM Tbl_Course c, Tbl_Dept d where c.Dept_id=d.Dept_id";
            
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();
        }

        public void BindDD()
        {
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            string query = "SELECT * FROM Tbl_Dept";
            SqlDataAdapter adpt = new SqlDataAdapter(query,con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            DDL_Dept.DataSource = dt;
            DDL_Dept.DataBind();
            DDL_Dept.DataTextField = "Dept_name";
            DDL_Dept.DataValueField = "Dept_id";
            DDL_Dept.DataBind();
            con.Close();
        }

        protected void DDL_Dept_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Tbl_Course where Dept_id=@deptId", con);
            cmd.Parameters.AddWithValue("deptId", DDL_Dept.SelectedValue);
            DDL_Course.DataSource = cmd.ExecuteReader();
            DDL_Course.DataTextField = "Course_name";
            DDL_Course.DataValueField = "Course_id";
            DDL_Course.DataBind();

        }

        protected void btn_Inset_Click(object sender, EventArgs e)
        {
            string course_name = txt_coursenm.Text;
            string Dept_name = DDL_Dept.SelectedValue.ToString();
            string query = "INSERT INTO Tbl_Course VALUES(@c_name, @D_name)";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query,con);
           
            cmd.Parameters.AddWithValue ("c_name", course_name);
            cmd.Parameters.AddWithValue("D_name", Dept_name);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();
        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            int course_id = Convert.ToInt16(row.Cells[1].Text);
            string course_name = txt_coursenm.Text;
            string Dept_id = DDL_Dept.SelectedValue.ToString();
            string query = "UPDATE Tbl_Course SET Course_name=@course_name, Dept_id=@Dept_id where Course_id=@courseid";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@course_name", course_name);
            cmd.Parameters.AddWithValue("@Dept_id", Dept_id);
            cmd.Parameters.AddWithValue("@courseid", course_id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            txt_coursenm.Text= row.Cells[2].Text;
            for(int i = 0; i < DDL_Dept.Items.Count; i++)
            {
                if (DDL_Dept.Items[i].Text == row.Cells[3].Text)
                {
                    DDL_Dept.SelectedIndex = i;            
                }
            }
            
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int c_id = Convert.ToInt16(row.Cells[1].Text);
            string query = "DELETE FROM Tbl_Course where Course_id=@courseid";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@courseid", c_id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();


        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            int course_id = Convert.ToInt16(row.Cells[1].Text);
                 
            string query = "DELETE FROM Tbl_Course where Course_id=@courseid";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@courseid", course_id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int c_id = Convert.ToInt16(row.Cells[1].Text);
            //int c_id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            //string Dept_id = (row.Cells[3].Controls[0] as TextBox).Text;
            string course_name = (row.Cells[2].Controls[0] as TextBox).Text;
            //TextBox course_name = GridView1.Rows[e.RowIndex].FindControl("TextBox1") as TextBox;
            string query = "UPDATE Tbl_Course SET Course_name=@course_name, Dept_id=@Dept_id where Course_id=@courseid";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@course_name", course_name);
           //cmd.Parameters.AddWithValue("@Dept_id", Dept_id);
            cmd.Parameters.AddWithValue("@courseid", c_id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();


        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Repeater.aspx");
        }
    }
}