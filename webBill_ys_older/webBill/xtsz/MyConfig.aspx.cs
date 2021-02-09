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

public partial class webBill_xtsz_MyConfig : System.Web.UI.Page
{
    Bll.ConfigBLL bllConfig = new Bll.ConfigBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            initControl();
        }
    }

    private void initControl()
    {
        //是否启用预算管理模块 rdb_BudgetControl_yes
        rdb_BudgetControl_yes.Checked = bllConfig.GetModuleDisabled("HasBudgetControl");
        rdb_BudgetControl_no.Checked = !bllConfig.GetModuleDisabled("HasBudgetControl");
        //是否启用报告申请单模块 rdb_BGSQ_yes  HasBGSQ
        rdb_BGSQ_yes.Checked = bllConfig.GetModuleDisabled("HasBGSQ");
        rdb_BGSQ_no.Checked = !bllConfig.GetModuleDisabled("HasBGSQ");
        //是否启用采购审批单模块 rdb_CGSP_yes  HasCGSP
        rdb_CGSP_yes.Checked = bllConfig.GetModuleDisabled("HasCGSP");
        rdb_CGSP_no.Checked = !bllConfig.GetModuleDisabled("HasCGSP");
        //是否启用项目支付申请单模块  rdb_XMZF_yes HasXMZF
        rdb_XMZF_yes.Checked = bllConfig.GetModuleDisabled("HasXMZF");
        rdb_XMZF_no.Checked = !bllConfig.GetModuleDisabled("HasXMZF");
        //是否启用出差申请单模块  rdb_CCSQ_yes HasCCSQ
        rdb_CCSQ_yes.Checked = bllConfig.GetModuleDisabled("HasCCSQ");
        rdb_CCSQ_no.Checked = !bllConfig.GetModuleDisabled("HasCCSQ");

        //是否启用归口继续分解 UseGKFJ
        rdbIsGoOnGkYes.Checked = bllConfig.GetModuleDisabled("UseGKFJ");
        rdbIsGoOnGkNo.Checked = !bllConfig.GetModuleDisabled("UseGKFJ");
        //一般报销单税额是否进总金额  TaxSwitch
        rdbIsInAllYes.Checked = bllConfig.GetModuleDisabled("TaxSwitch");
        rdbIsInAllNo.Checked = !bllConfig.GetModuleDisabled("TaxSwitch");
        //凭证是记到使用部门上 还是归口部门上  pingzhengbygkorsy
        rdbBM.Checked = bllConfig.GetModuleDisabled("pingzhengbygkorsy");
        rdbGK.Checked = !bllConfig.GetModuleDisabled("pingzhengbygkorsy");
        //是否控制点数 1为控制 0或不设置都是不控制  ISControlPoint
        rdbIsCPYes.Checked = bllConfig.GetModuleDisabled("ISControlPoint");
        rdbIsCPNo.Checked = !bllConfig.GetModuleDisabled("ISControlPoint");
        //是否检测到狗  HasDog  
        rdbCheckDogYes.Checked = bllConfig.GetModuleDisabled("HasDog");
        rdbCheckDogNo.Checked = !bllConfig.GetModuleDisabled("HasDog");

        //是否启用费用提成该模块  HasSaleRebate
        rdbIsSaleRebateYes.Checked = bllConfig.GetModuleDisabled("HasSaleRebate");
        rdbIsSaleRebateNo.Checked = !bllConfig.GetModuleDisabled("HasSaleRebate");

        //是否启用费用提成该模块  HasSaleRebate
        rdbIsSaleRebateYes.Checked = bllConfig.GetModuleDisabled("HasSaleRebate");
        rdbIsSaleRebateNo.Checked = !bllConfig.GetModuleDisabled("HasSaleRebate");

        //在桌面上回车 打开的报销单的地址   EnterForBxURL
        DataTable dt = bllConfig.GetDtByKey("EnterForBxURL");
        if (dt.Rows.Count>0)
        {
             txtEnterForBxURL.Text = Convert.ToString(dt.Rows[0]["avalue"]);
        }
       
        //nc系统的平台路径   ToNcURL
        dt = bllConfig.GetDtByKey("ToNcURL");
        if (dt.Rows.Count > 0)
        {
            txtToNcURL.Text = Convert.ToString(dt.Rows[0]["avalue"]);
        }
        //凭证制作页面的地址   pingzhengdetailurl
        dt = bllConfig.GetDtByKey("pingzhengdetailurl");
        if (dt.Rows.Count > 0)
        {
             txtpingzhengdetailurl.Text = Convert.ToString(dt.Rows[0]["avalue"]);
         }
        //是否开启滚动费用控制   IsRollCtrl
        rdbRollYes.Checked = bllConfig.GetModuleDisabled("IsRollCtrl");
        rdbRollNo.Checked = !bllConfig.GetModuleDisabled("IsRollCtrl");

        //404提示信息    ErrorMsg
         dt = bllConfig.GetDtByKey("ErrorMsg");
        if (dt.Rows.Count > 0)
        {
            txt_ErrorMsg.Text = Convert.ToString(dt.Rows[0]["avalue"]);
        }

         //企业简称   
        dt = bllConfig.GetDtByKey("CompanyName");
        if (dt.Rows.Count > 0)
        {
            txt_CompanyName.Text = Convert.ToString(dt.Rows[0]["avalue"]);
        }

        //企业Logo图片配置
        dt = bllConfig.GetDtByKey("CompanyLogo");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            lbfj0.Text = string.Format("<a href=\"../../" + Convert.ToString(dt.Rows[0]["avalue"]) + " \" target='_blank'>点击查看附件</a>"); ;
        }



        //登录界面图片1
        dt = bllConfig.GetDtByKey("LoginImg1");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            lbfj1.Text = string.Format("<a href=\"../../" + Convert.ToString(dt.Rows[0]["avalue"]) + " \" target='_blank'>点击查看附件</a>"); ;
        }
        //登录界面图片2
        dt = bllConfig.GetDtByKey("LoginImg2");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            lbfj2.Text = string.Format("<a href=\"../../" + Convert.ToString(dt.Rows[0]["avalue"]) + " \" target='_blank'>点击查看附件</a>"); ;
        }

        //登录界面图片3
        dt = bllConfig.GetDtByKey("LoginImg3");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            lbfj3.Text = string.Format("<a href=\"../../" + Convert.ToString(dt.Rows[0]["avalue"]) + " \" target='_blank'>点击查看附件</a>"); ;
        }

        //预算调整时目标预算过程必须是已开启预算  AllowTzUodoYs
        rdbAllowTzUodoYes.Checked = bllConfig.GetModuleDisabled("AllowTzUodoYs");
        rdbAllowTzUodoNo.Checked = !bllConfig.GetModuleDisabled("AllowTzUodoYs");


        //核算部门是否必须是末级部门  HsDeptIsLast
        rdbHsDeptIsLastYes.Checked = bllConfig.GetModuleDisabled("HsDeptIsLast");
        rdbHsDeptIsLastNo.Checked = !bllConfig.GetModuleDisabled("HsDeptIsLast");

        //一般报销核算项目模式级别  YbbxHsxmMode
        rdbHsxmHighModel.Checked = bllConfig.GetModuleDisabled("YbbxHsxmMode");
        rdbHsxmLowModel.Checked = !bllConfig.GetModuleDisabled("YbbxHsxmMode");
      
    }
    /// <summary>
    /// 保存修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e) {
        
        string strVal="1";
        int iRel=0;
        string strOutMsg = "";
        //是否启用预算管理模块 rdb_BudgetControl_yes  HasBudgetControl 
        strVal = this.rdb_BudgetControl_yes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasBudgetControl", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用预算管理模块失败，原因" + strOutMsg);
        }
        //是否启用报告申请单模块 rdb_BGSQ_yes  rdb_BGSQ_yes  HasBGSQ
        strVal = this.rdb_BGSQ_yes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasBGSQ",strVal,out strOutMsg);
        if (iRel<1)
        {
            Response.Write("设置是否启用报告申请单模块失败，原因" + strOutMsg);
        }
        //是否启用采购审批单模块 rdb_CGSP_yes  HasCGSP
        strVal = this.rdb_CGSP_yes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasCGSP", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用采购审批单模块失败，原因" + strOutMsg);
        }
        //是否启用项目支付申请单模块  rdb_XMZF_yes HasXMZF
        strVal = this.rdb_XMZF_yes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasXMZF", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用项目支付申请单模块失败，原因" + strOutMsg);
        }
        //是否启用出差申请单模块  rdb_CCSQ_yes HasCCSQ
        strVal = this.rdb_CCSQ_yes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasCCSQ", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用出差申请单模块失败，原因" + strOutMsg);
        }
        



        //是否启用归口继续分解 UseGKFJ
        strVal = this.rdbIsGoOnGkYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("UseGKFJ", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用归口继续分解失败，原因" + strOutMsg);
        }
      
        //一般报销单税额是否进总金额  TaxSwitch
        strVal = this.rdbIsInAllYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("TaxSwitch", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置一般报销单税额是否进总金额失败，原因" + strOutMsg);
        }
       
        //凭证是记到使用部门上 还是归口部门上  pingzhengbygkorsy
        strVal = this.rdbBM.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("pingzhengbygkorsy", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置凭证是记到使用部门上 还是归口部门上失败，原因" + strOutMsg);
        }
      
        //是否控制点数 1为控制 0或不设置都是不控制  ISControlPoint
        strVal = this.rdbIsCPYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("ISControlPoint", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否控制点数失败，原因" + strOutMsg);
        }
      
        //是否检测到狗  HasDog  
        strVal = this.rdbCheckDogYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasDog", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否检测Dog失败，原因" + strOutMsg);
        }
       

        //是否启用费用提成该模块  HasSaleRebate
        strVal = this.rdbIsSaleRebateYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HasSaleRebate", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置是否启用费用提成该模块失败，原因" + strOutMsg);
        }
       


        //在桌面上回车 打开的报销单的地址   EnterForBxURL
        strVal = this.txtEnterForBxURL.Text.Trim();
        iRel = bllConfig.SetModuleDisable("EnterForBxURL", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置在桌面上回车 打开的报销单的地址失败，原因" + strOutMsg);
        }
       
        //nc系统的平台路径   ToNcURL
        strVal = this.txtToNcURL.Text.Trim();
        iRel = bllConfig.SetModuleDisable("ToNcURL", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置nc系统的平台路径失败，原因" + strOutMsg);
        }
      
        //凭证制作页面的地址   pingzhengdetailurl
        strVal = this.txtpingzhengdetailurl.Text.Trim();
        iRel = bllConfig.SetModuleDisable("pingzhengdetailurl", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置凭证制作页面的地址失败，原因" + strOutMsg);
        }
       
        //凭证数据库连接名称   pingzhengdblinkname
        strVal = this.txtpingzhengdblinkname.Text.Trim();
        iRel = bllConfig.SetModuleDisable("pingzhengdblinkname", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置凭证数据库连接名称失败，原因" + strOutMsg);
        }

        //是否开启滚动费用控制   IsRollCtrl
        strVal = this.rdbRollYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("IsRollCtrl", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置凭404提示信息失败，原因" + strOutMsg);
        }

        //404提示信息   IsRollCtrl
        strVal = this.txt_ErrorMsg.Text.Trim();
        iRel = bllConfig.SetModuleDisable("ErrorMsg", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置404提示信息失败，原因" + strOutMsg);
        }


        //企业简称 CompanyName
        strVal = this.txt_CompanyName.Text.Trim();
        iRel = bllConfig.SetModuleDisable("CompanyName", strVal, out strOutMsg);
        if (iRel < 1)
        {
            Response.Write("设置企业简称失败，原因" + strOutMsg);
        }

        //企业Logo图片配置   
        if (!string.IsNullOrEmpty(f0.Value))
        {
            strVal = this.f0.Value.Trim();
            iRel = bllConfig.SetModuleDisable("CompanyLogo", strVal, out strOutMsg);
            if (iRel < 1)
            {
                Response.Write("企业Logo图片配置失败，原因" + strOutMsg);
            }
        }

      


        //登录界面图片1

        if (!string.IsNullOrEmpty(f1.Value))
        {
            strVal = this.f1.Value.Trim();
            iRel = bllConfig.SetModuleDisable("LoginImg1", strVal, out strOutMsg);
            if (iRel < 1)
            {
                Response.Write("登录界面图片1配置失败，原因" + strOutMsg);
            }
        }    
        //登录界面图片2
        if (!string.IsNullOrEmpty(f2.Value))
        {
            strVal = this.f2.Value.Trim();
            iRel = bllConfig.SetModuleDisable("LoginImg2", strVal, out strOutMsg);
            if (iRel < 1)
            {
                Response.Write("登录界面图片2配置失败，原因" + strOutMsg);
            }
        }

        //登录界面图片3
        if (!string.IsNullOrEmpty(f3.Value))
        {
            strVal = this.f3.Value.Trim();
            iRel = bllConfig.SetModuleDisable("LoginImg3", strVal, out strOutMsg);
            if (iRel < 1)
            {
                Response.Write("登录界面图片3配置失败，原因" + strOutMsg);
            }
        }

        //预算调整时目标预算过程必须是已开启预算  AllowTzUodoYs 1是 否
        strVal = this.rdbAllowTzUodoYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("AllowTzUodoYs", strVal, out strOutMsg);


        //核算部门是否必须是末级部门  HsDeptIsLast
        strVal = this.rdbHsDeptIsLastYes.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("HsDeptIsLast", strVal, out strOutMsg);


        //一般报销核算项目模式级别  YbbxHsxmMode
        strVal = this.rdbHsxmHighModel.Checked == true ? "1" : "0";
        iRel = bllConfig.SetModuleDisable("YbbxHsxmMode", strVal, out strOutMsg);
        
        
        if (iRel < 1)
        {
            Response.Write("设置是否控制点数失败，原因" + strOutMsg);
        }
        
        if (iRel>0)
        {
             ClientScript.RegisterStartupScript(this.GetType(), "doOpen", "alert('保存成功！');", true);
        }
    }
}
