using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserType"] != null && Session["UserType"].ToString() == "1")//Administrator
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "2")//District
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "3")//ULB
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "4")//Zone Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "5")//Contractor Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "6")//Circle Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "7")//Division Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"] != null && Session["UserType"].ToString() == "8")//Organizational Admin
        {
            if (Session["Person_BranchOffice_Id"].ToString() == "2" && Session["PersonJuridiction_DesignationId"].ToString() == "1")
            {
                if (Session["Login_IsDefault"].ToString() == "1")
                    Response.Redirect("DashboardSMD.aspx");
                else
                    Response.Redirect("ChangePassword.aspx");
            }
            else
            {
                if (Session["Login_IsDefault"].ToString() == "1")
                    Response.Redirect("Dashboard.aspx");
                else
                    Response.Redirect("ChangePassword.aspx");
            }
        }
        else
        {
            //Response.Redirect("Index.aspx");
        }
        txtUserName.Focus();
        if (!IsPostBack)
        {

        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text.Trim().Replace("'", "") == "")
        {
            MessageBox.Show("Please Provide Valid User Name..!!");
            txtUserName.Focus();
            return;
        }
        if (txtPassowrd.Text.Trim().Replace("'", "") == "")
        {
            MessageBox.Show("Please Provide Password!!");
            txtPassowrd.Focus();
            return;
        }
        DataSet ds = new DataSet();
        ds = new DataLayer().getLoginDetails(txtUserName.Text.Trim().Replace("'", ""), txtPassowrd.Text.Trim().Replace("'", ""));
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            init_Session(ds);
        }
        else
        {
            MessageBox.Show("Invalid Login Credentials!!");
            return;
        }
    }

    private void init_Session(DataSet ds)
    {
        Session["LoginHistory_Id"] = (new DataLayer()).Insert_tbl_LoginHistory(ds.Tables[0].Rows[0]["Person_Id"].ToString());
        Session["User_Permission"] = (new DataLayer()).get_User_Permission(ds.Tables[0].Rows[0]["Person_BranchOffice_Id"].ToString().Trim(), ds.Tables[0].Rows[0]["PersonJuridiction_DesignationId"].ToString());

        Session["Login_Id"] = ds.Tables[0].Rows[0]["Login_Id"].ToString().Trim();
        Session["Person_BranchOffice_Id"] = ds.Tables[0].Rows[0]["Person_BranchOffice_Id"].ToString().Trim();
        Session["UserName"] = ds.Tables[0].Rows[0]["Login_UserName"].ToString().Trim();
        Session["UserType"] = ds.Tables[0].Rows[0]["PersonJuridiction_UserTypeId"].ToString();
        Session["UserTypeName"] = ds.Tables[0].Rows[0]["UserType_Desc_E"].ToString();
        Session["Person_Id"] = ds.Tables[0].Rows[0]["Person_Id"].ToString();
        Session["Person_Name"] = ds.Tables[0].Rows[0]["Person_Name"].ToString();
        Session["Person_Mobile1"] = ds.Tables[0].Rows[0]["Person_Mobile1"].ToString();
        Session["ServerDate"] = ds.Tables[0].Rows[0]["ServerDate"].ToString();
        Session["M_Level_Id"] = ds.Tables[0].Rows[0]["M_Level_Id"].ToString();
        Session["M_Jurisdiction_Id"] = ds.Tables[0].Rows[0]["M_Jurisdiction_Id"].ToString();
        Session["PersonJuridiction_DesignationId"] = ds.Tables[0].Rows[0]["PersonJuridiction_DesignationId"].ToString();
        Session["PersonJuridiction_DepartmentId"] = ds.Tables[0].Rows[0]["PersonJuridiction_DepartmentId"].ToString();
        Session["PersonJuridiction_Project_Id"] = ds.Tables[0].Rows[0]["PersonJuridiction_Project_Id"].ToString();
        Session["Department_Name"] = ds.Tables[0].Rows[0]["Department_Name"].ToString();
        Session["Designation_DesignationName"] = ds.Tables[0].Rows[0]["Designation_DesignationName"].ToString();
        Session["TypingMode"] = "G";
        Session["ULB_District_Id"] = ds.Tables[0].Rows[0]["District_Id"].ToString();
        Session["ULB_Id"] = ds.Tables[0].Rows[0]["ULB_Id"].ToString();
        Session["Zone_Name"] = ds.Tables[0].Rows[0]["Zone_Name"].ToString();
        Session["Circle_Name"] = ds.Tables[0].Rows[0]["Circle_Name"].ToString();
        Session["Division_Name"] = ds.Tables[0].Rows[0]["Division_Name"].ToString();
        Session["PersonJuridiction_ZoneId"] = ds.Tables[0].Rows[0]["PersonJuridiction_ZoneId"].ToString();
        Session["PersonJuridiction_CircleId"] = ds.Tables[0].Rows[0]["PersonJuridiction_CircleId"].ToString();
        Session["PersonJuridiction_DivisionId"] = ds.Tables[0].Rows[0]["PersonJuridiction_DivisionId"].ToString();
        Session["Login_IsDefault"] = ds.Tables[0].Rows[0]["Login_IsDefault"].ToString();

        if (Session["UserType"].ToString() == "1")//Administrator
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "2")//District
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "3")//ULB
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "4")//Zone Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "5")//Contractor Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "6")//Circle Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "7")//Division Officer
        {
            if (Session["Login_IsDefault"].ToString() == "1")
                Response.Redirect("Dashboard.aspx");
            else
                Response.Redirect("ChangePassword.aspx");
        }
        else if (Session["UserType"].ToString() == "8")//Organizational Admin
        {
            if (Session["Person_BranchOffice_Id"].ToString() == "2" && Session["PersonJuridiction_DesignationId"].ToString() == "1")
            {
                if (Session["Login_IsDefault"].ToString() == "1")
                    Response.Redirect("DashboardSMD.aspx");
                else
                    Response.Redirect("ChangePassword.aspx");
            }
            else
            {
                if (Session["Login_IsDefault"].ToString() == "1")
                    Response.Redirect("Dashboard.aspx");
                else
                    Response.Redirect("ChangePassword.aspx");
            }
        }
        else
        {
            MessageBox.Show("Not Authorized To Login...!!");
            return;
        }
    }
}