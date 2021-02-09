using Dal.FeiYong_DZ;
using Dal.SysDictionary;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_fysq_YksqdDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    dz_yksqdDal yksqDal = new dz_yksqdDal();
    string strCtrl = "";
    string strbillcode = "";
    string strBh = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["Code"] != null && Request["Code"].ToString() != "")
            {
                strbillcode = Request["Code"].ToString();
            }
            else
            {
                Databind();
            }
            if (Request["Ctrl"] != null && Request["Ctrl"].ToString() != "")
            {
                strCtrl = Request["Ctrl"].ToString();

                if (!string.IsNullOrEmpty(strCtrl) && strCtrl == "Add")
                {
                    strBh = new DataDicDal().GetYbbxBillName("zcgz", DateTime.Now.ToString("yyyyMMdd"), 1);
                    txt_sqsj.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txt_sqr.Text = "[" + Session["userCode"].ToString() + "]" + Session["userName"].ToString();
                }
                else
                {
                    if (strCtrl == "View")
                    {
                        this.btn_save.Visible = false;
                    }
                    Databind();
                }
            }
        }
    }
    void Databind()
    {
        dz_yksqd yksqModel = new dz_yksqd();
        yksqModel = yksqDal.GetModel(strbillcode);
        if (yksqModel!=null)
        {
            //为控件绑定值
            txt_bmfzr_qz.Text = yksqModel.bmfzr_qz;
            txt_bmfzr_rq.Text = yksqModel.bmfzr_rq;
            txt_bmfzr_yj.Text = yksqModel.bmfzr_yj;
            txt_cwfzr_qz.Text = yksqModel.cwbfzr_qz;
            txt_cwfzr_rq.Text = yksqModel.cwbfzr_rq;
            drp_cwfzr_yj.SelectedValue = yksqModel.cwbfzr_yj;
            txt_cwxz_qz.Text = yksqModel.cwxz_qz;
            txt_cwxz_rq.Text = yksqModel.cwxz_rq;
            txt_cwxz_yj.Text = yksqModel.cwxz_yj;
            txt_dsz_qz.Text = yksqModel.dsz_qz;
            txt_dsz_rq.Text = yksqModel.dsz_rq;
            txt_dsz_yj.Text = yksqModel.dsz_yj;
            txt_khh.Text = yksqModel.khh;
            txt_kxje_dx.Text = yksqModel.kxje_dx;
            txt_kxje_xx.Text = yksqModel.kxje_xx.ToString();
            txt_kxyt.Text = yksqModel.kxyt;
            txt_skdw.Text = yksqModel.skdw;
            txt_sqr.Text = yksqModel.sqr;
            txt_sqsj.Text = yksqModel.sqsj;
            txt_yfkzy_qz.Text = yksqModel.yfzkzy_qz;
            txt_yfkzy_rq.Text = yksqModel.yfzkzy_rq;
            txt_yfkzy_yj.Text = yksqModel.yfzkzy_yj;
            txt_ykdept.Text = yksqModel.note0;
            txt_ykrq.Text = yksqModel.ykrq;
            txt_zh.Text = yksqModel.zh;
            ddl_ykfs.SelectedValue = yksqModel.ykfs;
            drp_sqlx.SelectedValue = yksqModel.sqlx;


        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {

        dz_yksqd yksqModel = new dz_yksqd();
        Bill_Main mainModel = new Bill_Main();
        if (!string.IsNullOrEmpty(strbillcode))
        {
            yksqModel.sqbh = strbillcode;
            mainModel.BillCode = strbillcode;
        }
        if (!string.IsNullOrEmpty(strBh))
        {
            mainModel.BillCode = strBh;
            yksqModel.sqbh = strBh;
        }
        if (!string.IsNullOrEmpty(txt_sqsj.Text))
        {
            mainModel.BillDate = DateTime.Parse(txt_sqsj.Text);
            yksqModel.sqsj = txt_sqsj.Text;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请时间不能为空')", true);
            return;
        }
        if (!string.IsNullOrEmpty(Session["userCode"].ToString().Trim()))
        {
            string dept = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            mainModel.BillDept = dept;
            mainModel.BillUser = Session["userCode"].ToString();
            yksqModel.sqr = Session["userCode"].ToString();
        }
        if (!string.IsNullOrEmpty(txt_kxje_xx.Text))
        {
            mainModel.BillJe = decimal.Parse(txt_kxje_xx.Text);
            yksqModel.kxje_xx = decimal.Parse(txt_kxje_xx.Text);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('款项金额(小写)不能为空。')", true);
            return;
        }

        mainModel.BillName = "大智用款申请单";
        mainModel.FlowId = "yksq_dz";
        mainModel.StepId = "-1";
        if (!string.IsNullOrEmpty(txt_kxje_dx.Text))
        {
            yksqModel.kxje_dx = txt_kxje_dx.Text;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('款项金额(大写)不能为空。')", true);
            return;
        }
      
        yksqModel.khh = txt_khh.Text;
        if (!string.IsNullOrEmpty(txt_kxyt.Text))
        {
            yksqModel.kxyt = txt_kxyt.Text;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('款项用途不能为空。')", true);
            return;
        }
      
        yksqModel.bmfzr_qz = txt_bmfzr_qz.Text;
        yksqModel.bmfzr_rq = txt_bmfzr_rq.Text;
        if (!string.IsNullOrEmpty(drp_sqlx.SelectedValue))
        {
            yksqModel.sqlx = drp_sqlx.SelectedValue;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请类型不能为空。')", true);
            return;
        }

        if (!string.IsNullOrEmpty(txt_sqr.Text))
        {
            yksqModel.sqr = txt_sqr.Text.Substring(1,txt_sqr.Text.IndexOf("]")-1);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请人不能为空。')", true);
            return;
        }
    
        yksqModel.bmfzr_yj = txt_bmfzr_yj.Text;
        yksqModel.cwbfzr_qz = txt_cwfzr_qz.Text;
        yksqModel.cwbfzr_rq = txt_cwfzr_rq.Text;
        yksqModel.cwbfzr_yj = drp_cwfzr_yj.SelectedValue;
        yksqModel.cwxz_qz = txt_cwxz_qz.Text;
        yksqModel.cwxz_rq = txt_cwxz_rq.Text;
        yksqModel.cwxz_yj = txt_cwxz_yj.Text;
        yksqModel.dsz_qz = txt_dsz_qz.Text;
        yksqModel.dsz_rq = txt_dsz_rq.Text;
        yksqModel.dsz_yj = txt_dsz_yj.Text;
        yksqModel.zh = txt_zh.Text;
        if (!string.IsNullOrEmpty(txt_ykrq.Text))
        {
            yksqModel.ykrq = txt_ykrq.Text;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('用款日期不能为空。')", true);
            return;
        }

        if (!string.IsNullOrEmpty(ddl_ykfs.SelectedValue))
        {
            yksqModel.ykfs = ddl_ykfs.SelectedValue;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('用款方式不能为空。')", true);
            return;
        }
      
       
        yksqModel.yfzkzy_yj = txt_yfkzy_yj.Text;
        yksqModel.yfzkzy_rq = txt_yfkzy_rq.Text;
        yksqModel.yfzkzy_qz = txt_yfkzy_qz.Text;
        if (!string.IsNullOrEmpty(txt_ykdept.Text))
        {
            yksqModel.note0 = txt_ykdept.Text;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('用款部门不能为空。')", true);
            return;
        }
    

        yksqModel.skdw = txt_skdw.Text;
     
        int intRow = yksqDal.Add(mainModel, yksqModel);
        if (intRow > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败')", true);
        }
    }
}