using System.Web;
using System.Web.UI;

public class SetMasterPage : Page
{
    public static string ReturnPage()
    {
        string _page = string.Empty;
        if (HttpContext.Current.Session["UserType"] != null)
        {
            if (HttpContext.Current.Session["UserType"].ToString() == "1")//Administrator
            {
                _page = "TemplateMasterAdmin.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "2")//District
            {
                _page = "TemplateMasterDistrict.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "3")//ULB
            {
                _page = "TemplateMasterULB.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "4")//Zone Officer
            {
                _page = "TemplateMasterZone.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "5")//Contractor Officer
            {
                _page = "TemplateMasterSection.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "6")//Circle Officer
            {
                _page = "TemplateMasterCircle.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "7")//Division Officer
            {
                _page = "TemplateMasterDivision.master";
            }
            else if (HttpContext.Current.Session["UserType"].ToString() == "8")//Organisational Admin
            {
                _page = "TemplateMasterAdminOrg.master";
            }
            else
            {
                _page = "TemplateMasterAdmin.master";
            }
            return _page;
        }
        else
        {
            HttpContext.Current.Response.Redirect("Index.aspx", true);
            return _page;
        }
    }
}
