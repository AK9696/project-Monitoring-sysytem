using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class MasterProjectWorkPackageAddBOQItemDivisionWise : System.Web.UI.Page
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



            if (Request.QueryString.Count > 0)
            {

                hf_ProjectWorkPkg_Id.Value = Request.QueryString["ProjectWorkPkg_Id"].ToString();

                hf_DivisionId.Value = Request.QueryString["DivisionId"].ToString();
                get_tbl_ProjectWorkPkg();
                get_tbl_PackageEMB();
            }
            else
            {
                Response.Redirect("MasterProjectWorkPackage.aspx");
            }

        }
    }


    protected void grdEMB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkPostDeliverables = e.Row.FindControl("chkPostDeliverables") as CheckBox;
            if (Convert.ToInt32(e.Row.Cells[1].Text) > 0)
            {
                chkPostDeliverables.Checked = true;
            }


        }
    }

    protected void get_tbl_ProjectWorkPkg()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_tbl_ProjectWorkPkg(0, 0, 0, 0, 0, 0, Convert.ToInt32(hf_ProjectWorkPkg_Id.Value), "", "", false, "");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdPackageDetails.DataSource = ds.Tables[0];
            grdPackageDetails.DataBind();
        }
        else
        {
            grdPackageDetails.DataSource = null;
            grdPackageDetails.DataBind();
            //MessageBox.Show("Package Details Not Found");
        }
        
    }
    private void get_tbl_PackageEMB()
    {
        DataSet ds = new DataSet();
        ds = (new DataLayer()).Get_PackageBOQItems(hf_ProjectWorkPkg_Id.Value);

        if (AllClasses.CheckDataSet(ds))
        {
            grdEMB.DataSource = ds.Tables[0];
            grdEMB.DataBind();
        }
        else
        {
            MessageBox.Show("Details Not Found!!");
            return;
        }
    }

    protected void chkSelectAllDeliverables_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAllApproveH = (sender as CheckBox);
        for (int i = 0; i < grdEMB.Rows.Count; i++)
        {
            CheckBox chkSelectAllApprove = grdEMB.Rows[i].FindControl("chkPostDeliverables") as CheckBox;
            chkSelectAllApprove.Checked = chkSelectAllApproveH.Checked;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hf_ProjectWorkPkg_Id.Value == "")
            {
                MessageBox.Show("Please Provide Package Id");
                return;
            }
            if (hf_DivisionId.Value == "")
            {
                MessageBox.Show("Please Provide Division Id");
                return;
            }

            List<tbl_PackageDivisionBOQItem> obj_tbl_PackageDivisionBOQItem_Li = new List<tbl_PackageDivisionBOQItem>();
            for (int i = 0; i < grdEMB.Rows.Count; i++)
            {
                CheckBox chkPostDeliverables = (grdEMB.Rows[i].FindControl("chkPostDeliverables") as CheckBox);
                if (chkPostDeliverables.Checked == true)
                {
                    tbl_PackageDivisionBOQItem obj_tbl_PackageDivisionBOQItem = new tbl_PackageDivisionBOQItem();
                    obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_ProjectWorkPkg_Id = Convert.ToInt32(hf_ProjectWorkPkg_Id.Value);
                    obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_DevisionId = Convert.ToInt32(hf_DivisionId.Value);
                    obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_PackageBOQ_Id = Convert.ToInt32(grdEMB.Rows[i].Cells[0].Text);
                    obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_Status = 1;
                    obj_tbl_PackageDivisionBOQItem_Li.Add(obj_tbl_PackageDivisionBOQItem);
                }

            }
            if (obj_tbl_PackageDivisionBOQItem_Li.Count == 0)
            {
                MessageBox.Show("Please Add At least A Item To Save");
                return;
            }
            if ((new DataLayer()).tbl_PackageDivisionBOQItem(obj_tbl_PackageDivisionBOQItem_Li))
            {
                MessageBox.Show("Package BOQ Saved Successfully!");
                return;
            }
            else
            {
                MessageBox.Show("Error In Creating Package BOQ!");
                return;
            }
        }
        catch
        {

        }
    }
}