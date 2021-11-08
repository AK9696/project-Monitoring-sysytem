using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Map_UP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            get_SVG_Map_Data("", "", 0, 0);
        }
    }

    protected void get_SVG_Map_Data(string fromDate, string tillDate, int Project_Id, int District_Id)
    {
        string innerHTML = "";
        DataSet ds = new DataSet();
        ds = (new DataLayer()).get_SVG_Map_Data(fromDate, tillDate, Project_Id, District_Id, 0, rbtData.SelectedValue);
        if (AllClasses.CheckDataSet(ds))
        {
            innerHTML += "<div class=\"svg-parent fadeInUp\" style=\"animation-delay: 2.5s;margin-top: -90px;\"><svg id=\"chart\" width=\"480\" height=\"450\" viewBox=\"0 0 480 450\" preserveAspectRatio=\"xMidYMid meet\"><g class=\"states\">";
            //All District Data in Append Mode
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                innerHTML += (HttpUtility.UrlDecode(ds.Tables[0].Rows[i]["SVGMapping_Name"].ToString())).Replace("<title></title>", "<title>" + ds.Tables[0].Rows[i]["Jurisdiction_Name_Eng"].ToString() + ":-" + ds.Tables[0].Rows[i]["Total_Work"].ToString() + ":" + ds.Tables[0].Rows[i]["ProjectDPR_BudgetAllocated"].ToString() + ":" + ds.Tables[0].Rows[i]["Fund_Released"].ToString() + ":" + ds.Tables[0].Rows[i]["Expenditure"].ToString() + "</title>").Replace("#43d8c9", ds.Tables[0].Rows[i]["ColorCodes_Code"].ToString()).Replace("path-region", "path-region gridViewToolTip");
            }
            innerHTML += "</g>";
            //Boundry Data
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    innerHTML += HttpUtility.UrlDecode(ds.Tables[1].Rows[0]["SVGMapping_Name"].ToString());
                }
            }
            innerHTML += "</svg></div>";
            div_Progress_Bar.InnerHtml = innerHTML;


            innerHTML = "";
            //Legends
            innerHTML += "<table id='simple-table' class='table table-bordered table-hover'>";
            innerHTML += "<thead>";
            innerHTML += "<tr>";
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    innerHTML += "<th style = 'background-color:" + ds.Tables[2].Rows[i]["ColorCodes_Code"].ToString() + "'>" + ds.Tables[2].Rows[i]["From_Range"].ToString() + " - " + ds.Tables[2].Rows[i]["Till_Range"].ToString() + "</th>";
                }
            }
            innerHTML += "</tr>";
            innerHTML += "</thead>";
            innerHTML += "</table>";
            divDataPointsL.InnerHtml = innerHTML;
        }
        else
        {
            innerHTML = "";
        }
    }

    protected void rbtData_SelectedIndexChanged(object sender, EventArgs e)
    {
        get_SVG_Map_Data("", "", 0, 0);
    }
}