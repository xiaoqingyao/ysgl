using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace webBillLibrary
{
    public class bxgl : System.Web.UI.Page
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        public void bindBxmxlx(System.Web.UI.WebControls.DropDownList drp)
        {
            DataSet temp = server.GetDataSet("select * from bill_DataDic where dicType='02'");
            drp.DataTextField = "dicName";
            drp.DataValueField = "dicCode";
            drp.DataSource = temp;
            drp.DataBind();
        }

        public void bindFysq(string lblBillCode,DataGrid myGrid)
        {
            DataSet temp = server.GetDataSet("select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as cglb from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh and b.cgbh in (select sqCode from bill_ybbx_fysq where billCode='" + lblBillCode + "')");
            myGrid.DataSource = temp;
            myGrid.DataBind();
        }

        public bool DeleteFykmMx(string fykm, string billcode)
        {
            if (fykm == "")
                return false;
            else
            {
                fykm = fykm.Substring(0, fykm.Length - 1);
                fykm = "'" + fykm.Replace("'", "','") + "'";

                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                list.Add("delete from bill_ybbxmxb_fykm where mxGuid in (" + fykm + ")");
                list.Add("delete from bill_ybbxmxb_fykm_ft where kmmxGuid in (" + fykm + ")");
                list.Add("delete from bill_ybbxmxb_fykm_dept where kmmxGuid in (" + fykm + ")");
                list.Add("delete from bill_ybbxmxb_hsxm where kmmxGuid in (" + fykm + ")");
                //同时更新bill_main
                list.Add("update bill_main set billje =(select isNull(sum(je),0) from bill_ybbxmxb_fykm where billcode='" + billcode + "')  where flowid='ybbx' and billcode ='" + billcode + "'");

                if (server.ExecuteNonQuerysArray(list) == -1)
                    return false;
                else
                    return true;
            }
        }

        public bool DeleteFyFTMx(string ftmxGuids, string billcode)
        {
            if (ftmxGuids == "")
                return false;
            else
            {
                ftmxGuids = ftmxGuids.Substring(0, ftmxGuids.Length - 1);
                ftmxGuids = "'" + ftmxGuids.Replace("'", "','") + "'";

                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

                list.Add("delete from bill_ybbxmxb_fykm_ft where ftmxGuid in (" + ftmxGuids + ")");
                //更新费用科目
                list.Add("update  bill_ybbxmxb_fykm set je = (select isNull(sum(je),0) from bill_ybbxmxb_fykm_ft where kmmxguid =bill_ybbxmxb_fykm.mxguid  ) where billcode = '" + billcode + "'");
                //更新bill_main
                list.Add("update bill_main set billje =(select isNull(sum(je),0) from bill_ybbxmxb_fykm where billcode='" + billcode + "')  where flowid='ybbx' and billcode ='" + billcode + "'");

                if (server.ExecuteNonQuerysArray(list) == -1)
                    return false;
                else
                    return true;
            }
        }

        public string AddFtMxb(string billCode, string kmmxGuid)
        {
            string ftmxGuid = (new GuidHelper()).getNewGuid();
            DataSet temp = server.GetDataSet("select * from bill_cbzx where zxCode not in (select cbzx from bill_ybbxmxb_fykm_ft where kmmxGuid='" + kmmxGuid + "')");
            if (temp.Tables[0].Rows.Count == 0)
            {
                return "所有成本中心均已分摊！";
            }
            else
            {
                //获取没有使用过的中心编号
                string sql = "insert into bill_ybbxmxb_fykm_ft(billCode,kmmxGuid,cbzx,je,ftmxGuid) select top 1 '" + billCode + "','" + kmmxGuid + "',zxCode,0,'" + ftmxGuid + "' from bill_cbzx where zxCode not in (select cbzx from bill_ybbxmxb_fykm_ft where kmmxGuid='" + kmmxGuid + "')";

                if (server.ExecuteNonQuery(sql) == -1)
                {
                    return "添加失败！";
                }
                else
                {
                    return "";
                }
            }
        }

        public bool UpdateFyftMxb(string[] list, string billcode)
        {
            try
            {
                System.Collections.Generic.List<string> sqls = new System.Collections.Generic.List<string>();

                string ftmxGuid = "";
                for (int i = 0; i <= list.Length - 1; i++)
                {
                    ftmxGuid = list[i].Replace("txtFykm", "");
                    i = i + 1;
                    string ftje = list[i];
                    //fykmJe += double.Parse(ftje);
                    //sqls.Add("update bill_ybbxmxb_fykm_ft set je=" + ftje.ToString() + " where ftmxGuid='" + ftmxGuid + "'");
                    //更新分摊表
                    sqls.Add("update bill_ybbxmxb_fykm set je=" + ftje.ToString() + " where mxGuid='" + ftmxGuid + "'");
                }
                //更新bill_main
                sqls.Add("update bill_main set billje =(select isNull(sum(je+isnull(se,0)),0) from bill_ybbxmxb_fykm where billcode='" + billcode + "')  where flowid='ybbx' and billcode ='" + billcode + "'");
                if (server.ExecuteNonQuerysArray(sqls) == -1)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateFySe(string[] list, string billcode)
        {
            try
            {
                System.Collections.Generic.List<string> sqls = new System.Collections.Generic.List<string>();

                string ftmxGuid = "";
                for (int i = 0; i <= list.Length - 1; i++)
                {
                    ftmxGuid = list[i].Replace("txtHjse", "");
                    i = i + 1;
                    string ftje = list[i];
                    //fykmJe += double.Parse(ftje);
                    //sqls.Add("update bill_ybbxmxb_fykm_ft set je=" + ftje.ToString() + " where ftmxGuid='" + ftmxGuid + "'");
                    //更新分摊表
                    sqls.Add("update bill_ybbxmxb_fykm set se=" + ftje.ToString() + " where mxGuid='" + ftmxGuid + "'");
                }
                //更新bill_main
                sqls.Add("update bill_main set billje =(select isNull(sum(je+isnull(se,0)),0) from bill_ybbxmxb_fykm where billcode='" + billcode + "')  where flowid='ybbx' and billcode ='" + billcode + "'");
                if (server.ExecuteNonQuerysArray(sqls) == -1)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public string UpdateFtmxCbzx(string ftmxGuid, string val)
        {
            //是否已存在非此记录 但是此成本中心的项目
            DataSet temp = server.GetDataSet("select * from bill_ybbxmxb_fykm_ft where kmmxGuid=(select kmmxGuid from bill_ybbxmxb_fykm_ft where ftmxGuid='" + ftmxGuid + "') and cbzx='" + val + "'");
            if (temp.Tables[0].Rows.Count != 0)
                return "此成本中心已存在分摊记录！";

            else
            {
                if (server.ExecuteNonQuery("update bill_ybbxmxb_fykm_ft set cbzx='" + val + "' where ftmxGuid='" + ftmxGuid + "'") == -1)
                    return "修改成本中心失败！";
                else
                    return "";
            }
        }
        public bool DeleteJkmxInfo(string jkdkGuid)
        {
            if (jkdkGuid == "")
                return false;
            else
            {
                jkdkGuid = jkdkGuid.Substring(0, jkdkGuid.Length - 1);
                jkdkGuid = "'" + jkdkGuid.Replace("'", "','") + "'";

                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                list.Add("update bill_ybbxmxb_fydk set status='2' where dkGuid in (" + jkdkGuid + ")");

                if (server.ExecuteNonQuerysArray(list) == -1)
                    return false;
                else
                    return true;
            }
        }
        public string CaluateYtYb(string billCode)
        {
            DataSet temp = server.GetDataSet("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billCode='" + billCode + "' and isnull(status,'1')<>'2'");//总金额
            DataSet temp2 = server.GetDataSet("select isnull(sum(je),0) from bill_fysq_mxb where mxGuid in (select jkmxCode from bill_ybbxmxb_fydk where isnull(status,'1')<>'2' and billCode='" + billCode + "')");

            double je = double.Parse(temp.Tables[0].Rows[0][0].ToString().Trim());
            double dkje = double.Parse(temp2.Tables[0].Rows[0][0].ToString().Trim());
            double cha = je - dkje;
            //这里是计算单位应退和应补 经费科目，抵扣变动时应更新
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            if (cha > 0)
            {
                string sqlYbbxmxb = "update bill_ybbxmxb set ytje='" + Math.Abs(cha).ToString("0.00") + "',ybje='0.00'  where billCode='" + billCode + "'";
                list.Add(sqlYbbxmxb);
                server.ExecuteNonQuerysArray(list);
                return Math.Abs(cha).ToString("0.00") + ",0.00";
            }
            else
            {
                string sqlYbbxmxb = "update bill_ybbxmxb set ytje='0.00',ybje='" + Math.Abs(cha).ToString("0.00") + "'  where billCode='" + billCode + "'";
                list.Add(sqlYbbxmxb);
                server.ExecuteNonQuerysArray(list);
                return "0.00," + Math.Abs(cha).ToString("0.00");
            }

        }

        public string DeleteFysq(string billCode, string sqlCode)
        {
            string status = server.GetCellValue("select status from bill_ybbx_fysq where billCode='" + billCode + "' and sqCode='" + sqlCode + "'");
            if (status == "1")
            {
                return "update bill_ybbx_fysq set status='2' where billCode='" + billCode + "' and sqCode='" + sqlCode + "'";
            }
            else
            {
                return "delete from bill_ybbx_fysq where billCode='" + billCode + "' and sqCode='" + sqlCode + "'";
            }
        }

        public void getFysq_Ybbx(string billCode,DataGrid myGrid)
        {
            DataSet temp = server.GetDataSet("select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as cglb from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh and b.cgbh in (select sqCode from bill_ybbx_fysq where billCode='" + billCode + "')");
            myGrid.DataSource = temp;
            myGrid.DataBind();
        }

        public string getFysk_string(string lblBillCode)
        {
            string tempStr = "";
            DataSet tempJkmx = server.GetDataSet("select mxName,je,(select billDate from bill_main where bill_main.billCode=a.billCode) as rq,a.billCode as billCode,b.dkGuid as dkGuid from bill_fysq_mxb a,bill_ybbxmxb_fydk b where isnull(b.status,'1')<>'2' and a.mxGuid=b.jkmxCode and b.billCode='" + lblBillCode + "'");
        
            for (int i = 0; i <= tempJkmx.Tables[0].Rows.Count - 1; i++)
            {
                tempStr += "<tr class=\"sfjk\" style=\"display:none;\">";
                tempStr += "<td style=\"text-align: center\">";
                tempStr += "<input type=\"checkbox\" id=\"chk\" class=\"jkmxList\" value=\"" + tempJkmx.Tables[0].Rows[i]["dkGuid"].ToString().Trim() + "\" /></td>";
                tempStr += " <td colspan=\"2\" style=\"width: 200px; text-align: center;\">";
                tempStr += tempJkmx.Tables[0].Rows[i]["mxName"].ToString().Trim() + "</td>";
                tempStr += "<td style=\"width: 100px; text-align: right;\">";
                tempStr += double.Parse(tempJkmx.Tables[0].Rows[i]["je"].ToString().Trim()).ToString("0.00") + "</td>";
                tempStr += "<td colspan=\"3\" style=\"text-align: center\">";
                tempStr += DateTime.Parse(tempJkmx.Tables[0].Rows[i]["rq"].ToString().Trim()).ToShortDateString() + "</td>";
                tempStr += "<td colspan=\"2\" style=\"text-align: center\">";
                tempStr += "<a href=# onclick=\"openFysqDetail('" + tempJkmx.Tables[0].Rows[i]["billCode"] + "');\">查看</a></td>";
                tempStr += " </tr>";
            }
            return tempStr;
        }
    }
}
