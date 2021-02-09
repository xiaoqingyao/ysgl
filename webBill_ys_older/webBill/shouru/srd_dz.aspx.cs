using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_shouru_srd_dz : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.txtDateF.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            this.txtDateT.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtzdrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
            string strdeptsql = @"select * from bill_departments where isnull(isgk,'N')='N' ";
            DataTable dtdept = new DataTable();
            dtdept = server.GetDataTable(strdeptsql, null);
            drp_dept.DataSource = dtdept;
            drp_dept.DataValueField = "deptJianma";
            drp_dept.DataTextField = "deptJianma";
            drp_dept.DataBind();
        }
    }
    protected void btn_select_Click(object sender, EventArgs e)
    {
        selectData();
    }
    decimal deheji = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string type = e.Row.Cells[4].Text.Trim();
            type = type.Equals("1") ? "课程" : "物品";
            e.Row.Cells[4].Text = type;
            //合计
            string jine = e.Row.Cells[6].Text.Trim();
            deheji += decimal.Parse(jine);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "合计：";
            e.Row.Cells[6].Text = deheji.ToString("N2");
        }
    }
    /// <summary>
    /// 制单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_makeBill_Click(object sender, EventArgs e)
    {
        ShouRuRel rel = getData();//获取数据
        this.GridView1.DataSource = getData().Data;
        this.GridView1.DataBind();
        string strsqlTemp = @"insert into sr_import_temp_dz
            (id,atype,CampusName,ReceiptNo,UserName,aDate,ItemName,TotalMoney
            ,EmployeeNames,ConfirmUserName,ConfirmTime,dostatus,note1) values ('{0}','{1}'
            ,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')";
        List<string> lstSqls = new List<string>();
        //唯一码
        string id = new GuidHelper().getNewGuid();
        //制单人
        string usercode = Session["usercode"].ToString();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            string atype = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[4].Text.ToString()))
            {
                atype = GridView1.Rows[i].Cells[4].Text.ToString();
            }
            string strCampusName = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[2].Text.ToString()))
            {
                strCampusName = GridView1.Rows[i].Cells[2].Text.ToString();
            }


            string strReceiptNo = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[1].Text.ToString()) && GridView1.Rows[i].Cells[1].Text.ToString() != "&nbsp;")
            {
                strReceiptNo = GridView1.Rows[i].Cells[1].Text.ToString();
            }
            string strUserName = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[3].Text.ToString()) && GridView1.Rows[i].Cells[3].Text.ToString() != "&nbsp;")
            {
                strUserName = GridView1.Rows[i].Cells[3].Text.ToString();

            }
            string straDate = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[0].Text.ToString()) && GridView1.Rows[i].Cells[0].Text.ToString() != "&nbsp;")
            {
                straDate = GridView1.Rows[i].Cells[0].Text.ToString();
            }
            string ItemName = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[5].Text.ToString()) && GridView1.Rows[i].Cells[5].Text.ToString() != "&nbsp;")
            {
                ItemName = GridView1.Rows[i].Cells[5].Text.ToString();
            }
            string strTotalMoney = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[6].Text.ToString()) && GridView1.Rows[i].Cells[6].Text.ToString() != "&nbsp;")
            {
                strTotalMoney = GridView1.Rows[i].Cells[6].Text.ToString();
            }

            string strEmployeeNames = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[7].Text.ToString()) && GridView1.Rows[i].Cells[7].Text.ToString() != "&nbsp;")
            {
                strEmployeeNames = GridView1.Rows[i].Cells[7].Text.ToString();
            }

            string strConfirmUserName = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[8].Text.ToString()) && GridView1.Rows[i].Cells[8].Text.ToString() != "&nbsp;")
            {
                strConfirmUserName = GridView1.Rows[i].Cells[8].Text.ToString();
            }

            string strConfirmTime = "";
            if (!string.IsNullOrEmpty(GridView1.Rows[i].Cells[9].Text.ToString()) && GridView1.Rows[i].Cells[9].Text.ToString() != "&nbsp;")
            {
                strConfirmTime = GridView1.Rows[i].Cells[9].Text.ToString();
            }

            //制单时间
            string strbilldate = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(txtzdrq.Text.ToString()))
            {
                strbilldate = txtzdrq.Text;
            }

            string strsql = string.Format(strsqlTemp, id, atype, strCampusName, strReceiptNo
                , strUserName, straDate, ItemName, strTotalMoney, strEmployeeNames, strConfirmUserName, strConfirmTime, '0', strbilldate);

            lstSqls.Add(strsql);
        }
        int irel = server.ExecuteNonQuerysArray(lstSqls);
        if (irel < 0)
        {
            Response.Write("对不起，在获取数据源的时候出现错误，请联系管理员解决");
            return;
        }
        //调用存储过程
        string strmakesrdsql = @"exec [sr_MakeSrd_dz] '" + id + "'";

        string strrel = server.ExecuteScalar(strmakesrdsql).ToString();
        if (strrel.IndexOf("error") > -1)
        {
            string strErrorMsg = strrel.Substring(6);
            if (strErrorMsg.IndexOf("科目对照关系") > -1)
            {
                strErrorMsg += "<a href='sr_kmdy_dz.aspx' style='color:blue'>点我去设置科目对照关系</a>";
            }
            Response.Write(strErrorMsg);
        }
        if (strrel == "success")
        {
            //验证通过 调用制单的存储过程
            strrel = server.ExecuteScalar("exec pro_makebxd '" + id + "','srd'").ToString();
            Response.Write("单据生成成功，对应单据号为：" + strrel);
            //修改标记
            string strsqlflg = @" update sr_import_temp_dz set dostatus='1' where id='" + id + "'";
            server.ExecuteNonQuery(strsqlflg);
        }
    }
    private void selectData()
    {
        try
        {
            this.GridView1.DataSource = getData().Data;
            this.GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write("对不起，操作失败，原因：" + ex.Message);
        }
    }
    private ShouRuRel getData()
    {
        string strurl = "http://api-data.xiaogj.com/dazhi/GetFeeList?sign=2584239397f66bbf5306a9b56fb4ce49&timestamp=1442560189";
        //string strurl = "http://api-data.xiaogj.com/dazhi/GetFeeList?sign=2584239397f66bbf5306a9b56fb4ce49&timestamp=1442560189&sdate=2015-09-18&edate=2015-09-22";

        string datef = this.txtDateF.ToString();
        string datet = this.txtDateT.ToString();
        if (datef.Equals(""))
        {
            Response.Write("制单时间起不能为空"); return null;
        }
        else if (datef.Equals(""))
        {
            Response.Write("制单时间止不能为空"); return null;
        }


        string queryPara = "";
        //拼接查询参数
        string df = this.txtDateF.Text.Trim();
        if (!df.Equals(""))
        {
            queryPara += string.Format("&sdate={0}", df);
        }
        string dt = this.txtDateT.Text.Trim();
        if (!dt.Equals(""))
        {
            queryPara += string.Format("&edate={0}", dt);
        }
        string CampusName = this.drp_dept.SelectedValue.Trim();
        if (!CampusName.Equals(""))
        {
            queryPara += string.Format("&campus={0}", CampusName);
        }
        // string.Format("{0}{1}", strurl, queryPara);

        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}{1}", strurl, queryPara));
        //接收结果
        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();   //获取响应的字符串流
        StreamReader sr = new StreamReader(stream);
        string rel = sr.ReadToEnd();
        ShouRuRel modelshouru = new ShouRuRel();
        using (StringReader stringreader = new StringReader(rel))
        {
            JsonSerializer serializer = new JsonSerializer();
            modelshouru = (ShouRuRel)serializer.Deserialize(new JsonTextReader(stringreader), typeof(ShouRuRel));
        }
        return modelshouru;

    }
}