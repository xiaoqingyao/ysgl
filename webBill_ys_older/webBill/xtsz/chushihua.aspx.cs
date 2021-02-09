using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_xtsz_chushihua : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            list.Add("truncate table bill_cbzx");//成本中心
            list.Add("truncate table bill_cgsp");//采购审批
            list.Add("truncate table bill_cgsp_lscg");//采购审批 对应报告申请表
            list.Add("truncate table bill_cgsp_mxb");//采购审批明细表
            list.Add("truncate table bill_cgsp_xjb");//采购审批询价表
            list.Add("truncate table bill_cwkm");//财务科目
            list.Add("delete from bill_datadic where dictype<>'00'");//数据字典
            list.Add("truncate table bill_departments");//单位表
            list.Add("insert into bill_departments values('000001','预算控制单位','','1')");//单位表
            list.Add("truncate table bill_dept_ywzg");//单位业务主管表
            list.Add("truncate table bill_dept_fgld");//单位分管领导表
            list.Add("truncate table bill_fysq");//费用申请
            list.Add("truncate table bill_fysq_fjb");//费用申请附件表
            list.Add("truncate table bill_fysq_mxb");//费用申请明细表
            list.Add("truncate table bill_main");//主单
            list.Add("truncate table bill_msg");//桌面友情提示表
            list.Add("truncate table bill_searchRight");//费用申请查询权限表
            list.Add("truncate table bill_usergroup");//角色表
            list.Add("truncate table bill_userright");//人员（角色）操作（管理）权限表
            list.Add("delete from bill_users where userCode<>'admin'");//人员表
            list.Add("update bill_users set userstatus='1',userpwd='',issystem='1'");//初始化系统管理员
            list.Add("truncate table bill_workflowRecord");//审核记录表
            list.Add("truncate table bill_workFlowAction");//流程动作表
            list.Add("truncate table bill_workflowGroup");//流程权限设置表
            list.Add("truncate table bill_workflowGroup");//流程权限设置表
            list.Add("delete from bill_workFlowStep where stepID not in ('begin','end')");//流程步骤表
            list.Add("truncate table bill_ybbx_fysq");//一般报销 对应费用申请表
            list.Add("truncate table bill_ybbxmxb");//一般报销明细表
            list.Add("truncate table bill_ybbxmxb_bxdj");//一般报销明细表 报销单据
            list.Add("truncate table bill_ybbxmxb_fydk");//一般报销明细表 抵扣
            list.Add("truncate table bill_ybbxmxb_fykm");//一般报销明细表 科目
            list.Add("truncate table bill_ybbxmxb_fykm_ft");//一般报销明细表 科目 费用分摊
            list.Add("truncate table bill_ysgc");//预算过程
            list.Add("truncate table bill_yskm");//预算科目
            list.Add("truncate table bill_yskm_dept");//单位启用预算科目表
            list.Add("truncate table bill_yskm_dzb");//预算科目 与财务科目的对照表
            list.Add("truncate table bill_ysmxb");//预算明细表
            list.Add("truncate table bill_ysmxb_smfj");//预算明细说明及附件
            list.Add("truncate table bill_ystz");//预算调整表


            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('初始化失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('初始化已完成！');", true);
            }
        }
    }
}
