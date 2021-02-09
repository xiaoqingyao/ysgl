using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cwkm_cwkmDetails : System.Web.UI.Page
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
        if (type == "add")
        {
            this.CreateCwkmCode();
        }
        else {
            string code = Page.Request.QueryString["kmCode"].ToString().Trim();
            DataSet temp = server.GetDataSet("select * from bill_cwkm where cwkmcode='" + code + "'");
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txb_kmbm.Text = temp.Tables[0].Rows[0]["cwkmbm"].ToString().Trim();
                this.txb_kmcode.Text = temp.Tables[0].Rows[0]["cwkmcode"].ToString().Trim();
                this.txb_kmmc.Value = temp.Tables[0].Rows[0]["cwkmmc"].ToString().Trim();
                this.Text1.Value = temp.Tables[0].Rows[0]["hsxm1"].ToString().Trim();
                this.Text2.Value = temp.Tables[0].Rows[0]["hsxm2"].ToString().Trim();
                this.Text3.Value = temp.Tables[0].Rows[0]["hsxm3"].ToString().Trim();
                this.Text4.Value = temp.Tables[0].Rows[0]["hsxm4"].ToString().Trim();
                this.Text5.Value = temp.Tables[0].Rows[0]["hsxm5"].ToString().Trim();
                this.txtFangxiang.Value = temp.Tables[0].Rows[0]["Fangxiang"].ToString().Trim();
                this.txtType.Value = temp.Tables[0].Rows[0]["Type"].ToString().Trim();
                this.txtXianShiMc.Value = temp.Tables[0].Rows[0]["XianShiMc"].ToString().Trim();
                this.txtFuZhuHeSuan.Value = temp.Tables[0].Rows[0]["FuZhuHeSuan"].ToString().Trim();
                this.txtJiCi.Value = temp.Tables[0].Rows[0]["JiCi"].ToString().Trim();
                this.txtShiFouFengCun.SelectedValue = temp.Tables[0].Rows[0]["ShiFouFengCun"].ToString().Trim();
                

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请与开发商联系！');", true);
                this.btn_save.Visible = false;
            }
        }
    }



    public void CreateCwkmCode()
    {
        string cwkmCode = (new billCoding()).getCwkmCode(Page.Request.QueryString["pCode"].ToString().Trim());
        if (cwkmCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成科目编码错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txb_kmcode.Text = cwkmCode;
        }
    }
    protected void btnAgain_Click(object sender, EventArgs e)
    {
        this.CreateCwkmCode();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string sql = "";
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select * from bill_cwkm where cwkmcode='" + this.txb_kmcode.Text.ToString().Trim() + "'");

            if (temp.Tables[0].Rows.Count != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的财务科目已存在,请重新生成编号！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            sql = "insert into bill_cwkm(cwkmcode,cwkmbm,cwkmmc,hsxm1,hsxm2,hsxm3,hsxm4,hsxm5,XianShiMc,Type,Fangxiang,JiCi,FuZhuHeSuan,ShiFouFengCun) values('" + this.txb_kmcode.Text.ToString().Trim() + "','" + this.txb_kmbm.Text.ToString().Trim() + "','" + this.txb_kmmc.Value.ToString().Trim() + "','" + this.Text1.Value.ToString().Trim() + "','" + this.Text2.Value.ToString().Trim() + "','" + this.Text3.Value.ToString().Trim() + "','" + this.Text4.Value.ToString().Trim() + "','" + this.Text5.Value.ToString().Trim() + "','"+this.txtXianShiMc.Value.ToString().Trim()+"','"+this.txtType.Value.ToString().Trim()+"','"+this.txtFangxiang.Value.ToString().Trim()+"','"+this.txtJiCi.Value.ToString().Trim()+"','"+this.txtFuZhuHeSuan.Value.ToString().Trim()+"','"+this.txtShiFouFengCun.SelectedValue.ToString().Trim()+"')";
        }
        else {
            sql = "update bill_cwkm set cwkmbm='" + this.txb_kmbm.Text.ToString().Trim() + "',cwkmmc='" + this.txb_kmmc.Value.ToString().Trim() + "',hsxm1='" + this.Text1.Value.ToString().Trim() + "',hsxm2='" + this.Text2.Value.ToString().Trim() + "',hsxm3='" + this.Text3.Value.ToString().Trim() + "',hsxm4='" + this.Text4.Value.ToString().Trim() + "',hsxm5='" + this.Text5.Value.ToString().Trim() + "',XianShiMc='" + this.txtXianShiMc.Value.ToString().Trim() + "',Type='" + this.txtType.Value.ToString().Trim() + "',Fangxiang='"+this.txtFangxiang.Value.ToString().Trim()+"',JiCi='"+this.txtJiCi.Value.ToString().Trim()+"',FuZhuHeSuan='"+this.txtFuZhuHeSuan.Value.ToString().Trim()+"',ShiFouFengCun='"+this.txtShiFouFengCun.SelectedValue.ToString().Trim()+"' where cwkmcode='" + this.txb_kmcode.Text.ToString().Trim() + "'";
        }

        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
}
