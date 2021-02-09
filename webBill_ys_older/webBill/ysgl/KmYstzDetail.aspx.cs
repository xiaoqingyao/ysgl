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
using Models;
using Bll.UserProperty;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Bll.Bills;

public partial class webBill_ysgl_KmYstzDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysMgr = new YsManager();
    SysManager sysMgr = new SysManager();
    string userCode = "";
    string strCtrl = "";
    string strBillCode = "";
    string strDeptCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            userCode = Session["userCode"].ToString().Trim();
        }
        object objCtrl = Request["Ctrl"];
        if (objCtrl != null)
        {
            strCtrl = objCtrl.ToString().Trim();
        }
        object objBillCode = Request["billCode"];
        if (objBillCode != null)
        {
            strBillCode = objBillCode.ToString().Trim();
            strDeptCode = server.GetCellValue("select billdept from bill_main where billcode='"+strBillCode+"'");
        }
        object objDeptCode = Request["deptcode"];
        if (objDeptCode != null)
        {
            strDeptCode = objDeptCode.ToString().Trim();
        }
        if (!IsPostBack)
        {
            Bind();
        }
    }
    /// <summary>
    /// 页面数据初始化
    /// </summary>
    private void Bind()
    {

        //如果参数不为空 部门默认为传的参数
        UserMessage userMgr = new UserMessage(userCode);
        Bill_Departments dept = userMgr.GetRootDept();
        #region 绑定人员管理下的部门
        string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
        DataTable dtuserRightDept;
        LaDept.Items.Clear();
        if (strDeptCodes != "")
        {
            if (strDeptCode != "")
            {
                LaDept.Items.Clear();
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in(" + strDeptCode + ")", null);

                string strcsdeptcode = server.GetCellValue("select deptCode from bill_departments where deptCode='" + strDeptCode + "'");
                string strcsdeptname = server.GetCellValue("select deptName from bill_departments where deptCode='" + strDeptCode + "'");
                this.LaDept.Text = strcsdeptcode;
                this.LaDept.Items.Insert(0, new ListItem("[" + strcsdeptcode + "]" + strcsdeptname, strcsdeptcode));
                this.LaDept.SelectedIndex = 0;
                for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                    li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                    this.LaDept.Items.Add(li);
                }
            }
            else
            {
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in(" + dept.DeptCode + ")", null);

                for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                    li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                    this.LaDept.Items.Add(li);
                }
            }
        }
        else
        {
            this.LaDept.Items.Insert(0, new ListItem("[" + dept.DeptCode + "]" + dept.DeptName, dept.DeptCode));
            this.LaDept.SelectedIndex = 0;
        }
        #endregion
        //根据部门绑定预算科目
        if (LaDept.SelectedIndex != -1)
        {
            BindKmByDept(LaDept.SelectedValue);
        }
        if (strCtrl.Equals("Add"))
        {
            //制单人
            txt_zdr.Text = "[" + userMgr.Users.UserCode + "]" + userMgr.Users.UserName;
            string strGcbh = ysMgr.GetYsgcCode(DateTime.Now);
            strGcbh = ysMgr.GetYsgcCodeName(strGcbh);
            if (strGcbh.Equals(""))//没有预算过程
	        {
                ClientScript.RegisterStartupScript(this.GetType(),"","alert('没有找到当前的预算过程，请先到预算过程管理模块生成！');window.close();",true);
                return;
	        }
            txt_source.Text = strGcbh;
            BindGridByDeptkm();
        }
        else if (strCtrl.Equals("View") || strCtrl.Equals("Edit"))
        {
            if (strBillCode.Equals(""))
            {
                return;
            }
            if (strCtrl.Equals("View"))
            {
                this.btn_save.Visible = false;
            }
            //表头信息
            Bill_Main modelBillMain = new Bill_Main();
            BillMainBLL bllBillMain = new BillMainBLL();
            modelBillMain = bllBillMain.GetModel(strBillCode);
            txt_source.Text = ysMgr.GetYsgcCodeName(modelBillMain.BillName);
            UserMessage userMgr2 = new UserMessage(userCode);
            this.txt_zdr.Text = "["+userMgr2.Users.UserCode+"]"+userMgr2.Users.UserName;
            this.txt_source.Text = ysMgr.GetYsgcCodeName(modelBillMain.BillName);
            this.LaDept.SelectedValue = modelBillMain.BillDept;
            this.drp_yskm.SelectedValue = server.GetCellValue("select distinct yskm from bill_ysmxb where   ysje>0 and billCode='" + strBillCode + "' ");// modelBillMain.BillName2;
            txt_zy.Text = modelBillMain.BillName2;
            //gridview
            string strSql = "select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=a.yskm) as yskmShowName,(select '['+gcbh+']'+xmmc from bill_ysgc where gcbh=a.gcbh) as gcbhShowName,gcbh,billCode,yskm,ysje,ysDept,ysType from bill_ysmxb a where billCode=@billCode and ysType='4' and yskm!=@yskm";
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@billCode", strBillCode), new SqlParameter("@yskm", server.GetCellValue("select top 1  yskm from bill_ysmxb  where billCode ='"+modelBillMain.BillCode+"'")) };
            MyGridView.DataSource = server.GetDataTable(strSql, arrSp);
            MyGridView.DataBind();
        }
    }
    /// <summary>
    /// 选择部门变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (LaDept.SelectedIndex != -1)
        {
            BindKmByDept(LaDept.SelectedValue);
        }
    }
    /// <summary>
    /// 选择科目变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drp_yskm_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridByDeptkm();
    }

    /// <summary>
    /// 数据表格行加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e == null) { return; }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strGcbh = e.Row.Cells[0].Text;
            string strKmbh = e.Row.Cells[1].Text;
            string strDeptCode = this.LaDept.SelectedValue.Trim();
            decimal ysje = ysMgr.GetYueYs(strGcbh, strDeptCode, strKmbh);//预算金额
            decimal hfje = ysMgr.GetYueHf(strGcbh, strDeptCode, strKmbh);//花费金额
            decimal zyje = ysMgr.GetYueNotEndje(strGcbh, strDeptCode, strKmbh);//占用金额
            e.Row.Cells[4].Text = ysje.ToString("N02");
            e.Row.Cells[5].Text = hfje.ToString("N02");
            e.Row.Cells[6].Text = (-zyje).ToString("N02");
            e.Row.Cells[7].Text = (ysje - hfje + zyje).ToString();
            if (strCtrl.Equals("View") || strCtrl.Equals("Edit"))
            {
                string strTzje = e.Row.Cells[9].Text;
                if (!string.IsNullOrEmpty(strTzje))
                {
                    (e.Row.Cells[8].FindControl("txtTzAmount") as TextBox).Text = (-decimal.Parse(strTzje)).ToString();
                }
            }
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (strCtrl.Equals("Edit") && strBillCode.Equals("")){return;}
        int iGridViewRowCount = this.MyGridView.Rows.Count;       
        if (iGridViewRowCount == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('调整明细项目为空，保存失败！');window.close();", true);
        }
        else
        {
            string strToKmCode = this.drp_yskm.SelectedValue.Trim();
            string strGcbh = this.MyGridView.Rows[0].Cells[0].Text.Trim();
            string strDeptCode = this.LaDept.SelectedValue.Trim();
            List<Bill_Ysmxb> lstYsMxb = new List<Bill_Ysmxb>();
            //主表信息
            Bill_Main modelBillMain = new Bill_Main();
            if (strCtrl.Equals("Edit"))
            {
                modelBillMain = new BillMainBLL().GetModel(strBillCode);
            }
            else if (strCtrl.Equals("Add"))
            {
                modelBillMain.BillCode = new GuidHelper().getNewGuid();
                // modelBillMain.BillName2 = strToKmCode;//用name2存目标预算科目
                modelBillMain.BillDept = strDeptCode;
            }
            modelBillMain.BillName = strGcbh;
            modelBillMain.BillDate = DateTime.Now;
            modelBillMain.BillUser = userCode;
            modelBillMain.FlowId = "kmystz";
            modelBillMain.BillName2 = txt_zy.Text.Trim();
            bool boYstzNeedAudit = new Bll.ConfigBLL().GetValueByKey("KmystzNeedAudit").Equals("0") ? false : true;
            if (boYstzNeedAudit)
            {
                modelBillMain.StepId = "-1";
            }
            else
            {
                modelBillMain.StepId = "end";
            }


            modelBillMain.BillJe = 0;

            for (int i = 0; i < iGridViewRowCount; i++)
            {
                string strFrmKmCode = this.MyGridView.Rows[i].Cells[1].Text;
                TextBox txtTzJe = this.MyGridView.Rows[i].Cells[8].FindControl("txtTzAmount") as TextBox;
                string strTzJe = txtTzJe.Text;
                decimal deTzJe = 0;
                if (!decimal.TryParse(strTzJe, out deTzJe) || deTzJe == 0)
                {
                    continue;
                }
                Bill_Ysmxb modelYsmxb = new Bill_Ysmxb();
                modelYsmxb.BillCode = modelBillMain.BillCode;
                modelYsmxb.Gcbh = strGcbh;
                modelYsmxb.YsDept = strDeptCode;
                modelYsmxb.Ysje = deTzJe;
                modelYsmxb.Yskm = strToKmCode;
                modelYsmxb.YsType = "4";
                lstYsMxb.Add(modelYsmxb);
                Bill_Ysmxb modelYsmxb2 = new Bill_Ysmxb();
                modelYsmxb2.BillCode = modelBillMain.BillCode;
                modelYsmxb2.Gcbh = strGcbh;
                modelYsmxb2.YsDept = strDeptCode;
                modelYsmxb2.Ysje = -deTzJe;
                modelYsmxb2.Yskm = strFrmKmCode;
                modelYsmxb2.YsType = "4";
                lstYsMxb.Add(modelYsmxb2);
                modelBillMain.BillJe += deTzJe;
            }

            if (lstYsMxb.Count > 0)
            {
                ysMgr.InsertYsmx(lstYsMxb, modelBillMain);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.close();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请填写调整金额！');", true);
            }
        }
    }

    /// <summary>
    /// 根据部门绑定科目
    /// </summary>
    /// <param name="strDeptCode"></param>
    private void BindKmByDept(string strDeptCode)
    {
        if (strDeptCode != "")
        {
            drp_yskm.Items.Clear();
            DataTable dtkm = server.GetDataTable("select yskmcode,('['+yskmcode+']'+yskmMc) as yskmname from Bill_Yskm a where a.yskmcode in (select yskmcode from bill_yskm_dept where deptcode='" + strDeptCode + "') and	isNUll(allowTz,'1')!='0'", null);
            if (dtkm.Rows.Count > 0)
            {
                for (int i = 0; i < dtkm.Rows.Count; i++)
                {
                    ListItem lidept = new ListItem();
                    lidept.Text = dtkm.Rows[i]["yskmname"].ToString().Trim();
                    lidept.Value = dtkm.Rows[i]["yskmcode"].ToString().Trim();
                    this.drp_yskm.Items.Add(lidept);
                }
                this.drp_yskm.SelectedIndex = 1;
            }
            else
            {
                this.drp_yskm.Items.Insert(0, new ListItem("--该部门没有可选科目--", ""));
                this.drp_yskm.SelectedIndex = 0;
            }
            BindGridByDeptkm();
        }
    }

    /// <summary>
    /// 表头信息绑定gridview
    /// </summary>
    private void BindGridByDeptkm()
    {
        //预算过程编号
        string strGcbh = this.txt_source.Text;
        if (string.IsNullOrEmpty(strGcbh))
        {
            return;
        }
        strGcbh = strGcbh.Substring(1, strGcbh.IndexOf(']') - 1);
        if (LaDept.SelectedValue == null)
        {
            return;
        }
        //单位编号
        string strDeptCode = this.LaDept.SelectedValue.Trim();
        if (drp_yskm.SelectedValue == null)
        {
            return;
        }
        //科目
        string strYskm = this.drp_yskm.SelectedValue.Trim();

        string strSql = "select distinct yskm,gcbh,ysDept,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=a.yskm) as yskmShowName,(select '['+gcbh+']'+xmmc from bill_ysgc where gcbh=a.gcbh) as gcbhShowName,0 as ysje from bill_ysmxb a where  gcbh=@gcbh and ysDept=@ysDept and yskm!=@yskm";
        SqlParameter[] arrSp = new SqlParameter[] { 
            new SqlParameter("@gcbh",strGcbh),
            new SqlParameter("@ysDept",strDeptCode),
            new SqlParameter("@yskm",strYskm)
        };
        MyGridView.DataSource = server.GetDataTable(strSql, arrSp);
        MyGridView.DataBind();
    }
    /// <summary>
    /// 查询是不是二级单位
    /// </summary>
    /// <param name="strus">是人员CODE？y:n</param>
    /// <param name="usercode">人员CODE</param>
    /// <returns></returns>
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
