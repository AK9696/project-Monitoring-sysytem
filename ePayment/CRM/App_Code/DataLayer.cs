using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web.SessionState;
using System.Web.UI;
public partial class DataLayer : Page, IRequiresSessionState
{
    string ConStr = "";

    public DataLayer()
    {
        ConStr = ConfigurationManager.AppSettings.Get("conn");
    }

    #region DataLayer Function
    private DataSet ExecuteSelectQuery(string Sql)
    {
        DataSet set1 = new DataSet();
        using (SqlConnection con = new SqlConnection(ConStr))
        {
            SqlCommand command1 = new SqlCommand(Sql, con);
            if (command1.Connection.State == ConnectionState.Closed)
            {
                command1.Connection.Open();
            }
            command1.CommandTimeout = 7000;
            SqlDataAdapter adapter1 = new SqlDataAdapter(command1);

            adapter1.Fill(set1);
        }
        return set1;
    }

    private DataSet ExecuteSelectQuerywithTransaction(SqlConnection Con, string Sql, SqlTransaction trans)
    {
        DataSet set1 = new DataSet();
        SqlCommand command1 = new SqlCommand(Sql, Con, trans);
        command1.CommandTimeout = 7000;
        SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
        adapter1.Fill(set1);
        return set1;
    }
    #endregion

    #region Login
    public DataSet getLoginDetails(string User_Name, string Password)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        tbl_PersonDetail.Person_Id,
	                    tbl_PersonDetail.Person_Name,
	                    Person_Name_Full = isnull(tbl_PersonDetail.Person_Name, '') + ', ' + isnull(Designation_DesignationName, '') + ', Mob: ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                    tbl_PersonDetail.Person_Mobile1,
	                    tbl_PersonJuridiction.M_Level_Id,
                        tbl_PersonDetail.Person_BranchOffice_Id,
	                    tbl_PersonJuridiction.M_Jurisdiction_Id,
	                    tbl_PersonJuridiction.PersonJuridiction_DesignationId,
	                    tbl_PersonJuridiction.PersonJuridiction_DepartmentId,
	                    tbl_PersonJuridiction.PersonJuridiction_UserTypeId,
	                    tbl_PersonJuridiction.PersonJuridiction_ParentPerson_Id,
						tbl_PersonJuridiction.PersonJuridiction_ZoneId,
						tbl_PersonJuridiction.PersonJuridiction_CircleId,
						tbl_PersonJuridiction.PersonJuridiction_DivisionId,
						tbl_PersonJuridiction.PersonJuridiction_ULB_Id,
                        tbl_PersonJuridiction.PersonJuridiction_Project_Id,
                        Person_BranchOffice_Id,
	                    Department_Name,
	                    Designation_DesignationName,
	                    Level_Name,
	                    UserType_Desc_E,
	                    Login_UserName,
                        Login_Id,
                        CONVERT(char(10),getdate(),103) ServerDate, 
                        District_Id = tbl_PersonJuridiction.M_Jurisdiction_Id, 
                        ULB_Id,
						Zone_Name,
						Circle_Name,
						Division_Name,
                        Login_IsDefault = isnull(Login_IsDefault, 0), 
						Person_BranchOffice_Id
                    from tbl_PersonDetail
                    join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                    left join tbl_Department on Department_Id = PersonJuridiction_DepartmentId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join M_Level on M_Level.M_Level_Id = tbl_PersonJuridiction.M_Level_Id
                    left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    join tbl_UserLogin on Login_PersonId = Person_Id
                    left join tbl_ULB on ULB_Id = tbl_PersonJuridiction.PersonJuridiction_ULB_Id
					left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
					left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
					left join tbl_Division on Division_Id = PersonJuridiction_DivisionId
                    where tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and (Login_UserName = '" + User_Name + "' or tbl_PersonDetail.Person_Mobile1 = '" + User_Name + "') and Login_password = '" + Password + "'; ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch (Exception)
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_User_Permission(string Office_Id, string Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        ProcessConfigMaster_Id,
	                    ProcessConfigMaster_Creation_Allowed,
	                    ProcessConfigMaster_Updation_Allowed, 
                        ProcessConfigMaster_Deduction_Allowed, 
                        ProcessConfigMaster_Transfer_Allowed
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = 'BOQ' and ProcessConfigMaster_OrgId = '" + Office_Id + "' and (ProcessConfigMaster_Designation_Id = '" + Designation_Id + "' or ProcessConfigMaster_Designation_Id1 = '" + Designation_Id + "'); ";

        strQuery += @"select 
                        ProcessConfigMaster_Id,
	                    ProcessConfigMaster_Creation_Allowed,
	                    ProcessConfigMaster_Updation_Allowed, 
                        ProcessConfigMaster_Deduction_Allowed, 
                        ProcessConfigMaster_Transfer_Allowed
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = 'EMB' and ProcessConfigMaster_OrgId = '" + Office_Id + "' and (ProcessConfigMaster_Designation_Id = '" + Designation_Id + "' or ProcessConfigMaster_Designation_Id1 = '" + Designation_Id + "'); ";

        strQuery += @"select 
                        ProcessConfigMaster_Id,
	                    ProcessConfigMaster_Creation_Allowed,
	                    ProcessConfigMaster_Updation_Allowed, 
                        ProcessConfigMaster_Deduction_Allowed, 
                        ProcessConfigMaster_Transfer_Allowed
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = 'Invoice' and ProcessConfigMaster_OrgId = '" + Office_Id + "' and (ProcessConfigMaster_Designation_Id = '" + Designation_Id + "' or ProcessConfigMaster_Designation_Id1 = '" + Designation_Id + "'); ";


        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch (Exception)
        {
            ds = null;
        }
        return ds;
    }

    #endregion

    #region Login History
    public int Insert_tbl_LoginHistory(string Person_Id)
    {
        DataSet ds = new DataSet();
        int iResult = 0;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string sql = "set dateformat dmy; insert into tbl_LoginHistory (LoginHistory_PersonId, LoginHistory_LoggedInTime, LoginHistory_LoggedOutTime, LoginHistory_IPAddress, LoginHistory_MACAddress, LoginHistory_Status) values ('" + Person_Id + "', getdate(), null, '" + AllClasses.getIPAddress() + ", " + AllClasses.getIPAddress2() + "', '" + AllClasses.getMACAddress() + "', 1); select @@IDENTITY";
                ds = ExecuteSelectQuery(sql);
                if (AllClasses.CheckDataSet(ds))
                {
                    iResult = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    iResult = 0;
                }
            }
            catch (Exception)
            {
                cn.Close();
                iResult = -1;
            }
        }
        return iResult;
    }

    public int Update_tbl_LoginHistory(string LoginHistory_Id)
    {
        DataSet ds = new DataSet();
        int iResult = 0;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string sql = "set dateformat dmy; update tbl_LoginHistory set LoginHistory_LoggedOutTime = getdate() where LoginHistory_Id = '" + LoginHistory_Id + "'";
                ds = ExecuteSelectQuery(sql);
                iResult = 1;
            }
            catch (Exception)
            {
                cn.Close();
                iResult = -1;
            }
        }
        return iResult;
    }

    public string get_tbl_LoginHistory_Last_Login(string Person_Id)
    {
        DataSet ds = new DataSet();
        string iResult = "";
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string sql = @"set dateformat dmy; with cte1 as (
                                select ROW_NUMBER() over(order by LoginHistory_Id desc) rr, * from tbl_LoginHistory
                                where LoginHistory_PersonId = '" + Person_Id + "') select convert(varchar, LoginHistory_LoggedInTime, 9) LastLogin from cte1 where rr = 2";
                ds = ExecuteSelectQuery(sql);
                if (AllClasses.CheckDataSet(ds))
                {
                    iResult = " (Your Last Login : " + ds.Tables[0].Rows[0]["LastLogin"].ToString() + ") ";
                }
                else
                {
                    iResult = " (Your Last Login : Never) ";
                }
            }
            catch (Exception)
            {
                cn.Close();
                iResult = " (Your Last Login : Never) ";
            }
        }
        return iResult;
    }
    #endregion

    #region SMS Log
    public void Insert_tbl_SMS(tbl_SMS obj_tbl_SMS)
    {
        DataSet set1 = new DataSet();
        SqlCommand command1 = null;
        SqlDataAdapter adapter1 = null;
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_SMS (SMS_Mobile_No, SMS_Content, SMS_Response, SMS_AddedOn) values ('" + obj_tbl_SMS.SMS_Mobile_No + "', '" + obj_tbl_SMS.SMS_Content + "','" + obj_tbl_SMS.SMS_Response + "', getdate());Select @@Identity";
        try
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                command1 = new SqlCommand(strQuery, con);
                if (command1.Connection.State == ConnectionState.Closed)
                {
                    command1.Connection.Open();
                }
                command1.CommandTimeout = 7000;
                adapter1 = new SqlDataAdapter(command1);
                adapter1.Fill(set1);
            }
        }
        catch
        {

        }
    }

    public DataSet get_tbl_SMS(string date)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select * from tbl_SMS where convert(date, convert(char(10), SMS_AddedOn, 103), 103) = convert(date, '" + date + "', 103)";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Change Password
    public bool Change_Password(string OldPassword, string NewPassword, int Person_Id)
    {
        string strQuery = "";
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                strQuery = " set dateformat dmy; select Login_Id from tbl_UserLogin where Login_PersonId='" + Person_Id + "' and Login_password='" + OldPassword + "' and Login_Status=1";
                try
                {
                    ds = ExecuteSelectQuery(strQuery);
                }
                catch
                {
                    ds = null;
                }
                if (AllClasses.CheckDataSet(ds))
                {
                    strQuery = "update tbl_UserLogin set Login_password='" + NewPassword + "', Login_IsDefault = 1 where Login_PersonId='" + Person_Id + "' and  Login_Status=1";

                    if (trans == null)
                    {
                        ExecuteSelectQuery(strQuery);
                    }
                    else
                    {
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    }

                    trans.Commit();
                    cn.Close();
                    flag = true;
                }

            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    #endregion

    #region Master Jurisdiction
    public DataSet get_M_Level(bool excludeState)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select M_Level_Id, Level_Name, Created_By, Created_Date, Is_Active from M_Level where Is_Active = 1 excludeStateCond order by M_Level_Id";
        if (excludeState)
        {
            strQuery = strQuery.Replace("excludeStateCond", " and M_Level_Id != 1 ");
        }
        else
        {
            strQuery = strQuery.Replace("excludeStateCond", " ");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_M_Jurisdiction(int M_Level_Id, int Parent_Jurisdiction_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select M_Jurisdiction.M_Jurisdiction_Id, M_Jurisdiction.M_Level_Id, M_Jurisdiction.Jurisdiction_Name_Eng, Jurisdiction_Name_With_Code = M_Jurisdiction.Jurisdiction_Name_Eng + ' - ' + isnull(M_Jurisdiction.Jurisdiction_Code, ''), M_Jurisdiction.Parent_Jurisdiction_Id, M_Jurisdiction.Jurisdiction_Code, Jurisdiction_Name_Eng_With_Parent = (case when isnull(Parent_M_Jurisdiction.Jurisdiction_Name_Eng, '') = '' then '' else isnull(Parent_M_Jurisdiction.Jurisdiction_Name_Eng, '-NA-') + '- ' end) + M_Jurisdiction.Jurisdiction_Name_Eng, M_Jurisdiction.Created_By, M_Jurisdiction.Created_Date, M_Jurisdiction.Is_Active, IsNULL(Parent_M_Jurisdiction.Jurisdiction_Name_Eng,'-NA-') Parent_Jurisdiction_Name_Eng, isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, M_Jurisdiction.Created_Date,tbl_PersonDetail1.Person_Name as ModifyBy,M_Jurisdiction.M_Jurisdiction_ModifiedOn from M_Jurisdiction left join M_Jurisdiction Parent_M_Jurisdiction on Parent_M_Jurisdiction.M_Jurisdiction_Id = M_Jurisdiction.Parent_Jurisdiction_Id left join tbl_PersonDetail on Person_Id = M_Jurisdiction.Created_By left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = M_Jurisdiction.M_Jurisdiction_ModifiedBy where M_Jurisdiction.Is_Active = 1 ";
        if (M_Level_Id != 0)
        {
            strQuery += " and M_Jurisdiction.M_Level_Id = '" + M_Level_Id + "'";
        }
        if (Parent_Jurisdiction_Id != 0)
        {
            strQuery += " and M_Jurisdiction.Parent_Jurisdiction_Id = '" + Parent_Jurisdiction_Id + "'";
        }
        strQuery += " order by M_Jurisdiction.Jurisdiction_Name_Eng";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_M_Jurisdiction_District_Additional(int State_Id, int Person_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select M_Jurisdiction_Id, M_Level_Id, Jurisdiction_Name_Eng, Jurisdiction_Name_With_Code = Jurisdiction_Name_Eng + ' - ' + isnull(Jurisdiction_Code, ''), Parent_Jurisdiction_Id, Jurisdiction_Code, Created_By, Created_Date, Is_Active SelectCondition from M_Jurisdiction JoinCondition where Is_Active = 1 and M_Level_Id = 3 and Parent_Jurisdiction_Id in (select M_Jurisdiction_Id from M_Jurisdiction where M_Level_Id = 2 and Parent_Jurisdiction_Id = '" + State_Id + "') and M_Jurisdiction_Id not in (select distinct M_Jurisdiction_Id from tbl_PersonJuridiction join tbl_PersonDetail on Person_Id = PersonJuridiction_PersonId where Person_Status = 1 and PersonJuridiction_Status = 1 and PersonJuridiction_UserTypeId = 1) order by Jurisdiction_Name_Eng";

        if (Person_Id > 0)
        {
            strQuery = strQuery.Replace("JoinCondition", " left join tbl_PersonAdditionalArea on PersonAdditionalArea_JurisdictionId = M_Jurisdiction_Id and PersonAdditionalArea_PersonId = '" + Person_Id + "' and PersonAdditionalArea_Status = 1 ");
            strQuery = strQuery.Replace("SelectCondition", " , isnull(PersonAdditionalArea_Id, 0) PersonAdditionalArea_Id ");
        }
        else
        {
            strQuery = strQuery.Replace("JoinCondition", " ");
            strQuery = strQuery.Replace("SelectCondition", " , PersonAdditionalArea_Id = 0 ");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public string get_State_Id(string District_Id)
    {
        string State_Id = "0";
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = "set dateformat dmy; select Zone.Parent_Jurisdiction_Id from M_Jurisdiction District join M_Jurisdiction Zone on Zone.M_Jurisdiction_Id = District.Parent_Jurisdiction_Id where District.M_Level_Id = 3 and District.M_Jurisdiction_Id = '" + District_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
            if (AllClasses.CheckDataSet(ds))
            {
                State_Id = ds.Tables[0].Rows[0]["Parent_Jurisdiction_Id"].ToString();
            }
            else
            {
                State_Id = "0";
            }
        }
        catch
        {
            ds = null;
            State_Id = "0";
        }
        return State_Id;
    }
    public bool Insert_M_Jurisdiction(M_Jurisdiction obj_M_Jurisdiction, int M_Jurisdiction_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacy_M_Jurisdiction(obj_M_Jurisdiction.Jurisdiction_Name_Eng, M_Jurisdiction_Id.ToString(), obj_M_Jurisdiction.M_Level_Id, trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (M_Jurisdiction_Id == 0)
                {
                    Insert_M_Jurisdiction(obj_M_Jurisdiction, trans, cn);
                }
                else
                {
                    obj_M_Jurisdiction.M_Jurisdiction_Id = M_Jurisdiction_Id;
                    Update_M_Jurisdiction(obj_M_Jurisdiction, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacy_M_Jurisdiction(string Jurisdiction_Name, string M_Jurisdiction_Id, int M_Level_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from M_Jurisdiction  where Is_Active = 1 and M_Level_Id = '" + M_Level_Id + "' and  Jurisdiction_Name_Eng = '" + Jurisdiction_Name + "' ";
        if (M_Jurisdiction_Id != "0")
        {
            strQuery += " AND M_Jurisdiction_Id <> '" + M_Jurisdiction_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_M_Jurisdiction(M_Jurisdiction obj_M_Jurisdiction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into M_Jurisdiction (M_Level_Id, Created_Date, Jurisdiction_Name_Eng, Parent_Jurisdiction_Id, Jurisdiction_Code, Created_By, Is_Active, Jurisdiction_LokSabhaId, Jurisdiction_VidhanSabhaId) values ('" + obj_M_Jurisdiction.M_Level_Id + "', getdate(), N'" + obj_M_Jurisdiction.Jurisdiction_Name_Eng + "','" + obj_M_Jurisdiction.Parent_Jurisdiction_Id + "', '" + obj_M_Jurisdiction.Jurisdiction_Code + "', '" + obj_M_Jurisdiction.Created_By + "', '" + obj_M_Jurisdiction.Is_Active + "', '" + obj_M_Jurisdiction.Jurisdiction_LokSabhaId + "', '" + obj_M_Jurisdiction.Jurisdiction_VidhanSabhaId + "');Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_M_Jurisdiction(M_Jurisdiction obj_M_Jurisdiction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  M_Jurisdiction set   Jurisdiction_Name_Eng = N'" + obj_M_Jurisdiction.Jurisdiction_Name_Eng + "', M_Level_Id = '" + obj_M_Jurisdiction.M_Level_Id + "', Parent_Jurisdiction_Id = '" + obj_M_Jurisdiction.Parent_Jurisdiction_Id + "', Jurisdiction_Code = '" + obj_M_Jurisdiction.Jurisdiction_Code + "', M_Jurisdiction_ModifiedOn = getDate(), M_Jurisdiction_ModifiedBy = '" + obj_M_Jurisdiction.Created_By + "', Jurisdiction_LokSabhaId = '" + obj_M_Jurisdiction.Jurisdiction_LokSabhaId + "', Jurisdiction_VidhanSabhaId = '" + obj_M_Jurisdiction.Jurisdiction_VidhanSabhaId + "'  where M_Jurisdiction_Id = '" + obj_M_Jurisdiction.M_Jurisdiction_Id + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Jurisdiction(int m_Jurisdiction_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  M_Jurisdiction set   Is_Active = 0 where M_Jurisdiction_Id = '" + m_Jurisdiction_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public M_Jurisdiction_Detailed get_M_Jurisdiction_Detailed(string M_Level_Id, string M_Jurisdiction_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        M_Jurisdiction_Detailed obj = new M_Jurisdiction_Detailed();
        obj.M_Level_Id = Convert.ToInt32(M_Level_Id);
        if (M_Jurisdiction_Id == "0")
        {

        }
        else if (M_Level_Id == "1")
        {//State
            strQuery = @"Select	                        
	                        State_Id = M_Jurisdiction_Id,
	                        State_Name = Jurisdiction_Name_Eng
                        From  M_Jurisdiction                         
                        Where M_Level_Id='" + M_Level_Id + "' And M_Jurisdiction_Id ='" + M_Jurisdiction_Id + "'";
            ds = ExecuteSelectQuery(strQuery);
            obj.State_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["State_Id"].ToString());
            obj.State_Name = ds.Tables[0].Rows[0]["State_Name"].ToString();
        }
        else if (M_Level_Id == "2")
        {//Mandal
            strQuery = @"Select
	                        Mandal_Id = Mandal.M_Jurisdiction_Id,
	                        Mandal_Name = Mandal.Jurisdiction_Name_Eng,
	                        State_Id = States.M_Jurisdiction_Id,
	                        State_Name = States.Jurisdiction_Name_Eng
                        From  M_Jurisdiction Mandal
                        Inner Join M_Jurisdiction States On States.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id
                        Where District.M_Level_Id='" + M_Level_Id + "' And Mandal.M_Jurisdiction_Id='" + M_Jurisdiction_Id + "'";
            ds = ExecuteSelectQuery(strQuery);
            if (AllClasses.CheckDataSet(ds))
            {
                obj.State_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["State_Id"].ToString());
                obj.State_Name = ds.Tables[0].Rows[0]["State_Name"].ToString();
                obj.Mandal_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Mandal_Id"].ToString());
                obj.Mandal_Name = ds.Tables[0].Rows[0]["Mandal_Name"].ToString();
            }
        }
        else if (M_Level_Id == "3")
        {//District
            strQuery = @"Select
	                        District_Id = District.M_Jurisdiction_Id,
	                        District_Name = District.Jurisdiction_Name_Eng,
	                        Mandal_Id = Mandal.M_Jurisdiction_Id,
	                        Mandal_Name = Mandal.Jurisdiction_Name_Eng,
	                        State_Id = States.M_Jurisdiction_Id,
	                        State_Name = States.Jurisdiction_Name_Eng
                        From  M_Jurisdiction District
                        Inner Join M_Jurisdiction Mandal On Mandal.M_Jurisdiction_Id = District.Parent_Jurisdiction_Id
                        Inner Join M_Jurisdiction States On States.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id
                        Where District.M_Level_Id='" + M_Level_Id + "' And District.M_Jurisdiction_Id='" + M_Jurisdiction_Id + "'";
            ds = ExecuteSelectQuery(strQuery);
            if (AllClasses.CheckDataSet(ds))
            {
                obj.State_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["State_Id"].ToString());
                obj.State_Name = ds.Tables[0].Rows[0]["State_Name"].ToString();
                obj.Mandal_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Mandal_Id"].ToString());
                obj.Mandal_Name = ds.Tables[0].Rows[0]["Mandal_Name"].ToString();
                obj.District_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["District_Id"].ToString());
                obj.District_Name = ds.Tables[0].Rows[0]["District_Name"].ToString();
            }
        }
        else if (M_Level_Id == "4")
        {//Block
            strQuery = @"Select
	                        Block_Id = Block.M_Jurisdiction_Id,
	                        Block_Name = Block.Jurisdiction_Name_Eng,
	                        District_Id = District.M_Jurisdiction_Id,
	                        District_Name = District.Jurisdiction_Name_Eng,
	                        Mandal_Id = Mandal.M_Jurisdiction_Id,
	                        Mandal_Name = Mandal.Jurisdiction_Name_Eng,
	                        State_Id = States.M_Jurisdiction_Id,
	                        State_Name = States.Jurisdiction_Name_Eng
                        From  M_Jurisdiction Block
                        Inner Join M_Jurisdiction District On District.M_Jurisdiction_Id = Block.Parent_Jurisdiction_Id
                        Inner Join M_Jurisdiction Mandal On Mandal.M_Jurisdiction_Id = District.Parent_Jurisdiction_Id
                        Inner Join M_Jurisdiction States On States.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id
                        Where District.M_Level_Id='" + M_Level_Id + "' And District.M_Jurisdiction_Id='" + M_Jurisdiction_Id + "'";
            ds = ExecuteSelectQuery(strQuery);
            if (AllClasses.CheckDataSet(ds))
            {
                obj.State_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["State_Id"].ToString());
                obj.State_Name = ds.Tables[0].Rows[0]["State_Name"].ToString();
                obj.Mandal_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Mandal_Id"].ToString());
                obj.Mandal_Name = ds.Tables[0].Rows[0]["Mandal_Name"].ToString();
                obj.District_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["District_Id"].ToString());
                obj.District_Name = ds.Tables[0].Rows[0]["District_Name"].ToString();
                obj.Block_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Block_Id"].ToString());
                obj.Block_Name = ds.Tables[0].Rows[0]["Block_Name"].ToString();
            }
        }
        return obj;
    }
    #endregion

    #region Master Designation
    public DataSet get_tbl_Designation()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Designation_Id, 
                        Designation_DesignationName, 
                        Designation_AddedBy, 
                        Designation_AddedOn, 
                        Designation_Status, 
                        Designation_Level, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Designation_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Modified_Date = Designation_ModifiedOn
                    from tbl_Designation
                    left join tbl_PersonDetail on Person_Id = Designation_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Designation_ModifiedBy
                    where Designation_Status = 1 order by Designation_DesignationName";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_Designation(tbl_Designation obj_tbl_Designation, int Designation_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDesignation(obj_tbl_Designation.Designation_DesignationName, Designation_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Designation_Id == 0)
                {
                    Insert_tbl_Designation(obj_tbl_Designation, trans, cn);
                }
                else
                {
                    obj_tbl_Designation.Designation_Id = Designation_Id;
                    Update_tbl_Designation(obj_tbl_Designation, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDesignation(string DesignationName, string Designation_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Designation  where Designation_Status = 1 and  Designation_DesignationName = '" + DesignationName + "' ";
        if (Designation_Id != "0")
        {
            strQuery += " AND Designation_Id  <> '" + Designation_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private int CheckAlreadyDesignation(string DesignationName, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Designation  where Designation_Status = 1 and  Designation_DesignationName = '" + DesignationName + "' ";

        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        if (AllClasses.CheckDataSet(ds))
        {
            return Convert.ToInt32(ds.Tables[0].Rows[0]["Designation_Id"].ToString());
        }
        else
        {
            return 0;
        }
    }

    private string Insert_tbl_Designation(tbl_Designation obj_tbl_Designation, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Designation( [Designation_AddedBy],[Designation_AddedOn],[Designation_DesignationName],[Designation_Status], [Designation_Level]) values('" + obj_tbl_Designation.Designation_AddedBy + "', getdate(), N'" + obj_tbl_Designation.Designation_DesignationName + "','" + obj_tbl_Designation.Designation_Status + "', '" + obj_tbl_Designation.Designation_Level + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Designation(tbl_Designation obj_tbl_Designation, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Designation set  Designation_DesignationName = N'" + obj_tbl_Designation.Designation_DesignationName + "', Designation_Level = '" + obj_tbl_Designation.Designation_Level + "', Designation_ModifiedOn =getDate(), Designation_ModifiedBy = '" + obj_tbl_Designation.Designation_AddedBy + "' where Designation_Id = '" + obj_tbl_Designation.Designation_Id + "' and Designation_Status = '" + obj_tbl_Designation.Designation_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Designation(int Designation_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Designation set   Designation_Status = 0 where Designation_Id = '" + Designation_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Department
    public DataSet get_tbl_Department()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; select Department_Id, Department_Name, Department_AddedOn, Department_AddedBy, Department_Status, isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, Created_Date = Department_AddedOn,tbl_PersonDetail1.Person_Name as ModifyBy, Modify_Date = Department_ModifiedOn 
          from tbl_Department
          left join tbl_PersonDetail on Person_Id = Department_AddedBy
          left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Department_ModifiedBy
          where Department_Status = 1 order by Department_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet Edit_tbl_Department(string Department_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " select Department_Id,Department_Name from tbl_Department where Department_Status=1 and Department_Id='" + Department_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_Department(tbl_Department obj_tbl_Department, int Department_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDepartment(obj_tbl_Department.Department_Name, Department_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Department_Id == 0)
                {
                    Insert_tbl_Department(obj_tbl_Department, trans, cn);
                }
                else
                {
                    obj_tbl_Department.Department_Id = Department_Id;
                    Update_tbl_Department(obj_tbl_Department, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDepartment(string DepartmentName, string Department_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Department  where Department_Status = 1 and  Department_Name = '" + DepartmentName + "' ";
        if (Department_Id != "0")
        {
            strQuery += " AND Department_Id  <> '" + Department_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Department(tbl_Department obj_tbl_Department, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Department( [Department_AddedBy],[Department_AddedOn],[Department_Name],[Department_Status] ) values('" + obj_tbl_Department.Department_AddedBy + "', getdate(), N'" + obj_tbl_Department.Department_Name + "','" + obj_tbl_Department.Department_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Department(tbl_Department obj_tbl_Department, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Department set  Department_Name = N'" + obj_tbl_Department.Department_Name + "',Department_ModifiedOn=getdate(),Department_ModifiedBy = '" + obj_tbl_Department.Department_AddedBy + "' where Department_Id = '" + obj_tbl_Department.Department_Id + "' and Department_Status = '" + obj_tbl_Department.Department_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Department(int department_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Department set   Department_Status = 0 where Department_Id = '" + department_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Role
    public DataSet get_tbl_Role()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Role_Id, 
                        Role_Name, 
                        Role_AddedOn, 
                        Role_AddedBy, 
                        Role_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Role_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Mdified_Date=Role_ModifiedOn 
                    from tbl_Role 
                    left join tbl_PersonDetail on Person_Id = Role_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = Role_ModifiedBy 
                    where Role_Status = 1 order by Role_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Role(tbl_Role obj_tbl_Role, int Role_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyRole(obj_tbl_Role.Role_Name, Role_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Role_Id == 0)
                {
                    Insert_tbl_Role(obj_tbl_Role, trans, cn);
                }
                else
                {
                    obj_tbl_Role.Role_Id = Role_Id;
                    Update_tbl_Role(obj_tbl_Role, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyRole(string RoleName, string Role_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Role  where Role_Status = 1 and  Role_Name = '" + RoleName + "' ";
        if (Role_Id != "0")
        {
            strQuery += " AND Role_Id  <> '" + Role_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Role(tbl_Role obj_tbl_Role, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Role( [Role_AddedBy],[Role_AddedOn],[Role_Name],[Role_Status] ) values('" + obj_tbl_Role.Role_AddedBy + "', getdate(), N'" + obj_tbl_Role.Role_Name + "','" + obj_tbl_Role.Role_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Role(tbl_Role obj_tbl_Role, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Role set  Role_Name = N'" + obj_tbl_Role.Role_Name + "',Role_ModifiedOn = getDate(),Role_ModifiedBy = '" + obj_tbl_Role.Role_AddedBy + "' where Role_Id = '" + obj_tbl_Role.Role_Id + "' and Role_Status = '" + obj_tbl_Role.Role_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Role(int Role_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Role set   Role_Status = 0 where Role_Id = '" + Role_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Branch Office
    public DataSet get_Branch_Office_Details(int OfficeBranch_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = "set dateformat dmy; ";
        strQuery += @"select 
	                    tbl_OfficeBranch.OfficeBranch_Id,
	                    tbl_OfficeBranch.OfficeBranch_RegistrationNo,
	                    tbl_OfficeBranch.OfficeBranch_Name,
	                    tbl_OfficeBranch.OfficeBranch_IsHO,
	                    tbl_OfficeBranch.OfficeBranch_JurisdictionId,
	                    tbl_OfficeBranch.OfficeBranch_LL,
	                    tbl_OfficeBranch.OfficeBranch_Mobile,
	                    tbl_OfficeBranch.OfficeBranch_FullAddress,
	                    tbl_OfficeBranch.OfficeBranch_GSTIN,
	                    tbl_OfficeBranch.OfficeBranch_EmailId,
	                    tbl_OfficeBranch.OfficeBranch_WebSite,
	                    tbl_OfficeBranch.OfficeBranch_PANNumber, 
	
	                    District = District.Jurisdiction_Name_Eng, 
	                    State_Id = State.M_Jurisdiction_Id,
	                    State_Name = State.Jurisdiction_Name_Eng, 
                        FullAddress = OfficeBranch_FullAddress +','+ District.Jurisdiction_Name_Eng +',' + State.Jurisdiction_Name_Eng,
						OfficeBranch_ARV_Formula, 
                        OfficeBranch_Logo
                    from tbl_OfficeBranch 
                    left join M_Jurisdiction District on District.M_Jurisdiction_Id = OfficeBranch_JurisdictionId
                    left join M_Jurisdiction Mandal on District.Parent_Jurisdiction_Id = Mandal.M_Jurisdiction_Id
                    left join M_Jurisdiction State on State.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id
                    where OfficeBranch_Status = 1";
        if (OfficeBranch_Id != 0)
        {
            strQuery += " and tbl_OfficeBranch.OfficeBranch_Id = '" + OfficeBranch_Id + "'";
        }
        strQuery += " order by OfficeBranch_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    private int Insert_tbl_OfficeBranch(tbl_OfficeBranch obj_tbl_OfficeBranch, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_OfficeBranch ( [OfficeBranch_AddedBy],[OfficeBranch_AddedOn],[OfficeBranch_EmailId],[OfficeBranch_FullAddress],[OfficeBranch_GSTIN],[OfficeBranch_IsHO],[OfficeBranch_JurisdictionId],[OfficeBranch_LL],[OfficeBranch_LogoPath],[OfficeBranch_Mobile],[OfficeBranch_Name],[OfficeBranch_PANNumber],[OfficeBranch_RegistrationNo],[OfficeBranch_Status],[OfficeBranch_WebSite], [OfficeBranch_ARV_Formula]) values ('" + obj_tbl_OfficeBranch.OfficeBranch_AddedBy + "',getdate(),'" + obj_tbl_OfficeBranch.OfficeBranch_EmailId + "','" + obj_tbl_OfficeBranch.OfficeBranch_FullAddress + "','" + obj_tbl_OfficeBranch.OfficeBranch_GSTIN + "','" + obj_tbl_OfficeBranch.OfficeBranch_IsHO + "','" + obj_tbl_OfficeBranch.OfficeBranch_JurisdictionId + "','" + obj_tbl_OfficeBranch.OfficeBranch_LL + "','" + obj_tbl_OfficeBranch.OfficeBranch_Logo + "','" + obj_tbl_OfficeBranch.OfficeBranch_Mobile + "','" + obj_tbl_OfficeBranch.OfficeBranch_Name + "','" + obj_tbl_OfficeBranch.OfficeBranch_PANNumber + "',N'" + obj_tbl_OfficeBranch.OfficeBranch_RegistrationNo + "','" + obj_tbl_OfficeBranch.OfficeBranch_Status + "','" + obj_tbl_OfficeBranch.OfficeBranch_WebSite + "','" + obj_tbl_OfficeBranch.OfficeBranch_ARV_Formula + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }
    private void Update_tbl_OfficeBranch(tbl_OfficeBranch obj_tbl_OfficeBranch, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_OfficeBranch set  OfficeBranch_EmailId = '" + obj_tbl_OfficeBranch.OfficeBranch_EmailId + "', OfficeBranch_RegistrationNo = N'" + obj_tbl_OfficeBranch.OfficeBranch_RegistrationNo + "' , OfficeBranch_FullAddress = '" + obj_tbl_OfficeBranch.OfficeBranch_FullAddress + "' , OfficeBranch_GSTIN = '" + obj_tbl_OfficeBranch.OfficeBranch_GSTIN + "' , OfficeBranch_IsHO = '" + obj_tbl_OfficeBranch.OfficeBranch_IsHO + "' , OfficeBranch_JurisdictionId = '" + obj_tbl_OfficeBranch.OfficeBranch_JurisdictionId + "', OfficeBranch_LL = '" + obj_tbl_OfficeBranch.OfficeBranch_LL + "' , OfficeBranch_Name = '" + obj_tbl_OfficeBranch.OfficeBranch_Name + "' , OfficeBranch_PANNumber = '" + obj_tbl_OfficeBranch.OfficeBranch_PANNumber + "', OfficeBranch_WebSite = '" + obj_tbl_OfficeBranch.OfficeBranch_WebSite + "', OfficeBranch_ARV_Formula = '" + obj_tbl_OfficeBranch.OfficeBranch_ARV_Formula + "', OfficeBranch_Logo = '" + obj_tbl_OfficeBranch.OfficeBranch_Logo + "' where OfficeBranch_Id = '" + obj_tbl_OfficeBranch.OfficeBranch_Id + "' and OfficeBranch_Status = '" + obj_tbl_OfficeBranch.OfficeBranch_Status + "' ";

        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    public bool Insert_Branch_Office(tbl_OfficeBranch obj_OfficeBranch)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string fileName = DateTime.Now.Ticks.ToString("x") + ".jpg";
                obj_OfficeBranch.OfficeBranch_Logo = "\\Downloads\\Logo\\" + fileName;
                if (obj_OfficeBranch.OfficeBranch_Logo_Bytes != null && obj_OfficeBranch.OfficeBranch_Logo_Bytes.Length > 0)
                {
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Logo\\" + fileName, obj_OfficeBranch.OfficeBranch_Logo_Bytes);
                }
                if (obj_OfficeBranch.OfficeBranch_Id == 0)
                {
                    obj_OfficeBranch.OfficeBranch_Id = Insert_tbl_OfficeBranch(obj_OfficeBranch, trans, cn);
                }
                else
                {
                    Update_tbl_OfficeBranch(obj_OfficeBranch, trans, cn);
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    #endregion

    #region Person Details
    public DataSet get_tbl_UserType(string UserType_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select UserType_Id, UserType_isInternal, UserType_Desc_E, UserType_Status from tbl_UserType where UserType_Status = 1 ";
        if (UserType_Id != "")
        {
            strQuery += " and UserType_Id in (" + UserType_Id + ") ";
        }
        strQuery += " order by UserType_Id ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_Employee(string UserType_Id, int State_Id, int Mandal_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, int Department_Id, string Designation_Id_In, string MobileNo, int ULB_Id, int Parent_Person_Id, int Project_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    tbl_PersonDetail.Person_Id,
	                    tbl_PersonDetail.Person_Name,
	                    tbl_PersonDetail.Person_FName,
	                    tbl_PersonDetail.Person_Mobile1,
	                    tbl_PersonDetail.Person_Mobile2,
	                    tbl_PersonDetail.Person_TelePhone,
	                    tbl_PersonDetail.Person_EmailId,
	                    tbl_PersonDetail.Person_AddressLine1,
	                    tbl_PersonDetail.Person_AddressLine2,
                        tbl_PersonDetail.Person_BranchOffice_Id, 
	                    tbl_PersonDetail.Person_Status, 
                        
                        Person_Name_Mobile = tbl_PersonDetail.Person_Name + ' (' + isnull(Designation_DesignationName, '') + ') - ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                    tbl_PersonJuridiction.PersonJuridiction_Id,
	                    tbl_PersonJuridiction.M_Level_Id,
	                    tbl_PersonJuridiction.M_Jurisdiction_Id,
	                    tbl_PersonJuridiction.PersonJuridiction_DesignationId,
	                    tbl_PersonJuridiction.PersonJuridiction_DepartmentId,
	                    tbl_PersonJuridiction.PersonJuridiction_UserTypeId,
	                    tbl_PersonJuridiction.PersonJuridiction_ParentPerson_Id,
	                    tbl_PersonJuridiction.PersonJuridiction_GSTIN,
                        tbl_PersonJuridiction.PersonJuridiction_PAN,
                        tbl_PersonJuridiction.PersonJuridiction_Firm_Name, 
                        tbl_PersonJuridiction.PersonJuridiction_ZoneId, 
                        tbl_PersonJuridiction.PersonJuridiction_CircleId, 
                        tbl_PersonJuridiction.PersonJuridiction_DivisionId, 
                        tbl_PersonJuridiction.PersonJuridiction_ULB_Id, 
                        tbl_PersonJuridiction.PersonJuridiction_EmpType,
                        tbl_PersonJuridiction.PersonJuridiction_Project_Id,
                        Department_Name,
	                    Designation_DesignationName,
	                    Level_Name,
                        UserType_Id,
	                    UserType_Desc_E,
	                    Reporting_Manager.Person_Name Reporting_Manager_Name,
                        Login_UserName, 

                        District.Jurisdiction_Name_Eng District, 
                        States.Jurisdiction_Name_Eng State, 
                        District.M_Jurisdiction_Id District_Id,  
                        States.M_Jurisdiction_Id State_Id, 
                        Zone_Name, 
                        Circle_Name, 
                        Division_Name,
                        Zone_Id, 
                        Circle_Id, 
                        Division_Id,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_BloodGroup,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EnableBiometrics,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_IdOnBiometrics,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonName,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonMobile,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftIn,		
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftOut,		
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_DOJ, 103) PersonAdditionalInfo_DOJ,
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_Birthday, 103) PersonAdditionalInfo_Birthday,
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_Aniversery, 103) PersonAdditionalInfo_Aniversery,
                        
                        tbl_PersonRoleInfo.List_Role,

	                    isnull(CreatedBy.Person_Name, 'Backend Entry') CreatedBy, 
	                    Created_Date = tbl_PersonDetail.Person_AddedOn,
                        
                        tbl_PersonDetail.Person_ProfilePIC2, 
                        tbl_PersonDetail.Person_ProfilePIC, 
	                    ULB_Id, 
	                    ULB_Name, 
                 
                        ULBAccount_BranchName,
                        ULBAccount_IFSCCode,
                        ULBAccount_AccountNo,
                        ULBAccount_BranchAddress,
                        ULBAccount_MICRCode,
                        ULBAccount_BankId, 
                        Bank_Name,
                        List_AdditionZone
                    from tbl_PersonDetail 
                    join (select Row_Number() over (partition by PersonJuridiction_PersonId order by PersonJuridiction_Id desc) rrJ, * from tbl_PersonJuridiction where PersonJuridiction_Status = 1) tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId and tbl_PersonJuridiction.rrJ = 1
                    left join tbl_Department on Department_Id = PersonJuridiction_DepartmentId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join M_Level on M_Level.M_Level_Id = tbl_PersonJuridiction.M_Level_Id
                    left Join M_Jurisdiction District On District.M_Jurisdiction_Id = tbl_PersonJuridiction.M_Jurisdiction_Id
                    left Join M_Jurisdiction Mandal On Mandal.M_Jurisdiction_Id = District.Parent_Jurisdiction_Id
                    left Join M_Jurisdiction States On States.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id                    
                    left join tbl_ULB on ULB_Id = tbl_PersonJuridiction.PersonJuridiction_ULB_Id
                    left join tbl_Division on Division_Id = PersonJuridiction_DivisionId
					left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
                    left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
                    left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    left join tbl_PersonDetail Reporting_Manager on Reporting_Manager.Person_Id = PersonJuridiction_ParentPerson_Id
                    left join tbl_UserLogin on Login_PersonId = tbl_PersonDetail.Person_Id and Login_Status = 1
                    left join tbl_PersonAdditionalInfo on PersonAdditionalInfo_PersonId = tbl_PersonDetail.Person_Id and PersonAdditionalInfo_Status = 1
                    left join tbl_PersonDetail CreatedBy on CreatedBy.Person_Id = tbl_PersonDetail.Person_AddedBy
                    left join tbl_ULBAccountDtls on ULBAccount_ULB_Id = tbl_PersonDetail.Person_Id and tbl_ULBAccountDtls.ULBAccount_Status = 1 and isnull(ULBAccount_SchemeId, 0) = 0
                    left join tbl_Bank on Bank_Id = ULBAccount_BankId
                    left join 
                    (
	                    SELECT	PersonRoleInfo_PersonId,
			                    STUFF((SELECT ', ' + CAST(PersonRoleInfo_RoleId AS VARCHAR(100)) [text()]
                                FROM tbl_PersonRoleInfo 
                                WHERE PersonRoleInfo_PersonId = t.PersonRoleInfo_PersonId and tbl_PersonRoleInfo.PersonRoleInfo_Status = 1
                                FOR XML PATH(''), TYPE)
                            .value('.','NVARCHAR(MAX)'),1,2,' ') List_Role
	                    FROM tbl_PersonRoleInfo t
                        where t.PersonRoleInfo_Status = 1
	                    GROUP BY PersonRoleInfo_PersonId
                    ) tbl_PersonRoleInfo on tbl_PersonRoleInfo.PersonRoleInfo_PersonId = tbl_PersonDetail.Person_Id
                left join (
                SELECT	PersonAdditionalCharge_PersonId,
			                                        STUFF((SELECT ', ' + CAST(PersonAdditionalCharge_Jurisdiction_Id AS VARCHAR(100)) [text()]
                                                    FROM tbl_PersonAdditionalCharge
									                WHERE PersonAdditionalCharge_PersonId = t.PersonAdditionalCharge_PersonId and tbl_PersonAdditionalCharge.PersonAdditionalCharge_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_AdditionZone
	                                        FROM tbl_PersonAdditionalCharge t
                                            where t.PersonAdditionalCharge_Status = 1
	                                        GROUP BY PersonAdditionalCharge_PersonId
							                ) tbl_PersonAdditionalCharge on tbl_PersonAdditionalCharge.PersonAdditionalCharge_PersonId=tbl_PersonDetail.Person_Id
                    where 1=1 and tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 ";
        if (UserType_Id != "")
        {
            strQuery += " and PersonJuridiction_UserTypeId in (" + UserType_Id + ")";
        }
        if (State_Id != 0)
        {
            strQuery += " and States.M_Jurisdiction_Id = '" + State_Id + "'";
        }
        if (Mandal_Id != 0)
        {
            strQuery += " and Mandal.M_Jurisdiction_Id = '" + Mandal_Id + "'";
        }
        if (District_Id != 0)
        {
            strQuery += " and District.M_Jurisdiction_Id = '" + District_Id + "'";
        }
        if (Zone_Id != 0)
        {
            if (Zone_Id == -1)
            {
                strQuery += " and isnull(PersonJuridiction_ZoneId, 0) = 0";
            }
            else
            {
                strQuery += " and PersonJuridiction_ZoneId = '" + Zone_Id + "'";
            }
        }
        if (Circle_Id != 0)
        {
            strQuery += " and PersonJuridiction_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            strQuery += " and PersonJuridiction_DivisionId = '" + Division_Id + "'";
        }
        if (Department_Id != 0)
        {
            strQuery += " and PersonJuridiction_DepartmentId = '" + Department_Id + "'";
        }
        if (Designation_Id_In != "" && Designation_Id_In != "0")
        {
            strQuery += " and PersonJuridiction_DesignationId in (" + Designation_Id_In + ")";
        }
        if (ULB_Id != 0)
        {
            strQuery += " and PersonJuridiction_ULB_Id = '" + ULB_Id + "'";
        }
        if (Parent_Person_Id != 0)
        {
            strQuery += " and PersonJuridiction_ParentPerson_Id = '" + Parent_Person_Id + "'";
        }
        if (Project_Id != 0)
        {
            strQuery += " and PersonJuridiction_Project_Id = '" + Project_Id + "'";
        }
        if (MobileNo != "")
        {
            strQuery += " and (tbl_PersonDetail.Person_Mobile1 like '%" + MobileNo + "%' or tbl_PersonDetail.Person_Mobile2 like '%" + MobileNo + "%' or tbl_PersonDetail.Person_Name like '%" + MobileNo + "%') ";
        }
        strQuery += " order by States.Jurisdiction_Name_Eng, District.Jurisdiction_Name_Eng, tbl_PersonDetail.Person_Name ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_PersonAdditionalArea_Edit(int Person_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery += @" select 
                            PersonAdditionalArea_Designation_Id,
                            Designation_Name=Designation_DesignationName,
                            PersonAdditionalArea_ZoneId,
                            Zone_Name,
                            PersonAdditionalArea_CircleId,
                            Circle_Name,
                            PersonAdditionalArea_DivisionId,
                            Division_Name 
                        from tbl_PersonAdditionalArea
                        inner join tbl_Zone on Zone_Id=PersonAdditionalArea_ZoneId
                        inner join tbl_Circle on Circle_Id=PersonAdditionalArea_CircleId
                        inner join tbl_Division on Division_Id=PersonAdditionalArea_DivisionId
                        inner join tbl_Designation on Designation_Id=PersonAdditionalArea_Designation_Id
                        where PersonAdditionalArea_Person_Id='" + Person_Id + "' and PersonAdditionalArea_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_Employee_BySetInspection(string UserType_Id, int Zone_Id, int Circle_Id, int Division_Id, string SetName)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    tbl_PersonDetail.Person_Id,
	                    tbl_PersonDetail.Person_Name,
	                    tbl_PersonDetail.Person_FName,
	                    tbl_PersonDetail.Person_Mobile1,
	                    tbl_PersonDetail.Person_Mobile2,
                        tbl_PersonDetail.Person_BranchOffice_Id, 
	                    Designation_Id=tbl_PersonJuridiction.PersonJuridiction_DesignationId,
	                    Designation_Name=Designation_DesignationName,
	                    Zone_Name, 
                        Circle_Name, 
                        Division_Name,
                        Zone_Id, 
                        Circle_Id, 
                        Division_Id
                    from tbl_PersonDetail 
                    join (select Row_Number() over (partition by PersonJuridiction_PersonId order by PersonJuridiction_Id desc) rrJ, * from tbl_PersonJuridiction where PersonJuridiction_Status = 1) tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId and tbl_PersonJuridiction.rrJ = 1
                    inner join (
                select ROW_NUMBER() Over(Partition by SetInspectionMaster_Name,SetInspectionMaster_Designation_Id order by SetInspectionMaster_Id desc) as rr,SetInspectionMaster_Name, SetInspectionMaster_Designation_Id,SetInspectionMaster_Id from tbl_SetInspectionMaster  where SetInspectionMaster_Status = 1 
                ) as tbl_SetInspectionMaster on SetInspectionMaster_Designation_Id=PersonJuridiction_DesignationId and  rr=1
                    inner join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join tbl_Division on Division_Id = PersonJuridiction_DivisionId
					left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
                    left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
                    where 1=1 and tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 ";
        if (UserType_Id != "")
        {
            strQuery += " and PersonJuridiction_UserTypeId in (" + UserType_Id + ")";
        }

        if (Zone_Id != 0)
        {
            if (Zone_Id == -1)
            {
                strQuery += " and isnull(PersonJuridiction_ZoneId, 0) = 0";
            }
            else
            {
                strQuery += " and PersonJuridiction_ZoneId = '" + Zone_Id + "'";
            }
        }
        if (Circle_Id != 0)
        {
            strQuery += " and PersonJuridiction_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            strQuery += " and PersonJuridiction_DivisionId = '" + Division_Id + "'";
        }

        if (SetName != "")
        {
            strQuery += " and SetInspectionMaster_Name = '" + SetName + "'";
        }
        strQuery += " order by tbl_PersonDetail.Person_Name  ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_Employee_Contractor(int District_Id, int Zone_Id, int Circle_Id, int Division_Id, string MobileNo, bool ShowLinkedContractors)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    tbl_PersonDetail.Person_Id,
	                    tbl_PersonDetail.Person_Name,
	                    tbl_PersonDetail.Person_FName,
	                    tbl_PersonDetail.Person_Mobile1,
	                    tbl_PersonDetail.Person_Mobile2,
	                    tbl_PersonDetail.Person_TelePhone,
	                    tbl_PersonDetail.Person_EmailId,
	                    tbl_PersonDetail.Person_AddressLine1,
	                    tbl_PersonDetail.Person_AddressLine2,
                        tbl_PersonDetail.Person_BranchOffice_Id, 
	                    tbl_PersonDetail.Person_Status, 
                        
                        Person_Name_Mobile = tbl_PersonDetail.Person_Name + ' (' + isnull(Designation_DesignationName, '') + ') - ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                    tbl_PersonJuridiction.PersonJuridiction_Id,
	                    tbl_PersonJuridiction.M_Level_Id,
	                    tbl_PersonJuridiction.M_Jurisdiction_Id,
	                    tbl_PersonJuridiction.PersonJuridiction_DesignationId,
	                    tbl_PersonJuridiction.PersonJuridiction_DepartmentId,
	                    tbl_PersonJuridiction.PersonJuridiction_UserTypeId,
	                    tbl_PersonJuridiction.PersonJuridiction_ParentPerson_Id,
	                    tbl_PersonJuridiction.PersonJuridiction_GSTIN,
                        tbl_PersonJuridiction.PersonJuridiction_PAN,
                        tbl_PersonJuridiction.PersonJuridiction_Firm_Name, 
                        tbl_PersonJuridiction.PersonJuridiction_ZoneId, 
                        tbl_PersonJuridiction.PersonJuridiction_CircleId, 
                        tbl_PersonJuridiction.PersonJuridiction_DivisionId, 
                        tbl_PersonJuridiction.PersonJuridiction_ULB_Id, 
                        tbl_PersonJuridiction.PersonJuridiction_EmpType,
                        Department_Name,
	                    Designation_DesignationName,
	                    Level_Name,
                        UserType_Id,
	                    UserType_Desc_E,
	                    Reporting_Manager.Person_Name Reporting_Manager_Name,
                        Login_UserName, 

                        District.Jurisdiction_Name_Eng District, 
                        States.Jurisdiction_Name_Eng State, 
                        District.M_Jurisdiction_Id District_Id,  
                        States.M_Jurisdiction_Id State_Id, 
                        Zone_Name, 
                        Circle_Name, 
                        Division_Name,
                        Zone_Id, 
                        Circle_Id, 
                        Division_Id,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_BloodGroup,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EnableBiometrics,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_IdOnBiometrics,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonName,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonMobile,
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftIn,		
                        tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftOut,		
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_DOJ, 103) PersonAdditionalInfo_DOJ,
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_Birthday, 103) PersonAdditionalInfo_Birthday,
                        convert(char(10), tbl_PersonAdditionalInfo.PersonAdditionalInfo_Aniversery, 103) PersonAdditionalInfo_Aniversery,
                        
                        tbl_PersonRoleInfo.List_Role,

	                    isnull(CreatedBy.Person_Name, 'Backend Entry') CreatedBy, 
	                    Created_Date = tbl_PersonDetail.Person_AddedOn,
                        
                        tbl_PersonDetail.Person_ProfilePIC2, 
                        tbl_PersonDetail.Person_ProfilePIC, 
	                    ULB_Id, 
	                    ULB_Name, 
                 
                        ULBAccount_BranchName,
                        ULBAccount_IFSCCode,
                        ULBAccount_AccountNo,
                        ULBAccount_BranchAddress,
                        ULBAccount_MICRCode,
                        ULBAccount_BankId, 
                        Bank_Name
                    from tbl_PersonDetail 
                    join (select Row_Number() over (partition by PersonJuridiction_PersonId order by PersonJuridiction_Id desc) rrJ, * from tbl_PersonJuridiction where PersonJuridiction_Status = 1) tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId and tbl_PersonJuridiction.rrJ = 1
                    left join tbl_Department on Department_Id = PersonJuridiction_DepartmentId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join M_Level on M_Level.M_Level_Id = tbl_PersonJuridiction.M_Level_Id
                    left Join M_Jurisdiction District On District.M_Jurisdiction_Id = tbl_PersonJuridiction.M_Jurisdiction_Id
                    left Join M_Jurisdiction Mandal On Mandal.M_Jurisdiction_Id = District.Parent_Jurisdiction_Id
                    left Join M_Jurisdiction States On States.M_Jurisdiction_Id = Mandal.Parent_Jurisdiction_Id                    
                    left join tbl_ULB on ULB_Id = tbl_PersonJuridiction.PersonJuridiction_ULB_Id
                    left join tbl_Division on Division_Id = PersonJuridiction_DivisionId
					left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
                    left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
                    left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    left join tbl_PersonDetail Reporting_Manager on Reporting_Manager.Person_Id = PersonJuridiction_ParentPerson_Id
                    left join tbl_UserLogin on Login_PersonId = tbl_PersonDetail.Person_Id and Login_Status = 1
                    left join tbl_PersonAdditionalInfo on PersonAdditionalInfo_PersonId = tbl_PersonDetail.Person_Id and PersonAdditionalInfo_Status = 1
                    left join tbl_PersonDetail CreatedBy on CreatedBy.Person_Id = tbl_PersonDetail.Person_AddedBy
                    left join tbl_ULBAccountDtls on ULBAccount_ULB_Id = tbl_PersonDetail.Person_Id and tbl_ULBAccountDtls.ULBAccount_Status = 1 and isnull(ULBAccount_SchemeId, 0) = 0
                    left join tbl_Bank on Bank_Id = ULBAccount_BankId
                    left join 
                    (
	                    SELECT	PersonRoleInfo_PersonId,
			                    STUFF((SELECT ', ' + CAST(PersonRoleInfo_RoleId AS VARCHAR(100)) [text()]
                                FROM tbl_PersonRoleInfo 
                                WHERE PersonRoleInfo_PersonId = t.PersonRoleInfo_PersonId and tbl_PersonRoleInfo.PersonRoleInfo_Status = 1
                                FOR XML PATH(''), TYPE)
                            .value('.','NVARCHAR(MAX)'),1,2,' ') List_Role
	                    FROM tbl_PersonRoleInfo t
                        where t.PersonRoleInfo_Status = 1
	                    GROUP BY PersonRoleInfo_PersonId
                    ) tbl_PersonRoleInfo on tbl_PersonRoleInfo.PersonRoleInfo_PersonId = tbl_PersonDetail.Person_Id
                    where 1=1 and tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and PersonJuridiction_UserTypeId = 5";
        if (District_Id != 0)
        {
            //strQuery += " and District.M_Jurisdiction_Id = '" + District_Id + "'";
            if (ShowLinkedContractors)
            {
                strQuery += " and tbl_PersonDetail.Person_Id in (select ProjectWorkPkg_Vendor_Id from tbl_ProjectWorkPkg join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id left join tbl_Division on Division_Id = ProjectWork_DivisionId left join tbl_Circle on Circle_Id = Division_CircleId where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 and ProjectWork_DistrictId = '" + District_Id + "')";
            }
        }
        if (Zone_Id != 0)
        {
            //strQuery += " and PersonJuridiction_ZoneId = '" + Zone_Id + "'";
            if (ShowLinkedContractors)
            {
                strQuery += " and tbl_PersonDetail.Person_Id in (select ProjectWorkPkg_Vendor_Id from tbl_ProjectWorkPkg join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id left join tbl_Division on Division_Id = ProjectWork_DivisionId left join tbl_Circle on Circle_Id = Division_CircleId where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 and Circle_ZoneId = '" + Zone_Id + "')";
            }
        }
        if (Circle_Id != 0)
        {
            //strQuery += " and PersonJuridiction_CircleId = '" + Circle_Id + "'";
            if (ShowLinkedContractors)
            {
                strQuery += " and tbl_PersonDetail.Person_Id in (select ProjectWorkPkg_Vendor_Id from tbl_ProjectWorkPkg join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id left join tbl_Division on Division_Id = ProjectWork_DivisionId left join tbl_Circle on Circle_Id = Division_CircleId where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 and Division_CircleId = '" + Circle_Id + "')";
            }
        }
        if (Division_Id != 0)
        {
            //strQuery += " and PersonJuridiction_DivisionId = '" + Division_Id + "'";
            if (ShowLinkedContractors)
            {
                strQuery += " and tbl_PersonDetail.Person_Id in (select ProjectWorkPkg_Vendor_Id from tbl_ProjectWorkPkg join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id left join tbl_Division on Division_Id = ProjectWork_DivisionId left join tbl_Circle on Circle_Id = Division_CircleId where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 and ProjectWork_DivisionId = '" + Division_Id + "')";
            }
        }
        if (MobileNo != "")
        {
            strQuery += " and (tbl_PersonDetail.Person_Mobile1 = '" + MobileNo + "' or tbl_PersonDetail.Person_Mobile2 = '" + MobileNo + "') ";
        }
        if (ShowLinkedContractors)
        {
            if (Session["UserType"].ToString() == "5")
            {//Vendor
                strQuery += " and tbl_PersonDetail.Person_Id = '" + Session["Person_Id"].ToString() + "'";
            }
        }
        strQuery += " order by States.Jurisdiction_Name_Eng, District.Jurisdiction_Name_Eng, tbl_PersonDetail.Person_Name ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_Employee(tbl_PersonDetail obj_PersonDetail, tbl_PersonJuridiction obj_PersonJuridiction, tbl_UserLogin obj_UserLogin, int Person_Id, tbl_PersonAdditionalInfo obj_tbl_PersonAdditionalInfo, List<tbl_PersonRoleInfo> obj_tbl_PersonRoleInfoLi, List<tbl_PersonAdditionalArea> obj_tbl_PersonAdditionalAreaLi, tbl_ULBAccountDtls obj_tbl_ULBAccountDtls, List<tbl_PersonAdditionalCharge> obj_tbl_PersonAdditionalCharge_Li, ref string errorMSG)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacy_Mobile_No(obj_PersonDetail.Person_Mobile1, obj_PersonDetail.Person_Mobile2, Person_Id, trans, cn)))
                {
                    errorMSG = "Mobile No Provided Already Exist in Database. Please Deactivate Existing Mobile No..";
                    trans.Commit();
                    cn.Close();
                    flag = true;
                    return flag;
                }
                errorMSG = "";
                if (obj_PersonDetail.Person_Id == 0)
                {
                    obj_PersonDetail.Person_Id = Insert_tbl_PersonDetail(obj_PersonDetail, trans, cn);
                }
                else
                {
                    Update_tbl_PersonDetail(obj_PersonDetail, trans, cn);
                }
                obj_PersonJuridiction.PersonJuridiction_PersonId = obj_PersonDetail.Person_Id;
                if (obj_PersonJuridiction.PersonJuridiction_Id == 0)
                {
                    Insert_tbl_PersonJuridiction(obj_PersonJuridiction, trans, cn);
                }
                else
                {
                    Update_tbl_PersonJuridiction(obj_PersonJuridiction, trans, cn);
                }

                if (Person_Id == 0 && obj_UserLogin != null)
                {
                    obj_UserLogin.Login_PersonId = obj_PersonDetail.Person_Id;
                    obj_UserLogin.Login_UserName = get_tbl_Employee_User_Name(trans, cn);
                    Insert_tbl_UserLogin(obj_UserLogin, trans, cn);
                }
                if (obj_tbl_PersonAdditionalInfo != null)
                {
                    obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_PersonId = obj_PersonDetail.Person_Id;
                    UpdateAndInsert_tbl_PersonAdditionalInfo(obj_tbl_PersonAdditionalInfo, trans, cn);
                }
                if (obj_tbl_PersonRoleInfoLi != null && obj_tbl_PersonRoleInfoLi.Count > 0)
                {
                    Update_tbl_PersonRoleInfo(obj_tbl_PersonRoleInfoLi[0], trans, cn);
                    for (int i = 0; i < obj_tbl_PersonRoleInfoLi.Count; i++)
                    {
                        obj_tbl_PersonRoleInfoLi[i].PersonRoleInfo_PersonId = obj_PersonDetail.Person_Id;
                        Insert_tbl_PersonRoleInfo(obj_tbl_PersonRoleInfoLi[i], trans, cn);
                    }
                }
                if (obj_tbl_PersonAdditionalAreaLi != null && obj_tbl_PersonAdditionalAreaLi.Count > 0)
                {
                    obj_tbl_PersonAdditionalAreaLi[0].PersonAdditionalArea_Person_Id = obj_PersonDetail.Person_Id;
                    Update_tbl_PersonAdditionalArea(obj_tbl_PersonAdditionalAreaLi[0], trans, cn);
                    for (int i = 0; i < obj_tbl_PersonAdditionalAreaLi.Count; i++)
                    {
                        obj_tbl_PersonAdditionalAreaLi[i].PersonAdditionalArea_Person_Id = obj_PersonDetail.Person_Id;
                        Insert_tbl_PersonAdditionalArea(obj_tbl_PersonAdditionalAreaLi[i], trans, cn);
                    }
                }
                if (obj_tbl_ULBAccountDtls != null)
                {
                    obj_tbl_ULBAccountDtls.ULBAccount_ULB_Id = obj_PersonDetail.Person_Id;
                    Update_tbl_ULBAccountDtls(obj_tbl_ULBAccountDtls, trans, cn);
                    Insert_tbl_ULBAccountDtls(obj_tbl_ULBAccountDtls, trans, cn);
                }

                if (obj_tbl_PersonAdditionalCharge_Li != null)
                {
                    Update_tbl_PersonAdditionalCharge(obj_PersonDetail.Person_AddedBy, obj_PersonDetail.Person_Id, trans, cn);
                    for (int i = 0; i < obj_tbl_PersonAdditionalCharge_Li.Count; i++)
                    {
                        obj_tbl_PersonAdditionalCharge_Li[i].PersonAdditionalCharge_PersonId = obj_PersonDetail.Person_Id;
                        Insert_tbl_PersonAdditionalCharge(obj_tbl_PersonAdditionalCharge_Li[i], trans, cn);
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Insert_tbl_PersonAdditionalCharge(tbl_PersonAdditionalCharge obj_tbl_PersonAdditionalCharge, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PersonAdditionalCharge ( [PersonAdditionalCharge_AddedBy],[PersonAdditionalCharge_AddedOn],[PersonAdditionalCharge_Jurisdiction_Id],[PersonAdditionalCharge_Level_Id],[PersonAdditionalCharge_PersonId],[PersonAdditionalCharge_Status] ) values ('" + obj_tbl_PersonAdditionalCharge.PersonAdditionalCharge_AddedBy + "',getdate(),'" + obj_tbl_PersonAdditionalCharge.PersonAdditionalCharge_Jurisdiction_Id + "','" + obj_tbl_PersonAdditionalCharge.PersonAdditionalCharge_Level_Id + "','" + obj_tbl_PersonAdditionalCharge.PersonAdditionalCharge_PersonId + "','" + obj_tbl_PersonAdditionalCharge.PersonAdditionalCharge_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Update_tbl_PersonAdditionalCharge(int Added_By, int PersonDetails_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;update tbl_PersonAdditionalCharge set PersonAdditionalCharge_Status = 0, PersonAdditionalCharge_ModifiedBy = '" + Added_By + "', PersonAdditionalCharge_ModifiedOn = getdate() where PersonAdditionalCharge_PersonId = '" + PersonDetails_Id + "' and PersonAdditionalCharge_Status = 1";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public string get_tbl_Employee_User_Name(SqlTransaction trans, SqlConnection cn)
    {
        int val = 0;
        DataTable dt = new DataTable();
        DataTable dtVoucher = new DataTable();
        string sql = "select count(*) from tbl_PersonDetail";

        if (trans == null)
        {
            dt = ExecuteSelectQuery(sql).Tables[0];
        }
        else
        {
            dt = ExecuteSelectQuerywithTransaction(cn, sql, trans).Tables[0];
        }
        try
        {
            if (AllClasses.CheckDt(dt))
            {
                val = Convert.ToInt32(dt.Rows[0][0].ToString()) + 1;
            }
            else
            {
                val = 1;
            }
        }
        catch (Exception)
        {
            val = 1;
        }
        return "E" + val.ToString().PadLeft(5, '0');
    }
    private void UpdateAndInsert_tbl_PersonAdditionalInfo(tbl_PersonAdditionalInfo obj_tbl_PersonAdditionalInfo, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_PersonAdditionalInfo set  PersonAdditionalInfo_Status =  '0', PersonAdditionalInfo_ModifiedBy = '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_AddedBy + "', PersonAdditionalInfo_ModifiedOn = getdate() where PersonAdditionalInfo_PersonId = '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_PersonId + "' ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        string col = "", val = "";
        if (obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_Aniversery != "")
        {
            col += ", [PersonAdditionalInfo_Aniversery]";
            val += ", '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_Aniversery + "'";
        }
        if (obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_DOJ != "")
        {
            col += ", [PersonAdditionalInfo_DOJ]";
            val += ", '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_DOJ + "'";
        }
        if (obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_Birthday != "")
        {
            col += ", [PersonAdditionalInfo_Birthday]";
            val += ", '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_Birthday + "'";
        }
        if (obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftIn != "")
        {
            col += ", [PersonAdditionalInfo_ShiftIn]";
            val += ", convert(datetime, '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftIn + "', 103)";
        }
        if (obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftOut != "")
        {
            col += ", [PersonAdditionalInfo_ShiftOut]";
            val += ", convert(datetime, '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_ShiftOut + "', 103)";
        }

        strQuery = " set dateformat dmy; insert into tbl_PersonAdditionalInfo ([PersonAdditionalInfo_AddedBy],[PersonAdditionalInfo_AddedOn],[PersonAdditionalInfo_BloodGroup],[PersonAdditionalInfo_EmergencyContactPersonMobile],[PersonAdditionalInfo_EmergencyContactPersonName],[PersonAdditionalInfo_EnableBiometrics],[PersonAdditionalInfo_IdOnBiometrics],[PersonAdditionalInfo_Status], [PersonAdditionalInfo_PersonId] " + col + ") values ('" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_AddedBy + "',getdate(),'" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_BloodGroup + "','" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonMobile + "','" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_EmergencyContactPersonName + "','" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_EnableBiometrics + "','" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_IdOnBiometrics + "','" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_Status + "', '" + obj_tbl_PersonAdditionalInfo.PersonAdditionalInfo_PersonId + "' " + val + ")";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Update_tbl_PersonRoleInfo(tbl_PersonRoleInfo obj_tbl_PersonRoleInfo, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_PersonRoleInfo set  PersonRoleInfo_Status =  '0', [PersonRoleInfo_ModifiedBy] = '" + obj_tbl_PersonRoleInfo.PersonRoleInfo_ModifiedBy + "', [PersonRoleInfo_ModifiedOn] = getdate() where PersonRoleInfo_PersonId = '" + obj_tbl_PersonRoleInfo.PersonRoleInfo_PersonId + "' and PersonRoleInfo_Status =  '1' ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PersonRoleInfo(tbl_PersonRoleInfo obj_tbl_PersonRoleInfo, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";

        strQuery = " set dateformat dmy; insert into tbl_PersonRoleInfo (PersonRoleInfo_PersonId, PersonRoleInfo_RoleId, PersonRoleInfo_AddedBy, PersonRoleInfo_AddedOn,  PersonRoleInfo_Status) values ('" + obj_tbl_PersonRoleInfo.PersonRoleInfo_PersonId + "','" + obj_tbl_PersonRoleInfo.PersonRoleInfo_RoleId + "','" + obj_tbl_PersonRoleInfo.PersonRoleInfo_AddedBy + "', getdate(),'" + obj_tbl_PersonRoleInfo.PersonRoleInfo_Status + "')";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Update_tbl_PersonAdditionalArea(tbl_PersonAdditionalArea obj_tbl_PersonAdditionalArea, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_PersonAdditionalArea set  PersonAdditionalArea_Status =  '0', [PersonAdditionalArea_ModifiedBy] = '" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_ModifiedBy + "', [PersonAdditionalArea_ModifiedOn] = getdate() where PersonAdditionalArea_Person_Id = '" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_Person_Id + "' and PersonAdditionalArea_Status =  '1' ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PersonAdditionalArea(tbl_PersonAdditionalArea obj_tbl_PersonAdditionalArea, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";

        strQuery = " set dateformat dmy; insert into tbl_PersonAdditionalArea (PersonAdditionalArea_Person_Id, PersonAdditionalArea_ZoneId,PersonAdditionalArea_CircleId,PersonAdditionalArea_DivisionId,PersonAdditionalArea_Designation_Id, PersonAdditionalArea_AddedBy, PersonAdditionalArea_AddedOn,  PersonAdditionalArea_Status) values ('" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_Person_Id + "','" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_ZoneId + "','" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_CircleId + "','" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_DivisionId + "','" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_Designation_Id + "','" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_AddedBy + "', getdate(),'" + obj_tbl_PersonAdditionalArea.PersonAdditionalArea_Status + "')";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private int Insert_tbl_PersonDetail(tbl_PersonDetail obj_tbl_PersonDetail, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PersonDetail ( [Person_AddedBy],[Person_AddedOn],[Person_AddressLine1],[Person_AddressLine2],[Person_EmailId],[Person_Mobile1],[Person_Mobile2],[Person_Name],[Person_Status],[Person_TelePhone], [Person_FName], [Person_BranchOffice_Id]) values ('" + obj_tbl_PersonDetail.Person_AddedBy + "',getdate(),'" + obj_tbl_PersonDetail.Person_AddressLine1 + "','" + obj_tbl_PersonDetail.Person_AddressLine2 + "','" + obj_tbl_PersonDetail.Person_EmailId + "','" + obj_tbl_PersonDetail.Person_Mobile1 + "','" + obj_tbl_PersonDetail.Person_Mobile2 + "','" + obj_tbl_PersonDetail.Person_Name + "','" + obj_tbl_PersonDetail.Person_Status + "','" + obj_tbl_PersonDetail.Person_TelePhone + "', '" + obj_tbl_PersonDetail.Person_FName + "', '" + obj_tbl_PersonDetail.Person_BranchOffice_Id + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }

    private void Update_tbl_PersonDetail(tbl_PersonDetail obj_tbl_PersonDetail, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_PersonDetail set  Person_AddedBy = '" + obj_tbl_PersonDetail.Person_AddedBy + "' , Person_AddressLine1 = '" + obj_tbl_PersonDetail.Person_AddressLine1 + "' , Person_AddressLine2 = '" + obj_tbl_PersonDetail.Person_AddressLine2 + "' , Person_EmailId = '" + obj_tbl_PersonDetail.Person_EmailId + "' , Person_Mobile1 = '" + obj_tbl_PersonDetail.Person_Mobile1 + "' , Person_Mobile2 = '" + obj_tbl_PersonDetail.Person_Mobile2 + "' , Person_ModifiedBy = '" + obj_tbl_PersonDetail.Person_ModifiedBy + "' ,  Person_ModifiedOn =  getdate(), Person_Name = '" + obj_tbl_PersonDetail.Person_Name + "' , Person_TelePhone = '" + obj_tbl_PersonDetail.Person_TelePhone + "', Person_FName = '" + obj_tbl_PersonDetail.Person_FName + "', Person_BranchOffice_Id = '" + obj_tbl_PersonDetail.Person_BranchOffice_Id + "' where Person_Id = '" + obj_tbl_PersonDetail.Person_Id + "' and Person_Status = '" + obj_tbl_PersonDetail.Person_Status + "' ";

        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private DataSet CheckDuplicacy_GSTIN(string Current_GSTIN, int current_PersonJuridiction_PersonId, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select * from tbl_PersonJuridiction where PersonJuridiction_Status = 1 and isnull(PersonJuridiction_GSTIN, '') != '' and PersonJuridiction_GSTIN = '" + Current_GSTIN + "' ";
        if (current_PersonJuridiction_PersonId > 0)
        {
            strQuery += " and PersonJuridiction_PersonId != '" + current_PersonJuridiction_PersonId + "' ";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private DataSet CheckDuplicacy_Mobile_No(string Current_Mobile1, string Current_Mobile2, int current_Person_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        string cond = "";
        if (Current_Mobile1 != "")
        {
            cond += "'" + Current_Mobile1 + "', ";
        }

        if (Current_Mobile2 != "")
        {
            cond += "'" + Current_Mobile2 + "', ";
        }
        cond += "''";
        strQuery = @" set dateformat dmy; 
                        select * from (
                        select Person_Mobile1 Person_Mobile, Person_Id from tbl_PersonDetail where Person_Status = 1 
                        UNION ALL
                        select Person_Mobile2 Person_Mobile, Person_Id from tbl_PersonDetail where Person_Status = 1 
                        ) tMobile
                        left join tbl_PersonJuridiction on PersonJuridiction_PersonId = tMobile.Person_Id
                        left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                        where isnull(Person_Mobile, '') != '' and Person_Mobile in (" + cond + ") ";
        if (current_Person_Id > 0)
        {
            strQuery += @" and Person_Id != '" + current_Person_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }
    public DataSet get_Employee_Reporting_Manager(int Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; select 
	                    tbl_PersonDetail.Person_Id,
	                    tbl_PersonDetail.Person_Name,
                        Person_Name_Full = isnull(tbl_PersonDetail.Person_Name, '') + ', ' + isnull(Designation_DesignationName, '') + ', Mob: ' + isnull(tbl_PersonDetail.Person_Mobile1, ''),
	                    tbl_PersonDetail.Person_Mobile1,
	                    
	                    Department_Name,
	                    Designation_DesignationName,
	                    ULB_Name,
                        Level_Name,
	                    UserType_Desc_E
                    from tbl_PersonDetail 
                    join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                    left join tbl_Department on Department_Id = PersonJuridiction_DepartmentId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join M_Level on M_Level.M_Level_Id = tbl_PersonJuridiction.M_Level_Id
                    left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    left join tbl_ULB on ULB_Id = PersonJuridiction_ULB_Id
                    where tbl_PersonDetail.Person_Status = 1 and PersonJuridiction_Status = 1 and UserType_Id not in (7, 6, 5) ";
        if (Designation_Id != 0)
        {
            strQuery += " and PersonJuridiction_DesignationId in (select Designation_Id from tbl_Designation where Designation_Level < (select Designation_Level from tbl_Designation where Designation_Id = '" + Designation_Id + "'))";
        }
        strQuery += " order by Level_Name, Department_Name, Designation_DesignationName, tbl_PersonDetail.Person_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Delete_Person(int Person_Id_Delete, int person_Id)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            string sql = "";
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                sql = "update tbl_PersonDetail set Person_Status = 0, Person_ModifiedBy = '" + person_Id + "', Person_ModifiedOn = getdate() where Person_Id = '" + Person_Id_Delete + "' and Person_Status = 1";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = "update tbl_PersonJuridiction set PersonJuridiction_Status = 0, PersonJuridiction_ModifiedBy = 0, PersonJuridiction_ModifiedOn = getdate() where PersonJuridiction_PersonId = '" + Person_Id_Delete + "' and PersonJuridiction_Status = 1";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = "update tbl_UserLogin set Login_Status = 0 where Login_PersonId = '" + Person_Id_Delete + "' and Login_Status = 1";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = "update tbl_PersonAdditionalInfo set PersonAdditionalInfo_Status = 0, PersonAdditionalInfo_ModifiedBy = '" + person_Id + "', PersonAdditionalInfo_ModifiedOn = getdate() where PersonAdditionalInfo_PersonId = '" + Person_Id_Delete + "' and PersonAdditionalInfo_Status = 1";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    #endregion

    #region User Login
    private void Insert_tbl_UserLogin(tbl_UserLogin obj_tbl_UserLogin, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_UserLogin ( [Login_AddedBy],[Login_Addeddatetime],[Login_password],[Login_PersonId],[Login_Status],[Login_UserName] ) values ('" + obj_tbl_UserLogin.Login_AddedBy + "',getdate(),'" + obj_tbl_UserLogin.Login_password + "','" + obj_tbl_UserLogin.Login_PersonId + "','" + obj_tbl_UserLogin.Login_Status + "','" + obj_tbl_UserLogin.Login_UserName + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region Person Jurisdiction
    private void Insert_tbl_PersonJuridiction(tbl_PersonJuridiction obj_tbl_PersonJuridiction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PersonJuridiction ( [M_Jurisdiction_Id],[M_Level_Id],[PersonJuridiction_AddedBy],[PersonJuridiction_AddedOn],[PersonJuridiction_DepartmentId],[PersonJuridiction_DesignationId],[PersonJuridiction_ParentPerson_Id],[PersonJuridiction_PersonId],[PersonJuridiction_Status],[PersonJuridiction_UserTypeId], [PersonJuridiction_GSTIN], [PersonJuridiction_Firm_Name], [PersonJuridiction_PAN], [PersonJuridiction_DivisionId], [PersonJuridiction_ULB_Id], [PersonJuridiction_EmpType], [PersonJuridiction_ZoneId], [PersonJuridiction_CircleId],PersonJuridiction_Project_Id) values ('" + obj_tbl_PersonJuridiction.M_Jurisdiction_Id + "','" + obj_tbl_PersonJuridiction.M_Level_Id + "','" + obj_tbl_PersonJuridiction.PersonJuridiction_AddedBy + "',getdate(),'" + obj_tbl_PersonJuridiction.PersonJuridiction_DepartmentId + "','" + obj_tbl_PersonJuridiction.PersonJuridiction_DesignationId + "','" + obj_tbl_PersonJuridiction.PersonJuridiction_ParentPerson_Id + "','" + obj_tbl_PersonJuridiction.PersonJuridiction_PersonId + "','" + obj_tbl_PersonJuridiction.PersonJuridiction_Status + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_UserTypeId + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_GSTIN + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_Firm_Name + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_PAN + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_DivisionId + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_ULB_Id + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_EmpType + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_ZoneId + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_CircleId + "', '" + obj_tbl_PersonJuridiction.PersonJuridiction_Project_Id + "'); Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Update_tbl_PersonJuridiction(tbl_PersonJuridiction obj_tbl_PersonJuridiction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_PersonJuridiction set  M_Level_Id = '" + obj_tbl_PersonJuridiction.M_Level_Id + "', M_Jurisdiction_Id = '" + obj_tbl_PersonJuridiction.M_Jurisdiction_Id + "' , PersonJuridiction_DepartmentId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_DepartmentId + "' , PersonJuridiction_DesignationId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_DesignationId + "' , PersonJuridiction_ModifiedBy = '" + obj_tbl_PersonJuridiction.PersonJuridiction_ModifiedBy + "' ,  PersonJuridiction_ModifiedOn =  getdate(), PersonJuridiction_ParentPerson_Id = '" + obj_tbl_PersonJuridiction.PersonJuridiction_ParentPerson_Id + "', PersonJuridiction_UserTypeId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_UserTypeId + "', PersonJuridiction_GSTIN = '" + obj_tbl_PersonJuridiction.PersonJuridiction_GSTIN + "', PersonJuridiction_Firm_Name = '" + obj_tbl_PersonJuridiction.PersonJuridiction_Firm_Name + "', PersonJuridiction_PAN = '" + obj_tbl_PersonJuridiction.PersonJuridiction_PAN + "', PersonJuridiction_ZoneId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_ZoneId + "', PersonJuridiction_CircleId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_CircleId + "', PersonJuridiction_DivisionId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_DivisionId + "', PersonJuridiction_ULB_Id = '" + obj_tbl_PersonJuridiction.PersonJuridiction_ULB_Id + "', PersonJuridiction_EmpType = '" + obj_tbl_PersonJuridiction.PersonJuridiction_EmpType + "', PersonJuridiction_Project_Id = '" + obj_tbl_PersonJuridiction.PersonJuridiction_Project_Id + "' where PersonJuridiction_PersonId = '" + obj_tbl_PersonJuridiction.PersonJuridiction_PersonId + "' and PersonJuridiction_Status = '" + obj_tbl_PersonJuridiction.PersonJuridiction_Status + "' ";

        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion    

    #region Master Lok Sabha
    public DataSet get_tbl_LokSabha(int District_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        LokSabha_Id, 
                        LokSabha_Name, 
                        LokSabha_DistrictId, 
                        District_Name = Jurisdiction_Name_Eng, 
                        LokSabha_AddedOn, 
                        LokSabha_AddedBy, 
                        LokSabha_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = LokSabha_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = LokSabha_ModifiedOn
                      from tbl_LokSabha
                      left join M_Jurisdiction on M_Jurisdiction_Id = LokSabha_DistrictId 
                      left join tbl_PersonDetail on Person_Id = LokSabha_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = LokSabha_ModifiedBy
                      where LokSabha_Status = 1 District_IdCond order by LokSabha_Name";
        if (District_Id > 0)
        {
            strQuery = strQuery.Replace("District_IdCond", "and LokSabha_DistrictId = '" + District_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("District_IdCond", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet Edit_tbl_LokSabha(string LokSabha_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " select LokSabha_Id,LokSabha_Name, LokSabha_DistrictId from tbl_LokSabha where LokSabha_Status=1 and LokSabha_Id ='" + LokSabha_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_LokSabha(tbl_LokSabha obj_tbl_LokSabha, int LokSabha_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyLokSabha(obj_tbl_LokSabha.LokSabha_Name, LokSabha_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }

                if (LokSabha_Id == 0)
                {
                    Insert_tbl_LokSabha(obj_tbl_LokSabha, trans, cn);
                }
                else
                {
                    obj_tbl_LokSabha.LokSabha_Id = LokSabha_Id;
                    Update_tbl_LokSabha(obj_tbl_LokSabha, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyLokSabha(string LokSabhaName, string LokSabha_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_LokSabha  where LokSabha_Status = 1 and  LokSabha_Name = '" + LokSabhaName + "' ";
        if (LokSabha_Id != "0")
        {
            strQuery += " AND LokSabha_Id  <> '" + LokSabha_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_LokSabha(tbl_LokSabha obj_tbl_LokSabha, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_LokSabha( [LokSabha_AddedBy],[LokSabha_AddedOn],[LokSabha_Name],[LokSabha_Status], [LokSabha_DistrictId]) values('" + obj_tbl_LokSabha.LokSabha_AddedBy + "', getdate(), N'" + obj_tbl_LokSabha.LokSabha_Name + "','" + obj_tbl_LokSabha.LokSabha_Status + "', '" + obj_tbl_LokSabha.LokSabha_DistrictId + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_LokSabha(tbl_LokSabha obj_tbl_LokSabha, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_LokSabha set  LokSabha_Name = N'" + obj_tbl_LokSabha.LokSabha_Name + "', LokSabha_DistrictId = N'" + obj_tbl_LokSabha.LokSabha_DistrictId + "', LokSabha_ModifiedOn = getdate(), LokSabha_ModifiedBy = '" + obj_tbl_LokSabha.LokSabha_AddedBy + "' where LokSabha_Id = '" + obj_tbl_LokSabha.LokSabha_Id + "' and LokSabha_Status = '" + obj_tbl_LokSabha.LokSabha_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_LokSabha(int LokSabha_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_LokSabha set   LokSabha_Status = 0 where LokSabha_Id = '" + LokSabha_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master VidhanSabha
    public DataSet get_tbl_VidhanSabha(int LokSabha_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        VidhanSabha_Id, 
                        VidhanSabha_LokSabhaId, 
                        LokSabha_Name, 
                        VidhanSabha_Name, 
                        VidhanSabha_AddedOn, 
                        VidhanSabha_AddedBy, 
                        VidhanSabha_ModifiedOn, 
                        VidhanSabha_ModifiedBy, 
                        VidhanSabha_Status 
                    from tbl_VidhanSabha
                    join tbl_LokSabha on LokSabha_Id = VidhanSabha_LokSabhaId
                    where VidhanSabha_Status = 1";
        if (LokSabha_Id != 0)
        {
            strQuery += " and VidhanSabha_LokSabhaId = '" + LokSabha_Id + "'";
        }
        strQuery += " order by LokSabha_Name, VidhanSabha_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_VidhanSabha(tbl_VidhanSabha obj_tbl_VidhanSabha, int VidhanSabha_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyVidhanSabha(obj_tbl_VidhanSabha.VidhanSabha_Name, VidhanSabha_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (VidhanSabha_Id == 0)
                {
                    Insert_tbl_VidhanSabha(obj_tbl_VidhanSabha, trans, cn);
                }
                else
                {
                    obj_tbl_VidhanSabha.VidhanSabha_Id = VidhanSabha_Id;
                    Update_tbl_VidhanSabha(obj_tbl_VidhanSabha, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyVidhanSabha(string VidhanSabha_Name, string VidhanSabhaId, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_VidhanSabha  where VidhanSabha_Status=1 and  VidhanSabha_Name= '" + VidhanSabha_Name + "' ";
        if (VidhanSabhaId != "0")
        {
            strQuery += " AND VidhanSabha_Id <> '" + VidhanSabhaId + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_VidhanSabha(tbl_VidhanSabha obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_VidhanSabha (VidhanSabha_Name, VidhanSabha_LokSabhaId, VidhanSabha_AddedBy, VidhanSabha_AddedOn, VidhanSabha_Status) values ('" + obj_tbl_Post.VidhanSabha_Name + "', '" + obj_tbl_Post.VidhanSabha_LokSabhaId + "', '" + obj_tbl_Post.VidhanSabha_AddedBy + "',getdate(),'" + obj_tbl_Post.VidhanSabha_Status + "');Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_VidhanSabha(tbl_VidhanSabha obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_VidhanSabha set   VidhanSabha_Name = '" + obj_tbl_Post.VidhanSabha_Name + "', VidhanSabha_LokSabhaId = '" + obj_tbl_Post.VidhanSabha_LokSabhaId + "' where VidhanSabha_Id = '" + obj_tbl_Post.VidhanSabha_Id + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region tbl_ULB
    public bool Delete_ULB(int ULB_Id, int Person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ULB set ULB_Status = 0, ULB_ModifiedOn = getdate(), ULB_ModifiedBy = '" + Person_Id + "' where ULB_Id = '" + ULB_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public DataSet get_tbl_ULB(int District_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ULB_Id,
	                    ULB_District_Id,
	                    Jurisdiction_Name_Eng,
	                    ULB_Name,
	                    ULB_Type,
	                    ULB_Old_District_Id,
                        ULB_LokSabha_Id,
                        ULB_VidhanSabha_Id,
	                    ULB_AddedOn,
	                    ULB_AddedBy,
	                    ULB_ModifiedOn,
	                    ULB_ModifiedBy,
	                    ULB_Status,
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = ULB_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Mdified_Date=ULB_ModifiedOn
                    from tbl_ULB 
                    join M_Jurisdiction on M_Jurisdiction_Id = ULB_District_Id
                    left join tbl_PersonDetail on Person_Id = ULB_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = ULB_ModifiedBy 
                    where ULB_Status = 1 District_IdCond order by Jurisdiction_Name_Eng, ULB_Name ";
        if (District_Id > 0)
        {
            strQuery = strQuery.Replace("District_IdCond", "and ULB_District_Id = '" + District_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("District_IdCond", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_ULB(tbl_ULB obj_tbl_ULB)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_ULB.ULB_Id > 0)
                    Update_tbl_ULB(obj_tbl_ULB, trans, cn);
                else
                    Insert_tbl_ULB(obj_tbl_ULB, trans, cn);
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private void Insert_tbl_ULB(tbl_ULB obj_tbl_ULB, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ULB ( [ULB_AddedBy],[ULB_AddedOn],[ULB_District_Id],[ULB_Name],[ULB_Status],[ULB_Type], [ULB_LokSabha_Id], [ULB_VidhanSabha_Id]) values ('" + obj_tbl_ULB.ULB_AddedBy + "',getdate(),'" + obj_tbl_ULB.ULB_District_Id + "',N'" + obj_tbl_ULB.ULB_Name + "','" + obj_tbl_ULB.ULB_Status + "','" + obj_tbl_ULB.ULB_Type + "','" + obj_tbl_ULB.ULB_LokSabha_Id + "','" + obj_tbl_ULB.ULB_VidhanSabha_Id + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Update_tbl_ULB(tbl_ULB obj_tbl_ULB, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ULB set  ULB_District_Id = '" + obj_tbl_ULB.ULB_District_Id + "' , ULB_ModifiedBy = '" + obj_tbl_ULB.ULB_AddedBy + "' ,  ULB_ModifiedOn =  getdate(), ULB_Name = N'" + obj_tbl_ULB.ULB_Name + "' , ULB_Type = '" + obj_tbl_ULB.ULB_Type + "', ULB_LokSabha_Id = '" + obj_tbl_ULB.ULB_LokSabha_Id + "', ULB_VidhanSabha_Id = '" + obj_tbl_ULB.ULB_VidhanSabha_Id + "' where ULB_Id = '" + obj_tbl_ULB.ULB_Id + "' and ULB_Status = '" + obj_tbl_ULB.ULB_Status + "' ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    #endregion

    #region Master Bank
    public DataSet get_tbl_Bank()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Bank_Id, 
                        Bank_Name, 
                        Bank_AddedOn, 
                        Bank_AddedBy, 
                        Bank_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Bank_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Mdified_Date=Bank_ModifiedOn 
                    from tbl_Bank 
                    left join tbl_PersonDetail on Person_Id = Bank_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = Bank_ModifiedBy 
                    where Bank_Status = 1 
                    order by Bank_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Bank(tbl_Bank obj_tbl_Bank, int Bank_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyBank(obj_tbl_Bank.Bank_Name, Bank_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Bank_Id == 0)
                {
                    Insert_tbl_Bank(obj_tbl_Bank, trans, cn);
                }
                else
                {
                    obj_tbl_Bank.Bank_Id = Bank_Id;
                    Update_tbl_Bank(obj_tbl_Bank, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyBank(string BankName, string Bank_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Bank  where Bank_Status = 1 and  Bank_Name = '" + BankName + "' ";
        if (Bank_Id != "0")
        {
            strQuery += " AND Bank_Id  <> '" + Bank_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Bank(tbl_Bank obj_tbl_Bank, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Bank( [Bank_AddedBy],[Bank_AddedOn],[Bank_Name],[Bank_Status] ) values('" + obj_tbl_Bank.Bank_AddedBy + "', getdate(), N'" + obj_tbl_Bank.Bank_Name + "','" + obj_tbl_Bank.Bank_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Bank(tbl_Bank obj_tbl_Bank, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Bank set  Bank_Name = N'" + obj_tbl_Bank.Bank_Name + "',Bank_ModifiedOn = getDate(),Bank_ModifiedBy = '" + obj_tbl_Bank.Bank_AddedBy + "' where Bank_Id = '" + obj_tbl_Bank.Bank_Id + "' and Bank_Status = '" + obj_tbl_Bank.Bank_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Bank(int Bank_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Bank set   Bank_Status = 0,Bank_ModifiedBy='" + person_Id + "',Bank_ModifiedOn=getdate() where Bank_Id = '" + Bank_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Branch List
    public DataSet get_Branch_List(string IFSC_Code)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select * from Branch_List where IFSC_code = '" + IFSC_Code + "' ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region ULB Account Details
    public DataSet get_tbl_ULBAccountDtls(int District_Id, int Zone_Id, int Circle_Id, int Division_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ULBAccount_Id,
	                    ULBAccount_ULB_Id,
	                    ULBAccount_Zone_Id,
	                    ULBAccount_Circle_Id,
	                    ULBAccount_Division_Id,
	                    ULBAccount_BranchName,
	                    ULBAccount_IFSCCode,
	                    ULBAccount_AccountNo,
	                    ULBAccount_AddedBy,
	                    ULBAccount_AddedOn,
	                    ULBAccount_Status,
	                    ULBAccount_BranchAddress,
	                    ULBAccount_MICRCode,
	                    ULBAccount_BankId,
	                    ULBAccount_SchemeId, 
	                    Bank_Name, 
	                    Jurisdiction_Name_Eng, 
	                    ULB_Name, 
	                    Project_Name, 
                        Zone_Name, 
                        Circle_Name, 
                        Division_Name
                    from tbl_ULBAccountDtls  
                    left join tbl_ULB on ULB_Id = ULBAccount_ULB_Id
                    left join M_Jurisdiction on ULB_District_Id = M_Jurisdiction_Id
                    join tbl_Bank on Bank_Id = ULBAccount_BankId
                    join tbl_Project on Project_Id = ULBAccount_SchemeId
                    left join tbl_Division on ULBAccount_Division_Id = Division_Id
                    left join tbl_Circle on Circle_Id = ULBAccount_Circle_Id
                    left join tbl_Zone on Zone_Id = ULBAccount_Zone_Id
                    where ULBAccount_Status = 1 ";
        if (Zone_Id > 0)
        {
            strQuery += "and ULBAccount_Zone_Id = " + Zone_Id.ToString();
        }
        if (Circle_Id > 0)
        {
            strQuery += "and ULBAccount_Circle_Id = " + Circle_Id.ToString();
        }
        if (Division_Id > 0)
        {
            strQuery += "and ULBAccount_Division_Id = " + Division_Id.ToString();
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_ULBAccountDtls(tbl_ULBAccountDtls obj_tbl_ULBAccountDtls, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Update_tbl_ULBAccountDtls(obj_tbl_ULBAccountDtls, trans, cn);
                Insert_tbl_ULBAccountDtls(obj_tbl_ULBAccountDtls, trans, cn);
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Update_tbl_ULBAccountDtls(tbl_ULBAccountDtls obj_tbl_ULBAccountDtls, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; update tbl_ULBAccountDtls set ULBAccount_Status = 0, ULBAccount_ModifiedBy = '" + obj_tbl_ULBAccountDtls.ULBAccount_AddedBy + "', ULBAccount_ModifiedOn = getdate() where ULBAccount_Status = 1 and ULBAccount_Zone_Id = '" + obj_tbl_ULBAccountDtls.ULBAccount_Zone_Id + "' and ULBAccount_Circle_Id = '" + obj_tbl_ULBAccountDtls.ULBAccount_Circle_Id + "' and ULBAccount_Division_Id = '" + obj_tbl_ULBAccountDtls.ULBAccount_Division_Id + "' and ULBAccount_SchemeId = '" + obj_tbl_ULBAccountDtls.ULBAccount_SchemeId + "'";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_ULBAccountDtls(tbl_ULBAccountDtls obj_tbl_ULBAccountDtls, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ULBAccountDtls ( [ULBAccount_AccountNo],[ULBAccount_AddedBy],[ULBAccount_AddedOn],[ULBAccount_BankId],[ULBAccount_BranchAddress],[ULBAccount_BranchName],[ULBAccount_IFSCCode],[ULBAccount_MICRCode],[ULBAccount_Status], [ULBAccount_ULB_Id], [ULBAccount_SchemeId], [ULBAccount_Zone_Id], [ULBAccount_Circle_Id], [ULBAccount_Division_Id]) values ('" + obj_tbl_ULBAccountDtls.ULBAccount_AccountNo + "','" + obj_tbl_ULBAccountDtls.ULBAccount_AddedBy + "',getdate(),'" + obj_tbl_ULBAccountDtls.ULBAccount_BankId + "','" + obj_tbl_ULBAccountDtls.ULBAccount_BranchAddress + "','" + obj_tbl_ULBAccountDtls.ULBAccount_BranchName + "','" + obj_tbl_ULBAccountDtls.ULBAccount_IFSCCode + "','" + obj_tbl_ULBAccountDtls.ULBAccount_MICRCode + "','" + obj_tbl_ULBAccountDtls.ULBAccount_Status + "', '" + obj_tbl_ULBAccountDtls.ULBAccount_ULB_Id + "', '" + obj_tbl_ULBAccountDtls.ULBAccount_SchemeId + "', '" + obj_tbl_ULBAccountDtls.ULBAccount_Zone_Id + "', '" + obj_tbl_ULBAccountDtls.ULBAccount_Circle_Id + "', '" + obj_tbl_ULBAccountDtls.ULBAccount_Division_Id + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_tbl_ULBAccountDtls(int ULBAccountDtls_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ULBAccountDtls set ULBAccount_Status = 0, ULBAccount_ModifiedBy = '" + person_Id + "', ULBAccount_ModifiedOn = getdate() where ULBAccount_Id = '" + ULBAccountDtls_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Zone
    public DataSet get_tbl_Zone()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Zone_Id, 
                        Zone_Name, 
                        Zone_AddedOn, 
                        Zone_AddedBy, 
                        Zone_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Zone_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = Zone_ModifiedOn
                      from tbl_Zone
                      left join tbl_PersonDetail on Person_Id = Zone_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Zone_ModifiedBy
                      where Zone_Status = 1 order by Zone_Name";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet Edit_tbl_Zone(string Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " select Zone_Id,Zone_Name, Zone_DistrictId from tbl_Zone where Zone_Status=1 and Zone_Id ='" + Zone_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_Zone(tbl_Zone obj_tbl_Zone, int Zone_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyZone(obj_tbl_Zone.Zone_Name, Zone_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }

                if (Zone_Id == 0)
                {
                    Insert_tbl_Zone(obj_tbl_Zone, trans, cn);
                }
                else
                {
                    obj_tbl_Zone.Zone_Id = Zone_Id;
                    Update_tbl_Zone(obj_tbl_Zone, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyZone(string ZoneName, string Zone_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Zone  where Zone_Status = 1 and  Zone_Name = '" + ZoneName + "' ";
        if (Zone_Id != "0")
        {
            strQuery += " AND Zone_Id  <> '" + Zone_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Zone(tbl_Zone obj_tbl_Zone, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Zone( [Zone_AddedBy],[Zone_AddedOn],[Zone_Name],[Zone_Status]) values('" + obj_tbl_Zone.Zone_AddedBy + "', getdate(), N'" + obj_tbl_Zone.Zone_Name + "','" + obj_tbl_Zone.Zone_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Zone(tbl_Zone obj_tbl_Zone, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Zone set  Zone_Name = N'" + obj_tbl_Zone.Zone_Name + "', Zone_ModifiedOn = getdate(), Zone_ModifiedBy = '" + obj_tbl_Zone.Zone_AddedBy + "' where Zone_Id = '" + obj_tbl_Zone.Zone_Id + "' and Zone_Status = '" + obj_tbl_Zone.Zone_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Zone(int Zone_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Zone set   Zone_Status = 0 where Zone_Id = '" + Zone_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Circle
    public DataSet get_tbl_Circle(int Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Circle_Id, 
                        Circle_ZoneId, 
                        Zone_Name, 
                        Circle_Name, 
                        Circle_AddedOn, 
                        Circle_AddedBy, 
                        Circle_ModifiedOn, 
                        Circle_ModifiedBy, 
                        Circle_Status 
                    from tbl_Circle
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    where Circle_Status = 1";
        if (Zone_Id != 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        strQuery += " order by Zone_Name, Circle_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Circle(tbl_Circle obj_tbl_Circle, int Circle_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyCircle(obj_tbl_Circle.Circle_Name, Circle_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Circle_Id == 0)
                {
                    Insert_tbl_Circle(obj_tbl_Circle, trans, cn);
                }
                else
                {
                    obj_tbl_Circle.Circle_Id = Circle_Id;
                    Update_tbl_Circle(obj_tbl_Circle, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyCircle(string Circle_Name, string CircleId, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Circle  where Circle_Status=1 and  Circle_Name= '" + Circle_Name + "' ";
        if (CircleId != "0")
        {
            strQuery += " AND Circle_Id <> '" + CircleId + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Circle(tbl_Circle obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Circle (Circle_Name, Circle_ZoneId, Circle_AddedBy, Circle_AddedOn, Circle_Status) values ('" + obj_tbl_Post.Circle_Name + "', '" + obj_tbl_Post.Circle_ZoneId + "', '" + obj_tbl_Post.Circle_AddedBy + "',getdate(),'" + obj_tbl_Post.Circle_Status + "');Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Circle(tbl_Circle obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_Circle set   Circle_Name = '" + obj_tbl_Post.Circle_Name + "', Circle_ZoneId = '" + obj_tbl_Post.Circle_ZoneId + "' where Circle_Id = '" + obj_tbl_Post.Circle_Id + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region Master Division
    public DataSet get_tbl_Division(int Circle_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Division_Id, 
                        Division_CircleId, 
                        Circle_Name, 
                        Division_Name, 
                        Division_AddedOn, 
                        Division_AddedBy, 
                        Division_ModifiedOn, 
                        Division_ModifiedBy, 
                        Division_Status 
                    from tbl_Division
                    join tbl_Circle on Circle_Id = Division_CircleId
                    where Division_Status = 1";
        if (Circle_Id != 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        strQuery += " order by Circle_Name, Division_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Division(tbl_Division obj_tbl_Division, int Division_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDivision(obj_tbl_Division.Division_Name, Division_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Division_Id == 0)
                {
                    Insert_tbl_Division(obj_tbl_Division, trans, cn);
                }
                else
                {
                    obj_tbl_Division.Division_Id = Division_Id;
                    Update_tbl_Division(obj_tbl_Division, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDivision(string Division_Name, string DivisionId, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Division  where Division_Status=1 and  Division_Name= '" + Division_Name + "' ";
        if (DivisionId != "0")
        {
            strQuery += " AND Division_Id <> '" + DivisionId + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Division(tbl_Division obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Division (Division_Name, Division_CircleId, Division_AddedBy, Division_AddedOn, Division_Status) values ('" + obj_tbl_Post.Division_Name + "', '" + obj_tbl_Post.Division_CircleId + "', '" + obj_tbl_Post.Division_AddedBy + "',getdate(),'" + obj_tbl_Post.Division_Status + "');Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Division(tbl_Division obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_Division set   Division_Name = '" + obj_tbl_Post.Division_Name + "', Division_CircleId = '" + obj_tbl_Post.Division_CircleId + "' where Division_Id = '" + obj_tbl_Post.Division_Id + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Division(int Division_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Division set   Division_Status = 0 where Division_Id = '" + Division_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Section
    public DataSet get_tbl_Section()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; select Section_Id, Section_Name, Section_AddedOn, Section_AddedBy, Section_Status, isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, Created_Date = Section_AddedOn,tbl_PersonDetail1.Person_Name as ModifiedBy,Mdified_Date=Section_ModifiedOn 
                    from tbl_Section 
                    left join tbl_PersonDetail on Person_Id = Section_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = Section_ModifiedBy 
                    where Section_Status = 1 order by Section_Id";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Section(tbl_Section obj_tbl_Section, int Section_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacySection(obj_tbl_Section.Section_Name, Section_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Section_Id == 0)
                {
                    Insert_tbl_Section(obj_tbl_Section, trans, cn);
                }
                else
                {
                    obj_tbl_Section.Section_Id = Section_Id;
                    Update_tbl_Section(obj_tbl_Section, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacySection(string SectionName, string Section_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Section  where Section_Status = 1 and  Section_Name = '" + SectionName + "' ";
        if (Section_Id != "0")
        {
            strQuery += " AND Section_Id  <> '" + Section_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Section(tbl_Section obj_tbl_Section, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Section( [Section_AddedBy],[Section_AddedOn],[Section_Name],[Section_Status] ) values('" + obj_tbl_Section.Section_AddedBy + "', getdate(), N'" + obj_tbl_Section.Section_Name + "','" + obj_tbl_Section.Section_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Section(tbl_Section obj_tbl_Section, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Section set  Section_Name = N'" + obj_tbl_Section.Section_Name + "',Section_ModifiedOn = getDate(),Section_ModifiedBy = '" + obj_tbl_Section.Section_AddedBy + "' where Section_Id = '" + obj_tbl_Section.Section_Id + "' and Section_Status = '" + obj_tbl_Section.Section_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Section(int Section_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Section set   Section_Status = 0 where Section_Id = '" + Section_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Project Type
    public DataSet get_tbl_ProjectType()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        ProjectType_Id, 
                        ProjectType_Name, 
                        ProjectType_AddedOn, 
                        ProjectType_AddedBy, 
                        ProjectType_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = ProjectType_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Mdified_Date=ProjectType_ModifiedOn 
                    from tbl_ProjectType 
                    left join tbl_PersonDetail on Person_Id = ProjectType_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = ProjectType_ModifiedBy 
                    where ProjectType_Status = 1 order by ProjectType_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_ProjectType(tbl_ProjectType obj_tbl_ProjectType, int ProjectType_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyProjectType(obj_tbl_ProjectType.ProjectType_Name, ProjectType_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (ProjectType_Id == 0)
                {
                    Insert_tbl_ProjectType(obj_tbl_ProjectType, trans, cn);
                }
                else
                {
                    obj_tbl_ProjectType.ProjectType_Id = ProjectType_Id;
                    Update_tbl_ProjectType(obj_tbl_ProjectType, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyProjectType(string ProjectTypeName, string ProjectType_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_ProjectType  where ProjectType_Status = 1 and  ProjectType_Name = '" + ProjectTypeName + "' ";
        if (ProjectType_Id != "0")
        {
            strQuery += " AND ProjectType_Id  <> '" + ProjectType_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_ProjectType(tbl_ProjectType obj_tbl_ProjectType, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_ProjectType( [ProjectType_AddedBy],[ProjectType_AddedOn],[ProjectType_Name],[ProjectType_Status] ) values('" + obj_tbl_ProjectType.ProjectType_AddedBy + "', getdate(), N'" + obj_tbl_ProjectType.ProjectType_Name + "','" + obj_tbl_ProjectType.ProjectType_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_ProjectType(tbl_ProjectType obj_tbl_ProjectType, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ProjectType set  ProjectType_Name = N'" + obj_tbl_ProjectType.ProjectType_Name + "',ProjectType_ModifiedOn = getDate(),ProjectType_ModifiedBy = '" + obj_tbl_ProjectType.ProjectType_AddedBy + "' where ProjectType_Id = '" + obj_tbl_ProjectType.ProjectType_Id + "' and ProjectType_Status = '" + obj_tbl_ProjectType.ProjectType_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_ProjectType(int ProjectType_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ProjectType set   ProjectType_Status = 0 where ProjectType_Id = '" + ProjectType_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Project
    public DataSet get_tbl_Project()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Project_Id, 
                        Project_Name, 
                        Project_Budget, 
                        Project_AddedOn, 
                        Project_AddedBy, 
                        Project_Status, 
                        Project_Guideline_Path, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Project_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = Project_ModifiedOn, 
                        Project_GO_Path
                    from tbl_Project
                    left join tbl_PersonDetail on Person_Id = Project_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Project_ModifiedBy
                    where Project_Status = 1 
                    order by Project_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_Project(string Scheme_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Project_Id, 
                        Project_Name, 
                        Project_Budget, 
                        Project_AddedOn, 
                        Project_AddedBy, 
                        Project_Status, 
                        Project_Guideline_Path, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Project_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = Project_ModifiedOn, 
                        Project_GO_Path
                      from tbl_Project
                      left join tbl_PersonDetail on Person_Id = Project_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = Project_ModifiedBy
                      where Project_Status = 1 and Project_Id = '" + Scheme_Id + "' order by Project_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet Edit_tbl_Project(int Project_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = "set dateformat dmy; select * from tbl_Project where Project_Status=1 and Project_Id='" + Project_Id + "'; ";

        strQuery += @"select 
                        FundingPattern_Id, 
                        FundingPattern_Name, 
                        FundingPattern_AddedOn, 
                        FundingPattern_AddedBy, 
                        FundingPattern_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = FundingPattern_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=FundingPattern_ModifiedOn, 
						ProjectFundingPattern_Value,
						ProjectFundingPattern_Percentage,
						ProjectFundingPattern_ProjectId,
						ProjectFundingPattern_Id
                    from tbl_FundingPattern 
					left join tbl_ProjectFundingPattern on FundingPattern_Id = ProjectFundingPattern_FundingPattern_Id and ProjectFundingPattern_ProjectId = Project_IdCond and ProjectFundingPattern_Status = 1
                    left join tbl_PersonDetail on Person_Id = FundingPattern_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = FundingPattern_ModifiedBy 
                    where FundingPattern_Status = 1 order by FundingPattern_Name";
        if (Project_Id > 0)
        {
            strQuery = strQuery.Replace("Project_IdCond", Project_Id.ToString());
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_Project(tbl_Project obj_tbl_Project, List<tbl_ProjectFundingPattern> obj_tbl_ProjectFundingPattern_Li, int Project_Id, ref string Msg, string fileExt1, string fileExt2)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyProject(obj_tbl_Project.Project_Name, Project_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }

                string fileName1 = DateTime.Now.Ticks.ToString("x") + "." + fileExt1;
                obj_tbl_Project.Project_GO_Path = "\\Downloads\\GO\\" + fileName1;
                if (obj_tbl_Project.Project_GO_Path_Bytes != null && obj_tbl_Project.Project_GO_Path_Bytes.Length > 0)
                {
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\GO\\" + fileName1, obj_tbl_Project.Project_GO_Path_Bytes);
                }
                else
                {
                    obj_tbl_Project.Project_GO_Path = "";
                }
                string fileName2 = DateTime.Now.Ticks.ToString("x") + "." + fileExt2;
                obj_tbl_Project.Project_Guideline_Path = "\\Downloads\\Guideline\\" + fileName2;
                if (obj_tbl_Project.Project_Guideline_Path_Bytes != null && obj_tbl_Project.Project_Guideline_Path_Bytes.Length > 0)
                {
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Guideline\\" + fileName2, obj_tbl_Project.Project_Guideline_Path_Bytes);
                }
                else
                {
                    obj_tbl_Project.Project_Guideline_Path = "";
                }
                if (Project_Id == 0)
                {
                    Insert_tbl_Project(obj_tbl_Project, trans, cn);
                }
                else
                {
                    obj_tbl_Project.Project_Id = Project_Id;
                    Update_tbl_Project(obj_tbl_Project, trans, cn);
                }

                Update_tbl_ProjectFundingPattern(obj_tbl_Project.Project_AddedBy, Project_Id, trans, cn);
                for (int i = 0; i < obj_tbl_ProjectFundingPattern_Li.Count; i++)
                {
                    Insert_tbl_ProjectFundingPattern(obj_tbl_ProjectFundingPattern_Li[i], trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Update_tbl_ProjectFundingPattern(int AddedBy, int Project_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ProjectFundingPattern set  ProjectFundingPattern_ModifiedBy = '" + AddedBy + "' ,  ProjectFundingPattern_ModifiedOn =  getdate(), ProjectFundingPattern_Status =  '0' where ProjectFundingPattern_Status =  '1' and ProjectFundingPattern_ProjectId = '" + Project_Id + "' ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_ProjectFundingPattern(tbl_ProjectFundingPattern obj_tbl_ProjectFundingPattern, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectFundingPattern ( [ProjectFundingPattern_AddedBy],[ProjectFundingPattern_AddedOn],[ProjectFundingPattern_FundingPattern_Id],[ProjectFundingPattern_Percentage],[ProjectFundingPattern_ProjectId],[ProjectFundingPattern_Status],[ProjectFundingPattern_Value] ) values ('" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_AddedBy + "',getdate(),'" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_FundingPattern_Id + "','" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Percentage + "','" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_ProjectId + "','" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Status + "','" + obj_tbl_ProjectFundingPattern.ProjectFundingPattern_Value + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private DataSet CheckDuplicacyProject(string ProjectName, string Project_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Project  where Project_Status = 1 and  Project_Name = '" + ProjectName + "' ";
        if (Project_Id != "0")
        {
            strQuery += " AND Project_Id  <> '" + Project_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Project(tbl_Project obj_tbl_Project, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Project( [Project_AddedBy],[Project_AddedOn],[Project_Name],[Project_Status], [Project_GO_Path], [Project_Budget], [Project_Guideline_Path]) values('" + obj_tbl_Project.Project_AddedBy + "', getdate(), N'" + obj_tbl_Project.Project_Name + "','" + obj_tbl_Project.Project_Status + "', '" + obj_tbl_Project.Project_GO_Path + "', '" + obj_tbl_Project.Project_Budget + "', '" + obj_tbl_Project.Project_Guideline_Path + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Project(tbl_Project obj_tbl_Project, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_Project set  Project_Name = N'" + obj_tbl_Project.Project_Name + "', Project_Budget= N'" + obj_tbl_Project.Project_Budget + "', Project_ModifiedOn = getdate(), Project_ModifiedBy = '" + obj_tbl_Project.Project_AddedBy + "', Project_GO_Path= N'" + obj_tbl_Project.Project_GO_Path + "', Project_Guideline_Path = N'" + obj_tbl_Project.Project_Guideline_Path + "' where Project_Id = '" + obj_tbl_Project.Project_Id + "' and Project_Status = '" + obj_tbl_Project.Project_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Project(int Project_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Project set   Project_Status = 0 where Project_Id = '" + Project_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Project Work
    public DataSet get_tbl_ProjectWork(int Project_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, int ULB_Id, string Status)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode, 
                        ProjectWork_Name, 
                        ProjectWork_Name_Code = isnull(ProjectWork_ProjectCode, '') + ' - ' + ProjectWork_Name,
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_GO_Date = convert(char(10), ProjectWork_GO_Date, 103), 
                        ProjectWork_GO_No,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
						ULB_Name, 
						Jurisdiction_Name_Eng, 
						Division_Name, 
						Circle_Name, 
						ProjectWork_DistrictId, 
                        ProjectWork_BlockId,
						ProjectWork_ULB_Id, 
						ProjectWork_DivisionId, 
						Division_CircleId
                    from tbl_ProjectWork
                    join tbl_Project on Project_Id = ProjectWork_Project_Id
					join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
					left join tbl_ULB on ULB_Id = ProjectWork_ULB_Id
					left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId
                    where ProjectWork_Status = 1  ";
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = '" + Project_Id + "'";
        }
        if (District_Id != 0)
        {
            strQuery += " and ProjectWork_DistrictId = '" + District_Id + "'";
        }
        if (Zone_Id != 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        if (Circle_Id != 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            strQuery += " and ProjectWork_DivisionId = '" + Division_Id + "'";
        }

        strQuery += " order by Project_Name, ProjectWork_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectWork_Edit(int ProjectWork_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode, 
                        ProjectWork_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_GO_Date = convert(char(10), ProjectWork_GO_Date, 103), 
                        ProjectWork_GO_No,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
						ULB_Name, 
						Jurisdiction_Name_Eng, 
						Division_Name, 
						Circle_Name, 
						ProjectWork_DistrictId, 
                        ProjectWork_BlockId,
						ProjectWork_ULB_Id, 
						ProjectWork_DivisionId, 
						Division_CircleId,
						Circle_ZoneId,
                        ProjectWork_ProjectType_Id,
                        ProjectWork_Centage,
                        ProjectWork_WorkCost
                    from tbl_ProjectWork
                    left join tbl_Project on Project_Id = ProjectWork_Project_Id
					join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
					left join tbl_ULB on ULB_Id = ProjectWork_ULB_Id
					left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId  
                    where ProjectWork_Status = 1 and ProjectWork_Id='" + ProjectWork_Id + "'";

        strQuery += @"select 
                        FundingPattern_Id,
                        FundingPattern_Name,
					    ProjectWorkFundingPattern_Percentage,
					    ProjectWorkFundingPattern_Value 
					from tbl_ProjectWorkFundingPattern 
					inner join tbl_FundingPattern on FundingPattern_Id=ProjectWorkFundingPattern_FundingPatternId
					where ProjectWorkFundingPattern_ProjectWorkId='" + ProjectWork_Id + "' and ProjectWorkFundingPattern_Status=1 ";

        strQuery += @" select 
                            ProjectAdditionalArea_ZoneId,
                            Zone_Name,
                            ProjectAdditionalArea_CircleId,
                            Circle_Name,
                            ProjectAdditionalArea_DevisionId,
                            Division_Name 
                        from tbl_ProjectAdditionalArea
                        inner join tbl_Zone on Zone_Id=ProjectAdditionalArea_ZoneId
                        inner join tbl_Circle on Circle_Id=ProjectAdditionalArea_CircleId
                        inner join tbl_Division on Division_Id=ProjectAdditionalArea_DevisionId
                        where ProjectAdditionalArea_ProjectWork_Id='" + ProjectWork_Id + "' and ProjectAdditionalArea_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }


    public bool Insert_tbl_ProjectWork(tbl_ProjectWork obj_tbl_ProjectWork, List<tbl_ProjectWorkFundingPattern> obj_tbl_ProjectWorkFundingPattern_Li, string extGO, List<tbl_ProjectAdditionalArea> obj_tbl_ProjectAdditionalArea_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string fileName = "";
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\GO\\"))
                {
                    Directory.Exists(Server.MapPath(".") + "\\Downloads\\GO\\");
                }
                if (obj_tbl_ProjectWork.ProjectWork_GO_Path_Bytes != null && obj_tbl_ProjectWork.ProjectWork_GO_Path_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + extGO;
                    obj_tbl_ProjectWork.ProjectWork_GO_Path = "\\Downloads\\GO\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\GO\\" + fileName, obj_tbl_ProjectWork.ProjectWork_GO_Path_Bytes);
                }
                else
                {
                    if (extGO == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWork.ProjectWork_GO_Path = "";
                    }
                }
                if (obj_tbl_ProjectWork.ProjectWork_Id == 0)
                {
                    obj_tbl_ProjectWork.ProjectWork_Id = Insert_tbl_ProjectWork(obj_tbl_ProjectWork, trans, cn);
                }
                else
                {
                    Update_tbl_ProjectWork(obj_tbl_ProjectWork, trans, cn);
                }

                Update_tbl_ProjectWorkFundingPattern(obj_tbl_ProjectWork.ProjectWork_AddedBy, obj_tbl_ProjectWork.ProjectWork_Id, trans, cn);

                for (int i = 0; i < obj_tbl_ProjectWorkFundingPattern_Li.Count; i++)
                {
                    obj_tbl_ProjectWorkFundingPattern_Li[i].ProjectWorkFundingPattern_ProjectWorkId = obj_tbl_ProjectWork.ProjectWork_Id;
                    Insert_tbl_ProjectWorkFundingPattern(obj_tbl_ProjectWorkFundingPattern_Li[i], trans, cn);
                }

                if (obj_tbl_ProjectAdditionalArea_Li != null)
                {
                    string strQuery = "";
                    strQuery = " set dateformat dmy; update tbl_ProjectAdditionalArea set ProjectAdditionalArea_ModifiedBy = '" + obj_tbl_ProjectWork.ProjectWork_AddedBy + "',ProjectAdditionalArea_ModifiedOn=getdate(),ProjectAdditionalArea_Status=0 where ProjectAdditionalArea_ProjectWork_Id = '" + obj_tbl_ProjectWork.ProjectWork_Id + "' and ProjectAdditionalArea_Status=1 ";

                    ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                    strQuery = "select ProjectWorkPkg_Id from tbl_ProjectWorkPkg where ProjectWorkPkg_Status = 1 and ProjectWorkPkg_Work_Id = '" + obj_tbl_ProjectWork.ProjectWork_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            for (int i = 0; i < obj_tbl_ProjectAdditionalArea_Li.Count; i++)
                            {
                                obj_tbl_ProjectAdditionalArea_Li[i].ProjectAdditionalArea_ProjectWork_Id = obj_tbl_ProjectWork.ProjectWork_Id;
                                obj_tbl_ProjectAdditionalArea_Li[i].ProjectAdditionalArea_ProjectWorkPkg_Id = Convert.ToInt32(ds.Tables[0].Rows[k]["ProjectWorkPkg_Id"].ToString());
                                Insert_tbl_ProjectAdditionalArea(obj_tbl_ProjectAdditionalArea_Li[i], trans, cn);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < obj_tbl_ProjectAdditionalArea_Li.Count; i++)
                        {
                            obj_tbl_ProjectAdditionalArea_Li[i].ProjectAdditionalArea_ProjectWork_Id = obj_tbl_ProjectWork.ProjectWork_Id;
                            Insert_tbl_ProjectAdditionalArea(obj_tbl_ProjectAdditionalArea_Li[i], trans, cn);
                        }
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private int Insert_tbl_ProjectWork(tbl_ProjectWork obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_ProjectWork (ProjectWork_Name, ProjectWork_Project_Id, ProjectWork_AddedBy, ProjectWork_AddedOn, ProjectWork_Status, ProjectWork_GO_Path, ProjectWork_Budget, ProjectWork_GO_Date, ProjectWork_GO_No, ProjectWork_ProjectType_Id, ProjectWork_Description, ProjectWork_DistrictId, ProjectWork_ULB_Id, ProjectWork_DivisionId, ProjectWork_ProjectCode, ProjectWork_BlockId,ProjectWork_Centage) values (N'" + obj_tbl_Post.ProjectWork_Name + "', '" + obj_tbl_Post.ProjectWork_Project_Id + "', '" + obj_tbl_Post.ProjectWork_AddedBy + "',getdate(),'" + obj_tbl_Post.ProjectWork_Status + "','" + obj_tbl_Post.ProjectWork_GO_Path + "','" + obj_tbl_Post.ProjectWork_Budget + "', convert(date, '" + obj_tbl_Post.ProjectWork_GO_Date + "', 103), N'" + obj_tbl_Post.ProjectWork_GO_No + "','" + obj_tbl_Post.ProjectWork_ProjectType_Id + "', N'" + obj_tbl_Post.ProjectWork_Description + "', '" + obj_tbl_Post.ProjectWork_DistrictId + "', '" + obj_tbl_Post.ProjectWork_ULB_Id + "', '" + obj_tbl_Post.ProjectWork_DivisionId + "', '" + obj_tbl_Post.ProjectWork_ProjectCode + "', '" + obj_tbl_Post.ProjectWork_BlockId + "', '" + obj_tbl_Post.ProjectWork_Centage + "');Select @@Identity";

        if (trans == null)
        {
            return Convert.ToInt32(ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return Convert.ToInt32(ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString());
        }
    }

    private void Update_tbl_ProjectWork(tbl_ProjectWork obj_tbl_Post, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_ProjectWork set   ProjectWork_Name = N'" + obj_tbl_Post.ProjectWork_Name + "', ProjectWork_Project_Id = '" + obj_tbl_Post.ProjectWork_Project_Id + "', ProjectWork_Budget = '" + obj_tbl_Post.ProjectWork_Budget + "', ProjectWork_GO_Path = N'" + obj_tbl_Post.ProjectWork_GO_Path + "', ProjectWork_ModifiedBy = '" + obj_tbl_Post.ProjectWork_AddedBy + "', ProjectWork_ModifiedOn = getdate(), ProjectWork_GO_Date = convert(date, '" + obj_tbl_Post.ProjectWork_GO_Date + "', 103), ProjectWork_GO_No = N'" + obj_tbl_Post.ProjectWork_GO_No + "', ProjectWork_ProjectType_Id = '" + obj_tbl_Post.ProjectWork_ProjectType_Id + "', ProjectWork_Description = N'" + obj_tbl_Post.ProjectWork_Description + "', ProjectWork_DistrictId = '" + obj_tbl_Post.ProjectWork_DistrictId + "', ProjectWork_ULB_Id = '" + obj_tbl_Post.ProjectWork_ULB_Id + "', ProjectWork_DivisionId = '" + obj_tbl_Post.ProjectWork_DivisionId + "', ProjectWork_ProjectCode = '" + obj_tbl_Post.ProjectWork_ProjectCode + "', ProjectWork_BlockId = '" + obj_tbl_Post.ProjectWork_BlockId + "', ProjectWork_Centage = '" + obj_tbl_Post.ProjectWork_Centage + "' where ProjectWork_Id = '" + obj_tbl_Post.ProjectWork_Id + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_ProjectWorkFundingPattern(tbl_ProjectWorkFundingPattern obj_tbl_ProjectWorkFundingPattern, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectWorkFundingPattern ( [ProjectWorkFundingPattern_AddedBy],[ProjectWorkFundingPattern_AddedOn],[ProjectWorkFundingPattern_FundingPatternId],[ProjectWorkFundingPattern_Percentage],[ProjectWorkFundingPattern_ProjectWorkId],[ProjectWorkFundingPattern_Status],[ProjectWorkFundingPattern_Value] ) values ('" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_AddedBy + "', getdate(),'" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_FundingPatternId + "','" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Percentage + "','" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_ProjectWorkId + "','" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Status + "','" + obj_tbl_ProjectWorkFundingPattern.ProjectWorkFundingPattern_Value + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_ProjectAdditionalArea(tbl_ProjectAdditionalArea obj_tbl_ProjectAdditionalArea, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectAdditionalArea ( [ProjectAdditionalArea_AddedBy],[ProjectAdditionalArea_AddedOn],[ProjectAdditionalArea_ProjectWork_Id],[ProjectAdditionalArea_ZoneId],[ProjectAdditionalArea_CircleId],[ProjectAdditionalArea_DevisionId],[ProjectAdditionalArea_Status],ProjectAdditionalArea_ProjectWorkPkg_Id ) values ('" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_AddedBy + "', getdate(),'" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_ProjectWork_Id + "','" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_ZoneId + "','" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_CircleId + "','" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_DevisionId + "','" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_Status + "','" + obj_tbl_ProjectAdditionalArea.ProjectAdditionalArea_ProjectWorkPkg_Id + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Update_tbl_ProjectWorkFundingPattern(int Added_By, int ProjectWork_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ProjectWorkFundingPattern set  ProjectWorkFundingPattern_Status =  '0' , ProjectWorkFundingPattern_ModifiedBy = '" + Added_By + "' ,  ProjectWorkFundingPattern_ModifiedOn =  getdate() where ProjectWorkFundingPattern_ProjectWorkId = '" + ProjectWork_Id + "' and ProjectWorkFundingPattern_Status = 1 ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    public bool Delete_ProjectWork(int Project_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ProjectWork set   ProjectWork_Status = 0 where ProjectWork_Id = '" + Project_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Master Deduction
    public DataSet get_tbl_Deduction(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Invoice_Id == 0)
            strQuery = @" set dateformat dmy; 
                        select 
                            Deduction_Id, 
                            Deduction_Name, 
                            Deduction_Mode,
                            Deduction_Type, 
                            Deduction_Category, 
                            Deduction_Value,
                            Deduction_AddedOn, 
                            Deduction_AddedBy, 
                            Deduction_Status,
                            PackageInvoiceAdditional_Deduction_Value_Final = 0,
                            PackageInvoiceAdditional_Comments = '',
                            PackageInvoiceAdditional_Deduction_isFlat = '0',
                            isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                            Created_Date = Deduction_AddedOn,
                            PersonDetail1.Person_Name as ModifiedBy,Moddified_Date = Deduction_ModifiedOn 
                        from tbl_Deduction  
                        left join tbl_PersonDetail on Person_Id = Deduction_AddedBy
                        left join tbl_PersonDetail as PersonDetail1 on PersonDetail1.Person_Id = Deduction_ModifiedBy
                        where Deduction_Status = 1 order by Deduction_Category, Deduction_Type, Deduction_Name";
        else
            strQuery = @" set dateformat dmy; 
                        select  
	                        isnull(PackageInvoiceAdditional_Id,0) as PackageInvoiceAdditional_Id,
	                        PackageInvoiceAdditional_Invoice_Id,
	                        Deduction_Id,
	                        Deduction_Name,
	                        Deduction_Mode,
	                        Deduction_Category,
	                        Deduction_Type,
	                        Deduction_Value = case when isnull(PackageInvoiceAdditional_Id, 0) = 0 then Deduction_Value else PackageInvoiceAdditional_Deduction_Value_Master end,
	                        PackageInvoiceAdditional_Deduction_Value_Final = isnull(PackageInvoiceAdditional_Deduction_Value_Final, 0),
	                        Deduction_AddedBy,
	                        Created_Date = Deduction_AddedOn, 
                            Deduction_Status, 
                            PackageInvoiceAdditional_Comments, 
                            PackageInvoiceAdditional_Deduction_isFlat = isnull(PackageInvoiceAdditional_Deduction_isFlat, 0)
                        from tbl_Deduction 
                        left join tbl_PackageInvoiceAdditional on Deduction_Id = PackageInvoiceAdditional_Deduction_Id and PackageInvoiceAdditional_Status = 1 and PackageInvoiceAdditional_Invoice_Id = '" + Invoice_Id + "' where Deduction_Status = 1 order by Deduction_Category, Deduction_Type, Deduction_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_Deduction(int Invoice_Id, int Added_By)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select  
	                    PackageInvoiceAdditional_Id,
	                    PackageInvoiceAdditional_Invoice_Id,
	                    Deduction_Id,
	                    Deduction_Mode,
	                    Deduction_Name,
	                    Deduction_Category,
	                    Deduction_Type,
	                    PackageInvoiceAdditional_Deduction_Value_Master = case when isnull(PackageInvoiceAdditional_Deduction_isFlat, 0) = 0 then convert(varchar, PackageInvoiceAdditional_Deduction_Value_Master) + '%' else 'Flat' end,
	                    PackageInvoiceAdditional_Deduction_Value_Final,
	                    PackageInvoiceAdditional_Comments, 
                        PackageInvoiceAdditional_Deduction_isFlat = isnull(PackageInvoiceAdditional_Deduction_isFlat, 0)
                    from tbl_PackageInvoiceAdditional 
                    join tbl_Deduction on Deduction_Id = PackageInvoiceAdditional_Deduction_Id 
                    where Deduction_Status = 1 and PackageInvoiceAdditional_Status = 1 and PackageInvoiceAdditional_Invoice_Id = '" + Invoice_Id + "' ";
        if (Added_By > 0)
        {
            strQuery += " and PackageInvoiceAdditional_AddedBy = '" + Added_By + "' ";
        }
        strQuery += " order by Deduction_Category, Deduction_Type, Deduction_Name ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_Deduction_Mode(int EMB_Master_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Deduction_Id, 
                        Deduction_Name, 
                        Deduction_Mode,
                        Deduction_Type, 
                        Deduction_Category, 
                        Deduction_Value,
                        Deduction_AddedOn, 
                        Deduction_AddedBy, 
                        Deduction_Status
                    from tbl_Deduction  
                    where Deduction_Status = 1 and Deduction_Mode = '+' 
                    order by Deduction_Category, Deduction_Type, Deduction_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_Deduction_WithPackageTax(int PackageEMB_Tax_Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Deduction_Id, 
						Isnull(PackageEMB_Tax_Deduction_Id,0) as PackageEMB_Tax_Deduction_Id,
						PackageEMB_Tax_Value,
                        Deduction_Name, 
                        Deduction_Mode,
                        Deduction_Type, 
                        Deduction_Category, 
                        Deduction_Value,
                        Deduction_AddedOn, 
                        Deduction_AddedBy, 
                        Deduction_Status
                    from tbl_Deduction  
					left join tbl_PackageEMB_Tax on PackageEMB_Tax_Deduction_Id=Deduction_Id and PackageEMB_Tax_Package_Id='" + PackageEMB_Tax_Package_Id + "' "; strQuery += @" and PackageEMB_Tax_Status = 1
                    where Deduction_Status = 1 and Deduction_Mode = '+' 
                    order by Deduction_Category, Deduction_Type, Deduction_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_Deduction_WithPackageADPTax(int Package_ADP_Item_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Deduction_Id, 
						Isnull(Package_ADP_Item_Tax_Package_Id,0) as Package_ADP_Item_Tax_Package_Id,
						Package_ADP_Item_Tax_Value,
                        Deduction_Name, 
                        Deduction_Mode,
                        Deduction_Type, 
                        Deduction_Category, 
                        Deduction_Value,
                        Deduction_AddedOn, 
                        Deduction_AddedBy, 
                        Deduction_Status
                    from tbl_Deduction  
					left join tbl_Package_ADP_Item_Tax on Package_ADP_Item_Tax_Deduction_Id=Deduction_Id and Package_ADP_Item_Tax_Package_ADP_Item_Id='" + Package_ADP_Item_Id + "' "; strQuery += @" and Package_ADP_Item_Tax_Status = 1
                    where Deduction_Status = 1 and Deduction_Mode = '+' 
                    order by Deduction_Category, Deduction_Type, Deduction_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_Deduction_WithPackageInvoiceItem_Tax(int PackageInvoiceItem_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Deduction_Id, 
						Isnull(PackageInvoiceItem_Tax_Deduction_Id,0) as PackageInvoiceItem_Tax_Deduction_Id,
						Isnull(PackageInvoiceItem_Tax_Value,0) as PackageInvoiceItem_Tax_Value,
                        Deduction_Name, 
                        Deduction_Mode,
                        Deduction_Type, 
                        Deduction_Category, 
                        Deduction_Value,
                        Deduction_AddedOn, 
                        Deduction_AddedBy, 
                        Deduction_Status
                    from tbl_Deduction  
					left join tbl_PackageInvoiceItem_Tax on PackageInvoiceItem_Tax_Deduction_Id=Deduction_Id and PackageInvoiceItem_Tax_Status = 1 and PackageInvoiceItem_Tax_PackageInvoiceItem_Id='" + PackageInvoiceItem_Id + "'";
        strQuery += @" where Deduction_Status = 1 and Deduction_Mode = '+' 
                    order by Deduction_Category, Deduction_Type, Deduction_Name ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Deduction(tbl_Deduction obj_tbl_Deduction, int Deduction_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDeduction(obj_tbl_Deduction.Deduction_Name, Deduction_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Deduction_Id == 0)
                {
                    Insert_tbl_Deduction(obj_tbl_Deduction, trans, cn);
                }
                else
                {
                    obj_tbl_Deduction.Deduction_Id = Deduction_Id;
                    Update_tbl_Deduction(obj_tbl_Deduction, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDeduction(string DeductionName, string Deduction_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Deduction  where Deduction_Status = 1 and  Deduction_Name = '" + DeductionName + "' ";
        if (Deduction_Id != "0")
        {
            strQuery += " AND Deduction_Id  <> '" + Deduction_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Deduction(tbl_Deduction obj_tbl_Deduction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Deduction([Deduction_AddedBy],[Deduction_AddedOn],[Deduction_Name],[Deduction_Status], [Deduction_Type], [Deduction_Value], [Deduction_Category], [Deduction_Mode]) values('" + obj_tbl_Deduction.Deduction_AddedBy + "', getdate(), N'" + obj_tbl_Deduction.Deduction_Name + "','" + obj_tbl_Deduction.Deduction_Status + "', '" + obj_tbl_Deduction.Deduction_Type + "', '" + obj_tbl_Deduction.Deduction_Value + "', '" + obj_tbl_Deduction.Deduction_Category + "', '" + obj_tbl_Deduction.Deduction_Mode + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Deduction(tbl_Deduction obj_tbl_Deduction, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Deduction set  Deduction_Name = N'" + obj_tbl_Deduction.Deduction_Name + "', Deduction_Type = '" + obj_tbl_Deduction.Deduction_Type + "', Deduction_Value = '" + obj_tbl_Deduction.Deduction_Value + "', Deduction_ModifiedOn = getDate(), Deduction_ModifiedBy = '" + obj_tbl_Deduction.Deduction_AddedBy + "', Deduction_Category = '" + obj_tbl_Deduction.Deduction_Category + "', Deduction_Mode = '" + obj_tbl_Deduction.Deduction_Mode + "' where Deduction_Id = '" + obj_tbl_Deduction.Deduction_Id + "' and Deduction_Status = '" + obj_tbl_Deduction.Deduction_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Deduction(int Deduction_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Deduction set   Deduction_Status = 0,Deduction_ModifiedBy='" + person_Id + "',Deduction_ModifiedOn=getdate() where Deduction_Id = '" + Deduction_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Funding Pattern
    public DataSet get_tbl_FundingPattern()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        FundingPattern_Id, 
                        FundingPattern_Name, 
                        FundingPattern_AddedOn, 
                        FundingPattern_AddedBy, 
                        ProjectWorkFundingPattern_Percentage=0,
                        ProjectWorkFundingPattern_Value=0,
                        FundingPattern_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = FundingPattern_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=FundingPattern_ModifiedOn, 
                        ProjectFundingPattern_Value = 0,
						ProjectFundingPattern_Percentage = 0,
						ProjectFundingPattern_ProjectId = 0
                    from tbl_FundingPattern 
                    left join tbl_PersonDetail on Person_Id = FundingPattern_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = FundingPattern_ModifiedBy 
                    where FundingPattern_Status = 1 order by FundingPattern_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectFundingPattern(int ProjectWorkId)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        FundingPattern_Id = 0,
	                    FundingPattern_Name = 'Total',
	                    ProjectWorkFundingPattern_Percentage = 0,
	                    ProjectWorkFundingPattern_Value = sum(isnull(ProjectWorkFundingPattern_Value, 0)), 
	                    As_Per_GO = isnull((select sum(ProjectWorkGO_TotalRelease) from tbl_ProjectWorkGO where ProjectWorkGO_Status=1 and ProjectWorkGO_Work_Id='ProjectWorkIdCond'),0),
                      Release = Isnull((select sum(FinancialTrans_TransAmount)  from tbl_FinancialTrans  
                            inner join tbl_PackageInvoice on PackageInvoice_Id=FinancialTrans_Invoice_Id
                            inner join tbl_ProjectWorkPkg on ProjectWorkPkg_Id=PackageInvoice_Package_Id
                            where PackageInvoice_Status=1 and FinancialTrans_Status=1 and ProjectWorkPkg_Work_Id='ProjectWorkIdCond'),0)
                    from tbl_ProjectWorkFundingPattern
                    join tbl_FundingPattern on FundingPattern_Id = ProjectWorkFundingPattern_FundingPatternId
                    where ProjectWorkFundingPattern_ProjectWorkId = 'ProjectWorkIdCond' and ProjectWorkFundingPattern_Status = 1
                    group by ProjectWorkFundingPattern_ProjectWorkId

                    union 

                    select 
                        FundingPattern_Id,
	                    FundingPattern_Name,
	                    ProjectWorkFundingPattern_Percentage,
	                    ProjectWorkFundingPattern_Value, 
	                    As_Per_GO = Case when FundingPattern_Name='Centrally funded' 
						then isnull((select sum(ProjectWorkGO_CentralShare) from tbl_ProjectWorkGO where ProjectWorkGO_Status=1 and ProjectWorkGO_Work_Id='ProjectWorkIdCond'),0)
						when FundingPattern_Name='State funded' then isnull((select sum(ProjectWorkGO_StateShare) from tbl_ProjectWorkGO where ProjectWorkGO_Status=1 and ProjectWorkGO_Work_Id='ProjectWorkIdCond'),0)
						when FundingPattern_Name='Local Bodies funded' then isnull((select sum(ProjectWorkGO_ULBShare) from tbl_ProjectWorkGO where ProjectWorkGO_Status=1 and ProjectWorkGO_Work_Id='ProjectWorkIdCond'),0)
						else 0 end, 
	                    Release = 0
                    from tbl_ProjectWorkFundingPattern
                    join tbl_FundingPattern on FundingPattern_Id = ProjectWorkFundingPattern_FundingPatternId
                    where ProjectWorkFundingPattern_ProjectWorkId = 'ProjectWorkIdCond' and ProjectWorkFundingPattern_Status = 1";
        strQuery = strQuery.Replace("ProjectWorkIdCond", ProjectWorkId.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_FundingPattern(tbl_FundingPattern obj_tbl_FundingPattern, int FundingPattern_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyFundingPattern(obj_tbl_FundingPattern.FundingPattern_Name, FundingPattern_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (FundingPattern_Id == 0)
                {
                    Insert_tbl_FundingPattern(obj_tbl_FundingPattern, trans, cn);
                }
                else
                {
                    obj_tbl_FundingPattern.FundingPattern_Id = FundingPattern_Id;
                    Update_tbl_FundingPattern(obj_tbl_FundingPattern, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyFundingPattern(string FundingPatternName, string FundingPattern_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_FundingPattern  where FundingPattern_Status = 1 and  FundingPattern_Name = '" + FundingPatternName + "' ";
        if (FundingPattern_Id != "0")
        {
            strQuery += " AND FundingPattern_Id  <> '" + FundingPattern_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_FundingPattern(tbl_FundingPattern obj_tbl_FundingPattern, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_FundingPattern( [FundingPattern_AddedBy],[FundingPattern_AddedOn],[FundingPattern_Name],[FundingPattern_Status] ) values('" + obj_tbl_FundingPattern.FundingPattern_AddedBy + "', getdate(), N'" + obj_tbl_FundingPattern.FundingPattern_Name + "','" + obj_tbl_FundingPattern.FundingPattern_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_FundingPattern(tbl_FundingPattern obj_tbl_FundingPattern, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_FundingPattern set  FundingPattern_Name = N'" + obj_tbl_FundingPattern.FundingPattern_Name + "',FundingPattern_ModifiedOn = getDate(),FundingPattern_ModifiedBy = '" + obj_tbl_FundingPattern.FundingPattern_AddedBy + "' where FundingPattern_Id = '" + obj_tbl_FundingPattern.FundingPattern_Id + "' and FundingPattern_Status = '" + obj_tbl_FundingPattern.FundingPattern_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_FundingPattern(int FundingPattern_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_FundingPattern set   FundingPattern_Status = 0 where FundingPattern_Id = '" + FundingPattern_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Unit
    public DataSet get_tbl_Unit()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Unit_Id, 
                        Unit_Name, 
                        Unit_Length_Applicable,
                        Unit_Bredth_Applicable,
                        Unit_Height_Applicable,
                        Unit_AddedOn, 
                        Unit_AddedBy, 
                        Unit_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Unit_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=Unit_ModifiedOn 
                    from tbl_Unit 
                    left join tbl_PersonDetail on Person_Id = Unit_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = Unit_ModifiedBy 
                    where Unit_Status = 1 order by Unit_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Unit(tbl_Unit obj_tbl_Unit, int Unit_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyUnit(obj_tbl_Unit.Unit_Name, Unit_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Unit_Id == 0)
                {
                    Insert_tbl_Unit(obj_tbl_Unit, trans, cn);
                }
                else
                {
                    obj_tbl_Unit.Unit_Id = Unit_Id;
                    Update_tbl_Unit(obj_tbl_Unit, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyUnit(string UnitName, string Unit_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Unit  where Unit_Status = 1 and  Unit_Name = '" + UnitName + "' ";
        if (Unit_Id != "0")
        {
            strQuery += " AND Unit_Id  <> '" + Unit_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Unit(tbl_Unit obj_tbl_Unit, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Unit( [Unit_AddedBy],[Unit_AddedOn],[Unit_Name],[Unit_Status], [Unit_Length_Applicable], [Unit_Bredth_Applicable], [Unit_Height_Applicable]) values('" + obj_tbl_Unit.Unit_AddedBy + "', getdate(), N'" + obj_tbl_Unit.Unit_Name + "','" + obj_tbl_Unit.Unit_Status + "','" + obj_tbl_Unit.Unit_Length_Applicable + "','" + obj_tbl_Unit.Unit_Bredth_Applicable + "','" + obj_tbl_Unit.Unit_Height_Applicable + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Unit(tbl_Unit obj_tbl_Unit, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Unit set  Unit_Name = N'" + obj_tbl_Unit.Unit_Name + "', Unit_ModifiedOn = getDate(),Unit_ModifiedBy = '" + obj_tbl_Unit.Unit_AddedBy + "', Unit_Length_Applicable = '" + obj_tbl_Unit.Unit_Length_Applicable + "', Unit_Bredth_Applicable = '" + obj_tbl_Unit.Unit_Bredth_Applicable + "', Unit_Height_Applicable = '" + obj_tbl_Unit.Unit_Height_Applicable + "' where Unit_Id = '" + obj_tbl_Unit.Unit_Id + "' and Unit_Status = '" + obj_tbl_Unit.Unit_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Unit(int Unit_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Unit set   Unit_Status = 0 where Unit_Id = '" + Unit_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Project Work Package
    public DataSet get_tbl_ProjectWorkPkg(int Work_Id, int Project_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, int ProjectWorkPkg_Id, string Project_Code, string Package_Code, bool ProjectWorkPkg_ApprovalFile_Path, string Status)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
						ProjectWorkPkg_Id,
						ProjectWorkPkg_Work_Id,
                        ProjectWorkPkg_Code, 
						ProjectWorkPkg_Name,
                        ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
						ProjectWorkPkg_AgreementAmount,
                        ProjectWorkPkg_Agreement_Path,
						ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
						ProjectWorkPkg_Agreement_No,
						ProjectWorkPkg_Indent_No,
						ProjectWorkPkg_Vendor_Id,
						ProjectWorkPkg_Staff_Id,
						Vendor_Name = Vendor.Person_Name,
						Vendor_Mobile = Vendor.Person_Mobile1,
						List_ReportingStaff_JEAPE_Name,
						List_ReportingStaff_AEPE_Name,
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Name, 
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
						ULB_Name, 
						Jurisdiction_Name_Eng, 
						Division_Name, 
						Circle_Name, 
						ProjectWork_DistrictId, 
						ProjectWork_ULB_Id, 
						ProjectWork_DivisionId, 
						Division_CircleId, 
                        ProjectWorkPkg_LockedOn, 
                        ProjectWorkPkg_Locked_By,
                        Total_BOQ = convert(varchar, isnull(t_PackageBOQ.Total_Approved, '0')) +' / ' + convert(varchar, isnull(t_PackageBOQ.Total_BOQ, '0')), 
                        Total_EMB = convert(varchar, isnull(t_PackageEMB.Total_Approved, '0')) +' / ' + convert(varchar, isnull(t_PackageEMB.Total_EMB, '0')),
                        isnull(ProjectWorkPkg_LastRABillNo,'0') as ProjectWorkPkg_LastRABillNo,
                        ProjectWorkPkg_ApprovalFile_Path, 
                        ProjectWorkPkg_LastRABillDate = convert(char(10), ProjectWorkPkg_LastRABillDate, 103), 
                        ProjectWorkPkg_LastRABillNo,
                        ProjectDPR_Id=isnull(ProjectDPR_Id,0),
                        ProjectDPR_DocumentDesignPath,
                        ProjectDPR_DPRPDFPath,
                        ProjectDPR_SitePic1Path,
                        ProjectDPR_SitePic2Path,
                        ProjectDPR_Comments,
                        DPRAddedBy,
                        ProjectDPR_AddedOn,
                        Convert(varchar(50),ProjectDPR_ReceivedAtHQDate,103) as ProjectDPR_ReceivedAtHQDate,
                        ProjectDPR_Verified_Comments,
                         Verifiedby,
                        ProjectDPR_VerifiedOn,
                        ProjectDPR_BudgetAllocated=cast(ProjectDPR_BudgetAllocated/100000 as decimal(18,2)),
						ProjectDPR_Approved_Comments,
						ProjectDPR_PhysicalProgressTrackingType,
						ApprovedBy,
						ProjectDPR_ApprovedOn
                    from tbl_ProjectWorkPkg
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on Project_Id = ProjectWork_Project_Id
					join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
					left join tbl_ULB on ULB_Id = ProjectWork_ULB_Id
					left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId
					left join tbl_PersonDetail Vendor on Vendor.Person_Id = ProjectWorkPkg_Vendor_Id
					left join tbl_PersonDetail Staff on Staff.Person_Id = ProjectWorkPkg_Staff_Id

                    left join (select ROW_NUMBER() Over(Partition by ProjectDPR_ProjectWorkPkg_Id Order by ProjectDPR_Id desc) as rr,
                    tbl_ProjectDPR.* ,DPR_Addedby.Person_Name as DPRAddedBy,Verifiedby.Person_Name as Verifiedby,ApprovedBy.Person_Name as ApprovedBy
                    from tbl_ProjectDPR 
                    left join tbl_PersonDetail as DPR_Addedby  on DPR_Addedby.Person_Id=ProjectDPR_Addedby 
                   left join tbl_PersonDetail as Verifiedby on Verifiedby.Person_Id=ProjectDPR_VerifiedBy
                    left join tbl_PersonDetail as ApprovedBy on ApprovedBy.Person_Id=ProjectDPR_ApprovedBy where ProjectDPR_Status=1) 
                    tbl_ProjectDPR on tbl_ProjectDPR.ProjectDPR_ProjectWorkPkg_Id=ProjectWorkPkg_Id and rr=1
					
                    left join (select PackageBOQ_Package_Id, Total_BOQ = count(*), Total_Approved = sum(case when isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 1 then 1 else 0 end) from tbl_PackageBOQ left join (select ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrr, PackageBOQ_Approval_Id, PackageBOQ_Approval_PackageBOQ_Id, PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), PackageBOQ_Approval_No, PackageBOQ_Approval_Comments, PackageBOQ_Approval_Approved_Qty, PackageBOQ_DocumentPath, PackageBOQ_Approval_Person_Id from tbl_PackageBOQ_Approval where PackageBOQ_Approval_Status = 1) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrr = 1 where PackageBOQ_Status = 1 group by PackageBOQ_Package_Id) t_PackageBOQ on PackageBOQ_Package_Id = ProjectWorkPkg_Id
                    
                    left join (select PackageEMB_Package_Id, Total_EMB = count(*), Total_Approved = sum(case when isnull(tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 0) > 1 then 1 else 0 end) from tbl_PackageEMB left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id from tbl_PackageEMB_Approval where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1 where PackageEMB_Status = 1 group by PackageEMB_Package_Id) t_PackageEMB on PackageEMB_Package_Id = ProjectWorkPkg_Id
        
                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                            where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id

                    where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1  ";
        if (Work_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Work_Id = '" + Work_Id + "'";
        }
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = '" + Project_Id + "'";
        }
        if (District_Id != 0)
        {
            strQuery += " and ProjectWork_DistrictId = '" + District_Id + "'";
        }
        if (Zone_Id != 0 && Division_Id == 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        if (Circle_Id != 0 && Division_Id == 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            //strQuery += " and ProjectWork_DivisionId = '" + Division_Id + "'";
            strQuery += "and (ProjectWork_DivisionId in (" + Division_Id + ") or ProjectWorkPkg_Id in (select ProjectAdditionalArea_ProjectWorkPkg_Id from tbl_ProjectAdditionalArea where ProjectAdditionalArea_Status = 1 and ProjectAdditionalArea_DevisionId = '" + Division_Id + "'))";
        }
        if (Project_Code != "")
        {
            strQuery += " and ProjectWork_ProjectCode = '" + Project_Code + "'";
        }
        if (Package_Code != "")
        {
            strQuery += " and ProjectWorkPkg_Code = '" + Package_Code + "'";
        }
        if (ProjectWorkPkg_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "'";
        }
        if (ProjectWorkPkg_ApprovalFile_Path == true)
        {
            strQuery += " and ISNULL(ProjectWorkPkg_ApprovalFile_Path,'') != '' ";
        }
        if (Status == "DPRUpload")
        {
            strQuery += " and isnull(ProjectDPR_Id,0) > 0 and isnull(ProjectDPR_IsVerified,0) = 0";
        }
        else if (Status == "IsVerified")
        {
            strQuery += " and isnull(ProjectDPR_IsVerified,0) > 0 and isnull(ProjectDPR_IsApproved,0) = 0";
        }
        else if (Status == "IsApproved")
        {
            strQuery += " and isnull(ProjectDPR_IsApproved,0) > 0";
        }
        else if (Status == "DPRUploadNew")
        {
            strQuery += " and isnull(ProjectDPR_Id,0) = 0";
        }

        strQuery += " order by ProjectWork_Name, ProjectWorkPkg_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectWorkPkg_ExtraItemApproval(int Work_Id, int Project_Id, int Zone_Id, int Circle_Id, int Division_Id, int ProjectWorkPkg_Id, string Status)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    Select  ProjectWorkPkg_Name,Package_ExtraItem_Id,
                            Package_ExtraItem_ProcessStatus,
                            Package_ExtraItem_AddedOn,
                            AddedBy.Person_Name as AddedBy,
                            Package_ExtraItem_ProcessedOn,
                            ProcessedBy.Person_Name as ProcessedBy,
                            Package_ExtraItem_ExtraItemFilePath,
                            Package_ExtraItem_ApprovalFilePath
                        from tbl_Package_ExtraItem
                        inner join tbl_ProjectWorkPkg on ProjectWorkPkg_Id=Package_ExtraItem_ProjectWorkPkg_Id
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                        join tbl_Project on Project_Id = ProjectWork_Project_Id
                        left join tbl_Division on Division_Id = ProjectWork_DivisionId
                        left join tbl_Circle on Circle_Id = Division_CircleId
                        inner join tbl_PersonDetail as AddedBy on AddedBy.Person_Id=Package_ExtraItem_AddedBy
                        left join tbl_PersonDetail as ProcessedBy on ProcessedBy.Person_Id=Package_ExtraItem_ProcessedBy  
                        where Package_ExtraItem_Status=1 ";
        if (Work_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Work_Id = '" + Work_Id + "'";
        }
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = '" + Project_Id + "'";
        }

        if (Zone_Id != 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        if (Circle_Id != 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            strQuery += " and ProjectWork_DivisionId = '" + Division_Id + "'";
        }
        if (ProjectWorkPkg_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "'";
        }
        if (Status == "Pending")
        {
            strQuery += " and Package_ExtraItem_ProcessStatus = 'Pending' ";
        }
        else if (Status == "Processed")
        {
            strQuery += " and Package_ExtraItem_ProcessStatus = 'Processed' ";
        }


        strQuery += " order by ProjectWorkPkg_Name ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool UpdateApproval_tbl_ProjectWorkPkg(tbl_ProjectWorkPkg obj_tbl_ProjectWorkPkg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        string strQuery = "";
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string fileName = "";
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\Pkg_Approval\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\Pkg_Approval\\");
                }

                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path_Bytes != null && obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Extention;
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path = "\\Downloads\\Pkg_Approval\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Pkg_Approval\\" + fileName, obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path_Bytes);
                }
                else
                {
                    if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Extention == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path = "";
                    }
                }

                string sql = "set dateformat dmy; update tbl_ProjectWorkPkg set  ProjectWorkPkg_ApprovalFile_Path = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_ApprovalFile_Path + "', ProjectWorkPkg__ApprovalRemark = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg__ApprovalRemark + "' where ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' ";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);


                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    public bool UpdateApproval_tbl_ProjectDPR(tbl_ProjectDPR obj_tbl_ProjectDPR)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        string strQuery = "";
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string fileName = "";
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\DPR\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\DPR\\");
                }

                if (obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Bytes != null && obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Extention;
                    obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath = "\\Downloads\\DPR\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\DPR\\" + fileName, obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath_Bytes);
                }
                else
                {
                    obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath = "";
                }
                if (obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Bytes != null && obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Extention;
                    obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath = "\\Downloads\\DPR\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\DPR\\" + fileName, obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath_Bytes);
                }
                else
                {

                    obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath = "";

                }
                if (obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Bytes != null && obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Extention;
                    obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path = "\\Downloads\\DPR\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\DPR\\" + fileName, obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path_Bytes);
                }
                else
                {

                    obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path = "";

                }
                if (obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Bytes != null && obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Extention;
                    obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path = "\\Downloads\\DPR\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\DPR\\" + fileName, obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path_Bytes);
                }
                else
                {

                    obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path = "";

                }

                if (obj_tbl_ProjectDPR != null)
                {
                    Insert_tbl_ProjectDPR(obj_tbl_ProjectDPR, trans, cn);
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private int Insert_tbl_ProjectDPR(tbl_ProjectDPR obj_tbl_ProjectDPR, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectDPR ( [ProjectDPR_AddedBy],[ProjectDPR_AddedOn],[ProjectDPR_Project_Id],[ProjectDPR_ProjectWork_Id],[ProjectDPR_Comments], [ProjectDPR_Status],ProjectDPR_DPRPDFPath,ProjectDPR_DocumentDesignPath,ProjectDPR_SitePic1Path,ProjectDPR_SitePic2Path,ProjectDPR_ProjectWorkPkg_Id) values ('" + obj_tbl_ProjectDPR.ProjectDPR_AddedBy + "', getdate(),'" + obj_tbl_ProjectDPR.ProjectDPR_Project_Id + "','" + obj_tbl_ProjectDPR.ProjectDPR_ProjectWork_Id + "',N'" + obj_tbl_ProjectDPR.ProjectDPR_Comments + "', '" + obj_tbl_ProjectDPR.ProjectDPR_Status + "', '" + obj_tbl_ProjectDPR.ProjectDPR_DPRPDFPath + "', '" + obj_tbl_ProjectDPR.ProjectDPR_DocumentDesignPath + "', '" + obj_tbl_ProjectDPR.ProjectDPR_SitePic1Path + "', '" + obj_tbl_ProjectDPR.ProjectDPR_SitePic2Path + "', '" + obj_tbl_ProjectDPR.ProjectDPR_ProjectWorkPkg_Id + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    public bool Insert_tbl_Package_ExtraItem(tbl_Package_ExtraItem obj_tbl_Package_ExtraItem, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        string strQuery = "";
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = "select * from tbl_Package_ExtraItem where Package_ExtraItem_ProjectWorkPkg_Id='" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ProjectWorkPkg_Id + "' and Package_ExtraItem_ProcessStatus='Pending' and Package_ExtraItem_Status=1";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);
                if (AllClasses.CheckDataSet(ds))
                {
                    Msg = "This Package Approval Is Pending.";
                    trans.Commit();
                    cn.Close();
                    flag = true;
                }
                else
                {

                    string fileName = "";
                    if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\Extra_Item\\"))
                    {
                        Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\Extra_Item\\");
                    }

                    if (obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath_Bytes != null && obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath_Bytes.Length > 0)
                    {
                        fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath_Extention;
                        obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath = "\\Downloads\\Extra_Item\\" + fileName;
                        File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Extra_Item\\" + fileName, obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath_Bytes);
                    }
                    else
                    {
                        obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath = "";
                    }

                    if (obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath_Bytes != null && obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath_Bytes.Length > 0)
                    {
                        fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath_Extention;
                        obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath = "\\Downloads\\Extra_Item\\" + fileName;
                        File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Extra_Item\\" + fileName, obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath_Bytes);
                    }
                    else
                    {
                        obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath = "";
                    }


                    sql = " set dateformat dmy;insert into tbl_Package_ExtraItem ( [Package_ExtraItem_AddedBy],[Package_ExtraItem_AddedOn],[Package_ExtraItem_ProjectWorkPkg_Id],[Package_ExtraItem_ProcessStatus],[Package_ExtraItem_Status], [Package_ExtraItem_ExtraItemFilePath],Package_ExtraItem_ApprovalFilePath) values ('" + obj_tbl_Package_ExtraItem.Package_ExtraItem_AddedBy + "', getdate(),'" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ProjectWorkPkg_Id + "','" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ProcessStatus + "','" + obj_tbl_Package_ExtraItem.Package_ExtraItem_Status + "', '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ExtraItemFilePath + "', '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ApprovalFilePath + "'); select @@IDENTITY";

                    ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);
                    trans.Commit();
                    cn.Close();
                    flag = true;
                }


            }
            catch (Exception ex)
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool ProcessedApproval_tbl_Package_ExtraItem(tbl_Package_ExtraItem obj_tbl_Package_ExtraItem)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        string strQuery = "";
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {

                string sql = "set dateformat dmy; update tbl_Package_ExtraItem set  Package_ExtraItem_ProcessStatus = '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ProcessStatus + "',Package_ExtraItem_ProcessedOn = getdate(),Package_ExtraItem_ProcessedBy = '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_ProcessedBy + "',Package_ExtraItem_Comment = '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_Comment + "' where Package_ExtraItem_Id = '" + obj_tbl_Package_ExtraItem.Package_ExtraItem_Id + "' ";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }


    public bool Insert_tbl_ProjectWorkPkg(tbl_ProjectWorkPkg obj_tbl_ProjectWorkPkg, List<tbl_ProjectPkg_PhysicalProgress> obj_tbl_ProjectPkg_PhysicalProgress_Li, List<tbl_ProjectPkg_Deliverables> obj_tbl_ProjectPkg_Deliverables_Li, List<tbl_ProjectWorkPkg_ReportingStaff_JE_APE> obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li, List<tbl_ProjectWorkPkg_ReportingStaff_AE_PE> obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li, List<tbl_ProjectAdditionalArea> obj_tbl_ProjectAdditionalArea_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string fileName = "";
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\AG\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\AG\\");
                }
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\BG\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\BG\\");
                }
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\PS\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\PS\\");
                }
                if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\MA\\"))
                {
                    Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\MA\\");
                }

                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementPath_Bytes != null && obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementPath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Extention;
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Path = "\\Downloads\\AG\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\AG\\" + fileName, obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementPath_Bytes);
                }
                else
                {
                    if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Extention == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Path = "";
                    }
                }
                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGuranteePath_Bytes != null && obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGuranteePath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Extention;
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Path = "\\Downloads\\BG\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\BG\\" + fileName, obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGuranteePath_Bytes);
                }
                else
                {
                    if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Extention == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Path = "";
                    }
                }

                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobelizationPath_Bytes != null && obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobelizationPath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Extention;
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Path = "\\Downloads\\MA\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\MA\\" + fileName, obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobelizationPath_Bytes);
                }
                else
                {
                    if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Extention == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Path = "";
                    }
                }

                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurityPath_Bytes != null && obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurityPath_Bytes.Length > 0)
                {
                    fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Extention;
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Path = "\\Downloads\\PS\\" + fileName;
                    File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\PS\\" + fileName, obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurityPath_Bytes);
                }
                else
                {
                    if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Extention == "-1")
                    {

                    }
                    else
                    {
                        obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Path = "";
                    }
                }
                if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id == 0)
                {
                    obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id = Insert_tbl_ProjectWorkPkg(obj_tbl_ProjectWorkPkg, trans, cn);
                }
                else
                {
                    Update_tbl_ProjectWorkPkg(obj_tbl_ProjectWorkPkg, trans, cn);
                }

                string sql = "set dateformat dmy; update tbl_Projectpkg_PhysicalProgress set Projectpkg_PhysicalProgress_ModifiedOn = getdate(), Projectpkg_PhysicalProgress_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', Projectpkg_PhysicalProgress_Status = 0 where Projectpkg_PhysicalProgress_Status = 1 and  ProjectPkg_PhysicalProgress_ProjectPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' and ProjectPkg_PhysicalProgress_PrjectWork_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                if (obj_tbl_ProjectPkg_PhysicalProgress_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectPkg_PhysicalProgress_Li.Count; i++)
                    {
                        obj_tbl_ProjectPkg_PhysicalProgress_Li[i].ProjectPkg_PhysicalProgress_ProjectPkg_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id;
                        obj_tbl_ProjectPkg_PhysicalProgress_Li[i].ProjectPkg_PhysicalProgress_PrjectWork_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id;
                        Insert_tbl_ProjectPkg_PhysicalProgress(obj_tbl_ProjectPkg_PhysicalProgress_Li[i], trans, cn);
                    }
                }


                sql = "set dateformat dmy; update tbl_ProjectPkg_Deliverables set ProjectPkg_Deliverables_ModifiedOn = getdate(), ProjectPkg_Deliverables_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', ProjectPkg_Deliverables_Status = 0 where ProjectPkg_Deliverables_Status = 1 and  ProjectPkg_Deliverables_ProjectPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' and ProjectPkg_Deliverables_ProjectWork_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                if (obj_tbl_ProjectPkg_Deliverables_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectPkg_Deliverables_Li.Count; i++)
                    {
                        obj_tbl_ProjectPkg_Deliverables_Li[i].ProjectPkg_Deliverables_ProjectPkg_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id;
                        obj_tbl_ProjectPkg_Deliverables_Li[i].ProjectPkg_Deliverables_ProjectWork_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id;
                        Insert_tbl_ProjectPkg_Deliverables(obj_tbl_ProjectPkg_Deliverables_Li[i], trans, cn);
                    }
                }

                sql = "set dateformat dmy; update tbl_ProjectWorkPkg_ReportingStaff_JE_APE set ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedOn = getdate(), ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', ProjectWorkPkg_ReportingStaff_JE_APE_Status = 0 where ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1 and  ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' ";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                if (obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li.Count; i++)
                    {
                        obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li[i].ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id;
                        Insert_tbl_ProjectWorkPkg_ReportingStaff_JE_APE(obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE_Li[i], trans, cn);
                    }
                }

                sql = "set dateformat dmy; update tbl_ProjectWorkPkg_ReportingStaff_AE_PE set ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedOn = getdate(), ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', ProjectWorkPkg_ReportingStaff_AE_PE_Status = 0 where ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1 and  ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' ";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                if (obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li.Count; i++)
                    {
                        obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li[i].ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id;
                        Insert_tbl_ProjectWorkPkg_ReportingStaff_AE_PE(obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE_Li[i], trans, cn);
                    }
                }

                if (obj_tbl_ProjectAdditionalArea_Li != null)
                {
                    string strQuery = "";
                    strQuery = " set dateformat dmy; update tbl_ProjectAdditionalArea set ProjectAdditionalArea_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "',ProjectAdditionalArea_ModifiedOn=getdate(),ProjectAdditionalArea_Status=0 where ProjectAdditionalArea_ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "' and ProjectAdditionalArea_Status=1 ";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                    for (int i = 0; i < obj_tbl_ProjectAdditionalArea_Li.Count; i++)
                    {
                        obj_tbl_ProjectAdditionalArea_Li[i].ProjectAdditionalArea_ProjectWork_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id;
                        obj_tbl_ProjectAdditionalArea_Li[i].ProjectAdditionalArea_ProjectWorkPkg_Id = obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id;
                        Insert_tbl_ProjectAdditionalArea(obj_tbl_ProjectAdditionalArea_Li[i], trans, cn);
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private int Insert_tbl_ProjectPkg_PhysicalProgress(tbl_ProjectPkg_PhysicalProgress obj_tbl_ProjectPkg_PhysicalProgress, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkg_PhysicalProgress ( [ProjectPkg_PhysicalProgress_AddedBy],[ProjectPkg_PhysicalProgress_AddedOn],[ProjectPkg_PhysicalProgress_PrjectWork_Id],[ProjectPkg_PhysicalProgress_ProjectPkg_Id],[ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id], [ProjectPkg_PhysicalProgress_Status]) values ('" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_AddedBy + "', getdate(),'" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_PrjectWork_Id + "','" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_ProjectPkg_Id + "','" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id + "', '" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectPkg_Deliverables(tbl_ProjectPkg_Deliverables obj_tbl_ProjectPkg_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkg_Deliverables ( [ProjectPkg_Deliverables_AddedBy],[ProjectPkg_Deliverables_AddedOn],[ProjectPkg_Deliverables_ProjectWork_Id],[ProjectPkg_Deliverables_ProjectPkg_Id],[ProjectPkg_Deliverables_Deliverables_Id], [ProjectPkg_Deliverables_Status]) values ('" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_AddedBy + "', getdate(),'" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_ProjectWork_Id + "','" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_ProjectPkg_Id + "','" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_Deliverables_Id + "', '" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectWorkPkg_ReportingStaff_JE_APE(tbl_ProjectWorkPkg_ReportingStaff_JE_APE obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectWorkPkg_ReportingStaff_JE_APE ( [ProjectWorkPkg_ReportingStaff_JE_APE_AddedBy],[ProjectWorkPkg_ReportingStaff_JE_APE_AddedOn],[ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id],[ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id], [ProjectWorkPkg_ReportingStaff_JE_APE_Status]) values ('" + obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_AddedBy + "', getdate(),'" + obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id + "','" + obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id + "','" + obj_tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectWorkPkg_ReportingStaff_AE_PE(tbl_ProjectWorkPkg_ReportingStaff_AE_PE obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectWorkPkg_ReportingStaff_AE_PE ( [ProjectWorkPkg_ReportingStaff_AE_PE_AddedBy],[ProjectWorkPkg_ReportingStaff_AE_PE_AddedOn],[ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id],[ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id], [ProjectWorkPkg_ReportingStaff_AE_PE_Status]) values ('" + obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_AddedBy + "', getdate(),'" + obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id + "','" + obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id + "','" + obj_tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectWorkPkg(tbl_ProjectWorkPkg obj_tbl_ProjectWorkPkg, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillDate == "")
        {
            strQuery = " set dateformat dmy;insert into tbl_ProjectWorkPkg ( [ProjectWorkPkg_AddedBy],[ProjectWorkPkg_AddedOn],[ProjectWorkPkg_Agreement_Date],[ProjectWorkPkg_Agreement_No],[ProjectWorkPkg_Agreement_Path],[ProjectWorkPkg_AgreementAmount],[ProjectWorkPkg_Indent_No],[ProjectWorkPkg_Name],[ProjectWorkPkg_Staff_Id],[ProjectWorkPkg_Status],[ProjectWorkPkg_Vendor_Id],[ProjectWorkPkg_Work_Id], [ProjectWorkPkg_Due_Date], [ProjectWorkPkg_Code], [ProjectWorkPkg_BankGurantee_Path], [ProjectWorkPkg_Mobelization_Path], [ProjectWorkPkg_PerformanceSecurity_Path],ProjectWorkPkg_LastRABillNo,ProjectWorkPkg_MobilizationAdvanceAmount) values ('" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', getdate(), convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Date + "', 103), N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_No + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Path + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementAmount + "',N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Indent_No + "',N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Name + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Staff_Id + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Status + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Vendor_Id + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id + "', convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Due_Date + "', 103), '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Code + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillNo + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobilizationAdvanceAmount + "'); Select @@Identity";
        }
        else
        {
            strQuery = " set dateformat dmy;insert into tbl_ProjectWorkPkg ( [ProjectWorkPkg_AddedBy],[ProjectWorkPkg_AddedOn],[ProjectWorkPkg_Agreement_Date],[ProjectWorkPkg_Agreement_No],[ProjectWorkPkg_Agreement_Path],[ProjectWorkPkg_AgreementAmount],[ProjectWorkPkg_Indent_No],[ProjectWorkPkg_Name],[ProjectWorkPkg_Staff_Id],[ProjectWorkPkg_Status],[ProjectWorkPkg_Vendor_Id],[ProjectWorkPkg_Work_Id], [ProjectWorkPkg_Due_Date], [ProjectWorkPkg_Code], [ProjectWorkPkg_BankGurantee_Path], [ProjectWorkPkg_Mobelization_Path], [ProjectWorkPkg_PerformanceSecurity_Path],ProjectWorkPkg_LastRABillNo,ProjectWorkPkg_LastRABillDate,ProjectWorkPkg_MobilizationAdvanceAmount) values ('" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', getdate(), convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Date + "', 103), N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_No + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Path + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementAmount + "',N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Indent_No + "',N'" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Name + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Staff_Id + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Status + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Vendor_Id + "','" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Work_Id + "', convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Due_Date + "', 103), '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Code + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_BankGurantee_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Mobelization_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_PerformanceSecurity_Path + "', '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillNo + "', convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillDate + "', 103), '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobilizationAdvanceAmount + "'); Select @@Identity";
        }
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private void Update_tbl_ProjectWorkPkg(tbl_ProjectWorkPkg obj_tbl_ProjectWorkPkg, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        if (obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillDate == "")
        {
            strQuery = " set dateformat dmy;update tbl_ProjectWorkPkg set ProjectWorkPkg_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', ProjectWorkPkg_ModifiedOn = getdate(),  ProjectWorkPkg_Agreement_Date =  convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Date + "', 103), ProjectWorkPkg_Due_Date = convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Due_Date + "', 103), ProjectWorkPkg_Agreement_No = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_No + "', ProjectWorkPkg_AgreementAmount = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementAmount + "', ProjectWorkPkg_Name = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Name + "', ProjectWorkPkg_Code = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Code + "',ProjectWorkPkg_Vendor_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Vendor_Id + "', ProjectWorkPkg_Staff_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Staff_Id + "', ProjectWorkPkg_Indent_No = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Indent_No + "', ProjectWorkPkg_LastRABillNo = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillNo + "', ProjectWorkPkg_MobilizationAdvanceAmount = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobilizationAdvanceAmount + "' where ProjectWorkPkg_Status = 1 and ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "'";
        }
        else
        {
            strQuery = " set dateformat dmy;update tbl_ProjectWorkPkg set ProjectWorkPkg_ModifiedBy = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AddedBy + "', ProjectWorkPkg_ModifiedOn = getdate(),  ProjectWorkPkg_Agreement_Date =  convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_Date + "', 103), ProjectWorkPkg_Due_Date = convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Due_Date + "', 103), ProjectWorkPkg_Agreement_No = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Agreement_No + "', ProjectWorkPkg_AgreementAmount = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_AgreementAmount + "', ProjectWorkPkg_Name = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Name + "', ProjectWorkPkg_Code = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Code + "',ProjectWorkPkg_Vendor_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Vendor_Id + "', ProjectWorkPkg_Staff_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Staff_Id + "', ProjectWorkPkg_Indent_No = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Indent_No + "', ProjectWorkPkg_LastRABillNo = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillNo + "',ProjectWorkPkg_LastRABillDate = convert(date, '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_LastRABillDate + "', 103), ProjectWorkPkg_MobilizationAdvanceAmount = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_MobilizationAdvanceAmount + "' where ProjectWorkPkg_Status = 1 and ProjectWorkPkg_Id = '" + obj_tbl_ProjectWorkPkg.ProjectWorkPkg_Id + "'";
        }
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Update_tbl_ProjectWorkPkg_Lock(List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li, int ProjectWorkPkg_Id, int Added_By)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string strQuery = "";
                for (int i = 0; i < obj_tbl_PackageBOQ_Approval_Li.Count; i++)
                {
                    tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = obj_tbl_PackageBOQ_Approval_Li[i];
                    strQuery = " set dateformat dmy;insert into tbl_PackageBOQ_Approval ( [PackageBOQ_Approval_AddedBy],[PackageBOQ_Approval_AddedOn],[PackageBOQ_Approval_Approved_Qty],[PackageBOQ_Approval_Comments],[PackageBOQ_Approval_Date],[PackageBOQ_Approval_No],[PackageBOQ_Approval_PackageBOQ_Id],[PackageBOQ_Approval_Person_Id],[PackageBOQ_Approval_Status],[PackageBOQ_DocumentPath] ) values ('" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy + "', getdate(),'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath + "');Select @@Identity";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }

                strQuery = @"set dateformat dmy; 
                            update tbl_ProjectWorkPkg set ProjectWorkPkg_Locked_By = '" + Added_By + "', ProjectWorkPkg_LockedOn = getdate() where ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "'";
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    public DataSet get_tbl_ProjectWorkPkg_Edit(int ProjectWorkPkg_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select ProjectWorkPkg_Id,
                    ProjectWorkPkg_Agreement_Date= convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                    ProjectWorkPkg_Due_Date= convert(char(10), ProjectWorkPkg_Due_Date, 103),
                    ProjectWorkPkg_LastRABillNo,
                    ProjectWorkPkg_LastRABillDate= convert(char(10), ProjectWorkPkg_LastRABillDate, 103),
                    ProjectWorkPkg_Agreement_No,
                    ProjectWorkPkg_AgreementAmount,
                    ProjectWorkPkg_Name,ProjectWorkPkg_Code,
                    ProjectWorkPkg_Vendor_Id,
                    ProjectWorkPkg_Staff_Id,
                    ProjectWorkPkg_Indent_No,
                    List_ReportingStaff_JEAPE_Id,
                    List_ReportingStaff_AEPE_Id,
                    ProjectWorkPkg_MobilizationAdvanceAmount=Isnull(ProjectWorkPkg_MobilizationAdvanceAmount,0)
                    from tbl_ProjectWorkPkg 
                    left join(
                            SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Id
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id
                              )tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                            SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
													WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Id
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                            where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id
                              )tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    where ProjectWorkPkg_Id='" + ProjectWorkPkg_Id + "' ";

        strQuery += @" select PhysicalProgressComponent_Id,
                        PhysicalProgressComponent_Component,
                        Unit_Name,
                        ProjectPkg_PhysicalProgress_Id=isnull(ProjectPkg_PhysicalProgress_Id,0)
                        from tbl_PhysicalProgressComponent
                        left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
                        left join tbl_ProjectPkg_PhysicalProgress on ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id=PhysicalProgressComponent_Id and ProjectPkg_PhysicalProgress_ProjectPkg_Id='" + ProjectWorkPkg_Id + "'  and ProjectPkg_PhysicalProgress_Status=1 where PhysicalProgressComponent_Status=1 ";

        strQuery += @" select Deliverables_Id,
                        Deliverables_Deliverables,
                        Unit_Name,
                        ProjectPkg_Deliverables_Id=isnull(ProjectPkg_Deliverables_Id,0)
                        from tbl_Deliverables
                        left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                        left join tbl_ProjectPkg_Deliverables on ProjectPkg_Deliverables_Deliverables_Id=Deliverables_Id and ProjectPkg_Deliverables_ProjectPkg_Id='" + ProjectWorkPkg_Id + "'  and ProjectPkg_Deliverables_Status=1 where Deliverables_Status=1 ";

        strQuery += @" select ProjectAdditionalArea_ZoneId,Zone_Name,ProjectAdditionalArea_CircleId,Circle_Name,ProjectAdditionalArea_DevisionId,Division_Name 
                        from tbl_ProjectAdditionalArea
                        inner join tbl_Zone on Zone_Id=ProjectAdditionalArea_ZoneId
                        inner join tbl_Circle on Circle_Id=ProjectAdditionalArea_CircleId
                        inner join tbl_Division on Division_Id=ProjectAdditionalArea_DevisionId
                        where ProjectAdditionalArea_ProjectWorkPkg_Id='" + ProjectWorkPkg_Id + "' and ProjectAdditionalArea_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Delete_tbl_ProjectWorkPkg(int ProjectWorkPkg_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ProjectWorkPkg set   ProjectWorkPkg_Status = 0,ProjectWorkPkg_ModifiedBy='" + person_Id + "',ProjectWorkPkg_ModifiedOn=getdate() where ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "' ";
            strQuery += " set dateformat dmy; Update  tbl_ProjectPkg_PhysicalProgress set   ProjectPkg_PhysicalProgress_Status = 0,ProjectPkg_PhysicalProgress_ModifiedBy='" + person_Id + "',ProjectPkg_PhysicalProgress_ModifiedOn=getdate() where ProjectPkg_PhysicalProgress_ProjectPkg_Id = '" + ProjectWorkPkg_Id + "' ";
            strQuery += " set dateformat dmy; Update  tbl_ProjectPkg_Deliverables set   ProjectPkg_Deliverables_Status = 0,ProjectPkg_Deliverables_ModifiedBy='" + person_Id + "',ProjectPkg_Deliverables_ModifiedOn=getdate() where ProjectPkg_Deliverables_ProjectPkg_Id = '" + ProjectWorkPkg_Id + "' ";
            strQuery += " set dateformat dmy; Update  tbl_ProjectWorkPkg_ReportingStaff_JE_APE set   ProjectWorkPkg_ReportingStaff_JE_APE_Status = 0,ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedBy='" + person_Id + "',ProjectWorkPkg_ReportingStaff_JE_APE_ModifiedOn=getdate() where ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "' ";
            strQuery += " set dateformat dmy; Update  tbl_ProjectWorkPkg_ReportingStaff_AE_PE set   ProjectWorkPkg_ReportingStaff_AE_PE_Status = 0,ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedBy='" + person_Id + "',ProjectWorkPkg_ReportingStaff_AE_PE_ModifiedOn=getdate() where ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Package BOQ
    public DataSet get_tbl_PackageBOQ_UodationReport_EmployeeWise()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select  
                        Person_Name as UpdatedBy,
                        Person_Id,
                        COUNT(*) as NoOfUpdation, 
					    Zone_Name, 
                        Circle_Name, 
                        Division_Name
                    from tbl_PackageBOQ_History
					join tbl_PackageBOQ on tbl_PackageBOQ.PackageBOQ_Id = tbl_PackageBOQ_History.PackageBOQ_Id
                    left join tbl_PersonDetail on Person_Id=tbl_PackageBOQ.PackageBOQ_ModifiedBy
					left join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
					left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
					left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
					left join tbl_Division on Division_Id = PersonJuridiction_DivisionId
                    where tbl_PackageBOQ_History.PackageBOQ_Status = 1
                    group by Person_Name,Person_Id, Zone_Name, Circle_Name, Division_Name ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_PackageBOQ_UodationReport_ItemWise(int UpdatedBy)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select  
                        Specification_Old = tbl_PackageBOQ_History.PackageBOQ_Specification,
                        Specification_New = tbl_PackageBOQ.PackageBOQ_Specification,
						Specification_Change = case when tbl_PackageBOQ_History.PackageBOQ_Specification != tbl_PackageBOQ.PackageBOQ_Specification then 1 else 0 end,
                        Unit_Name,
                        Unit_Id_Old = tbl_PackageBOQ_History.PackageBOQ_Unit_Id,
                        Unit_Id_New = tbl_PackageBOQ.PackageBOQ_Unit_Id,
						Unit_Change = case when tbl_PackageBOQ_History.PackageBOQ_Unit_Id != tbl_PackageBOQ.PackageBOQ_Unit_Id then 1 else 0 end,
                        tbl_PackageBOQ_History.PackageBOQ_Qty,
                        tbl_PackageBOQ_History.PackageBOQ_RateEstimated,
                        tbl_PackageBOQ_History.PackageBOQ_AmountEstimated,
                        tbl_PackageBOQ_History.PackageBOQ_RateQuoted,
                        tbl_PackageBOQ_History.PackageBOQ_AmountQuoted,
                        QtyPaid_Old = tbl_PackageBOQ_History.PackageBOQ_QtyPaid,
                        QtyPaid_New = tbl_PackageBOQ.PackageBOQ_QtyPaid,
						QtyPaid_Change = case when tbl_PackageBOQ_History.PackageBOQ_QtyPaid != tbl_PackageBOQ.PackageBOQ_QtyPaid then 1 else 0 end,
                        QtyPaid_Percentage_Old = tbl_PackageBOQ_History.PackageBOQ_PercentageValuePaidTillDate,
                        QtyPaid_Percentage_New = tbl_PackageBOQ.PackageBOQ_PercentageValuePaidTillDate,
                        QtyPaid_Percentage_Change = case when tbl_PackageBOQ_History.PackageBOQ_PercentageValuePaidTillDate != tbl_PackageBOQ.PackageBOQ_PercentageValuePaidTillDate then 1 else 0 end,
                        tbl_PackageBOQ_History.GSTType,
                        tbl_PackageBOQ_History.GSTPercenatge,
                        Person_Name as UpdatedBy,
                        tbl_PackageBOQ_History.PackageBOQ_ModifiedOn as UpdatedDate
                    from tbl_PackageBOQ_History
					join tbl_PackageBOQ on tbl_PackageBOQ.PackageBOQ_Id = tbl_PackageBOQ_History.PackageBOQ_Id
                    left join tbl_Unit on Unit_Id= tbl_PackageBOQ_History.PackageBOQ_Unit_Id
                    left join tbl_PersonDetail on Person_Id= tbl_PackageBOQ_History.PackageBOQ_ModifiedBy
                    where tbl_PackageBOQ_History.PackageBOQ_Status=1 and tbl_PackageBOQ.PackageBOQ_ModifiedBy= " + UpdatedBy + " order by tbl_PackageBOQ_History.PackageBOQ_ModifiedOn desc";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_PackageBOQ(int Work_Id, int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageBOQ_Id,
	                    PackageBOQ_Package_Id,
	                    PackageBOQ_Specification,
	                    PackageBOQ_Unit_Id,
                        Unit_Name,
	                    PackageBOQ_Qty,
                        PackageBOQ_QtyPaid,
	                    PackageBOQ_RateEstimated,
	                    PackageBOQ_AmountEstimated,
	                    PackageBOQ_RateQuoted,
	                    PackageBOQ_AmountQuoted,
	                    PackageBOQ_AddedBy,
	                    PackageBOQ_AddedOn,
	                    PackageBOQ_Status, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments,
                        isnull(PackageBOQ_PercentageValuePaidTillDate,0) as PackageBOQ_PercentageValuePaidTillDate,
                        GSTType,
                        GSTPercenatge
                    from tbl_PackageBOQ
                    join tbl_ProjectWorkPkg on PackageBOQ_Package_Id = ProjectWorkPkg_Id
                    left join tbl_Unit on Unit_Id=PackageBOQ_Unit_Id
                    left join (select ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrr, PackageBOQ_Approval_Id, PackageBOQ_Approval_PackageBOQ_Id, PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), PackageBOQ_Approval_No, PackageBOQ_Approval_Comments, PackageBOQ_Approval_Approved_Qty, PackageBOQ_DocumentPath, PackageBOQ_Approval_Person_Id, PackageBOQ_Approval_Person = Person_Name from tbl_PackageBOQ_Approval join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id where PackageBOQ_Approval_Status = 1) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrr = 1
                    where PackageBOQ_Status = 1 ";
        if (Package_Id > 0)
        {
            strQuery += " and PackageBOQ_Package_Id = '" + Package_Id + "'";
        }
        if (Work_Id > 0)
        {
            strQuery += " and ProjectWorkPkg_Work_Id = '" + Work_Id + "'";
        }
        strQuery += " order by PackageBOQ_OrderNo";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_PackageBOQ(List<tbl_PackageBOQ> obj_tbl_PackageBOQ_Li, int Default_Approved)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PackageBOQ_Li.Count; i++)
                {
                    if (obj_tbl_PackageBOQ_Li[i].PackageBOQ_Id == 0)
                    {
                        obj_tbl_PackageBOQ_Li[i].PackageBOQ_Id = Insert_tbl_PackageBOQ(obj_tbl_PackageBOQ_Li[i], trans, cn);
                        if (Default_Approved == 1)
                        {
                            tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = obj_tbl_PackageBOQ_Li[i].PackageBOQ_Qty;
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = "";
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = "";
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = obj_tbl_PackageBOQ_Li[i].PackageBOQ_Id;
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
                            obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";
                            Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval, trans, cn);
                        }
                    }
                    else
                    {
                        if (obj_tbl_PackageBOQ_Li[i].PackageBOQ_Is_Approved == 0)
                        {
                            Update_tbl_PackageBOQ(obj_tbl_PackageBOQ_Li[i], trans, cn);
                        }
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Update_tbl_PackageBOQ(List<tbl_PackageBOQ> obj_tbl_PackageBOQ_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PackageBOQ_Li.Count; i++)
                {
                    Update_tbl_PackageBOQ(obj_tbl_PackageBOQ_Li[i], trans, cn);
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Update_tbl_PackageBOQ(tbl_PackageBOQ obj_tbl_PackageBOQ, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;update tbl_PackageBOQ set [PackageBOQ_ModifiedBy] = '" + obj_tbl_PackageBOQ.PackageBOQ_AddedBy + "', [PackageBOQ_ModifiedOn] = getdate(),  [PackageBOQ_AmountEstimated] = '" + obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated + "', [PackageBOQ_AmountQuoted] = '" + obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted + "', [PackageBOQ_Qty] = '" + obj_tbl_PackageBOQ.PackageBOQ_Qty + "', [PackageBOQ_RateEstimated] = '" + obj_tbl_PackageBOQ.PackageBOQ_RateEstimated + "', [PackageBOQ_RateQuoted] = '" + obj_tbl_PackageBOQ.PackageBOQ_RateQuoted + "', [PackageBOQ_Specification] = N'" + obj_tbl_PackageBOQ.PackageBOQ_Specification + "', [PackageBOQ_Unit_Id] = '" + obj_tbl_PackageBOQ.PackageBOQ_Unit_Id + "', [PackageBOQ_Package_Id] = '" + obj_tbl_PackageBOQ.PackageBOQ_Package_Id + "', PackageBOQ_QtyPaid = '" + obj_tbl_PackageBOQ.PackageBOQ_QtyPaid + "', PackageBOQ_PercentageValuePaidTillDate = '" + obj_tbl_PackageBOQ.PackageBOQ_PercentageValuePaidTillDate + "', GSTType = '" + obj_tbl_PackageBOQ.GSTType + "', GSTPercenatge = '" + obj_tbl_PackageBOQ.GSTPercenatge + "' where [PackageBOQ_Status] = 1 and [PackageBOQ_Id] = '" + obj_tbl_PackageBOQ.PackageBOQ_Id + "'";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private int Insert_tbl_PackageBOQ(tbl_PackageBOQ obj_tbl_PackageBOQ, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageBOQ ( [PackageBOQ_AddedBy],[PackageBOQ_AddedOn],[PackageBOQ_AmountEstimated],[PackageBOQ_AmountQuoted],[PackageBOQ_Package_Id],[PackageBOQ_Qty],[PackageBOQ_RateEstimated],[PackageBOQ_RateQuoted],[PackageBOQ_Specification],[PackageBOQ_Status],[PackageBOQ_Unit_Id], [PackageBOQ_QtyPaid],PackageBOQ_OrderNo) values ('" + obj_tbl_PackageBOQ.PackageBOQ_AddedBy + "', getdate(),'" + obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated + "','" + obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted + "','" + obj_tbl_PackageBOQ.PackageBOQ_Package_Id + "','" + obj_tbl_PackageBOQ.PackageBOQ_Qty + "','" + obj_tbl_PackageBOQ.PackageBOQ_RateEstimated + "','" + obj_tbl_PackageBOQ.PackageBOQ_RateQuoted + "',N'" + obj_tbl_PackageBOQ.PackageBOQ_Specification + "','" + obj_tbl_PackageBOQ.PackageBOQ_Status + "','" + obj_tbl_PackageBOQ.PackageBOQ_Unit_Id + "','" + obj_tbl_PackageBOQ.PackageBOQ_QtyPaid + "','" + obj_tbl_PackageBOQ.PackageBOQ_OrderNo + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    public bool Delete_tbl_PackageBOQ(int PackageBOQ_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_PackageBOQ set PackageBOQ_Status = 0, PackageBOQ_ModifiedBy = '" + person_Id + "', PackageBOQ_ModifiedOn = getdate() where PackageBOQ_Id = '" + PackageBOQ_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Insert_tbl_PackageBOQ_Approval(List<tbl_PackageBOQ_Approval> obj_tbl_PackageBOQ_Approval_Li, decimal? Approved_Qty)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string strQuery = "";
                for (int i = 0; i < obj_tbl_PackageBOQ_Approval_Li.Count; i++)
                {
                    Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval_Li[i], trans, cn);
                }
                if (Approved_Qty != null)
                {
                    tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = obj_tbl_PackageBOQ_Approval_Li[0];
                    strQuery = @"set dateformat dmy; 
                    update tbl_PackageBOQ set PackageBOQ_Qty = '" + Approved_Qty + "', PackageBOQ_ModifiedBy = '" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy + "', PackageBOQ_ModifiedOn = getdate() where PackageBOQ_Id = '" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Update_tbl_EMB_Set_Billing(List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li, tbl_PackageInvoice obj_tbl_PackageInvoice, List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li, int ProjectWorkPkg_Id, int Added_By, tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval, tbl_PackageEMBApproval obj_tbl_PackageEMBApproval, int OrgId, int Designation_Id, List<tbl_PackageEMB_Tax> obj_tbl_PackageEMB_Tax_Li, List<tbl_PackageEMB> obj_tbl_PackageEMB_Li, tbl_PackageEMB_Master obj_tbl_PackageEMB_Master, List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_li, List<tbl_PackageInvoiceEMBMasterLink> obj_tbl_PackageInvoiceEMBMasterLink_Li, List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li, tbl_PackageEMB_ExtraItem obj_tbl_PackageEMB_ExtraItem)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_PackageEMB_Approval_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageEMB_Approval_Li.Count; i++)
                    {
                        Insert_tbl_PackageEMB_Approval(obj_tbl_PackageEMB_Approval_Li[i], trans, cn);
                    }
                }
                if (obj_tbl_PackageEMB_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageEMB_Li.Count; i++)
                    {
                        string strQuery = "";
                        strQuery = " set dateformat dmy; Update  tbl_PackageEMB set   PackageEMB_Qty = '" + obj_tbl_PackageEMB_Li[i].PackageEMB_Qty + "'  where  PackageEMB_Id = '" + obj_tbl_PackageEMB_Li[i].PackageEMB_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    }
                }
                if (obj_tbl_PackageInvoiceItem_Li != null && obj_tbl_PackageInvoiceItem_Li.Count > 0)
                {
                    if (obj_tbl_PackageInvoice.PackageInvoice_VoucherNo == "")
                    {
                        obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = get_tbl_TransactionNos(VoucherTypes.Invoice, obj_tbl_PackageInvoice.PackageInvoice_Package_Id, trans, cn);
                    }
                    //obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id;
                    obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);

                    for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
                    {
                        obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                        obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id = Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                        if (obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li != null)
                        {
                            for (int k = 0; k < obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li.Count; k++)
                            {
                                obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k].PackageInvoiceItem_Tax_PackageInvoiceItem_Id = obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id;
                                Insert_tbl_PackageInvoiceItem_Tax(obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k], trans, cn); ;
                            }
                        }
                    }

                    if (obj_tbl_PackageInvoiceAdditional_li != null)
                    {
                        for (int i = 0; i < obj_tbl_PackageInvoiceAdditional_li.Count; i++)
                        {
                            obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                            string strQuery = "";
                            strQuery = " set dateformat dmy; Update  tbl_PackageInvoiceAdditional set   PackageInvoiceAdditional_ModifiedOn = getdate(),PackageInvoiceAdditional_ModifiedBy='" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_AddedBy + "',PackageInvoiceAdditional_Status=0 where PackageInvoiceAdditional_Invoice_Id = '" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Invoice_Id + "' and PackageInvoiceAdditional_Status=1 and PackageInvoiceAdditional_Deduction_Id='" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Deduction_Id + "'";
                            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                            Insert_tbl_PackageInvoiceAdditional(obj_tbl_PackageInvoiceAdditional_li[i], trans, cn);
                        }
                    }

                    if (obj_tbl_PackageInvoiceEMBMasterLink_Li != null)
                    {
                        for (int i = 0; i < obj_tbl_PackageInvoiceEMBMasterLink_Li.Count; i++)
                        {
                            obj_tbl_PackageInvoiceEMBMasterLink_Li[i].PackageInvoiceEMBMasterLink_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                            Insert_tbl_PackageInvoiceEMBMasterLink(obj_tbl_PackageInvoiceEMBMasterLink_Li[i], trans, cn);
                        }

                    }
                    if (obj_tbl_PackageEMBApproval_Li != null)
                    {
                        for (int i = 0; i < obj_tbl_PackageEMBApproval_Li.Count; i++)
                        {

                            int _Loop = get_Loop("EMB", OrgId, Designation_Id, trans, cn);
                            ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                            if (AllClasses.CheckDataSet(ds))
                            {
                                obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                                obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                                obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_PackageEMBMaster_Id;
                                obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                                Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval_Li[i], trans, cn);
                            }
                            else
                            {
                                ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                                if (AllClasses.CheckDataSet(ds))
                                {
                                    obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Next_Designation_Id = 0;
                                    obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Next_Organisation_Id = 0;
                                    obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_Step_Count = 0;
                                    Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval_Li[i], trans, cn);
                                }
                            }
                        }
                        ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                        if (AllClasses.CheckDataSet(ds))
                        {
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 1;
                            Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                        }
                    }
                }

                if (obj_tbl_PackageEMBApproval != null)
                {
                    int _Loop = get_Loop("EMB", OrgId, Designation_Id, trans, cn);
                    ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                        if (AllClasses.CheckDataSet(ds))
                        {
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 0;
                            Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);

                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 1;
                            Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                        }
                    }
                }

                if (obj_tbl_PackageEMB_Tax_Li != null)
                {
                    int Package_Id = 0;
                    for (int i = 0; i < obj_tbl_PackageEMB_Tax_Li.Count; i++)
                    {
                        if (Package_Id != obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id)
                        {
                            string strQuery = "";
                            strQuery = " set dateformat dmy; Update  tbl_PackageEMB_Tax set   PackageEMB_Tax_Status = 0,PackageEMB_Tax_ModifiedBy='" + obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_AddedBy + "',PackageEMB_Tax_ModifiedOn=getdate() where PackageEMB_Tax_Package_Id = '" + obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id + "' ";
                            //ExecuteSelectQuery(strQuery);
                            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                            Package_Id = obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id;
                        }
                        Insert_tbl_PackageEMB_Tax(obj_tbl_PackageEMB_Tax_Li[i], trans, cn);
                    }
                }



                if (obj_tbl_PackageEMB_Master != null)
                {
                    string strQuery = "";
                    strQuery = " set dateformat dmy; Update  tbl_PackageEMB_Master set   PackageEMB_Master_Date = convert(date, '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Date + "', 103),PackageEMB_Master_VoucherNo='" + obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo + "',PackageEMB_Master_RA_BillNo='" + obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo + "' where PackageEMB_Master_Id = '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Id + "' ";
                    ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }

                if (obj_tbl_PackageEMB_ExtraItem != null)
                {
                    string fileName = "";
                    if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\"))
                    {
                        Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\");
                    }

                    if (obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes != null && obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes.Length > 0)
                    {
                        fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention;
                        obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath = "\\Downloads\\Qty_Variation\\" + fileName;
                        File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\" + fileName, obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes);
                    }
                    else
                    {
                        obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath = "";
                    }

                    string strQuery = " set dateformat dmy;insert into tbl_PackageEMB_ExtraItem ( [PackageEMB_ExtraItem_AddedBy],[PackageEMB_ExtraItem_AddedOn],[PackageEMB_ExtraItem_PackageEMB_Master_Id],[PackageEMB_ExtraItem_ApproveNo],[PackageEMB_ExtraItem_ApproveDate],[PackageEMB_ExtraItem_Comment],[PackageEMB_ExtraItem_Status],[PackageEMB_ExtraItem_ApprovalFilePath] ) values ('" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_PackageEMB_Master_Id + "',N'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveNo + "',convert(date, '" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveDate + "', 103),N'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Comment + "','" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Status + "','" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath + "');Select @@Identity";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private void Insert_tbl_PackageEMBApproval(tbl_PackageEMBApproval obj_tbl_PackageEMBApproval, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMBApproval ( [PackageEMBApproval_AddedBy],[PackageEMBApproval_AddedOn],[PackageEMBApproval_Comments],[PackageEMBApproval_Date],[PackageEMBApproval_Next_Designation_Id],[PackageEMBApproval_Next_Organisation_Id],[PackageEMBApproval_Package_Id],[PackageEMBApproval_PackageEMBMaster_Id],[PackageEMBApproval_Status],[PackageEMBApproval_Status_Id], [PackageEMBApproval_Step_Count]) values ('" + obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy + "', getdate(), N'" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Comments + "', convert(date, '" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Date + "', 103), '" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id + "','" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id + "','" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id + "','" + obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id + "','" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Status + "','" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id + "', '" + obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Update_tbl_EMB_Set_Approval(List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li, tbl_PackageEMBApproval obj_tbl_PackageEMBApproval, tbl_PackageInvoice obj_tbl_PackageInvoice, List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li, int OrgId, int Designation_Id, List<tbl_PackageEMB_Tax> obj_tbl_PackageEMB_Tax_Li, List<tbl_PackageEMB> obj_tbl_PackageEMB_Li, tbl_PackageEMB_Master obj_tbl_PackageEMB_Master, tbl_PackageEMB_ExtraItem obj_tbl_PackageEMB_ExtraItem)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_PackageEMB_Approval_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageEMB_Approval_Li.Count; i++)
                    {
                        Insert_tbl_PackageEMB_Approval(obj_tbl_PackageEMB_Approval_Li[i], trans, cn);
                    }
                }
                if (obj_tbl_PackageEMB_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageEMB_Li.Count; i++)
                    {
                        string strQuery = "";
                        strQuery = " set dateformat dmy; Update  tbl_PackageEMB set   PackageEMB_Qty = '" + obj_tbl_PackageEMB_Li[i].PackageEMB_Qty + "',PackageEMB_QtyExtra = '" + obj_tbl_PackageEMB_Li[i].PackageEMB_QtyExtra + "'  where  PackageEMB_Id = '" + obj_tbl_PackageEMB_Li[i].PackageEMB_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    }
                }

                if (obj_tbl_PackageEMBApproval != null)
                {
                    int _Loop = get_Loop("EMB", OrgId, Designation_Id, trans, cn);
                    if (obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 1 || obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 4)
                    {
                        ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_Prev("EMB", OrgId, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    //ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                    }
                    else
                    {
                        if (obj_tbl_PackageInvoiceItem_Li != null && obj_tbl_PackageInvoiceItem_Li.Count > 0)
                        {
                            if (obj_tbl_PackageInvoice.PackageInvoice_VoucherNo == "")
                            {
                                obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = get_tbl_TransactionNos(VoucherTypes.Invoice, obj_tbl_PackageInvoice.PackageInvoice_Package_Id, trans, cn);
                            }
                            obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id;
                            obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);

                            for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
                            {
                                obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                                obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id = Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                                if (obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li != null)
                                {
                                    for (int k = 0; k < obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li.Count; k++)
                                    {
                                        obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k].PackageInvoiceItem_Tax_PackageInvoiceItem_Id = obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id;
                                        Insert_tbl_PackageInvoiceItem_Tax(obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k], trans, cn); ;
                                    }
                                }
                            }

                            ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                            if (AllClasses.CheckDataSet(ds))
                            {
                                obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
                                obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
                                obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 0;
                                Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);

                                tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy;
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = obj_tbl_PackageEMBApproval.PackageEMBApproval_Date;
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id;
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                                try
                                {
                                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                                }
                                catch
                                {
                                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = 0;
                                }
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 1;
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
                                obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id;
                                Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                            }
                        }


                    }
                }

                if (obj_tbl_PackageEMB_Tax_Li != null)
                {
                    int Package_Id = 0;
                    for (int i = 0; i < obj_tbl_PackageEMB_Tax_Li.Count; i++)
                    {
                        if (Package_Id != obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id)
                        {
                            string strQuery = "";
                            strQuery = " set dateformat dmy; Update  tbl_PackageEMB_Tax set   PackageEMB_Tax_Status = 0,PackageEMB_Tax_ModifiedBy='" + obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_AddedBy + "',PackageEMB_Tax_ModifiedOn=getdate() where PackageEMB_Tax_Package_Id = '" + obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id + "' ";
                            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                            Package_Id = obj_tbl_PackageEMB_Tax_Li[i].PackageEMB_Tax_Package_Id;
                        }
                        Insert_tbl_PackageEMB_Tax(obj_tbl_PackageEMB_Tax_Li[i], trans, cn);
                    }
                }


                if (obj_tbl_PackageEMB_Master != null)
                {
                    string strQuery = "";
                    strQuery = " set dateformat dmy; Update  tbl_PackageEMB_Master set   PackageEMB_Master_Date = convert(date, '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Date + "', 103),PackageEMB_Master_VoucherNo='" + obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo + "',PackageEMB_Master_RA_BillNo='" + obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo + "' where PackageEMB_Master_Id = '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Id + "' ";
                    ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }

                if (obj_tbl_PackageEMB_ExtraItem != null)
                {
                    string fileName = "";
                    if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\"))
                    {
                        Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\");
                    }

                    if (obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes != null && obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes.Length > 0)
                    {
                        fileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Extention;
                        obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath = "\\Downloads\\Qty_Variation\\" + fileName;
                        File.WriteAllBytes(Server.MapPath(".") + "\\Downloads\\Qty_Variation\\" + fileName, obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath_Bytes);
                    }
                    else
                    {
                        obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath = "";
                    }

                    string strQuery = " set dateformat dmy;insert into tbl_PackageEMB_ExtraItem ( [PackageEMB_ExtraItem_AddedBy],[PackageEMB_ExtraItem_AddedOn],[PackageEMB_ExtraItem_PackageEMB_Master_Id],[PackageEMB_ExtraItem_ApproveNo],[PackageEMB_ExtraItem_ApproveDate],[PackageEMB_ExtraItem_Comment],[PackageEMB_ExtraItem_Status],[PackageEMB_ExtraItem_ApprovalFilePath] ) values ('" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_PackageEMB_Master_Id + "',N'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveNo + "',convert(date, '" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApproveDate + "', 103),N'" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Comment + "','" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_Status + "','" + obj_tbl_PackageEMB_ExtraItem.PackageEMB_ExtraItem_ApprovalFilePath + "');Select @@Identity";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private void Insert_tbl_PackageBOQ_Approval(tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageBOQ_Approval ( [PackageBOQ_Approval_AddedBy],[PackageBOQ_Approval_AddedOn],[PackageBOQ_Approval_Approved_Qty],[PackageBOQ_Approval_Comments],[PackageBOQ_Approval_Date],[PackageBOQ_Approval_No],[PackageBOQ_Approval_PackageBOQ_Id],[PackageBOQ_Approval_Person_Id],[PackageBOQ_Approval_Status],[PackageBOQ_DocumentPath] ) values ('" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy + "', getdate(),'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id + "','" + obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status + "',N'" + obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_PackageEMB_Approval(tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMB_Approval ( [PackageEMB_Approval_AddedBy],[PackageEMB_Approval_AddedOn],[PackageEMB_Approval_Approved_Qty],[PackageEMB_Approval_Comments],[PackageEMB_Approval_Date],[PackageEMB_Approval_No],[PackageEMB_Approval_PackageEMB_Id],[PackageEMB_Approval_Person_Id],[PackageEMB_Approval_Status],[PackageEMB_DocumentPath] ) values ('" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Comments + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Date + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_No + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Person_Id + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Status + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_DocumentPath + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_PackageEMB_Tax(tbl_PackageEMB_Tax obj_tbl_PackageEMB_Tax, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMB_Tax ( [PackageEMB_Tax_AddedBy],[PackageEMB_Tax_AddedOn],[PackageEMB_Tax_Package_Id],[PackageEMB_Tax_Deduction_Id],[PackageEMB_Tax_Value],[PackageEMB_Tax_Status]) values ('" + obj_tbl_PackageEMB_Tax.PackageEMB_Tax_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Package_Id + "','" + obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Deduction_Id + "','" + obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Value + "','" + obj_tbl_PackageEMB_Tax.PackageEMB_Tax_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_PackageInvoiceItem_Tax(tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoiceItem_Tax ( [PackageInvoiceItem_Tax_AddedBy],[PackageInvoiceItem_Tax_AddedOn],[PackageInvoiceItem_Tax_PackageInvoiceItem_Id],[PackageInvoiceItem_Tax_Deduction_Id],[PackageInvoiceItem_Tax_Value],[PackageInvoiceItem_Tax_Status]) values ('" + obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy + "', getdate(),'" + obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_PackageInvoiceItem_Id + "','" + obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id + "','" + obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value + "','" + obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region Package EMB
    public DataSet get_Deduction_PackageEMB(int PackageEMB_Master_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" select PackageEMBAdditional_PackageEMBMaster_Id,
                    PackageEMBAdditional_Deduction_Id,
                    PackageEMBAdditional_Id,
                    PackageEMBAdditional_Deduction_Value,
                    PackageEMBAdditional_Deduction_Type
                     from tbl_PackageEMBAdditional
                     where PackageEMBAdditional_Status = 1 and PackageEMBAdditional_PackageEMBMaster_Id = '" + PackageEMB_Master_Id + "' ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_PackageEMB_ExtraItemApprove(int PackageEMB_Master_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" select PackageEMB_ExtraItem_ApproveNo,CONVERT(varchar(15),PackageEMB_ExtraItem_ApproveDate,101) as PackageEMB_ExtraItem_ApproveDate,
                    PackageEMB_ExtraItem_Comment,PackageEMB_ExtraItem_ApprovalFilePath
                    from tbl_PackageEMB_ExtraItem
                    where PackageEMB_ExtraItem_PackageEMB_Master_Id='" + PackageEMB_Master_Id + "' and  PackageEMB_ExtraItem_Status=1 ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_PackageEMB(int Package_Id, int PackageEMB_Master_Id, string _Mode, bool includeCurrentEMB)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (_Mode == "")
        {
            strQuery = @"set dateformat dmy; 
                        select 
	                        PackageEMB_Id,
                            PackageEMB_PackageEMB_Master_Id,
                            PackageBOQ_Id,
	                        PackageEMB_Package_Id = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_Package_Id else PackageEMB_Package_Id end,
	                        PackageEMB_Specification = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_Specification else PackageEMB_Specification end,
	                        PackageEMB_Unit_Id = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_Unit_Id else PackageEMB_Unit_Id end,
                            Unit_Name = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then (select top 1 Unit_Name from tbl_Unit where Unit_Id=PackageBOQ_Unit_Id)  else (select top 1 Unit_Name from tbl_Unit where Unit_Id=PackageEMB_Unit_Id)  end,
                            isnull(Unit_Length_Applicable, 0) Unit_Length_Applicable,
                            isnull(Unit_Bredth_Applicable, 0) Unit_Bredth_Applicable,
                            isnull(Unit_Height_Applicable, 0) Unit_Height_Applicable,
	                        PackageEMB_Qty = isnull(PackageEMB_Qty, 0),
	                        PackageEMB_PercentageToBeReleased = isnull(PackageEMB_PercentageToBeReleased, 100),
	                        PackageEMB_QtyExtra,
                            PackageEMB_QtyExtraPer=case when Isnull(PackageEMB_QtyExtra,0)=0 then '0 %' 
							 when isnull(PackageBOQ_Qty,0)=0 then '0 %' else convert(varchar(150),cast(((PackageEMB_QtyExtra/PackageBOQ_Qty)*100) as int ) )+' %' end,
                            PackageBOQ_Qty,
	                        PackageEMB_Length = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then '' else PackageEMB_Length end,
	                        PackageEMB_Breadth = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then '' else PackageEMB_Breadth end,
	                        PackageEMB_Height = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then '' else PackageEMB_Height end,
	                        PackageEMB_Contents = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then '' else PackageEMB_Contents end,
	                        PackageEMB_AddedBy = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_AddedBy else PackageEMB_AddedBy end,
	                        PackageEMB_AddedOn = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_AddedOn else PackageEMB_AddedOn end,
	                        PackageEMB_Status = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_Status else PackageEMB_Status end, 
                            tbl_PackageEMB.PackageEMB_Approval_Id, 
						    tbl_PackageEMB.PackageEMB_Approval_Date, 
						    tbl_PackageEMB.PackageEMB_Approval_No, 
						    tbl_PackageEMB.PackageEMB_Approval_Person, 
						    tbl_PackageEMB.PackageEMB_Approval_Approved_Qty, 
						    tbl_PackageEMB.PackageEMB_Approval_Comments, 
                            PackageEMB_Master_Date = convert(char(10), tbl_PackageEMB.PackageEMB_Master_Date, 103), 
                            tbl_PackageEMB.PackageEMB_Master_VoucherNo,
                            tbl_PackageEMB.PackageEMB_Master_RA_BillNo,
                            PackageBOQ_RateEstimated,
                            PackageBOQ_AmountEstimated,
                            PackageBOQ_RateQuoted,
                            PackageBOQ_AmountQuoted, 
                            PackageEMB_Is_Billed = isnull(PackageInvoiceItem_Id, 0), 
                            tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments, 
                            PackageInvoiceItem_Id = isnull(tbl_PackageEMB.PackageInvoiceItem_Id, 0), 
                            PackageBOQ_QtyPaid = isnull(PackageBOQ_QtyPaid, 0) + isnull(PackageEMB_Qty_1, 0),
                            PercentageValuePaidTillDate=Isnull(PackageBOQ_PercentageValuePaidTillDate,0)+Isnull(PackageBOQ_PercentageValuePaidTillDate_1,0),
                            GSTType,
                            GSTPercenatge,
                            tbl_PackageEMB.PackageEMB_GSTType,
                            tbl_PackageEMB.PackageEMB_GSTPercenatge,
                            PackageEMB_PackageBOQ_OrderNo=case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_OrderNo else PackageEMB_PackageBOQ_OrderNo end
                        from tbl_PackageBOQ
                        left join 
					    (
						    select 
							    * 
						    from tbl_PackageEMB
                            left join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            left join 
						    (
							    select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id, PackageEMB_Approval_Person = Person_Name from tbl_PackageEMB_Approval join tbl_PersonDetail on Person_Id = PackageEMB_Approval_Person_Id where PackageEMB_Approval_Status = 1
						    ) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
                            left join 
						    (
							    select ROW_NUMBER() over (partition by PackageInvoiceItem_PackageEMB_Id order by PackageInvoiceItem_Id desc) rrrI, PackageInvoiceItem_Id, PackageInvoiceItem_PackageEMB_Id, PackageInvoiceItem_Invoice_Id, PackageInvoiceItem_Total_Qty_BOQ, PackageInvoiceItem_Total_Qty_Billed, PackageInvoiceItem_Total_Qty, PackageInvoiceItem_RateEstimated, PackageInvoiceItem_RateQuoted, PackageInvoiceItem_AmountQuoted, PackageInvoiceItem_Remarks from tbl_PackageInvoiceItem where PackageInvoiceItem_Status = 1
						    ) tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id = PackageEMB_Id and rrrI = 1
						    where PackageEMB_Status = 1 and isnull(PackageEMB_PackageEMB_Master_Id, 0) = 0 and isnull(PackageEMB_Approval_PackageEMB_Id, 0) = 0
					    ) tbl_PackageEMB on PackageBOQ_Id = PackageEMB_PackageBOQ_Id                    

                        left join 
                        (
                            select 
                                ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrrA, 
                                PackageBOQ_Approval_Id, 
                                PackageBOQ_Approval_PackageBOQ_Id, 
                                PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), 
                                PackageBOQ_Approval_No, 
                                PackageBOQ_Approval_Comments, 
                                PackageBOQ_Approval_Approved_Qty, 
                                PackageBOQ_DocumentPath, 
                                PackageBOQ_Approval_Person_Id, 
                                PackageBOQ_Approval_Person = Person_Name 
                            from tbl_PackageBOQ_Approval 
                            join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id 
                            where PackageBOQ_Approval_Status = 1
                        ) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrrA = 1
                        left join tbl_Unit on Unit_Id = case when isnull(tbl_PackageEMB.PackageEMB_Id, 0) = 0 then PackageBOQ_Unit_Id else PackageEMB_Unit_Id end
                        left join 
					    (
						    select 
	                            PackageEMB_PackageBOQ_Id, PackageEMB_Qty_1 = sum(isnull(PackageEMB_Qty, 0)),
                                PackageBOQ_PercentageValuePaidTillDate_1 = sum(isnull(PackageEMB_PercentageToBeReleased, 0))
                            from tbl_PackageEMB
                            join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            where PackageEMB_Status = 1 includeCurrentEMBCond
                            group by PackageEMB_PackageBOQ_Id
					    ) Qty_Paid_Till_Date on Qty_Paid_Till_Date.PackageEMB_PackageBOQ_Id = PackageBOQ_Id     
                        where PackageBOQ_Status = 1 and isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 0 and PackageBOQ_Package_Id = '" + Package_Id + "'  ";
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        select 
	                        PackageEMB_Id,
                            PackageEMB_PackageEMB_Master_Id,
                            PackageEMB_Master_Date = convert(char(10), PackageEMB_Master_Date, 103), 
                            PackageEMB_Master_VoucherNo,
                            PackageEMB_Master_RA_BillNo,
                            PackageBOQ_Id,
	                        PackageEMB_Package_Id,
	                        PackageEMB_Specification,
	                        PackageEMB_Unit_Id,
                            Unit_Name =(select top 1 Unit_Name from tbl_Unit where Unit_Id=PackageEMB_Unit_Id),
                            isnull(Unit_Length_Applicable, 0) Unit_Length_Applicable,
                            isnull(Unit_Bredth_Applicable, 0) Unit_Bredth_Applicable,
                            isnull(Unit_Height_Applicable, 0) Unit_Height_Applicable,
                            PackageEMB_PercentageToBeReleased,
	                        PackageEMB_Qty,
                            PackageBOQ_Qty,
                            PackageEMB_QtyExtra,
                            PackageEMB_QtyExtraPer=case when Isnull(PackageEMB_QtyExtra,0)=0 then '0 %' 
							 when isnull(PackageBOQ_Qty,0)=0 then '0 %' else convert(varchar(150),cast(((PackageEMB_QtyExtra/PackageBOQ_Qty)*100) as int ) )+' %' end,
	                        PackageEMB_Length,
	                        PackageEMB_Breadth,
	                        PackageEMB_Height,
	                        PackageEMB_Contents,
	                        PackageEMB_AddedBy,
	                        PackageEMB_AddedOn,
	                        PackageEMB_Status, 
                            tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Date, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_No, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Person, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Comments, 
                            PackageBOQ_RateEstimated,
                            PackageBOQ_AmountEstimated,
                            PackageBOQ_RateQuoted,
                            PackageBOQ_AmountQuoted, 
                            PackageEMB_Is_Billed = isnull(PackageInvoiceItem_Id, 0), 
                            tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments, 
                            PackageInvoiceItem_Id = isnull(tbl_PackageInvoiceItem.PackageInvoiceItem_Id, 0), 
                            PackageBOQ_QtyPaid = isnull(PackageBOQ_QtyPaid, 0) + isnull(PackageEMB_Qty_1, 0),
                            GSTType,
                            GSTPercenatge,
                            PackageEMB_GSTType,
                            PackageEMB_GSTPercenatge,
                            PackageEMB_PackageBOQ_OrderNo
                        from tbl_PackageEMB
                        left join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                        join tbl_PackageBOQ on PackageBOQ_Id = PackageEMB_PackageBOQ_Id and PackageBOQ_Status = 1
                        left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id, PackageEMB_Approval_Person = Person_Name from tbl_PackageEMB_Approval join tbl_PersonDetail on Person_Id = PackageEMB_Approval_Person_Id where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
                        left join 
						(
							select ROW_NUMBER() over (partition by PackageInvoiceItem_PackageEMB_Id order by PackageInvoiceItem_Id desc) rrrI, PackageInvoiceItem_Id, PackageInvoiceItem_PackageEMB_Id, PackageInvoiceItem_Invoice_Id, PackageInvoiceItem_Total_Qty_BOQ, PackageInvoiceItem_Total_Qty_Billed, PackageInvoiceItem_Total_Qty, PackageInvoiceItem_RateEstimated, PackageInvoiceItem_RateQuoted, PackageInvoiceItem_AmountQuoted, PackageInvoiceItem_Remarks from tbl_PackageInvoiceItem where PackageInvoiceItem_Status = 1
						) tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id = PackageEMB_Id and rrrI = 1
                        left join 
                        (
                            select 
                                ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrrA, 
                                PackageBOQ_Approval_Id, 
                                PackageBOQ_Approval_PackageBOQ_Id, 
                                PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), 
                                PackageBOQ_Approval_No, 
                                PackageBOQ_Approval_Comments, 
                                PackageBOQ_Approval_Approved_Qty, 
                                PackageBOQ_DocumentPath, 
                                PackageBOQ_Approval_Person_Id, 
                                PackageBOQ_Approval_Person = Person_Name 
                            from tbl_PackageBOQ_Approval 
                            join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id 
                            where PackageBOQ_Approval_Status = 1
                        ) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrrA = 1
                        left join tbl_Unit on Unit_Id = PackageEMB_Unit_Id
                        left join 
					    (
						    select 
	                            PackageEMB_PackageBOQ_Id, PackageEMB_Qty_1 = sum(isnull(PackageEMB_Qty, 0))
                            from tbl_PackageEMB
                            join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            where PackageEMB_Status = 1 includeCurrentEMBCond
                            group by PackageEMB_PackageBOQ_Id
					    ) Qty_Paid_Till_Date on Qty_Paid_Till_Date.PackageEMB_PackageBOQ_Id = PackageBOQ_Id     
                        where PackageEMB_Status = 1 and isnull(PackageEMB_PackageEMB_Master_Id, 0) > 0 and isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 0 and PackageEMB_Package_Id = '" + Package_Id + "'  ";
        }
        if (PackageEMB_Master_Id > 0)
        {
            strQuery += " and PackageEMB_PackageEMB_Master_Id = '" + PackageEMB_Master_Id + "'";
            if (includeCurrentEMB)
            {
                strQuery = strQuery.Replace("includeCurrentEMBCond", "");
            }
            else
            {
                strQuery = strQuery.Replace("includeCurrentEMBCond", "and PackageEMB_Master_Id != '" + PackageEMB_Master_Id + "'");
            }
        }
        else
        {
            strQuery = strQuery.Replace("includeCurrentEMBCond", "");
        }
        if (PackageEMB_Master_Id > 0)
        {
            strQuery += " order by PackageEMB_PackageBOQ_OrderNo ";
        }
        else
        {
            strQuery += " order by PackageBOQ_OrderNo ";
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_PackageEMB_Combined(string PackageEMB_Master_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery = @"set dateformat dmy; 
                        select 
	                        PackageEMB_Id,
                            PackageEMB_PackageEMB_Master_Id,
                            PackageEMB_Master_Date = convert(char(10), PackageEMB_Master_Date, 103), 
                            PackageEMB_Master_VoucherNo,
                            PackageEMB_Master_RA_BillNo,
                            PackageBOQ_Id,
	                        PackageEMB_Package_Id,
	                        PackageEMB_Specification,
	                        PackageEMB_Unit_Id,
                            Unit_Name =(select top 1 Unit_Name from tbl_Unit where Unit_Id=PackageEMB_Unit_Id),
                            isnull(Unit_Length_Applicable, 0) Unit_Length_Applicable,
                            isnull(Unit_Bredth_Applicable, 0) Unit_Bredth_Applicable,
                            isnull(Unit_Height_Applicable, 0) Unit_Height_Applicable,
                            PackageEMB_PercentageToBeReleased,
	                        PackageEMB_Qty,
                            PackageBOQ_Qty,
                            PackageEMB_QtyExtra,
	                        PackageEMB_Length,
	                        PackageEMB_Breadth,
	                        PackageEMB_Height,
	                        PackageEMB_Contents,
	                        PackageEMB_AddedBy,
	                        PackageEMB_AddedOn,
	                        PackageEMB_Status, 
                            tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Date, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_No, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Person, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty, 
						    tbl_PackageEMB_Approval.PackageEMB_Approval_Comments, 
                            PackageBOQ_RateEstimated,
                            PackageBOQ_AmountEstimated,
                            PackageBOQ_RateQuoted,
                            PackageBOQ_AmountQuoted, 
                            PackageEMB_Is_Billed = isnull(PackageInvoiceItem_Id, 0), 
                            tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						    tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments, 
                            PackageInvoiceItem_Id = isnull(tbl_PackageInvoiceItem.PackageInvoiceItem_Id, 0), 
                            PackageBOQ_QtyPaid = isnull(PackageBOQ_QtyPaid, 0) + isnull(PackageEMB_Qty_1, 0),
                            PackageEMB_GSTType,
                            PackageEMB_GSTPercenatge,
                            PackageEMB_PackageBOQ_OrderNo
                        from tbl_PackageEMB
                        left join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                        join tbl_PackageBOQ on PackageBOQ_Id = PackageEMB_PackageBOQ_Id and PackageBOQ_Status = 1
                        left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id, PackageEMB_Approval_Person = Person_Name from tbl_PackageEMB_Approval join tbl_PersonDetail on Person_Id = PackageEMB_Approval_Person_Id where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
                        left join 
						(
							select ROW_NUMBER() over (partition by PackageInvoiceItem_PackageEMB_Id order by PackageInvoiceItem_Id desc) rrrI, PackageInvoiceItem_Id, PackageInvoiceItem_PackageEMB_Id, PackageInvoiceItem_Invoice_Id, PackageInvoiceItem_Total_Qty_BOQ, PackageInvoiceItem_Total_Qty_Billed, PackageInvoiceItem_Total_Qty, PackageInvoiceItem_RateEstimated, PackageInvoiceItem_RateQuoted, PackageInvoiceItem_AmountQuoted, PackageInvoiceItem_Remarks from tbl_PackageInvoiceItem where PackageInvoiceItem_Status = 1
						) tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id = PackageEMB_Id and rrrI = 1
                        left join 
                        (
                            select 
                                ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrrA, 
                                PackageBOQ_Approval_Id, 
                                PackageBOQ_Approval_PackageBOQ_Id, 
                                PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), 
                                PackageBOQ_Approval_No, 
                                PackageBOQ_Approval_Comments, 
                                PackageBOQ_Approval_Approved_Qty, 
                                PackageBOQ_DocumentPath, 
                                PackageBOQ_Approval_Person_Id, 
                                PackageBOQ_Approval_Person = Person_Name 
                            from tbl_PackageBOQ_Approval 
                            join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id 
                            where PackageBOQ_Approval_Status = 1
                        ) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrrA = 1
                        left join tbl_Unit on Unit_Id = PackageEMB_Unit_Id
                        left join 
					    (
						    select 
	                            PackageEMB_PackageBOQ_Id, PackageEMB_Qty_1 = sum(isnull(PackageEMB_Qty, 0))
                            from tbl_PackageEMB
                            join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            where PackageEMB_Status = 1 
                            group by PackageEMB_PackageBOQ_Id
					    ) Qty_Paid_Till_Date on Qty_Paid_Till_Date.PackageEMB_PackageBOQ_Id = PackageBOQ_Id     
                        where PackageEMB_Status = 1 and isnull(PackageEMB_PackageEMB_Master_Id, 0) > 0 and isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 0 ";

        if (PackageEMB_Master_Id != "")
        {
            strQuery += " and PackageEMB_PackageEMB_Master_Id in (" + PackageEMB_Master_Id + ")";
            //strQuery = strQuery.Replace("includeCurrentEMBCond", "and PackageEMB_Master_Id != '" + PackageEMB_Master_Id + "'");

        }
        else
        {
            //strQuery = strQuery.Replace("includeCurrentEMBCond", "");
        }

        strQuery += "Order by PackageEMB_PackageBOQ_OrderNo";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_PackageEMB_Approve(int Work_Id, int Project_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, int ProjectWorkPkg_Id, string Project_Code, string Package_Code, int Organisation_Id, int Designation_Id, int PackageEMB_Master_Id, string RA_BillNo, bool ShowOnlyEMB)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageEMB_Master_Id,
	                    PackageEMB_Master_Date = CONVERT(char(10), PackageEMB_Master_Date, 103),
	                    PackageEMB_Master_VoucherNo,
	                    PackageEMB_Master_RA_BillNo,
	                    PackageEMB_Master_Narration,
	                    PackageEMB_Master_Package_Id, 
	                    Total_Items, 
	                    Total_Approved, 
	                    ProjectWorkPkg_Id,
	                    ProjectWorkPkg_Work_Id,
                        ProjectWorkPkg_Code, 
	                    ProjectWorkPkg_Name,
                        ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
	                    ProjectWorkPkg_AgreementAmount,
	                    ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
	                    ProjectWorkPkg_Agreement_No,
	                    ProjectWorkPkg_Indent_No,
	                    ProjectWorkPkg_Vendor_Id,
	                    ProjectWorkPkg_Staff_Id,
	                    Vendor_Name = Vendor.Person_Name,
	                    Vendor_Mobile = Vendor.Person_Mobile1,
	                    List_ReportingStaff_JEAPE_Name,
						List_ReportingStaff_AEPE_Name,
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Name, 
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
	                    ULB_Name, 
	                    Jurisdiction_Name_Eng, 
	                    Division_Name, 
	                    Circle_Name, 
	                    ProjectWork_DistrictId, 
	                    ProjectWork_ULB_Id, 
	                    ProjectWork_DivisionId, 
	                    Division_CircleId, 
                        ProjectWorkPkg_LockedOn, 
                        ProjectWorkPkg_Locked_By, 
	                    tEMBApproval.Designation_DesignationName, 
	                    tEMBApproval.OfficeBranch_Name, 
	                    tEMBApproval.PackageEMBApproval_Next_Designation_Id, 
	                    tEMBApproval.PackageEMBApproval_Next_Organisation_Id, 
	                    tEMBApproval.PackageEMBApproval_Date, 
	                    tEMBApproval.PackageEMBApproval_Status_Id,
                        Invoice_Status = isnull(InvoiceStatus_Name, 'Pending'), 
                        isnull(PackageInvoice_Id,0) as PackageInvoice_Id,
                        Designation_Current, 
	                    Organisation_Current ,
                        Fund_Allocated = convert(decimal(18,2), (isnull(ttbl_FinancialTrans.FinancialTrans_TransAmount, 0) / 100000)),
                        ProjectWorkPkg_Vendor_Id
                    from tbl_PackageEMB_Master
                    join
                    (
	                    select 
		                    PackageEMB_PackageEMB_Master_Id, 
		                    Total_Items = COUNT(*), 
		                    Total_Approved = SUM(case when ISNULL(tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 0) > 0 then 1 else 0 end)
	                    from tbl_PackageEMB
	                    left join 
	                    (
		                    select 
			                    ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, 
			                    PackageEMB_Approval_Id, 
			                    PackageEMB_Approval_PackageEMB_Id, 
			                    PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), 
			                    PackageEMB_Approval_No, 
			                    PackageEMB_Approval_Comments, 
			                    PackageEMB_Approval_Approved_Qty
		                    from tbl_PackageEMB_Approval 
		                    where PackageEMB_Approval_Status = 1
	                    ) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
	                    where PackageEMB_Status = 1
	                    GROUP by PackageEMB_PackageEMB_Master_Id
                    ) tbl_PackageEMB on PackageEMB_PackageEMB_Master_Id = PackageEMB_Master_Id
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageEMB_Master_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on Project_Id = ProjectWork_Project_Id
                    join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
                    left join tbl_ULB on ULB_Id = ProjectWork_ULB_Id
                    left join tbl_Division on Division_Id = ProjectWork_DivisionId
                    left join tbl_Circle on Circle_Id = Division_CircleId
                    left join tbl_PersonDetail Vendor on Vendor.Person_Id = ProjectWorkPkg_Vendor_Id
                    left join tbl_PersonDetail Staff on Staff.Person_Id = ProjectWorkPkg_Staff_Id
                    left join 
                    (
                        select ROW_NUMBER() over (partition by FinancialTrans_Work_Id, FinancialTrans_Package_Id, FinancialTrans_TransType order by FinancialTrans_Id asc) rrr, FinancialTrans_Work_Id, FinancialTrans_Package_Id, FinancialTrans_TransAmount from tbl_FinancialTrans where FinancialTrans_Status = 1 and FinancialTrans_TransType = 'C'
                    )ttbl_FinancialTrans on ttbl_FinancialTrans.FinancialTrans_Work_Id=ProjectWork_Id and FinancialTrans_Package_Id=ProjectWorkPkg_Id

                     left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                            where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id

                    left join 
                    (
	                    select 
		                    ROW_NUMBER() over (partition by PackageEMBApproval_PackageEMBMaster_Id order by PackageEMBApproval_Id desc) rrrrr,
		                    PackageEMBApproval_Next_Designation_Id,
		                    PackageEMBApproval_Next_Organisation_Id,
		                    PackageEMBApproval_Comments,
		                    PackageEMBApproval_AddedBy,
		                    PackageEMBApproval_AddedOn,
		                    PackageEMBApproval_Status_Id,
		                    PackageEMBApproval_Package_Id,
		                    PackageEMBApproval_PackageEMBMaster_Id,
                            InvoiceStatus_Name,
                            PackageInvoice_Id,
		                    PackageEMBApproval_Date = convert(char(10), PackageEMBApproval_Date, 103),
		                    PackageEMBApproval_Id, 
		                    tbl_Designation.Designation_DesignationName, 
		                    tbl_OfficeBranch.OfficeBranch_Name, 
                            Designation_Current = Designation_Current.Designation_DesignationName, 
	                        Organisation_Current = Organisation_Current.OfficeBranch_Name 
	                    from tbl_PackageEMBApproval
	                    left join tbl_OfficeBranch on OfficeBranch_Id = PackageEMBApproval_Next_Organisation_Id
	                    left join tbl_Designation on Designation_Id = PackageEMBApproval_Next_Designation_Id
                        join tbl_PersonDetail on Person_Id = PackageEMBApproval_AddedBy 
                        join tbl_PersonJuridiction on PersonJuridiction_PersonId = PackageEMBApproval_AddedBy
                        left join tbl_Designation Designation_Current on Designation_Current.Designation_Id = PersonJuridiction_DesignationId
                        left join tbl_OfficeBranch Organisation_Current on Organisation_Current.OfficeBranch_Id = Person_BranchOffice_Id
                        left join tbl_InvoiceStatus on InvoiceStatus_Id = PackageEMBApproval_Status_Id
                        left join tbl_PackageInvoice on PackageInvoice_PackageEMBMaster_Id = PackageEMBApproval_PackageEMBMaster_Id
	                    where PackageEMBApproval_Status = 1
                    ) tEMBApproval on tEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = PackageEMB_Master_Id and tEMBApproval.rrrrr = 1
                    where PackageEMB_Master_Status = 1 and ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 AdditionalCondition ";
        if (PackageEMB_Master_Id > 0)
        {
            strQuery += " and PackageEMB_Master_Id = '" + PackageEMB_Master_Id + "'";
        }
        if (Work_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Work_Id = '" + Work_Id + "'";
        }
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = '" + Project_Id + "'";
        }
        if (District_Id != 0)
        {
            strQuery += " and ProjectWork_DistrictId = '" + District_Id + "'";
        }
        if (Zone_Id != 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        if (Circle_Id != 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            strQuery += " and ProjectWork_DivisionId = '" + Division_Id + "'";
        }
        if (Project_Code != "")
        {
            strQuery += " and ProjectWork_ProjectCode = '" + Project_Code + "'";
        }
        if (Package_Code != "")
        {
            strQuery += " and ProjectWorkPkg_Code = '" + Package_Code + "'";
        }
        if (ProjectWorkPkg_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "'";
        }
        if (Organisation_Id > 0 && Designation_Id > 0)
        {
            strQuery = strQuery.Replace("AdditionalCondition", " and PackageEMBApproval_Next_Organisation_Id = '" + Organisation_Id + "' and PackageEMBApproval_Next_Designation_Id = '" + Designation_Id + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("AdditionalCondition", "");
        }
        if (RA_BillNo != "")
        {
            strQuery += " and PackageEMB_Master_RA_BillNo in ('" + RA_BillNo + "')";
        }
        if (ShowOnlyEMB == true)
        {
            strQuery += @" and PackageEMB_Master_Id not in
                        (select distinct PackageInvoiceEMBMasterLink_EMBMaster_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status = 1) ";
        }
        strQuery += " order by ProjectWork_Name, ProjectWorkPkg_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Update_tbl_ProjectPackage_Fund_Allocation(tbl_FinancialTrans obj_tbl_FinancialTrans, File_Objects obj_File_Objects)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_File_Objects.File_Path_Bytes_1 != null && obj_File_Objects.File_Path_Bytes_1.Length > 0)
                {
                    File.WriteAllBytes(Server.MapPath(".") + obj_File_Objects.File_Path_1, obj_File_Objects.File_Path_Bytes_1);
                }
                else
                {
                    obj_File_Objects.File_Path_1 = "";
                }

                obj_tbl_FinancialTrans.FinancialTrans_FilePath1 = obj_File_Objects.File_Path_1;
                Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans, trans, cn);


                trans.Commit();
                cn.Close();
                iResult = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }
    public DataSet get_tbl_PackageEMB_Tax(int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery += @"select Deduction_Name,PackageEMB_Tax_Value,PackageEMB_Tax_Id
                    from tbl_PackageEMB_Tax
                    inner join tbl_Deduction on Deduction_Id=PackageEMB_Tax_Deduction_Id
                    where PackageEMB_Tax_Package_Id='" + Package_Id + "' and PackageEMB_Tax_Status=1 ";


        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectPackage_Transaction_Details(int Work_Id, int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery += @"select 
                        FinancialTrans_Id,
                        FinancialTrans_Date = convert(char(10), FinancialTrans_Date, 103),
	                    FinancialTrans_EntryType,
	                    FinancialTrans_Comments,
	                    FinancialTrans_FilePath1,
	                    FinancialTrans_GO_Date = case when FinancialTrans_TransType = 'C' then convert(char(10), FinancialTrans_GO_Date, 103) else convert(char(10), FinancialTrans_Date, 103) end,
	                    FinancialTrans_GO_Number,
	                    TransAmount_C = convert(decimal(18,2), (isnull(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end, 0) / 100000)), 
	                    TransAmount_D = convert(decimal(18,2), (isnull(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end, 0) / 100000))
                    from tbl_FinancialTrans
                    where FinancialTrans_Status = 1 and FinancialTrans_Package_Id = ProjectPackage_IdCond and FinancialTrans_Work_Id = ProjectWork_IdCond
                    order by FinancialTrans_Id; ";

        if (Package_Id != 0)
        {
            strQuery = strQuery.Replace("ProjectPackage_IdCond", Package_Id.ToString());
        }
        if (Work_Id != 0)
        {
            strQuery = strQuery.Replace("ProjectWork_IdCond", Work_Id.ToString());
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_PackageEMB(tbl_PackageEMB_Master obj_tbl_PackageEMB_Master, tbl_PackageEMBApproval obj_tbl_PackageEMBApproval, List<tbl_PackageEMB> obj_tbl_PackageEMB_Li, int OrgId, int Designation_Id, List<tbl_PackageEMB_ADP_Item> obj_tbl_PackageEMB_ADP_Item_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_PackageEMB_Master != null && obj_tbl_PackageEMB_Master.PackageEMB_Master_Id == 0)
                {
                    if (obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo == "")
                    {
                        obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo = get_tbl_TransactionNos(VoucherTypes.EMB, obj_tbl_PackageEMB_Master.PackageEMB_Master_Package_Id, trans, cn);
                    }
                    obj_tbl_PackageEMB_Master.PackageEMB_Master_Id = Insert_tbl_PackageEMB_Master(obj_tbl_PackageEMB_Master, trans, cn);
                }
                for (int i = 0; i < obj_tbl_PackageEMB_Li.Count; i++)
                {
                    if (obj_tbl_PackageEMB_Master != null)
                    {
                        obj_tbl_PackageEMB_Li[i].PackageEMB_PackageEMB_Master_Id = obj_tbl_PackageEMB_Master.PackageEMB_Master_Id;
                    }
                    if (obj_tbl_PackageEMB_Li[i].PackageEMB_Id == 0)
                    {
                        Insert_tbl_PackageEMB(obj_tbl_PackageEMB_Li[i], trans, cn);
                    }
                    else
                    {
                        if (obj_tbl_PackageEMB_Li[i].PackageEMB_Is_Approved == 0)
                        {
                            Update_tbl_PackageEMB(obj_tbl_PackageEMB_Li[i], trans, cn);
                        }
                    }
                }
                if (obj_tbl_PackageEMB_ADP_Item_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageEMB_ADP_Item_Li.Count; i++)
                    {
                        obj_tbl_PackageEMB_ADP_Item_Li[0].PackageEMB_ADP_Item_PackageEMB_Master_Id = obj_tbl_PackageEMB_Master.PackageEMB_Master_Id;
                        Insert_tbl_PackageEMB_ADP_Item(obj_tbl_PackageEMB_ADP_Item_Li[i], trans, cn);
                    }
                }

                if (obj_tbl_PackageEMBApproval != null && obj_tbl_PackageEMB_Master != null)
                {
                    int _Loop = get_Loop("EMB", OrgId, Designation_Id, trans, cn);
                    ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMB_Master.PackageEMB_Master_Id, _Loop, trans, cn);
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMB_Master.PackageEMB_Master_Id;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);

                        ds = get_ProcessConfigMaster_Next("EMB", OrgId, Designation_Id, obj_tbl_PackageEMB_Master.PackageEMB_Master_Id, _Loop, trans, cn);
                        if (AllClasses.CheckDataSet(ds))
                        {
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMB_Master.PackageEMB_Master_Id;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                            Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                        }
                    }
                    else
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id = obj_tbl_PackageEMB_Master.PackageEMB_Master_Id;
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 0;
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public DataSet CheckPackageApproval(string ProjectWorkPkg_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  ProjectWorkPkg_Id from tbl_ProjectWorkPkg  where  ProjectWorkPkg_Id = '" + ProjectWorkPkg_Id + "' and ISNULL(ProjectWorkPkg_ApprovalFile_Path,'') = '' ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }

        return ds;
    }

    public DataSet CheckPackageBOQ(string ProjectWorkPkg_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  PackageBOQ_Id from tbl_PackageBOQ  where  PackageBOQ_Package_Id = '" + ProjectWorkPkg_Id + "' and PackageBOQ_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }

        return ds;
    }
    public DataSet Get_PackageBOQItems(string ProjectWorkPkg_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" select PackageBOQ_Id,
                        PackageBOQ_Specification,
                        Unit_Name,
                        PackageDivisionBOQItem_Id = ISNULL(PackageDivisionBOQItem_Id, 0)
                        from tbl_PackageBOQ
                        left join tbl_Unit on Unit_Id=PackageBOQ_Unit_Id
                        left join tbl_PackageDivisionBOQItem on PackageDivisionBOQItem_PackageBOQ_Id = PackageBOQ_Id and PackageDivisionBOQItem_Status = 1
                        where PackageBOQ_Package_Id = '" + ProjectWorkPkg_Id + "' and PackageBOQ_Status = 1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }

        return ds;
    }

    public bool tbl_PackageDivisionBOQItem(List<tbl_PackageDivisionBOQItem> obj_tbl_PackageDivisionBOQItem_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_PackageDivisionBOQItem_Li != null)
                {
                    string strQuery = @"set dateformat dmy; 
                    update tbl_PackageDivisionBOQItem set PackageDivisionBOQItem_Status = '0', PackageDivisionBOQItem_ModifiedBy = '" + obj_tbl_PackageDivisionBOQItem_Li[1].PackageDivisionBOQItem_AddedBy + "', PackageDivisionBOQItem_ModifiedOn = getdate() where PackageDivisionBOQItem_ProjectWorkPkg_Id = '" + obj_tbl_PackageDivisionBOQItem_Li[1].PackageDivisionBOQItem_ProjectWorkPkg_Id + "' and PackageDivisionBOQItem_DevisionId='" + obj_tbl_PackageDivisionBOQItem_Li[1].PackageDivisionBOQItem_DevisionId + "' and PackageDivisionBOQItem_Status=1";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                    for (int i = 0; i < obj_tbl_PackageDivisionBOQItem_Li.Count; i++)
                    {
                        tbl_PackageDivisionBOQItem(obj_tbl_PackageDivisionBOQItem_Li[i], trans, cn);
                    }
                }


                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private int tbl_PackageDivisionBOQItem(tbl_PackageDivisionBOQItem obj_tbl_PackageDivisionBOQItem, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageDivisionBOQItem ( [PackageDivisionBOQItem_AddedBy],[PackageDivisionBOQItem_AddedOn],[PackageDivisionBOQItem_ProjectWorkPkg_Id],[PackageDivisionBOQItem_DevisionId],[PackageDivisionBOQItem_PackageBOQ_Id],[PackageDivisionBOQItem_Status]) values ('" + obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_AddedBy + "', getdate(), '" + obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_ProjectWorkPkg_Id + "', N'" + obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_DevisionId + "','" + obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_PackageBOQ_Id + "', '" + obj_tbl_PackageDivisionBOQItem.PackageDivisionBOQItem_Status + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }

    private int Insert_tbl_PackageEMB_Master(tbl_PackageEMB_Master obj_tbl_PackageEMB_Master, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMB_Master ( [PackageEMB_Master_AddedBy],[PackageEMB_Master_AddedOn],[PackageEMB_Master_Date],[PackageEMB_Master_Narration],[PackageEMB_Master_Package_Id],[PackageEMB_Master_Status],[PackageEMB_Master_VoucherNo], [PackageEMB_Master_RA_BillNo]) values ('" + obj_tbl_PackageEMB_Master.PackageEMB_Master_AddedBy + "', getdate(), convert(date, '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Date + "', 103), N'" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Narration + "','" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Package_Id + "', '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_Status + "', N'" + obj_tbl_PackageEMB_Master.PackageEMB_Master_VoucherNo + "', '" + obj_tbl_PackageEMB_Master.PackageEMB_Master_RA_BillNo + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }

    private void Update_tbl_PackageEMB(tbl_PackageEMB obj_tbl_PackageEMB, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;update tbl_PackageEMB set [PackageEMB_ModifiedBy] = '" + obj_tbl_PackageEMB.PackageEMB_AddedBy + "', [PackageEMB_ModifiedOn] = getdate(),  [PackageEMB_Length] = '" + obj_tbl_PackageEMB.PackageEMB_Length + "', [PackageEMB_Breadth] = '" + obj_tbl_PackageEMB.PackageEMB_Breadth + "', [PackageEMB_Qty] = '" + obj_tbl_PackageEMB.PackageEMB_Qty + "', [PackageEMB_Contents] = '" + obj_tbl_PackageEMB.PackageEMB_Contents + "', [PackageEMB_Height] = '" + obj_tbl_PackageEMB.PackageEMB_Height + "', [PackageEMB_QtyExtra] = '" + obj_tbl_PackageEMB.PackageEMB_QtyExtra + "', [PackageEMB_Specification] = N'" + obj_tbl_PackageEMB.PackageEMB_Specification + "', [PackageEMB_Unit_Id] = '" + obj_tbl_PackageEMB.PackageEMB_Unit_Id + "', [PackageEMB_Package_Id] = '" + obj_tbl_PackageEMB.PackageEMB_Package_Id + "', PackageEMB_PackageBOQ_Id = '" + obj_tbl_PackageEMB.PackageEMB_PackageBOQ_Id + "', PackageEMB_PackageEMB_Master_Id = '" + obj_tbl_PackageEMB.PackageEMB_PackageEMB_Master_Id + "', PackageEMB_PercentageToBeReleased = '" + obj_tbl_PackageEMB.PackageEMB_PercentageToBeReleased + "', PackageEMB_GSTType = '" + obj_tbl_PackageEMB.PackageEMB_GSTType + "', PackageEMB_GSTPercenatge = '" + obj_tbl_PackageEMB.PackageEMB_GSTPercenatge + "', PackageEMB_PackageBOQ_OrderNo = '" + obj_tbl_PackageEMB.PackageEMB_PackageBOQ_OrderNo + "' where [PackageEMB_Status] = 1 and [PackageEMB_Id] = '" + obj_tbl_PackageEMB.PackageEMB_Id + "'";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PackageEMB(tbl_PackageEMB obj_tbl_PackageEMB, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMB ( [PackageEMB_AddedBy],[PackageEMB_AddedOn],[PackageEMB_Length],[PackageEMB_Breadth],[PackageEMB_Package_Id],[PackageEMB_Qty],[PackageEMB_Height],[PackageEMB_Contents],[PackageEMB_Specification],[PackageEMB_Status],[PackageEMB_Unit_Id], [PackageEMB_PackageBOQ_Id], [PackageEMB_PackageEMB_Master_Id], [PackageEMB_QtyExtra], [PackageEMB_PercentageToBeReleased],PackageEMB_GSTType,PackageEMB_GSTPercenatge,PackageEMB_PackageBOQ_OrderNo) values ('" + obj_tbl_PackageEMB.PackageEMB_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB.PackageEMB_Length + "','" + obj_tbl_PackageEMB.PackageEMB_Breadth + "','" + obj_tbl_PackageEMB.PackageEMB_Package_Id + "','" + obj_tbl_PackageEMB.PackageEMB_Qty + "','" + obj_tbl_PackageEMB.PackageEMB_Height + "','" + obj_tbl_PackageEMB.PackageEMB_Contents + "',N'" + obj_tbl_PackageEMB.PackageEMB_Specification + "','" + obj_tbl_PackageEMB.PackageEMB_Status + "','" + obj_tbl_PackageEMB.PackageEMB_Unit_Id + "', '" + obj_tbl_PackageEMB.PackageEMB_PackageBOQ_Id + "', '" + obj_tbl_PackageEMB.PackageEMB_PackageEMB_Master_Id + "', '" + obj_tbl_PackageEMB.PackageEMB_QtyExtra + "', '" + obj_tbl_PackageEMB.PackageEMB_PercentageToBeReleased + "', '" + obj_tbl_PackageEMB.PackageEMB_GSTType + "', '" + obj_tbl_PackageEMB.PackageEMB_GSTPercenatge + "', '" + obj_tbl_PackageEMB.PackageEMB_PackageBOQ_OrderNo + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PackageEMB_ADP_Item(tbl_PackageEMB_ADP_Item obj_tbl_PackageEMB_ADP_Item, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageEMB_ADP_Item ( [PackageEMB_ADP_Item_AddedBy],[PackageEMB_ADP_Item_AddedOn],[PackageEMB_ADP_Item_Id],[PackageEMB_ADP_Item_Category_Id],[PackageEMB_ADP_Item_Specification],[PackageEMB_ADP_Item_PackageEMB_Master_Id],[PackageEMB_ADP_Item_Unit_Id],[PackageEMB_ADP_Item_Rate],[PackageEMB_ADP_Item_Qty],[PackageEMB_ADP_Item_Status]) values ('" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Id + "','" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Category_Id + "',N'" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Specification + "','" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_PackageEMB_Master_Id + "','" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Unit_Id + "','" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Rate + "',N'" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Qty + "','" + obj_tbl_PackageEMB_ADP_Item.PackageEMB_ADP_Item_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Insert_tbl_PackageEMB_Approval(List<tbl_PackageEMB_Approval> obj_tbl_PackageEMB_Approval_Li, decimal? Approved_Qty)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string strQuery = "";
                for (int i = 0; i < obj_tbl_PackageEMB_Approval_Li.Count; i++)
                {
                    tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval = obj_tbl_PackageEMB_Approval_Li[i];
                    strQuery = " set dateformat dmy;insert into tbl_PackageEMB_Approval ( [PackageEMB_Approval_AddedBy],[PackageEMB_Approval_AddedOn],[PackageEMB_Approval_Approved_Qty],[PackageEMB_Approval_Comments],[PackageEMB_Approval_Date],[PackageEMB_Approval_No],[PackageEMB_Approval_PackageEMB_Id],[PackageEMB_Approval_Person_Id],[PackageEMB_Approval_Status],[PackageEMB_DocumentPath] ) values ('" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy + "', getdate(),'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Comments + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Date + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_No + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Person_Id + "','" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_Status + "',N'" + obj_tbl_PackageEMB_Approval.PackageEMB_DocumentPath + "');Select @@Identity";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }

                if (Approved_Qty != null)
                {
                    tbl_PackageEMB_Approval obj_tbl_PackageEMB_Approval = obj_tbl_PackageEMB_Approval_Li[0];
                    strQuery = @"set dateformat dmy; 
                    update tbl_PackageEMB set PackageEMB_Qty = '" + Approved_Qty + "', PackageEMB_ModifiedBy = '" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_AddedBy + "', PackageEMB_ModifiedOn = getdate() where PackageEMB_Id = '" + obj_tbl_PackageEMB_Approval.PackageEMB_Approval_PackageEMB_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                }



                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Delete_tbl_PackageEMB(int PackageEMB_Id, int person_Id, string PackageEMB_StatusType)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_PackageEMB set   PackageEMB_Status = 0,PackageEMB_StatusType='" + PackageEMB_StatusType + "',PackageEMB_ModifiedBy='" + person_Id + "',PackageEMB_ModifiedOn=getdate() where PackageEMB_Id = '" + PackageEMB_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Package Invoicing
    public DataSet get_tbl_ProjectWorkPkg_Invoice(int Work_Id, int Project_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, string Project_Code, string Package_Code, int Org_Id, int Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
						ProjectWorkPkg_Id,
						ProjectWorkPkg_Work_Id,
                        ProjectWorkPkg_Code, 
						ProjectWorkPkg_Name,
                        ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
						ProjectWorkPkg_AgreementAmount,
						ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
						ProjectWorkPkg_Agreement_No,
						ProjectWorkPkg_Indent_No,
						ProjectWorkPkg_Vendor_Id,
						ProjectWorkPkg_Staff_Id,
						Vendor_Name = Vendor.Person_Name,
						Vendor_Mobile = Vendor.Person_Mobile1,
						List_ReportingStaff_JEAPE_Name,
						List_ReportingStaff_AEPE_Name,
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Name, 
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
						ULB_Name, 
						Jurisdiction_Name_Eng, 
						Division_Name, 
						Circle_Name, 
                        Zone_Name,
						ProjectWork_DistrictId, 
						ProjectWork_ULB_Id, 
						ProjectWork_DivisionId, 
						Division_CircleId, 
                        ProjectWorkPkg_LockedOn, 
                        ProjectWorkPkg_Locked_By,
                        Total_BOQ = convert(varchar, isnull(t_PackageBOQ.Total_Approved, '0')) +' / ' + convert(varchar, isnull(t_PackageBOQ.Total_BOQ, '0')), 
                        Total_EMB = convert(varchar, isnull(t_PackageEMB.Total_Approved, '0')) +' / ' + convert(varchar, isnull(t_PackageEMB.Total_EMB, '0'))
                    from tbl_ProjectWorkPkg
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on Project_Id = ProjectWork_Project_Id
					join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
					left join tbl_ULB on ULB_Id = ProjectWork_ULB_Id
					left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId
					left join tbl_Zone on Zone_Id = Circle_ZoneId
					left join tbl_PersonDetail Vendor on Vendor.Person_Id = ProjectWorkPkg_Vendor_Id
					left join tbl_PersonDetail Staff on Staff.Person_Id = ProjectWorkPkg_Staff_Id
					
                    left join (select PackageBOQ_Package_Id, Total_BOQ = count(*), Total_Approved = sum(case when isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 1 then 1 else 0 end) from tbl_PackageBOQ left join (select ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrr, PackageBOQ_Approval_Id, PackageBOQ_Approval_PackageBOQ_Id, PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), PackageBOQ_Approval_No, PackageBOQ_Approval_Comments, PackageBOQ_Approval_Approved_Qty, PackageBOQ_DocumentPath, PackageBOQ_Approval_Person_Id from tbl_PackageBOQ_Approval where PackageBOQ_Approval_Status = 1) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrr = 1 where PackageBOQ_Status = 1 group by PackageBOQ_Package_Id) t_PackageBOQ on PackageBOQ_Package_Id = ProjectWorkPkg_Id
                    
                    left join (select PackageEMB_Package_Id, Total_EMB = count(*), Total_Approved = sum(case when isnull(tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 0) > 1 then 1 else 0 end) from tbl_PackageEMB left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id from tbl_PackageEMB_Approval where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1 where PackageEMB_Status = 1 group by PackageEMB_Package_Id) t_PackageEMB on PackageEMB_Package_Id = ProjectWorkPkg_Id

                     left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                            SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                    STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
												inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id
									            WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                FOR XML PATH(''), TYPE)
                                            .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Name
	                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                        where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                    GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id                          
                        ) tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    join 
                    (
	                    select 
		                    PackageInvoice_Package_Id, 
		                    count(*) Total_Invoices 
	                    from tbl_PackageInvoice 
	                    join (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrApp, PackageInvoiceApproval_PackageInvoice_Id, PackageInvoiceApproval_Package_Id, PackageInvoiceApproval_Id, PackageInvoiceApproval_Next_Organisation_Id, PackageInvoiceApproval_Next_Designation_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 AdditionalCondition) t_PackageInvoiceApproval on t_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and rrApp = 1
	                    where PackageInvoice_Status = 1 group by PackageInvoice_Package_Id
                    ) tInvoices on PackageInvoice_Package_Id = ProjectWorkPkg_Id 
                    where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1  ";
        if (Org_Id > 0 && Designation_Id > 0)
        {
            strQuery = strQuery.Replace("AdditionalCondition", " and PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("AdditionalCondition", "");
        }
        if (Work_Id != 0)
        {
            strQuery += " and ProjectWorkPkg_Work_Id = '" + Work_Id + "'";
        }
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = '" + Project_Id + "'";
        }
        if (District_Id != 0)
        {
            strQuery += " and ProjectWork_DistrictId = '" + District_Id + "'";
        }
        if (Zone_Id != 0)
        {
            strQuery += " and Circle_ZoneId = '" + Zone_Id + "'";
        }
        if (Circle_Id != 0)
        {
            strQuery += " and Division_CircleId = '" + Circle_Id + "'";
        }
        if (Division_Id != 0)
        {
            //strQuery += " and ProjectWork_DivisionId = '" + Division_Id + "'";
            strQuery += "and (ProjectWork_DivisionId in (" + Division_Id + ") or ProjectWork_DivisionId in (select ProjectAdditionalArea_DevisionId from tbl_ProjectAdditionalArea where ProjectAdditionalArea_Status = 1 and ProjectAdditionalArea_DevisionId = '" + Division_Id + "'))";
        }
        if (Project_Code != "")
        {
            strQuery += " and ProjectWork_ProjectCode = '" + Project_Code + "'";
        }
        if (Package_Code != "")
        {
            strQuery += " and ProjectWorkPkg_Code = '" + Package_Code + "'";
        }
        strQuery += " order by ProjectWork_Name, ProjectWorkPkg_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_PackageInvoice(tbl_PackageInvoice obj_tbl_PackageInvoice, List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li, List<tbl_PackageInvoiceDocs> obj_tbl_PackageInvoiceDocs_Li, List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li, tbl_PackageEMBApproval obj_tbl_PackageEMBApproval)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_tbl_PackageInvoice.PackageInvoice_VoucherNo == "")
                {
                    obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = get_tbl_TransactionNos(VoucherTypes.Invoice, obj_tbl_PackageInvoice.PackageInvoice_Package_Id, trans, cn);
                }
                obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id;
                obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);

                for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
                {
                    obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                }

                for (int i = 0; i < obj_tbl_PackageInvoiceAdditional_Li.Count; i++)
                {
                    obj_tbl_PackageInvoiceAdditional_Li[i].PackageInvoiceAdditional_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    Insert_tbl_PackageInvoiceAdditional(obj_tbl_PackageInvoiceAdditional_Li[i], trans, cn);
                }
                if (obj_tbl_PackageInvoiceDocs_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageInvoiceDocs_Li.Count; i++)
                    {
                        obj_tbl_PackageInvoiceDocs_Li[i].PackageInvoiceDocs_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                        Insert_tbl_PackageInvoiceDocs(obj_tbl_PackageInvoiceDocs_Li[i], trans, cn);
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private int Insert_tbl_PackageInvoice(tbl_PackageInvoice obj_tbl_PackageInvoice, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoice ( [PackageInvoice_AddedBy],[PackageInvoice_AddedOn],[PackageInvoice_Date],[PackageInvoice_DBR_No],[PackageInvoice_Narration],[PackageInvoice_Package_Id],[PackageInvoice_SBR_No],[PackageInvoice_Status],[PackageInvoice_VoucherNo], [PackageInvoice_PackageEMBMaster_Id]) values ('" + obj_tbl_PackageInvoice.PackageInvoice_AddedBy + "', getdate(), convert(date, '" + obj_tbl_PackageInvoice.PackageInvoice_Date + "', 103), N'" + obj_tbl_PackageInvoice.PackageInvoice_DBR_No + "',N'" + obj_tbl_PackageInvoice.PackageInvoice_Narration + "','" + obj_tbl_PackageInvoice.PackageInvoice_Package_Id + "',N'" + obj_tbl_PackageInvoice.PackageInvoice_SBR_No + "','" + obj_tbl_PackageInvoice.PackageInvoice_Status + "',N'" + obj_tbl_PackageInvoice.PackageInvoice_VoucherNo + "', '" + obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }
    private void Update_tbl_PackageInvoice(tbl_PackageInvoice obj_tbl_PackageInvoice, string Updation_Mode, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();
        string strQuery = "";
        if (Updation_Mode == "P")
            strQuery = " set dateformat dmy; update tbl_PackageInvoice set PackageInvoice_ProcessedBy = '" + obj_tbl_PackageInvoice.PackageInvoice_ProcessedBy + "', PackageInvoice_ProcessedOn = getdate(), PackageInvoice_Date = convert(date, '" + obj_tbl_PackageInvoice.PackageInvoice_Date + "', 103), PackageInvoice_DBR_No = '" + obj_tbl_PackageInvoice.PackageInvoice_DBR_No + "', PackageInvoice_Narration = '" + obj_tbl_PackageInvoice.PackageInvoice_Narration + "', PackageInvoice_VoucherNo = '" + obj_tbl_PackageInvoice.PackageInvoice_VoucherNo + "' where PackageInvoice_Id = '" + obj_tbl_PackageInvoice.PackageInvoice_Id + "'";
        else
            strQuery = " set dateformat dmy; update tbl_PackageInvoice set PackageInvoice_ApprovedBy = '" + obj_tbl_PackageInvoice.PackageInvoice_ApprovedBy + "', PackageInvoice_ApprovedOn = getdate(), PackageInvoice_Date = convert(date, '" + obj_tbl_PackageInvoice.PackageInvoice_Date + "', 103), PackageInvoice_DBR_No = '" + obj_tbl_PackageInvoice.PackageInvoice_DBR_No + "', PackageInvoice_Narration = '" + obj_tbl_PackageInvoice.PackageInvoice_Narration + "', PackageInvoice_VoucherNo = '" + obj_tbl_PackageInvoice.PackageInvoice_VoucherNo + "' where PackageInvoice_Id = '" + obj_tbl_PackageInvoice.PackageInvoice_Id + "'";

        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PackageInvoiceAdditional(tbl_PackageInvoiceAdditional obj_tbl_PackageInvoiceAdditional, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_PackageInvoiceAdditional ( [PackageInvoiceAdditional_AddedBy],[PackageInvoiceAdditional_AddedOn],[PackageInvoiceAdditional_Deduction_Id],[PackageInvoiceAdditional_Deduction_Type],[PackageInvoiceAdditional_Deduction_Value_Final],[PackageInvoiceAdditional_Deduction_Value_Master],[PackageInvoiceAdditional_Invoice_Id],[PackageInvoiceAdditional_Status], [PackageInvoiceAdditional_Comments], [PackageInvoiceAdditional_Deduction_isFlat]) values ('" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_AddedBy + "', getdate(),'" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Id + "','" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Type + "','" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Final + "','" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_Value_Master + "','" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Invoice_Id + "','" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Status + "', '" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Comments + "', '" + obj_tbl_PackageInvoiceAdditional.PackageInvoiceAdditional_Deduction_isFlat + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Update_tbl_PackageInvoiceAdditional(int Invoice_Id, int Added_By, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; update tbl_PackageInvoiceAdditional set PackageInvoiceAdditional_Status = 0, PackageInvoiceAdditional_ModifiedBy = '" + Added_By + "', PackageInvoiceAdditional_ModifiedOn = getdate() where PackageInvoiceAdditional_Status = 1 and PackageInvoiceAdditional_Invoice_Id = '" + Invoice_Id + "'";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_PackageInvoiceDocs(tbl_PackageInvoiceDocs obj_tbl_PackageInvoiceDocs, SqlTransaction trans, SqlConnection cn)
    {
        if (!Directory.Exists(Server.MapPath(".") + "\\Downloads\\Invoice\\" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id.ToString()))
        {
            Directory.CreateDirectory(Server.MapPath(".") + "\\Downloads\\Invoice\\" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id.ToString());
        }
        if (obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileBytes != null && obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileBytes.Length > 0)
        {
            obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName = DateTime.Now.Ticks.ToString("x") + "." + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName;
            obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName = "\\Downloads\\Invoice\\" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id.ToString() + "\\" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName;
            File.WriteAllBytes(Server.MapPath(".") + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName, obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileBytes);
        }
        else
        {
            obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName = "";
        }
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoiceDocs ([PackageInvoiceDocs_AddedBy],[PackageInvoiceDocs_AddedOn],[PackageInvoiceDocs_FileName],[PackageInvoiceDocs_Invoice_Id],[PackageInvoiceDocs_Status],[PackageInvoiceDocs_Type], [PackageInvoiceDocs_OrderNo], [PackageInvoiceDocs_Comments]) values ('" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_AddedBy + "', getdate(),N'" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_FileName + "','" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Invoice_Id + "','" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Status + "',N'" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Type + "', '" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_OrderNo + "', '" + obj_tbl_PackageInvoiceDocs.PackageInvoiceDocs_Comments + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_PackageInvoiceEMBMasterLink(tbl_PackageInvoiceEMBMasterLink obj_tbl_PackageInvoiceEMBMasterLink, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_PackageInvoiceEMBMasterLink ( [PackageInvoiceEMBMasterLink_AddedBy],[PackageInvoiceEMBMasterLink_AddedOn],[PackageInvoiceEMBMasterLink_Invoice_Id],PackageInvoiceEMBMasterLink_EMBMaster_Id,[PackageInvoiceEMBMasterLink_Status]) values ('" + obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_AddedBy + "', getdate(),'" + obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id + "','" + obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_EMBMaster_Id + "','" + obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private int Insert_tbl_PackageInvoiceItem(tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem, SqlTransaction trans, SqlConnection cn)
    {
        DataSet dsInsert = new DataSet();

        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoiceItem ( [PackageInvoiceItem_AddedBy],[PackageInvoiceItem_AddedOn],[PackageInvoiceItem_AmountQuoted],[PackageInvoiceItem_BOQ_Id],[PackageInvoiceItem_Invoice_Id],[PackageInvoiceItem_RateEstimated],[PackageInvoiceItem_RateQuoted],[PackageInvoiceItem_Remarks],[PackageInvoiceItem_Status],[PackageInvoiceItem_Total_Qty],[PackageInvoiceItem_Total_Qty_Billed],[PackageInvoiceItem_Total_Qty_BOQ], [PackageInvoiceItem_PercentageToBeReleased],PackageInvoiceItem_PackageEMB_Id,PackageInvoiceItem_GSTType,PackageInvoiceItem_QtyExtra,PackageInvoiceItem_PackageBOQ_OrderNo) values ('" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy + "', getdate(),'" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AmountQuoted + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Invoice_Id + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted + "',N'" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Remarks + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_Billed + "','" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ + "', '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased + "', '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageEMB_Id + "', '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_GSTType + "', '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra + "', '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageBOQ_OrderNo + "');Select @@Identity";
        int ret_type = 0;
        if (trans == null)
        {
            try
            {
                dsInsert = ExecuteSelectQuery(strQuery);
                ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                ret_type = 0;
            }
        }
        else
        {
            dsInsert = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
            ret_type = Convert.ToInt32(dsInsert.Tables[0].Rows[0][0].ToString());
        }
        return ret_type;
    }
    private void Update_tbl_PackageInvoiceItem(tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; update tbl_PackageInvoiceItem set PackageInvoiceItem_Remarks = '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Remarks + "' where PackageInvoiceItem_Id = '" + obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Id + "'";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    public DataSet get_tbl_PackageInvoice(int Package_Id, int Zone_Id, int Circle_Id, int Division_Id, int Scheme_Id, int Org_Id, int Designation_Id, bool Show_All_For_Report)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoice_Id, 
                        PackageInvoice_PackageEMBMaster_Id,
	                    PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    PackageInvoice_VoucherNo,
	                    PackageInvoice_Narration,
	                    PackageInvoice_SBR_No,
	                    PackageInvoice_DBR_No,
	                    PackageInvoice_AddedBy,
	                    PackageInvoice_AddedOn,
	                    PackageInvoice_Package_Id, 
                        PackageInvoice_ProcessedOn, 
                        PackageInvoice_ProcessedBy,
	                    tPackageInvoiceItem.Total_Line_Items, 
	                    tPackageInvoiceItem.PackageInvoiceItem_Total_Qty, 
	                    tPackageInvoiceItem.Total_Amount, 
                        ProjectWorkPkg_Vendor_Id, 
	                    ProjectWork_DivisionId, 
	                    ProjectWork_Id, 
	                    Vendor_Name = Person_Name, 
	                    Vendor_Mobile = Person_Mobile1, 
	                    Vendor_EmailId = Person_EmailId, 
                        Vendor_Address = Person_AddressLine1, 
	                    Vendor_GSTIN = '',
	                    Division_Name, 
	                    tInvoiceApproval.Designation_DesignationName, 
	                    tInvoiceApproval.OfficeBranch_Name, 
	                    tInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id, 
	                    tInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id, 
	                    tInvoiceApproval.PackageInvoiceApproval_Date, 
						t_FinancialTrans.FinancialTrans_Amount, 
						t_FinancialTrans.FinancialTrans_TransAmount,
                        tInvoiceApproval.PackageInvoiceApproval_Status_Id,
                        Invoice_Status = isnull(InvoiceStatus_Name, 'Pending'), 
                        Designation_Current, 
	                    Organisation_Current, 
                        Circle_Name, 
                        Division_Name, 
                        Project_Name, 
                        ProjectWork_ProjectCode, 
                        ProjectWork_Name, 
                        ProjectWorkPkg_Code, 
                        ProjectWorkPkg_Name,
                        List_EMBNo,
                        PackageInvoiceCover_Id=Isnull(PackageInvoiceCover_Id,0)
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    as tbl_PackageInvoiceEMBMasterLink1 on tbl_PackageInvoiceEMBMasterLink1.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
					join tbl_Project on ProjectWork_Project_Id = Project_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    join tbl_Circle on Circle_Id = Division_CircleId
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    left join tbl_PersonDetail on Person_Id = ProjectWorkPkg_Vendor_Id
                    left join tbl_PackageInvoiceCover on PackageInvoiceCover_Invoice_Id=PackageInvoice_Id and PackageInvoiceCover_Status=1
                    left join 
                    (
	                    select 
		                    PackageInvoiceItem_Invoice_Id,
		                    Total_Line_Items = count(*),
		                    PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
                            Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                    from tbl_PackageInvoiceItem
	                    where PackageInvoiceItem_Status = 1
	                    group by PackageInvoiceItem_Invoice_Id
                    ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
                    left join 
                    (
	                    select 
		                    ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrrrr,
		                    PackageInvoiceApproval_Next_Designation_Id,
		                    PackageInvoiceApproval_Next_Organisation_Id,
		                    PackageInvoiceApproval_Comments,
		                    PackageInvoiceApproval_AddedBy,
		                    PackageInvoiceApproval_AddedOn,
		                    PackageInvoiceApproval_Status_Id,
		                    PackageInvoiceApproval_Package_Id,
		                    PackageInvoiceApproval_PackageInvoice_Id,
                            InvoiceStatus_Name,
		                    PackageInvoiceApproval_Date = convert(char(10), PackageInvoiceApproval_Date, 103),
		                    PackageInvoiceApproval_Id, 
		                    tbl_Designation.Designation_DesignationName, 
		                    tbl_OfficeBranch.OfficeBranch_Name, 
                            Designation_Current = Designation_Current.Designation_DesignationName, 
	                        Organisation_Current = Organisation_Current.OfficeBranch_Name 
	                    from tbl_PackageInvoiceApproval
	                    left join tbl_OfficeBranch on OfficeBranch_Id = PackageInvoiceApproval_Next_Organisation_Id
	                    left join tbl_Designation on Designation_Id = PackageInvoiceApproval_Next_Designation_Id
                        join tbl_PersonDetail on Person_Id = PackageInvoiceApproval_AddedBy 
                        join tbl_PersonJuridiction on PersonJuridiction_PersonId = PackageInvoiceApproval_AddedBy
                        left join tbl_Designation Designation_Current on Designation_Current.Designation_Id = PersonJuridiction_DesignationId
                        left join tbl_OfficeBranch Organisation_Current on Organisation_Current.OfficeBranch_Id = Person_BranchOffice_Id
                        left join tbl_InvoiceStatus on InvoiceStatus_Id = PackageInvoiceApproval_Status_Id
	                    where PackageInvoiceApproval_Status = 1
                    ) tInvoiceApproval on tInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and tInvoiceApproval.rrrrr = 1
					left join 
					(
						select 
							FinancialTrans_Invoice_Id, 
							FinancialTrans_TransAmount = sum(isnull(FinancialTrans_TransAmount, 0)),
							FinancialTrans_Amount = max(isnull(FinancialTrans_Amount, 0))
						from tbl_FinancialTrans
						where FinancialTrans_Status = 1
						group by FinancialTrans_Invoice_Id
					) t_FinancialTrans on t_FinancialTrans.FinancialTrans_Invoice_Id = PackageInvoice_Id
                left join (
                SELECT	PackageInvoiceEMBMasterLink_Invoice_Id,
			                                        STUFF((SELECT ', ' + CAST(PackageEMB_Master_VoucherNo AS VARCHAR(100)) [text()]
                                                    FROM tbl_PackageInvoiceEMBMasterLink
									                inner join tbl_PackageEMB_Master on PackageEMB_Master_Id=PackageInvoiceEMBMasterLink_EMBMaster_Id 
                                                    WHERE PackageInvoiceEMBMasterLink_Invoice_Id = t.PackageInvoiceEMBMasterLink_Invoice_Id and tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_EMBNo
	                                        FROM tbl_PackageInvoiceEMBMasterLink t
                                            where t.PackageInvoiceEMBMasterLink_Status = 1
	                                        GROUP BY PackageInvoiceEMBMasterLink_Invoice_Id
                ) tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    where PackageInvoice_Status = 1 AdditionalCondition2";
        if (Show_All_For_Report)
        {
            if (Org_Id > 0 && Designation_Id > 0)
            {
                strQuery = strQuery.Replace("AdditionalCondition1", " and PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
                strQuery = strQuery.Replace("AdditionalCondition2", "");
            }
            else
            {
                strQuery = strQuery.Replace("AdditionalCondition1", "");
                strQuery = strQuery.Replace("AdditionalCondition2", "");
            }
        }
        else
        {
            if (Org_Id > 0 && Designation_Id > 0)
            {
                strQuery = strQuery.Replace("AdditionalCondition2", " and tInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and tInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
                strQuery = strQuery.Replace("AdditionalCondition1", "");
            }
            else
            {
                strQuery = strQuery.Replace("AdditionalCondition1", "");
                strQuery = strQuery.Replace("AdditionalCondition2", "");
            }
        }

        if (Show_All_For_Report)
        {
            if (Designation_Id == 33)
            {
                strQuery += " and Zone_Id in (select PersonAdditionalCharge_Jurisdiction_Id from tbl_PersonAdditionalCharge where PersonAdditionalCharge_Status = 1 and PersonAdditionalCharge_PersonId = '" + Session["Person_Id"].ToString() + "')";
            }
        }
        else
        {
            if (Designation_Id == 33)
            {
                strQuery += " and Zone_Id in (select PersonAdditionalCharge_Jurisdiction_Id from tbl_PersonAdditionalCharge where PersonAdditionalCharge_Status = 1 and PersonAdditionalCharge_PersonId = '" + Session["Person_Id"].ToString() + "')";
            }
        }

        if (Package_Id > 0)
        {
            strQuery += "and PackageInvoice_Package_Id = '" + Package_Id + "'";
        }
        if (Scheme_Id > 0)
        {
            strQuery += "and ProjectWork_Project_Id = '" + Scheme_Id + "'";
        }
        if (Division_Id > 0)
        {
            strQuery += "and ProjectWork_DivisionId = '" + Division_Id + "'";
        }
        if (Circle_Id > 0)
        {
            strQuery += "and Circle_Id = '" + Circle_Id + "'";
        }
        if (Zone_Id > 0)
        {
            strQuery += "and Zone_Id = '" + Zone_Id + "'";
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_Invoice_Details(int Invoice_Id)
    {

        //Total_Amount = case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end,
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoice_Id, 
	                    PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    PackageInvoice_VoucherNo,
	                    PackageInvoice_Narration,
	                    PackageInvoice_SBR_No,
	                    PackageInvoice_DBR_No,
	                    PackageInvoice_AddedBy,
	                    PackageInvoice_AddedOn,
	                    PackageInvoice_Package_Id, 
                        PackageInvoice_ProcessedOn, 
                        PackageInvoice_ProcessedBy, 
                        Isnull(PackageInvoice_PackageEMBMaster_Id,0) as PackageInvoice_PackageEMBMaster_Id,
                        ProjectWorkPkg_Vendor_Id, 
                        ProjectWork_DivisionId, 
                        Vendor_Name = Person_Name, 
                        Vendor_Mobile = Person_Mobile1, 
                        Vendor_EmailId = Person_EmailId, 
                        Vendor_Address = Person_AddressLine1, 
                        Vendor_GSTIN = '',
                        Division_Name, 
                        ProjectWorkPkg_AgreementAmount,
                        start_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        End_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103),
                        ProjectWorkPkg_Agreement_No, 
                        Project_Name, 
                        ProjectWorkPkg_Name,
                        ProjectWork_Name,
                        ProjectWork_Description
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on ProjectWork_Project_Id = Project_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    left join tbl_PersonDetail on Person_Id = ProjectWorkPkg_Vendor_Id
                    where PackageInvoice_Status = 1 and PackageInvoice_Id = Invoice_IdCond; 

                    select 
	                    PackageInvoiceItem_Id,
	                    PackageInvoiceItem_Invoice_Id,
	                    PackageInvoiceItem_PackageEMB_Id,
                        PackageEMB_Unit_Id,
                        Unit_Name,
	                    PackageEMB_Specification,
	                    PackageEMB_Length,
	                    PackageEMB_Height,
	                    PackageEMB_Breadth,
	                    PackageEMB_Contents,
	                    PackageEMB_QtyExtra,
	                    PackageInvoiceItem_Total_Qty_BOQ,
	                    PackageInvoiceItem_Total_Qty_Billed,
                        Isnull(PackageInvoiceItem_QtyExtra,0) as PackageInvoiceItem_QtyExtra,
                        PackageInvoiceItem_QtyExtraPer=case when Isnull(PackageInvoiceItem_QtyExtra,0)=0 then '0 %' 
							 when isnull(PackageInvoiceItem_Total_Qty,0)=0 then '0 %' else convert(varchar(150),cast(((PackageInvoiceItem_QtyExtra/PackageInvoiceItem_Total_Qty)*100) as int ) )+' %' end ,
	                    PackageInvoiceItem_Total_Qty,
	                    PackageInvoiceItem_RateEstimated,
	                    PackageInvoiceItem_RateQuoted,
	                    PackageInvoiceItem_AmountQuoted,
	                    PackageInvoiceItem_Remarks,
	                    PackageInvoiceItem_AddedBy,
	                    PackageInvoiceItem_AddedOn,
	                    PackageInvoiceItem_Status, 
	                    PackageInvoiceItem_PercentageToBeReleased = isnull(PackageInvoiceItem_PercentageToBeReleased, 100), 
                        Total_Tax,
                        Total_Amount = Total_Amount, 
                        Total_Rate = Total_Rate, 
                        Amount=cast((case when PackageInvoiceItem_PercentageToBeReleased=0 then 
						(PackageInvoiceItem_Total_Qty_BOQ*Total_Rate) when PackageInvoiceItem_PercentageToBeReleased=100 then (PackageInvoiceItem_Total_Qty_BOQ*Total_Rate) else 
						(((PackageInvoiceItem_Total_Qty_BOQ*Total_Rate)*PackageInvoiceItem_PercentageToBeReleased)/100) end) as decimal(18,2)),
                        PackageInvoiceItem_GSTType,
						EMB_Master_Date = convert(char(10), PackageEMB_Master_Date, 103),
						PackageEMB_Master_VoucherNo,
						PackageEMB_Master_RA_BillNo, 
                        PackageBOQ_QtyPaid
                    from tbl_PackageInvoiceItem
                    join tbl_PackageEMB on PackageEMB_Id = PackageInvoiceItem_PackageEMB_Id
                    join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                    JOIN tbl_PackageBOQ on PackageBOQ_Id = PackageEMB_PackageBOQ_Id
                    left join tbl_Unit on Unit_Id = PackageEMB_Unit_Id
                    where PackageInvoiceItem_Status = 1 and PackageInvoiceItem_Invoice_Id = Invoice_IdCond order by PackageInvoiceItem_PackageBOQ_OrderNo ; 

                    select 
	                    PackageInvoice_Id, 
	                    PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    PackageInvoice_VoucherNo,
	                    PackageInvoice_Narration,
	                    PackageInvoice_SBR_No,
	                    PackageInvoice_DBR_No,
	                    PackageInvoice_AddedBy,
	                    PackageInvoice_AddedOn,
	                    PackageInvoice_Package_Id, 
	                    tPackageInvoiceItem.Total_Line_Items, 
	                    tPackageInvoiceItem.PackageInvoiceItem_Total_Qty, 
	                    tPackageInvoiceItem.Total_Amount, 
                        ProjectWorkPkg_Vendor_Id, 
	                    ProjectWork_DivisionId, 
	                    ProjectWork_Id, 
	                    Vendor_Name = Person_Name, 
	                    Vendor_Mobile = Person_Mobile1, 
	                    Vendor_EmailId = Person_EmailId, 
                        Vendor_Address = Person_AddressLine1, 
	                    Vendor_GSTIN = '',
	                    Division_Name,
                        List_EMBNo
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                     as tbl_PackageInvoiceEMBMasterLink1 on tbl_PackageInvoiceEMBMasterLink1.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    left join tbl_PersonDetail on Person_Id = ProjectWorkPkg_Vendor_Id
                    left join 
                    (
	                    select 
		                    PackageInvoiceItem_Invoice_Id,
		                    Total_Line_Items = count(*),
		                    PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
                            Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                    from tbl_PackageInvoiceItem
	                    where PackageInvoiceItem_Status = 1
	                    group by PackageInvoiceItem_Invoice_Id
                    ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
                    left join (
                    SELECT	PackageInvoiceEMBMasterLink_Invoice_Id,
			                                            STUFF((SELECT ', ' + CAST(PackageEMB_Master_VoucherNo AS VARCHAR(100)) [text()]
                                                        FROM tbl_PackageInvoiceEMBMasterLink
									                    inner join tbl_PackageEMB_Master on PackageEMB_Master_Id=PackageInvoiceEMBMasterLink_EMBMaster_Id 
                                                        WHERE PackageInvoiceEMBMasterLink_Invoice_Id = t.PackageInvoiceEMBMasterLink_Invoice_Id and tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status = 1
                                                        FOR XML PATH(''), TYPE)
                                                    .value('.','NVARCHAR(MAX)'),1,2,' ') as List_EMBNo
	                                            FROM tbl_PackageInvoiceEMBMasterLink t
                                                where t.PackageInvoiceEMBMasterLink_Status = 1
	                                            GROUP BY PackageInvoiceEMBMasterLink_Invoice_Id
                    ) tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    where PackageInvoice_Status = 1 and PackageInvoice_Id = Invoice_IdCond; 

					select 
						PackageInvoiceAdditional_Invoice_Id,
						Deduction_Category,
						PackageInvoiceAdditional_Deduction_isFlat,
                        Deduction_Mode,
						Deduction_Name = (case when ISNULL(PackageInvoiceAdditional_Deduction_isFlat, '0') = '0' then Deduction_Name + ' (' + rtrim(ltrim(convert(varchar, PackageInvoiceAdditional_Deduction_Value_Master)))  + ')' else Deduction_Name end) + '[' + Deduction_Mode + ']',
						PackageInvoiceAdditional_Deduction_Value_Final = rtrim(ltrim(PackageInvoiceAdditional_Deduction_Value_Final)) 
					from tbl_PackageInvoiceAdditional
					join tbl_Deduction on Deduction_Id = PackageInvoiceAdditional_Deduction_Id  and PackageInvoiceAdditional_Status = 1 and PackageInvoiceAdditional_Invoice_Id = Invoice_IdCond order by Deduction_Mode ;

                    select 
	                    PackageInvoiceDocs_Id,
	                    PackageInvoiceDocs_Invoice_Id,
	                    PackageInvoiceDocs_FileName,
	                    TradeDocument_Name,
	                    PackageInvoiceDocs_AddedBy,
	                    PackageInvoiceDocs_AddedOn,
	                    Person_Name,
	                    Designation_DesignationName, 
                        PackageInvoiceDocs_OrderNo, 
                        PackageInvoiceDocs_Comments
                    from tbl_PackageInvoiceDocs
                    left join tbl_TradeDocument on TradeDocument_Id = PackageInvoiceDocs_Type
                    join tbl_PersonDetail on Person_Id = PackageInvoiceDocs_AddedBy
                    join tbl_PersonJuridiction on PersonJuridiction_PersonId = Person_Id
                    join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    where PackageInvoiceDocs_Status = 1 and PackageInvoiceDocs_Invoice_Id = Invoice_IdCond ";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_Invoice_Documents_Details(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoiceDocs_Id,
	                    PackageInvoiceDocs_Invoice_Id,
	                    PackageInvoiceDocs_FileName,
	                    TradeDocument_Name,
	                    PackageInvoiceDocs_AddedBy,
	                    PackageInvoiceDocs_AddedOn,
	                    Person_Name,
	                    Designation_DesignationName
                    from tbl_PackageInvoiceDocs
                    left join tbl_TradeDocument on TradeDocument_Id = PackageInvoiceDocs_Type
                    join tbl_PersonDetail on Person_Id = PackageInvoiceDocs_AddedBy
                    join tbl_PersonJuridiction on PersonJuridiction_PersonId = Person_Id
                    join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    where PackageInvoiceDocs_Status = 1 and PackageInvoiceDocs_Invoice_Id = Invoice_IdCond ";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_PreviousInvoiceDetails(int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select PackageInvoice_Id,
	               PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	               PackageInvoice_VoucherNo,
	               PackageInvoice_AddedOn,
	               Total_Amount=cast((ISNULL(FinancialTrans_TransAmount,0)/100000) as decimal(18,2))
                from tbl_PackageInvoice
                inner join tbl_FinancialTrans on FinancialTrans_Invoice_Id=PackageInvoice_Id
                where PackageInvoice_Status=1 
                and PackageInvoice_Package_Id=Package_IdCond
                and FinancialTrans_EntryType='Fund Allocated' and FinancialTrans_TransType='C'
                and PackageInvoice_Id not in (select PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1) ";

        strQuery = strQuery.Replace("Package_IdCond", Package_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_EMBDetailsInvoiceWise(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"
                    select PackageEMB_Master_Id,
                    PackageEMB_Master_VoucherNo,
                    PackageEMB_Master_RA_BillNo,
                    PackageEMB_Master_Date=CONVERT(varchar(150),PackageEMB_Master_Date,101) 
                    from tbl_PackageEMB_Master
                    inner join tbl_PackageInvoiceEMBMasterLink on PackageInvoiceEMBMasterLink_EMBMaster_Id=PackageEMB_Master_Id and PackageInvoiceEMBMasterLink_Status=1
                    where PackageEMB_Master_Status=1 and PackageInvoiceEMBMasterLink_Invoice_Id=Invoice_IdCond ";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_EMBItemsDetailsInvoiceWise(int PackageEMB_Master_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"
                    select PackageEMB_Specification,
		            Unit_Name,
		            PackageEMB_Qty,
                    PackageEMB_Length,
		            PackageEMB_Breadth,
		            PackageEMB_Height,
		            PackageEMB_Contents,
		            PackageEMB_QtyExtra,
		            PackageEMB_PercentageToBeReleased,
		            PackageEMB_GSTType,PackageEMB_GSTPercenatge
            from tbl_PackageEMB
            inner join tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id=PackageEMB_Id
            left join tbl_Unit on Unit_Id=PackageEMB_Unit_Id
            where PackageEMB_Status=1 and PackageEMB_PackageEMB_Master_Id=PackageEMB_Master_IdCond ";

        strQuery = strQuery.Replace("PackageEMB_Master_IdCond", PackageEMB_Master_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Update_tbl_PackageInvoice(tbl_PackageInvoice obj_tbl_PackageInvoice, List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_Li, List<tbl_PackageInvoiceDocs> obj_tbl_PackageInvoiceDocs_Li, List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li, tbl_FinancialTrans obj_tbl_FinancialTrans, string Updation_Mode, tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval, int Organisation_Id, int Designation_Id)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Update_tbl_PackageInvoice(obj_tbl_PackageInvoice, Updation_Mode, trans, cn);

                for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
                {
                    Update_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                    string strQuery = "";
                    strQuery = " set dateformat dmy; Update  tbl_PackageInvoiceItem_Tax set   PackageInvoiceItem_Tax_Status = 0,PackageInvoiceItem_Tax_ModifiedBy='" + obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_AddedBy + "',PackageInvoiceItem_Tax_ModifiedOn=getdate() where PackageInvoiceItem_Tax_PackageInvoiceItem_Id = '" + obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id + "' ";
                    ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    if (obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        for (int k = 0; k < obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li.Count; k++)
                        {
                            obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k].PackageInvoiceItem_Tax_PackageInvoiceItem_Id = obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id;
                            Insert_tbl_PackageInvoiceItem_Tax(obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k], trans, cn); ;
                        }
                    }


                }
                Update_tbl_PackageInvoiceAdditional(obj_tbl_PackageInvoice.PackageInvoice_Id, obj_tbl_PackageInvoice.PackageInvoice_AddedBy, trans, cn);
                for (int i = 0; i < obj_tbl_PackageInvoiceAdditional_Li.Count; i++)
                {
                    obj_tbl_PackageInvoiceAdditional_Li[i].PackageInvoiceAdditional_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    Insert_tbl_PackageInvoiceAdditional(obj_tbl_PackageInvoiceAdditional_Li[i], trans, cn);
                }
                if (obj_tbl_PackageInvoiceDocs_Li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageInvoiceDocs_Li.Count; i++)
                    {
                        obj_tbl_PackageInvoiceDocs_Li[i].PackageInvoiceDocs_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                        Insert_tbl_PackageInvoiceDocs(obj_tbl_PackageInvoiceDocs_Li[i], trans, cn);
                    }
                }
                int _Loop = get_Loop("Invoice", Organisation_Id, Designation_Id, trans, cn);

                if (obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id == 1 || obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id == 4)
                {
                    ds = get_ProcessConfigMaster_Next("Invoice", Organisation_Id, Designation_Id, obj_tbl_PackageInvoice.PackageInvoice_Id, _Loop, trans, cn);
                }
                else
                {
                    ds = get_ProcessConfigMaster_Prev("Invoice", Organisation_Id, Designation_Id, obj_tbl_PackageInvoice.PackageInvoice_Id, _Loop, trans, cn);
                }
                if (AllClasses.CheckDataSet(ds))
                {
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                    Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                }
                else
                {
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = 0;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = 0;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 0;
                    Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                }
                if (obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id == 1)
                {
                    if (obj_tbl_FinancialTrans != null)
                    {
                        Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans, trans, cn);
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Update_tbl_PackageInvoice_Mark(List<tbl_PackageInvoiceApproval> obj_tbl_PackageInvoiceApproval_Li, int Organisation_Id, int Designation_Id)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PackageInvoiceApproval_Li.Count; i++)
                {
                    tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = obj_tbl_PackageInvoiceApproval_Li[i];
                    int _Loop = get_Loop("Invoice", Organisation_Id, Designation_Id, trans, cn);
                    if (obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id == 1 || obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id == 4)
                    {
                        ds = get_ProcessConfigMaster_Next("Invoice", Organisation_Id, Designation_Id, obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id, _Loop, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_Prev("Invoice", Organisation_Id, Designation_Id, obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id, _Loop, trans, cn);
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                    }
                    else
                    {
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = 0;
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = 0;
                        obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 0;
                        Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Delete_Invoicing(int Invoice_Id, int person_Id)
    {
        bool flag = false;
        try
        {


            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    string strQueryInvoice = @"select PackageInvoiceApproval_Id 
                                                from tbl_PackageInvoiceApproval where PackageInvoiceApproval_PackageInvoice_Id = '" + Invoice_Id + "' and PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_Next_Organisation_Id=2 ";
                    try
                    {
                        ds = ExecuteSelectQuery(strQueryInvoice);
                    }
                    catch
                    {
                        ds = null;
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        cn.Close();
                        flag = false;
                    }
                    else
                    {

                        string strQuery = "";
                        strQuery = " set dateformat dmy; Update  tbl_PackageInvoice set   PackageInvoice_Status = 0,PackageInvoice_ModifiedBy='" + person_Id + "',PackageInvoice_ModifiedOn=getdate() where PackageInvoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceItem set   PackageInvoiceItem_Status = 0,PackageInvoiceItem_ModifiedBy='" + person_Id + "',PackageInvoiceItem_ModifiedOn=getdate() where PackageInvoiceItem_Invoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceItem_Tax set   PackageInvoiceItem_Tax_Status = 0,PackageInvoiceItem_Tax_ModifiedBy='" + person_Id + "',PackageInvoiceItem_Tax_ModifiedOn=getdate() where PackageInvoiceItem_Tax_PackageInvoiceItem_Id in (select PackageInvoiceItem_Id from tbl_PackageInvoiceItem where PackageInvoiceItem_Invoice_Id = '" + Invoice_Id + "' ) ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceAdditional set   PackageInvoiceAdditional_Status = 0,PackageInvoiceAdditional_ModifiedBy='" + person_Id + "',PackageInvoiceAdditional_ModifiedOn=getdate() where PackageInvoiceAdditional_Invoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceEMBMasterLink set   PackageInvoiceEMBMasterLink_Status = 0,PackageInvoiceEMBMasterLink_ModifiedBy='" + person_Id + "',PackageInvoiceEMBMasterLink_ModifiedOn=getdate() where PackageInvoiceEMBMasterLink_Invoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceApproval set   PackageInvoiceApproval_Status = 0,PackageInvoiceApproval_ModifiedBy='" + person_Id + "',PackageInvoiceApproval_ModifiedOn=getdate() where PackageInvoiceApproval_PackageInvoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceCover set   PackageInvoiceCover_Status = 0,PackageInvoiceCover_ModifiedBy='" + person_Id + "',PackageInvoiceCover_ModifiedOn=getdate() where PackageInvoiceCover_Invoice_Id = '" + Invoice_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceDocs set   PackageInvoiceDocs_Status = 0,PackageInvoiceDocs_ModifiedBy='" + person_Id + "',PackageInvoiceDocs_ModifiedOn=getdate() where PackageInvoiceDocs_Invoice_Id = '" + Invoice_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                        trans.Commit();
                        cn.Close();
                        flag = true;
                    }

                }
                catch
                {
                    trans.Rollback();
                    cn.Close();
                    flag = false;
                }
            }


            return flag;
        }
        catch
        {
            return flag;
        }
    }
    public bool RollBack_Invoicing(int PackageInvoiceApproval_Id, int Invoice_Id, int person_Id)
    {
        bool flag = false;
        try
        {


            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    string strQueryInvoice = @"select PackageInvoiceApproval_Id 
                                                from tbl_PackageInvoiceApproval where PackageInvoiceApproval_PackageInvoice_Id = '" + Invoice_Id + "' and PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_Next_Organisation_Id=2 ";
                    try
                    {
                        ds = ExecuteSelectQuery(strQueryInvoice);
                    }
                    catch
                    {
                        ds = null;
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        cn.Close();
                        flag = false;
                    }
                    else
                    {

                        string strQuery = "";
                        strQuery += " set dateformat dmy; Update  tbl_PackageInvoiceApproval set   PackageInvoiceApproval_Status = 0,PackageInvoiceApproval_ModifiedBy='" + person_Id + "',PackageInvoiceApproval_ModifiedOn=getdate() where PackageInvoiceApproval_Id = '" + PackageInvoiceApproval_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                        trans.Commit();
                        cn.Close();
                        flag = true;
                    }

                }
                catch
                {
                    trans.Rollback();
                    cn.Close();
                    flag = false;
                }
            }


            return flag;
        }
        catch
        {
            return flag;
        }
    }

    public bool Delete_EMB(int EMBMaster_Id, int person_Id)
    {
        bool flag = false;
        try
        {


            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    string strQueryInvoice = @"select PackageInvoiceEMBMasterLink_Id 
                                                from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_EMBMaster_Id = '" + EMBMaster_Id + "' and PackageInvoiceEMBMasterLink_Status = 1";
                    try
                    {
                        ds = ExecuteSelectQuery(strQueryInvoice);
                    }
                    catch
                    {
                        ds = null;
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        cn.Close();
                        flag = false;
                    }
                    else
                    {

                        string strQuery = "";
                        strQuery = " set dateformat dmy; Update  tbl_PackageEMB_Master set   PackageEMB_Master_Status = 0,PackageEMB_Master_ModifiedBy='" + person_Id + "',PackageEMB_Master_ModifiedOn=getdate() where PackageEMB_Master_Id = '" + EMBMaster_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMB set   PackageEMB_Status = 0,PackageEMB_ModifiedBy='" + person_Id + "',PackageEMB_ModifiedOn=getdate() where PackageEMB_PackageEMB_Master_Id = '" + EMBMaster_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMBApproval set   PackageEMBApproval_Status = 0,PackageEMBApproval_ModifiedBy='" + person_Id + "',PackageEMBApproval_ModifiedOn=getdate() where PackageEMBApproval_PackageEMBMaster_Id = '" + EMBMaster_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMB_Tax set   PackageEMB_Tax_Status = 0,PackageEMB_Tax_ModifiedBy='" + person_Id + "',PackageEMB_Tax_ModifiedOn=getdate() where PackageEMB_Tax_Package_Id in (select PackageEMB_Id from tbl_PackageEMB where PackageEMB_PackageEMB_Master_Id = '" + EMBMaster_Id + "' ) ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMB_Approval set   PackageEMB_Approval_Status = 0,PackageEMB_Approval_ModifiedBy='" + person_Id + "',PackageEMB_Approval_ModifiedOn=getdate() where PackageEMB_Approval_PackageEMB_Id in (select PackageEMB_Id from tbl_PackageEMB where PackageEMB_PackageEMB_Master_Id = '" + EMBMaster_Id + "' ) ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMB_ADP_Item set   PackageEMB_ADP_Item_Status = 0,PackageEMB_ADP_Item_ModifiedBy='" + person_Id + "',PackageEMB_ADP_Item_ModifiedOn=getdate() where PackageEMB_ADP_Item_PackageEMB_Master_Id = '" + EMBMaster_Id + "' ";
                        strQuery += Environment.NewLine;
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMB_ExtraItem set   PackageEMB_ExtraItem_Status = 0,PackageEMB_ExtraItem_ModifiedBy='" + person_Id + "',PackageEMB_ExtraItem_ModifiedOn=getdate() where PackageEMB_ExtraItem_PackageEMB_Master_Id = '" + EMBMaster_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                        trans.Commit();
                        cn.Close();
                        flag = true;
                    }

                }
                catch
                {
                    trans.Rollback();
                    cn.Close();
                    flag = false;
                }
            }


            return flag;
        }
        catch
        {
            return flag;
        }
    }
    public bool RollBack_EMB(int PackageEMBApproval_Id, int EMBMaster_Id, int person_Id)
    {
        bool flag = false;
        try
        {


            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(ConStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                SqlTransaction trans = cn.BeginTransaction();
                try
                {
                    string strQueryInvoice = @"select PackageInvoiceEMBMasterLink_Id 
                                                from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_EMBMaster_Id = '" + EMBMaster_Id + "' and PackageInvoiceEMBMasterLink_Status = 1";
                    try
                    {
                        ds = ExecuteSelectQuery(strQueryInvoice);
                    }
                    catch
                    {
                        ds = null;
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        cn.Close();
                        flag = false;
                    }
                    else
                    {
                        string strQuery = "";
                        strQuery += " set dateformat dmy; Update  tbl_PackageEMBApproval set   PackageEMBApproval_Status = 0,PackageEMBApproval_ModifiedBy='" + person_Id + "',PackageEMBApproval_ModifiedOn=getdate() where PackageEMBApproval_Id = '" + PackageEMBApproval_Id + "' ";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                        trans.Commit();
                        cn.Close();
                        flag = true;


                    }

                }
                catch
                {
                    trans.Rollback();
                    cn.Close();
                    flag = false;
                }
            }


            return flag;
        }
        catch
        {
            return flag;
        }
    }
    #endregion

    #region EMB Marking
    public bool Update_tbl_PackageEMB_Mark(List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li, int Organisation_Id, int Designation_Id)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PackageEMBApproval_Li.Count; i++)
                {
                    tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = obj_tbl_PackageEMBApproval_Li[i];
                    int _Loop = get_Loop("EMB", Organisation_Id, Designation_Id, trans, cn);
                    if (obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 1 || obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 4)
                    {
                        ds = get_ProcessConfigMaster_Next("EMB", Organisation_Id, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_Prev("EMB", Organisation_Id, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                        if (AllClasses.CheckDataSet(ds))
                        {
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 0;
                            Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);

                            int PackageInvoice_Id = generate_EMB_Invoice(obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, trans, cn);
                            tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = obj_tbl_PackageEMBApproval.PackageEMBApproval_AddedBy;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = obj_tbl_PackageEMBApproval.PackageEMBApproval_Date;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = obj_tbl_PackageEMBApproval.PackageEMBApproval_Package_Id;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 1;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 4;
                            obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
                            Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                        }
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    public bool Update_CombinedInvoiceGenerate(List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li, int Organisation_Id, int Designation_Id)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PackageEMBApproval_Li.Count; i++)
                {
                    tbl_PackageEMBApproval obj_tbl_PackageEMBApproval = obj_tbl_PackageEMBApproval_Li[i];
                    int _Loop = get_Loop("EMB", Organisation_Id, Designation_Id, trans, cn);
                    if (obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 1 || obj_tbl_PackageEMBApproval.PackageEMBApproval_Status_Id == 4)
                    {
                        ds = get_ProcessConfigMaster_Next("EMB", Organisation_Id, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_Prev("EMB", Organisation_Id, Designation_Id, obj_tbl_PackageEMBApproval.PackageEMBApproval_PackageEMBMaster_Id, _Loop, trans, cn);
                    }
                    if (AllClasses.CheckDataSet(ds))
                    {
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                        obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = Convert.ToInt32(ds.Tables[0].Rows[0]["rr"].ToString());
                        Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);
                    }
                    else
                    {
                        ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                        if (AllClasses.CheckDataSet(ds))
                        {
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Designation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Next_Organisation_Id = 0;
                            obj_tbl_PackageEMBApproval.PackageEMBApproval_Step_Count = 0;
                            Insert_tbl_PackageEMBApproval(obj_tbl_PackageEMBApproval, trans, cn);

                        }
                    }
                }
                ds = get_ProcessConfigMaster_First("Invoice", trans, cn);
                if (AllClasses.CheckDataSet(ds))
                {
                    string PackageEMB_Master_Id = "0";
                    for (int i = 0; i < obj_tbl_PackageEMBApproval_Li.Count; i++)
                    {
                        PackageEMB_Master_Id = PackageEMB_Master_Id + "," + obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_PackageEMBMaster_Id.ToString();
                    }
                    int PackageInvoice_Id = generate_EMB_Invoice_Combined(PackageEMB_Master_Id, obj_tbl_PackageEMBApproval_Li, trans, cn);

                    tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval = new tbl_PackageInvoiceApproval();
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy = obj_tbl_PackageEMBApproval_Li[0].PackageEMBApproval_AddedBy;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date = obj_tbl_PackageEMBApproval_Li[0].PackageEMBApproval_Date;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id = obj_tbl_PackageEMBApproval_Li[0].PackageEMBApproval_Package_Id;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Designation_Id"].ToString());
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_OrgId"].ToString());
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count = 1;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 4;
                    obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status = 1;
                    Insert_tbl_PackageInvoiceApproval(obj_tbl_PackageInvoiceApproval, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private int generate_EMB_Invoice(int PackageEMBMaster_Id, SqlTransaction trans, SqlConnection cn)
    {
        int PackageInvoice_Id = 0;
        DataSet ds = new DataSet();
        string strQuery = @"set dateformat dmy; 
                            select 
	                            PackageEMB_Id,
                                PackageEMB_PackageEMB_Master_Id,
                                PackageEMB_Master_Date = convert(char(10), PackageEMB_Master_Date, 103), 
                                PackageEMB_Master_VoucherNo,
                                PackageEMB_Master_RA_BillNo,
                                PackageBOQ_Id,
	                            PackageEMB_Package_Id,
	                            PackageEMB_Specification,
	                            PackageEMB_Unit_Id,
                                isnull(Unit_Length_Applicable, 0) Unit_Length_Applicable,
                                isnull(Unit_Bredth_Applicable, 0) Unit_Bredth_Applicable,
                                isnull(Unit_Height_Applicable, 0) Unit_Height_Applicable,
	                            PackageEMB_Qty,
                                PackageBOQ_Qty,
                                PackageEMB_QtyExtra,
	                            PackageEMB_Length,
	                            PackageEMB_Breadth,
	                            PackageEMB_Height,
	                            PackageEMB_Contents,
	                            PackageEMB_AddedBy,
	                            PackageEMB_AddedOn,
	                            PackageEMB_Status, 
	                            PackageEMB_PercentageToBeReleased = isnull(PackageEMB_PercentageToBeReleased, 100), 
                                tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Date, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_No, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Person, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Comments, 
                                PackageBOQ_RateEstimated,
                                PackageBOQ_AmountEstimated,
                                PackageBOQ_RateQuoted,
                                PackageBOQ_AmountQuoted, 
                                PackageEMB_Is_Billed = isnull(PackageInvoiceItem_Id, 0), 
                                tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments, 
                                PackageInvoiceItem_Id = isnull(tbl_PackageInvoiceItem.PackageInvoiceItem_Id, 0), 
                                PackageBOQ_QtyPaid = isnull(PackageBOQ_QtyPaid, 0) + isnull(PackageEMB_Qty_1, 0),
                                PackageEMB_GSTType
                            from tbl_PackageEMB
                            left join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            join tbl_PackageBOQ on PackageBOQ_Id = PackageEMB_PackageBOQ_Id and PackageBOQ_Status = 1
                            left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id, PackageEMB_Approval_Person = Person_Name from tbl_PackageEMB_Approval join tbl_PersonDetail on Person_Id = PackageEMB_Approval_Person_Id where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
                            left join 
						    (
							    select ROW_NUMBER() over (partition by PackageInvoiceItem_PackageEMB_Id order by PackageInvoiceItem_Id desc) rrrI, PackageInvoiceItem_Id, PackageInvoiceItem_PackageEMB_Id, PackageInvoiceItem_Invoice_Id, PackageInvoiceItem_Total_Qty_BOQ, PackageInvoiceItem_Total_Qty_Billed, PackageInvoiceItem_Total_Qty, PackageInvoiceItem_RateEstimated, PackageInvoiceItem_RateQuoted, PackageInvoiceItem_AmountQuoted, PackageInvoiceItem_Remarks from tbl_PackageInvoiceItem where PackageInvoiceItem_Status = 1
						    ) tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id = PackageEMB_Id and rrrI = 1
                            left join 
                            (
                                select 
                                    ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrrA, 
                                    PackageBOQ_Approval_Id, 
                                    PackageBOQ_Approval_PackageBOQ_Id, 
                                    PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), 
                                    PackageBOQ_Approval_No, 
                                    PackageBOQ_Approval_Comments, 
                                    PackageBOQ_Approval_Approved_Qty, 
                                    PackageBOQ_DocumentPath, 
                                    PackageBOQ_Approval_Person_Id, 
                                    PackageBOQ_Approval_Person = Person_Name 
                                from tbl_PackageBOQ_Approval 
                                join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id 
                                where PackageBOQ_Approval_Status = 1
                            ) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrrA = 1
                            left join tbl_Unit on Unit_Id = PackageEMB_Unit_Id
                            left join 
					        (
						        select 
	                                PackageEMB_PackageBOQ_Id, PackageEMB_Qty_1 = sum(isnull(PackageEMB_Qty, 0))
                                from tbl_PackageEMB
                                join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                                where PackageEMB_Status = 1
                                group by PackageEMB_PackageBOQ_Id
					        ) Qty_Paid_Till_Date on Qty_Paid_Till_Date.PackageEMB_PackageBOQ_Id = PackageBOQ_Id     
                            where PackageEMB_Status = 1 and isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 0 and PackageEMB_PackageEMB_Master_Id = '" + PackageEMBMaster_Id + "' Order by PackageEMB_PackageBOQ_OrderNo ";
        ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        if (AllClasses.CheckDataSet(ds))
        {
            tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
            obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageInvoice.PackageInvoice_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["PackageEMB_Package_Id"].ToString());
            obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
            obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = ds.Tables[0].Rows[0]["PackageEMB_Master_RA_BillNo"].ToString();

            List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int is_Approved = 0;
                try
                {
                    is_Approved = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Approval_Id"].ToString());
                }
                catch
                {
                    is_Approved = 0;
                }
                int is_Billed = 0;
                try
                {
                    is_Billed = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Is_Billed"].ToString());
                }
                catch
                {
                    is_Billed = 0;
                }

                if (is_Approved > 0 && is_Billed == 0)
                {
                    tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageBOQ_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageEMB_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_GSTType = ds.Tables[0].Rows[i]["PackageEMB_GSTType"].ToString();
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_Qty"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_QtyExtra"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_PercentageToBeReleased"].ToString());
                    }
                    catch
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = 100;
                    }
                    if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased == 0)
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = 100;
                    }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_QtyPaid"].ToString());
                    }
                    catch
                    {

                    }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_RateEstimated"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_RateQuoted"].ToString());
                    }
                    catch
                    { }
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status = 1;
                    DataSet dsTax = new DataSet();
                    string strQueryTax = @" select PackageEMB_Tax_Deduction_Id,PackageEMB_Tax_Value 
                                            from tbl_PackageEMB_Tax where PackageEMB_Tax_Package_Id='" + ds.Tables[0].Rows[i]["PackageEMB_Id"].ToString() + "' and PackageEMB_Tax_Status=1 ";
                    dsTax = ExecuteSelectQuerywithTransaction(cn, strQueryTax, trans);
                    if (AllClasses.CheckDataSet(dsTax))
                    {
                        List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
                        for (int k = 0; k < dsTax.Tables[0].Rows.Count; k++)
                        {
                            tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                            try
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = Convert.ToInt32(dsTax.Tables[0].Rows[i]["PackageEMB_Tax_Deduction_Id"].ToString());
                            }
                            catch
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = 0;
                            }
                            try
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = Convert.ToDecimal(dsTax.Tables[0].Rows[i]["PackageEMB_Tax_Value"].ToString());
                            }
                            catch
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = 0;
                            }
                            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status = 1;
                            if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                            {
                                obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                            }
                            if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                            {
                                obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                            }
                        }
                    }

                    obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);
                }
            }
            int ProjectWorkPkg_Id = obj_tbl_PackageInvoice.PackageInvoice_Package_Id;

            if (string.IsNullOrEmpty(obj_tbl_PackageInvoice.PackageInvoice_VoucherNo))
            {
                obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = get_tbl_TransactionNos(VoucherTypes.Invoice, obj_tbl_PackageInvoice.PackageInvoice_Package_Id, trans, cn);
            }
            obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id = PackageEMBMaster_Id;
            obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);
            PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;

            for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
            {
                obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                // Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id = Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                if (obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li != null)
                {
                    for (int k = 0; k < obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li.Count; k++)
                    {
                        obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k].PackageInvoiceItem_Tax_PackageInvoiceItem_Id = obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id;
                        Insert_tbl_PackageInvoiceItem_Tax(obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k], trans, cn); ;
                    }
                }
            }

        }
        return PackageInvoice_Id;
    }
    private int generate_EMB_Invoice_Combined(string PackageEMBMaster_Id, List<tbl_PackageEMBApproval> obj_tbl_PackageEMBApproval_Li, SqlTransaction trans, SqlConnection cn)
    {
        int PackageInvoice_Id = 0;
        DataSet ds = new DataSet();
        string strQuery = @"set dateformat dmy; 
                            select 
	                            PackageEMB_Id,
                                PackageEMB_PackageEMB_Master_Id,
                                PackageEMB_Master_Date = convert(char(10), PackageEMB_Master_Date, 103), 
                                PackageEMB_Master_VoucherNo,
                                PackageEMB_Master_RA_BillNo,
                                PackageBOQ_Id,
	                            PackageEMB_Package_Id,
	                            PackageEMB_Specification,
	                            PackageEMB_Unit_Id,
                                isnull(Unit_Length_Applicable, 0) Unit_Length_Applicable,
                                isnull(Unit_Bredth_Applicable, 0) Unit_Bredth_Applicable,
                                isnull(Unit_Height_Applicable, 0) Unit_Height_Applicable,
	                            PackageEMB_Qty,
                                PackageBOQ_Qty,
                                PackageEMB_QtyExtra,
	                            PackageEMB_Length,
	                            PackageEMB_Breadth,
	                            PackageEMB_Height,
	                            PackageEMB_Contents,
	                            PackageEMB_AddedBy,
	                            PackageEMB_AddedOn,
	                            PackageEMB_Status, 
	                            PackageEMB_PercentageToBeReleased = isnull(PackageEMB_PercentageToBeReleased, 100), 
                                tbl_PackageEMB_Approval.PackageEMB_Approval_Id, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Date, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_No, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Person, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Approved_Qty, 
						        tbl_PackageEMB_Approval.PackageEMB_Approval_Comments, 
                                PackageBOQ_RateEstimated,
                                PackageBOQ_AmountEstimated,
                                PackageBOQ_RateQuoted,
                                PackageBOQ_AmountQuoted, 
                                PackageEMB_Is_Billed = isnull(PackageInvoiceItem_Id, 0), 
                                tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_No, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty, 
						        tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments, 
                                PackageInvoiceItem_Id = isnull(tbl_PackageInvoiceItem.PackageInvoiceItem_Id, 0), 
                                PackageBOQ_QtyPaid = isnull(PackageBOQ_QtyPaid, 0) + isnull(PackageEMB_Qty_1, 0),
                                PackageEMB_GSTType
                            from tbl_PackageEMB
                            left join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                            join tbl_PackageBOQ on PackageBOQ_Id = PackageEMB_PackageBOQ_Id and PackageBOQ_Status = 1
                            left join (select ROW_NUMBER() over (partition by PackageEMB_Approval_PackageEMB_Id order by PackageEMB_Approval_Id desc) rrr, PackageEMB_Approval_Id, PackageEMB_Approval_PackageEMB_Id, PackageEMB_Approval_Date = convert(char(10), PackageEMB_Approval_Date, 103), PackageEMB_Approval_No, PackageEMB_Approval_Comments, PackageEMB_Approval_Approved_Qty, PackageEMB_DocumentPath, PackageEMB_Approval_Person_Id, PackageEMB_Approval_Person = Person_Name from tbl_PackageEMB_Approval join tbl_PersonDetail on Person_Id = PackageEMB_Approval_Person_Id where PackageEMB_Approval_Status = 1) tbl_PackageEMB_Approval on PackageEMB_Approval_PackageEMB_Id = PackageEMB_Id and rrr = 1
                            left join 
						    (
							    select ROW_NUMBER() over (partition by PackageInvoiceItem_PackageEMB_Id order by PackageInvoiceItem_Id desc) rrrI, PackageInvoiceItem_Id, PackageInvoiceItem_PackageEMB_Id, PackageInvoiceItem_Invoice_Id, PackageInvoiceItem_Total_Qty_BOQ, PackageInvoiceItem_Total_Qty_Billed, PackageInvoiceItem_Total_Qty, PackageInvoiceItem_RateEstimated, PackageInvoiceItem_RateQuoted, PackageInvoiceItem_AmountQuoted, PackageInvoiceItem_Remarks from tbl_PackageInvoiceItem where PackageInvoiceItem_Status = 1
						    ) tbl_PackageInvoiceItem on PackageInvoiceItem_PackageEMB_Id = PackageEMB_Id and rrrI = 1
                            left join 
                            (
                                select 
                                    ROW_NUMBER() over (partition by PackageBOQ_Approval_PackageBOQ_Id order by PackageBOQ_Approval_Id desc) rrrA, 
                                    PackageBOQ_Approval_Id, 
                                    PackageBOQ_Approval_PackageBOQ_Id, 
                                    PackageBOQ_Approval_Date = convert(char(10), PackageBOQ_Approval_Date, 103), 
                                    PackageBOQ_Approval_No, 
                                    PackageBOQ_Approval_Comments, 
                                    PackageBOQ_Approval_Approved_Qty, 
                                    PackageBOQ_DocumentPath, 
                                    PackageBOQ_Approval_Person_Id, 
                                    PackageBOQ_Approval_Person = Person_Name 
                                from tbl_PackageBOQ_Approval 
                                join tbl_PersonDetail on Person_Id = PackageBOQ_Approval_Person_Id 
                                where PackageBOQ_Approval_Status = 1
                            ) tbl_PackageBOQ_Approval on PackageBOQ_Approval_PackageBOQ_Id = PackageBOQ_Id and rrrA = 1
                            left join tbl_Unit on Unit_Id = PackageEMB_Unit_Id
                            left join 
					        (
						        select 
	                                PackageEMB_PackageBOQ_Id, PackageEMB_Qty_1 = sum(isnull(PackageEMB_Qty, 0))
                                from tbl_PackageEMB
                                join tbl_PackageEMB_Master on PackageEMB_Master_Id = PackageEMB_PackageEMB_Master_Id
                                where PackageEMB_Status = 1
                                group by PackageEMB_PackageBOQ_Id
					        ) Qty_Paid_Till_Date on Qty_Paid_Till_Date.PackageEMB_PackageBOQ_Id = PackageBOQ_Id     
                            where PackageEMB_Status = 1 and isnull(tbl_PackageBOQ_Approval.PackageBOQ_Approval_Id, 0) > 0 and PackageEMB_PackageEMB_Master_Id in (" + PackageEMBMaster_Id + ") Order by PackageEMB_PackageBOQ_OrderNo ";
        ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        if (AllClasses.CheckDataSet(ds))
        {
            tbl_PackageInvoice obj_tbl_PackageInvoice = new tbl_PackageInvoice();
            obj_tbl_PackageInvoice.PackageInvoice_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
            obj_tbl_PackageInvoice.PackageInvoice_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            obj_tbl_PackageInvoice.PackageInvoice_Package_Id = Convert.ToInt32(ds.Tables[0].Rows[0]["PackageEMB_Package_Id"].ToString());
            obj_tbl_PackageInvoice.PackageInvoice_Status = 1;
            obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = ds.Tables[0].Rows[0]["PackageEMB_Master_RA_BillNo"].ToString();

            List<tbl_PackageInvoiceEMBMasterLink> obj_tbl_PackageInvoiceEMBMasterLink_Li = new List<tbl_PackageInvoiceEMBMasterLink>();
            for (int i = 0; i < obj_tbl_PackageEMBApproval_Li.Count; i++)
            {
                tbl_PackageInvoiceEMBMasterLink obj_tbl_PackageInvoiceEMBMasterLink = new tbl_PackageInvoiceEMBMasterLink();
                obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_EMBMaster_Id = Convert.ToInt32(obj_tbl_PackageEMBApproval_Li[i].PackageEMBApproval_PackageEMBMaster_Id);
                obj_tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Status = 1;
                obj_tbl_PackageInvoiceEMBMasterLink_Li.Add(obj_tbl_PackageInvoiceEMBMasterLink);
            }

            List<tbl_PackageInvoiceItem> obj_tbl_PackageInvoiceItem_Li = new List<tbl_PackageInvoiceItem>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int is_Approved = 0;
                try
                {
                    is_Approved = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Approval_Id"].ToString());
                }
                catch
                {
                    is_Approved = 0;
                }
                int is_Billed = 0;
                try
                {
                    is_Billed = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Is_Billed"].ToString());
                }
                catch
                {
                    is_Billed = 0;
                }

                if (is_Approved > 0 && is_Billed == 0)
                {
                    tbl_PackageInvoiceItem obj_tbl_PackageInvoiceItem = new tbl_PackageInvoiceItem();
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_BOQ_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageBOQ_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PackageEMB_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PackageEMB_Id"].ToString());
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_GSTType = ds.Tables[0].Rows[i]["PackageEMB_GSTType"].ToString();
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty_BOQ = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_Qty"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_QtyExtra = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_QtyExtra"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageEMB_PercentageToBeReleased"].ToString());
                    }
                    catch
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = 100;
                    }
                    if (obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased == 0)
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_PercentageToBeReleased = 100;
                    }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Total_Qty = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_QtyPaid"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateEstimated = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_RateEstimated"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        obj_tbl_PackageInvoiceItem.PackageInvoiceItem_RateQuoted = Convert.ToDecimal(ds.Tables[0].Rows[i]["PackageBOQ_RateQuoted"].ToString());
                    }
                    catch
                    { }
                    obj_tbl_PackageInvoiceItem.PackageInvoiceItem_Status = 1;
                    DataSet dsTax = new DataSet();
                    string strQueryTax = @" select PackageEMB_Tax_Deduction_Id,PackageEMB_Tax_Value 
                                            from tbl_PackageEMB_Tax where PackageEMB_Tax_Package_Id='" + ds.Tables[0].Rows[i]["PackageEMB_Id"].ToString() + "' and PackageEMB_Tax_Status=1 ";
                    dsTax = ExecuteSelectQuerywithTransaction(cn, strQueryTax, trans);
                    if (AllClasses.CheckDataSet(dsTax))
                    {
                        List<tbl_PackageInvoiceItem_Tax> obj_tbl_PackageInvoiceItem_Tax_Li = new List<tbl_PackageInvoiceItem_Tax>();
                        for (int k = 0; k < dsTax.Tables[0].Rows.Count; k++)
                        {
                            tbl_PackageInvoiceItem_Tax obj_tbl_PackageInvoiceItem_Tax = new tbl_PackageInvoiceItem_Tax();
                            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                            try
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = Convert.ToInt32(dsTax.Tables[0].Rows[i]["PackageEMB_Tax_Deduction_Id"].ToString());
                            }
                            catch
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id = 0;
                            }
                            try
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = Convert.ToDecimal(dsTax.Tables[0].Rows[i]["PackageEMB_Tax_Value"].ToString());
                            }
                            catch
                            {
                                obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Value = 0;
                            }
                            obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Status = 1;
                            if (obj_tbl_PackageInvoiceItem_Tax.PackageInvoiceItem_Tax_Deduction_Id > 0)
                            {
                                obj_tbl_PackageInvoiceItem_Tax_Li.Add(obj_tbl_PackageInvoiceItem_Tax);
                            }
                            if (obj_tbl_PackageInvoiceItem_Tax_Li != null)
                            {
                                obj_tbl_PackageInvoiceItem.obj_tbl_PackageInvoiceItem_Tax_Li = obj_tbl_PackageInvoiceItem_Tax_Li;
                            }
                        }
                    }

                    obj_tbl_PackageInvoiceItem_Li.Add(obj_tbl_PackageInvoiceItem);
                }
            }
            int ProjectWorkPkg_Id = obj_tbl_PackageInvoice.PackageInvoice_Package_Id;

            if (string.IsNullOrEmpty(obj_tbl_PackageInvoice.PackageInvoice_VoucherNo))
            {
                obj_tbl_PackageInvoice.PackageInvoice_VoucherNo = get_tbl_TransactionNos(VoucherTypes.Invoice, obj_tbl_PackageInvoice.PackageInvoice_Package_Id, trans, cn);
            }
            //obj_tbl_PackageInvoice.PackageInvoice_PackageEMBMaster_Id = PackageEMBMaster_Id;
            if (obj_tbl_PackageInvoiceItem_Li.Count > 0)
            {
                obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);
                PackageInvoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;

                for (int i = 0; i < obj_tbl_PackageInvoiceItem_Li.Count; i++)
                {
                    obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    // Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                    obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id = Insert_tbl_PackageInvoiceItem(obj_tbl_PackageInvoiceItem_Li[i], trans, cn);
                    if (obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li != null)
                    {
                        for (int k = 0; k < obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li.Count; k++)
                        {
                            obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k].PackageInvoiceItem_Tax_PackageInvoiceItem_Id = obj_tbl_PackageInvoiceItem_Li[i].PackageInvoiceItem_Id;
                            Insert_tbl_PackageInvoiceItem_Tax(obj_tbl_PackageInvoiceItem_Li[i].obj_tbl_PackageInvoiceItem_Tax_Li[k], trans, cn); ;
                        }
                    }
                }

                for (int i = 0; i < obj_tbl_PackageInvoiceEMBMasterLink_Li.Count; i++)
                {
                    obj_tbl_PackageInvoiceEMBMasterLink_Li[i].PackageInvoiceEMBMasterLink_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    Insert_tbl_PackageInvoiceEMBMasterLink(obj_tbl_PackageInvoiceEMBMasterLink_Li[i], trans, cn);
                }
            }

        }
        return PackageInvoice_Id;
    }
    #endregion

    #region Financial Transaction
    private void Insert_tbl_FinancialTrans(tbl_FinancialTrans obj_tbl_FinancialTrans, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        if (obj_tbl_FinancialTrans.FinancialTrans_Package_Id > 0 && obj_tbl_FinancialTrans.FinancialTrans_Work_Id > 0)
        {
            strQuery = " set dateformat dmy;insert into tbl_FinancialTrans ( [FinancialTrans_AddedBy],[FinancialTrans_AddedOn],[FinancialTrans_Comments],[FinancialTrans_Date],[FinancialTrans_EntryType],[FinancialTrans_FinancialYear_Id],[FinancialTrans_Invoice_Id],[FinancialTrans_Status], [FinancialTrans_Amount], [FinancialTrans_TransAmount],[FinancialTrans_TransType],FinancialTrans_Work_Id,FinancialTrans_Package_Id,FinancialTrans_FilePath1,FinancialTrans_GO_Date,FinancialTrans_GO_Number) values ('" + obj_tbl_FinancialTrans.FinancialTrans_AddedBy + "', getdate(), N'" + obj_tbl_FinancialTrans.FinancialTrans_Comments + "', convert(date, '" + obj_tbl_FinancialTrans.FinancialTrans_Date + "', 103), '" + obj_tbl_FinancialTrans.FinancialTrans_EntryType + "','" + obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Invoice_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Status + "', '" + obj_tbl_FinancialTrans.FinancialTrans_Amount + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransAmount + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransType + "','" + obj_tbl_FinancialTrans.FinancialTrans_Work_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Package_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_FilePath1 + "',convert(date, '" + obj_tbl_FinancialTrans.FinancialTrans_GO_Date + "', 103),'" + obj_tbl_FinancialTrans.FinancialTrans_GO_Number + "');Select @@Identity";
        }
        else
        {
            strQuery = " set dateformat dmy;insert into tbl_FinancialTrans ( [FinancialTrans_AddedBy],[FinancialTrans_AddedOn],[FinancialTrans_Comments],[FinancialTrans_Date],[FinancialTrans_EntryType],[FinancialTrans_FinancialYear_Id],[FinancialTrans_Invoice_Id],[FinancialTrans_Status], [FinancialTrans_Amount], [FinancialTrans_TransAmount],[FinancialTrans_TransType] ) values ('" + obj_tbl_FinancialTrans.FinancialTrans_AddedBy + "', getdate(), N'" + obj_tbl_FinancialTrans.FinancialTrans_Comments + "', convert(date, '" + obj_tbl_FinancialTrans.FinancialTrans_Date + "', 103), '" + obj_tbl_FinancialTrans.FinancialTrans_EntryType + "','" + obj_tbl_FinancialTrans.FinancialTrans_FinancialYear_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Invoice_Id + "','" + obj_tbl_FinancialTrans.FinancialTrans_Status + "', '" + obj_tbl_FinancialTrans.FinancialTrans_Amount + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransAmount + "','" + obj_tbl_FinancialTrans.FinancialTrans_TransType + "');Select @@Identity";
        }

        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    #endregion

    #region BOQ Import
    public DataSet Insert_BOQ_Data_And_Map(List<string> obj_Data_Colums, DataTable dt_BOQ_Data, int Added_By, string Project_Code_Column, string Package_Code_Column)
    {
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = @"IF OBJECT_ID('tbl_BOQ_Temp_Data', 'U') IS NOT NULL
                                    DROP TABLE tbl_BOQ_Temp_Data";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = @"CREATE TABLE [dbo].[tbl_BOQ_Temp_Data](
	                        [BOQ_Temp_Data_Id] [int] IDENTITY(1,1) NOT NULL,
	                        [Person_Id] [int] NULL,
	                        [Project_Id] [int] NULL,
	                        [Package_Id] [int] NULL,
	                        [Unit_Id] [int] NULL,
                            Additional_Colums
                         CONSTRAINT [PK_BOQ_Temp_Data] PRIMARY KEY CLUSTERED 
                        (
	                        [BOQ_Temp_Data_Id] ASC
                        ) ON [PRIMARY]
                        ) ON [PRIMARY]";
                string sqlTmp = Environment.NewLine;
                for (int i = 1; i < obj_Data_Colums.Count; i++)
                {
                    sqlTmp += "[" + obj_Data_Colums[i] + "] [nvarchar](max) NULL, ";
                    sqlTmp += Environment.NewLine;
                }
                sql = sql.Replace("Additional_Colums", sqlTmp);
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                SqlBulkCopy objBulk = null;
                objBulk = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, trans);
                objBulk.DestinationTableName = "tbl_BOQ_Temp_Data";
                objBulk.BatchSize = 1000;
                objBulk.BulkCopyTimeout = 1000;
                for (int i = 1; i < obj_Data_Colums.Count; i++)
                {
                    objBulk.ColumnMappings.Add(obj_Data_Colums[i], obj_Data_Colums[i]);
                }

                objBulk.WriteToServer(dt_BOQ_Data);
                objBulk.Close();

                sql = @"with cte1 as (
                        select 
                            Project_Id_U = case when isnull(ProjectWork_Id, 0) = 0 then Project_Id else ProjectWork_Id end,
                            tbl_BOQ_Temp_Data.Project_Id 
                        from (select ROW_NUMBER() over (partition by BOQ_Temp_Data_Id order by BOQ_Temp_Data_Id desc) rrr, * 
                        from tbl_BOQ_Temp_Data
                        left join tbl_ProjectWork on ProjectWork_ProjectCode = [" + Project_Code_Column + "] and ProjectWork_Status = 1) tbl_BOQ_Temp_Data where rrr = 1) update cte1 set Project_Id = Project_Id_U";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = @"with cte1 as (
                        select 
                            Package_Id_U = case when isnull(ProjectWorkPkg_Id, 0) = 0 then Package_Id else ProjectWorkPkg_Id end,
                            tbl_BOQ_Temp_Data.Package_Id 
                        from (select ROW_NUMBER() over (partition by BOQ_Temp_Data_Id order by BOQ_Temp_Data_Id desc) rrr, * 
                        from tbl_BOQ_Temp_Data
                        left join tbl_ProjectWorkPkg on ProjectWorkPkg_Code = [" + Package_Code_Column + "] join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1) tbl_BOQ_Temp_Data where rrr = 1) update cte1 set Package_Id = Package_Id_U";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = @"with cte1 as (
                        select 
                            Unit_Id_U = case when isnull(Unit_Id_M, 0) = 0 then tbl_BOQ_Temp_Data.Unit_Id else Unit_Id_M end,
                            tbl_BOQ_Temp_Data.Unit_Id 
                        from (select ROW_NUMBER() over (partition by BOQ_Temp_Data_Id order by BOQ_Temp_Data_Id desc) rrr, tbl_BOQ_Temp_Data.*, Unit_Id_M = tbl_Unit.Unit_Id 
                        from tbl_BOQ_Temp_Data
                        left join tbl_Unit on Unit_Name = ltrim(rtrim(replace([Unit], '.', ''))) and Unit_Status = 1) tbl_BOQ_Temp_Data where rrr = 1) update cte1 set Unit_Id = Unit_Id_U";
                ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = @"select 
                            Person_Id,
                            Project_Id, 
                            Package_Id, 
                            Unit_Id, 
                            Additional_Colums_Select
                        from tbl_BOQ_Temp_Data where 1 = 1 and (isnull(Project_Id, 0) = 0 or isnull(Package_Id, 0) = 0)";

                sqlTmp = Environment.NewLine;
                for (int i = 1; i < obj_Data_Colums.Count; i++)
                {
                    sqlTmp += "tbl_BOQ_Temp_Data.[" + obj_Data_Colums[i] + "], ";
                    sqlTmp += Environment.NewLine;
                }
                sql = sql.Replace("Additional_Colums_Select", sqlTmp + "Status = 1 ");
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                trans.Commit();
                cn.Close();
            }
            catch (Exception ee)
            {
                trans.Rollback();
                cn.Close();
                ds = null;
            }
        }
        return ds;
    }

    public bool Insert_BOQ_Data(List<string> obj_Data_Colums, ref string msg)
    {
        DataSet ds = new DataSet();
        bool flag = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            int i = 0;
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = @"select 
                                Person_Id,
                                Project_Id, 
                                Package_Id, 
                                Unit_Id, 
                                Additional_Colums_Select
                            from tbl_BOQ_Temp_Data where 1 = 1 and isnull(Project_Id, 0) > 0 and isnull(Package_Id, 0) > 0 ";

                string sqlTmp = Environment.NewLine;
                for (i = 1; i < obj_Data_Colums.Count; i++)
                {
                    sqlTmp += "tbl_BOQ_Temp_Data.[" + obj_Data_Colums[i] + "], ";
                    sqlTmp += Environment.NewLine;
                }
                sql = sql.Replace("Additional_Colums_Select", sqlTmp + "Status = 1 ");
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                DataSet ds1 = new DataSet();
                string sqlquery = "select top 1 PackageBOQ_OrderNo from tbl_PackageBOQ order by PackageBOQ_OrderNo desc ";
                ds1 = ExecuteSelectQuerywithTransaction(cn, sqlquery, trans);
                int PackageBOQ_OrderNo = 0;

                if (AllClasses.CheckDataSet(ds1))
                {
                    PackageBOQ_OrderNo = Convert.ToInt32(ds1.Tables[0].Rows[0]["PackageBOQ_OrderNo"].ToString().Trim()) + 1;
                }
                else
                {
                    PackageBOQ_OrderNo = 1;
                }

                if (AllClasses.CheckDataSet(ds))
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tbl_PackageBOQ obj_tbl_PackageBOQ = new tbl_PackageBOQ();
                        obj_tbl_PackageBOQ.PackageBOQ_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                        obj_tbl_PackageBOQ.PackageBOQ_OrderNo = PackageBOQ_OrderNo + i;
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated = decimal.Parse(ds.Tables[0].Rows[i]["Tender Amount"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_AmountEstimated = 0;
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted = decimal.Parse(ds.Tables[0].Rows[i]["Contractor Agreed Amount"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_AmountQuoted = 0;
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_RateEstimated = decimal.Parse(ds.Tables[0].Rows[i]["Tender Rate"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_RateEstimated = 0;
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_RateQuoted = decimal.Parse(ds.Tables[0].Rows[i]["Contractor Agreed Rate"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_RateQuoted = 0;
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_Unit_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Unit_Id"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_Unit_Id = 0;
                        }
                        if (obj_tbl_PackageBOQ.PackageBOQ_Unit_Id == 0 && ds.Tables[0].Rows[i]["Unit"].ToString().Trim() != "")
                        {
                            tbl_Unit obj_tbl_Unit = new tbl_Unit();
                            obj_tbl_Unit.Unit_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                            obj_tbl_Unit.Unit_Name = ds.Tables[0].Rows[i]["Unit"].ToString().Trim();
                            obj_tbl_Unit.Unit_Status = 1;
                            DataSet dsU = CheckDuplicacyUnit(obj_tbl_Unit.Unit_Name, "0", trans, cn);
                            string Unit_Id = "0";
                            if (AllClasses.CheckDataSet(dsU))
                            {
                                Unit_Id = dsU.Tables[0].Rows[0]["Unit_Id"].ToString();
                            }
                            else
                            {
                                Unit_Id = Insert_tbl_Unit(obj_tbl_Unit, trans, cn);
                            }

                            obj_tbl_PackageBOQ.PackageBOQ_Unit_Id = Convert.ToInt32(Unit_Id);
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_Qty = decimal.Parse(ds.Tables[0].Rows[i]["Quantity"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_Qty = 0;
                        }
                        try
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_QtyPaid = decimal.Parse(ds.Tables[0].Rows[i]["Quantity Paid Till Date"].ToString().Trim());
                        }
                        catch
                        {
                            obj_tbl_PackageBOQ.PackageBOQ_QtyPaid = 0;
                        }
                        obj_tbl_PackageBOQ.PackageBOQ_Id = 0;
                        obj_tbl_PackageBOQ.PackageBOQ_Package_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Package_Id"].ToString().Trim());
                        obj_tbl_PackageBOQ.PackageBOQ_Specification = ds.Tables[0].Rows[i]["BOQ Description"].ToString().Trim().Replace("'", "") + Environment.NewLine + ds.Tables[0].Rows[i]["Specification"].ToString().Trim().Replace("'", "");
                        obj_tbl_PackageBOQ.PackageBOQ_Is_Approved = 1;
                        obj_tbl_PackageBOQ.PackageBOQ_Status = 1;
                        obj_tbl_PackageBOQ.PackageBOQ_Id = Insert_tbl_PackageBOQ(obj_tbl_PackageBOQ, trans, cn);

                        tbl_PackageBOQ_Approval obj_tbl_PackageBOQ_Approval = new tbl_PackageBOQ_Approval();
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Approved_Qty = obj_tbl_PackageBOQ.PackageBOQ_Qty;
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Comments = "";
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Date = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_No = "";
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_PackageBOQ_Id = obj_tbl_PackageBOQ.PackageBOQ_Id;
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Person_Id = Convert.ToInt32(Session["Person_Id"].ToString());
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_Approval_Status = 1;
                        obj_tbl_PackageBOQ_Approval.PackageBOQ_DocumentPath = "";

                        Insert_tbl_PackageBOQ_Approval(obj_tbl_PackageBOQ_Approval, trans, cn);
                    }
                }
                msg = "";
                flag = true;
                trans.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                flag = false;
                trans.Rollback();
                cn.Close();
            }
        }
        return flag;
    }
    #endregion

    #region Process Configuration
    public DataSet get_tbl_ProcessConfigMaster(string Process_Name)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Process_Name == "Invoice")
        {
            strQuery = @"set dateformat dmy; 
                        select 
	                        ProcessConfigMaster_Id,
	                        ProcessConfigMaster_Process_Name,
	                        ProcessConfigMaster_OrgId,
	                        ProcessConfigMaster_Department_Id,
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1=isnull(ProcessConfigMaster_Designation_Id1,0),
	                        ProcessConfigMaster_AddedOn,
	                        ProcessConfigMaster_AddedBy,
	                        ProcessConfigMaster_ModifiedOn,
	                        ProcessConfigMaster_ModifiedBy,
	                        ProcessConfigMaster_Status, 
	                        ProcessConfigMaster_Creation_Allowed, 
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed,
                            ProcessConfigMaster_Loop,
	                        Organization_Name = OfficeBranch_Name, 
	                        Department_Name,
	                        Designation_Name = tbl_Designation.Designation_DesignationName, 
                            Designation_Name1 = Isnull(Designation1.Designation_DesignationName,''), 
						    tbl_ProcessConfigInvStatus_Id.ProcessConfigInvStatus_InvoiceStatus_Id,
						    tbl_ProcessConfigInvStatus_Name.InvoiceStatus_Name, 
							tbl_ProcessConfigInvDocumentMaster_Name.TradeDocument_Name,
						    tbl_ProcessConfigDocumentMaster_Id.ProcessConfigDocumentLinking_DocumentMaster_Id
                        from tbl_ProcessConfigMaster
                        join tbl_OfficeBranch on OfficeBranch_Id = ProcessConfigMaster_OrgId
                        join tbl_Department on Department_Id = ProcessConfigMaster_Department_Id
                        join tbl_Designation on tbl_Designation.Designation_Id = ProcessConfigMaster_Designation_Id
                        left join tbl_Designation as Designation1 on Designation1.Designation_Id = ProcessConfigMaster_Designation_Id1
						
					    left join 
                        (
	                        SELECT	ProcessConfigInvStatus_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(ProcessConfigInvStatus_InvoiceStatus_Id AS VARCHAR(100)) [text()]
                                    FROM tbl_ProcessConfigInvStatus 
                                    WHERE ProcessConfigInvStatus_ConfigMasterId = t.ProcessConfigInvStatus_ConfigMasterId and tbl_ProcessConfigInvStatus.ProcessConfigInvStatus_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') ProcessConfigInvStatus_InvoiceStatus_Id
	                        FROM tbl_ProcessConfigInvStatus t
                            where t.ProcessConfigInvStatus_Status = 1
	                        GROUP BY ProcessConfigInvStatus_ConfigMasterId
                        ) tbl_ProcessConfigInvStatus_Id on tbl_ProcessConfigInvStatus_Id.ProcessConfigInvStatus_ConfigMasterId = ProcessConfigMaster_Id

					    left join 
                        (
	                        SELECT	ProcessConfigInvStatus_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(InvoiceStatus_Name AS VARCHAR(100)) [text()]
                                    FROM tbl_ProcessConfigInvStatus 
								    join tbl_InvoiceStatus on InvoiceStatus_Id = ProcessConfigInvStatus_InvoiceStatus_Id
                                    WHERE ProcessConfigInvStatus_ConfigMasterId = t.ProcessConfigInvStatus_ConfigMasterId and tbl_ProcessConfigInvStatus.ProcessConfigInvStatus_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') InvoiceStatus_Name
	                        FROM tbl_ProcessConfigInvStatus t
                            where t.ProcessConfigInvStatus_Status = 1
	                        GROUP BY ProcessConfigInvStatus_ConfigMasterId
                        ) tbl_ProcessConfigInvStatus_Name on tbl_ProcessConfigInvStatus_Name.ProcessConfigInvStatus_ConfigMasterId = ProcessConfigMaster_Id

						left join 
                        (
	                        SELECT	ProcessConfigDocumentLinking_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(ProcessConfigDocumentLinking_DocumentMaster_Id AS VARCHAR(100)) [text()]
                                    FROM tbl_ProcessConfigDocumentLinking 
                                    WHERE ProcessConfigDocumentLinking_ConfigMasterId = t.ProcessConfigDocumentLinking_ConfigMasterId and tbl_ProcessConfigDocumentLinking.ProcessConfigDocumentLinking_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') ProcessConfigDocumentLinking_DocumentMaster_Id
	                        FROM tbl_ProcessConfigDocumentLinking t
                            where t.ProcessConfigDocumentLinking_Status = 1
	                        GROUP BY ProcessConfigDocumentLinking_ConfigMasterId
                        ) tbl_ProcessConfigDocumentMaster_Id on tbl_ProcessConfigDocumentMaster_Id.ProcessConfigDocumentLinking_ConfigMasterId = ProcessConfigMaster_Id

					    left join 
                        (
	                        SELECT	ProcessConfigDocumentLinking_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(TradeDocument_Name AS VARCHAR(1000)) [text()]
                                    FROM tbl_ProcessConfigDocumentLinking 
								    join tbl_TradeDocument on TradeDocument_Id = ProcessConfigDocumentLinking_DocumentMaster_Id
                                    WHERE ProcessConfigDocumentLinking_ConfigMasterId = t.ProcessConfigDocumentLinking_ConfigMasterId and tbl_ProcessConfigDocumentLinking.ProcessConfigDocumentLinking_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') TradeDocument_Name
	                        FROM tbl_ProcessConfigDocumentLinking t
                            where t.ProcessConfigDocumentLinking_Status = 1
	                        GROUP BY ProcessConfigDocumentLinking_ConfigMasterId
                        ) tbl_ProcessConfigInvDocumentMaster_Name on tbl_ProcessConfigInvDocumentMaster_Name.ProcessConfigDocumentLinking_ConfigMasterId = ProcessConfigMaster_Id
                        where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' order by ProcessConfigMaster_Loop, ProcessConfigMaster_Id ";
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        select 
	                        ProcessConfigMaster_Id,
	                        ProcessConfigMaster_Process_Name,
	                        ProcessConfigMaster_OrgId,
	                        ProcessConfigMaster_Department_Id,
	                        ProcessConfigMaster_Designation_Id,
	                        ProcessConfigMaster_AddedOn,
	                        ProcessConfigMaster_AddedBy,
	                        ProcessConfigMaster_ModifiedOn,
	                        ProcessConfigMaster_ModifiedBy,
	                        ProcessConfigMaster_Status, 
	                        ProcessConfigMaster_Creation_Allowed, 
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed,
                            ProcessConfigMaster_Transfer_Allowed,
                            ProcessConfigMaster_Loop,
	                        Organization_Name = OfficeBranch_Name, 
	                        Department_Name,
	                        Designation_Name = Designation_DesignationName, 
						    tbl_ProcessConfigInvStatus_Id.ProcessConfigInvStatus_InvoiceStatus_Id,
						    tbl_ProcessConfigInvStatus_Name.InvoiceStatus_Name 
                        from tbl_ProcessConfigMaster
                        join tbl_OfficeBranch on OfficeBranch_Id = ProcessConfigMaster_OrgId
                        join tbl_Department on Department_Id = ProcessConfigMaster_Department_Id
                        join tbl_Designation on Designation_Id = ProcessConfigMaster_Designation_Id
                        left join 
                        (
	                        SELECT	ProcessConfigInvStatus_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(ProcessConfigInvStatus_InvoiceStatus_Id AS VARCHAR(100)) [text()]
                                    FROM tbl_ProcessConfigInvStatus 
                                    WHERE ProcessConfigInvStatus_ConfigMasterId = t.ProcessConfigInvStatus_ConfigMasterId and tbl_ProcessConfigInvStatus.ProcessConfigInvStatus_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') ProcessConfigInvStatus_InvoiceStatus_Id
	                        FROM tbl_ProcessConfigInvStatus t
                            where t.ProcessConfigInvStatus_Status = 1
	                        GROUP BY ProcessConfigInvStatus_ConfigMasterId
                        ) tbl_ProcessConfigInvStatus_Id on tbl_ProcessConfigInvStatus_Id.ProcessConfigInvStatus_ConfigMasterId = ProcessConfigMaster_Id

					    left join 
                        (
	                        SELECT	ProcessConfigInvStatus_ConfigMasterId,
			                        STUFF((SELECT ', ' + CAST(InvoiceStatus_Name AS VARCHAR(100)) [text()]
                                    FROM tbl_ProcessConfigInvStatus 
								    join tbl_InvoiceStatus on InvoiceStatus_Id = ProcessConfigInvStatus_InvoiceStatus_Id
                                    WHERE ProcessConfigInvStatus_ConfigMasterId = t.ProcessConfigInvStatus_ConfigMasterId and tbl_ProcessConfigInvStatus.ProcessConfigInvStatus_Status = 1
                                    FOR XML PATH(''), TYPE)
                                .value('.','NVARCHAR(MAX)'),1,2,' ') InvoiceStatus_Name
	                        FROM tbl_ProcessConfigInvStatus t
                            where t.ProcessConfigInvStatus_Status = 1
	                        GROUP BY ProcessConfigInvStatus_ConfigMasterId
                        ) tbl_ProcessConfigInvStatus_Name on tbl_ProcessConfigInvStatus_Name.ProcessConfigInvStatus_ConfigMasterId = ProcessConfigMaster_Id
                        where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' order by ProcessConfigMaster_Loop, ProcessConfigMaster_Id ";
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public int get_Loop(string Process_Name, int OrgId, int Designation_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery = @"select
                        ProcessConfigMaster_Loop = isnull(ProcessConfigMaster_Loop, 0)
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' and ProcessConfigMaster_OrgId = '" + OrgId + "' and  (ProcessConfigMaster_Designation_Id = '" + Designation_Id + "' or ProcessConfigMaster_Designation_Id1 = '" + Designation_Id + "')";

        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        if (AllClasses.CheckDataSet(ds))
        {
            return Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessConfigMaster_Loop"].ToString());
        }
        else
        {
            return 0;
        }
    }
    public DataSet get_ProcessConfigMaster_Last(string Process_Name, int Loop, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select * from (
                    select 
	                    ROW_NUMBER() over (order by ProcessConfigMaster_Id desc) rr,
                        ProcessConfigMaster_Id,
	                    ProcessConfigMaster_OrgId, 
	                    ProcessConfigMaster_Designation_Id,
                        ProcessConfigMaster_Designation_Id1,
	                    ProcessConfigMaster_Creation_Allowed,
	                    ProcessConfigMaster_Updation_Allowed, 
                        ProcessConfigMaster_Deduction_Allowed,
                        ProcessConfigMaster_Transfer_Allowed
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) tData where tData.rr = 1 ";
        if (Loop > 0)
        {
            strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
        }
        else
        {
            strQuery = strQuery.Replace("loopCondition", " ");
        }
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    public DataSet get_ProcessConfig_Current(string Process_Name, int OrgId, int Designation_Id, int Loop, int PackageInvoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Process_Name == "Invoice")
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrStep, isnull(PackageInvoiceApproval_Step_Count, 0) Step_Count, PackageInvoiceApproval_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_PackageInvoice_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                        select 
	                        ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";

            strQuery += Environment.NewLine;

            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                        declare @Loop int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageEMBApproval_PackageEMBMaster_Id order by PackageEMBApproval_Id desc) rrStep, isnull(PackageEMBApproval_Step_Count, 0) Step_Count, PackageEMBApproval_Id from tbl_PackageEMBApproval where PackageEMBApproval_Status = 1 and PackageEMBApproval_PackageEMBMaster_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                            select 
	                            ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                                ProcessConfigMaster_Id,
	                            ProcessConfigMaster_OrgId, 
	                            ProcessConfigMaster_Designation_Id,
                                ProcessConfigMaster_Designation_Id1,
	                            ProcessConfigMaster_Creation_Allowed,
	                            ProcessConfigMaster_Updation_Allowed, 
                                ProcessConfigMaster_Deduction_Allowed, 
                                ProcessConfigMaster_Transfer_Allowed
                            from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";

            strQuery += Environment.NewLine;

            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
        }
        ds = ExecuteSelectQuery(strQuery);
        return ds;
    }

    public DataSet get_ProcessConfigMaster_First(string Process_Name, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select * from (
                    select 
	                    ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                        ProcessConfigMaster_Id,
	                    ProcessConfigMaster_OrgId, 
	                    ProcessConfigMaster_Designation_Id,
                        ProcessConfigMaster_Designation_Id1,
	                    ProcessConfigMaster_Creation_Allowed,
	                    ProcessConfigMaster_Updation_Allowed, 
                        ProcessConfigMaster_Deduction_Allowed, 
                        ProcessConfigMaster_Transfer_Allowed
                    from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "') tData where tData.rr = 1 ";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    public DataSet get_ProcessConfigMaster_Next(string Process_Name, int OrgId, int Designation_Id, int PackageInvoice_Id, int Loop, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Process_Name == "Invoice")
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrStep, isnull(PackageInvoiceApproval_Step_Count, 0) Step_Count, PackageInvoiceApproval_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_PackageInvoice_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                        select 
	                        ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
            strQuery += Environment.NewLine;

            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) + 1 ";
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageEMBApproval_PackageEMBMaster_Id order by PackageEMBApproval_Id desc) rrStep, isnull(PackageEMBApproval_Step_Count, 0) Step_Count, PackageEMBApproval_Id from tbl_PackageEMBApproval where PackageEMBApproval_Status = 1 and PackageEMBApproval_PackageEMBMaster_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                        select 
	                        ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
            strQuery += Environment.NewLine;

            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) + 1 ";
        }
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    public DataSet get_ProcessConfigMaster_Prev(string Process_Name, int OrgId, int Designation_Id, int PackageInvoice_Id, int Loop, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Process_Name == "Invoice")
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrStep, isnull(PackageInvoiceApproval_Step_Count, 0) Step_Count, PackageInvoiceApproval_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_PackageInvoice_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                        select 
	                        ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
            strQuery += Environment.NewLine;
            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) - 1 ";
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        declare @Step_Count int = 0;
                    
                        set @Step_Count = (select isnull(Step_Count, 0) from (select ROW_NUMBER() over (partition by PackageEMBApproval_PackageEMBMaster_Id order by PackageEMBApproval_Id desc) rrStep, isnull(PackageEMBApproval_Step_Count, 0) Step_Count, PackageEMBApproval_Id from tbl_PackageEMBApproval where PackageEMBApproval_Status = 1 and PackageEMBApproval_PackageEMBMaster_Id = '" + PackageInvoice_Id + "') tD where tD.rrStep = 1) ";
            strQuery += Environment.NewLine;

            strQuery += @";with cte as (
                            select 
	                            ROW_NUMBER() over (order by ProcessConfigMaster_Id asc) rr,
                                ProcessConfigMaster_Id,
	                            ProcessConfigMaster_OrgId, 
	                            ProcessConfigMaster_Designation_Id,
                                ProcessConfigMaster_Designation_Id1,
	                            ProcessConfigMaster_Creation_Allowed,
	                            ProcessConfigMaster_Updation_Allowed, 
                                ProcessConfigMaster_Deduction_Allowed, 
                                ProcessConfigMaster_Transfer_Allowed
                            from tbl_ProcessConfigMaster where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' loopCondition) ";
            if (Loop > 0)
            {
                strQuery = strQuery.Replace("loopCondition", " and ProcessConfigMaster_Loop = '" + Loop + "'");
            }
            else
            {
                strQuery = strQuery.Replace("loopCondition", " ");
            }
            strQuery += Environment.NewLine;
            strQuery += @"select 
	                        top 1
	                        rr,
                            ProcessConfigMaster_Id,
	                        ProcessConfigMaster_OrgId, 
	                        ProcessConfigMaster_Designation_Id,
                            ProcessConfigMaster_Designation_Id1,
	                        ProcessConfigMaster_Creation_Allowed,
	                        ProcessConfigMaster_Updation_Allowed, 
                            ProcessConfigMaster_Deduction_Allowed, 
                            ProcessConfigMaster_Transfer_Allowed
                        from cte where cte.rr = isnull(@Step_Count, 0) - 1 ";
        }
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    public bool Insert_tbl_ProcessConfigMaster(List<tbl_ProcessConfigMaster> obj_tbl_ProcessConfigMaster_Li, int Added_By, string Process_Name)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Update_tbl_ProcessConfigMaster(Process_Name, Added_By, trans, cn);
                for (int i = 0; i < obj_tbl_ProcessConfigMaster_Li.Count; i++)
                {
                    string sql = "";
                    obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigMaster_Id = Insert_tbl_ProcessConfigMaster(obj_tbl_ProcessConfigMaster_Li[i], trans, cn);
                    if (obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigInvStatus_InvoiceStatus_Id != null)
                    {
                        string[] Status_Id = obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigInvStatus_InvoiceStatus_Id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < Status_Id.Length; j++)
                        {
                            sql = "set dateformat dmy; insert into tbl_ProcessConfigInvStatus (ProcessConfigInvStatus_ConfigMasterId, ProcessConfigInvStatus_InvoiceStatus_Id, ProcessConfigInvStatus_AddedOn, ProcessConfigInvStatus_AddedBy, ProcessConfigInvStatus_Status) values ('" + obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigMaster_Id + "', '" + Status_Id[j].Trim() + "', getdate(), '" + obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigMaster_AddedBy + "', 1); select @@IDENTITY";
                            ExecuteSelectQuerywithTransaction(cn, sql, trans);
                        }
                    }
                    if (obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigDocumentLinking_DocumentMaster_Id != null)
                    {
                        string[] Document_Id = obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigDocumentLinking_DocumentMaster_Id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < Document_Id.Length; j++)
                        {
                            sql = " set dateformat dmy;insert into tbl_ProcessConfigDocumentLinking ( [ProcessConfigDocumentLinking_AddedBy],[ProcessConfigDocumentLinking_AddedOn],[ProcessConfigDocumentLinking_ConfigMasterId],[ProcessConfigDocumentLinking_DocumentMaster_Id],[ProcessConfigDocumentLinking_Status] ) values ('" + obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigMaster_AddedBy + "',getdate(), '" + obj_tbl_ProcessConfigMaster_Li[i].ProcessConfigMaster_Id + "','" + Document_Id[j].Trim() + "', 1); Select @@Identity";
                            ExecuteSelectQuerywithTransaction(cn, sql, trans);
                        }
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    private int Insert_tbl_ProcessConfigMaster(tbl_ProcessConfigMaster obj_tbl_ProcessConfigMaster, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProcessConfigMaster ( [ProcessConfigMaster_AddedBy],[ProcessConfigMaster_AddedOn],[ProcessConfigMaster_Department_Id],[ProcessConfigMaster_Designation_Id],[ProcessConfigMaster_OrgId],[ProcessConfigMaster_Process_Name],[ProcessConfigMaster_Status], [ProcessConfigMaster_Updation_Allowed], [ProcessConfigMaster_Creation_Allowed], [ProcessConfigMaster_Deduction_Allowed], [ProcessConfigMaster_Transfer_Allowed], [ProcessConfigMaster_Loop],ProcessConfigMaster_Designation_Id1) values ('" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_AddedBy + "',getdate(),'" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Department_Id + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_OrgId + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Process_Name + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Status + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Updation_Allowed + "','" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Creation_Allowed + "', '" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Deduction_Allowed + "', '" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Transfer_Allowed + "', '" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Loop + "', '" + obj_tbl_ProcessConfigMaster.ProcessConfigMaster_Designation_Id1 + "'); Select @@Identity";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    private void Update_tbl_ProcessConfigMaster(string ProcessName, int AddedBy, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;update tbl_ProcessConfigMaster set ProcessConfigMaster_ModifiedOn = getdate(), ProcessConfigMaster_ModifiedBy = '" + AddedBy + "', ProcessConfigMaster_Status = 0 where ProcessConfigMaster_Process_Name = '" + ProcessName + "' and ProcessConfigMaster_Status = 1";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region Report Data Entry Analysis
    public DataSet get_Division_Wise_Data_Entry_Analysis(int Scheme_Id, int District_Id, int Zone_Id, int Circle_Id, int Division_Id, bool BOQ_Uploaded)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        Zone_Name,
                        Circle_Name, 
                        Division_Name,
	                    ProjectWork_Id, 
	                    ProjectWork_Name,
                        ProjectWork_ProjectCode,
	                    Project_Budget = ProjectWork_Budget, 
	                    tPackage_Details.Total_Packages, 
	                    tPackage_Details.Package_AgreementAmount, 
	                    Total_BOQ,
	                    BOQ_Qty,
	                    Qty_Paid, 
						Updated, 
						Freezed
                    from tbl_ProjectWork
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    join tbl_Circle on Circle_Id = Division_CircleId
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    left join 
                    (
	                    select 
		                    ProjectWorkPkg_Work_Id, 
		                    count(*) Total_Packages,
		                    Package_AgreementAmount = sum(isnull(ProjectWorkPkg_AgreementAmount, 0)), 
		                    Total_BOQ = sum(isnull(tBOQ.Total_BOQ, 0)), 
		                    BOQ_Qty = sum(isnull(tBOQ.BOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(tBOQ.Qty_Paid, 0)), 
		                    Updated = sum(isnull(tBOQ.Updated, 0)), 
							Freezed = sum(case when ISNULL(ProjectWorkPkg_ApprovalFile_Path,'') != '' then 1 else 0 end)
	                    from tbl_ProjectWorkPkg 
	                    left join
	                    (
		                    select 
			                    tbl_PackageBOQ.PackageBOQ_Package_Id, 
			                    count(*) Total_BOQ, 
			                    BOQ_Qty = sum(isnull(tbl_PackageBOQ.PackageBOQ_Qty, 0)), 
			                    Qty_Paid = sum(isnull(tbl_PackageBOQ.PackageBOQ_QtyPaid, 0)), 
								Updated = count(distinct tbl_PackageBOQ_History.PackageBOQ_Package_Id)
		                    from tbl_PackageBOQ
							left join tbl_PackageBOQ_History on tbl_PackageBOQ_History.PackageBOQ_Id = tbl_PackageBOQ.PackageBOQ_Id
		                    where tbl_PackageBOQ.PackageBOQ_Status = 1
		                    group by tbl_PackageBOQ.PackageBOQ_Package_Id
	                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id	                    
	                    where ProjectWorkPkg_Status = 1 
	                    group by ProjectWorkPkg_Work_Id
                    ) tPackage_Details on tPackage_Details.ProjectWorkPkg_Work_Id = ProjectWork_Id
                    where ProjectWork_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond Division_IdCond Zone_IdCond Circle_IdCond Total_BOQCond 
                    order by  Zone_Name, Circle_Name, Division_Name, ProjectWork_Name";

        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        if (Zone_Id > 0)
            strQuery = strQuery.Replace("Zone_IdCond", "and Circle_ZoneId = " + Zone_Id.ToString());
        else
            strQuery = strQuery.Replace("Zone_IdCond", "");

        if (Circle_Id > 0)
            strQuery = strQuery.Replace("Circle_IdCond", "and Division_CircleId = " + Circle_Id.ToString());
        else
            strQuery = strQuery.Replace("Circle_IdCond", "");

        if (Division_Id > 0)
            strQuery = strQuery.Replace("Division_IdCond", "and ProjectWork_DivisionId = " + Division_Id.ToString());
        else
            strQuery = strQuery.Replace("Division_IdCond", "");

        if (BOQ_Uploaded)
            strQuery = strQuery.Replace("Total_BOQCond", "and isnull(Total_BOQ, 0) > 0 ");
        else
            strQuery = strQuery.Replace("Total_BOQCond", "");

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Delete_BOQ(int Work_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = @"set dateformat dmy; 
                        update tbl_PackageBOQ
	                        set PackageBOQ_ModifiedBy = '" + person_Id + "', PackageBOQ_ModifiedOn = GETDATE(), PackageBOQ_Status = 0 from tbl_PackageBOQ join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageBOQ_Package_Id where ProjectWorkPkg_Work_Id = '" + Work_Id + "' and ProjectWorkPkg_Status = 1 and PackageBOQ_Status = 1 ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Report Zone Wise Analysis
    public DataSet get_Zone_Wise_Analysis(int Scheme_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Zone_Id, 
	                    Zone_Name, 
	                    tPackage_Details.Total_Project, 
	                    tPackage_Details.Project_Budget, 
	                    tPackage_Details.Total_Packages, 
	                    tPackage_Details.Package_AgreementAmount, 
	                    Total_BOQ,
	                    BOQ_Qty,
	                    Qty_Paid, 
	                    Total_Invoice, 
	                    InvoiceItem_Total_Qty, 
	                    Total_Invoice_Amount
                    from tbl_Zone
                    left join 
                    (
	                    select 
		                    Circle_ZoneId, 
		                    count(distinct ProjectWorkPkg_Work_Id) Total_Project,
		                    Project_Budget = sum(convert(decimal(18,3), isnull(ProjectWork_Budget, '0'))),
		                    count(*) Total_Packages,
		                    Package_AgreementAmount = sum(isnull(ProjectWorkPkg_AgreementAmount, 0)), 
		                    Total_BOQ = sum(isnull(tBOQ.Total_BOQ, 0)), 
		                    BOQ_Qty = sum(isnull(tBOQ.BOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(tBOQ.Qty_Paid, 0)), 
		                    Total_Invoice = sum(tInvoice.Total_Invoice), 
		                    InvoiceItem_Total_Qty = sum(tInvoice.PackageInvoiceItem_Total_Qty), 
		                    Total_Invoice_Amount = sum(tInvoice.Total_Amount)
	                    from tbl_ProjectWorkPkg 
	                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
	                    join tbl_Division on ProjectWork_DivisionId = Division_Id
	                    join tbl_Circle on Circle_Id = Division_CircleId
	                    left join
	                    (
		                    select 
			                    PackageBOQ_Package_Id, 
			                    count(*) Total_BOQ, 
			                    BOQ_Qty = sum(isnull(PackageBOQ_Qty, 0)), 
			                    Qty_Paid = sum(isnull(PackageBOQ_QtyPaid, 0)) 
		                    from tbl_PackageBOQ
		                    where PackageBOQ_Status = 1
		                    group by PackageBOQ_Package_Id
	                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id

	                    left join
	                    (
		                    select 
			                    PackageInvoice_Package_Id, 
			                    count(*) Total_Invoice, 
			                    Total_Line_Items = sum(tPackageInvoiceItem.Total_Line_Items), 
			                    PackageInvoiceItem_Total_Qty = sum(tPackageInvoiceItem.PackageInvoiceItem_Total_Qty), 
			                    Total_Amount = sum(tPackageInvoiceItem.Total_Amount) 
		                    from tbl_PackageInvoice
		                    left join 
                            (
	                            select 
		                            PackageInvoiceItem_Invoice_Id,
		                            Total_Line_Items = count(*),
		                            PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
                                    Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                            from tbl_PackageInvoiceItem
	                            where PackageInvoiceItem_Status = 1
	                            group by PackageInvoiceItem_Invoice_Id
                            ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
		                    where PackageInvoice_Status = 1
		                    group by PackageInvoice_Package_Id
	                    ) tInvoice on tInvoice.PackageInvoice_Package_Id = ProjectWorkPkg_Id

	                    where ProjectWorkPkg_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond
	                    group by Circle_ZoneId
                    ) tPackage_Details on tPackage_Details.Circle_ZoneId = Zone_Id
                    where Zone_Status = 1
                    order by Zone_Name";
        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Circle Wise Analysis
    public DataSet get_Circle_Wise_Analysis(int Scheme_Id, int Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Circle_Id, 
	                    Circle_Name, 
	                    tPackage_Details.Total_Project, 
	                    tPackage_Details.Project_Budget, 
	                    tPackage_Details.Total_Packages, 
	                    tPackage_Details.Package_AgreementAmount, 
	                    Total_BOQ,
	                    BOQ_Qty,
	                    Qty_Paid, 
	                    Total_Invoice, 
	                    InvoiceItem_Total_Qty, 
	                    Total_Invoice_Amount
                    from tbl_Circle
                    left join 
                    (
	                    select 
		                    Division_CircleId, 
		                    count(distinct ProjectWorkPkg_Work_Id) Total_Project,
		                    Project_Budget = sum(convert(decimal(18,3), isnull(ProjectWork_Budget, '0'))),
		                    count(*) Total_Packages,
		                    Package_AgreementAmount = sum(isnull(ProjectWorkPkg_AgreementAmount, 0)), 
		                    Total_BOQ = sum(isnull(tBOQ.Total_BOQ, 0)), 
		                    BOQ_Qty = sum(isnull(tBOQ.BOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(tBOQ.Qty_Paid, 0)), 
		                    Total_Invoice = sum(tInvoice.Total_Invoice), 
		                    InvoiceItem_Total_Qty = sum(tInvoice.PackageInvoiceItem_Total_Qty), 
		                    Total_Invoice_Amount = sum(tInvoice.Total_Amount)
	                    from tbl_ProjectWorkPkg 
	                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
	                    join tbl_Division on ProjectWork_DivisionId = Division_Id
	                    join tbl_Circle on Circle_Id = Division_CircleId
	                    left join
	                    (
		                    select 
			                    PackageBOQ_Package_Id, 
			                    count(*) Total_BOQ, 
			                    BOQ_Qty = sum(isnull(PackageBOQ_Qty, 0)), 
			                    Qty_Paid = sum(isnull(PackageBOQ_QtyPaid, 0)) 
		                    from tbl_PackageBOQ
		                    where PackageBOQ_Status = 1
		                    group by PackageBOQ_Package_Id
	                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id

	                    left join
	                    (
		                    select 
			                    PackageInvoice_Package_Id, 
			                    count(*) Total_Invoice, 
			                    Total_Line_Items = sum(tPackageInvoiceItem.Total_Line_Items), 
			                    PackageInvoiceItem_Total_Qty = sum(tPackageInvoiceItem.PackageInvoiceItem_Total_Qty), 
			                    Total_Amount = sum(tPackageInvoiceItem.Total_Amount) 
		                    from tbl_PackageInvoice
		                    left join 
                            (
	                            select 
		                            PackageInvoiceItem_Invoice_Id,
		                            Total_Line_Items = count(*),
		                            PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)),
                                    Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                            from tbl_PackageInvoiceItem
	                            where PackageInvoiceItem_Status = 1
	                            group by PackageInvoiceItem_Invoice_Id
                            ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
		                    where PackageInvoice_Status = 1
		                    group by PackageInvoice_Package_Id
	                    ) tInvoice on tInvoice.PackageInvoice_Package_Id = ProjectWorkPkg_Id

	                    where ProjectWorkPkg_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond and Circle_ZoneId = Zone_IdCond
	                    group by Division_CircleId
                    ) tPackage_Details on tPackage_Details.Division_CircleId = Circle_Id
                    where Circle_Status = 1 and Circle_ZoneId = Zone_IdCond
                    order by Circle_Name";
        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        strQuery = strQuery.Replace("Zone_IdCond", Zone_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Division Wise Analysis
    public DataSet get_Division_Wise_Analysis(int Scheme_Id, int Circle_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Division_Id, 
	                    Division_Name, 
	                    tPackage_Details.Total_Project, 
	                    tPackage_Details.Project_Budget, 
	                    tPackage_Details.Total_Packages, 
	                    tPackage_Details.Package_AgreementAmount, 
	                    Total_BOQ,
	                    BOQ_Qty,
	                    Qty_Paid, 
	                    Total_Invoice, 
	                    InvoiceItem_Total_Qty, 
	                    Total_Invoice_Amount
                    from tbl_Division
                    left join 
                    (
	                    select 
		                    ProjectWork_DivisionId, 
		                    count(distinct ProjectWorkPkg_Work_Id) Total_Project,
		                    Project_Budget = sum(convert(decimal(18,3), isnull(ProjectWork_Budget, '0'))),
		                    count(*) Total_Packages,
		                    Package_AgreementAmount = sum(isnull(ProjectWorkPkg_AgreementAmount, 0)), 
		                    Total_BOQ = sum(isnull(tBOQ.Total_BOQ, 0)), 
		                    BOQ_Qty = sum(isnull(tBOQ.BOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(tBOQ.Qty_Paid, 0)), 
		                    Total_Invoice = sum(tInvoice.Total_Invoice), 
		                    InvoiceItem_Total_Qty = sum(tInvoice.PackageInvoiceItem_Total_Qty), 
		                    Total_Invoice_Amount = sum(tInvoice.Total_Amount)
	                    from tbl_ProjectWorkPkg 
	                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
	                    join tbl_Division on ProjectWork_DivisionId = Division_Id
	                    left join
	                    (
		                    select 
			                    PackageBOQ_Package_Id, 
			                    count(*) Total_BOQ, 
			                    BOQ_Qty = sum(isnull(PackageBOQ_Qty, 0)), 
			                    Qty_Paid = sum(isnull(PackageBOQ_QtyPaid, 0)) 
		                    from tbl_PackageBOQ
		                    where PackageBOQ_Status = 1
		                    group by PackageBOQ_Package_Id
	                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id

	                    left join
	                    (
		                    select 
			                    PackageInvoice_Package_Id, 
			                    count(*) Total_Invoice, 
			                    Total_Line_Items = sum(tPackageInvoiceItem.Total_Line_Items), 
			                    PackageInvoiceItem_Total_Qty = sum(tPackageInvoiceItem.PackageInvoiceItem_Total_Qty), 
			                    Total_Amount = sum(tPackageInvoiceItem.Total_Amount) 
		                    from tbl_PackageInvoice
		                    left join 
                            (
	                            select 
		                            PackageInvoiceItem_Invoice_Id,
		                            Total_Line_Items = count(*),
		                            PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
		                            Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                            from tbl_PackageInvoiceItem
	                            where PackageInvoiceItem_Status = 1
	                            group by PackageInvoiceItem_Invoice_Id
                            ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
		                    where PackageInvoice_Status = 1
		                    group by PackageInvoice_Package_Id
	                    ) tInvoice on tInvoice.PackageInvoice_Package_Id = ProjectWorkPkg_Id

	                    where ProjectWorkPkg_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond and Division_CircleId = Circle_IdCond
	                    group by ProjectWork_DivisionId
                    ) tPackage_Details on tPackage_Details.ProjectWork_DivisionId = Division_Id
                    where Division_Status = 1 and Division_CircleId = Circle_IdCond 
                    order by Division_Name";
        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        strQuery = strQuery.Replace("Circle_IdCond", Circle_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Project Wise Analysis
    public DataSet get_Project_Wise_Analysis(int Scheme_Id, int Division_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ProjectWork_Id, 
	                    ProjectWork_Name,
                        ProjectWork_ProjectCode,
	                    Project_Budget = ProjectWork_Budget, 
	                    tPackage_Details.Total_Packages, 
	                    tPackage_Details.Package_AgreementAmount, 
	                    Total_BOQ,
	                    BOQ_Qty,
	                    Qty_Paid, 
	                    Total_Invoice, 
	                    InvoiceItem_Total_Qty, 
	                    Total_Invoice_Amount
                    from tbl_ProjectWork
                    left join 
                    (
	                    select 
		                    ProjectWorkPkg_Work_Id, 
		                    count(*) Total_Packages,
		                    Package_AgreementAmount = sum(isnull(ProjectWorkPkg_AgreementAmount, 0)), 
		                    Total_BOQ = sum(isnull(tBOQ.Total_BOQ, 0)), 
		                    BOQ_Qty = sum(isnull(tBOQ.BOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(tBOQ.Qty_Paid, 0)), 
		                    Total_Invoice = sum(tInvoice.Total_Invoice), 
		                    InvoiceItem_Total_Qty = sum(tInvoice.PackageInvoiceItem_Total_Qty), 
		                    Total_Invoice_Amount = sum(tInvoice.Total_Amount)
	                    from tbl_ProjectWorkPkg 
	                    left join
	                    (
		                    select 
			                    PackageBOQ_Package_Id, 
			                    count(*) Total_BOQ, 
			                    BOQ_Qty = sum(isnull(PackageBOQ_Qty, 0)), 
			                    Qty_Paid = sum(isnull(PackageBOQ_QtyPaid, 0)) 
		                    from tbl_PackageBOQ
		                    where PackageBOQ_Status = 1
		                    group by PackageBOQ_Package_Id
	                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id

	                    left join
	                    (
		                    select 
			                    PackageInvoice_Package_Id, 
			                    count(*) Total_Invoice, 
			                    Total_Line_Items = sum(tPackageInvoiceItem.Total_Line_Items), 
			                    PackageInvoiceItem_Total_Qty = sum(tPackageInvoiceItem.PackageInvoiceItem_Total_Qty), 
			                    Total_Amount = sum(tPackageInvoiceItem.Total_Amount) 
		                    from tbl_PackageInvoice
		                    left join 
                            (
	                            select 
		                            PackageInvoiceItem_Invoice_Id,
		                            Total_Line_Items = count(*),
		                            PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
		                            Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                            from tbl_PackageInvoiceItem
	                            where PackageInvoiceItem_Status = 1
	                            group by PackageInvoiceItem_Invoice_Id
                            ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
		                    where PackageInvoice_Status = 1
		                    group by PackageInvoice_Package_Id
	                    ) tInvoice on tInvoice.PackageInvoice_Package_Id = ProjectWorkPkg_Id

	                    where ProjectWorkPkg_Status = 1 
	                    group by ProjectWorkPkg_Work_Id
                    ) tPackage_Details on tPackage_Details.ProjectWorkPkg_Work_Id = ProjectWork_Id
                    where ProjectWork_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond and ProjectWork_DivisionId = Division_IdCond
                    order by ProjectWork_Name";
        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        strQuery = strQuery.Replace("Division_IdCond", Division_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Package Wise Analysis
    public DataSet get_Package_Wise_Analysis(int ProjectWork_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ProjectWorkPkg_Id,
	                    ProjectWorkPkg_Work_Id,
                        ProjectWorkPkg_Code, 
	                    ProjectWorkPkg_Name,
                        ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
	                    ProjectWorkPkg_AgreementAmount,
	                    ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
	                    ProjectWorkPkg_Agreement_No,
	                    ProjectWorkPkg_Indent_No,
	                    ProjectWorkPkg_Vendor_Id,
	                    ProjectWorkPkg_Staff_Id,
	                    Vendor_Name = Vendor.Person_Name,
	                    Vendor_Mobile = Vendor.Person_Mobile1,
	                    List_ReportingStaff_JEAPE_Name,
						List_ReportingStaff_AEPE_Name,
                        ProjectWork_Id, 
                        ProjectWork_Project_Id, 
                        Project_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Name, 
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
	                    Jurisdiction_Name_Eng, 
	                    Division_Name, 
	                    Circle_Name, 
	                    ProjectWork_DistrictId, 
	                    ProjectWork_ULB_Id, 
	                    ProjectWork_DivisionId, 
	                    Division_CircleId, 
	                    Package_AgreementAmount = isnull(ProjectWorkPkg_AgreementAmount, 0), 
	                    Total_BOQ = isnull(tBOQ.Total_BOQ, 0), 
	                    BOQ_Qty = isnull(tBOQ.BOQ_Qty, 0), 
	                    Qty_Paid = isnull(tBOQ.Qty_Paid, 0), 
	                    Total_Invoice = tInvoice.Total_Invoice, 
	                    InvoiceItem_Total_Qty = tInvoice.PackageInvoiceItem_Total_Qty, 
	                    Total_Invoice_Amount = tInvoice.Total_Amount
                    from tbl_ProjectWorkPkg
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on Project_Id = ProjectWork_Project_Id
                    join M_Jurisdiction on M_Jurisdiction_Id = ProjectWork_DistrictId
                    left join tbl_Division on Division_Id = ProjectWork_DivisionId
                    left join tbl_Circle on Circle_Id = Division_CircleId
                    left join tbl_PersonDetail Vendor on Vendor.Person_Id = ProjectWorkPkg_Vendor_Id
                    left join tbl_PersonDetail Staff on Staff.Person_Id = ProjectWorkPkg_Staff_Id

                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_JE_APE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_JE_APE.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_JEAPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_JE_APE t
                                            where t.ProjectWorkPkg_ReportingStaff_JE_APE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_JE_APE on ProjectWorkPkg_ReportingStaff_JE_APE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join(
                               SELECT	ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id,
			                                        STUFF((SELECT ', ' + CAST(Person_Name AS VARCHAR(100)) [text()]
                                                    FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE
													inner join tbl_PersonDetail on Person_Id=ProjectWorkPkg_ReportingStaff_AE_PE_Person_Id
									                WHERE ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id = t.ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id and tbl_ProjectWorkPkg_ReportingStaff_AE_PE.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
                                                    FOR XML PATH(''), TYPE)
                                                .value('.','NVARCHAR(MAX)'),1,2,' ') as List_ReportingStaff_AEPE_Name
	                                        FROM tbl_ProjectWorkPkg_ReportingStaff_AE_PE t
                                            where t.ProjectWorkPkg_ReportingStaff_AE_PE_Status = 1
	                                        GROUP BY ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id                          
                            ) tbl_ProjectWorkPkg_ReportingStaff_AE_PE on ProjectWorkPkg_ReportingStaff_AE_PE_ProjectWorkPkg_Id=ProjectWorkPkg_Id
                    left join
                    (
	                    select 
		                    PackageBOQ_Package_Id, 
		                    count(*) Total_BOQ, 
		                    BOQ_Qty = sum(isnull(PackageBOQ_Qty, 0)), 
		                    Qty_Paid = sum(isnull(PackageBOQ_QtyPaid, 0)) 
	                    from tbl_PackageBOQ
	                    where PackageBOQ_Status = 1
	                    group by PackageBOQ_Package_Id
                    ) tBOQ on tBOQ.PackageBOQ_Package_Id = ProjectWorkPkg_Id

                    left join
                    (
	                    select 
		                    PackageInvoice_Package_Id, 
		                    count(*) Total_Invoice, 
		                    Total_Line_Items = sum(tPackageInvoiceItem.Total_Line_Items), 
		                    PackageInvoiceItem_Total_Qty = sum(tPackageInvoiceItem.PackageInvoiceItem_Total_Qty), 
		                    Total_Amount = sum(tPackageInvoiceItem.Total_Amount) 
	                    from tbl_PackageInvoice
	                    left join 
                        (
	                        select 
		                        PackageInvoiceItem_Invoice_Id,
		                        Total_Line_Items = count(*),
		                        PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
		                        Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                        from tbl_PackageInvoiceItem
	                        where PackageInvoiceItem_Status = 1
	                        group by PackageInvoiceItem_Invoice_Id
                        ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
	                    where PackageInvoice_Status = 1
	                    group by PackageInvoice_Package_Id
                    ) tInvoice on tInvoice.PackageInvoice_Package_Id = ProjectWorkPkg_Id
                    where ProjectWorkPkg_Status = 1 and ProjectWork_Status = 1 and ProjectWorkPkg_Work_Id = '" + ProjectWork_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Zone Wise Invoice Status
    public DataSet get_Zone_Wise_Invoice_Status_Analysis(int Scheme_Id, int Org_Id, int Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Zone_Id,
	                    Zone_Name,
	                    count(*) Total_Invoices, 
	                    Pending = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0 then 1 else 0 end),
	                    Approved = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 1 then 1 else 0 end),
	                    De_Escelate = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 2 then 1 else 0 end),
	                    Rejected = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 3 then 1 else 0 end)
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrApp, PackageInvoiceApproval_Status_Id, PackageInvoiceApproval_PackageInvoice_Id, PackageInvoiceApproval_Package_Id, PackageInvoiceApproval_Id, PackageInvoiceApproval_Next_Organisation_Id, PackageInvoiceApproval_Next_Designation_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 AdditionalCondition) t_PackageInvoiceApproval on t_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and rrApp = 1
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id and ProjectWorkPkg_Status = 1
                    join tbl_ProjectWork on ProjectWorkPkg_Work_Id = ProjectWork_Id and ProjectWork_Status = 1
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    join tbl_Circle on Circle_Id = Division_CircleId
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    where PackageInvoice_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond
                    group by Zone_Id, Zone_Name";
        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        if (Org_Id > 0 && Designation_Id > 0)
        {
            strQuery = strQuery.Replace("AdditionalCondition", " and PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("AdditionalCondition", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Circle Wise Invoice Status
    public DataSet get_Circle_Wise_Invoice_Status_Analysis(int Scheme_Id, int Zone_Id, int Org_Id, int Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Circle_Id,
	                    Circle_Name,
	                    count(*) Total_Invoices, 
	                    Pending = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0 then 1 else 0 end),
	                    Approved = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 1 then 1 else 0 end),
	                    De_Escelate = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 2 then 1 else 0 end),
	                    Rejected = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 3 then 1 else 0 end)
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrApp, PackageInvoiceApproval_Status_Id, PackageInvoiceApproval_PackageInvoice_Id, PackageInvoiceApproval_Package_Id, PackageInvoiceApproval_Id, PackageInvoiceApproval_Next_Organisation_Id, PackageInvoiceApproval_Next_Designation_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 AdditionalCondition) t_PackageInvoiceApproval on t_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and rrApp = 1
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id and ProjectWorkPkg_Status = 1
                    join tbl_ProjectWork on ProjectWorkPkg_Work_Id = ProjectWork_Id and ProjectWork_Status = 1
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    join tbl_Circle on Circle_Id = Division_CircleId
                    where PackageInvoice_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond and Circle_ZoneId = Zone_IdCond
                    group by Circle_Id, Circle_Name";

        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        strQuery = strQuery.Replace("Zone_IdCond", Zone_Id.ToString());

        if (Org_Id > 0 && Designation_Id > 0)
        {
            strQuery = strQuery.Replace("AdditionalCondition", " and PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("AdditionalCondition", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Division Wise Invoice Status
    public DataSet get_Division_Wise_Invoice_Status_Analysis(int Scheme_Id, int Circle_Id, int Org_Id, int Designation_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Division_Id,
	                    Division_Name,
	                    count(*) Total_Invoices, 
	                    Pending = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 0 then 1 else 0 end),
	                    Approved = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 1 then 1 else 0 end),
	                    De_Escelate = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 2 then 1 else 0 end),
	                    Rejected = sum(case when t_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id = 3 then 1 else 0 end)
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join (select ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrApp, PackageInvoiceApproval_Status_Id, PackageInvoiceApproval_PackageInvoice_Id, PackageInvoiceApproval_Package_Id, PackageInvoiceApproval_Id, PackageInvoiceApproval_Next_Organisation_Id, PackageInvoiceApproval_Next_Designation_Id from tbl_PackageInvoiceApproval where PackageInvoiceApproval_Status = 1 AdditionalCondition) t_PackageInvoiceApproval on t_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and rrApp = 1
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id and ProjectWorkPkg_Status = 1
                    join tbl_ProjectWork on ProjectWorkPkg_Work_Id = ProjectWork_Id and ProjectWork_Status = 1
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    where PackageInvoice_Status = 1 and ProjectWork_Project_Id = Scheme_IdCond and Division_CircleId = Circle_IdCond
                    group by Division_Id, Division_Name";

        strQuery = strQuery.Replace("Scheme_IdCond", Scheme_Id.ToString());
        strQuery = strQuery.Replace("Circle_IdCond", Circle_Id.ToString());

        if (Org_Id > 0 && Designation_Id > 0)
        {
            strQuery = strQuery.Replace("AdditionalCondition", " and PackageInvoiceApproval_Next_Organisation_Id = '" + Org_Id + "' and PackageInvoiceApproval_Next_Designation_Id = '" + Designation_Id + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("AdditionalCondition", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Auto_Voucher_Numbers

    public string get_tbl_TransactionNos(VoucherTypes _VoucherTypes, int ProjectWorkPkg_Id, SqlTransaction trans, SqlConnection cn)
    {
        string sqlVoucerno = "", strVoucherNo = "";
        DataTable dt = new DataTable();
        int val = 0;
        if (_VoucherTypes == VoucherTypes.Invoice)
        {
            sqlVoucerno = "select count(*) from tbl_PackageInvoice where PackageInvoice_Package_Id = '" + ProjectWorkPkg_Id + "'";
        }
        if (_VoucherTypes == VoucherTypes.EMB)
        {
            sqlVoucerno = "select count(*) from tbl_PackageEMB_Master where PackageEMB_Master_Package_Id = '" + ProjectWorkPkg_Id + "'";
        }

        try
        {
            if (trans == null)
            {
                dt = ExecuteSelectQuery(sqlVoucerno).Tables[0];
            }
            else
            {
                dt = ExecuteSelectQuerywithTransaction(cn, sqlVoucerno, trans).Tables[0];
            }
            if (AllClasses.CheckDt(dt))
            {
                val = Convert.ToInt32(dt.Rows[0][0].ToString()) + 1;
            }
            else
            {
                val = 1;
            }
        }
        catch (Exception)
        {
            dt = null;
        }
        if (_VoucherTypes == VoucherTypes.Invoice)
        {
            strVoucherNo = "UPJN/I/" + DateTime.Now.Year.ToString() + "/" + val.ToString().PadLeft(5, '0');
        }
        if (_VoucherTypes == VoucherTypes.EMB)
        {
            strVoucherNo = "UPJN/E/" + DateTime.Now.Year.ToString() + "/" + val.ToString().PadLeft(5, '0');
        }
        return strVoucherNo;
    }
    #endregion

    #region Package Invoice Approval
    public DataSet get_tbl_PackageInvoiceApproval_History(int PackageInvoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoiceApproval_Id,
	                    PackageInvoiceApproval_Date = convert(char(10), PackageInvoiceApproval_Date, 103),
	                    PackageInvoiceApproval_PackageInvoice_Id,
	                    PackageInvoiceApproval_Package_Id,
	                    PackageInvoiceApproval_Status_Id,
	                    PackageInvoiceApproval_AddedBy,
	                    PackageInvoiceApproval_AddedOn,
	                    PackageInvoiceApproval_Next_Organisation_Id,
	                    PackageInvoiceApproval_Next_Designation_Id,
	                    PackageInvoiceApproval_Status,
	                    PackageInvoiceApproval_Status_Text = InvoiceStatus_Name,
	                    Designation_Current = Designation_Current.Designation_DesignationName, 
	                    Organisation_Current = Organisation_Current.OfficeBranch_Name, 
	                    Designation_Next = Designation_Next.Designation_DesignationName, 
	                    Organisation_Next = Organisation_Next.OfficeBranch_Name, 
	                    PackageInvoiceApproval_Comments,
	                    Person_Name, 
                        tDeduction.PackageInvoiceApproval_Deduction
                    from tbl_PackageInvoiceApproval 
                    left join tbl_InvoiceStatus on InvoiceStatus_Id = PackageInvoiceApproval_Status_Id
                    left join tbl_Designation Designation_Next on Designation_Next.Designation_Id = PackageInvoiceApproval_Next_Designation_Id
                    left join tbl_OfficeBranch Organisation_Next on Organisation_Next.OfficeBranch_Id = PackageInvoiceApproval_Next_Organisation_Id
                    join tbl_PersonDetail on Person_Id = PackageInvoiceApproval_AddedBy 
                    join tbl_PersonJuridiction on PersonJuridiction_PersonId = PackageInvoiceApproval_AddedBy
                    left join tbl_Designation Designation_Current on Designation_Current.Designation_Id = PersonJuridiction_DesignationId
                    left join tbl_OfficeBranch Organisation_Current on Organisation_Current.OfficeBranch_Id = Person_BranchOffice_Id
					left join 
					(
						select 
							PackageInvoice_Id, 
							PackageInvoiceAdditional_AddedBy, 
							SUM(ISNULL(PackageInvoiceAdditional_Deduction_Value_Final, 0)) PackageInvoiceApproval_Deduction
						from tbl_PackageInvoiceAdditional
						join tbl_PackageInvoice on PackageInvoice_Id = PackageInvoiceAdditional_Invoice_Id
						group by PackageInvoice_Id, PackageInvoiceAdditional_AddedBy 
					) tDeduction on tDeduction.PackageInvoice_Id = PackageInvoiceApproval_PackageInvoice_Id and PackageInvoiceApproval_AddedBy = PackageInvoiceAdditional_AddedBy
                    where PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_PackageInvoice_Id = '" + PackageInvoice_Id + "' order by PackageInvoiceApproval_Id";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_PackageEMBApproval_History(int PackageInvoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageEMBApproval_Id,
	                    PackageEMBApproval_Date = convert(char(10), PackageEMBApproval_Date, 103),
	                    PackageEMBApproval_PackageEMBMaster_Id,
                        PackageInvoice_Id,
	                    PackageEMBApproval_Package_Id,
						PackageEMBApproval_Status_Id,
						PackageEMBApproval_AddedBy,
						PackageEMBApproval_AddedOn,
	                    PackageEMBApproval_Next_Organisation_Id,
						PackageEMBApproval_Next_Designation_Id,
	                    PackageEMBApproval_Status,
	                    PackageEMBApproval_Status_Text = case when PackageEMBApproval_Status_Id = 0 then 'Pending' when PackageEMBApproval_Status_Id = 1 then 'Approved' when PackageEMBApproval_Status_Id = 2 then 'De-Escalate' when PackageEMBApproval_Status_Id = 3 then 'Rejected' else '-NA-' end,
	                    Designation_Current = Designation_Current.Designation_DesignationName, 
	                    Organisation_Current = Organisation_Current.OfficeBranch_Name, 
	                    Designation_Next = Designation_Next.Designation_DesignationName, 
	                    Organisation_Next = Organisation_Next.OfficeBranch_Name, 
	                    PackageEMBApproval_Comments,
	                    Person_Name
                    from tbl_PackageEMBApproval 
                    left join tbl_PackageInvoice on PackageInvoice_PackageEMBMaster_Id = PackageEMBApproval_PackageEMBMaster_Id
                    left join tbl_Designation Designation_Next on Designation_Next.Designation_Id = PackageEMBApproval_Next_Designation_Id
                    left join tbl_OfficeBranch Organisation_Next on Organisation_Next.OfficeBranch_Id = PackageEMBApproval_Next_Organisation_Id
                    left join tbl_PersonDetail on Person_Id = PackageEMBApproval_AddedBy 
                    left join tbl_PersonJuridiction on PersonJuridiction_PersonId = PackageEMBApproval_AddedBy
                    left join tbl_Designation Designation_Current on Designation_Current.Designation_Id = PersonJuridiction_DesignationId
                    left join tbl_OfficeBranch Organisation_Current on Organisation_Current.OfficeBranch_Id = Person_BranchOffice_Id
                    where PackageEMBApproval_Status = 1 and PackageInvoice_Id = '" + PackageInvoice_Id + "' order by PackageEMBApproval_Id";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    private void Insert_tbl_PackageInvoiceApproval(tbl_PackageInvoiceApproval obj_tbl_PackageInvoiceApproval, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoiceApproval ( [PackageInvoiceApproval_AddedBy],[PackageInvoiceApproval_AddedOn],[PackageInvoiceApproval_Comments],[PackageInvoiceApproval_Date],[PackageInvoiceApproval_Next_Designation_Id],[PackageInvoiceApproval_Next_Organisation_Id],[PackageInvoiceApproval_Package_Id],[PackageInvoiceApproval_PackageInvoice_Id],[PackageInvoiceApproval_Status],[PackageInvoiceApproval_Status_Id], [PackageInvoiceApproval_Step_Count]) values ('" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_AddedBy + "', getdate(), N'" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Comments + "', convert(date, '" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Date + "', 103),'" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id + "','" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id + "','" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Package_Id + "','" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id + "','" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status + "','" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Status_Id + "', '" + obj_tbl_PackageInvoiceApproval.PackageInvoiceApproval_Step_Count + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region Invoice Status
    public DataSet get_tbl_InvoiceStatus(int Org_Id, int Designation_Id, int ConfigMasterId, string Process_Name)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        if (Org_Id == 0 && Designation_Id == 0)
        {
            strQuery = @"set dateformat dmy; 
                        select 
                            InvoiceStatus_Id, 
                            InvoiceStatus_Name, 
                            InvoiceStatus_Status
                        from tbl_InvoiceStatus 
                        where InvoiceStatus_Status = 1 order by InvoiceStatus_Id";
        }
        else
        {
            strQuery = @"set dateformat dmy; 
                        select 
                            InvoiceStatus_Id, 
                            InvoiceStatus_Name, 
                            InvoiceStatus_Status
                        from tbl_InvoiceStatus 
					    join 
					    (
						    select 
							    ProcessConfigInvStatus_InvoiceStatus_Id 
						    from tbl_ProcessConfigInvStatus  
						    join tbl_ProcessConfigMaster on ProcessConfigMaster_Id = ProcessConfigInvStatus_ConfigMasterId 
						    where ProcessConfigMaster_Status = 1 and ProcessConfigMaster_Process_Name = '" + Process_Name + "' and ProcessConfigInvStatus_Status = 1 ConfigMasterIdCond) tStatus on tStatus.ProcessConfigInvStatus_InvoiceStatus_Id = InvoiceStatus_Id where InvoiceStatus_Status = 1 order by InvoiceStatus_Id";
        }
        if (ConfigMasterId > 0)
        {
            strQuery = strQuery.Replace("ConfigMasterIdCond", " and ProcessConfigInvStatus_ConfigMasterId = '" + ConfigMasterId + "' ");
        }
        else
        {
            strQuery = strQuery.Replace("ConfigMasterIdCond", " and ProcessConfigMaster_OrgId = '" + Org_Id + "' and (ProcessConfigMaster_Designation_Id = '" + Designation_Id + "' or ProcessConfigMaster_Designation_Id1='" + Designation_Id + "')");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Trade Document Master
    public DataSet get_tbl_TradeDocument()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        TradeDocument_Id, 
                        TradeDocument_Name, 
                        TradeDocument_AddedOn, 
                        TradeDocument_AddedBy, 
                        TradeDocument_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = TradeDocument_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = TradeDocument_ModifiedOn 
                    from tbl_TradeDocument
                    left join tbl_PersonDetail on Person_Id = TradeDocument_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = TradeDocument_ModifiedBy
                    where TradeDocument_Status = 1 order by TradeDocument_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_TradeDocument(int ConfigMasterId)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        TradeDocument_Id, 
                        TradeDocument_Name, 
                        TradeDocument_AddedOn, 
                        TradeDocument_AddedBy, 
                        TradeDocument_Status, 
                        ProcessConfigDocumentLinking_Id, 
                        ProcessConfigDocumentLinking_DocumentMaster_Id
                    from tbl_TradeDocument
                    join tbl_ProcessConfigDocumentLinking on ProcessConfigDocumentLinking_DocumentMaster_Id = TradeDocument_Id
                    where TradeDocument_Status = 1 and ProcessConfigDocumentLinking_Status = 1 and ProcessConfigDocumentLinking_ConfigMasterId = '" + ConfigMasterId + "' order by TradeDocument_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_TradeDocument(tbl_TradeDocument obj_tbl_TradeDocument, int TradeDocument_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyTradeDocument(obj_tbl_TradeDocument.TradeDocument_Name, TradeDocument_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (TradeDocument_Id == 0)
                {
                    Insert_tbl_TradeDocument(obj_tbl_TradeDocument, trans, cn);
                }
                else
                {
                    obj_tbl_TradeDocument.TradeDocument_Id = TradeDocument_Id;
                    Update_tbl_TradeDocument(obj_tbl_TradeDocument, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyTradeDocument(string TradeDocumentName, string TradeDocument_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_TradeDocument  where TradeDocument_Status = 1 and  TradeDocument_Name = '" + TradeDocumentName + "' ";
        if (TradeDocument_Id != "0")
        {
            strQuery += " AND TradeDocument_Id  <> '" + TradeDocument_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_TradeDocument(tbl_TradeDocument obj_tbl_TradeDocument, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_TradeDocument( [TradeDocument_AddedBy],[TradeDocument_AddedOn],[TradeDocument_Name],[TradeDocument_Status] ) values('" + obj_tbl_TradeDocument.TradeDocument_AddedBy + "', getdate(), N'" + obj_tbl_TradeDocument.TradeDocument_Name + "','" + obj_tbl_TradeDocument.TradeDocument_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_TradeDocument(tbl_TradeDocument obj_tbl_TradeDocument, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_TradeDocument set  TradeDocument_Name = N'" + obj_tbl_TradeDocument.TradeDocument_Name + "',TradeDocument_ModifiedOn=getdate(),TradeDocument_ModifiedBy = '" + obj_tbl_TradeDocument.TradeDocument_AddedBy + "' where TradeDocument_Id = '" + obj_tbl_TradeDocument.TradeDocument_Id + "' and TradeDocument_Status = '" + obj_tbl_TradeDocument.TradeDocument_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_TradeDocument(int TradeDocument_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_TradeDocument set   TradeDocument_Status = 0,TradeDocument_ModifiedBy='" + person_Id + "',TradeDocument_ModifiedOn=getdate() where TradeDocument_Id = '" + TradeDocument_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Report_Summery
    public DataSet get_Report_Summery_Details(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoice_Id, 
	                    PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    PackageInvoice_VoucherNo,
	                    PackageInvoice_Narration,
	                    tPackageInvoiceItem.Total_Amount,                         
						Vendor_Name = Person_Name, 
                        Vendor_Mobile = Person_Mobile1, 
                        Vendor_EmailId = Person_EmailId, 
                        Vendor_Address = Person_AddressLine1, 
                        Vendor_GSTIN = '',
                        Division_Name, 
                        ProjectWorkPkg_AgreementAmount,
                        Work_Amount = ProjectWorkPkg_AgreementAmount,
                        Tender_Cost = ProjectWorkPkg_AgreementAmount,
                        Tender_Cost_Less = convert(decimal(18, 2), ProjectWorkPkg_AgreementAmount *.8),
                        start_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        End_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103),
                        ProjectWorkPkg_Agreement_No, 
                        Project_Name, 
                        ProjectWorkPkg_Name,
                        ProjectWork_Name,
                        ProjectWork_Description, 
						tFunding.Central_Share, 
						tFunding.State_Share, 
						tFunding.ULB_Share, 
						Payment_Made_Earlier = 0,
						Payment_To_Jal_Nigam = 0, 
						Withheld_Amount = 0, 
						Extra_Item = '',
                        PackageInvoiceCover_Total_Payment_Earlier,
                        PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam,
                        PackageInvoiceCover_Total_Release,
                        PackageInvoiceCover_Total_With_Held_Amount,
                        PackageInvoiceCover_Total_Balance
                    from tbl_PackageInvoice 
                    join tbl_PackageInvoiceCover on PackageInvoice_Id = PackageInvoiceCover_Invoice_Id and PackageInvoiceCover_Status=1
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on ProjectWork_Project_Id = Project_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    left join tbl_PersonDetail on Person_Id = ProjectWorkPkg_Vendor_Id
					left join (select ProjectWorkFundingPattern_ProjectWorkId, Central_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 1 then ProjectWorkFundingPattern_Value else 0 end), State_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 2 then ProjectWorkFundingPattern_Value else 0 end), ULB_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 2 then ProjectWorkFundingPattern_Value else 0 end) from tbl_ProjectWorkFundingPattern where ProjectWorkFundingPattern_Status = 1 group by ProjectWorkFundingPattern_ProjectWorkId) tFunding on tFunding.ProjectWorkFundingPattern_ProjectWorkId = ProjectWork_Id
					left join 
                    (
	                    select 
		                    PackageInvoiceItem_Invoice_Id,
		                    Total_Line_Items = count(*),
		                    PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
		                    Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                    from tbl_PackageInvoiceItem
	                    where PackageInvoiceItem_Status = 1
	                    group by PackageInvoiceItem_Invoice_Id
                    ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
                    where PackageInvoice_Status = 1 and PackageInvoice_Id = Invoice_IdCond;";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_Report_Cover_Details(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoice_Id, 
	                    PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    PackageInvoice_VoucherNo,
	                    PackageInvoice_Narration,
	                    tPackageInvoiceItem.Total_Amount,                         
						Vendor_Name = Person_Name, 
                        Vendor_Mobile = Person_Mobile1, 
                        Vendor_EmailId = Person_EmailId, 
                        Vendor_Address = Person_AddressLine1, 
                        Vendor_GSTIN = '',
                        Division_Name, 
                        ProjectWorkPkg_AgreementAmount,
                        PackageInvoiceCover_SanctionedAmount,
                        PackageInvoiceCover_TenderAmount,
                        Tender_Cost_Less = convert(decimal(18, 2), PackageInvoiceCover_TenderAmount *.8),
                        start_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        End_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103),
                        ProjectWorkPkg_Agreement_No, 
                        Project_Name, 
                        ProjectWorkPkg_Name,
                        ProjectWork_Name,
						ProjectWork_ProjectCode,
                        ProjectWork_Description, 
						tFunding.Central_Share, 
						tFunding.State_Share, 
						tFunding.ULB_Share, 
						PackageInvoiceCover_Centage,
						PackageInvoiceCover_ReleaseTillDate,
						PackageInvoiceCover_DiversionOut,
						PackageInvoiceCover_DiversionIn,
						PackageInvoiceCover_PaymentTillDate,
						PackageInvoiceCover_MoblizationAdvance,
						PackageInvoiceCover_MoblizationAdvanceAdjustment,
                        PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill,
                        PackageInvoiceCover_Total_Invoice_Value,
                        PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion,
                        PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency,
                        PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement,
                        PackageInvoiceCover_Amount_Released_To_Division,
                        PackageInvoiceCover_Expenditure_By_Division,
                        PackageInvoiceCover_Total_Payment_Earlier,
                        PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam,
                        PackageInvoiceCover_Total_Release,
                        PackageInvoiceCover_Total_With_Held_Amount,
                        PackageInvoiceCover_Total_Balance,
						Withheld_Amount = 0, 
						Extra_Item = '',
                        ProjectType_Name
                    from tbl_PackageInvoice 
                    inner join (select distinct PackageInvoiceEMBMasterLink_Invoice_Id from tbl_PackageInvoiceEMBMasterLink where PackageInvoiceEMBMasterLink_Status=1)
                    tbl_PackageInvoiceEMBMasterLink on tbl_PackageInvoiceEMBMasterLink.PackageInvoiceEMBMasterLink_Invoice_Id=PackageInvoice_Id
                    join tbl_PackageInvoiceCover on PackageInvoice_Id = PackageInvoiceCover_Invoice_Id and PackageInvoiceCover_Status=1
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    left join tbl_ProjectType on ProjectType_Id=ProjectWork_ProjectType_Id
                    join tbl_Project on ProjectWork_Project_Id = Project_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    left join tbl_PersonDetail on Person_Id = ProjectWorkPkg_Vendor_Id
					left join (select ProjectWorkFundingPattern_ProjectWorkId, Central_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 1 then ProjectWorkFundingPattern_Value else 0 end), State_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 2 then ProjectWorkFundingPattern_Value else 0 end), ULB_Share = max(case when ProjectWorkFundingPattern_FundingPatternId = 2 then ProjectWorkFundingPattern_Value else 0 end) from tbl_ProjectWorkFundingPattern where ProjectWorkFundingPattern_Status = 1 group by ProjectWorkFundingPattern_ProjectWorkId) tFunding on tFunding.ProjectWorkFundingPattern_ProjectWorkId = ProjectWork_Id
					left join 
                    (
	                    select 
		                    PackageInvoiceItem_Invoice_Id,
		                    Total_Line_Items = count(*),
		                    PackageInvoiceItem_Total_Qty = sum(isnull(PackageInvoiceItem_Total_Qty_BOQ, 0)), 
		                    Total_Amount = sum(case when isnull(PackageInvoiceItem_PercentageToBeReleased, 100) = 100 then (convert(decimal(18, 2), isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0))) else convert(decimal(18, 2), (isnull(PackageInvoiceItem_RateQuoted, 0) * isnull(PackageInvoiceItem_Total_Qty_BOQ, 0) * isnull(PackageInvoiceItem_PercentageToBeReleased, 100) / 100)) end)
	                    from tbl_PackageInvoiceItem
	                    where PackageInvoiceItem_Status = 1
	                    group by PackageInvoiceItem_Invoice_Id
                    ) tPackageInvoiceItem on PackageInvoiceItem_Invoice_Id = PackageInvoice_Id
                    where PackageInvoice_Status = 1 and PackageInvoice_Id = Invoice_IdCond;";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_OfficeOrder(int Invoice_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                   select ProjectWork_Name,Year(ProjectWork_AddedOn) as ProjectWotkYear,ProjectWork_Budget,ProjectWork_WorkCost,
                ProjectWorkPkg_AgreementAmount,
                Isnull((select ProjectWorkFundingPattern_Value from tbl_ProjectWorkFundingPattern 
                where ProjectWorkFundingPattern_Status=1 and ProjectWorkFundingPattern_ProjectWorkId=ProjectWork_Id and ProjectWorkFundingPattern_FundingPatternId=1),0)
                 as CentralShare,
                 Isnull((select ProjectWorkFundingPattern_Value from tbl_ProjectWorkFundingPattern 
                where ProjectWorkFundingPattern_Status=1 and ProjectWorkFundingPattern_ProjectWorkId=ProjectWork_Id and ProjectWorkFundingPattern_FundingPatternId=2),0)
                 as StateShare,isnull(ProjectWork_Centage,0) as ProjectWork_Centage,
                  Isnull((select ProjectWorkFundingPattern_Value from tbl_ProjectWorkFundingPattern 
                where ProjectWorkFundingPattern_Status=1 and ProjectWorkFundingPattern_ProjectWorkId=ProjectWork_Id and ProjectWorkFundingPattern_FundingPatternId=2),0)
                 as ULBShare,'0' as Total
                from tbl_PackageInvoice
                inner join tbl_ProjectWorkPkg on ProjectWorkPkg_Id=PackageInvoice_Package_Id
                inner join tbl_ProjectWork on ProjectWork_Id=ProjectWorkPkg_Work_Id
                where PackageInvoice_Status=1 and PackageInvoice_Id=Invoice_IdCond
                union All
                select case when rr=1 then N'प्रथम धनराशि' when rr=2 then N'दिव्तीय धनराशि' when rr=3 then N'तिर्तीय धनराशि' else '' end ProjectWork_Name,
                ProjectWotkYear,ProjectWork_Budget,ProjectWork_WorkCost,ProjectWorkPkg_AgreementAmount,CentralShare,StateShare,ProjectWork_Centage,ULBShare,'0' as Total
                from (select ROW_NUMBER() Over(Order by ProjectWorkGO_Id asc) as rr,'' as ProjectWork_Name,'' as ProjectWotkYear,0 as ProjectWork_Budget,0 as ProjectWork_WorkCost,0 as ProjectWorkPkg_AgreementAmount,
                cast(ProjectWorkGO_CentralShare/100000 as decimal(18,2)) as CentralShare,
                cast(ProjectWorkGO_StateShare/100000 as decimal(18,2)) as StateShare,0 as ProjectWork_Centage,
                cast(ProjectWorkGO_ULBShare/100000 as decimal(18,2)) as ULBShare
                from tbl_PackageInvoice
                inner join tbl_ProjectWorkPkg on ProjectWorkPkg_Id=PackageInvoice_Package_Id
                inner join tbl_ProjectWorkGO on ProjectWorkGO_Work_Id=ProjectWorkPkg_Work_Id
                where PackageInvoice_Id=Invoice_IdCond and ProjectWorkGO_Status=1) as t ;";

        strQuery += @" select ProjectWork_Name,Year(ProjectWork_AddedOn) as ProjectWotkYear,ProjectWork_Budget,
                isnull(ProjectWork_Centage, 0) as ProjectWork_Centage,ProjectWork_WorkCost,
                convert(varchar(150), ProjectWork_WorkCost) + ' / ' + Convert(varchar(150), isnull(ProjectWork_Centage, 0)) as WorkCostWithCentage,
                ProjectWorkPkg_AgreementAmount,
                isnull(tbl_ProjectWorkGO1.CentralShare, 0) as CentralShare1,isnull(tbl_ProjectWorkGO1.StateShare, 0) as StateShare1,
                (isnull(tbl_ProjectWorkGO1.CentralShare, 0) + isnull(tbl_ProjectWorkGO1.StateShare, 0)) as Total1,
                isnull(tbl_ProjectWorkGO2.CentralShare, 0) as CentralShare2,isnull(tbl_ProjectWorkGO2.StateShare, 0) as StateShare2,
                (isnull(tbl_ProjectWorkGO2.CentralShare, 0) + isnull(tbl_ProjectWorkGO2.StateShare, 0)) as Total2,
                isnull(tbl_ProjectWorkGO3.CentralShare, 0) as CentralShare3,isnull(tbl_ProjectWorkGO2.StateShare, 0) as StateShare3,
                (isnull(tbl_ProjectWorkGO3.CentralShare, 0) + isnull(tbl_ProjectWorkGO3.StateShare, 0)) as Total3
                from tbl_PackageInvoice
                inner join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                inner join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                left
                join (
                select ROW_NUMBER() Over(Order by ProjectWorkGO_Id asc) as rr,ProjectWorkGO_Work_Id,
                cast(ProjectWorkGO_CentralShare / 100000 as decimal(18, 2)) as CentralShare,
                cast(ProjectWorkGO_StateShare / 100000 as decimal(18, 2)) as StateShare
                from tbl_ProjectWorkGO
                where ProjectWorkGO_Status = 1
                ) as tbl_ProjectWorkGO1 on tbl_ProjectWorkGO1.ProjectWorkGO_Work_Id = ProjectWork_Id and tbl_ProjectWorkGO1.rr = 1
                left join(
                select ROW_NUMBER() Over(Order by ProjectWorkGO_Id asc) as rr,ProjectWorkGO_Work_Id,
                cast(ProjectWorkGO_CentralShare / 100000 as decimal(18, 2)) as CentralShare,
                cast(ProjectWorkGO_StateShare / 100000 as decimal(18, 2)) as StateShare
                from tbl_ProjectWorkGO
                where ProjectWorkGO_Status = 1
                ) as tbl_ProjectWorkGO2 on tbl_ProjectWorkGO2.ProjectWorkGO_Work_Id = ProjectWork_Id and tbl_ProjectWorkGO2.rr = 2
                left join(
                select ROW_NUMBER() Over(Order by ProjectWorkGO_Id asc) as rr,ProjectWorkGO_Work_Id,
                cast(ProjectWorkGO_CentralShare / 100000 as decimal(18, 2)) as CentralShare,
                cast(ProjectWorkGO_StateShare / 100000 as decimal(18, 2)) as StateShare
                from tbl_ProjectWorkGO
                where ProjectWorkGO_Status = 1
                ) as tbl_ProjectWorkGO3 on tbl_ProjectWorkGO3.ProjectWorkGO_Work_Id = ProjectWork_Id and tbl_ProjectWorkGO3.rr = 3
                where PackageInvoice_Status = 1 and PackageInvoice_Id = Invoice_IdCond ";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Cover Letter
    public DataSet get_PackageInvoiceCover(int Invoice_Id, int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    PackageInvoiceCover_Id,
	                    PackageInvoiceCover_Invoice_Id,
                        PackageInvoiceCover_SanctionedAmount,
	                    PackageInvoiceCover_Centage,
	                    PackageInvoiceCover_TenderAmount,
	                    PackageInvoiceCover_CentralShare,
	                    PackageInvoiceCover_StateShare,
	                    PackageInvoiceCover_ULBShare,
	                    PackageInvoiceCover_ReleaseTillDate,
	                    PackageInvoiceCover_DiversionOut,
	                    PackageInvoiceCover_DiversionIn,
	                    PackageInvoiceCover_PaymentTillDate,
	                    PackageInvoiceCover_MoblizationAdvance,
	                    PackageInvoiceCover_MoblizationAdvanceAdjustment,
                        PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill,
                        PackageInvoiceCover_Total_Invoice_Value,
                        PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion,
                        PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency,
                        PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement,
                        PackageInvoiceCover_Amount_Released_To_Division,
                        PackageInvoiceCover_Expenditure_By_Division,
                        PackageInvoiceCover_Total_Payment_Earlier,
                        PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam,
                        PackageInvoiceCover_Total_Release,
                        PackageInvoiceCover_Total_With_Held_Amount,
                        PackageInvoiceCover_Total_Balance
                    from tbl_PackageInvoiceCover 
                    where PackageInvoiceCover_Status = 1 and PackageInvoiceCover_Invoice_Id = Invoice_IdCond; ";
        strQuery += Environment.NewLine;
        strQuery += @"select 
                        FinancialTrans_Id,
	                    FinancialTrans_Package_Id,
	                    FinancialTrans_TransAmount=convert(decimal(18,2), (isnull(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end, 0) / 100000)),
	                    FinancialTrans_GO_Date = convert(char(10), FinancialTrans_GO_Date, 103),
	                    FinancialTrans_GO_Number,
	                    FinancialTrans_FilePath1
                    from tbl_FinancialTrans
                    where FinancialTrans_Status = 1 and FinancialTrans_Package_Id = Package_IdCond; ";

        strQuery = strQuery.Replace("Invoice_IdCond", Invoice_Id.ToString());
        strQuery = strQuery.Replace("Package_IdCond", Package_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_PackageDetailsFillCoverLetter(int Work_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" select ProjectWork_WorkCost as PackageInvoiceCover_SanctionedAmount from tbl_ProjectWork where ProjectWork_Id='" + Work_Id + "' and ProjectWork_Status=1 ";
        strQuery += @" select ProjectWork_Centage as PackageInvoiceCover_Centage from tbl_ProjectWork where ProjectWork_Id='" + Work_Id + "' and ProjectWork_Status=1 ";
        strQuery += @" select sum(ProjectWorkPkg_AgreementAmount) as PackageInvoiceCover_TenderAmount from tbl_ProjectWorkPkg where ProjectWorkPkg_Work_Id='" + Work_Id + "' and ProjectWorkPkg_Status=1 ";
        strQuery += @" select ProjectWorkFundingPattern_Value as PackageInvoiceCover_CentralShare from tbl_ProjectWorkFundingPattern 
                        where ProjectWorkFundingPattern_ProjectWorkId='" + Work_Id + "' and ProjectWorkFundingPattern_FundingPatternId=1 and ProjectWorkFundingPattern_Status=1 ";
        strQuery += @" select ProjectWorkFundingPattern_Value as PackageInvoiceCover_StateShare from tbl_ProjectWorkFundingPattern 
                        where ProjectWorkFundingPattern_ProjectWorkId='" + Work_Id + "' and ProjectWorkFundingPattern_FundingPatternId=2 and ProjectWorkFundingPattern_Status=1 ";
        strQuery += @" select ProjectWorkFundingPattern_Value as PackageInvoiceCover_ULBShare from tbl_ProjectWorkFundingPattern 
                        where ProjectWorkFundingPattern_ProjectWorkId='" + Work_Id + "' and ProjectWorkFundingPattern_FundingPatternId=3 and ProjectWorkFundingPattern_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectWorkGO(int Work_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ProjectWorkGO_Id,
	                    ProjectWorkGO_Work_Id,
                        ProjectWorkGO_GO_Date=convert(char(10), ProjectWorkGO_GO_Date, 103),
                        ProjectWorkGO_GO_Number,
                        ProjectWorkGO_TotalRelease=convert(decimal(18,2), (isnull(ProjectWorkGO_TotalRelease,0) / 100000)),
	                    ProjectWorkGO_CentralShare=convert(decimal(18,2), (isnull(ProjectWorkGO_CentralShare,0) / 100000)),
	                    ProjectWorkGO_StateShare=convert(decimal(18,2), (isnull(ProjectWorkGO_StateShare,0) / 100000)),
	                    ProjectWorkGO_ULBShare=convert(decimal(18,2), (isnull(ProjectWorkGO_ULBShare,0) / 100000))
                    from tbl_ProjectWorkGO 
                    where ProjectWorkGO_Status = 1 and ProjectWorkGO_Work_Id = '" + Work_Id + "' ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_PackageInvoiceCover(tbl_PackageInvoiceCover obj_tbl_PackageInvoiceCover, List<tbl_FinancialTrans> obj_tbl_FinancialTrans_Li, int Package_Id, string Type, List<tbl_ProjectWorkGO> obj_tbl_ProjectWorkGO_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                bool isUpdate = false;
                string strQuery = " set dateformat dmy; select * from tbl_PackageInvoiceCover where PackageInvoiceCover_Status = 1 and PackageInvoiceCover_Invoice_Id = " + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id;
                ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                if (AllClasses.CheckDataSet(ds))
                {
                    isUpdate = true;
                }
                if (isUpdate && obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id > 0)
                {
                    Update_tbl_PackageInvoiceCover(obj_tbl_PackageInvoiceCover, obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id, obj_tbl_PackageInvoiceCover.PackageInvoiceCover_AddedBy, Type, trans, cn);
                }
                else
                {
                    Insert_tbl_PackageInvoiceCover(obj_tbl_PackageInvoiceCover, trans, cn);
                }

                if (obj_tbl_FinancialTrans_Li != null)
                {
                    for (int i = 0; i < obj_tbl_FinancialTrans_Li.Count; i++)
                    {
                        Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans_Li[i], trans, cn);
                    }
                }
                if (obj_tbl_ProjectWorkGO_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectWorkGO_Li.Count; i++)
                    {
                        if (obj_tbl_ProjectWorkGO_Li[i].ProjectWorkGO_Id > 0)
                        {
                            Update_tbl_ProjectWorkGO(obj_tbl_ProjectWorkGO_Li[i], trans, cn);
                        }
                        else
                        {
                            Insert_tbl_ProjectWorkGO(obj_tbl_ProjectWorkGO_Li[i], trans, cn);
                        }

                    }
                }
                //if (Type == "CoverLetter")
                //{

                //    Update_tbl_PackageInstallmentDetails(Package_Id, obj_tbl_PackageInvoiceCover.PackageInvoiceCover_AddedBy, trans, cn);
                //    for (int i = 0; i < obj_tbl_PackageInstallmentDetails_Li.Count; i++)
                //    {
                //        Insert_tbl_PackageInstallmentDetails(obj_tbl_PackageInstallmentDetails_Li[i], trans, cn);
                //    }
                //}
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }
    public bool Insert_PreviousInvoiceGenerate(tbl_PackageInvoice obj_tbl_PackageInvoice, tbl_FinancialTrans obj_tbl_FinancialTrans, List<tbl_PackageInvoiceAdditional> obj_tbl_PackageInvoiceAdditional_li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {

                obj_tbl_PackageInvoice.PackageInvoice_Id = Insert_tbl_PackageInvoice(obj_tbl_PackageInvoice, trans, cn);


                if (obj_tbl_PackageInvoiceAdditional_li != null)
                {
                    for (int i = 0; i < obj_tbl_PackageInvoiceAdditional_li.Count; i++)
                    {
                        obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                        string strQuery = "";
                        strQuery = " set dateformat dmy; Update  tbl_PackageInvoiceAdditional set   PackageInvoiceAdditional_ModifiedOn = getdate(),PackageInvoiceAdditional_ModifiedBy='" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_AddedBy + "',PackageInvoiceAdditional_Status=0 where PackageInvoiceAdditional_Invoice_Id = '" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Invoice_Id + "' and PackageInvoiceAdditional_Status=1 and PackageInvoiceAdditional_Deduction_Id='" + obj_tbl_PackageInvoiceAdditional_li[i].PackageInvoiceAdditional_Deduction_Id + "'";
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                        Insert_tbl_PackageInvoiceAdditional(obj_tbl_PackageInvoiceAdditional_li[i], trans, cn);
                    }
                }


                if (obj_tbl_FinancialTrans != null)
                {
                    obj_tbl_FinancialTrans.FinancialTrans_Invoice_Id = obj_tbl_PackageInvoice.PackageInvoice_Id;
                    Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans, trans, cn);

                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Insert_tbl_PackageInvoiceCover(tbl_PackageInvoiceCover obj_tbl_PackageInvoiceCover, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_PackageInvoiceCover ( [PackageInvoiceCover_AddedBy],[PackageInvoiceCover_AddedOn],[PackageInvoiceCover_Centage],[PackageInvoiceCover_CentralShare],[PackageInvoiceCover_DiversionIn],[PackageInvoiceCover_DiversionOut],[PackageInvoiceCover_Invoice_Id],[PackageInvoiceCover_MoblizationAdvance],[PackageInvoiceCover_MoblizationAdvanceAdjustment],[PackageInvoiceCover_PaymentTillDate],[PackageInvoiceCover_ReleaseTillDate],[PackageInvoiceCover_StateShare],[PackageInvoiceCover_Status],[PackageInvoiceCover_TenderAmount],[PackageInvoiceCover_ULBShare], [PackageInvoiceCover_SanctionedAmount],PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill,PackageInvoiceCover_Total_Invoice_Value,PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion,PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency,PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement,PackageInvoiceCover_Amount_Released_To_Division,PackageInvoiceCover_Expenditure_By_Division,PackageInvoiceCover_Total_Payment_Earlier,PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam,PackageInvoiceCover_Total_Release,PackageInvoiceCover_Total_With_Held_Amount,PackageInvoiceCover_Total_Balance) values ('" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_AddedBy + "', getdate(), '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Centage + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_CentralShare + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionIn + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionOut + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Invoice_Id + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvance + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvanceAdjustment + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_PaymentTillDate + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ReleaseTillDate + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_StateShare + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Status + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_TenderAmount + "','" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ULBShare + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_SanctionedAmount + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Invoice_Value + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Released_To_Division + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_By_Division + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Payment_Earlier + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Release + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_With_Held_Amount + "', '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Balance + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Update_tbl_PackageInvoiceCover(tbl_PackageInvoiceCover obj_tbl_PackageInvoiceCover, int Invoice_Id, int Added_By, string Type, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        if (Type == "CoverLetter")
        {
            strQuery = " set dateformat dmy;Update  tbl_PackageInvoiceCover set  PackageInvoiceCover_ModifiedBy = '" + Added_By + "' ,  PackageInvoiceCover_ModifiedOn =  getdate() , PackageInvoiceCover_Centage =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Centage + "', PackageInvoiceCover_CentralShare =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_CentralShare + "', PackageInvoiceCover_DiversionIn =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionIn + "', PackageInvoiceCover_DiversionOut =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_DiversionOut + "', PackageInvoiceCover_MoblizationAdvance =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvance + "', PackageInvoiceCover_MoblizationAdvanceAdjustment =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_MoblizationAdvanceAdjustment + "', PackageInvoiceCover_PaymentTillDate =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_PaymentTillDate + "', PackageInvoiceCover_ReleaseTillDate =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ReleaseTillDate + "', PackageInvoiceCover_StateShare =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_StateShare + "', PackageInvoiceCover_TenderAmount =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_TenderAmount + "', PackageInvoiceCover_ULBShare =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_ULBShare + "', PackageInvoiceCover_SanctionedAmount =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_SanctionedAmount + "', PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Mobelization_Advance_Adjustment_In_Current_Bill + "', PackageInvoiceCover_Total_Invoice_Value =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Invoice_Value + "', PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Received_To_Implementing_Agency_Including_Diversion + "', PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_Done_By_Implementing_Agency + "', PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Balance_Amount_As_In_Bank_Statement + "', PackageInvoiceCover_Amount_Released_To_Division =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Amount_Released_To_Division + "', PackageInvoiceCover_Expenditure_By_Division =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Expenditure_By_Division + "' where PackageInvoiceCover_Status =  '1' and PackageInvoiceCover_Invoice_Id = '" + Invoice_Id + "' ";
        }
        else if (Type == "SummaryInvoice")
        {
            strQuery = " set dateformat dmy;Update  tbl_PackageInvoiceCover set  PackageInvoiceCover_ModifiedBy = '" + Added_By + "' ,  PackageInvoiceCover_ModifiedOn =  getdate() , PackageInvoiceCover_Total_Payment_Earlier =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Payment_Earlier + "', PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Proposed_Payment_Jal_Nigam + "', PackageInvoiceCover_Total_Release =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Release + "', PackageInvoiceCover_Total_With_Held_Amount =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_With_Held_Amount + "', PackageInvoiceCover_Total_Balance =  '" + obj_tbl_PackageInvoiceCover.PackageInvoiceCover_Total_Balance + "' where PackageInvoiceCover_Status =  '1' and PackageInvoiceCover_Invoice_Id = '" + Invoice_Id + "' ";
        }

        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Insert_tbl_ProjectWorkGO(tbl_ProjectWorkGO obj_tbl_ProjectWorkGO, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";

        strQuery = " set dateformat dmy;insert into tbl_ProjectWorkGO ( [ProjectWorkGO_AddedBy],[ProjectWorkGO_AddedOn],[ProjectWorkGO_Work_Id],[ProjectWorkGO_GO_Date],[ProjectWorkGO_GO_Number],[ProjectWorkGO_TotalRelease],[ProjectWorkGO_CentralShare],[ProjectWorkGO_StateShare], [ProjectWorkGO_ULBShare], [ProjectWorkGO_Status]) values ('" + obj_tbl_ProjectWorkGO.ProjectWorkGO_AddedBy + "', getdate(), N'" + obj_tbl_ProjectWorkGO.ProjectWorkGO_Work_Id + "', convert(date, '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Date + "', 103), '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Number + "','" + obj_tbl_ProjectWorkGO.ProjectWorkGO_TotalRelease + "','" + obj_tbl_ProjectWorkGO.ProjectWorkGO_CentralShare + "','" + obj_tbl_ProjectWorkGO.ProjectWorkGO_StateShare + "', '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_ULBShare + "','" + obj_tbl_ProjectWorkGO.ProjectWorkGO_Status + "');Select @@Identity";


        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private void Update_tbl_ProjectWorkGO(tbl_ProjectWorkGO obj_tbl_ProjectWorkGO, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";

        strQuery = " set dateformat dmy;Update  tbl_ProjectWorkGO set  ProjectWorkGO_ModifiedBy = '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_AddedBy + "' ,  ProjectWorkGO_ModifiedOn =  getdate() , ProjectWorkGO_GO_Date =  convert(date, '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Date + "', 103), ProjectWorkGO_GO_Number =  '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_GO_Number + "', ProjectWorkGO_TotalRelease =  '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_TotalRelease + "', ProjectWorkGO_CentralShare =  '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_CentralShare + "', ProjectWorkGO_StateShare =  '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_StateShare + "', ProjectWorkGO_ULBShare =  '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_ULBShare + "' where ProjectWorkGO_Status =  '1' and ProjectWorkGO_Id = '" + obj_tbl_ProjectWorkGO.ProjectWorkGO_Id + "' ";


        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    #endregion

    #region Master TicketCategory
    public DataSet get_tbl_TicketCategory()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        TicketCategory_Id, 
                        TicketCategory_Name, 
                        TicketCategory_AddedOn, 
                        TicketCategory_AddedBy, 
                        TicketCategory_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = TicketCategory_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifyBy, 
                        Modify_Date = TicketCategory_ModifiedOn 
                      from tbl_TicketCategory
                      left join tbl_PersonDetail on Person_Id = TicketCategory_AddedBy
                      left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = TicketCategory_ModifiedBy
                      where TicketCategory_Status = 1 order by TicketCategory_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet Edit_tbl_TicketCategory(string TicketCategory_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " select TicketCategory_Id,TicketCategory_Name from tbl_TicketCategory where TicketCategory_Status=1 and TicketCategory_Id='" + TicketCategory_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_TicketCategory(tbl_TicketCategory obj_tbl_TicketCategory, int TicketCategory_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyTicketCategory(obj_tbl_TicketCategory.TicketCategory_Name, TicketCategory_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (TicketCategory_Id == 0)
                {
                    Insert_tbl_TicketCategory(obj_tbl_TicketCategory, trans, cn);
                }
                else
                {
                    obj_tbl_TicketCategory.TicketCategory_Id = TicketCategory_Id;
                    Update_tbl_TicketCategory(obj_tbl_TicketCategory, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyTicketCategory(string TicketCategoryName, string TicketCategory_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_TicketCategory  where TicketCategory_Status = 1 and  TicketCategory_Name = '" + TicketCategoryName + "' ";
        if (TicketCategory_Id != "0")
        {
            strQuery += " AND TicketCategory_Id  <> '" + TicketCategory_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_TicketCategory(tbl_TicketCategory obj_tbl_TicketCategory, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_TicketCategory( [TicketCategory_AddedBy],[TicketCategory_AddedOn],[TicketCategory_Name],[TicketCategory_Status] ) values('" + obj_tbl_TicketCategory.TicketCategory_AddedBy + "', getdate(), N'" + obj_tbl_TicketCategory.TicketCategory_Name + "','" + obj_tbl_TicketCategory.TicketCategory_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_TicketCategory(tbl_TicketCategory obj_tbl_TicketCategory, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_TicketCategory set  TicketCategory_Name = N'" + obj_tbl_TicketCategory.TicketCategory_Name + "',TicketCategory_ModifiedOn=getdate(),TicketCategory_ModifiedBy = '" + obj_tbl_TicketCategory.TicketCategory_AddedBy + "' where TicketCategory_Id = '" + obj_tbl_TicketCategory.TicketCategory_Id + "' and TicketCategory_Status = '" + obj_tbl_TicketCategory.TicketCategory_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_TicketCategory(int TicketCategory_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_TicketCategory set   TicketCategory_Status = 0 where TicketCategory_Id = '" + TicketCategory_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master TicketDetails
    public DataSet get_tbl_TicketDetails(int District_Id, int ULB_Id, int Project_Id, int Person_Id, string FromDate, string TillDate)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select TicketDetails_Id,
                    ProjectWork_Name,
                    tbl_PersonDetail.Person_Name as CreatedBy,
                    TicketDetails_AddedOn as CreatedDate,
                    tbl_PersonDetail1.Person_Name as ClosedBy,
                    TicketDetails_UpdatedOn as closedDate,
					Jurisdiction_Name_Eng,
					ULB_Name,
					TicketDetails_Description,
					TicketDetails_CloseDescription,
					TicketDetails_TicketStatus
                    from tbl_TicketDetails 
                    left join tbl_PersonDetail on Person_Id=TicketDetails_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id=TicketDetails_UpdatedBy
                    inner join tbl_ProjectWork on ProjectWork_Id=TicketDetails_ProjectWork_Id
                    inner join tbl_ProjectDPR on ProjectDPR_Id=TicketDetails_ProjectDPR_Id
                    inner join tbl_TicketCategory on TicketCategory_Id=TicketDetails_TicketCategoryId
					left join M_Jurisdiction on M_Jurisdiction_Id = ProjectDPR_District_Jurisdiction_Id
                    left join tbl_ULB on ULB_Id = ProjectDPR_NP_JurisdictionId
                    where TicketDetails_Status=1 ";
        if (District_Id != 0)
        {
            strQuery += " and ProjectDPR_District_Jurisdiction_Id = " + District_Id + "";
        }
        if (ULB_Id != 0)
        {
            strQuery += " and ProjectDPR_NP_JurisdictionId = " + ULB_Id + "";
        }
        if (Project_Id != 0)
        {
            strQuery += " and ProjectWork_Project_Id = " + Project_Id + "";
        }
        if (Person_Id != 0)
        {
            strQuery += " and (ProjectDPR_NP_JurisdictionId in (select PersonJuridiction_ULB_Id from tbl_PersonJuridiction where PersonJuridiction_PersonId = '" + Person_Id + "')  or ProjectDPR_NP_JurisdictionId in (select PersonAdditionalULB_ULB_Id from tbl_PersonAdditionalULB where PersonAdditionalULB_Person_Id = '" + Person_Id + "')) ";
        }
        if (!String.IsNullOrEmpty(FromDate) && !String.IsNullOrEmpty(TillDate))
        {
            strQuery += " and Convert(date, TicketDetails_AddedOn, 103)  between CONVERT(date, Convert(varchar(10),'" + FromDate + "', 103), 103) And CONVERT(date, Convert(varchar(10),'" + TillDate + "', 103), 103) ";
        }
        strQuery += " Order by TicketDetails_AddedOn desc";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Update_TicketDetails(int Ticket_Id, int person_Id, string Remark)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_TicketDetails set   TicketDetails_TicketStatus = 'Closed',TicketDetails_CloseDescription = N'" + Remark + "',TicketDetails_UpdatedBy='" + person_Id + "',TicketDetails_UpdatedOn=getdate() where TicketDetails_Id = '" + Ticket_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Set Inspection Master
    public DataSet get_tbl_SetInspectionMaster()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select * from (select ROW_NUMBER() Over(Partition by SetInspectionMaster_Name order by SetInspectionMaster_Id desc) as rr,
                        SetInspectionMaster_Id, 
                        SetInspectionMaster_Name,
                        SetInspectionMaster_Designation_Id, 
						SetInspectionMaster_MLevel_Id,
                        Level_Name,
                        Designation_DesignationName,
                        SetInspectionMaster_AddedOn, 
                        SetInspectionMaster_AddedBy, 
                        SetInspectionMaster_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = SetInspectionMaster_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=SetInspectionMaster_ModifiedOn 
                    from tbl_SetInspectionMaster 
					inner join M_Level on M_Level_Id=SetInspectionMaster_MLevel_Id
                    inner join tbl_Designation on Designation_Id=SetInspectionMaster_Designation_Id
                    left join tbl_PersonDetail on Person_Id = SetInspectionMaster_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = SetInspectionMaster_ModifiedBy 
                   where SetInspectionMaster_Status = 1 ) as t where rr=1 order by SetInspectionMaster_Id desc ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_SetInspectionMaster_BySetName(string SetName)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select SetInspectionMaster_Designation_Id,
                    SetInspectionMaster_MLevel_Id,
                    Designation_DesignationName,
                    SetInspectionMaster_Name,
                    Level_Name
                    from tbl_SetInspectionMaster 
                    inner join M_Level on M_Level_Id=SetInspectionMaster_MLevel_Id
                     inner join tbl_Designation on Designation_Id=SetInspectionMaster_Designation_Id
                    where SetInspectionMaster_Name='" + SetName + "' and SetInspectionMaster_Status=1  order by SetInspectionMaster_Id desc";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_M_Level_SetInspection()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; select M_Level_Id, Level_Name, Created_By, Created_Date, Is_Active from M_Level where Is_Active = 1 and M_Level_Id in (6,7,8)  order by M_Level_Id";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_SetInspectionMaster(List<tbl_SetInspectionMaster> obj_tbl_SetInspectionMasterLi, int SetInspectionMaster_Id, string SetName, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {

                if (obj_tbl_SetInspectionMasterLi != null && obj_tbl_SetInspectionMasterLi.Count > 0)
                {
                    string strQuery = " set dateformat dmy; Update  tbl_SetInspectionMaster set  SetInspectionMaster_Status =  '0' , SetInspectionMaster_ModifiedBy = '" + obj_tbl_SetInspectionMasterLi[0].SetInspectionMaster_AddedBy + "' ,  SetInspectionMaster_ModifiedOn =  getdate() where SetInspectionMaster_Name = '" + obj_tbl_SetInspectionMasterLi[0].SetInspectionMaster_Name + "' and SetInspectionMaster_Status =  '1' ";
                    if (trans == null)
                    {
                        try
                        {
                            ExecuteSelectQuery(strQuery);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
                    }

                    if (AllClasses.CheckDataSet(CheckDuplicacySetInspectionMaster(SetName, SetInspectionMaster_Id.ToString(), trans, cn)))
                    {
                        Msg = "A";
                        trans.Commit();
                        cn.Close();
                        return false;
                    }


                    for (int i = 0; i < obj_tbl_SetInspectionMasterLi.Count; i++)
                    {
                        Insert_tbl_SetInspectionMaster(obj_tbl_SetInspectionMasterLi[i], trans, cn);
                    }
                }

                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacySetInspectionMaster(string SetInspectionMaster_Name, string SetInspectionMaster_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_SetInspectionMaster  where SetInspectionMaster_Status = 1 and  SetInspectionMaster_Name = '" + SetInspectionMaster_Name + "' ";
        //if (SetInspectionMaster_Id != "0")
        //{
        //    strQuery += " AND SetInspectionMaster_Id  <> '" + SetInspectionMaster_Id + "'";
        //}
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_SetInspectionMaster(tbl_SetInspectionMaster obj_tbl_SetInspectionMaster, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_SetInspectionMaster( [SetInspectionMaster_AddedBy],[SetInspectionMaster_AddedOn],[SetInspectionMaster_Name],[SetInspectionMaster_Status],SetInspectionMaster_Designation_Id,SetInspectionMaster_MLevel_Id ) values('" + obj_tbl_SetInspectionMaster.SetInspectionMaster_AddedBy + "', getdate(), N'" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Name + "','" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Status + "','" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Designation_Id + "','" + obj_tbl_SetInspectionMaster.SetInspectionMaster_MLevel_Id + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_SetInspectionMaster(tbl_SetInspectionMaster obj_tbl_SetInspectionMaster, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_SetInspectionMaster set  SetInspectionMaster_Name = N'" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Name + "',SetInspectionMaster_ModifiedOn = getDate(),SetInspectionMaster_ModifiedBy = '" + obj_tbl_SetInspectionMaster.SetInspectionMaster_AddedBy + "',SetInspectionMaster_Designation_Id = '" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Designation_Id + "',SetInspectionMaster_MLevel_Id = '" + obj_tbl_SetInspectionMaster.SetInspectionMaster_MLevel_Id + "' where SetInspectionMaster_Id = '" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Id + "' and SetInspectionMaster_Status = '" + obj_tbl_SetInspectionMaster.SetInspectionMaster_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_SetInspectionMaster(string SetInspectionMaster_Name, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_SetInspectionMaster set   SetInspectionMaster_Status = 0,SetInspectionMaster_ModifiedOn = getDate(),SetInspectionMaster_ModifiedBy = '" + person_Id + "' where SetInspectionMaster_Name = '" + SetInspectionMaster_Name + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Physical Progress Component
    public DataSet get_tbl_PhysicalProgressComponent()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        PhysicalProgressComponent_Id, 
                        PhysicalProgressComponent_Unit_Id,
                        PhysicalProgressComponent_Component, 
						Unit_Name,
                        PhysicalProgressComponent_AddedOn, 
                        PhysicalProgressComponent_AddedBy, 
                        PhysicalProgressComponent_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = PhysicalProgressComponent_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=PhysicalProgressComponent_ModifiedOn ,
                        0 as ProjectPkg_PhysicalProgress_Id
                    from tbl_PhysicalProgressComponent 
					left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
                    left join tbl_PersonDetail on Person_Id = PhysicalProgressComponent_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = PhysicalProgressComponent_ModifiedBy 
                    where PhysicalProgressComponent_Status = 1 order by PhysicalProgressComponent_Id desc";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_PhysicalProgressComponent(tbl_PhysicalProgressComponent obj_tbl_PhysicalProgressComponent, int PhysicalProgressComponent_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyPhysicalProgressComponent(obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Component, PhysicalProgressComponent_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (PhysicalProgressComponent_Id == 0)
                {
                    Insert_tbl_PhysicalProgressComponent(obj_tbl_PhysicalProgressComponent, trans, cn);
                }
                else
                {
                    obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Id = PhysicalProgressComponent_Id;
                    Update_tbl_PhysicalProgressComponent(obj_tbl_PhysicalProgressComponent, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyPhysicalProgressComponent(string PhysicalProgressComponent_Component, string PhysicalProgressComponent_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_PhysicalProgressComponent  where PhysicalProgressComponent_Status = 1 and  PhysicalProgressComponent_Component = '" + PhysicalProgressComponent_Component + "' ";
        if (PhysicalProgressComponent_Id != "0")
        {
            strQuery += " AND PhysicalProgressComponent_Id  <> '" + PhysicalProgressComponent_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_PhysicalProgressComponent(tbl_PhysicalProgressComponent obj_tbl_PhysicalProgressComponent, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_PhysicalProgressComponent( [PhysicalProgressComponent_AddedBy],[PhysicalProgressComponent_AddedOn],[PhysicalProgressComponent_Component],[PhysicalProgressComponent_Status],PhysicalProgressComponent_Unit_Id ) values('" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_AddedBy + "', getdate(), N'" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Component + "','" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Status + "','" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Unit_Id + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_PhysicalProgressComponent(tbl_PhysicalProgressComponent obj_tbl_PhysicalProgressComponent, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_PhysicalProgressComponent set  PhysicalProgressComponent_Component = N'" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Component + "',PhysicalProgressComponent_ModifiedOn = getDate(),PhysicalProgressComponent_ModifiedBy = '" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_AddedBy + "',PhysicalProgressComponent_Unit_Id = '" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Unit_Id + "' where PhysicalProgressComponent_Id = '" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Id + "' and PhysicalProgressComponent_Status = '" + obj_tbl_PhysicalProgressComponent.PhysicalProgressComponent_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_PhysicalProgressComponent(int PhysicalProgressComponent_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_PhysicalProgressComponent set   PhysicalProgressComponent_Status = 0,PhysicalProgressComponent_ModifiedOn = getDate(),PhysicalProgressComponent_ModifiedBy = '" + person_Id + "' where PhysicalProgressComponent_Id = '" + PhysicalProgressComponent_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Deliverables
    public DataSet get_tbl_Deliverables()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        Deliverables_Id, 
                        Deliverables_Unit_Id,
                        Deliverables_Deliverables, 
						Unit_Name,
                        Deliverables_AddedOn, 
                        Deliverables_AddedBy, 
                        Deliverables_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = Deliverables_AddedOn, 
                        tbl_PersonDetail1.Person_Name as ModifiedBy, 
                        Mdified_Date=Deliverables_ModifiedOn,
                        0 as ProjectPkg_Deliverables_Id
                    from tbl_Deliverables 
					left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                    left join tbl_PersonDetail on Person_Id = Deliverables_AddedBy 
                    left join tbl_PersonDetail as tbl_PersonDetail1  on tbl_PersonDetail1.Person_Id = Deliverables_ModifiedBy 
                    where Deliverables_Status = 1 order by Deliverables_Id desc";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_Deliverables(tbl_Deliverables obj_tbl_Deliverables, int Deliverables_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDeliverables(obj_tbl_Deliverables.Deliverables_Deliverables, Deliverables_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (Deliverables_Id == 0)
                {
                    Insert_tbl_Deliverables(obj_tbl_Deliverables, trans, cn);
                }
                else
                {
                    obj_tbl_Deliverables.Deliverables_Id = Deliverables_Id;
                    Update_tbl_Deliverables(obj_tbl_Deliverables, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDeliverables(string Deliverables_Deliverables, string Deliverables_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_Deliverables  where Deliverables_Status = 1 and  Deliverables_Deliverables = '" + Deliverables_Deliverables + "' ";
        if (Deliverables_Id != "0")
        {
            strQuery += " AND Deliverables_Id  <> '" + Deliverables_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_Deliverables(tbl_Deliverables obj_tbl_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Deliverables( [Deliverables_AddedBy],[Deliverables_AddedOn],[Deliverables_Deliverables],[Deliverables_Status],Deliverables_Unit_Id ) values('" + obj_tbl_Deliverables.Deliverables_AddedBy + "', getdate(), N'" + obj_tbl_Deliverables.Deliverables_Deliverables + "','" + obj_tbl_Deliverables.Deliverables_Status + "','" + obj_tbl_Deliverables.Deliverables_Unit_Id + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_Deliverables(tbl_Deliverables obj_tbl_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_Deliverables set  Deliverables_Deliverables = N'" + obj_tbl_Deliverables.Deliverables_Deliverables + "',Deliverables_ModifiedOn = getDate(),Deliverables_ModifiedBy = '" + obj_tbl_Deliverables.Deliverables_AddedBy + "',Deliverables_Unit_Id = '" + obj_tbl_Deliverables.Deliverables_Unit_Id + "' where Deliverables_Id = '" + obj_tbl_Deliverables.Deliverables_Id + "' and Deliverables_Status = '" + obj_tbl_Deliverables.Deliverables_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_Deliverables(int Deliverables_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_Deliverables set   Deliverables_Status = 0,Deliverables_ModifiedOn = getDate(),Deliverables_ModifiedBy = '" + person_Id + "' where Deliverables_Id = '" + Deliverables_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master DPR_Status
    public DataSet get_tbl_DPR_Status()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        DPR_Status_Id, 
                        DPR_Status_DPR_StatusName, 
                        DPR_Status_AddedBy, 
                        DPR_Status_AddedOn, 
                        DPR_Status_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = DPR_Status_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Modified_Date = DPR_Status_ModifiedOn
                    from tbl_DPR_Status
                    left join tbl_PersonDetail on Person_Id = DPR_Status_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = DPR_Status_ModifiedBy
                    where DPR_Status_Status = 1 order by DPR_Status_DPR_StatusName";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_DPR_Status(tbl_DPR_Status obj_tbl_DPR_Status, int DPR_Status_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyDPR_Status(obj_tbl_DPR_Status.DPR_Status_DPR_StatusName, DPR_Status_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (DPR_Status_Id == 0)
                {
                    Insert_tbl_DPR_Status(obj_tbl_DPR_Status, trans, cn);
                }
                else
                {
                    obj_tbl_DPR_Status.DPR_Status_Id = DPR_Status_Id;
                    Update_tbl_DPR_Status(obj_tbl_DPR_Status, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyDPR_Status(string DPR_StatusName, string DPR_Status_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_DPR_Status  where DPR_Status_Status = 1 and  DPR_Status_DPR_StatusName = '" + DPR_StatusName + "' ";
        if (DPR_Status_Id != "0")
        {
            strQuery += " AND DPR_Status_Id  <> '" + DPR_Status_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_DPR_Status(tbl_DPR_Status obj_tbl_DPR_Status, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_DPR_Status( [DPR_Status_AddedBy],[DPR_Status_AddedOn],[DPR_Status_DPR_StatusName],[DPR_Status_Status]) values('" + obj_tbl_DPR_Status.DPR_Status_AddedBy + "', getdate(), N'" + obj_tbl_DPR_Status.DPR_Status_DPR_StatusName + "','" + obj_tbl_DPR_Status.DPR_Status_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_DPR_Status(tbl_DPR_Status obj_tbl_DPR_Status, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_DPR_Status set  DPR_Status_DPR_StatusName = N'" + obj_tbl_DPR_Status.DPR_Status_DPR_StatusName + "', DPR_Status_ModifiedOn = getDate(), DPR_Status_ModifiedBy = '" + obj_tbl_DPR_Status.DPR_Status_AddedBy + "' where DPR_Status_Id = '" + obj_tbl_DPR_Status.DPR_Status_Id + "' and DPR_Status_Status = '" + obj_tbl_DPR_Status.DPR_Status_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_DPR_Status(int DPR_Status_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_DPR_Status set   DPR_Status_Status = 0 where DPR_Status_Id = '" + DPR_Status_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master ADP_Category
    public DataSet get_tbl_ADP_Category()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        ADP_Category_Id, 
                        ADP_Category_Name, 
                        ADP_Category_AddedBy, 
                        ADP_Category_AddedOn, 
                        ADP_Category_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = ADP_Category_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Modified_Date = ADP_Category_ModifiedOn
                    from tbl_ADP_Category
                    left join tbl_PersonDetail on Person_Id = ADP_Category_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = ADP_Category_ModifiedBy
                    where ADP_Category_Status = 1 order by ADP_Category_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_ADP_Category(tbl_ADP_Category obj_tbl_ADP_Category, int ADP_Category_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (AllClasses.CheckDataSet(CheckDuplicacyADP_Category(obj_tbl_ADP_Category.ADP_Category_Name, ADP_Category_Id.ToString(), trans, cn)))
                {
                    Msg = "A";
                    trans.Commit();
                    cn.Close();
                    return false;
                }
                if (ADP_Category_Id == 0)
                {
                    Insert_tbl_ADP_Category(obj_tbl_ADP_Category, trans, cn);
                }
                else
                {
                    obj_tbl_ADP_Category.ADP_Category_Id = ADP_Category_Id;
                    Update_tbl_ADP_Category(obj_tbl_ADP_Category, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private DataSet CheckDuplicacyADP_Category(string ADP_Category_Name, string ADP_Category_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = " set dateformat dmy; Select  * from tbl_ADP_Category  where ADP_Category_Status = 1 and  ADP_Category_Name = '" + ADP_Category_Name + "' ";
        if (ADP_Category_Id != "0")
        {
            strQuery += " AND ADP_Category_Id  <> '" + ADP_Category_Id + "'";
        }
        if (trans == null)
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return ds;
    }

    private string Insert_tbl_ADP_Category(tbl_ADP_Category obj_tbl_ADP_Category, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_ADP_Category( [ADP_Category_AddedBy],[ADP_Category_AddedOn],[ADP_Category_Name],[ADP_Category_Status]) values('" + obj_tbl_ADP_Category.ADP_Category_AddedBy + "', getdate(), N'" + obj_tbl_ADP_Category.ADP_Category_Name + "','" + obj_tbl_ADP_Category.ADP_Category_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_ADP_Category(tbl_ADP_Category obj_tbl_ADP_Category, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ADP_Category set  ADP_Category_Name = N'" + obj_tbl_ADP_Category.ADP_Category_Name + "', ADP_Category_ModifiedOn = getDate(), ADP_Category_ModifiedBy = '" + obj_tbl_ADP_Category.ADP_Category_AddedBy + "' where ADP_Category_Id = '" + obj_tbl_ADP_Category.ADP_Category_Id + "' and ADP_Category_Status = '" + obj_tbl_ADP_Category.ADP_Category_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_ADP_Category(int ADP_Category_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ADP_Category set   ADP_Category_Status = 0,ADP_Category_ModifiedOn=getdate(),ADP_Category_ModifiedBy='" + person_Id + "' where ADP_Category_Id = '" + ADP_Category_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master ADP_Item
    public DataSet get_tbl_ADP_Item()
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        ADP_Item_Id, 
                        ADP_Item_Category_Id,
                        ADP_Item_Unit_Id,
                        ADP_Category_Name,
                        Unit_Name,
                        ADP_Item_Specification,
                        ADP_Item_Rate,
                        ADP_Item_Qty,
                        ADP_Item_AddedBy, 
                        ADP_Item_AddedOn, 
                        ADP_Item_Status, 
                        isnull(tbl_PersonDetail.Person_Name, 'Backend Entry') CreatedBy, 
                        Created_Date = ADP_Item_AddedOn,
                        tbl_PersonDetail1.Person_Name as ModifiedBy,
                        Modified_Date = ADP_Item_ModifiedOn
                    from tbl_ADP_Item
                    inner join tbl_ADP_Category on ADP_Category_Id=ADP_Item_Category_Id
                    left join tbl_Unit on Unit_Id=ADP_Item_Unit_Id
                    left join tbl_PersonDetail on Person_Id = ADP_Item_AddedBy
                    left join tbl_PersonDetail as tbl_PersonDetail1 on tbl_PersonDetail1.Person_Id = ADP_Item_ModifiedBy
                    where ADP_Item_Status = 1 order by ADP_Item_Id desc";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ADP_Item_PackageWise(int Package_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        ADP_Item_Id, 
                        Package_ADP_Item_Id=Isnull(Package_ADP_Item_Id,0),
                        ADP_Item_Category_Id,
                        ADP_Item_Unit_Id,
                        ADP_Category_Name,
                        Unit_Name,
                        ADP_Item_Specification,
                        ADP_Item_Rate=Isnull(Package_ADP_Item_Rate,ADP_Item_Rate),
                        ADP_Item_Qty=Isnull(Package_ADP_Item_Qty,ADP_Item_Qty)
                    from tbl_ADP_Item
                    inner join tbl_ADP_Category on ADP_Category_Id=ADP_Item_Category_Id
                    left join tbl_Unit on Unit_Id=ADP_Item_Unit_Id
                    left join tbl_Package_ADP_Item on Package_ADP_Item_Item_Id=ADP_Item_Id and Package_ADP_Item_Status=1 and Package_ADP_Item_Package_Id=CondPackage_ADP_Item_Package_Id
                    where ADP_Item_Status = 1 order by ADP_Item_Id desc";
        strQuery = strQuery.Replace("CondPackage_ADP_Item_Package_Id", Package_Id.ToString());
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public bool Insert_tbl_ADP_Item(tbl_ADP_Item obj_tbl_ADP_Item, int ADP_Item_Id, ref string Msg)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (ADP_Item_Id == 0)
                {
                    Insert_tbl_ADP_Item(obj_tbl_ADP_Item, trans, cn);
                }
                else
                {
                    obj_tbl_ADP_Item.ADP_Item_Id = ADP_Item_Id;
                    Update_tbl_ADP_Item(obj_tbl_ADP_Item, trans, cn);
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }


    private string Insert_tbl_ADP_Item(tbl_ADP_Item obj_tbl_ADP_Item, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_ADP_Item( [ADP_Item_AddedBy],[ADP_Item_AddedOn],[ADP_Item_Category_Id],ADP_Item_Specification,ADP_Item_Unit_Id,ADP_Item_Rate,ADP_Item_Qty,[ADP_Item_Status]) values('" + obj_tbl_ADP_Item.ADP_Item_AddedBy + "', getdate(), N'" + obj_tbl_ADP_Item.ADP_Item_Category_Id + "',N'" + obj_tbl_ADP_Item.ADP_Item_Specification + "','" + obj_tbl_ADP_Item.ADP_Item_Unit_Id + "','" + obj_tbl_ADP_Item.ADP_Item_Rate + "','" + obj_tbl_ADP_Item.ADP_Item_Qty + "','" + obj_tbl_ADP_Item.ADP_Item_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString();
        }
    }

    private void Update_tbl_ADP_Item(tbl_ADP_Item obj_tbl_ADP_Item, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;Update  tbl_ADP_Item set  ADP_Item_Category_Id='" + obj_tbl_ADP_Item.ADP_Item_Category_Id + "',ADP_Item_Specification = N'" + obj_tbl_ADP_Item.ADP_Item_Specification + "',ADP_Item_Unit_Id='" + obj_tbl_ADP_Item.ADP_Item_Unit_Id + "',ADP_Item_Rate='" + obj_tbl_ADP_Item.ADP_Item_Rate + "',ADP_Item_Qty='" + obj_tbl_ADP_Item.ADP_Item_Qty + "', ADP_Item_ModifiedOn = getDate(), ADP_Item_ModifiedBy = '" + obj_tbl_ADP_Item.ADP_Item_AddedBy + "' where ADP_Item_Id = '" + obj_tbl_ADP_Item.ADP_Item_Id + "' and ADP_Item_Status = '" + obj_tbl_ADP_Item.ADP_Item_Status + "' ";
        if (trans == null)
        {
            ExecuteSelectQuery(strQuery);
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Delete_tbl_ADP_Item(int ADP_Item_Id, int person_Id)
    {
        try
        {
            string strQuery = "";
            strQuery = " set dateformat dmy; Update  tbl_ADP_Item set   ADP_Item_Status = 0,ADP_Item_ModifiedBy='" + person_Id + "',ADP_Item_ModifiedOn=getdate() where ADP_Item_Id = '" + ADP_Item_Id + "' ";
            ExecuteSelectQuery(strQuery);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Master Package_ADP_Item
    public bool Insert_tbl_Package_ADP_Item(List<tbl_Package_ADP_Item> obj_tbl_Package_ADP_Item_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string strQuery = "";
                strQuery = " set dateformat dmy; Update  tbl_Package_ADP_Item set   Package_ADP_Item_Status = 0,Package_ADP_Item_ModifiedBy='" + obj_tbl_Package_ADP_Item_Li[0].Package_ADP_Item_AddedBy + "',Package_ADP_Item_ModifiedOn=getdate() where Package_ADP_Item_Package_Id = '" + obj_tbl_Package_ADP_Item_Li[0].Package_ADP_Item_Package_Id + "' ";
                strQuery += " set dateformat dmy; Update  tbl_Package_ADP_Item_Tax set   Package_ADP_Item_Tax_Status = 0,Package_ADP_Item_Tax_ModifiedBy='" + obj_tbl_Package_ADP_Item_Li[0].Package_ADP_Item_AddedBy + "',Package_ADP_Item_Tax_ModifiedOn=getdate() where Package_ADP_Item_Tax_Package_Id = '" + obj_tbl_Package_ADP_Item_Li[0].Package_ADP_Item_Package_Id + "' ";
                ExecuteSelectQuerywithTransaction(cn, strQuery, trans);

                if (obj_tbl_Package_ADP_Item_Li != null)
                {
                    for (int i = 0; i < obj_tbl_Package_ADP_Item_Li.Count; i++)
                    {
                        obj_tbl_Package_ADP_Item_Li[i].Package_ADP_Item_Id = Insert_tbl_Package_ADP_Item(obj_tbl_Package_ADP_Item_Li[i], trans, cn);
                        if (obj_tbl_Package_ADP_Item_Li[i].tbl_Package_ADP_Item_Tax != null)
                        {
                            for (int k = 0; k < obj_tbl_Package_ADP_Item_Li[i].tbl_Package_ADP_Item_Tax.Count; k++)
                            {
                                obj_tbl_Package_ADP_Item_Li[i].tbl_Package_ADP_Item_Tax[k].Package_ADP_Item_Tax_Package_ADP_Item_Id = obj_tbl_Package_ADP_Item_Li[i].Package_ADP_Item_Id;
                                Insert_tbl_Package_ADP_Item_Tax(obj_tbl_Package_ADP_Item_Li[i].tbl_Package_ADP_Item_Tax[k], trans, cn);
                            }
                        }
                    }

                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private int Insert_tbl_Package_ADP_Item(tbl_Package_ADP_Item obj_tbl_Package_ADP_Item, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_Package_ADP_Item( [Package_ADP_Item_AddedBy],[Package_ADP_Item_AddedOn],[Package_ADP_Item_Item_Id],Package_ADP_Item_Package_Id,Package_ADP_Item_Category_Id,Package_ADP_Item_Specification,Package_ADP_Item_Unit_Id,[Package_ADP_Item_Rate],Package_ADP_Item_Qty,Package_ADP_Item_Status) values('" + obj_tbl_Package_ADP_Item.Package_ADP_Item_AddedBy + "', getdate(), N'" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Item_Id + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Package_Id + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Category_Id + "',N'" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Specification + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Unit_Id + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Rate + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Qty + "','" + obj_tbl_Package_ADP_Item.Package_ADP_Item_Status + "'); Select @@Identity";

        if (trans == null)
        {
            return Convert.ToInt32(ExecuteSelectQuery(strQuery).Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return Convert.ToInt32(ExecuteSelectQuerywithTransaction(cn, strQuery, trans).Tables[0].Rows[0][0].ToString());
        }
    }
    private void Insert_tbl_Package_ADP_Item_Tax(tbl_Package_ADP_Item_Tax obj_tbl__Package_ADP_Item_Tax, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl__Package_ADP_Item_Tax( [_Package_ADP_Item_Tax_AddedBy],[_Package_ADP_Item_Tax_AddedOn],[Package_ADP_Item_Tax_Package_ADP_Item_Id],Package_ADP_Item_Tax_Package_Id,Package_ADP_Item_Tax_Deduction_Id,Package_ADP_Item_Tax_Value,Package_ADP_Item_Tax_Status) values('" + obj_tbl__Package_ADP_Item_Tax.Package_ADP_Item_Tax_AddedBy + "', getdate(), '" + obj_tbl__Package_ADP_Item_Tax.Package_ADP_Item_Tax_Package_ADP_Item_Id + "','" + obj_tbl__Package_ADP_Item_Tax.Package_ADP_Item_Tax_Package_Id + "','" + obj_tbl__Package_ADP_Item_Tax.Package_ADP_Item_Tax_Value + "','" + obj_tbl__Package_ADP_Item_Tax.Package_ADP_Item_Tax_Status + "');";

        if (trans == null)
        {
            Convert.ToInt32(ExecuteSelectQuery(strQuery));
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region DPR Questionire
    public DataSet get_tbl_DPRQuestionnaire(int Project_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        DPRQuestionnaire_Id,
                        DPRQuestionnaire_ProjectId,
                        DPRQuestionnaire_Name,
                        DPRQuestionnaire_AddedBy,
                        DPRQuestionnaire_AddedOn,
                        DPRQuestionnaire_Status
                    from tbl_DPRQuestionnaire 
                    where DPRQuestionnaire_Status = 1 ";
        if (Project_Id > 0)
        {
            strQuery += " and DPRQuestionnaire_ProjectId = '" + Project_Id + "'";
        }
        strQuery += " order by DPRQuestionnaire_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public bool Insert_tbl_DPRQuestionnaire(List<tbl_DPRQuestionnaire> obj_tbl_DPRQuestionnaire_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Update_tbl_DPRQuestionnaire(obj_tbl_DPRQuestionnaire_Li[0].DPRQuestionnaire_AddedBy, obj_tbl_DPRQuestionnaire_Li[0].DPRQuestionnaire_ProjectId, trans, cn);
                if (obj_tbl_DPRQuestionnaire_Li != null && obj_tbl_DPRQuestionnaire_Li.Count > 0)
                {
                    for (int i = 0; i < obj_tbl_DPRQuestionnaire_Li.Count; i++)
                    {
                        obj_tbl_DPRQuestionnaire_Li[i].DPRQuestionnaire_Id = Insert_tbl_DPRQuestionnaire(obj_tbl_DPRQuestionnaire_Li[i], trans, cn);
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Update_tbl_DPRQuestionnaire(int Added_By, int Project_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_DPRQuestionnaire set  DPRQuestionnaire_Status =  '0', DPRQuestionnaire_ModifiedBy = '" + Added_By + "' ,  DPRQuestionnaire_ModifiedOn =  getdate() where DPRQuestionnaire_ProjectId = '" + Project_Id + "' and DPRQuestionnaire_Status = 1 ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private int Insert_tbl_DPRQuestionnaire(tbl_DPRQuestionnaire obj_tbl_DPRQuestionnaire, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = " set dateformat dmy; insert into tbl_DPRQuestionnaire ( [DPRQuestionnaire_AddedBy],[DPRQuestionnaire_AddedOn],[DPRQuestionnaire_ProjectId],[DPRQuestionnaire_Name],[DPRQuestionnaire_Status] ) values ('" + obj_tbl_DPRQuestionnaire.DPRQuestionnaire_AddedBy + "', getdate(), '" + obj_tbl_DPRQuestionnaire.DPRQuestionnaire_ProjectId + "', N'" + obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Name + "','" + obj_tbl_DPRQuestionnaire.DPRQuestionnaire_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    public DataSet get_ProjectWork_Update_Detailed(int ProjectWork_Id, int ProjectDPR_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy;";

        strQuery += @" select 
                            PhysicalProgressComponent_Id,
                            PhysicalProgressComponent_Component,
                            Unit_Name,
                            ProjectDPR_PhysicalProgress_Id=isnull(ProjectDPR_PhysicalProgress_Id,0)
                        from tbl_PhysicalProgressComponent
                        left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
                        left join tbl_ProjectDPR_PhysicalProgress on ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id=PhysicalProgressComponent_Id and ProjectDPR_PhysicalProgress_PrjectWork_Id ='" + ProjectWork_Id + "' and ProjectDPR_PhysicalProgress_ProjectDPR_Id ='" + ProjectDPR_Id + "' and ProjectDPR_PhysicalProgress_Status = 1 where PhysicalProgressComponent_Status = 1 ";

        strQuery += @" select 
                            Deliverables_Id,
                            Deliverables_Deliverables,
                            Unit_Name,
                            ProjectDPR_Deliverables_Id=isnull(ProjectDPR_Deliverables_Id,0)
                        from tbl_Deliverables
                        left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                        left join tbl_ProjectDPR_Deliverables on ProjectDPR_Deliverables_Deliverables_Id=Deliverables_Id and ProjectDPR_Deliverables_ProjectWork_Id='" + ProjectWork_Id + "' and ProjectDPR_Deliverables_ProjectDPR_Id ='" + ProjectDPR_Id + "' and ProjectDPR_Deliverables_Status = 1 where Deliverables_Status = 1 ";

        strQuery += @" select 
                            DPRQuestionnaire_Name,
                            ProjectDPRQuestionire_Answer,
                            ProjectDPRQuestionire_Remark 
                        from tbl_ProjectDPRQuestionire
                        inner join tbl_DPRQuestionnaire on DPRQuestionnaire_Id=ProjectDPRQuestionire_Questionire_Id
                        where ProjectDPRQuestionire_DPR_Id='" + ProjectDPR_Id + "' and ProjectDPRQuestionire_Work_Id='" + ProjectWork_Id + "' and ProjectDPRQuestionire_Status=1 ";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Questionire
    public bool Insert_tbl_Project_Questionire(List<tbl_ProjectQuestionnaire> obj_tbl_ProjectQuestionnaire_Li)
    {
        bool flag = false;
        DataSet ds = new DataSet();
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Update_tbl_ProjectQuestionnaire(obj_tbl_ProjectQuestionnaire_Li[0].ProjectQuestionnaire_AddedBy, obj_tbl_ProjectQuestionnaire_Li[0].ProjectQuestionnaire_ProjectId, trans, cn);
                Update_tbl_ProjectAnswer(obj_tbl_ProjectQuestionnaire_Li[0].ProjectQuestionnaire_AddedBy, obj_tbl_ProjectQuestionnaire_Li[0].ProjectQuestionnaire_ProjectId, trans, cn);
                if (obj_tbl_ProjectQuestionnaire_Li != null && obj_tbl_ProjectQuestionnaire_Li.Count > 0)
                {
                    for (int i = 0; i < obj_tbl_ProjectQuestionnaire_Li.Count; i++)
                    {
                        obj_tbl_ProjectQuestionnaire_Li[i].ProjectQuestionnaire_Id = Insert_tbl_ProjectQuestionnaire(obj_tbl_ProjectQuestionnaire_Li[i], trans, cn);

                        if (obj_tbl_ProjectQuestionnaire_Li[i].obj_tbl_ProjectAnswer_Li != null && obj_tbl_ProjectQuestionnaire_Li[i].obj_tbl_ProjectAnswer_Li.Count > 0)
                        {
                            for (int j = 0; j < obj_tbl_ProjectQuestionnaire_Li[i].obj_tbl_ProjectAnswer_Li.Count; j++)
                            {
                                obj_tbl_ProjectQuestionnaire_Li[i].obj_tbl_ProjectAnswer_Li[j].ProjectAnswer_ProjectQuestionnaireId = obj_tbl_ProjectQuestionnaire_Li[i].ProjectQuestionnaire_Id;
                                Insert_tbl_ProjectAnswer(obj_tbl_ProjectQuestionnaire_Li[i].obj_tbl_ProjectAnswer_Li[j], trans, cn);
                            }
                        }
                    }
                }
                trans.Commit();
                cn.Close();
                flag = true;
            }
            catch
            {
                trans.Rollback();
                cn.Close();
                flag = false;
            }
        }
        return flag;
    }

    private void Update_tbl_ProjectQuestionnaire(int Added_By, int Project_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_ProjectQuestionnaire set  ProjectQuestionnaire_Status =  '0', ProjectQuestionnaire_ModifiedBy = '" + Added_By + "' ,  ProjectQuestionnaire_ModifiedOn =  getdate() where ProjectQuestionnaire_ProjectId = '" + Project_Id + "' and ProjectQuestionnaire_Status = 1 ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    private int Insert_tbl_ProjectQuestionnaire(tbl_ProjectQuestionnaire obj_tbl_ProjectQuestionnaire, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = " set dateformat dmy; insert into tbl_ProjectQuestionnaire ( [ProjectQuestionnaire_AddedBy],[ProjectQuestionnaire_AddedOn],[ProjectQuestionnaire_ProjectId],[ProjectQuestionnaire_Name],[ProjectQuestionnaire_Status] ) values ('" + obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_AddedBy + "', getdate(), '" + obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_ProjectId + "', N'" + obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Name + "','" + obj_tbl_ProjectQuestionnaire.ProjectQuestionnaire_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    private void Update_tbl_ProjectAnswer(int Added_By, int Project_Id, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; Update  tbl_ProjectAnswer set  ProjectAnswer_Status =  '0', ProjectAnswer_ModifiedBy = '" + Added_By + "' ,  ProjectAnswer_ModifiedOn =  getdate() where ProjectAnswer_ProjectQuestionnaireId in (select ProjectQuestionnaire_Id from tbl_ProjectQuestionnaire where ProjectQuestionnaire_ProjectId = 1 and ProjectQuestionnaire_Status = 1) and ProjectAnswer_Status = 1 ";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {

            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_ProjectAnswer(tbl_ProjectAnswer obj_tbl_ProjectAnswer, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy; insert into tbl_ProjectAnswer ( [ProjectAnswer_AddedBy],[ProjectAnswer_AddedOn],[ProjectAnswer_ProjectQuestionnaireId],[ProjectAnswer_Name],[ProjectAnswer_Status] ) values ('" + obj_tbl_ProjectAnswer.ProjectAnswer_AddedBy + "', getdate(), '" + obj_tbl_ProjectAnswer.ProjectAnswer_ProjectQuestionnaireId + "', N'" + obj_tbl_ProjectAnswer.ProjectAnswer_Name + "','" + obj_tbl_ProjectAnswer.ProjectAnswer_Status + "')";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public DataSet get_tbl_ProjectQuestionnaire(int Project_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        ProjectQuestionnaire_Id,
                        ProjectQuestionnaire_ProjectId,
                        ProjectQuestionnaire_Name,
                        ProjectQuestionnaire_AddedBy,
                        ProjectQuestionnaire_AddedOn,
                        ProjectQuestionnaire_Status
                    from tbl_ProjectQuestionnaire 
                    where ProjectQuestionnaire_Status = 1 ";
        if (Project_Id > 0)
        {
            strQuery += " and ProjectQuestionnaire_ProjectId = '" + Project_Id + "'";
        }
        strQuery += " order by ProjectQuestionnaire_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectAnswer(int Questionnaire_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @" set dateformat dmy; 
                    select 
                        ProjectAnswer_Id,
                        ProjectAnswer_ProjectQuestionnaireId,
                        ProjectAnswer_Name,
                        ProjectAnswer_AddedBy,
                        ProjectAnswer_AddedOn,
                        ProjectAnswer_Status
                    from tbl_ProjectAnswer 
                    where ProjectAnswer_Status = 1 ";
        if (Questionnaire_Id > 0)
        {
            strQuery += " and ProjectAnswer_ProjectQuestionnaireId = '" + Questionnaire_Id + "'";
        }
        strQuery += " order by ProjectAnswer_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Tender Status
    public bool Update_tbl_ProjectPkg_TenderStatus(tbl_ProjectPkgTenderInfo obj_tbl_ProjectPkgTenderInfo, tbl_PersonDetail obj_PersonDetail, tbl_PersonJuridiction obj_PersonJuridiction, ref string errorMSG)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                if (obj_PersonDetail != null && obj_PersonDetail.Person_Id == 0)
                {
                    if (AllClasses.CheckDataSet(CheckDuplicacy_Mobile_No(obj_PersonDetail.Person_Mobile1, obj_PersonDetail.Person_Mobile2, 0, trans, cn)))
                    {
                        errorMSG = "Mobile No Provided Already Exist in Database. Please Select from Drop Down List..";
                        trans.Commit();
                        cn.Close();
                        iResult = true;
                        return iResult;
                    }
                }
                if (obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_VendorPersonId < 0 && obj_PersonDetail != null)
                {
                    obj_PersonDetail.Person_Id = Insert_tbl_PersonDetail(obj_PersonDetail, trans, cn);

                    obj_PersonJuridiction.PersonJuridiction_PersonId = obj_PersonDetail.Person_Id;
                    Insert_tbl_PersonJuridiction(obj_PersonJuridiction, trans, cn);

                    obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_VendorPersonId = obj_PersonDetail.Person_Id;
                }

                string sql = "set dateformat dmy; update tbl_ProjectWorkPkg set ProjectWorkPkg_Vendor_Id = '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_VendorPersonId + "' where ProjectWorkPkg_Id = '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectPkg_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                Insert_tbl_ProjectPkgTenderInfo(obj_tbl_ProjectPkgTenderInfo, trans, cn);

                trans.Commit();
                cn.Close();
                iResult = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }
    private void Insert_tbl_ProjectPkgTenderInfo(tbl_ProjectPkgTenderInfo obj_tbl_ProjectPkgTenderInfo, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkgTenderInfo ( [ProjectPkgTenderInfo_AddedBy],[ProjectPkgTenderInfo_AddedOn],[ProjectPkgTenderInfo_Comments],[ProjectPkgTenderInfo_ProjectPkg_Id],[ProjectPkgTenderInfo_ProjectWork_Id],[ProjectPkgTenderInfo_Status],[ProjectPkgTenderInfo_TenderAmount],[ProjectPkgTenderInfo_TenderDate],[ProjectPkgTenderInfo_VendorPersonId],[ProjectPkgTenderInfo_NITDate],[ProjectPkgTenderInfo_TenderStatus], [ProjectPkgTenderInfo_CompletionTime],ProjectPkgTenderInfo_Centage,ProjectPkgTenderInfo_WorkCostIn,ProjectPkgTenderInfo_WorkCostOut,ProjectPkgTenderInfo_GSTNotIncludeWorkCost,ProjectPkgTenderInfo_PrebidMeetingDate,ProjectPkgTenderInfo_TenderOutDate,ProjectPkgTenderInfo_TenderTechnicalDate,ProjectPkgTenderInfo_TenderFinancialDate,ProjectPkgTenderInfo_ContractSignDate,ProjectPkgTenderInfo_ContractBondNo) values ('" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_AddedBy + "', getdate(), N'" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Comments + "','" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectPkg_Id + "','" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectWork_Id + "','" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Status + "','" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderAmount + "', convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderDate + "', 103), '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_VendorPersonId + "', convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_NITDate + "', 103), '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderStatus + "', '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_CompletionTime + "', '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Centage + "', '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostIn + "', '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_WorkCostOut + "', '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_GSTNotIncludeWorkCost + "', convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_PrebidMeetingDate + "', 103),convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderOutDate + "', 103), convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderTechnicalDate + "', 103), convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_TenderFinancialDate + "', 103),convert(date, '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ContractSignDate + "', 103), '" + obj_tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ContractBondNo + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public DataSet get_tbl_ProjectPkgTenderInfo(string ProjectPkg_Id, string ProjectWork_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ProjectPkgTenderInfo_Id,
                        ProjectPkgTenderInfo_ProjectPkg_Id,
                        ProjectPkgTenderInfo_ProjectWork_Id,
                        ProjectPkgTenderInfo_VendorPersonId,
                        ProjectPkgTenderInfo_TenderAmount = convert(decimal(18, 3), ProjectPkgTenderInfo_TenderAmount / 100000),
                        ProjectPkgTenderInfo_TenderDate = convert(char(10), ProjectPkgTenderInfo_TenderDate, 103),
                        ProjectPkgTenderInfo_Comments,
                        ProjectPkgTenderInfo_CompletionTime,
                        ProjectPkgTenderInfo_AddedOn,
                        ProjectPkgTenderInfo_AddedBy,
                        ProjectPkgTenderInfo_Status,
                        ProjectPkgTenderInfo_ModifiedBy,
                        ProjectPkgTenderInfo_ModifiedOn,
                        ProjectPkgTenderInfo_NITDate = convert(char(10), ProjectPkgTenderInfo_NITDate, 103),
                        ProjectPkgTenderInfo_TenderStatus,
                        ProjectPkgTenderInfo_Centage,
                        ProjectPkgTenderInfo_WorkCostIn = convert(decimal(18, 3), ProjectPkgTenderInfo_WorkCostIn / 100000),
                        ProjectPkgTenderInfo_WorkCostOut = convert(decimal(18, 3), ProjectPkgTenderInfo_WorkCostOut / 100000),
                        ProjectPkgTenderInfo_GSTNotIncludeWorkCost,
                        ProjectPkgTenderInfo_PrebidMeetingDate = convert(char(10), ProjectPkgTenderInfo_PrebidMeetingDate, 103),
                        ProjectPkgTenderInfo_TenderOutDate = convert(char(10), ProjectPkgTenderInfo_TenderOutDate, 103),
                        ProjectPkgTenderInfo_TenderTechnicalDate = convert(char(10), ProjectPkgTenderInfo_TenderTechnicalDate, 103),
                        ProjectPkgTenderInfo_TenderFinancialDate = convert(char(10), ProjectPkgTenderInfo_TenderFinancialDate, 103),
                        ProjectPkgTenderInfo_ContractSignDate = convert(char(10), ProjectPkgTenderInfo_ContractSignDate, 103),
                        ProjectPkgTenderInfo_ContractBondNo
                    from tbl_ProjectPkgTenderInfo
                    where ProjectPkgTenderInfo_Status = 1 and ProjectPkgTenderInfo_ProjectPkg_Id = '" + ProjectPkg_Id + "' and ProjectPkgTenderInfo_ProjectWork_Id = '" + ProjectWork_Id + "' order by ProjectPkgTenderInfo_Id desc";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Inspection Status
    public bool Update_tbl_ProjectDPR_InspectionStatus(List<tbl_PersonDetail_Temp> obj_tbl_PersonDetail_Temp_Li, int ProjectPkg_Id, int ProjectWork_Id, ref string errorMSG)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_PersonDetail_Temp_Li.Count; i++)
                {
                    if (obj_tbl_PersonDetail_Temp_Li[i].Person_Id == 0)
                    {
                        if (AllClasses.CheckDataSet(CheckDuplicacy_Mobile_No(obj_tbl_PersonDetail_Temp_Li[i].Person_Mobile1, obj_tbl_PersonDetail_Temp_Li[i].Person_Mobile2, 0, trans, cn)))
                        {
                            errorMSG = "Mobile No Provided Already Exist in Database. Please Select from Drop Down List..";
                            trans.Commit();
                            cn.Close();
                            iResult = true;
                            return iResult;
                        }
                    }

                    tbl_ProjectPKGInspectionInfo obj_tbl_ProjectPKGInspectionInfo = new tbl_ProjectPKGInspectionInfo();
                    if (obj_tbl_PersonDetail_Temp_Li[i].Person_Id > 0)
                    {
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_AddedBy = obj_tbl_PersonDetail_Temp_Li[i].Person_AddedBy;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_InspectionPersonId = obj_tbl_PersonDetail_Temp_Li[i].Person_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectPkg_Id = ProjectPkg_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectWork_Id = ProjectWork_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_Status = 1;
                    }
                    else
                    {
                        int Designation_Id = CheckAlreadyDesignation(obj_tbl_PersonDetail_Temp_Li[i].Designation_Name, trans, cn);
                        if (Designation_Id == 0)
                        {
                            tbl_Designation obj_tbl_Designation = new tbl_Designation();
                            obj_tbl_Designation.Designation_AddedBy = Convert.ToInt32(Session["Person_Id"].ToString());
                            obj_tbl_Designation.Designation_DesignationName = obj_tbl_PersonDetail_Temp_Li[i].Designation_Name;
                            obj_tbl_Designation.Designation_Level = 6;
                            obj_tbl_Designation.Designation_Status = 1;
                            Designation_Id = Convert.ToInt32(Insert_tbl_Designation(obj_tbl_Designation, trans, cn));
                        }
                        tbl_PersonDetail obj_PersonDetail = new tbl_PersonDetail();
                        tbl_PersonJuridiction obj_PersonJuridiction = new tbl_PersonJuridiction();
                        obj_PersonDetail.Person_AddedBy = obj_tbl_PersonDetail_Temp_Li[i].Person_AddedBy;
                        obj_PersonDetail.Person_Id = 0;
                        obj_PersonDetail.Person_BranchOffice_Id = 1;
                        obj_PersonDetail.Person_AddressLine1 = "";
                        obj_PersonDetail.Person_EmailId = "";
                        obj_PersonDetail.Person_Mobile1 = obj_tbl_PersonDetail_Temp_Li[i].Person_Mobile1;
                        obj_PersonDetail.Person_Mobile2 = obj_tbl_PersonDetail_Temp_Li[i].Person_Mobile2;
                        obj_PersonDetail.Person_Name = obj_tbl_PersonDetail_Temp_Li[i].Person_Name;
                        obj_PersonDetail.Person_FName = "";
                        obj_PersonDetail.Person_Status = 1;

                        obj_PersonJuridiction.PersonJuridiction_Id = 0;
                        obj_PersonJuridiction.PersonJuridiction_PersonId = 0;
                        obj_PersonJuridiction.M_Level_Id = 3;
                        obj_PersonJuridiction.PersonJuridiction_ZoneId = obj_tbl_PersonDetail_Temp_Li[i].Zone_Id;
                        obj_PersonJuridiction.PersonJuridiction_CircleId = obj_tbl_PersonDetail_Temp_Li[i].Circle_Id;
                        obj_PersonJuridiction.PersonJuridiction_DivisionId = obj_tbl_PersonDetail_Temp_Li[i].Division_Id;
                        obj_PersonJuridiction.PersonJuridiction_AddedBy = obj_tbl_PersonDetail_Temp_Li[i].Person_AddedBy;
                        obj_PersonJuridiction.PersonJuridiction_DepartmentId = 0;
                        obj_PersonJuridiction.PersonJuridiction_DesignationId = Designation_Id;
                        obj_PersonJuridiction.PersonJuridiction_ParentPerson_Id = 0;
                        obj_PersonJuridiction.PersonJuridiction_Status = 1;
                        obj_PersonJuridiction.PersonJuridiction_UserTypeId = 6;

                        obj_PersonDetail.Person_Id = Insert_tbl_PersonDetail(obj_PersonDetail, trans, cn);

                        obj_PersonJuridiction.PersonJuridiction_PersonId = obj_PersonDetail.Person_Id;
                        Insert_tbl_PersonJuridiction(obj_PersonJuridiction, trans, cn);

                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_AddedBy = obj_tbl_PersonDetail_Temp_Li[i].Person_AddedBy;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_InspectionPersonId = obj_PersonDetail.Person_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectPkg_Id = ProjectPkg_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectWork_Id = ProjectWork_Id;
                        obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_Status = 1;
                    }
                    Insert_tbl_ProjectPKGInspectionInfo(obj_tbl_ProjectPKGInspectionInfo, trans, cn);
                }

                trans.Commit();
                cn.Close();
                iResult = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }
    private void Insert_tbl_ProjectPKGInspectionInfo(tbl_ProjectPKGInspectionInfo obj_tbl_ProjectPKGInspectionInfo, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPKGInspectionInfo ( [ProjectPKGInspectionInfo_AddedBy],[ProjectPKGInspectionInfo_AddedOn],[ProjectPKGInspectionInfo_ProjectPkg_Id],[ProjectPKGInspectionInfo_ProjectWork_Id],[ProjectPKGInspectionInfo_Status],[ProjectPKGInspectionInfo_InspectionPersonId]) values ('" + obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_AddedBy + "', getdate(),'" + obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectPkg_Id + "','" + obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_ProjectWork_Id + "','" + obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_Status + "','" + obj_tbl_ProjectPKGInspectionInfo.ProjectPKGInspectionInfo_InspectionPersonId + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public DataSet get_tbl_ProjectDPRInspectionInfo(string ProjectPKG_Id, string ProjectWork_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
                        ProjectPKGInspectionInfo_Id,
                        ProjectPKGInspectionInfo_ProjectPkg_Id,
                        ProjectPKGInspectionInfo_ProjectWork_Id,
                        ProjectPKGInspectionInfo_InspectionPersonId,
                        ProjectPKGInspectionInfo_AddedOn,
                        ProjectPKGInspectionInfo_AddedBy,
                        ProjectPKGInspectionInfo_Status,
                        Person_Id, 
                        Designation_Id, 
                        Zone_Id,
                        Zone_Name, 
						Circle_Id,
                        Circle_Name, 
						Division_Id,
                        Division_Name, 
                        Designation_Name = Designation_DesignationName, 
                        Person_Name, 
                        Person_Mobile1, 
                        Person_Mobile2
                    from tbl_ProjectPKGInspectionInfo
                    join tbl_PersonDetail on Person_Id = ProjectPKGInspectionInfo_InspectionPersonId
                    left join tbl_PersonJuridiction on PersonJuridiction_PersonId = ProjectPKGInspectionInfo_InspectionPersonId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join tbl_Zone on Zone_Id = tbl_PersonJuridiction.PersonJuridiction_ZoneId
					left join tbl_Circle on Circle_Id = tbl_PersonJuridiction.PersonJuridiction_CircleId
					left join tbl_Division on Division_Id = tbl_PersonJuridiction.PersonJuridiction_DivisionId
                    where ProjectPKGInspectionInfo_Status = 1 and ProjectPKGInspectionInfo_ProjectPkg_Id = '" + ProjectPKG_Id + "' and ProjectPkgInspectionInfo_ProjectWork_Id = '" + ProjectWork_Id + "' order by ProjectPKGInspectionInfo_Id desc";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Project Work Status
    public DataSet get_tbl_ProjectUC(int ProjectPkg_Id, int Work_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery += @"select 
	                    ProjectWorkPkg_Id, 
					    ProjectWork_GO_Path,
                        ProjectUC_Id,
                        ProjectUC_FilePath1, 
                        ProjectUC_FilePath2, 
                        ProjectUC_BudgetUtilized = convert(decimal(18,2), isnull(ProjectUC_BudgetUtilized, 0) / 100000), 
                        ProjectUC_Achivment, 
                        ProjectUC_PhysicalProgress,
                        ProjectUC_Total_Allocated = convert(decimal(18, 3), ProjectUC_Total_Allocated / 100000),
	                    ProjectUC_SubmitionDate1 = convert(char(10), ProjectUC_SubmitionDate, 103), 
                        ProjectWork_GO_No,
	                    ProjectWork_GO_Date1 = convert(char(10), ProjectWork_GO_Date, 103),
						Division_Id,
						Division_Name,
						Circle_Id,
						Circle_Name,
						Zone_Id,
						Zone_Name
                    from tbl_ProjectUC 
					join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = ProjectUC_ProjectPkg_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id and ProjectWork_Status = 1
					join tbl_Division on Division_Id=ProjectWork_DivisionId
					join tbl_Circle on Circle_Id=Division_CircleId
					join tbl_Zone on Zone_Id=Circle_ZoneId
                    where ProjectUC_Status = 1 and ProjectUC_ProjectPkg_Id = '" + ProjectPkg_Id + "' and ProjectUC_ProjectWork_Id = '" + Work_Id + "' order by ProjectUC_Id; ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectUC_ReleaseDetails(int Package_Id, int ProjectWork_Id)
    {
        //int FinancialYear_Id = Convert.ToInt32(Session["FinancialYear_Id"].ToString());
        //int FinancialYearPrev_Id = FinancialYear_Id - 1;
        string strQuery = "set dateformat dmy; ";
        DataSet ds = new DataSet();

        //ReleaseTillPrevFinYear
        strQuery += @" select case when isnull(sum(FinancialTrans_TransAmount),0)>0 then cast((isnull(sum(FinancialTrans_TransAmount),0)/100000) as decimal(18,3)) else 0.000 end as ReleaseTillPrevFinYear 
                    from tbl_FinancialTrans 
                    where FinancialTrans_TransType='c' and FinancialTrans_Status=1 and FinancialTrans_Package_Id='" + Package_Id + "' " +
                    "and FinancialTrans_Work_Id='" + ProjectWork_Id + "'   ";

        //ReleaseCurrentFinYear
        strQuery += @" select case when isnull(sum(FinancialTrans_TransAmount),0)>0 then cast((isnull(sum(FinancialTrans_TransAmount),0)/100000) as decimal(18,3)) else 0.000 end as ReleaseTillPrevFinYear 
                    from tbl_FinancialTrans 
                    where FinancialTrans_TransType='c' and FinancialTrans_Status=1 and FinancialTrans_Package_Id='" + Package_Id + "' " +
                    "and FinancialTrans_Work_Id='" + ProjectWork_Id + "'   ";

        //ReleasePrevMonth
        strQuery += @" select case when isnull(sum(FinancialTrans_TransAmount),0)>0 then cast((isnull(sum(FinancialTrans_TransAmount),0)/100000) as decimal(18,3)) else 0.000 end as ReleasePrevMonth 
                    from tbl_FinancialTrans 
                    where FinancialTrans_TransType='c' and FinancialTrans_Status=1 and FinancialTrans_Package_Id='" + Package_Id + "' " +
                    "and FinancialTrans_Work_Id='" + ProjectWork_Id + "' and MONTH(FinancialTrans_Date)=(MONTH(GETDATE())-1) and YEAR(FinancialTrans_Date)=YEAR(GETDATE())  ";

        //ReleaseCurrentMonth
        strQuery += @" select case when isnull(sum(FinancialTrans_TransAmount),0)>0 then cast((isnull(sum(FinancialTrans_TransAmount),0)/100000) as decimal(18,3)) else 0.000 end as ReleaseCurrentMonth 
                    from tbl_FinancialTrans 
                    where FinancialTrans_TransType='c' and FinancialTrans_Status=1 and FinancialTrans_Package_Id='" + Package_Id + "' " +
                    "and FinancialTrans_Work_Id='" + ProjectWork_Id + "' and MONTH(FinancialTrans_Date)=MONTH(GETDATE()) and YEAR(FinancialTrans_Date)=YEAR(GETDATE())  ";

        //ExpenditureAmtTllPrevFinYear
        strQuery += @" select top 1 case when isnull(ProjectUC_BudgetUtilized,0)>0 then cast((isnull(ProjectUC_BudgetUtilized,0)/100000) as decimal(18,3)) else 0.000 end as ExpenditureAmtTllPrevFinYear 
                    from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //ExpenditureAmtCurrentFinYear
        strQuery += @" select top 1 case when isnull(ProjectUC_BudgetUtilized,0)>0 then cast((isnull(ProjectUC_BudgetUtilized,0)/100000) as decimal(18,3)) else 0.000 end as ExpenditureAmtCurrentFinYear 
                    from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //ExpenditureAmtPrevMonth
        strQuery += @" select top 1 case when isnull(ProjectUC_BudgetUtilized,0)>0 then cast((isnull(ProjectUC_BudgetUtilized,0)/100000) as decimal(18,3)) else 0.000 end as ExpenditureAmtPrevMonth 
                    from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "'and MONTH(ProjectUC_AddedOn)=(MONTH(GETDATE())-1) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";

        //ExpenditureAmtCurrentMonth
        strQuery += @" select top 1 case when isnull(ProjectUC_BudgetUtilized,0)>0 then cast((isnull(ProjectUC_BudgetUtilized,0)/100000) as decimal(18,3)) else 0.000 end as ExpenditureAmtCurrentMonth 
                    from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "'and MONTH(ProjectUC_AddedOn)=MONTH(GETDATE()) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";

        //CentageTllPrevFinYear
        strQuery += @" select top 1 case when isnull(ProjectUC_Centage,0)>0 then cast((isnull(ProjectUC_Centage,0)/100000) as decimal(18,3)) else 0.000 end as CentageTllPrevFinYear 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //CentageCurrentFinYear
        strQuery += @" select top 1 case when isnull(ProjectUC_Centage,0)>0 then cast((isnull(ProjectUC_Centage,0)/100000) as decimal(18,3)) else 0.000 end as CentageCurrentFinYear 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //CentageAmtPrevMonth
        strQuery += @" select top 1 isnull(ProjectUC_Centage,0) as CentageAmtPrevMonth 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "'and MONTH(ProjectUC_AddedOn)=(MONTH(GETDATE())-1) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";

        //CentageAmtCurrentMonth
        strQuery += @" select top 1 case when isnull(ProjectUC_Centage,0)>0 then cast((isnull(ProjectUC_Centage,0)/100000) as decimal(18,3)) else 0.000 end as CentageAmtCurrentMonth 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "'and MONTH(ProjectUC_AddedOn)=MONTH(GETDATE()) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";

        //PhysicalPrevTotal
        strQuery += @" select top 1 isnull(ProjectUC_PhysicalProgress,0) as PhysicalPrevTotal 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //PhysicalLastMonth
        strQuery += @" select top 1 isnull(ProjectUC_PhysicalProgress,0) as PhysicalLastMonth 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' and MONTH(ProjectUC_AddedOn)=(MONTH(GETDATE())-1) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";

        //PhysicalCurrentMonth
        strQuery += @" select top 1 isnull(ProjectUC_PhysicalProgress,0) as PhysicalCurrentMonth 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' and MONTH(ProjectUC_AddedOn)=MONTH(GETDATE()) and YEAR(ProjectUC_AddedOn)=YEAR(GETDATE()) order by ProjectUC_Id desc  ";


        //PhysicalTotal
        strQuery += @" select top 1 isnull(ProjectUC_PhysicalProgress,0) as PhysicalTotal 
                        from tbl_ProjectUC where ProjectUC_Status=1 and ProjectUC_ProjectPkg_Id='" + Package_Id + "' and ProjectUC_ProjectWork_Id='" + ProjectWork_Id + "' order by ProjectUC_Id desc  ";

        //Physical Compenent Progress
        strQuery += @" select PhysicalProgressComponent_Id,
                        PhysicalProgressComponent_Component,
                        Unit_Name,
                        PhysicalProgressPreTotal=isnull(tbl_ProjectUC_PhysicalProgressPrevTotal.ProjectUC_PhysicalProgress_PhysicalProgress,0),
						PhysicalProgressLastMonth=isnull(tbl_ProjectUC_PhysicalProgressLastMonth.ProjectUC_PhysicalProgress_PhysicalProgress,0),
						PhysicalProgressCurrentMonth=isnull(tbl_ProjectUC_PhysicalProgressCurrentMonth.ProjectUC_PhysicalProgress_PhysicalProgress,0),
						PhysicalProgressTotal=isnull(tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_PhysicalProgress,0)
                        from tbl_ProjectPkg_PhysicalProgress
                        left join tbl_PhysicalProgressComponent on ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id=PhysicalProgressComponent_Id  
						left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
						left join (select ROW_NUMBER() Over(Partition by ProjectUC_PhysicalProgress_ProjectWork_Id,ProjectUC_PhysicalProgress_ProjectPkg_Id,ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id order by ProjectUC_PhysicalProgress_Id desc) as rr,*
					from tbl_ProjectUC_PhysicalProgress where ProjectUC_PhysicalProgress_Status=1) as tbl_ProjectUC_PhysicalProgressPrevTotal on tbl_ProjectUC_PhysicalProgressPrevTotal.rr=1 and tbl_ProjectUC_PhysicalProgressPrevTotal.ProjectUC_PhysicalProgress_ProjectWork_Id=ProjectPkg_PhysicalProgress_PrjectWork_Id and tbl_ProjectUC_PhysicalProgressPrevTotal.ProjectUC_PhysicalProgress_ProjectPkg_Id=ProjectPkg_PhysicalProgress_ProjectPkg_Id and tbl_ProjectUC_PhysicalProgressPrevTotal.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id=ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id
					left join (select ROW_NUMBER() Over(Partition by ProjectUC_PhysicalProgress_ProjectWork_Id,ProjectUC_PhysicalProgress_ProjectPkg_Id,ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id order by ProjectUC_PhysicalProgress_Id desc) as rr,*
					from tbl_ProjectUC_PhysicalProgress where ProjectUC_PhysicalProgress_Status=1 and  MONTH(ProjectUC_PhysicalProgress_AddedOn)=(MONTH(GETDATE())-1) and YEAR(ProjectUC_PhysicalProgress_AddedOn)=YEAR(GETDATE())) as tbl_ProjectUC_PhysicalProgressLastMonth on tbl_ProjectUC_PhysicalProgressLastMonth.rr=1 and tbl_ProjectUC_PhysicalProgressLastMonth.ProjectUC_PhysicalProgress_ProjectWork_Id=ProjectPkg_PhysicalProgress_PrjectWork_Id and tbl_ProjectUC_PhysicalProgressLastMonth.ProjectUC_PhysicalProgress_ProjectPkg_Id=ProjectPkg_PhysicalProgress_ProjectPkg_Id and tbl_ProjectUC_PhysicalProgressLastMonth.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id=ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id
					left join (select ROW_NUMBER() Over(Partition by ProjectUC_PhysicalProgress_ProjectWork_Id,ProjectUC_PhysicalProgress_ProjectPkg_Id,ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id order by ProjectUC_PhysicalProgress_Id desc) as rr,*
					from tbl_ProjectUC_PhysicalProgress where ProjectUC_PhysicalProgress_Status=1 and  MONTH(ProjectUC_PhysicalProgress_AddedOn)=MONTH(GETDATE()) and YEAR(ProjectUC_PhysicalProgress_AddedOn)=YEAR(GETDATE())) as tbl_ProjectUC_PhysicalProgressCurrentMonth on tbl_ProjectUC_PhysicalProgressCurrentMonth.rr=1 and tbl_ProjectUC_PhysicalProgressCurrentMonth.ProjectUC_PhysicalProgress_ProjectWork_Id=ProjectPkg_PhysicalProgress_PrjectWork_Id and tbl_ProjectUC_PhysicalProgressCurrentMonth.ProjectUC_PhysicalProgress_ProjectPkg_Id=ProjectPkg_PhysicalProgress_ProjectPkg_Id and tbl_ProjectUC_PhysicalProgressCurrentMonth.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id=ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id
					left join (select ROW_NUMBER() Over(Partition by ProjectUC_PhysicalProgress_ProjectWork_Id,ProjectUC_PhysicalProgress_ProjectPkg_Id,ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id order by ProjectUC_PhysicalProgress_Id desc) as rr,*
					from tbl_ProjectUC_PhysicalProgress where ProjectUC_PhysicalProgress_Status=1) as tbl_ProjectUC_PhysicalProgressTotal on tbl_ProjectUC_PhysicalProgressTotal.rr=1 and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_ProjectWork_Id=ProjectPkg_PhysicalProgress_PrjectWork_Id and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_ProjectPkg_Id=ProjectPkg_PhysicalProgress_ProjectPkg_Id and tbl_ProjectUC_PhysicalProgressTotal.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id=ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id

						where ProjectPkg_PhysicalProgress_Status=1  and ProjectPkg_PhysicalProgress_PrjectWork_Id= '" + ProjectWork_Id + "' and ProjectPkg_PhysicalProgress_ProjectPkg_Id='" + Package_Id + "'  ";

        //Deliverables
        strQuery += @" select Deliverables_Id,
                        Deliverables_Deliverables,
                        Unit_Name,
                        DeliverablesPrevTotal=isnull(tbl_ProjectUC_DeliverablesPrevTotal.ProjectUC_Deliverables_Deliverables,0),
						DeliverablesLastMonth=isnull(tbl_ProjectUC_DeliverablesLastMonth.ProjectUC_Deliverables_Deliverables,0),
						DeliverablesCurrentMonth=isnull(tbl_ProjectUC_DeliverablesCurrentMonth.ProjectUC_Deliverables_Deliverables,0),
						DeliverablesTotal=isnull(tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_Deliverables,0)
                        from tbl_ProjectPkg_Deliverables
                        left join tbl_Deliverables on ProjectPkg_Deliverables_Deliverables_Id=Deliverables_Id  
						left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
						left join (select ROW_NUMBER() Over(Partition by ProjectUC_Deliverables_ProjectWork_Id,ProjectUC_Deliverables_ProjectPkg_Id,ProjectUC_Deliverables_Deliverables_Id order by ProjectUC_Deliverables_Id desc) as rr,*
					from tbl_ProjectUC_Deliverables where ProjectUC_Deliverables_Status=1) as tbl_ProjectUC_DeliverablesPrevTotal on tbl_ProjectUC_DeliverablesPrevTotal.rr=1 and tbl_ProjectUC_DeliverablesPrevTotal.ProjectUC_Deliverables_ProjectWork_Id=ProjectPkg_Deliverables_ProjectWork_Id and tbl_ProjectUC_DeliverablesPrevTotal.ProjectUC_Deliverables_ProjectPkg_Id=ProjectPkg_Deliverables_ProjectPkg_Id and tbl_ProjectUC_DeliverablesPrevTotal.ProjectUC_Deliverables_Deliverables_Id=ProjectPkg_Deliverables_Deliverables_Id 
                        left join (select ROW_NUMBER() Over(Partition by ProjectUC_Deliverables_ProjectWork_Id,ProjectUC_Deliverables_ProjectPkg_Id,ProjectUC_Deliverables_Deliverables_Id order by ProjectUC_Deliverables_Id desc) as rr,*
					from tbl_ProjectUC_Deliverables where ProjectUC_Deliverables_Status=1 and  MONTH(ProjectUC_Deliverables_AddedOn)=(MONTH(GETDATE())-1) and YEAR(ProjectUC_Deliverables_AddedOn)=YEAR(GETDATE())) as tbl_ProjectUC_DeliverablesLastMonth on tbl_ProjectUC_DeliverablesLastMonth.rr=1 and tbl_ProjectUC_DeliverablesLastMonth.ProjectUC_Deliverables_ProjectWork_Id=ProjectPkg_Deliverables_ProjectWork_Id and tbl_ProjectUC_DeliverablesLastMonth.ProjectUC_Deliverables_ProjectPkg_Id=ProjectPkg_Deliverables_ProjectPkg_Id and tbl_ProjectUC_DeliverablesLastMonth.ProjectUC_Deliverables_Deliverables_Id=ProjectPkg_Deliverables_Deliverables_Id
					left join (select ROW_NUMBER() Over(Partition by ProjectUC_Deliverables_ProjectWork_Id,ProjectUC_Deliverables_ProjectPkg_Id,ProjectUC_Deliverables_Deliverables_Id order by ProjectUC_Deliverables_Id desc) as rr,*
					from tbl_ProjectUC_Deliverables where ProjectUC_Deliverables_Status=1 and  MONTH(ProjectUC_Deliverables_AddedOn)=MONTH(GETDATE()) and YEAR(ProjectUC_Deliverables_AddedOn)=YEAR(GETDATE())) as tbl_ProjectUC_DeliverablesCurrentMonth on tbl_ProjectUC_DeliverablesCurrentMonth.rr=1 and tbl_ProjectUC_DeliverablesCurrentMonth.ProjectUC_Deliverables_ProjectWork_Id=ProjectPkg_Deliverables_ProjectWork_Id and tbl_ProjectUC_DeliverablesCurrentMonth.ProjectUC_Deliverables_ProjectPkg_Id=ProjectPkg_Deliverables_ProjectPkg_Id and tbl_ProjectUC_DeliverablesCurrentMonth.ProjectUC_Deliverables_Deliverables_Id=ProjectPkg_Deliverables_Deliverables_Id
					 left join (select ROW_NUMBER() Over(Partition by ProjectUC_Deliverables_ProjectWork_Id,ProjectUC_Deliverables_ProjectPkg_Id,ProjectUC_Deliverables_Deliverables_Id order by ProjectUC_Deliverables_Id desc) as rr,*
					from tbl_ProjectUC_Deliverables where ProjectUC_Deliverables_Status=1) as tbl_ProjectUC_DeliverablesTotal on tbl_ProjectUC_DeliverablesTotal.rr=1 and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_ProjectWork_Id=ProjectPkg_Deliverables_ProjectWork_Id and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_ProjectPkg_Id=ProjectPkg_Deliverables_ProjectPkg_Id and tbl_ProjectUC_DeliverablesTotal.ProjectUC_Deliverables_Deliverables_Id=ProjectPkg_Deliverables_Deliverables_Id
						where ProjectPkg_Deliverables_Status=1 and ProjectPkg_Deliverables_ProjectWork_Id='" + ProjectWork_Id + "' and ProjectPkg_Deliverables_ProjectPkg_Id='" + Package_Id + "'  ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    private void Insert_tbl_ProjectPkgSitePics(tbl_ProjectPkgSitePics obj_tbl_ProjectPkgSitePics, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkgSitePics ( [ProjectPkgSitePics_AddedBy],[ProjectPkgSitePics_AddedOn],[ProjectPkgSitePics_ProjectPkg_Id],[ProjectPkgSitePics_ProjectWork_Id],[ProjectPkgSitePics_ReportSubmitted],[ProjectPkgSitePics_ReportSubmittedBy_PersonId],[ProjectPkgSitePics_SitePic_Path1],[ProjectPkgSitePics_SitePic_Type],[ProjectPkgSitePics_Status] ) values ('" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_AddedBy + "',getdate(),'" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectPkg_Id + "','" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ProjectWork_Id + "',N'" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ReportSubmitted + "','" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_ReportSubmittedBy_PersonId + "',N'" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1 + "',N'" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Type + "','" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_Status + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public bool Update_tbl_ProjectDPR_WorkStatus(tbl_ProjectUC obj_tbl_ProjectUC, List<tbl_ProjectUC_Concent> obj_tbl_ProjectUC_Concent_Li, List<tbl_ProjectUC_PhysicalProgress> obj_tbl_ProjectUC_PhysicalProgress_Li, List<tbl_ProjectUC_Deliverables> obj_tbl_ProjectUC_Deliverables_Li, List<tbl_ProjectPkgSitePics> obj_tbl_ProjectPkgSitePics_Li, tbl_FinancialTrans obj_tbl_FinancialTrans)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < obj_tbl_ProjectPkgSitePics_Li.Count; i++)
                {
                    tbl_ProjectPkgSitePics obj_tbl_ProjectPkgSitePics = obj_tbl_ProjectPkgSitePics_Li[i];
                    if (obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Bytes1 != null && obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Bytes1.Length > 0)
                    {
                        obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1 = "\\Downloads\\Work\\" + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1;
                        File.WriteAllBytes(Server.MapPath(".") + obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Path1, obj_tbl_ProjectPkgSitePics.ProjectPkgSitePics_SitePic_Bytes1);
                        Insert_tbl_ProjectPkgSitePics(obj_tbl_ProjectPkgSitePics, trans, cn);
                    }
                }

                obj_tbl_ProjectUC.ProjectUC_Id = Insert_tbl_ProjectUC(obj_tbl_ProjectUC, trans, cn);

                for (int i = 0; i < obj_tbl_ProjectUC_PhysicalProgress_Li.Count; i++)
                {
                    Insert_tbl_ProjectUC_PhysicalProgress(obj_tbl_ProjectUC_PhysicalProgress_Li[i], trans, cn);
                }

                for (int i = 0; i < obj_tbl_ProjectUC_Deliverables_Li.Count; i++)
                {
                    Insert_tbl_ProjectUC_Deliverables(obj_tbl_ProjectUC_Deliverables_Li[i], trans, cn);
                }

                for (int i = 0; i < obj_tbl_ProjectUC_Concent_Li.Count; i++)
                {
                    obj_tbl_ProjectUC_Concent_Li[i].ProjectUC_Concent_ProjectUC_Id = obj_tbl_ProjectUC.ProjectUC_Id;
                    Insert_tbl_ProjectUC_Concent(obj_tbl_ProjectUC_Concent_Li[i], trans, cn);
                }

                Insert_tbl_FinancialTrans(obj_tbl_FinancialTrans, trans, cn);
                iResult = true;
                trans.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }

    private int Insert_tbl_ProjectUC(tbl_ProjectUC obj_tbl_ProjectUC, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectUC ( [ProjectUC_Achivment],[ProjectUC_AddedBy],[ProjectUC_AddedOn],[ProjectUC_BudgetUtilized],[ProjectUC_Comments],[ProjectUC_ProjectPkg_Id],[ProjectUC_ProjectWork_Id],[ProjectUC_Status],[ProjectUC_SubmitionDate], [ProjectUC_PhysicalProgress], [ProjectUC_Latitude], [ProjectUC_Longitude], [ProjectUC_FinancialYear_Id], [ProjectUC_Total_Allocated],ProjectUC_Centage) values ('" + obj_tbl_ProjectUC.ProjectUC_Achivment + "','" + obj_tbl_ProjectUC.ProjectUC_AddedBy + "', getdate(),'" + obj_tbl_ProjectUC.ProjectUC_BudgetUtilized + "',N'" + obj_tbl_ProjectUC.ProjectUC_Comments + "','" + obj_tbl_ProjectUC.ProjectUC_ProjectPkg_Id + "','" + obj_tbl_ProjectUC.ProjectUC_ProjectWork_Id + "','" + obj_tbl_ProjectUC.ProjectUC_Status + "', convert(date, '" + obj_tbl_ProjectUC.ProjectUC_SubmitionDate + "', 103), '" + obj_tbl_ProjectUC.ProjectUC_PhysicalProgress + "', '" + obj_tbl_ProjectUC.ProjectUC_Latitude + "', '" + obj_tbl_ProjectUC.ProjectUC_Longitude + "', '" + obj_tbl_ProjectUC.ProjectUC_FinancialYear_Id + "', '" + obj_tbl_ProjectUC.ProjectUC_Total_Allocated + "', '" + obj_tbl_ProjectUC.ProjectUC_Centage + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectUC_PhysicalProgress(tbl_ProjectUC_PhysicalProgress obj_tbl_ProjectUC_PhysicalProgress, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectUC_PhysicalProgress ( [ProjectUC_PhysicalProgress_AddedBy],[ProjectUC_PhysicalProgress_AddedOn],[ProjectUC_PhysicalProgress_ProjectWork_Id],[ProjectUC_PhysicalProgress_ProjectPkg_Id],[ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id], [ProjectUC_PhysicalProgress_Status],ProjectUC_PhysicalProgress_FinancialYear_Id,ProjectUC_PhysicalProgress_PhysicalProgress) values ('" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_AddedBy + "', getdate(),'" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_ProjectWork_Id + "','" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_ProjectPkg_Id + "','" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id + "', '" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_Status + "', '" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_FinancialYear_Id + "', '" + obj_tbl_ProjectUC_PhysicalProgress.ProjectUC_PhysicalProgress_PhysicalProgress + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectUC_Deliverables(tbl_ProjectUC_Deliverables obj_tbl_ProjectUC_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectUC_Deliverables ( [ProjectUC_Deliverables_AddedBy],[ProjectUC_Deliverables_AddedOn],[ProjectUC_Deliverables_ProjectWork_Id],[ProjectUC_Deliverables_ProjectPkg_Id],[ProjectUC_Deliverables_Deliverables_Id], [ProjectUC_Deliverables_Status],ProjectUC_Deliverables_FinancialYear_Id,ProjectUC_Deliverables_Deliverables) values ('" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_AddedBy + "', getdate(),'" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_ProjectWork_Id + "','" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_ProjectPkg_Id + "','" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_Deliverables_Id + "', '" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_Status + "', '" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_FinancialYear_Id + "', '" + obj_tbl_ProjectUC_Deliverables.ProjectUC_Deliverables_Deliverables + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectDPR_PhysicalProgress(tbl_ProjectPkg_PhysicalProgress obj_tbl_ProjectPkg_PhysicalProgress, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkg_PhysicalProgress ( [ProjectPkg_PhysicalProgress_AddedBy],[ProjectPkg_PhysicalProgress_AddedOn],[ProjectPkg_PhysicalProgress_PrjectWork_Id],[ProjectPkg_PhysicalProgress_ProjectPkg_Id],[ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id], [ProjectPkg_PhysicalProgress_Status]) values ('" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_AddedBy + "', getdate(),'" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_PrjectWork_Id + "','" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_ProjectPkg_Id + "','" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_PhysicalProgressComponent_Id + "', '" + obj_tbl_ProjectPkg_PhysicalProgress.ProjectPkg_PhysicalProgress_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectDPR_Deliverables(tbl_ProjectPkg_Deliverables obj_tbl_ProjectPkg_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectPkg_Deliverables ( [ProjectPkg_Deliverables_AddedBy],[ProjectPkg_Deliverables_AddedOn],[ProjectPkg_Deliverables_ProjectWork_Id],[ProjectPkg_Deliverables_ProjectPkg_Id],[ProjectPkg_Deliverables_Deliverables_Id], [ProjectPkg_Deliverables_Status]) values ('" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_AddedBy + "', getdate(),'" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_ProjectWork_Id + "','" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_ProjectPkg_Id + "','" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_Deliverables_Id + "', '" + obj_tbl_ProjectPkg_Deliverables.ProjectPkg_Deliverables_Status + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }

    private void Insert_tbl_ProjectUC_Concent(tbl_ProjectUC_Concent objtbl_ProjectUC_Concent, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectUC_Concent (ProjectUC_Concent_ProjectUC_Id, ProjectUC_Concent_Questionire_Id, ProjectUC_Concent_Answer_Id, ProjectUC_Concent_AddedOn, ProjectUC_Concent_AddedBy, ProjectUC_Concent_Status, ProjectUC_Concent_Comments, ProjectUC_Concent_Answer) values ('" + objtbl_ProjectUC_Concent.ProjectUC_Concent_ProjectUC_Id + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Questionire_Id + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Answer_Id + "', getdate(), '" + objtbl_ProjectUC_Concent.ProjectUC_Concent_AddedBy + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Status + "','" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Comments + "', N'" + objtbl_ProjectUC_Concent.ProjectUC_Concent_Answer + "')";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }

    public DataSet get_Work_Updation_Authority(int Work_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Authority = 'EO: ' + ISNULL(Person_Name, '') + ', Mob: ' + ISNULL(Person_Mobile1, 'NA'),
	                    Person_Id
                    from tbl_ProjectWork
                    join tbl_ProjectDPR on ProjectWork_Id = ProjectDPR_Work_Id and ProjectWork_Status = 1
                    join tbl_ULB on ULB_Id = ProjectDPR_NP_JurisdictionId
                    join tbl_PersonJuridiction on PersonJuridiction_ULB_Id = ProjectDPR_NP_JurisdictionId
                    join tbl_PersonDetail on Person_Id = PersonJuridiction_PersonId
                    where ProjectWork_Id = Work_IdCond

                    union all

                    select
	                    Authority = 'Executing Agency: ' + ISNULL(Person_Name, '') + ', Mob: ' + ISNULL(Person_Mobile1, 'NA'),
	                    Person_Id
                    from tbl_ProjectDPRTenderInfo 
                    join tbl_PersonDetail on ProjectDPRTenderInfo_VendorPersonId = Person_Id
                    where ProjectDPRTenderInfo_ProjectWork_Id = Work_IdCond and ProjectDPRTenderInfo_Status = 1

                    union all

                    select 
	                    Authority = 'Verifiying Officers: ' + ISNULL(Person_Name, '') + ', Mob: ' + ISNULL(Person_Mobile1, 'NA'),
	                    Person_Id
                    from tbl_ProjectDPRInspectionInfo 
                    join tbl_PersonDetail on ProjectDPRInspectionInfo_InspectionPersonId = Person_Id
                    where ProjectDPRInspectionInfo_ProjectWork_Id = Work_IdCond and ProjectDPRInspectionInfo_Status = 1";
        if (Work_Id != 0)
        {
            strQuery = strQuery.Replace("Work_IdCond", Work_Id.ToString());
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Login_History_Report
    public DataSet get_Login_History_Report(string fromDate, string tillDate)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy;
                    select 
	                    Person_Id,
	                    Person_Name, 
	                    Person_Mobile1, 
	                    Zone_Name, 
	                    Circle_Name, 
	                    Division_Name,
	                    Designation_DesignationName, 
	                    OfficeBranch_Name, 
	                    COUNT(*) Total_Login, 
	                    MIN(LoginHistory_LoggedInTime) First_LoginTime,
	                    MIN(LoginHistory_LoggedInTime) Last_LoginTime
                    from tbl_LoginHistory
                    join tbl_PersonDetail on Person_Id = LoginHistory_PersonId
                    join tbl_PersonJuridiction on PersonJuridiction_PersonId = LoginHistory_PersonId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    left join tbl_OfficeBranch on tbl_OfficeBranch.OfficeBranch_Id = Person_BranchOffice_Id
                    left join tbl_Division on PersonJuridiction_DivisionId = Division_Id
                    left join tbl_Circle on Circle_Id = PersonJuridiction_CircleId
                    left join tbl_Zone on Zone_Id = PersonJuridiction_ZoneId
                    where CONVERT(date, convert(char(10), LoginHistory_LoggedInTime, 103), 103) >= CONVERT(date, '" + fromDate + "', 103) and CONVERT(date, convert(char(10), LoginHistory_LoggedInTime, 103), 103) <= CONVERT(date, '" + tillDate + "', 103) group by Person_Id, Person_Name, Person_Mobile1, Zone_Name, Circle_Name, Division_Name, Designation_DesignationName, OfficeBranch_Name  order by Zone_Name, Circle_Name, Division_Name, Person_Name";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region DPR Verificaion
    public bool Update_tbl_ProjectDPR_Verification(tbl_ProjectDPRStatus obj_tbl_ProjectDPRStatus, List<tbl_ProjectDPRQuestionire> obj_tbl_ProjectDPRQuestionire_Li, string ReceivingDate)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                Insert_tbl_ProjectDPRStatus(obj_tbl_ProjectDPRStatus, trans, cn);


                if (obj_tbl_ProjectDPRQuestionire_Li != null)
                {
                    string sql = "set dateformat dmy; update tbl_ProjectDPRQuestionire set ProjectDPRQuestionire_ModifiedBy='" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_AddedBy + "',ProjectDPRQuestionire_ModifiedOn=getdate(),ProjectDPRQuestionire_Status=0 where ProjectDPRQuestionire_DPR_Id = '" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_ProjectDPR_Id + "' and ProjectDPRQuestionire_Work_Id='" + obj_tbl_ProjectDPRStatus.ProjectDPR_Work_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);


                    for (int i = 0; i < obj_tbl_ProjectDPRQuestionire_Li.Count; i++)
                    {
                        Insert_tbl_ProjectDPRQuestionire(obj_tbl_ProjectDPRQuestionire_Li[i], trans, cn);
                    }
                }

                if (obj_tbl_ProjectDPRStatus.ProjectDPRStatus_DPR_StatusId == 7)
                {
                    string sql = "set dateformat dmy; update tbl_ProjectDPR set ProjectDPR_ReceivedAtHQDate =convert(date, '" + ReceivingDate + "', 103),ProjectDPR_IsVerified=1,ProjectDPR_Verified_Comments='" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_Comments + "',ProjectDPR_VerifiedBy='" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_AddedBy + "',ProjectDPR_VerifiedOn=getdate() where ProjectDPR_Id = '" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_ProjectDPR_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                }
                else
                {
                    string sql1 = "set dateformat dmy; update tbl_ProjectDPR set ProjectDPR_ReceivedAtHQDate =convert(date, '" + ReceivingDate + "', 103) where ProjectDPR_Id = '" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_ProjectDPR_Id + "'";
                    ds = ExecuteSelectQuerywithTransaction(cn, sql1, trans);

                }

                trans.Commit();
                cn.Close();
                iResult = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }
    private void Insert_tbl_ProjectDPRQuestionire(tbl_ProjectDPRQuestionire obj_tbl_ProjectDPRQuestionire, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectDPRQuestionire ( [ProjectDPRQuestionire_AddedBy],[ProjectDPRQuestionire_AddedOn],[ProjectDPRQuestionire_Answer],[ProjectDPRQuestionire_DPR_Id],[ProjectDPRQuestionire_Questionire_Id],[ProjectDPRQuestionire_Status],[ProjectDPRQuestionire_Work_Id],ProjectDPRQuestionire_Remark,ProjectDPRQuestionire_ProjectWorkPkg_Id ) values ('" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_AddedBy + "', getdate(), N'" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_Answer + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_DPR_Id + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_Questionire_Id + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_Status + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_Work_Id + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_Remark + "','" + obj_tbl_ProjectDPRQuestionire.ProjectDPRQuestionire_ProjectWorkPkg_Id + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    private void Insert_tbl_ProjectDPRStatus(tbl_ProjectDPRStatus obj_tbl_ProjectDPRStatus, SqlTransaction trans, SqlConnection cn)
    {
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectDPRStatus ( [ProjectDPRStatus_AddedBy],[ProjectDPRStatus_AddedOn],[ProjectDPRStatus_Comments],[ProjectDPRStatus_Date],[ProjectDPRStatus_DPR_StatusId],[ProjectDPRStatus_ProjectDPR_Id],[ProjectDPRStatus_Status],ProjectDPRStatus_ProjectWorkPkg_Id ) values ('" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_AddedBy + "', getdate(), N'" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_Comments + "', convert(date, '" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_Date + "', 103),'" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_DPR_StatusId + "','" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_ProjectDPR_Id + "','" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_Status + "','" + obj_tbl_ProjectDPRStatus.ProjectDPRStatus_ProjectWorkPkg_Id + "');Select @@Identity";
        if (trans == null)
        {
            try
            {
                ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
    }
    #endregion

    #region DPR Approval
    public bool Update_tbl_ProjectDPR_Approval(decimal Allocated_Budget, string GO_No, string GO_Date, File_Objects obj_File_Objects, List<tbl_ProjectDPR_PhysicalProgress> obj_tbl_ProjectDPR_PhysicalProgress_Li, List<tbl_ProjectDPR_Deliverables> obj_tbl_ProjectDPR_Deliverables_Li, string ProgressTrackingType)
    {
        DataSet ds = new DataSet();
        bool iResult = false;
        using (SqlConnection cn = new SqlConnection(ConStr))
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            SqlTransaction trans = cn.BeginTransaction();
            try
            {

                if (obj_File_Objects.File_Path_Bytes_1 != null && obj_File_Objects.File_Path_Bytes_1.Length > 0)
                {
                    File.WriteAllBytes(Server.MapPath(".") + obj_File_Objects.File_Path_1, obj_File_Objects.File_Path_Bytes_1);
                }
                else
                {
                    obj_File_Objects.File_Path_1 = "";
                }
                string sql = "set dateformat dmy; update tbl_ProjectWorkPkg set ProjectWorkPkg_Agreement_No = N'" + GO_No + "', ProjectWorkPkg_Agreement_Path = '" + obj_File_Objects.File_Path_1 + "', ProjectWorkPkg_Agreement_Date = convert(date, '" + GO_Date + "', 103) where ProjectWorkPkg_Id = '" + obj_File_Objects.Package_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = "set dateformat dmy; update tbl_ProjectDPR set ProjectDPR_BudgetAllocated = '" + Allocated_Budget + "', ProjectDPR_BudgetAllocatedBy = '" + obj_File_Objects.Added_By + "', ProjectDPR_BudgetAllocatedOn = getdate(), ProjectDPR_BudgetAllocationComments = N'" + obj_File_Objects.Comments + "', ProjectDPR_IsApproved = '1', ProjectDPR_Approved_Comments = N'" + obj_File_Objects.Comments + "', ProjectDPR_ApprovedBy = '" + obj_File_Objects.Added_By + "', ProjectDPR_ApprovedOn = getdate(), ProjectDPR_PhysicalProgressTrackingType ='" + ProgressTrackingType + "' where ProjectDPR_ProjectWork_Id = '" + obj_File_Objects.Work_Id + "' and ProjectDPR_ProjectWorkPkg_Id = '" + obj_File_Objects.Package_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                sql = "set dateformat dmy; update tbl_ProjectDPR_PhysicalProgress set ProjectDPR_PhysicalProgress_ModifiedOn = getdate(), ProjectDPR_PhysicalProgress_ModifiedBy = '" + obj_File_Objects.Added_By + "', ProjectDPR_PhysicalProgress_Status = 0 where ProjectDPR_PhysicalProgress_Status = 1 and  ProjectDPR_PhysicalProgress_ProjectDPR_Id = '" + obj_File_Objects.ProjectDPR_Id + "' and ProjectDPR_PhysicalProgress_PrjectWork_Id = '" + obj_File_Objects.Work_Id + "' and ProjectDPR_PhysicalProgress_ProjectWorkPkg_Id = '" + obj_File_Objects.Package_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);
                if (obj_tbl_ProjectDPR_PhysicalProgress_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectDPR_PhysicalProgress_Li.Count; i++)
                    {
                        obj_tbl_ProjectDPR_PhysicalProgress_Li[i].ProjectDPR_PhysicalProgress_ProjectDPR_Id = obj_File_Objects.ProjectDPR_Id;
                        obj_tbl_ProjectDPR_PhysicalProgress_Li[i].ProjectDPR_PhysicalProgress_PrjectWork_Id = obj_File_Objects.Work_Id;
                        obj_tbl_ProjectDPR_PhysicalProgress_Li[i].ProjectDPR_PhysicalProgress_ProjectWorkPkg_Id = obj_File_Objects.Package_Id;
                        Insert_tbl_ProjectDPR_PhysicalProgress(obj_tbl_ProjectDPR_PhysicalProgress_Li[i], trans, cn);
                    }
                }

                sql = "set dateformat dmy; update tbl_ProjectDPR_Deliverables set ProjectDPR_Deliverables_ModifiedOn = getdate(), ProjectDPR_Deliverables_ModifiedBy = '" + obj_File_Objects.Added_By + "', ProjectDPR_Deliverables_Status = 0 where ProjectDPR_Deliverables_Status = 1 and  ProjectDPR_Deliverables_ProjectDPR_Id = '" + obj_File_Objects.ProjectDPR_Id + "' and ProjectDPR_Deliverables_ProjectWork_Id = '" + obj_File_Objects.Work_Id + "' and ProjectDPR_Deliverables_ProjectWorkPkg_Id = '" + obj_File_Objects.Package_Id + "'";
                ds = ExecuteSelectQuerywithTransaction(cn, sql, trans);

                if (obj_tbl_ProjectDPR_Deliverables_Li != null)
                {
                    for (int i = 0; i < obj_tbl_ProjectDPR_Deliverables_Li.Count; i++)
                    {
                        obj_tbl_ProjectDPR_Deliverables_Li[i].ProjectDPR_Deliverables_ProjectDPR_Id = obj_File_Objects.ProjectDPR_Id;
                        obj_tbl_ProjectDPR_Deliverables_Li[i].ProjectDPR_Deliverables_ProjectWork_Id = obj_File_Objects.Work_Id;
                        obj_tbl_ProjectDPR_Deliverables_Li[i].ProjectDPR_Deliverables_ProjectWorkPkg_Id = obj_File_Objects.Package_Id;
                        Insert_tbl_ProjectDPR_Deliverables(obj_tbl_ProjectDPR_Deliverables_Li[i], trans, cn);
                    }
                }

                trans.Commit();
                cn.Close();
                iResult = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                cn.Close();
                iResult = false;
            }
        }
        return iResult;
    }
    private int Insert_tbl_ProjectDPR_Deliverables(tbl_ProjectDPR_Deliverables obj_tbl_ProjectDPR_Deliverables, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_Deliverables ( [ProjectDPR_Deliverables_AddedBy],[ProjectDPR_Deliverables_AddedOn],[ProjectDPR_Deliverables_ProjectWork_Id],[ProjectDPR_Deliverables_ProjectDPR_Id],[ProjectDPR_Deliverables_Deliverables_Id], [ProjectDPR_Deliverables_Status],ProjectDPR_Deliverables_ProjectWorkPkg_Id) values ('" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_AddedBy + "', getdate(),'" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_ProjectWork_Id + "','" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_Deliverables_Id + "', '" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_ProjectWorkPkg_Id + "', '" + obj_tbl_ProjectDPR_Deliverables.ProjectDPR_Deliverables_ProjectWorkPkg_Id + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    private int Insert_tbl_ProjectDPR_PhysicalProgress(tbl_ProjectDPR_PhysicalProgress obj_tbl_ProjectDPR_PhysicalProgress, SqlTransaction trans, SqlConnection cn)
    {
        DataSet ds = new DataSet();
        string strQuery = "";
        strQuery = " set dateformat dmy;insert into tbl_ProjectDPR_PhysicalProgress ( [ProjectDPR_PhysicalProgress_AddedBy],[ProjectDPR_PhysicalProgress_AddedOn],[ProjectDPR_PhysicalProgress_PrjectWork_Id],[ProjectDPR_PhysicalProgress_ProjectDPR_Id],[ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id], [ProjectDPR_PhysicalProgress_Status],ProjectDPR_PhysicalProgress_ProjectWorkPkg_Id) values ('" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_AddedBy + "', getdate(),'" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_PrjectWork_Id + "','" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_ProjectDPR_Id + "','" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id + "', '" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_Status + "', '" + obj_tbl_ProjectDPR_PhysicalProgress.ProjectDPR_PhysicalProgress_ProjectWorkPkg_Id + "'); select @@IDENTITY";
        if (trans == null)
        {
            try
            {
                ds = ExecuteSelectQuery(strQuery);
            }
            catch
            {
            }
        }
        else
        {
            ds = ExecuteSelectQuerywithTransaction(cn, strQuery, trans);
        }
        return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
    }
    #endregion

    #region Dashboard Data
    public DataSet get_Dashboard_Data(int Scheme_Id, int Zone_Id, string fromDate, string tillDate)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; ";

        strQuery += @"select 
	                    count(*) Total_ULB  
                    from tbl_Division 
                    join tbl_Circle on Circle_Id = Division_CircleId
                    where Division_Status = 1 District_IdCond_ULB;";

        strQuery += Environment.NewLine;
        strQuery += @"select SUM(Total_Project) Total_Project from (
                        select 
	                        count(*) Total_Project 
                        from tbl_ProjectWorkPkg 
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                        join tbl_Division on Division_Id = ProjectWork_DivisionId
                        join tbl_Circle on Circle_Id = Division_CircleId
                        full outer join (
	                        select 
		                        FinancialTrans_Work_Id,
		                        FinancialTrans_Package_Id
	                        from tbl_FinancialTrans where FinancialTrans_Status = 1 
	                        group by FinancialTrans_Work_Id, FinancialTrans_Package_Id
                        ) tFYData on FinancialTrans_Package_Id = ProjectWorkPkg_Id and FinancialTrans_Work_Id = ProjectWorkPkg_Work_Id 
                        where ProjectWorkPkg_Status = 1 Scheme_IdCond1 District_IdCond_Project DateCond_Project
                        ) tData;";

        strQuery += Environment.NewLine;
        strQuery += @"select SUM(Budget_Allocated) Budget_Allocated from (
                    select 
	                    convert(decimal(18, 3), sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) / 100) Budget_Allocated 
                    from tbl_ProjectWorkPkg 
	                join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
	                join tbl_Division on Division_Id = ProjectWork_DivisionId
	                join tbl_Circle on Circle_Id = Division_CircleId
                    where 1 = 1 Scheme_IdCond3 District_IdCond_Budget  DateCond_Project  
                ) tData;";

        strQuery += Environment.NewLine;
        strQuery += @"select SUM(TransAmount_C) TransAmount_C, SUM(TransAmount_D) TransAmount_D from (
                        select 
	                        TransAmount_C = convert(decimal(18, 3), sum(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end) / 10000000), 
	                        TransAmount_D = convert(decimal(18, 3), sum(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end) / 10000000) 
                        from tbl_FinancialTrans 
                        join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = FinancialTrans_Package_Id and ProjectWorkPkg_Status = 1
						join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
                        where FinancialTrans_Status = 1 Scheme_IdCond2 District_IdCond_Fund  DateCond_Project 
                    ) tData;";

        strQuery += Environment.NewLine;
        strQuery += @"select 
	                    count(*) Total_UC, 
	                    AVG_UC_Percentage = convert(decimal(18,2), sum(isnull(ProjectUC_Achivment, 0)) / count(*)),
	                    BudgetUtilized = sum(isnull(ProjectUC_BudgetUtilized, 0)) / 100000,
	                    Physical_Progress_Percentage = convert(decimal(18,0), sum(isnull(ProjectUC_PhysicalProgress, 0)) / count(*))
                    from 
                    (
	                    select 
		                    ROW_NUMBER() over (partition by ProjectUC_ProjectPkg_Id, ProjectUC_ProjectWork_Id order by ProjectUC_Id desc) rr, 
		                    ProjectUC_Achivment,
		                    ProjectUC_SubmitionDate,
		                    ProjectUC_BudgetUtilized,
		                    ProjectUC_PhysicalProgress,
		                    ProjectUC_Total_Allocated 
	                    from tbl_ProjectUC 
	                    join tbl_ProjectWorkPkg on ProjectUC_ProjectPkg_Id = ProjectWorkPkg_Id and ProjectWorkPkg_Status = 1
	                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
	                    where ProjectUC_Status = 1 Scheme_IdCond4 District_IdCond_UC  DateCond_Project
                    ) tData where tData.rr = 1 ";
        //strQuery = strQuery.Replace("Financial_Id_P_Cond", FinancialYear_Id.ToString());
        //strQuery = strQuery.Replace("FinancialYearIdCondF", "and FinancialTrans_FinancialYear_Id = " + FinancialYear_Id.ToString());
        //strQuery = strQuery.Replace("FinancialYear_IdCond", "and ProjectWork_FinancialYear_Id = " + FinancialYear_Id.ToString());
        //strQuery1 = strQuery1.Replace("FinancialYear_IdCond", "and ProjectWork_FinancialYear_Id = " + FinancialYear_Id.ToString());
        //strQuery2 = strQuery2.Replace("FinancialYear_IdCond", "and ProjectWork_FinancialYear_Id = " + FinancialYear_Id.ToString());
        //strQuery3 = strQuery3.Replace("FinancialYear_IdCond", "and ProjectWork_FinancialYear_Id = " + FinancialYear_Id.ToString());

        if (Scheme_Id != 0)
        {
            strQuery = strQuery.Replace("Scheme_IdCond1", "and ProjectWork_Project_Id = " + Scheme_Id.ToString());
            strQuery = strQuery.Replace("Scheme_IdCond2", "and ProjectWork_Project_Id = " + Scheme_Id.ToString());
            strQuery = strQuery.Replace("Scheme_IdCond3", "and ProjectWork_Project_Id = " + Scheme_Id.ToString());
            strQuery = strQuery.Replace("Scheme_IdCond4", "and ProjectWork_Project_Id = " + Scheme_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("Scheme_IdCond1", "");
            strQuery = strQuery.Replace("Scheme_IdCond2", "");
            strQuery = strQuery.Replace("Scheme_IdCond3", "");
            strQuery = strQuery.Replace("Scheme_IdCond4", "");
        }
        if (Zone_Id != 0)
        {
            strQuery = strQuery.Replace("District_IdCond_ULB", "and Circle_ZoneId = " + Zone_Id.ToString());
            strQuery = strQuery.Replace("District_IdCond_Project", "and Circle_ZoneId = " + Zone_Id.ToString());
            strQuery = strQuery.Replace("District_IdCond_Budget", "and Circle_ZoneId = " + Zone_Id.ToString());
            strQuery = strQuery.Replace("District_IdCond_Fund", "and Circle_ZoneId = " + Zone_Id.ToString());
            strQuery = strQuery.Replace("District_IdCond_UC", "and Circle_ZoneId = " + Zone_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("District_IdCond_ULB", "");
            strQuery = strQuery.Replace("District_IdCond_Project", "");
            strQuery = strQuery.Replace("District_IdCond_Budget", "");
            strQuery = strQuery.Replace("District_IdCond_Fund", "");
            strQuery = strQuery.Replace("District_IdCond_UC", "");
        }
        if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(tillDate))
        {
            strQuery = strQuery.Replace("DateCond_Project", " and Convert(date, ProjectWorkPkg_Agreement_Date, 103)  between CONVERT(date, Convert(varchar(10),'" + fromDate + "', 103), 103) And CONVERT(date, Convert(varchar(10),'" + tillDate + "', 103), 103) ");
        }
        else
        {
            strQuery = strQuery.Replace("DateCond_Project", " ");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report SVG Map Data
    public DataSet get_SVG_Map_Data(string fromDate, string tillDate, int Project_Id, int Zone_Id, int FinancialYear_Id, string DataMode)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        string _Step_Cond = "";
        if (DataMode == "1")
        {
            _Step_Cond = "@step = (max(convert(decimal(18, 3), tDPR.Total_Work)) - min(convert(decimal(18, 3), tDPR.Total_Work))) / 10";
        }
        if (DataMode == "2")
        {
            _Step_Cond = "@step = (max(convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated)) - min(convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated))) / 10";
        }
        if (DataMode == "3")
        {
            _Step_Cond = "@step = (max(convert(decimal(18, 3), tStatement.TransAmount_C)) - min(convert(decimal(18, 3), tStatement.TransAmount_C))) / 10";
        }
        strQuery = @"set dateformat dmy; 

                    declare @step decimal(18,2) = 0;
                    declare @stepF int = 0;

                    select 
	                    _Step_Cond_Replacement
                    from M_Jurisdiction
                    left join 
                    (
	                    select 
		                    ProjectWork_DistrictId, 
		                    count(*) Total_Work,
		                    count(distinct ProjectWork_DivisionId) Total_ULB, 
		                    sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) ProjectDPR_BudgetAllocated 
	                    from tbl_ProjectWorkPkg 
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
	                    where ProjectWorkPkg_Status = 1 
	                    group by ProjectWork_DistrictId
                    ) tDPR on tDPR.ProjectWork_DistrictId = M_Jurisdiction_Id 
                    left join 
                    (
	                    select 
		                    ProjectWork_DistrictId, 
		                    TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then (isnull(FinancialTrans_TransAmount, 0) / 100000) else 0 end), 
		                    TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then (isnull(FinancialTrans_TransAmount, 0) / 100000) else 0 end) 
	                    from tbl_FinancialTrans 
	                    join tbl_ProjectWorkPkg on FinancialTrans_Package_Id = ProjectWorkPkg_Id 
						join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
	                    where FinancialTrans_Status = 1 
	                    group by ProjectWork_DistrictId
                    ) tStatement on tStatement.ProjectWork_DistrictId = M_Jurisdiction_Id
                    where M_Level_Id = 3 and Is_Active = 1; 

                    set @stepF = convert(int, ROUND(@step, 0))
                    set @stepF = ROUND(@stepF, (len(@stepF) -1) * -1)

                    ; with cte as (
	                    select 
		                    M_Jurisdiction_Id,
		                    Jurisdiction_Name_Eng,
		                    tDPR.Total_ULB, 
		                    tDPR.Total_Work, 
		                    ProjectDPR_BudgetAllocated = convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated),
		                    Fund_Released = convert(decimal(18, 3), isnull(tStatement.TransAmount_C, 0) / 100000), 
		                    Expenditure = convert(decimal(18, 3), isnull(tStatement.TransAmount_D, 0) / 100000), 
		                    Balance = convert(decimal(18, 3), ((isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0)) / 100000)), 
		                    SVGMapping_Name, 
		                    Step_Id = case when (convert(int, convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated) / @stepF) + 1) > 10 then 10 else (convert(int, convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated) / @stepF) + 1) end
	                    from M_Jurisdiction
	                    left join 
	                    (
		                    select 
			                    ProjectWork_DistrictId, 
			                    count(*) Total_Work,
			                    count(distinct ProjectWork_DivisionId) Total_ULB, 
			                    sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) ProjectDPR_BudgetAllocated 
		                    from tbl_ProjectWorkPkg 
							join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
							join tbl_Division on Division_Id = ProjectWork_DivisionId
							join tbl_Circle on Circle_Id = Division_CircleId
							where ProjectWorkPkg_Status = 1
		                    group by ProjectWork_DistrictId
	                    ) tDPR on tDPR.ProjectWork_DistrictId = M_Jurisdiction_Id 
	                    left join 
	                    (
		                    select 
			                    ProjectWork_DistrictId, 
			                    TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then isnull(FinancialTrans_TransAmount, 0) else 0 end), 
			                    TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then isnull(FinancialTrans_TransAmount, 0) else 0 end) 
		                    from tbl_FinancialTrans 
		                    join tbl_ProjectWorkPkg on FinancialTrans_Package_Id = ProjectWorkPkg_Id 
							join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
							join tbl_Division on Division_Id = ProjectWork_DivisionId
							join tbl_Circle on Circle_Id = Division_CircleId
		                    where FinancialTrans_Status = 1 
		                    group by ProjectWork_DistrictId
	                    ) tStatement on tStatement.ProjectWork_DistrictId = M_Jurisdiction_Id
	                    join tbl_SVGMapping on SVGMapping_DistrictId = M_Jurisdiction_Id and SVGMapping_Status = 1
	                    where M_Level_Id = 3 and Is_Active = 1 
                    )
                    select *, ColorCodes_Code from cte 
                    join tbl_ColorCodes on ColorCodes_Id = Step_Id
                    order by cte.ProjectDPR_BudgetAllocated; ";

        strQuery += "select * from tbl_SVGMapping where SVGMapping_Status = 1 and SVGMapping_DistrictId = 0;";

        strQuery += "select *, convert(int, @step * (ColorCodes_Id -1)) From_Range, convert(int, @step * ColorCodes_Id) Till_Range from tbl_ColorCodes;";

        strQuery = strQuery.Replace("_Step_Cond_Replacement", _Step_Cond);

        if (FinancialYear_Id > 0)
        {
            strQuery = strQuery.Replace("FinancialYear_Id1", "and ProjectWork_FinancialYear_Id = " + FinancialYear_Id.ToString());
            strQuery = strQuery.Replace("FinancialYear_Id2", " and FinancialTrans_FinancialYear_Id = " + FinancialYear_Id + "");
        }
        else
        {
            strQuery = strQuery.Replace("FinancialYear_Id1", "");
            strQuery = strQuery.Replace("FinancialYear_Id2", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    #region Report Scheme
    public DataSet get_Scheme_Wise_Details(int Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery = @"set dateformat dmy; 
                    select 
	                    Project_Id,
                        Project_Name,
                        tDPR.Total_Division, 
                        Total_Work = isnull(tDPR.Total_Work, 0), 
                        ProjectDPR_BudgetAllocated = convert(decimal(18, 3), isnull(tDPR.ProjectDPR_BudgetAllocated, 0)),
                        Fund_Released = convert(decimal(18, 3), isnull(tStatement.TransAmount_C, 0)), 
                        Expenditure = convert(decimal(18, 3), isnull(tStatement.TransAmount_D, 0)), 
                        Balance = convert(decimal(18, 3), ((isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0))))
                    from tbl_Project
                    left join 
                    (
	                    select 
		                    tbl_ProjectWork.ProjectWork_Project_Id ProjectDPR_Project_Id, 
		                    count(distinct ProjectWork_Id) Total_Work,
		                    count(distinct ProjectWork_DivisionId) Total_Division, 
		                    sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) ProjectDPR_BudgetAllocated 
	                    from tbl_ProjectWorkPkg 
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                        join tbl_Division on Division_Id = ProjectWork_DivisionId
                        join tbl_Circle on Circle_Id = Division_CircleId
                        full outer join (
	                        select 
								ProjectWork_Project_Id,
		                        FinancialTrans_Work_Id,
		                        FinancialTrans_Package_Id
	                        from tbl_FinancialTrans 
							join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = FinancialTrans_Package_Id 
							join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
							where FinancialTrans_Status = 1 
	                        group by FinancialTrans_Work_Id, FinancialTrans_Package_Id, ProjectWork_Project_Id
                        ) tFYData on FinancialTrans_Package_Id = ProjectWorkPkg_Id and FinancialTrans_Work_Id = ProjectWorkPkg_Work_Id 
	                    where ProjectWorkPkg_Status = 1 Zone_IdCond 
	                    group by tbl_ProjectWork.ProjectWork_Project_Id
                    ) tDPR on tDPR.ProjectDPR_Project_Id = Project_Id 
                    left join 
                    (
	                    select 
		                    ProjectWork_Project_Id, 
		                    TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then isnull(FinancialTrans_TransAmount, 0) else 0 end), 
		                    TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then isnull(FinancialTrans_TransAmount, 0) else 0 end) 
	                    from tbl_FinancialTrans 
                        join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = FinancialTrans_Package_Id and ProjectWorkPkg_Status = 1
						join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
	                    where FinancialTrans_Status = 1 Zone_IdCond 
	                    group by ProjectWork_Project_Id
                    ) tStatement on tStatement.ProjectWork_Project_Id = Project_Id 
                    where 1 = 1 ";

        if (Zone_Id != 0)
        {
            strQuery = strQuery.Replace("Zone_IdCond", "and Circle_ZoneId = " + Zone_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("Zone_IdCond", "");
        }

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    #endregion

    public DataSet get_District_Wise_Details(int Project_Id, int Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    Zone_Id,
	                    Zone_Name,
	                    tDPR.Total_Division, 
	                    tDPR.Total_Work, 
	                    ProjectDPR_BudgetAllocated = convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated),
	                    Fund_Released = convert(decimal(18, 3), isnull(tStatement.TransAmount_C, 0)), 
	                    Expenditure = convert(decimal(18, 3), isnull(tStatement.TransAmount_D, 0)), 
                        Balance = convert(decimal(18, 3), ((isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0))))
                    from tbl_Zone
                    left join 
                    (
	                    select 
	                        Circle_ZoneId, 
	                        count(distinct ProjectWork_Id) Total_Work,
		                    count(distinct ProjectWork_DivisionId) Total_Division, 
		                    sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) ProjectDPR_BudgetAllocated 
                        from tbl_ProjectWorkPkg 
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                        join tbl_Division on Division_Id = ProjectWork_DivisionId
                        join tbl_Circle on Circle_Id = Division_CircleId
	                    where ProjectWorkPkg_Status = 1 Project_IdCond
	                    group by Circle_ZoneId
                    ) tDPR on tDPR.Circle_ZoneId = Zone_Id 
                    left join 
                    (
	                    select 
		                    Circle_ZoneId, 
		                    TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then isnull(FinancialTrans_TransAmount, 0) else 0 end), 
		                    TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then isnull(FinancialTrans_TransAmount, 0) else 0 end) 
	                    from tbl_FinancialTrans 
                        join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = FinancialTrans_Package_Id and ProjectWorkPkg_Status = 1
						join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
                        where 1=1 Project_IdCond
	                    group by Circle_ZoneId
                    ) tStatement on tStatement.Circle_ZoneId = Zone_Id
                    where Zone_Status = 1 
                    order by Zone_Name";

        if (Project_Id != 0)
        {
            strQuery = strQuery.Replace("Project_IdCond", "and ProjectWork_Project_Id = " + Project_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("Project_IdCond", "");
        }
        //if (Zone_Id != 0)
        //{
        //    strQuery = strQuery.Replace("District_IdCond", "and ProjectDPR_District_Jurisdiction_Id = " + District_Id.ToString());
        //    strQuery = strQuery.Replace("Global_DistrictId_Cond", "and M_Jurisdiction_Id = " + District_Id.ToString());
        //}
        //else
        //{
        //    strQuery = strQuery.Replace("District_IdCond", "");
        //    strQuery = strQuery.Replace("Global_DistrictId_Cond", "");
        //}

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_Circle_Wise_Details(int Project_Id, int Zone_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    tbl_Circle.Circle_Id,
	                    Circle_Name,
	                    tDPR.Total_Division, 
	                    tDPR.Total_Work, 
	                    ProjectDPR_BudgetAllocated = convert(decimal(18, 3), tDPR.ProjectDPR_BudgetAllocated),
	                    Fund_Released = convert(decimal(18, 3), isnull(tStatement.TransAmount_C, 0)), 
	                    Expenditure = convert(decimal(18, 3), isnull(tStatement.TransAmount_D, 0)), 
                        Balance = convert(decimal(18, 3), ((isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0))))
                    from tbl_Circle
                    left join 
                    (
	                    select 
	                        Circle_Id, 
	                        count(distinct ProjectWork_Id) Total_Work,
		                    count(distinct ProjectWork_DivisionId) Total_Division, 
		                    sum(isnull(ProjectWorkPkg_AgreementAmount, 0)) ProjectDPR_BudgetAllocated 
                        from tbl_ProjectWorkPkg 
                        join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                        join tbl_Division on Division_Id = ProjectWork_DivisionId
                        join tbl_Circle on Circle_Id = Division_CircleId
	                    where ProjectWorkPkg_Status = 1 Project_IdCond
	                    group by Circle_Id
                    ) tDPR on tDPR.Circle_Id = tbl_Circle.Circle_Id 
                    left join 
                    (
	                    select 
		                    Circle_Id, 
		                    TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then isnull(FinancialTrans_TransAmount, 0) else 0 end), 
		                    TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then isnull(FinancialTrans_TransAmount, 0) else 0 end) 
	                    from tbl_FinancialTrans 
                        join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = FinancialTrans_Package_Id and ProjectWorkPkg_Status = 1
						join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id  
						join tbl_Division on Division_Id = ProjectWork_DivisionId
						join tbl_Circle on Circle_Id = Division_CircleId
                        where 1=1 Project_IdCond
	                    group by Circle_Id
                    ) tStatement on tStatement.Circle_Id = tbl_Circle.Circle_Id
                    where Circle_Status = 1 Zone_IdCond 
                    order by Circle_Name";

        if (Project_Id != 0)
        {
            strQuery = strQuery.Replace("Project_IdCond", "and ProjectWork_Project_Id = " + Project_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("Project_IdCond", "");
        }
        if (Zone_Id != 0)
        {
            strQuery = strQuery.Replace("Zone_IdCond", "and tbl_Circle.Circle_ZoneId = " + Zone_Id.ToString());
        }
        else
        {
            strQuery = strQuery.Replace("Zone_IdCond", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectDPR_Details(int ProjectWorkPkg_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery = @"set dateformat dmy; 
                    select 
                        distinct
                        Project_Name,
	                    ProjectDPR_Id,
	                    ProjectDPR_Project_Id,
	                    ProjectWorkPkg_Id,
						ProjectWorkPkg_Work_Id,
                        ProjectWorkPkg_Code, 
						ProjectWorkPkg_Name,
                        ProjectWorkPkg_Name_Code = isnull(ProjectWorkPkg_Code, '') + ' - ' + ProjectWorkPkg_Name,
						ProjectWorkPkg_AgreementAmount,
						ProjectWorkPkg_Agreement_Date = convert(char(10), ProjectWorkPkg_Agreement_Date, 103),
                        ProjectWorkPkg_Due_Date = convert(char(10), ProjectWorkPkg_Due_Date, 103), 
						ProjectWorkPkg_Agreement_No,
						Project_Name, 
                        ProjectWork_ProjectCode,
                        ProjectWork_Name, 
                        ProjectWork_Description, 
                        ProjectWork_GO_Path,
                        ProjectWork_Budget,
                        ProjectWork_AddedOn, 
                        ProjectWork_AddedBy, 
                        ProjectWork_ModifiedOn, 
                        ProjectWork_ModifiedBy, 
                        ProjectWork_Status, 
						Division_Name, 
						Circle_Name, 
						ProjectWork_DistrictId, 
						ProjectWork_ULB_Id, 
						ProjectWork_DivisionId, 
						Division_CircleId,
	                    ProjectDPR_DPRPDFPath,
	                    ProjectDPR_DocumentDesignPath,
	                    ProjectDPR_IsVerified,
	                    ProjectDPR_BudgetAllocated = convert(decimal(18,2), ProjectDPR_BudgetAllocated / 100000),
	                    ProjectDPR_ProjectWork_Id,
	                    ProjectWork_Name,
	                    ProjectWork_Id,
	                    ProjectWork_Project_Id,
	                    ProjectWork_GO_Path,
	                    ProjectWork_Budget,
	                    ProjectWork_GO_Date = convert(char(10), ProjectWork_GO_Date, 103),
	                    ProjectWork_GO_No,
	                    Vendor_Name = tbl_ProjectPkgTenderInfo.Person_Name, 
	                    tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_Id, 
	                    Total_Release = case when isnull(tStatement.TransAmount_C, 0) > 0 then convert(decimal(18,2), isnull(tStatement.TransAmount_C, 0) / 100000) else 0 end, 
	                    Total_Expenditure = case when isnull(tStatement.TransAmount_D, 0) > 0 then convert(decimal(18,2), isnull(tStatement.TransAmount_D, 0) / 100000) else 0 end, 
	                    Total_Available = case when isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0) > 0 then convert(decimal(18,2), (isnull(tStatement.TransAmount_C, 0) - isnull(tStatement.TransAmount_D, 0)) / 100000) else 0 end
                    from tbl_ProjectWorkPkg
					join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_ProjectDPR on ProjectDPR_ProjectWorkPkg_Id = ProjectWorkPkg_Id 
                    join tbl_Project on Project_Id = ProjectWork_Project_Id 
                    left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId
                    left join (select ROW_NUMBER() over (partition by ProjectPkgTenderInfo_ProjectPkg_Id, ProjectPkgTenderInfo_ProjectWork_Id order by ProjectPkgTenderInfo_Id desc) rrrT, ProjectPkgTenderInfo_ProjectPkg_Id,ProjectPkgTenderInfo_Id, ProjectPkgTenderInfo_ProjectWork_Id, ProjectPkgTenderInfo_VendorPersonId, Person_Name from tbl_ProjectPkgTenderInfo join tbl_PersonDetail on Person_Id = ProjectPkgTenderInfo_VendorPersonId) tbl_ProjectPkgTenderInfo on tbl_ProjectPkgTenderInfo.ProjectPkgTenderInfo_ProjectPkg_Id = ProjectWorkPkg_Id and  tbl_ProjectPkgTenderInfo.rrrT = 1
                    left join (select  FinancialTrans_Package_Id, FinancialTrans_Work_Id, TransAmount_C = sum(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end), TransAmount_D = sum(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end) from tbl_FinancialTrans where FinancialTrans_Status = 1 group by FinancialTrans_Package_Id, FinancialTrans_Work_Id) tStatement on tStatement.FinancialTrans_Package_Id = ProjectWorkPkg_Id 
                    where ProjectDPR_Status = 1 and ProjectWorkPkg_Id = ProjectDPR_IdCond";

        strQuery += Environment.NewLine;
        strQuery += @"select 
                        FinancialTrans_Id,
                        FinancialTrans_Date = convert(char(10), FinancialTrans_Date, 103),
	                    FinancialTrans_EntryType,
	                    FinancialTrans_Comments,
	                    FinancialTrans_FilePath1,
	                    FinancialTrans_GO_Date = case when FinancialTrans_TransType = 'C' then convert(char(10), FinancialTrans_GO_Date, 103) else convert(char(10), FinancialTrans_Date, 103) end,
	                    FinancialTrans_GO_Number,
	                    TransAmount_C = convert(decimal(18,2), (isnull(case when FinancialTrans_TransType = 'C' then FinancialTrans_TransAmount else 0 end, 0) / 100000)), 
	                    TransAmount_D = convert(decimal(18,2), (isnull(case when FinancialTrans_TransType = 'D' then FinancialTrans_TransAmount else 0 end, 0) / 100000))
                    from tbl_FinancialTrans
                    where FinancialTrans_Status = 1 and FinancialTrans_Package_Id = ProjectDPR_IdCond
                    order by FinancialTrans_Id; ";

        strQuery += Environment.NewLine;
        strQuery += @"select 
	                    ProjectDPR_Id, 
						ProjectDPR_DPRPDFPath,
                        ProjectWork_GO_Path,
                        ProjectUC_Id,
                        ProjectUC_FilePath1, 
                        ProjectUC_FilePath2, 
                        ProjectUC_BudgetUtilized = convert(decimal(18,2), isnull(ProjectUC_BudgetUtilized, 0) / 100000), 
                        ProjectUC_Achivment, 
                        ProjectUC_PhysicalProgress,
                        ProjectDPR_PhysicalProgressTrackingType,
                        ProjectUC_Centage=convert(decimal(18,2), isnull(ProjectUC_Centage, 0) / 100000),
                        ProjectUC_Total_Allocated = convert(decimal(18, 3), ProjectUC_Total_Allocated / 100000),
	                    ProjectWork_GO_No,
	                    ProjectWork_GO_Date1 = convert(char(10), ProjectWork_GO_Date, 103), 
                        ProjectUC_SubmitionDate1 = convert(char(10), ProjectUC_SubmitionDate, 103),
                        ProjectUC_Latitude,
	                    ProjectUC_Longitude,
						Division_Name, 
						Circle_Name, 
						ProjectWork_DivisionId, 
						Division_CircleId
                    from tbl_ProjectUC 
					join tbl_ProjectDPR on ProjectDPR_ProjectWorkPkg_Id = ProjectUC_ProjectPkg_Id and ProjectUC_ProjectWork_Id=ProjectDPR_ProjectWork_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectDPR_ProjectWork_Id and ProjectWork_Status = 1
					left join tbl_Division on Division_Id = ProjectWork_DivisionId
					left join tbl_Circle on Circle_Id = Division_CircleId
                    where ProjectUC_Status = 1 and ProjectUC_ProjectPkg_Id = ProjectDPR_IdCond 
                    Order by Circle_Name,Division_Name ";

        strQuery += Environment.NewLine;
        strQuery += @"select 
	                    Person_Name,
	                    UserType_Desc_E,
	                    ProjectDPRSitePics_Id,
	                    ProjectDPRSitePics_ProjectDPR_Id,
	                    ProjectDPRSitePics_ProjectWork_Id,
	                    ProjectDPRSitePics_ReportSubmittedBy_PersonId,
	                    ProjectDPRSitePics_ReportSubmitted,
	                    ProjectDPRSitePics_SitePic_Path1,
	                    ProjectDPRSitePics_SitePic_Type = case when ProjectDPRSitePics_SitePic_Type = 'B' then 'Before' else 'After' end,
	                    ProjectDPRSitePics_AddedOn = convert(char(10), ProjectDPRSitePics_AddedOn, 103)
                    from tbl_ProjectDPRSitePics
                    join tbl_PersonDetail on Person_Id = ProjectDPRSitePics_ReportSubmittedBy_PersonId
                    join tbl_PersonJuridiction on Person_Id = PersonJuridiction_PersonId
                    left join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    where ProjectDPRSitePics_Status = 1 and ProjectDPRSitePics_ProjectDPR_Id = ProjectDPR_IdCond ; ";

        strQuery += Environment.NewLine;
        strQuery += @"select 
	                    Person_Id,
	                    UserType_Desc_E, 
	                    Person_Name, 
	                    Person_Mobile1, 
	                    Person_Mobile2,
	                    Designation_DesignationName
                    from tbl_ProjectPKGInspectionInfo
                    join tbl_PersonDetail on Person_Id = ProjectPKGInspectionInfo_InspectionPersonId
                    join tbl_PersonJuridiction on PersonJuridiction_PersonId = ProjectPKGInspectionInfo_InspectionPersonId
                    left join tbl_Designation on Designation_Id = PersonJuridiction_DesignationId
                    join tbl_UserType on UserType_Id = PersonJuridiction_UserTypeId
                    where ProjectPKGInspectionInfo_Status = 1 and ProjectPKGInspectionInfo_ProjectPkg_Id = ProjectDPR_IdCond ; ";

        strQuery += Environment.NewLine;
        strQuery += @"select top 1 
                        ProjectPkgTenderInfo_Id,
                        ProjectPkgTenderInfo_ProjectPkg_Id,
                        ProjectPkgTenderInfo_ProjectWork_Id,
                        ProjectPkgTenderInfo_VendorPersonId,
                        ProjectDPRTenderInfo_TenderAmount = convert(decimal(18, 3), ProjectPkgTenderInfo_TenderAmount / 100000),
                        ProjectDPRTenderInfo_TenderDate = convert(char(10), ProjectPkgTenderInfo_TenderDate, 103),
                        ProjectPkgTenderInfo_Comments,
                        ProjectPkgTenderInfo_AddedOn,
                       ProjectPkgTenderInfo_AddedBy,
                        ProjectPkgTenderInfo_Status,
                        ProjectPkgTenderInfo_ModifiedBy,
                        ProjectPkgTenderInfo_ModifiedOn,
                        ProjectDPRTenderInfo_NITDate = convert(char(10), ProjectPkgTenderInfo_NITDate, 103),
                        ProjectPkgTenderInfo_TenderStatus, 
                        Vendor_Person_Name = isnull(Person_Name, '') + ', Mob: ' + ISNULL(Person_Mobile1, ''),
                       ProjectPkgTenderInfo_CompletionTime,
                        ProjectPkgTenderInfo_Centage,
                        ProjectDPRTenderInfo_WorkCostIn = convert(decimal(18, 3), ProjectPkgTenderInfo_WorkCostIn / 100000),
                        ProjectDPRTenderInfo_WorkCostOut = convert(decimal(18, 3), ProjectPkgTenderInfo_WorkCostOut / 100000),
                        ProjectPkgTenderInfo_GSTNotIncludeWorkCost,
                        ProjectDPRTenderInfo_PrebidMeetingDate = convert(char(10), ProjectPkgTenderInfo_PrebidMeetingDate, 103),
                        ProjectDPRTenderInfo_TenderOutDate = convert(char(10), ProjectPkgTenderInfo_TenderOutDate, 103),
                        ProjectDPRTenderInfo_TenderTechnicalDate = convert(char(10), ProjectPkgTenderInfo_TenderTechnicalDate, 103),
                        ProjectDPRTenderInfo_TenderFinancialDate = convert(char(10), ProjectPkgTenderInfo_TenderFinancialDate, 103),
                        ProjectDPRTenderInfo_ContractSignDate = convert(char(10), ProjectPkgTenderInfo_ContractSignDate, 103),
                        ProjectPkgTenderInfo_ContractBondNo
                    from tbl_ProjectPkgTenderInfo
                    join tbl_PersonDetail on Person_Id = ProjectPkgTenderInfo_VendorPersonId
                    where ProjectPkgTenderInfo_Status = 1 and ProjectPkgTenderInfo_ProjectPkg_Id = ProjectDPR_IdCond  order by ProjectPkgTenderInfo_Id desc; ";

        if (ProjectWorkPkg_Id != 0)
        {
            strQuery = strQuery.Replace("ProjectDPR_IdCond", ProjectWorkPkg_Id.ToString());
        }

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectDPR_UC_Report(int ProjectUC_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    District_Name = M_Jurisdiction.Jurisdiction_Name_Eng, 
	                    ULB_Name = M_Jurisdiction_NP.ULB_Name, 
	                    Project_Name, 
	                    ProjectWork_GO_No,
	                    ProjectWork_GO_Date = convert(char(10), ProjectWork_GO_Date, 103), 
	                    ProjectUC_Id,
	                    ProjectUC_Achivment,
                        ProjectDPR_BudgetAllocated,
	                    ProjectUC_SubmitionDate = convert(char(10), ProjectUC_SubmitionDate, 103), 
	                    ProjectUC_BudgetUtilized,
	                    ProjectUC_Comments,
	                    ProjectUC_PhysicalProgress,
	                    ProjectUC_Total_Allocated, 
	                    ULB_Type
                    from tbl_ProjectUC 
                    join tbl_ProjectDPR on ProjectDPR_Id = ProjectUC_ProjectDPR_Id  
                    join tbl_ProjectWork on ProjectWork_Id = ProjectUC_ProjectWork_Id
                    join tbl_Project on ProjectDPR_Project_Id = Project_Id 
                    join M_Jurisdiction on ProjectDPR_District_Jurisdiction_Id = M_Jurisdiction_Id 
                    join tbl_ULB M_Jurisdiction_NP on M_Jurisdiction_NP.ULB_Id = tbl_ProjectDPR.ProjectDPR_NP_JurisdictionId
                    where ProjectUC_Id = " + ProjectUC_Id + "";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectDPR_UC_Questionire_Form(int ProjectUC_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    select 
	                    ProjectQuestionnaire_Name, 
	                    ProjectAnswer_Name = case when isnull(ProjectUC_Concent_Answer_Id, 0) = 0 then ProjectUC_Concent_Answer else ProjectAnswer_Name end
                    from  tbl_ProjectUC_Concent 
                    join tbl_ProjectQuestionnaire on ProjectQuestionnaire_Id = ProjectUC_Concent_Questionire_Id
                    left join tbl_ProjectAnswer on ProjectAnswer_Id = ProjectUC_Concent_Answer_Id
                    where ProjectUC_Concent_Status = 1 and ProjectUC_Concent_ProjectUC_Id = " + ProjectUC_Id + "";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
    public DataSet get_tbl_ProjectDPR_UC_PhysicalProgressComponentAndDeliverables_Form(int ProjectDPR_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"select PhysicalProgressComponent_Component,Unit_Name,ProjectUC_PhysicalProgress_PhysicalProgress 
                    from tbl_ProjectUC_PhysicalProgress
                    inner join tbl_PhysicalProgressComponent on PhysicalProgressComponent_Id=ProjectUC_PhysicalProgress_PhysicalProgressComponent_Id
                    left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
                    where ProjectUC_PhysicalProgress_Status=1 and ProjectUC_PhysicalProgress_ProjectDPR_Id='" + ProjectDPR_Id + "'";
        strQuery += @" select Deliverables_Deliverables,Unit_Name,ProjectUC_Deliverables_Deliverables 
                        from tbl_ProjectUC_Deliverables
                        inner join tbl_Deliverables on Deliverables_Id=ProjectUC_Deliverables_Deliverables_Id
                        left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                        where ProjectUC_Deliverables_Status=1 and ProjectUC_Deliverables_ProjectDPR_Id='" + ProjectDPR_Id + "'";
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_tbl_ProjectDPRRequest_Details(int ProjectDPR_Id)
    {
        string strQuery = "";
        DataSet ds = new DataSet();

        strQuery = @"set dateformat dmy; 
                    select 
	                    	ProjectWork_Id,
	                        ProjectWork_Project_Id,
							Project_Name,
	                        ProjectWork_Name,
	                        ProjectWork_GO_Path,
	                        ProjectWork_Budget,
                            ProjectWork_ProjectType_Id,
	                        convert(char(10), ProjectWork_Target_Date, 103) ProjectWork_Target_Date,
	                        convert(char(10), ProjectWork_GO_Date, 103) ProjectWork_GO_Date,
	                        ProjectWork_GO_No,	
	                        ProjectDPR_Id,
	                        ProjectDPR_District_Jurisdiction_Id,
	                        ProjectDPR_NP_JurisdictionId,
                            ProjectDPR_LokSabha_Jurisdiction_Id,
                            ProjectDPR_VidhanSabha_Jurisdiction_Id,
                            ProjectDPR_PhysicalProgressTrackingType,
	                        ProjectDPR_Comments,
                            ProjectDPR_RefrenceNo,
	                        ProjectDPR_BudgetAllocated=cast(isnull(ProjectDPR_BudgetAllocated,0)/100000 as decimal(18,3)),
	                        ProjectDPR_FilePath1,
	                        ProjectDPR_FilePath2,
	                        ProjectDPR_Verified_Comments,
	                        ProjectDPR_Upload_Comments,
	                        ProjectDPR_BudgetAllocationComments,
	                        ProjectDPR_VerifiedOn,
	                        ProjectDPR_BudgetAllocatedBy,
	                        ProjectDPR_VerifiedBy,
	                        ProjectDPR_UploadedBy,
	                        ProjectDPRAdditionalInfo_RecomendatorMobile,
	                        ProjectDPRAdditionalInfo_RecomendatorName,
	                        ProjectDPRAdditionalInfo_Designation, 
                            ProjectDPRAdditionalInfo_InstructionByCompetentAuthority,
							Designation_DesignationName,
                            ProjectDPRAdditionalInfo_CompetentAuthorityName=Person_Name,
							
	                        ULB_Name, 
	                        M_Jurisdiction.Jurisdiction_Name_Eng, 
							LokSabha.LokSabha_Name as LokSabha,
							VidhanSabha.VidhanSabha_Name as VidhanSabha,
                            ProjectWork_IsVerified,
							tbl_ProjectType.ProjectType_Name,
                            ProjectDPR_Upload_Comments,
							  ProjectDPR_FilePath1,
	                          ProjectDPR_File1,
							  ProjectDPR_File2,
							convert(char(10), ProjectDPR_UploadedOn, 103) ProjectDPR_UploadedOn,
                            ProjectDPR_Latitude,
							ProjectDPR_Longitude,
                            convert(char(10), ProjectDPR_ReceivedAtHQDate, 103) ProjectDPR_ReceivedAtHQDate,
                            convert(char(10), ProjectDPR_ApprovedOn, 103) ProjectDPR_ApprovedOn,
							ProjectDPR_RefrenceNo
                    from tbl_ProjectWork 
                    join tbl_ProjectDPR on ProjectDPR_Work_Id = ProjectWork_Id 
					join tbl_Project on Project_Id=ProjectWork_Project_Id
					left join tbl_LokSabha as LokSabha on LokSabha.LokSabha_Id=tbl_ProjectDPR.ProjectDPR_LokSabha_Jurisdiction_Id
					left join tbl_VidhanSabha as VidhanSabha on VidhanSabha.VidhanSabha_Id=tbl_ProjectDPR.ProjectDPR_VidhanSabha_Jurisdiction_Id
					left join tbl_ProjectType on tbl_ProjectType.ProjectType_Id=ProjectWork_ProjectType_Id
                    left join tbl_ProjectDPRAdditionalInfo on ProjectDPRAdditionalInfo_ProjectDPR_Id = ProjectDPR_Id and ProjectDPRAdditionalInfo_Status = 1
					left join tbl_Designation on Designation_Id=ProjectDPRAdditionalInfo_InstructionByCompetentAuthority
					left join tbl_PersonDetail on Person_Id=ProjectDPRAdditionalInfo_CompetentAuthorityName
                    left join M_Jurisdiction on M_Jurisdiction.M_Jurisdiction_Id = ProjectDPR_District_Jurisdiction_Id
                    left join tbl_ULB on ULB_Id = ProjectDPR_NP_JurisdictionId   
					          
                    where ProjectWork_Status = 1 and ProjectDPR_Status = 1 and ProjectDPR_Id='" + ProjectDPR_Id + "'";

        strQuery += Environment.NewLine;
        strQuery += @"select DPR_Status_DPR_StatusName,
                        convert(char(10), ProjectDPRStatus_Date, 103) ProjectDPRStatus_Date,
                        ProjectDPRStatus_Comments
                        from tbl_ProjectDPRStatus 
                        inner join tbl_DPR_Status on DPR_Status_Id=ProjectDPRStatus_DPR_StatusId
                        where ProjectDPRStatus_ProjectDPR_Id='" + ProjectDPR_Id + "'";
        strQuery += Environment.NewLine;
        strQuery += @" select DPRQuestionnaire_Name,
                        ProjectDPRQuestionire_Answer,
                        ProjectDPRQuestionire_Remark 
                        from tbl_ProjectDPRQuestionire
                        inner join tbl_DPRQuestionnaire on DPRQuestionnaire_Id=ProjectDPRQuestionire_Questionire_Id
                        where ProjectDPRQuestionire_DPR_Id='" + ProjectDPR_Id + "' and ProjectDPRQuestionire_Status=1 ";

        strQuery += Environment.NewLine;
        strQuery += @" select PhysicalProgressComponent_Component,
                        Unit_Name
                        from tbl_ProjectDPR_PhysicalProgress
                        inner join tbl_PhysicalProgressComponent on PhysicalProgressComponent_Id=ProjectDPR_PhysicalProgress_PhysicalProgressComponent_Id
                        left join tbl_Unit on Unit_Id=PhysicalProgressComponent_Unit_Id
                        where ProjectDPR_PhysicalProgress_ProjectDPR_Id='" + ProjectDPR_Id + "' and ProjectDPR_PhysicalProgress_Status=1 ";

        strQuery += Environment.NewLine;
        strQuery += @" select Deliverables_Deliverables,
                        Unit_Name
                        from tbl_ProjectDPR_Deliverables
                        inner join tbl_Deliverables on Deliverables_Id=ProjectDPR_Deliverables_Deliverables_Id
                        left join tbl_Unit on Unit_Id=Deliverables_Unit_Id
                        where ProjectDPR_Deliverables_ProjectDPR_Id='" + ProjectDPR_Id + "' and ProjectDPR_Deliverables_Status=1 ";

        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }

    public DataSet get_Invoice_TAT_Report(int Zone_Id, int Circle_Id, int Division_Id, int Scheme_Id, int Org_Id, int Designation_Id, bool Show_All_For_Report)
    {
        string strQuery = "";
        DataSet ds = new DataSet();
        strQuery = @"set dateformat dmy; 
                    with cte as (
                    select 
	                    *, 
	                    Date_Diff_Slab = case when Date_Diff_Action < 7 then 'Less Than 7 Days' when Date_Diff_Action >= 7 and Date_Diff_Action < 15 then 'More Than 7 and Less Than 15 Days' when Date_Diff_Action >= 15 then 'More Than 15 Days' else '' end, 
	                    Pending = case when (PackageInvoiceApproval_Next_Organisation_Id = 2 and PackageInvoiceApproval_Next_Designation_Id = 1) then 1 else 0 end,   
	                    Marked = case when (PackageInvoiceApproval_Status_Id = 4) then 1 else 0 end,   
	                    Approved = case when (PackageInvoiceApproval_Status_Id = 1) then 1 else 0 end   
                    from (
                    select 
	                    PackageInvoice_Id, 
                        PackageInvoice_Date = convert(char(10), PackageInvoice_Date, 103),
	                    tInvoiceApproval.PackageInvoiceApproval_Next_Designation_Id, 
	                    tInvoiceApproval.PackageInvoiceApproval_Next_Organisation_Id, 
	                    tInvoiceApproval.PackageInvoiceApproval_Date, 
	                    tInvoiceApproval.PackageInvoiceApproval_Status_Id,
                        Invoice_Status = isnull(InvoiceStatus_Name, 'Pending'), 
                        Date_Diff_Invoice = DATEDIFF(DD, PackageInvoice_Date, getdate()), 
                        Date_Diff_Action = DATEDIFF(DD, convert(date, PackageInvoiceApproval_Date, 103), getdate())
                    from tbl_PackageInvoice 
                    join tbl_ProjectWorkPkg on ProjectWorkPkg_Id = PackageInvoice_Package_Id
                    join tbl_ProjectWork on ProjectWork_Id = ProjectWorkPkg_Work_Id
                    join tbl_Project on ProjectWork_Project_Id = Project_Id
                    join tbl_Division on ProjectWork_DivisionId = Division_Id
                    join tbl_Circle on Circle_Id = Division_CircleId
                    join tbl_Zone on Zone_Id = Circle_ZoneId
                    join 
                    (
	                    select 
		                    ROW_NUMBER() over (partition by PackageInvoiceApproval_PackageInvoice_Id order by PackageInvoiceApproval_Id desc) rrrrr,
		                    PackageInvoiceApproval_Next_Designation_Id,
		                    PackageInvoiceApproval_Next_Organisation_Id,
		                    PackageInvoiceApproval_Comments,
		                    PackageInvoiceApproval_AddedBy,
		                    PackageInvoiceApproval_AddedOn,
		                    PackageInvoiceApproval_Status_Id,
		                    PackageInvoiceApproval_Package_Id,
		                    PackageInvoiceApproval_PackageInvoice_Id,
                            InvoiceStatus_Name,
		                    PackageInvoiceApproval_Date = convert(char(10), PackageInvoiceApproval_Date, 103),
		                    PackageInvoiceApproval_Id, 
		                    tbl_Designation.Designation_DesignationName, 
		                    tbl_OfficeBranch.OfficeBranch_Name, 
                            Designation_Current = Designation_Current.Designation_DesignationName, 
	                        Organisation_Current = Organisation_Current.OfficeBranch_Name 
	                    from tbl_PackageInvoiceApproval
	                    left join tbl_OfficeBranch on OfficeBranch_Id = PackageInvoiceApproval_Next_Organisation_Id
	                    left join tbl_Designation on Designation_Id = PackageInvoiceApproval_Next_Designation_Id
                        join tbl_PersonDetail on Person_Id = PackageInvoiceApproval_AddedBy 
                        join tbl_PersonJuridiction on PersonJuridiction_PersonId = PackageInvoiceApproval_AddedBy
                        left join tbl_Designation Designation_Current on Designation_Current.Designation_Id = PersonJuridiction_DesignationId
                        left join tbl_OfficeBranch Organisation_Current on Organisation_Current.OfficeBranch_Id = Person_BranchOffice_Id
                        left join tbl_InvoiceStatus on InvoiceStatus_Id = PackageInvoiceApproval_Status_Id
	                    where PackageInvoiceApproval_Status = 1 and PackageInvoiceApproval_Next_Organisation_Id = 2 and PackageInvoiceApproval_Next_Designation_Id in (1, 33)
                    ) tInvoiceApproval on tInvoiceApproval.PackageInvoiceApproval_PackageInvoice_Id = PackageInvoice_Id and tInvoiceApproval.rrrrr = 1
                    where PackageInvoice_Status = 1 SchemeCondition ZoneCondition CircleCondition DivisionCondition) tData)

                    select 
	                    Date_Diff_Slab, 
	                    Total_Invoice = count(*), 
	                    Total_Pending = sum(isnull(Pending, 0)),
	                    Total_Marked = sum(isnull(Marked, 0)),
	                    Total_Approved = sum(isnull(Approved, 0))
                    from cte
                    group by cte.Date_Diff_Slab";

        if (Scheme_Id > 0)
        {
            strQuery = strQuery.Replace("SchemeCondition", "and ProjectWork_Project_Id = '" + Scheme_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("SchemeCondition", "");
        }
        if (Division_Id > 0)
        {
            strQuery = strQuery.Replace("DivisionCondition", "and ProjectWork_DivisionId = '" + Division_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("DivisionCondition", "");
        }
        if (Circle_Id > 0)
        {
            strQuery = strQuery.Replace("CircleCondition", "and Circle_Id = '" + Circle_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("CircleCondition", "");
        }
        if (Zone_Id > 0)
        {
            strQuery = strQuery.Replace("ZoneCondition", "and Zone_Id = '" + Zone_Id + "'");
        }
        else
        {
            strQuery = strQuery.Replace("ZoneCondition", "");
        }
        try
        {
            ds = ExecuteSelectQuery(strQuery);
        }
        catch
        {
            ds = null;
        }
        return ds;
    }
}