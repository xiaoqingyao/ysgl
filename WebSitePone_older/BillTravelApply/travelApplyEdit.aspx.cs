using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Dal.SysDictionary;
using System.Text;
using Models;
using Bll.UserProperty;
using Bll.FeeApplication;

public partial class BillTravelApply_travelApplyEdit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["type"]))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Index.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindDDL();
            if (Request["type"] == "add")
            {
                CreateCode();
                txt_billDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                txt_billUser.Text = server.GetCellValue("select  '['+usercode+']'+userName from bill_users  where usercode='" + Session["userCode"].ToString() + "'");
                txt_bm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString() + "')");

            }
            else if (Request["type"] == "edit" && !string.IsNullOrEmpty(Request["billCode"]))
            {
                BindData();
            }
        }
    }

    private void BindDDL()
    {
        //DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='03' order by dicCode");
        //ddlType.DataTextField = "dicName";
        //ddlType.DataValueField = "dicCode";
        //ddlType.DataSource = temp;
        //ddlType.DataBind();
    }
    /// <summary>
    /// 生成编号
    /// </summary>
    private void CreateCode()
    {
        string lscgCode = new DataDicDal().GetYbbxBillName("ccsq", DateTime.Now.ToString("yyyyMMdd"), 1);
        if (lscgCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
        }
        else
        {
            this.txt_bh.Text = lscgCode;
        }
    }
    private void BindData()
    {
        string code = Convert.ToString(Request["billCode"]);
        DataTable dt = server.GetDataTable("select distinct  maincode,typecode,arrdess,travelDate,reasion,travelplan,needAmount,transport,moreThanStandard,reportCode,jiaotongfei,zhusufei,yewuzhaodaifei,huiyifei,yinshuafei,qitafei,sendDept,b.billCode as billCode,b.billName as billName, convert(varchar(10),b.billdate,121) as billDate,b.billDept as billDept ,b.billje as billje ,b.billuser from bill_travelApplication a, bill_main b where a.mainCode=b.billCode and a.mainCode='" + code + "'", null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txt_bh.Text = ObjectToStr(dr["billCode"]);
            txt_bm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + ObjectToStr(dr["sendDept"]) + "'");
            txt_billDate.Text = Convert.ToDateTime(ObjectToStr(dr["billDate"])).ToString("yyyy-MM-dd");
            txt_userDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + ObjectToStr(dr["billDept"]) + "'");
            txt_billUser.Text = server.GetCellValue("select '['+usercode+']'+userName from bill_users where usercode='" + ObjectToStr(dr["billuser"]) + "'");
            txt_travelDate.Text = ObjectToStr(dr["travelDate"]);
            string persons = "";
            DataTable pdt = server.GetDataTable("select travelPersionCode  as p from bill_travelApplication where mainCode='" + code + "' ", null);
            if (pdt.Rows.Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pdt.Rows.Count; i++)
                {
                    DataTable itemdt = server.GetDataTable("select '['+usercode+']'+userName as userCode,(select '['+deptCode+']'+deptName from bill_departments where deptCode=userDept) as deptCode  from bill_users where usercode='" + pdt.Rows[i]["p"].ToString() + "'", null);
                    sb.Append("<tr><td  class='mytdEnenRight tdborder'>" + itemdt.Rows[0]["userCode"] + "</td><td class='mytdEnenRight tdborder'>" + itemdt.Rows[0]["deptCode"] + "</td><td  class='tdborder'><input type='button' data-iconpos='notext' data-icon='delete' onclick='RemoveRow(this);'/></td></tr>");
                }
                persons = sb.ToString();
            }
            hfpersons.Value = persons;
            txt_address.Text = ObjectToStr(dr["arrdess"]);
            txt_reasion.Text = ObjectToStr(dr["reasion"]);
            txt_plan.Text = ObjectToStr(dr["travelplan"]);
            txt_zje.Text = NullToNUm(dr["billje"]);
            txt_jtf.Text = NullToNUm(dr["jiaotongfei"]);
            txt_zsf.Text = NullToNUm(dr["zhusufei"]);
            txt_zdf.Text = NullToNUm(dr["yewuzhaodaifei"]);
            txt_ysf.Text = NullToNUm(dr["yinshuafei"]);
            txt_qt.Text = NullToNUm(dr["qitafei"]);
            txt_jtgj.Text = ObjectToStr(dr["transport"]);
            txt_hyf.Text = NullToNUm(dr["huiyifei"]);
            ddlIsbz.SelectedValue = ObjectToStr(dr["moreThanStandard"]);

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        bill_travelApplicationBLL bllTravelApplication = new bill_travelApplicationBLL();



        string strCtrl = Request["type"];
        string strBillCode = Request["billCode"];
        string str_billuser = PubMethod.SubString(txt_billUser.Text);
        string users = hfuser.Value;
        if (string.IsNullOrEmpty(users))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('出差人不能为空！');", true);
            return;
        }
        string[] userArr = users.Split(',');
        string travelPersons = "";
        for (int i = 0; i < userArr.Length; i++)
        {
            if (travelPersons.IndexOf(userArr[i]) != -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('有重复的出差人！');", true);
                return;
            }
            travelPersons += "|&|" + PubMethod.SubString(userArr[i]);
        }


        string str_billdate = txt_billDate.Text.Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string bm = PubMethod.SubString(txt_userDept.Text);
        //申请表
        Bill_TravelApplication modelTravelApplication = new Bill_TravelApplication();
        //主表
        Bill_Main modelMainBill = new Bill_Main();
        string strMsg = "";
        try
        {
            //添加修改
            if (strCtrl.Equals("edit") && strBillCode.Equals(""))
            {
                if (strBillCode.Equals(""))
                {
                    throw new Exception("单号丢失！");
                }
                modelTravelApplication = bllTravelApplication.GetModel(strBillCode);
            }

            //添加
            modelMainBill.BillName = "出差管理单";
            modelMainBill.BillType = "";
            modelMainBill.BillUser = str_billuser;
            modelMainBill.FlowId = "ccsq";
            modelMainBill.GkDept = "";
            modelMainBill.IsGk = "";
            modelMainBill.LoopTimes = 0;
            modelMainBill.StepId = "-1";

            //出差申请单表
            modelTravelApplication.arrdess = this.txt_address.Text.Trim();
            modelTravelApplication.maincode = txt_bh.Text.Trim();
            modelTravelApplication.MoreThanStandard = Convert.ToInt32(ddlIsbz.SelectedValue);
            //modelTravelApplication.needAmount = int.Parse(this.txtFeePlan.Text.Trim());
            modelTravelApplication.reasion = this.txt_reasion.Text.Trim();
            modelTravelApplication.Transport = this.txt_jtgj.Text.Trim();
            modelTravelApplication.travelDate = this.txt_travelDate.Text.Trim();

            modelTravelApplication.sendDept = PubMethod.SubString(txt_bm.Text);
            string strAppPersion = this.txt_billUser.Text.Trim();
            if (string.IsNullOrEmpty(strAppPersion))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请人不能为空！');", true);
                return;
            }
            modelTravelApplication.travelPersionCode = travelPersons;
            modelTravelApplication.travelplan = this.txt_plan.Text.Trim(); ;

            modelTravelApplication.jiaotongfei = ToIntNum(txt_jtf.Text);
            modelTravelApplication.zhusufei = ToIntNum(txt_zsf.Text);
            modelTravelApplication.yewuzhaodaifei = ToIntNum(txt_zdf.Text);
            modelTravelApplication.huiyifei = ToIntNum(txt_hyf.Text);
            modelTravelApplication.yinshuafei = ToIntNum(txt_ysf.Text);
            modelTravelApplication.qitafei = ToIntNum(txt_qt.Text);
            modelTravelApplication.needAmount = modelTravelApplication.jiaotongfei + modelTravelApplication.zhusufei + modelTravelApplication.yewuzhaodaifei + modelTravelApplication.huiyifei + modelTravelApplication.yinshuafei + modelTravelApplication.qitafei;
            modelMainBill.BillJe = (decimal)modelTravelApplication.needAmount;
            //if (this.ddlTravelType.SelectedValue == null)
            //{
            //    throw new Exception("出差类型不能为空！");
            //}
            //modelTravelApplication.typecode = this.ddlTravelType.SelectedValue.Trim();

            modelMainBill.BillCode = txt_bh.Text.Trim();

            DateTime dtBillDate;
            bool boBillDate = DateTime.TryParse(str_billdate, out dtBillDate);
            if (boBillDate)
            {
                modelMainBill.BillDate = dtBillDate;
            }
            else
            {
                throw new Exception("日期格式输入不正确！");
            }
            UserMessage user = new UserMessage(modelMainBill.BillUser);
            string strBillDept = user.GetRootDept().DeptCode;
            if (string.IsNullOrEmpty(strBillDept))
            {
                throw new Exception("未发现人员所在单位！");
            }
            modelMainBill.BillDept = strBillDept;
            //modelMainBill.BillJe = int.Parse(this.txtFeePlan.Text.Trim());//单据金额

            int iRel = bllTravelApplication.AddNote(modelMainBill, modelTravelApplication, out strMsg);
            if (iRel < 1)
            {
                throw new Exception(strMsg);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='travelApplyList.aspx';", true);
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,原因：" + ex.Message + "');", true);
        }
    }

    private int ToIntNum(object obj)
    {
        int de;
        bool flag = Int32.TryParse(Convert.ToString(obj), out de);
        if (flag)
        {
            return de;
        }
        else
        {
            return 0;
        }
    }

    private string NullToNUm(object obj)
    {
        string num = ObjectToStr(obj);
        if (string.IsNullOrEmpty(num))
        {
            return "0.00";
        }
        else
            return Convert.ToDecimal(num).ToString("N02");
    }
    private string ObjectToStr(object obj)
    {
        if (obj == null || Convert.ToString(obj) == string.Empty)
        {
            return "";
        }
        else
        {
            return obj.ToString();
        }
    }

}
