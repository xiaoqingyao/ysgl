using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_MyWorkFlow_WorkReturnHistory : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlShenPiRen.DataSource = server.GetDataTable("select distinct usercode from bill_ReturnHistory ", null);
            ddlShenPiRen.DataTextField = "usercode";
            ddlShenPiRen.DataValueField = "usercode";
            ddlShenPiRen.DataBind();
            ddlShenPiRen.Items.Insert(0, new ListItem("--全部--", ""));


            DropDownList1.DataSource = server.GetDataTable("select * from dbo.bill_djlx where djbh not in('srys','zcys','chys','wlys','yshz','ys','xmys','xmyshz','yszjhz') ", null);
            DropDownList1.DataTextField = "djmc";
            DropDownList1.DataValueField = "djbh";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("--全部--", ""));

            binddata();
        }
    }
    private void binddata()
    {
        this.myGrid.DataSource = getdt();
        this.myGrid.DataBind();
    }

    public DataTable getdt()
    {
        string usercode = this.ddlShenPiRen.SelectedValue;
        string djlx = DropDownList1.SelectedValue;

        string sql = @"select main.billCode as mainbillcode,(select flowname from mainworkflow where flowID=main.flowID) as djlx,* from bill_ReturnHistory bh,bill_main main 
 where (bh.billcode=main.billCode or bh.billcode=main.billName)   and flowID not in('ys','xmys','xmyshz','yshz','srys') ";

        if (usercode != "")
        {
            sql += " and bh.usercode='" + usercode + "'";
        }

        if (!string.IsNullOrEmpty(djlx))
        {
            sql += " and flowid='" + djlx + "'";
        }

        string bgtime = begtime.Text;
        if (!string.IsNullOrEmpty(bgtime))
        {
            sql += " and dt >='" + bgtime + "'";
        }
        string edtime = endtime.Text;
        if (!string.IsNullOrEmpty(edtime))
        {
            sql += " and dt <='" + edtime + "'";
        }

        string billname = txt_billname.Text;
        if (!string.IsNullOrEmpty(billname))
        {
            sql += " and bh.billcode like '%" + billname + "%'";
        }
        sql += "  order by bh.billcode ,bh.usercode";
        //   Response.Write(sql);
        return server.GetDataTable(sql, null);

    }
    protected void ddlShenPiRen_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddata();
    }
    protected void btn_excle_Click(object sender, EventArgs e)
    {

        DataTable dt = getdt();

        //   new ExcelHelper().ExpExcel(dt);
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        string billcode = e.Item.Cells[0].Text;
        string billname = e.Item.Cells[1].Text;
        string flowid = e.Item.Cells[6].Text;
        string deptcode = e.Item.Cells[7].Text;
        //bxgl/bxDetailForDz.aspx?type=look&billCode=20160217016&dydj=06&djmxlx=01&isDZ=1

        // bxgl/bxDetailForDz.aspx?type=look&billCode=20160217077&dydj=02&djmxlx=01&isDZ=1
        if (!string.IsNullOrEmpty(flowid) && (flowid == "yksq_dz"))
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailForDz.aspx?type=look&billCode=" + billname + "&dydj=06&djmxlx=01&isDZ=1');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
            //   e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../ysgl/yszjEdit.aspx?type=look&billCode=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";

        }
        if (!string.IsNullOrEmpty(flowid) && (flowid == "ybbx"))
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailForDz.aspx?type=look&billCode=" + billname + "&dydj=02&djmxlx=01&isDZ=1');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";

        }
        if (!string.IsNullOrEmpty(flowid) && flowid == "tfsq")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/jkDetailForDz.aspx?type=look&billCode=" + billname + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";

        }
        //关系学员特惠信息 fysq/gxxythxxDetail_dz.aspx?ctrl=view&billcode=1BA5949A-74FA-45B3-9DE6-45C29A2FAE53
        if (!string.IsNullOrEmpty(flowid) && flowid == "xyth")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../fysq/gxxythxxDetail_dz.aspx?ctrl=view&billcode=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";

        }
        //物品申购单
        if (!string.IsNullOrEmpty(flowid) && flowid == "fzcgz")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/jkDetailForDz.aspx?ctrl=view&billcode=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";

        }
        //预算调整/yd_ystzDetail_dz.aspx?type=look&deptcode=0104&billCode=18376F6C-D8FE-429C-A2CB-20698B435D8D
        if (!string.IsNullOrEmpty(flowid) && flowid == "ystz")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../ysgl/yd_ystzDetail_dz.aspx?type=look&billcode=" + billcode + "&deptcode=" + deptcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
        //jfsq bxgl/ZijinShenqingDetails.aspx?ctrl=view&billcode=1D52619A-EEE4-47EA-B5C0-C3C0F3D1BDAF
        if (!string.IsNullOrEmpty(flowid) && flowid == "jfsq")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/ZijinShenqingDetails.aspx?ctrl=view&billcode=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
        //            bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=B59C4E5A-C4F4-475C-92F9-32ECDD784805&flg=n
        //nzfzxsqd    bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=2D0922CA-E3C7-419F-985D-D6822D2B634D&flg=k
        if (!string.IsNullOrEmpty(flowid) && flowid == "nzfzxsqd")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=" + billcode + "'&flg=n);\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
        if (!string.IsNullOrEmpty(flowid) && flowid == "zfzxsqd")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=" + billcode + "'&flg=k);\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
        //ysgl/yszjEdit.aspx?type=look&billCode=AC678608-986E-4D7F-A873-50DB9B46FACB
        if (!string.IsNullOrEmpty(flowid) && flowid == "yszj")
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../ysgl/yszjEdit.aspx?type=look&billCode=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
        // 资产购置申请单  物品申购单 fysq/ZcgzsqDetail.aspx?Ctrl=View&Code=zcgz20160219002

        if (!string.IsNullOrEmpty(flowid) && (flowid == "zcgz" || flowid == "fzcgz"))
        {
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('../fysq/ZcgzsqDetail.aspx?Ctrl=View&Code=" + billcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
    }

    protected void btn_cx_Click(object sender, EventArgs e)
    {
        binddata();
    }
}