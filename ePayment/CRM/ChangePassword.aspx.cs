using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.MasterPageFile = SetMasterPage.ReturnPage();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Person_Id"] == null || Session["Login_Id"] == null)
        {
            Response.Redirect("Index.aspx");
        }
        if (!IsPostBack)
        {
            
        }
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (txtOld.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Password Old");
            txtOld.Focus();
            return;
        }
        if (txtNewPassowrd.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Password New");
            txtNewPassowrd.Focus();
            return;
        }
        if (txtConfirmPassowrd.Text.Trim() == "")
        {
            MessageBox.Show("Please Provide Password Confimation");
            txtConfirmPassowrd.Focus();
            return;
        }
        if (txtNewPassowrd.Text.Trim() != txtConfirmPassowrd.Text.Trim())
        {
            MessageBox.Show("Password and Confirm Password Does Not Match");
            txtNewPassowrd.Focus();
            return;
        }

        int Person_Id= Convert.ToInt32(Session["Person_Id"].ToString());
        if (new DataLayer().Change_Password(txtOld.Text.Trim().Replace("'", ""), txtNewPassowrd.Text.Trim().Replace("'", ""),Person_Id))
        {
            MessageBox.Show("Password Change Successfully!!");
            return;
        }
        else
        {
            MessageBox.Show("Worng Password!!");
            return;
        }

    }
}