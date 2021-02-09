using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using System.Data;
using Dal.UserProperty;
using System.Data.OleDb;
using System.IO;

public partial class webBill_ysglnew_Bmfjjeqr : System.Web.UI.Page
{
    Dal.newysgl.Bmfj dal = new Dal.newysgl.Bmfj();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    DepartmentDal deptDal = new DepartmentDal();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string yskmtype = string.Empty;//预算科目类型  如果传入了该参数  将对填报的科目类型进行限制   对应的传入url键为yskmtype  可以不传  不传默认该部门对应的所有科目
    string strfkxx = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }

        #region 获取URL配置参数
        //预算科目类型
        object objYskmType = Request["yskmtype"];
        if (objYskmType != null)
        {
            yskmtype = objYskmType.ToString();
        }
        #endregion

        string usercode = Session["userCode"].ToString().Trim();
        DataTable dtdept = deptDal.getUsercodeName(Session["userCode"].ToString().Trim());

        strNowDeptCode = dtdept.Rows[0]["deptcode"].ToString();
        strNowDeptName = dtdept.Rows[0]["deptName"].ToString();


        
       
        string strDeptCodes =new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");


        strfkxx = server.GetCellValue(" select avalue from dbo.t_Config where akey='Isbmfkysxx'");//是否允许部门反馈预算信息
        if (!IsPostBack)
        {
            //年度
            string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
            DataTable selectdt = server.GetDataTable(selectndsql, null);
            for (int i = 0; i < selectdt.Rows.Count; i++)
            {
                drpSelectNd.Items.Add(new ListItem(selectdt.Rows[i]["xmmc"].ToString(), selectdt.Rows[i]["nian"].ToString()));
            }
            //1.根据配置项 判断是否预算到末级 
            string strsfmj = server.GetCellValue("select avalue from dbo.t_Config where akey='deptjc'");
            if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")//如果是预算到末级
            {
                string strnd = drpSelectNd.SelectedValue;
                if (!string.IsNullOrEmpty(strnd))
                {
                    dtuserRightDept = deptDal.getRigtusers(strDeptCodes, strNowDeptCode, strsfmj, strnd);
                }
            }
            else
            {
                dtuserRightDept = deptDal.getRigtusers(strDeptCodes, strNowDeptCode);
            }


            //如果启用归口分解 显示提示信息
            bool bo = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
            this.lblTs.Visible = bo;

            #region 绑定人员管理下的部门
            if (!strNowDeptCode.Equals(""))
            {
                //获取人员管理下的部门
                if (strDeptCodes != "")
                {
                    for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                    {
                        ListItem li = new ListItem();
                        li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                        li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                        this.LaDept.Items.Add(li);
                    }
                }
                this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                this.LaDept.Items.Insert(0, new ListItem("--全部--", ""));
                this.LaDept.SelectedIndex = 1;
            }

            #endregion
            //2.读取配置项是否允许部门反馈预算信息
            
            if (!string.IsNullOrEmpty(strfkxx) && strfkxx == "N")//不允许反馈信息
            {
                btn_Submit.Visible = addAttachment.Visible = false;
            }

            Bind();
        }
    }

    private void Bind()
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            strfkxx = server.GetCellValue(" select avalue from dbo.t_Config where akey='Isbmfkysxx'");
            string deptcode = LaDept.SelectedValue.Trim();
            //LaState.Text = dal.GetDeptStateByUserCode(deptcode, drpSelectNd.SelectedValue);
            string strDeptCodes = "";
            if (this.LaDept.Items.Count >= 2)//必须有个全部 有个当前部门
            {
                if (this.LaDept.SelectedValue.Trim() != "")
                {
                    //选择全部
                    strDeptCodes = this.LaDept.SelectedValue.Trim();
                }
                else
                {
                    //选择某个部门的时候
                    int iDaCount = dtuserRightDept.Rows.Count;
                    if (iDaCount > 0)
                    {
                        for (int i = 0; i < iDaCount; i++)
                        {
                            string strEveDeptCode = dtuserRightDept.Rows[i]["deptCode"].ToString();
                            strDeptCodes += ",'";
                            strDeptCodes += strEveDeptCode;
                            strDeptCodes += "'";
                        }
                        //再加上自己的部门
                        strDeptCodes += ",'" + strNowDeptCode + "'";
                        strDeptCodes = strDeptCodes.Substring(1, strDeptCodes.Length - 1);
                    }
                }
                if (strDeptCodes != "")
                {
                    DataTable bmqr = dal.GetListByDept(strDeptCodes, drpSelectNd.SelectedValue, yskmtype);
                    GridView1.DataSource = bmqr;
                    GridView1.DataBind();
                    RowsBound();
                }
            }
        }
    }

    private void RowsBound()
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            //GridView1.Rows[i].Cells[3].Text = Convert.ToDecimal(GridView1.Rows[i].Cells[3].Text).ToString("0.00");
            TextBox txtje = GridView1.Rows[i].FindControl("txtjyje") as TextBox;
            if (Convert.ToDecimal(txtje.Text == "" ? "0" : txtje.Text) == 0)
            {
                txtje.Text = "";
            }
            else
            {
                txtje.Text = Convert.ToDecimal(txtje.Text).ToString("N02");
            }
            GridView1.Rows[i].Cells[0].Text = (i + 1).ToString();

            if (GridView1.Rows[i].Cells[8].Text.Replace("&nbsp;", "").Trim() == "部门确认")
            {
                txtje.ReadOnly = true;
                txtje.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }
    /// <summary>
    /// 确认通过
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        //if (LaState.Text == "部门确认")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('当前状态已经是部门确认无须重复确认！');", true);
        //    return;
        //}LaState.Text != "部门确认" && 

        if (drpSelectNd.Items.Count > 0) 
        {
            string deptcode = LaDept.SelectedValue.Trim();
            IList<bill_ys_xmfjbm> bmlist = new List<bill_ys_xmfjbm>();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                string kmbh = (GridView1.Rows[i].FindControl("Hiddkmbh") as HiddenField).Value;
                string xmbh = (GridView1.Rows[i].FindControl("Hiddxmbh") as HiddenField).Value;
                string kzje = GridView1.Rows[i].Cells[3].Text;
                //string sm = (GridView1.Rows[i].FindControl("txtsm") as TextBox).Text;
                bill_ys_xmfjbm bm = new bill_ys_xmfjbm();
                bm.procode = xmbh;//年度
                bm.kmcode = kmbh;//科目
                bm.by1 = kzje;
                //bm.by2 = sm;
                bm.deptcode = GridView1.Rows[i].Cells[6].Text;
                string strstatus = GridView1.Rows[i].Cells[8].Text.Trim();
                strstatus = strstatus.Replace("&nbsp;", "");
                // if (strstatus == "预算确认")
                //  {
                bmlist.Add(bm);
                // }
            }
            if (dal.InsertBmFjApprove(bmlist))
            {
                //根据传入参数获取flowid
                string flowid = "ys";
                if (!string.IsNullOrEmpty(yskmtype))
                {
                    flowid = new Dal.Bills.MainDal().getFlowId(yskmtype);
                }
                try
                {
                    if (deptcode != "")//选择某一个部门
                    {
                        new Bll.newysgl.YsglMainBll().GetFjtbNdys(drpSelectNd.SelectedValue, deptcode, Session["userCode"].ToString().Trim(), drpSelectNd.SelectedValue + "0001", flowid);
                    }
                    else
                    {
                        //选择全部  比如sdtv一般这样操作
                        new Bll.newysgl.YsglMainBll().GetFjtbNdys(drpSelectNd.SelectedValue, strNowDeptCode, Session["userCode"].ToString().Trim(), drpSelectNd.SelectedValue + "0001", flowid);
                        for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                        {
                            new Bll.newysgl.YsglMainBll().GetFjtbNdys(drpSelectNd.SelectedValue, dtuserRightDept.Rows[i]["deptCode"].ToString(), Session["userCode"].ToString().Trim(), drpSelectNd.SelectedValue + "0001", flowid);
                        }
                    }
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('确认完毕，生成预算数据时出现错误！请联系管理员');", true);
                    return;
                }
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('确认完毕！');", true);
                Bind();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败！请联系管理员！');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败！找不到预算过程和预算未确认的确认数据');", true);
        }
    }


    /// <summary>
    /// 提交建议金额
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        //if (LaState.Text != "部门确认")
        //{
        string deptcode = LaDept.SelectedValue.Trim();
        IList<bill_ys_xmfjbm> bmlist = new List<bill_ys_xmfjbm>();
        bool boCheckFlg = false;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            string kmbh = (GridView1.Rows[i].Cells[4].FindControl("Hiddkmbh") as HiddenField).Value;
            string xmbh = (GridView1.Rows[i].Cells[4].FindControl("Hiddxmbh") as HiddenField).Value;
            string jyje = (GridView1.Rows[i].Cells[4].FindControl("txtjyje") as TextBox).Text;//建议金额 申报金额
            if (string.IsNullOrEmpty(jyje))
            {
                continue;
            }
            boCheckFlg = true;
            string sm = (GridView1.Rows[i].Cells[5].FindControl("txtsm") as TextBox).Text;
            bill_ys_xmfjbm bm = new bill_ys_xmfjbm();
            bm.procode = xmbh;
            bm.kmcode = kmbh;
            bm.by1 = jyje;
            bm.by2 = sm;
            bm.by3 = "3";
            bm.deptcode = deptcode;
            bmlist.Add(bm);
        }
        if (!boCheckFlg)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请直接在需要申报预算金额的栏内填写申报金额再单击申报提交按钮！');", true);
            return;
        }
        if (dal.SaveBmFjApprove(bmlist))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交成功！');", true);
            Bind();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败！请联系管理员！');", true);
        }
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算金额已经确认！');", true);
        //}
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }


    decimal deTotalAmount = 0;
    double deTotalShenSu = 0;//申诉金额
    protected void GridView_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e == null)
        {
            return;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            deTotalAmount = 0;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strEveAmount = e.Row.Cells[3].Text;
            decimal deEveAmount = 0;
            if (decimal.TryParse(strEveAmount, out deEveAmount))
            {
                deTotalAmount += deEveAmount;
            }

            TextBox txtEveShenbao = e.Row.Cells[4].FindControl("txtjyje") as TextBox;
            double dbEveShenbao = 0;
            if (txtEveShenbao != null)
            {
                string streveshenbao = txtEveShenbao.Text.Trim();
                if (double.TryParse(streveshenbao, out dbEveShenbao))
                {
                    deTotalShenSu += dbEveShenbao;
                }
            }

            //附件
            string strfujian = e.Row.Cells[9].Text;
            if (!strfujian.Equals("") && strfujian != "&nbsp;")
            {
                e.Row.Cells[9].Text = "<a href='../../Uploads/bmfjjeqr/" + System.IO.Path.GetFileName(strfujian) + @"' target='_blank'  >下载</a>";
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[3].Text = deTotalAmount.ToString("N02");
            e.Row.Cells[0].Style.Add("text-align", "right");
            e.Row.Cells[3].Style.Add("text-align", "right");

            e.Row.Cells[4].Text = deTotalShenSu.ToString("N02");
            e.Row.Cells[4].Style.Add("text-align", "right");
        }

        //如果不允许反馈信息 在列表页隐藏预算申报金额、说明、附件三个列。
        if (!string.IsNullOrEmpty(strfkxx)&&strfkxx=="N")
        {
            e.Row.Cells[4].CssClass = e.Row.Cells[5].CssClass = e.Row.Cells[9].CssClass = "hiddenbill";
 
        }

    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([序号] VarChar,[部门] VarChar,[预算科目] VarChar,[预算控制金额] VarChar,[预算申报金额] VarChar,[申报说明] text,[预算金额状态] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@xuhao,@dept,@yskm,@je,@sbje,@sbsm,@status)", con))
                {
                    string strxuhao = GridView1.Rows[i].Cells[0].Text.Trim();
                    string dept = GridView1.Rows[i].Cells[1].Text.Trim();
                    string km = GridView1.Rows[i].Cells[2].Text.Trim();
                    string je = GridView1.Rows[i].Cells[3].Text.Trim();
                    string status = GridView1.Rows[i].Cells[8].Text.Trim();
                    string strsbje = "0";
                    TextBox txtsbje = GridView1.Rows[i].Cells[4].FindControl("txtjyje") as TextBox;//申报金额
                    if (txtsbje != null)
                    {
                        strsbje = txtsbje.Text;
                    }

                    string strsbsm = "";
                    TextBox txtsbsm = GridView1.Rows[i].Cells[5].FindControl("txtsm") as TextBox;//申报金额
                    if (txtsbje != null)
                    {
                        strsbsm = txtsbsm.Text;
                    }

                    cmd.Parameters.AddWithValue("@xuhao", strxuhao);
                    cmd.Parameters.AddWithValue("@dept", dept);
                    cmd.Parameters.AddWithValue("@yskm", km);
                    cmd.Parameters.AddWithValue("@je", je);
                    cmd.Parameters.AddWithValue("@sbje", strsbje);
                    cmd.Parameters.AddWithValue("@sbsm", strsbsm);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=bmfjje.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }
}
