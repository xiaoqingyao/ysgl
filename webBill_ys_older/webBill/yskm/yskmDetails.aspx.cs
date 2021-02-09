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
using Bll.UserProperty;
using Models;

public partial class webBill_yskm_yskmDetails : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                this.showData();
            }
        }
    }

    void showData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        //绑定对应单据类型
        DataTable dtbill = server.GetDataTable("select diccode,dicname from bill_datadic where dictype='18'", null);
        if (dtbill != null && dtbill.Rows.Count > 0)
        {
            this.ddlBill.DataSource = dtbill;
            this.ddlBill.DataTextField = "dicname";
            this.ddlBill.DataValueField = "diccode";
            this.ddlBill.DataBind();
            this.ddlBill.Items.Insert(0, new ListItem("--请选择--", ""));
            this.ddlBill.SelectedValue = "02";
        }


        dtbill = server.GetDataTable("select diccode,dicname from bill_datadic where dictype='22'", null);
        if (dtbill != null && dtbill.Rows.Count > 0)
        {
            this.DropDownList4.DataSource = dtbill;
            this.DropDownList4.DataTextField = "dicname";
            this.DropDownList4.DataValueField = "diccode";
            this.DropDownList4.DataBind();
            this.DropDownList4.Items.Insert(0, new ListItem("--请选择--", ""));
        }
        if (type == "add")
        {
            this.getYskmCode();
            allowTzYes.Checked = true;
        }
        else
        {
            string code = Page.Request.QueryString["kmCode"].ToString().Trim();
            DataSet temp = server.GetDataSet("select * from bill_yskm where yskmcode='" + code + "'");
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txb_kmcode.Text = temp.Tables[0].Rows[0]["yskmcode"].ToString().Trim();
                this.txb_kmmc.Value = temp.Tables[0].Rows[0]["yskmmc"].ToString().Trim();
                this.txtTbsm.Value = temp.Tables[0].Rows[0]["tbsm"].ToString().Trim();
                this.DropDownList1.SelectedValue = temp.Tables[0].Rows[0]["tblx"].ToString().Trim();
                this.DropDownList2.SelectedValue = temp.Tables[0].Rows[0]["kmStatus"].ToString().Trim();
                this.DropDownList3.SelectedValue = temp.Tables[0].Rows[0]["kmlx"].ToString().Trim();

                string strnamecode = temp.Tables[0].Rows[0]["kmzg"].ToString().Trim();
                if (strnamecode != "" && strnamecode != null)
                {
                    string[] arrCodes = strnamecode.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    string strShowStr = "";
                    for (int i = 0; i < arrCodes.Length; i++)
                    {
                        string strsql = "select  usercode+'['+ username+']' from bill_users where usercode='" + arrCodes[i] + "'";
                        strShowStr += server.GetCellValue(strsql);
                        strShowStr += ":";
                    }
                    if (strShowStr.Length > 0)
                    {
                        this.txt_spr.Value = strShowStr.Substring(0, strShowStr.Length - 1);
                    }
                }
                //this.txt_spr.Value = temp.Tables[0].Rows[0]["kmzg"].ToString().Trim();

                if (temp.Tables[0].Rows[0]["gkfy"].ToString().Trim() != "1")
                {
                    gkfy.Checked = true;
                }
                else
                {
                    gkfy1.Checked = true;
                }
                if (temp.Tables[0].Rows[0]["xmhs"].ToString().Trim() != "1")
                {
                    xmhs.Checked = true;
                }
                else
                {
                    xmhs1.Checked = true;
                }
                if (temp.Tables[0].Rows[0]["bmhs"].ToString().Trim() != "1")
                {
                    bmhs.Checked = true;
                }
                else
                {
                    bmhs1.Checked = true;
                }
                if (temp.Tables[0].Rows[0]["ryhs"].ToString().Trim() != "1")
                {
                    ryhs.Checked = true;
                }
                else
                {
                    ryhs1.Checked = true;
                }

                if (temp.Tables[0].Rows[0]["zjhs"].ToString().Trim() != "1")
                {
                    rdbJian.Checked = true;

                }
                else
                {
                    rdbAdd.Checked = true;
                }
                //对应单据
                this.ddlBill.SelectedValue = temp.Tables[0].Rows[0]["dydj"].ToString().Trim();
                this.DropDownList4.SelectedValue = Convert.ToString(temp.Tables[0].Rows[0]["kmType"]).Trim();
                if (temp.Tables[0].Rows[0]["allowTz"].ToString().Trim() != "1")
                {
                    allowTzNo.Checked = true;

                }
                else
                {
                    allowTzYes.Checked = true;
                }
                if (string.IsNullOrEmpty(temp.Tables[0].Rows[0]["iszyys"].ToString().Trim()) || temp.Tables[0].Rows[0]["iszyys"].ToString().Trim() == "1")
                {
                    allowZyysYes.Checked = true;
                }
                else
                {
                    allowZyysNo.Checked = true;
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请管理员联系！');", true);
                this.btn_save.Visible = false;
            }
        }
    }



    public void getYskmCode()
    {
        string yskmCode = (new billCoding()).getYskmCode(Page.Request.QueryString["pCode"].ToString().Trim());
        if (yskmCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成科目编码错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txb_kmcode.Text = yskmCode;
        }
    }
    protected void btnAgain_Click(object sender, EventArgs e)
    {
        this.getYskmCode();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string sql = "";
        string type = Page.Request.QueryString["type"].ToString().Trim();
        string strgkfy = "1";

        if (gkfy.Checked == true)
        {
            strgkfy = "0";
        }
        string strxmhs = "1";
        if (xmhs.Checked == true)
        {
            strxmhs = "0";
        }
        string strbmhs = "1";
        if (bmhs.Checked == true)
        {
            strbmhs = "0";
        }
        string strryhs = "1";
        if (ryhs.Checked == true)
        {
            strryhs = "0";
        }

        string strzjhs = "1";
        if (rdbJian.Checked == true)
        {
            strzjhs = "0";
        }

        string strAllowTz = "1";
        if (allowTzNo.Checked)
        {
            strAllowTz = "0";

        }
        string strZyys = "1";
        if (allowZyysNo.Checked)
        {
            strZyys = "0";
        }


        string strloankm = this.txt_spr.Value.Trim();
        string[] arrKm = strloankm.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        string strkmRel = "";
        //if (arrKm.Length>1)
        //{

        for (int i = 0; i < arrKm.Length; i++)
        {
            strkmRel += arrKm[i].Substring(0, arrKm[i].IndexOf("["));
            strkmRel += ":";
        }

        if (arrKm.Length > 0)
        {
            strkmRel = strkmRel.Substring(0, strkmRel.Length - 1);
        }

        //对应单据
        string strdydj = this.ddlBill.SelectedValue;
        if (strdydj.Equals(""))
        {
            strdydj = "02";
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择对应单据！');;", true);
            //return;
        }

        string kmType = this.DropDownList4.SelectedValue;
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select * from bill_yskm where yskmcode='" + this.txb_kmcode.Text.ToString().Trim() + "'");

            if (temp.Tables[0].Rows.Count != 0)
            {
                this.getYskmCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的预算科目已存在,系统已重新生成,请保存！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            sql = "insert into bill_yskm(yskmcode,yskmbm,yskmmc,tbsm,tblx,kmStatus,kmlx,gkfy,xmhs,bmhs,ryhs,kmzg,dydj,zjhs,allowTz,kmType,iszyys) values('" + this.txb_kmcode.Text.ToString().Trim() + "','" + this.txb_kmcode.Text.ToString().Trim() + "','" + this.txb_kmmc.Value.ToString().Trim() + "','" + this.txtTbsm.Value.ToString().Trim() + "','" + this.DropDownList1.SelectedItem.Value + "','" + this.DropDownList2.SelectedItem.Value + "','" + this.DropDownList3.SelectedItem.Value + "','" + strgkfy + "','" + strxmhs + "','" + strbmhs + "','" + strryhs + "','" + strkmRel + "','" + strdydj + "','" + strzjhs + "','" + strAllowTz + "','" + kmType + "','" + strZyys + "')";
        }
        else
        {
            sql = "update bill_yskm set yskmbm='" + this.txb_kmcode.Text.ToString().Trim() + "',yskmmc='" + this.txb_kmmc.Value.ToString().Trim() + "',tbsm='" + this.txtTbsm.Value.ToString().Trim() + "',tblx='" + this.DropDownList1.SelectedItem.Value + "',kmStatus='" + this.DropDownList2.SelectedItem.Value + "',kmlx='" + this.DropDownList3.SelectedItem.Value + "',gkfy='" + strgkfy + "',xmhs='" + strxmhs + "',bmhs='" + strbmhs + "',ryhs='" + strryhs + "',kmzg='" + strkmRel + "',dydj='" + strdydj + "',zjhs='" + strzjhs + "', allowTz='" + strAllowTz + "',iszyys='" + strZyys + "' ,kmType='" + kmType + "'  where yskmcode='" + this.txb_kmcode.Text.ToString().Trim() + "'";
        }


        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }

        else
        {
            if (type == "edit")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！')", true);
            this.getYskmCode();
            this.txb_kmmc.Value = "";
            this.txtTbsm.Value = "";
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
    }
}
