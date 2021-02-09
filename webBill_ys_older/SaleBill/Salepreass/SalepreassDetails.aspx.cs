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

public partial class SaleBill_Salepreass_SalepreassDetails : System.Web.UI.Page
{
    Dal.SaleProcess.SaleProcessDal saldal = new Dal.SaleProcess.SaleProcessDal();
    Models.SaleProcessMode salemode = new Models.SaleProcessMode();


    string strunsernid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        object objRequest = Request["Code"];
        if (objRequest != null && !string.IsNullOrEmpty(objRequest.ToString()))
        {
            strunsernid = objRequest.ToString();
        }

        if (!IsPostBack)
        {
            init();
        }
    }
    private void init()
    {


        //如果是修改的话 为控件赋初始值
        if (!string.IsNullOrEmpty(strunsernid))
        {

            salemode = saldal.GetModel(strunsernid);
            if (salemode == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>showMessage('获取对象失败，请刷新后重试！');</script>");
                return;
            }
            else
            {
                this.txtnid.Enabled = false;
                if (Request["ctrl"].ToString() == "view")
                {
                    this.txtnid.Enabled = false;
                    this.txtname.Enabled = false;
                    Button4.Visible = false;
                    this.redY.Enabled = false;
                    this.redN.Enabled = false;

                    this.Button1.Text = "关 闭";
                }
                this.txtnid.Text = salemode.Code.ToString();

                this.txtname.Text = salemode.PName;
                if (salemode.Status == "1")
                {
                    this.redY.Checked = true;
                    this.redN.Checked = false;
                }
                else
                {
                    this.redN.Checked = true;
                    this.redY.Checked = false;
                }

            }

        }
        else
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");

            string strcode = pusbll.GetBillCode("SP", strneed, 1, 6);
            this.txtnid.Text = strcode;
            // this.redY.Checked;

        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Models.SaleProcessMode salemode = new Models.SaleProcessMode();
        salemode.Code = txtnid.Text;
        salemode.PName = this.txtname.Text;
        if (this.redN.Checked)
        {
            salemode.Status = "0";
        }
        else
        {
            salemode.Status = "1";
        }

        salemode.Note1 = "";
        salemode.Note2 = "";
        salemode.Note3 = "";
        salemode.Note4 = "";
        salemode.Note5 = "";
        string script;

        if (!string.IsNullOrEmpty(strunsernid))
        {
            salemode.Code = strunsernid;

            if (saldal.Updetesalep(salemode) > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改成功！');window.returnValue=\"sucess\";self.close();", true);
            }

            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改失败！');window.returnValue=\"sucess\";self.close();", true);
            }
        }
        else
        {
            try
            {
                if (this.txtname.Text != "" && this.txtname.Text != null)
                {
                    saldal.Add(salemode);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('名称不能为空！');", true);

                }

            }
            catch (Exception ex)
            {
                script = "<script>alert('" + ex + "');</script>";
            }
        }

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("SaleProcessList.aspx");
    }
}
