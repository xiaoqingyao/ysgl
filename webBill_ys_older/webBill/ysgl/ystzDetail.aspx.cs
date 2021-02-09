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
using Ajax;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Linq;

public partial class webBill_ysgl_ystzDetail : System.Web.UI.Page
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
            //Ajax.Utility.RegisterTypeForAjax(typeof(webBill_ysgl_ystzDetail));
            if (!IsPostBack)
            {
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        string type = Request.Params["type"];
        if (type == "edit")
        {
            YsManager ysMgr = new YsManager();
            string billCode = Request.Params["billCode"];
            IList<Bill_Ysmxb> list = ysMgr.GetYsmxByCode(billCode);
            string sYs = (from tempsys in list
                          where tempsys.Ysje < 0
                          select tempsys.Gcbh).First();
            string dYs = (from tempdys in list
                          where tempdys.Ysje > 0
                          select tempdys.Gcbh).First();
            string sysName = ysMgr.GetYsgcCodeName(sYs);
            string dysName = ysMgr.GetYsgcCodeName(dYs);
            ListItem li = new ListItem();
            li.Text = dysName;
            li.Value = dYs;
            drpTarget.Items.Add(li);

            ListItem lis = new ListItem();
            lis.Text = sysName;
            lis.Value = sYs;
            drpSource.Items.Add(lis);
            bindDataGrid();
            for (int i = 0; i < myGrid1.Items.Count; i++)
            {
                string kmCode = myGrid1.Items[i].Cells[0].Text.Split(']')[0].Trim('[');
                string deptCode = myGrid1.Items[i].Cells[1].Text.Split(']')[0].Trim('[');
                var tempList = from rowData in list
                               where rowData.Gcbh == dYs && rowData.YsDept == deptCode && rowData.Yskm == kmCode
                               select rowData;
                decimal je = 0;
                foreach (var temp in tempList)
                {
                    je += temp.Ysje;
                }
                ((TextBox)myGrid1.Items[i].FindControl("TextBox1")).Text = Convert.ToString(je);
            }
        }
        else
        {
            //根据配置,查看具体启动的是月预算还是季度预算
            string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
            string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];

            DateTime dt = System.DateTime.Now;
            string nian = dt.Year.ToString();
            string yue = dt.Month.ToString();
            string userCode = Session["userCode"].ToString().Trim();
            UserMessage userMgr = new UserMessage(userCode);
            YsManager ysmgr = new YsManager();


            string rootDept = userMgr.GetRootDept().DeptCode;

            

            
            string code = ysmgr.GetYsgcCode(dt);//获得根据时间获得预算过程编号,如果启用了月就取得月过程编号，否则取得季度过程编号！
            IList<Bill_Ysgc> list;
            if (config == "1")
            {
                //季度
                list = ysmgr.GetJdYsByYear(nian);
            }
            else if (config == "0")
            {
                list = ysmgr.GetNianYs();
            }
            else
            {
                //月度
                list = ysmgr.GetYueYsByMonth(nian);
            }
            Bill_Ysgc byys = (from nowys in list
                              where nowys.Gcbh == code
                              select nowys).First();
            list.Remove(byys);

            drpSource.DataTextField = "Xmmc";
            drpSource.DataValueField = "Gcbh";
            drpSource.DataSource = list;
            drpSource.DataBind();

            ListItem li = new ListItem();
            li.Text = byys.Xmmc;
            li.Value = byys.Gcbh;
            drpTarget.Items.Add(li);
            
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //保存
        string userCode = Session["userCode"].ToString();
        UserMessage userMgr = new UserMessage(userCode);
        string deptCode = userMgr.GetRootDept().DeptCode;
        string sCode = this.drpSource.SelectedItem.Value;
        string tCode = this.drpTarget.SelectedItem.Value;
        string guid="";
        if (Request.Params["type"]=="add")
        {
            guid = (new GuidHelper()).getNewGuid();
        }
        else
        {
            guid = Request.Params["billCode"];
        }

        List<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
        for (int i = 0; i < myGrid1.Items.Count; i++)
        {
            string str_ysje = ((TextBox)myGrid1.Items[i].FindControl("TextBox1")).Text;
            if (!string.IsNullOrEmpty(str_ysje) && str_ysje!="0")
            {
                string kmCode = myGrid1.Items[i].Cells[0].Text.Split(']')[0].Trim('[');
                decimal zjje = Convert.ToDecimal(str_ysje);
                Bill_Ysmxb sYs = new Bill_Ysmxb();
                sYs.BillCode = guid;
                sYs.Gcbh = sCode;
                sYs.YsDept = deptCode;
                sYs.Ysje = -zjje;
                sYs.Yskm = kmCode;
                sYs.YsType = "3";
                list.Add(sYs);

                Bill_Ysmxb tYs = new Bill_Ysmxb();
                tYs.BillCode = guid;
                tYs.Gcbh = tCode;
                tYs.YsDept = deptCode;
                tYs.Ysje = zjje;
                tYs.Yskm = kmCode;
                tYs.YsType = "3";
                list.Add(tYs);
            }
        }

        Bill_Main main = new Bill_Main();
        main.BillCode = guid;
        main.BillDate = DateTime.Now;
        main.BillDept = deptCode;
        main.BillJe = 0;
        main.BillUser = userCode;
        main.FlowId = "ystz";
        main.StepId = "-1";

        try
        {
            YsManager ysMgr = new YsManager();
            ysMgr.InsertYsmx(list, main);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('ystz.aspx','_self');", true);
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }

    public void bindDataGrid()
    {
        this.Label1.Text = this.drpSource.SelectedItem.Text;
        this.Label2.Text = this.drpTarget.SelectedItem.Text;
        string tzGch = drpSource.SelectedValue;
        string mbGch = drpTarget.SelectedValue;
        string userCode = Session["userCode"].ToString().Trim();
        UserMessage userMgr = new UserMessage(userCode);
        string depCode = userMgr.GetRootDept().DeptCode;


        this.trResult.Style["display"] = "";
        this.trSelect.Style["display"] = "none";
        this.Button2.Visible = false;
        YsManager ysMgr = new YsManager();
        IList<Bill_Ysmxb> list = ysMgr.GetYsmxByDeptYue(tzGch, depCode);
        myGrid1.DataSource = list;
        myGrid1.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        this.bindDataGrid();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystz.aspx");
    }
    protected void myGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            string kmCode = e.Item.Cells[0].Text;
            e.Item.Cells[0].Text = sysMgr.GetYskmNameCode(kmCode);
            string depCode = e.Item.Cells[1].Text;
            e.Item.Cells[1].Text = sysMgr.GetDeptCodeName(depCode);
            string ysje = e.Item.Cells[2].Text;
            YsManager ysMgr = new YsManager();

            e.Item.Cells[3].Text = Convert.ToString(ysMgr.GetYueYs(drpTarget.SelectedValue, depCode, kmCode) - ysMgr.GetYueHf(drpTarget.SelectedValue, depCode, kmCode));
            string gcbh = e.Item.Cells[7].Text;

            decimal sourceYs = Convert.ToDecimal(e.Item.Cells[2].Text);
            e.Item.Cells[2].Text = Convert.ToString(sourceYs - ysMgr.GetYueHf(drpSource.SelectedValue, depCode, kmCode));

            e.Item.Cells[4].Text = Convert.ToString(-ysMgr.GetYueNotEndje(drpSource.SelectedValue, depCode, kmCode));
            e.Item.Cells[5].Text = (Convert.ToDecimal(e.Item.Cells[2].Text) - Convert.ToDecimal(e.Item.Cells[4].Text)).ToString();
        }
    }
}
