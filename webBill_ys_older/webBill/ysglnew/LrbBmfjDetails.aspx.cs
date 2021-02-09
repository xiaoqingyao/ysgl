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

public partial class webBill_ysglnew_LrbBmfjDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    string strNd = "";//年度
    string strdeptCode = "";//部门编号
    string strkmCode = "";//科目编号
    string strymoney = "";//原预算金额
    string strwfpmoney = "";//未分配金额 


    protected void Page_Load(object sender, EventArgs e)
    {
        // openDetail("LrbBmfjDetails.aspx?nd="+varnd+"&deptCode=" + vardeptcode+"&yskmcode"+varkmcode+"&ymoney="+ysmoney+"&wfpmoney="+varwfpmoney);

        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                if (Request["nd"] != null)
                {
                    strNd = Request["nd"].ToString() + "0001";
                }
                if (Request["deptCode"] != null)
                {
                    strdeptCode = Request["deptCode"].ToString();
                }
                if (Request["yskmcode"] != null)
                {
                    strkmCode = Request["yskmcode"].ToString();
                }
                if (Request["ymoney"] != null)
                {
                    strymoney = Request["ymoney"].ToString();
                }
                else
                {
                    strymoney = "0.00";
                }
                if (Request["wfpmoney"] != null)
                {
                    strwfpmoney = Request["wfpmoney"].ToString();
                }
                else
                {
                    strwfpmoney = "0.00";
                }
                this.showData();
            }
        }
    }
    /// <summary>
    /// 显示数据
    /// </summary>
    private void showData()
    {
        this.lblwfmoney.Text = strwfpmoney.ToString().Trim();//未分配金额
        if (strymoney.ToString().Trim() != "")
        {
            this.lblysmoney.Text = strymoney.ToString().Trim();//原预算金额
        }
        else
        {
            this.lblysmoney.Text = "0";
        }

    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btsave_Click(object sender, EventArgs e)
    {
        double decfpmoney = 0;//追加的
        double decysmoney = 0;//预算
        double decwfpmoney = 0;//未分配金额
        strwfpmoney = Request["wfpmoney"].ToString();
        string strtxtymoney = this.txtymoney.Text.Trim();//追加金额
        if (strtxtymoney != "")
        {
            double.TryParse(strtxtymoney, out decfpmoney);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('可追加金额不能为空！');", true);
            return;
        }
        string strlbymoney = this.lblysmoney.Text;//原预算金额
        if (strlbymoney != "")
        {
            double.TryParse(strlbymoney, out decysmoney);
        }
        else
        {
            double.TryParse("0", out decysmoney);
        }

        double.TryParse(strwfpmoney, out decwfpmoney);

        double fendmoney = 0;
        fendmoney = decysmoney + decfpmoney;//预算+追加的
        if (decfpmoney > decwfpmoney)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('分配金额超出可分配金额！');", true);
            return;
        }

        //2014-05-29 beg
        //年度 各月份的预算和
        string strYfpzje = server.GetCellValue("select isnull(sum(ysje),0)from bill_ysmxb where  ysType in (1,5) and gcbh in (select gcbh from bill_ysgc where nian='" + Request["nd"] + "' and ystype='2') and ysDept ='" + Request["deptCode"] + "' and yskm='" + Request["yskmcode"] + "'");
        double dYfpzje = Convert.ToDouble(strYfpzje);

        //年度预算
        string strNdys = server.GetCellValue("select isnull(sum(ysje),0)from bill_ysmxb where  ysType in (1,5) and gcbh in (select gcbh from bill_ysgc where nian='" + Request["nd"] + "' and ystype='0') and ysDept ='" + Request["deptCode"] + "' and yskm='" + Request["yskmcode"] + "'");
        double dNdys = Convert.ToDouble(strNdys);
        if (dYfpzje > decfpmoney + dNdys && dNdys >= 0) //已分配金额>追加后的金额和 则提示追加不成功
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('追加后金额(" + string.Format("{0:N2}", decfpmoney + dNdys) + "小于已分配金额(" + string.Format("{0:N2}", dYfpzje) + "),相差:" + string.Format("{0:N2}", dYfpzje - (decfpmoney + dNdys)) + "！');", true);
            return;
        }
        else if (dYfpzje < decfpmoney + dNdys && dNdys < 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('追加后金额(" + string.Format("{0:N2}", decfpmoney + dNdys) + "小于已分配金额(" + string.Format("{0:N2}", dYfpzje) + "),相差:" + string.Format("{0:N2}", dYfpzje - (decfpmoney + dNdys)) + "！');", true);
            return;
        }
        //2014-05-29 end
        string stryscg = Request["nd"].ToString() + "0001";
        string strcodedept = Request["deptCode"].ToString();
        string strcodekm = Request["yskmcode"].ToString();
        string strNd = Request["nd"].ToString();
        if (stryscg.Equals("") || strcodedept.Equals("") || strcodekm.Equals("") || strNd.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('参数不足，不能完成操作，请联系管理员！');", true);
            return;
        }

        string billcode = System.Guid.NewGuid().ToString().ToUpper();
        string strSelectsql = "select by3 from bill_ys_xmfjbm where procode='" + strNd + "' and kmcode='" + strcodekm + "' and deptcode='" + strcodedept + "'";
        object objSelectRel = server.ExecuteScalar(strSelectsql);
        //未确认的也就是该科目之前没有分配过任何金额的（在bill_ys_xmfjbm中的记录为空）  要往bill_ys_xmfjbm表中增加状态为‘3’的记录同时增加bill_main和bill_ysmxb增加记录
        if (objSelectRel == null || objSelectRel.ToString() == "")
        {
            System.Collections.Generic.List<string> lstSql = new System.Collections.Generic.List<string>();
            lstSql.Add("insert into bill_ys_xmfjbm(procode,deptcode,kmcode,je,by1,by2,by3) values('" + strNd + "','" + strcodedept + "','" + strcodekm + "','" + decfpmoney + "','0','','2')");
            lstSql.Add("insert into bill_main (billCode,billName,flowid,stepid,billDept,billUser,billDate,billJe,LoopTimes,billType) values('" + billcode + "','" + stryscg + "','ysnzj','end','" + strcodedept + "','" + Session["userCode"].ToString().Trim() + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) + "','" + decfpmoney + "','1','5')");
            lstSql.Add("insert into bill_ysmxb (gcbh,billcode,yskm,ysje,ysdept,ystype) values('" + stryscg + "','" + billcode + "','" + strcodekm + "','" + decfpmoney + "','" + strcodedept + "','5')");
            if (server.ExecuteNonQuerysArray(lstSql) > -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功，请到预算内追加页面增加该部门的月度或季度报销额度。');window.returnValue=\"sucess\";self.close();", true);
            }
            #region 直接修改预算的方式  已屏蔽代码
            ////获取年度预算的billcode
            //string strselectbillcode = "select billcode from bill_main where flowid='ys' and stepid='end' and billname='" + stryscg + "' and billdept='" + strcodedept + "'";
            //object objbillcode = server.ExecuteScalar(strselectbillcode);
            //if (objbillcode == null || objbillcode.ToString().Equals(""))//未做过全年预算的 
            //{
            //    System.Collections.Generic.List<string> lstSql = new System.Collections.Generic.List<string>();
            //    lstSql.Add("insert into bill_ys_xmfjbm(procode,deptcode,kmcode,je,by1,by2,by3) values('" + strNd + "','" + strcodedept + "','" + strcodekm + "','" + decfpmoney + "','0','','2')");
            //    lstSql.Add("insert into bill_ysmxb (gcbh,billcode,yskm,ysje,ysdept,ystype) values('" + stryscg + "','" + billcode + "','" + strcodekm + "','" + fendmoney + "','" + strcodedept + "','1')");
            //    lstSql.Add("insert into bill_main (billCode,billName,flowid,stepid,billDept,billUser,billDate,billJe,LoopTimes,billType) values('" + billcode + "','" + stryscg + "','ys','end','" + strcodedept + "','" + Session["userCode"].ToString().Trim() + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) + "','" + fendmoney + "','1','1')");
            //    int iRel = server.ExecuteNonQuerysArray(lstSql);
            //    if (iRel >= 0)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功，请到预算内追加页面增加部门的月度或季度报销额度。');window.returnValue=\"sucess\";self.close();", true);
            //    }
            //    else
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            //    }
            //}
            //else {
            //    System.Collections.Generic.List<string> lstSql = new System.Collections.Generic.List<string>();
            //    lstSql.Add("insert into bill_ys_xmfjbm(procode,deptcode,kmcode,je,by1,by2,by3) values('" + strNd + "','" + strcodedept + "','" + strcodekm + "','" + decfpmoney + "','0','','2')");
            //    lstSql.Add("insert into bill_ysmxb (gcbh,billcode,yskm,ysje,ysdept,ystype) values('" + stryscg + "','" + objbillcode.ToString() + "','" + strcodekm + "','" + fendmoney + "','" + strcodedept + "','1')");
            //    int iRel = server.ExecuteNonQuerysArray(lstSql);
            //    if (iRel >= 0)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功，请到预算内追加页面增加部门的月度或季度报销额度。');window.returnValue=\"sucess\";self.close();", true);
            //    }
            //    else
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            //    }   
            //}
            #endregion
        }
        //预算确认/部门异议
        else if (objSelectRel.ToString() == "1" || objSelectRel.ToString() == "3")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先等待部门确认后再增加预算内金额！');", true);
            return;
        }
        //部门预算确认的
        else if (objSelectRel.ToString().Equals("2"))
        {
            //操作
            System.Collections.Generic.List<string> lstSql = new System.Collections.Generic.List<string>();
            lstSql.Add("update bill_ys_xmfjbm set je='" + fendmoney + "' where procode='" + Request["nd"].ToString() + "' and deptcode='" + strcodedept + "' and kmcode='" + strcodekm + "'");//修改项目预算分解部门的金额
            lstSql.Add("insert into bill_main (billCode,billName,flowid,stepid,billDept,billUser,billDate,billJe,LoopTimes,billType) values('" + billcode + "','" + stryscg + "','ysnzj','end','" + strcodedept + "','" + Session["userCode"].ToString().Trim() + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) + "','" + decfpmoney + "','1','5')");
            lstSql.Add("insert into bill_ysmxb (gcbh,billcode,yskm,ysje,ysdept,ystype) values('" + stryscg + "','" + billcode + "','" + strcodekm + "','" + decfpmoney + "','" + strcodedept + "','5')");

            if (server.ExecuteNonQuerysArray(lstSql) > -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功，请到预算内追加页面增加该部门的月度或季度报销额度。');window.returnValue=\"sucess\";self.close();", true);
            }
            #region 直接修改预算的方式  已屏蔽代码
            //int intRow = server.ExecuteNonQuery("update bill_ysmxb set ysje='" + fendmoney + "' where gcbh='" + stryscg + "' and yskm='" + strcodekm + "' and ysDept='" + strcodedept + "' and ysType='1'");
            //if (intRow != 0)
            //{
            //    int row = server.ExecuteNonQuery("update bill_ys_xmfjbm set je='" + fendmoney + "' where procode='" + Request["nd"].ToString() + "' and deptcode='" + strcodedept + "' and kmcode='" + strcodekm + "'");
            //    if (row != 0)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功，请到预算内追加页面增加部门的月度或季度报销额度。');window.returnValue=\"sucess\";self.close();", true);
            //    }
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            //}
            #endregion
        }
    }
}
