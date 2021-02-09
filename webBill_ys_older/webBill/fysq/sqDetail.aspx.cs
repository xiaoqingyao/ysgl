using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using Ajax;

public partial class fysq_sqDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(fysq_sqDetail));

            string type = Page.Request.QueryString["type"].ToString().Trim();
            if (type == "look")
            {
                this.rad_sfjkf.Enabled = false;
                this.rad_sfjks.Enabled = false;
                this.btn_sqr.Visible = false;
            }
            else
            {
                this.txt_billDate.Attributes.Add("onfocus", "javascript:setday(this);");
                this.rad_sfjks.Attributes.Add("onclick", "javascript:changeJkStatus();");
                this.rad_sfjkf.Attributes.Add("onclick", "javascript:changeJkStatus();");
               
                //绑定选择申请人
                this.btn_sqr.Attributes.Add("onclick", "javascript:openDetail('../select/userFrame.aspx');return false;");
            }
            if (!IsPostBack)
            {
                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='01' order by dicCode");
                this.ddl_jkdjlx.DataTextField = "dicName";
                this.ddl_jkdjlx.DataValueField = "dicCode";
                this.ddl_jkdjlx.DataSource = temp;
                this.ddl_jkdjlx.DataBind();
                this.showData();
            }
        }
    }

    #region 绑定页面数据

    protected void showData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();//生成guid作为billCode
        if (type == "add")
        {
            //当前登录人员
            txt_billUser.Text = "[" + Session["userCode"].ToString() + "]" + Session["userName"].ToString();
            txt_billDate.Text = System.DateTime.Now.ToShortDateString();
        }
        else
        {
            this.lbl_BillCode.Text = Page.Request.QueryString["billCode"].ToString().Trim();
            //获取单据的详细信息并显示

            #region 绑定申请基本信息项
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.billCode,(select '['+usercode+']'+username from bill_users where usercode =b.jbr) as jbr,(select '['+usercode+']'+username from bill_users where usercode =a.billUser) as billUser,a.billJe,convert(varchar(11),a.billDate,20) as billDate,b.jkdjlx,a.billJe,b.sqzy,b.sqbz,b.dwmc,b.khh,b.yhzh,b.sfjk ");
            sb.Append(" from [bill_main] a ,[bill_fysq] b ");
            sb.Append(" where  a.billcode =b.billcode and a.billcode='" + Request.QueryString["billCode"].ToString() + "'");
            DataSet temp = server.GetDataSet(sb.ToString());
            if (temp.Tables[0].Rows.Count == 1)
            {
                //主表信息项
                txt_billUser.Text = temp.Tables[0].Rows[0]["billUser"].ToString();
                txt_billDate.Text = temp.Tables[0].Rows[0]["billDate"].ToString();
                txt_billJe.Text = temp.Tables[0].Rows[0]["billJe"].ToString();
                //明细表信息项
                txt_jkr.Value = temp.Tables[0].Rows[0]["jbr"].ToString();
                ddl_jkdjlx.SelectedValue = temp.Tables[0].Rows[0]["jkdjlx"].ToString();
                txt_sqzy.Text = temp.Tables[0].Rows[0]["sqzy"].ToString();
                txt_sqbz.Text = temp.Tables[0].Rows[0]["sqbz"].ToString();
                if (temp.Tables[0].Rows[0]["sfjk"].ToString() == "1")
                {
                    rad_sfjks.Checked = true;
                }
                else
                {
                    rad_sfjkf.Checked = true;
                }
                //附件可能存在多个需要处理
                txt_dwmc.Text = temp.Tables[0].Rows[0]["dwmc"].ToString();
                txt_khh.Text = temp.Tables[0].Rows[0]["khh"].ToString();
                txt_yhzh.Text = temp.Tables[0].Rows[0]["yhzh"].ToString();
            }
            #endregion

            #region 绑定借款金额
            string str_jksql = "select mxname ,sum(je) as je from bill_fysq_mxb where billcode='" + Request.QueryString["billCode"].ToString() + "' group by mxname ";
            DataSet ds = server.GetDataSet(str_jksql);
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rad_sfjks.Checked = true;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["mxname"].ToString() == "支票")
                        {
                            txt_zp.Text = ds.Tables[0].Rows[i]["je"].ToString();
                        }
                        else if (ds.Tables[0].Rows[i]["mxname"].ToString() == "现金")
                        {
                            txt_xj.Text = ds.Tables[0].Rows[i]["je"].ToString();
                        }
                        else if (ds.Tables[0].Rows[i]["mxname"].ToString() == "汇款")
                        {
                            txt_hk.Text = ds.Tables[0].Rows[i]["je"].ToString();
                        }
                    }
                }
            }
            catch { }
            #endregion

            //绑定附件
            bindFj();

            if (type == "look")
            {
                this.btn_sqr.Visible = false;
                this.Button1.Visible = false;
                this.btn_bc.Visible = false;
                this.upLoadFiles.Visible = false;

                txt_billDate.Enabled=false;//借款日期
                txt_billJe.Enabled = false;//借款金额
                ddl_jkdjlx.Enabled = false;//借款类型
                txt_sqzy.Enabled = false;//申请摘要
                txt_sqbz.Enabled = false;//申请备注 
                txt_dwmc.Enabled = false;//单位名称
                txt_khh.Enabled = false;//开户行
                txt_yhzh.Enabled = false;  //银行账户

                //借款明细
                txt_xj.Enabled = false;//现金
                txt_zp.Enabled = false;//支票
                txt_hk.Enabled = false;//汇款
            }
            ClientScript.RegisterStartupScript(this.GetType(), "", "changeJkStatus();", true);
        }
    }
    #endregion

    protected void btn_bc_Click(object sender, EventArgs e)
    {
        List<string> str_sql = new List<string>();
        string str_stepid = "-1";
        //主表信息项
        if (this.txt_jkr.Value.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('借款人不能为空！');", true);
            return;
        }
        string str_billuser = txt_jkr.Value.Substring(txt_jkr.Value.IndexOf("[") + 1, txt_jkr.Value.IndexOf("]") - 1);

        //借款人所在单位
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string str_billdate = txt_billDate.Text;//借款日期
        string str_billje = txt_billJe.Text;//借款金额

        //明细表信息项
        if (this.txt_billUser.Text.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('经办人不能为空！');", true);
            return;
        }
        string str_jkr = this.txt_billUser.Text.Substring(txt_billUser.Text.IndexOf("[") + 1, txt_billUser.Text.IndexOf("]") - 1);


        string str_sfjk = "0";//是否借款
        if (rad_sfjks.Checked)
        {
            str_sfjk = "1";
        }
        string str_jkdwlx = ddl_jkdjlx.SelectedValue;//借款类型
        string str_sqzy = txt_sqzy.Text;//申请摘要
        string str_sqbz = txt_sqbz.Text;//申请备注 
        string str_dwmc = txt_dwmc.Text;//单位名称
        string str_khh = txt_khh.Text;//开户行
        string str_yhzh = txt_yhzh.Text;  //银行账户

        //借款明细
        string str_xj = txt_xj.Text;//现金
        string str_zp = txt_zp.Text;//支票
        string str_hk = txt_hk.Text;//汇款

        if (Request.QueryString["type"].ToString() == "add")
        {
            //添加单据时 
            //查询最大Request.QueryString["billCode"].ToString()
            //string str_billcode = server.GetCellValue("select Convert(Varchar(8),GetDate(),112)+IsNull(Right( '0000'+Rtrim(Right(Max(billcode),4)+1),4), '0001 ') From   bill_main   Where   Left(billcode,8)=   Convert(Varchar(10),GetDate(),112) ");
            //添加单据时billcode 为guid
            string str_billcode = lbl_BillCode.Text.Trim();

            //借款申明主表添加
            str_sql.Add("insert into bill_main(looptimes,billType,billcode,billname,flowid,stepid,billuser,billdate,billdept,billje) values(1,'1','" + str_billcode + "','','fysq','" + str_stepid + "','" + str_billuser + "','" + str_billdate + "','" + str_billdept + "','" + str_billje + "')");
            //借款申请明细表添加
            str_sql.Add("insert into bill_fysq(billcode,jbr,jkdjlx,sqzy,sqbz,sfjk,dwmc,khh,yhzh,sfgf,sfth,hjje) values('" + str_billcode + "','" + str_jkr + "','" + str_jkdwlx + "','" + str_sqzy + "','" + str_sqbz + "','" + str_sfjk + "','" + str_dwmc + "','" + str_khh + "','" + str_yhzh + "','0','0'," + str_billje + ")");
            //借款明细根据是否借款
            if (rad_sfjks.Checked)
            {
                //借款申请借款表添加 
                if (str_xj != "0.00" && str_xj != "")
                {
                    string newGuid = (new GuidHelper()).getNewGuid();
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je,mxGuid) values('" + str_billcode + "','现金'," + str_xj + ",'"+newGuid+"')");
                }
                if (str_zp != "0.00" && str_zp != "")
                {
                    string newGuid = (new GuidHelper()).getNewGuid();
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je,mxGuid) values('" + str_billcode + "','支票'," + str_zp + ",'" + newGuid + "')");
                }
                if (str_hk != "0.00" && str_hk != "")
                {
                    string newGuid = (new GuidHelper()).getNewGuid();
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je,mxGuid) values('" + str_billcode + "','汇款'," + str_hk + ",'" + newGuid + "')");
                }
            }
            //借款申请附件表添加，更新附件表的 billcode 
            str_sql.Add("update   bill_fysq_fjb set  djstatus='1' where  billcode='" + lbl_BillCode.Text.Trim() + "'");

        }
        else if (Request.QueryString["type"].ToString() == "edit")
        {
            //修改单据时
            str_sql.Add("update bill_fysq set  jbr='" + str_jkr + "',jkdjlx='" + str_jkdwlx + "',sqzy='" + str_sqzy + "',sqbz='" + str_sqbz + "',sfjk='" + str_sfjk + "',dwmc='" + str_dwmc + "',khh='" + str_khh + "',yhzh='" + str_yhzh + "',hjje=" + str_billje + " WHERE billcode='" + Request.QueryString["billCode"].ToString() + "'");
            str_sql.Add("update bill_main set  billname='' , billuser='" + str_billuser + "',billdate='" + str_billdate + "',billdept='" + str_billdept + "',billje='" + str_billje + "',stepid='" + str_stepid + "' where flowid='fysq' and  billcode='" + Request.QueryString["billCode"].ToString() + "'");
            if (rad_sfjks.Checked)
            {
                //借款申请借款表修改：先删除再添加
                str_sql.Add("delete from bill_fysq_mxb where billcode='" + Request.QueryString["billCode"].ToString() + "'");
                if (str_xj != "0.00" && str_xj != "")
                {
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je) values('" + Request.QueryString["billCode"].ToString() + "','现金'," + str_xj + ")");
                }
                if (str_zp != "0.00" && str_zp != "")
                {
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je) values('" + Request.QueryString["billCode"].ToString() + "','支票'," + str_zp + ")");
                }
                if (str_hk != "0.00" && str_hk != "")
                {
                    str_sql.Add(" insert  into bill_fysq_mxb(billcode,mxname,je) values('" + Request.QueryString["billCode"].ToString() + "','汇款'," + str_hk + ")");
                }
            }
            else
            {  //借款申请借款表修改：选择不借款则删除明细
                str_sql.Add("delete from bill_fysq_mxb where billcode='" + Request.QueryString["billCode"].ToString() + "'");
            }
            //借款申请附件表添加，更新附件表的 billcode 
            str_sql.Add("update   bill_fysq_fjb set  djstatus='1' where  billcode='" + Request.QueryString["billCode"].ToString() + "'");
        }
        else
        { }
        if (server.ExecuteNonQuerysArray(str_sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.upLoadFiles.PostedFile.ContentLength == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待上传文件！');", true);
            return;
        }

        string guid = (new GuidHelper()).getNewGuid();
        string fileName = this.upLoadFiles.PostedFile.FileName;
        string cFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1, fileName.Length - fileName.LastIndexOf("\\") - 1);//0\\2
        string extName = fileName.Substring(fileName.LastIndexOf(".") + 1, fileName.Length - fileName.LastIndexOf(".") - 1);

        try
        {
            string newPath = Server.MapPath(".") + "/files/" + guid + "." + extName;

            FileInfo file2 = new FileInfo(newPath);

            if (file2.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('文件上传失败！');", true);
                return;
            }
            else
            {
                this.upLoadFiles.PostedFile.SaveAs(newPath);

                string sql = "insert into bill_fysq_fjb(billcode,fjguid,fjurl,fjname,djstatus) values('" + this.lbl_BillCode.Text.ToString().Trim() + "','" + guid + "','" + guid + "." + extName + "','" + cFileName + "','0');";

                if (server.ExecuteNonQuery(sql) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上传失败！');", true);
                    return;
                }
                else
                {
                    //上传成功后的处理 显示列表
                    bindFj();
                    ClientScript.RegisterStartupScript(this.GetType(), "", "changeJkStatus();", true);
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('数据库记录失败！');", true);
            return;
        }

    }

    #region 绑定附件
    protected void bindFj()
    {
        string str_sql = "select * from bill_fysq_fjb where 1=1";
        if (Page.Request.QueryString["type"].ToString().Trim() == "add")
        {
            //添加的时候显示
            str_sql += " and billcode='" + this.lbl_BillCode.Text.Trim() + "'";
        }
        else
        {
            str_sql += " and billcode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'";
        }

        DataSet temp = server.GetDataSet(str_sql);//修改为 billCode='"+this.lbl_BillCode.Text.ToString().Trim()+"'
        StringBuilder sb = new StringBuilder();
        //sb.Append("<table border=0 cellpadding=0 cellspacing=0 width=100%>");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string classStr = "class=\"tdNone\"";
            if (i == temp.Tables[0].Rows.Count - 1)
            { classStr = "class=\"tdNone\""; }
            else
            {
                classStr = "";
            }
            sb.Append("<tr><td " + classStr + " colspan=4>&nbsp;<a href=files/" + temp.Tables[0].Rows[i]["fjUrl"].ToString().Trim() + " target=_blank>" + temp.Tables[0].Rows[i]["fjName"].ToString().Trim() + "</a>&nbsp;&nbsp;");
            if (Page.Request.QueryString["type"].ToString().Trim() != "look")
            {
                sb.Append("[<a href=# onclick=\"deleteFj(this,'" + temp.Tables[0].Rows[i]["fjUrl"].ToString().Trim() + "','" + temp.Tables[0].Rows[i]["fjGuid"].ToString().Trim() + "');\">删除</a>]</td></tr>");
            }
            else {
                sb.Append("</td></tr>");
            }
        }
        //sb.Append("</table>");

        this.fjList.InnerHtml = sb.ToString();
        //ClientScript.RegisterStartupScript(this.GetType(), "", "changeJkStatus();", true);
    }
    #endregion

    #region 返回

    protected void btn_fh_Click(object sender, EventArgs e)
    {
        //删除增加的附件 数据库信息
        System.Collections.Generic.List<string> list = new List<string>();
        //list.Add("delete from bill_fysq_fjb where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and djstatus='0'");
        //list.Add("delete from bill_fysq_mxb where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");

        //if (server.ExecuteNonQuerysArray(list) == -1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('附件删除失败！');self.close();", true);
        //}
        //else
        //{
            ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
        //}
    }
    #endregion

    #region 附件删除方法

    /// <summary>
    /// 删除附件Ajax方法
    /// </summary>
    /// <param name="fjGuid"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DelteFj(string tjurl, string fjGuid)
    {
        //先删除附件再删除数据库记录

        if (server.ExecuteNonQuery("delete from bill_fysq_fjb where fjGuid='" + fjGuid + "'") == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
}
