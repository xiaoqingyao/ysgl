using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using System.Data;
using System.Configuration;

public partial class webBill_ysgl_YstzDetailNew : System.Web.UI.Page
{
    string strdeptcode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userCode = Convert.ToString(Session["userCode"]);
            YsManager ysMgr = new YsManager();
            SysManager sysMgr = new SysManager();
            string nowGcbh = "";

            if (string.IsNullOrEmpty(userCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                return;
            }
            if (Request.Params["deptcode"] != null && Request.Params["deptcode"].ToString() != "")
            {
                strdeptcode = Request.Params["deptcode"].ToString().Trim();
            }
            

            if (Request.Params["type"] == "add")
            {
                hf_billcode.Value = (new GuidHelper()).getNewGuid();

                
               

                btn_getYs.Visible = true;
                drp_yskm.Visible = true;
                lb_yskm.Visible = false;
                //绑定预算过程txt_source
                //2014-05-16 beg 预算调整时目标预算过程必须是已开启预算
                string strYsgc = "select '['+gcbh+']'+xmmc as gcname,gcbh from bill_ysgc where ysType='2' and nian=(select max(nian) from bill_ysgc)";
                string configVal = server.GetCellValue("select avalue from t_Config where akey='AllowTzUodoYs' ");
                if (!string.IsNullOrEmpty(configVal) && configVal == "1")
                {
                    strYsgc += "  and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)=(select max(nian) from bill_ysgc) and ysdept='" + strdeptcode + "' and ystype='1')";
                }
                strYsgc += " order by gcbh desc";
                DataTable dtysgc = server.GetDataTable(strYsgc, null);
                //2014-05-16 end
                //DataTable dtysgc = server.GetDataTable("select '['+gcbh+']'+xmmc as gcname,gcbh from bill_ysgc where ysType='2' and nian='" + DateTime.Now.Year.ToString() + "'", null);
                this.txt_source.DataSource = dtysgc;
                this.txt_source.DataTextField = "gcname";
                this.txt_source.DataValueField = "gcbh";
                this.txt_source.DataBind();
                try
                {
                    nowGcbh = ysMgr.GetYsgcCode(DateTime.Now);
                    this.txt_source.SelectedValue = nowGcbh;
                }
                catch (Exception)
                {

                }
            }
            else
            {
                if (Request.Params["type"] == "look")
                {
                    btn_save.Visible = false;
                }

                string billCode = Request.Params["billCode"];
                hf_billcode.Value = billCode;
                Bill_Main main = new BillManager().GetMainByCode(billCode);
                userCode = main.BillUser;
                nowGcbh = server.GetCellValue("select top 1 gcbh from bill_ysmxb where billCode='" + billCode + "' and ysje>0 and ystype='3'", null); //ysMgr.GetYsgcCode(main.BillDate.Value);
                //绑定预算过程txt_source
                DataTable dtysgc = server.GetDataTable("select '['+gcbh+']'+xmmc as gcname,gcbh from bill_ysgc where ysType='2' and nian='" + DateTime.Parse(main.BillDate.ToString()).Year + "'", null);
                this.txt_source.DataSource = dtysgc;
                this.txt_source.DataTextField = "gcname";
                this.txt_source.DataValueField = "gcbh";
                this.txt_source.DataBind();
                this.txt_source.SelectedValue = nowGcbh;

                btn_getYs.Visible = false;
                drp_yskm.Visible = false;
                lb_yskm.Visible = true;
                txt_dept.Text = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode=(select billdept from bill_main where billcode='" + billCode + "')", null);
                txt_zy.Text = server.GetCellValue(" select top 1 billName2 from bill_main where billCode ='" + billCode + "'");
                IList<Bill_Ysmxb> ysList = ysMgr.GetYsmxByCode(billCode);

                StringBuilder sb = new StringBuilder();
                StringBuilder sb_km = new StringBuilder();
                foreach (Bill_Ysmxb ysmx in ysList)//循环选择的预算科目 添加到列表
                {
                    if (nowGcbh != ysmx.Gcbh)
                    {

                        string kmname = "";
                        string kmtemp = sysMgr.GetYskmNameCode(ysmx.Yskm);
                        if (sb.ToString().IndexOf(ysmx.Yskm) == -1 || sb.ToString().IndexOf(ysmx.Yskm) == 0)
                        {
                            kmname = sysMgr.GetYskmNameCode(ysmx.Yskm);
                        }

                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append(kmtemp);
                        sb.Append("</td>");

                        string gcmc = ysMgr.GetYsgcCodeName(ysmx.Gcbh);
                        sb.Append("<td>");
                        sb.Append(gcmc);
                        sb.Append("</td>");

                        sb.Append("<td>");
                        decimal ysje = ysMgr.GetYueYs(ysmx.Gcbh, ysmx.YsDept, ysmx.Yskm);
                        sb.Append(ysje.ToString());
                        sb.Append("</td>");

                        sb.Append("<td>");
                        decimal hfje = ysMgr.GetYueHf(ysmx.Gcbh, ysmx.YsDept, ysmx.Yskm);
                        sb.Append(hfje.ToString());
                        sb.Append("</td>");

                        sb.Append("<td>");
                        decimal zyje = -ysMgr.GetYueNotEndje(ysmx.Gcbh, ysmx.YsDept, ysmx.Yskm);
                        sb.Append(zyje.ToString());
                        sb.Append("</td>");

                        sb.Append("<td>");
                        decimal ktzje = ysje - hfje - zyje;
                        sb.Append(ktzje.ToString());
                        sb.Append("</td>");
                        sb.Append("<td><input type=\"text\"  onkeyup='replaceNaN(this);' class=\"ysje\" value=\"" + (-ysmx.Ysje).ToString() + "\" /></td>");

                        sb.Append("</tr>");
                        if (!string.IsNullOrEmpty(kmname))
                        {
                            sb_km.Append(kmname);
                            sb_km.Append(",");
                        }

                    }
                }
                tb_ys.InnerHtml = sb.ToString();
                lb_yskm.Text = sb_km.ToString().Length > 1 ? sb_km.ToString().Substring(0, sb_km.Length - 1) : sb_km.ToString();

            }
            UserMessage userMgr = new UserMessage(userCode);
            Bill_Departments dept = userMgr.GetRootDept();
            if (strdeptcode != "")
            {
                string strdpcode = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode='" + strdeptcode + "'");
                txt_dept.Text = strdpcode.Trim();
            }
            // txt_dept.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
            txt_zdr.Text = "[" + userMgr.Users.UserCode + "]" + userMgr.Users.UserName;

            IList<Bill_Yskm> list = sysMgr.GetYskmByDep(dept.DeptCode);
            sysMgr.SetEndYsbm(list);
            //IList<Bill_Yskm> listend = new List<Bill_Yskm>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (list[i].IsEnd.Equals("1"))
            //    {
            //        listend.Add(list[i]);
            //    }
            //}
            // txt_source.Text = ysMgr.GetYsgcCodeName(nowGcbh);
            foreach (Bill_Yskm item in list)
            {
                drp_yskm.Items.Add(new ListItem("[" + item.YskmCode + "]" + item.YskmMc, item.YskmCode));
            }
            //drp_yskm.DataTextField = "YskmMc";
            //drp_yskm.DataValueField = "YskmCode";
            //drp_yskm.DataSource = list;
            //drp_yskm.DataBind();
        }
    }
}
