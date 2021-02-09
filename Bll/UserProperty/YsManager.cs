using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;
using System.Data.SqlClient;
using System.Data;
using Bll.newysgl;

namespace Bll.UserProperty
{
    public class YsManager
    {
        YsglDal ysDal = new YsglDal();
        YsgcDal ysgcDal = new YsgcDal();

        /// <summary>
        /// 根据时间获得预算金额
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueYs(DateTime dt, string deptCode, string kmCode)
        {
            return ysDal.GetYueYsje(dt, deptCode, kmCode);
        }

        /// <summary>
        /// 根据年份取得预算过程
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IList<Bill_Ysgc> GetYsgcByYear(string year)
        {
            string config = (new SysManager()).GetsysConfigBynd(year)["MonthOrQuarter"];
            return ysgcDal.GetYsgcByYear(year, config);
        }
        /// <summary>
        /// 根据过程编号获得预算金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueYs(string gcbh, string deptCode, string kmCode)
        {
            return ysDal.GetYueYsje(gcbh, deptCode, kmCode);
        }
        /// <summary>
        /// 根据部门求对应部门经费申请预算
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        //public decimal GetYueYsje_dept(string gcbh, string deptCode)
        //{
        //    return ysDal.GetYueYsje_dept(gcbh, deptCode);
        //}

        /// <summary>
        /// 获得预算过程编号+名称
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        public string GetYsgcCodeName(string gcbh)
        {
            Bill_Ysgc ysgc = ysDal.GetYsgcByCode(gcbh);
            return ysgc == null ? "" : "[" + ysgc.Gcbh + "]" + ysgc.Xmmc;
        }
        /// <summary>
        /// 根据时间获得已花费金额
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueHf(DateTime dt, string deptCode, string kmCode)
        {
            return ysDal.GetYueHfje(dt, deptCode, kmCode);
        }
        /// <summary>
        /// 根据过程编号获得已花费金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueHf(string gcbh, string deptCode, string kmCode)
        {
            //生成开始结束时间
            string[] temp = GetYsYearMonth(gcbh);
            string[] yf = temp[2].Split('|');
            DateTime begDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[0]), 1);
            DateTime endDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[yf.Length - 1]), 1).AddMonths(1);
            //返回花费金额
            return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <param name="billdate"></param>
        /// <returns></returns>
        public decimal GetYueHf(string gcbh, string deptCode, string kmCode, string strdydj)
        {
            string strflowid = "";
            DateTime begDate = new DateTime();
            DateTime endDate = new DateTime();
            string strcn = new ConfigBLL().GetValueByKey("CYLX");//是否是财年

            if (!string.IsNullOrEmpty(strcn) && strcn == "Y" && (!string.IsNullOrEmpty(strdydj)))
            {
                string strsql = " select * from bill_ysgc where gcbh='" + gcbh + "'";

                DataTable dttime = new sqlHelper.sqlHelper().GetDataTable(strsql, null);//根据计划过程获取开始结束时间
                if (dttime != null && dttime.Rows.Count > 0)
                {
                    begDate = Convert.ToDateTime(dttime.Rows[0]["kssj"].ToString());
                    endDate = Convert.ToDateTime(dttime.Rows[0]["jzsj"].ToString());
                }
            }
            else
            {
                string[] temp = GetYsYearMonth(gcbh);
                string[] yf = temp[2].Split('|');
                begDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[0]), 1);
                endDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[yf.Length - 1]), 1).AddMonths(1);
            }
            //返回花费金额

            //hfje=new sqlHelper.sqlHelper().GetCellValue("exec dz_hfje '"++"','','',''")
            bool dz_syys_flg = new ConfigBLL().GetValueByKey("dz_syys_flg").Equals("1");//大智剩余预算取值方式flg 大智的取值通过存储过程 dz_hfje
            if (!dz_syys_flg)
            {
                //根据对应单据找出对应决算flowid
                if (!string.IsNullOrEmpty(strdydj))
                {
                    MainDal maindal = new MainDal();
                    strflowid = maindal.getJSFlowId(strdydj);
                    return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode, strflowid);
                }
                else
                {
                    return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode);

                }
            }
            else
            {
                decimal deje = 0;
                ////string nd = "";
                ////string strcnyf = "";
                ////根据过程编号获取年月




                //                // exec [dz_hfje_bxd] '0119','2017','020107','2017-02'


                //                //string strsql_new = @"exec dz_hfje_bxd '" + begDate + "','" + endDate + "','" + deptCode + "','" + kmCode + "'";

                //                try
                //                {
                //                    string strcnyfsql = @"select nian + '-' +right('0'+ yue,2) as cnyf ,*
                //                                        from bill_ysgc  where gcbh='" + gcbh + "' and ysType = 2 ";
                //                    DataTable dtysgc = new sqlHelper.sqlHelper().GetDataTable(strcnyfsql, null);
                //                    if (dtysgc!=null&&dtysgc.Rows.Count>0)
                //                    {
                //                        nd = dtysgc.Rows[0]["nian"].ToString();
                //                        strcnyf = dtysgc.Rows[0]["cnyf"].ToString();
                //                    }

                //                    IList<yskmhf> ysmxb_hf = new Bll.newysgl.bill_ysmxbBll().getkmje(deptCode, nd);
                //                    string strje = ysmxb_hf.Where(p => p.fykm == kmCode && p.cnyf == strcnyf).Sum(p => p.je).ToString();
                //                    if (decimal.TryParse(strje, out deje))
                //                    {
                //                        return deje;
                //                    }
                //                    else
                //                    {
                //                        return 0;
                //                    }

                //                }
                //                catch (Exception)
                //                {

                //                    return 0;
                //                }



                string strsql = @"exec dz_hfje '" + begDate + "','" + endDate + "','" + deptCode + "','" + kmCode + "'";

                try
                {
                    string je = new sqlHelper.sqlHelper().GetCellValue(strsql);
                    if (!string.IsNullOrEmpty(je))
                    {

                        decimal.TryParse(je, out deje);

                    }
                    return deje;
                }
                catch (Exception)
                {

                    return 0;
                }


            }
        }

        /// <summary>
        /// 根据部门求话费金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        //public decimal GetYueHf_dept(string gcbh, string deptCode)
        //{
        //    string strflowid = "";
        //    DateTime begDate = new DateTime();
        //    DateTime endDate = new DateTime();
        //    string strcn = new ConfigBLL().GetValueByKey("CYLX");//是否是财年

        //    if (!string.IsNullOrEmpty(strcn) && strcn == "Y")
        //    {
        //        string strsql = " select * from bill_ysgc where gcbh='" + gcbh + "'";

        //        DataTable dttime = new sqlHelper.sqlHelper().GetDataTable(strsql, null);//根据计划过程获取开始结束时间
        //        if (dttime != null && dttime.Rows.Count > 0)
        //        {
        //            begDate = Convert.ToDateTime(dttime.Rows[0]["kssj"].ToString());
        //            endDate = Convert.ToDateTime(dttime.Rows[0]["jzsj"].ToString());
        //        }
        //    }
        //    else
        //    {
        //        string[] temp = GetYsYearMonth(gcbh);
        //        string[] yf = temp[2].Split('|');
        //        begDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[0]), 1);
        //        endDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[yf.Length - 1]), 1).AddMonths(1);
        //    }
        //    //返回花费金额

        //    string je = new sqlHelper.sqlHelper().GetCellValue("exec dz_deptsyje '" + begDate + "','" + endDate + "','" + deptCode + "'");
        //    decimal deje = 0;
        //    decimal.TryParse(je, out deje);
        //    return deje;

        //}



        public decimal GetYueHf_tf(string gcbh, string deptCode, string kmCode, string strdydj)
        {
            string strflowid = "";
            DateTime begDate = new DateTime();
            DateTime endDate = new DateTime();
            string strcn = new ConfigBLL().GetValueByKey("CYLX");//是否是财年

            if (!string.IsNullOrEmpty(strcn) && strcn == "Y" && (!string.IsNullOrEmpty(strdydj)))
            {
                string strsql = " select * from bill_ysgc where gcbh='" + gcbh + "'";

                DataTable dttime = new sqlHelper.sqlHelper().GetDataTable(strsql, null);//根据计划过程获取开始结束时间
                if (dttime != null && dttime.Rows.Count > 0)
                {
                    begDate = Convert.ToDateTime(dttime.Rows[0]["kssj"].ToString());
                    endDate = Convert.ToDateTime(dttime.Rows[0]["jzsj"].ToString());
                }
            }
            else
            {
                string[] temp = GetYsYearMonth(gcbh);
                string[] yf = temp[2].Split('|');
                begDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[0]), 1);
                endDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[yf.Length - 1]), 1).AddMonths(1);
            }
            //返回花费金额

            //hfje=new sqlHelper.sqlHelper().GetCellValue("exec dz_hfje '"++"','','',''")

            //根据对应单据找出对应决算flowid
            if (!string.IsNullOrEmpty(strdydj))
            {
                MainDal maindal = new MainDal();
                strflowid = "tfsq";// maindal.getJSFlowId(strdydj);
                return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode, strflowid);
            }
            else
            {
                return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode);

            }


        }
        /// <summary>
        /// 取得冻结金额(已提交未审核通过的预算金额)
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueNotEndje(string gcbh, string deptCode, string kmCode)
        {
            return ysDal.GetYueNotEndje(gcbh, deptCode, kmCode);
        }

        /// <summary>
        /// 根据月份，年份获得预算过程编号
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>过程年编号，季度编号，月编号</returns>
        public string GetYsgcCode(DateTime dt)
        {
            string year = dt.Year.ToString();
            int month = dt.Month;
            //判断是否有财务年度的设置，如果有则先转换
            string strcn = new ConfigBLL().GetValueByKey("CYLX");
            if (!string.IsNullOrEmpty(strcn) && strcn == "Y")
            {
                //1.根据申请日期转换成财年日期
                string strsql = " select year_moth from bill_Cnpz where beg_time<= '" + dt.ToString("yyyy-MM-dd") + "' and end_time>= '" + dt.ToString("yyyy-MM-dd") + "'";
                string yearmonth = new sqlHelper.sqlHelper().GetCellValue(strsql);
                if (!string.IsNullOrEmpty(yearmonth))
                {
                    year = yearmonth.Substring(0, 4);
                    month = int.Parse(yearmonth.Substring(5, 2));
                }
                //else
                //{
                //    return "-1";
                //}
            }
            string[] ret = new string[3];
            string temp = "";//月度编号
            string tempjd = "";//季度编号
            switch (month)
            {
                case 1:
                    temp = "0006";
                    tempjd = "0002";
                    break;
                case 2:
                    temp = "0007";
                    tempjd = "0002";
                    break;
                case 3:
                    temp = "0008";
                    tempjd = "0002";
                    break;
                case 4:
                    temp = "0009";
                    tempjd = "0003";
                    break;
                case 5:
                    temp = "0010";
                    tempjd = "0003";
                    break;
                case 6:
                    temp = "0011";
                    tempjd = "0003";
                    break;
                case 7:
                    temp = "0012";
                    tempjd = "0004";
                    break;
                case 8:
                    temp = "0013";
                    tempjd = "0004";
                    break;
                case 9:
                    temp = "0014";
                    tempjd = "0004";
                    break;
                case 10:
                    temp = "0015";
                    tempjd = "0005";
                    break;
                case 11:
                    temp = "0016";
                    tempjd = "0005";
                    break;
                case 12:
                    temp = "0017";
                    tempjd = "0005";
                    break;
            }
            ret[0] = year + "0001";
            ret[1] = year + tempjd;
            ret[2] = year + temp;
            //根据预算到的时间点返回对应的过程编号
            string config = (new SysManager()).GetsysConfigBynd(year)["MonthOrQuarter"];

            switch (config)
            {
                case "0": return ret[0];//0表示预算到年
                case "1": return ret[1];//1表示预算到季度
                case "2": return ret[2];//2表示预算到月度
                default: return ret[2];
            }

        }

        ///// <summary>
        ///// 根据时间返回年月季度预算过程编号
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public string GetYsgcCode(DateTime dt)
        //{
        //    string nd = dt.ToString("yyyy-MM-dd").Substring(0, 4);
        //    string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];
        //    string nowGcbh;
        //    if (config == "0")
        //    {
        //        nowGcbh = GetYsgcCode(Convert.ToString(dt.Year), dt.Month)[0];
        //    }
        //    else if (config == "1")
        //    {
        //        nowGcbh = GetYsgcCode(Convert.ToString(dt.Year), dt.Month)[1];
        //    }
        //    else
        //    {
        //        nowGcbh = GetYsgcCode(Convert.ToString(dt.Year), dt.Month)[2];
        //    }
        //    return nowGcbh;
        //}

        /// <summary>
        /// 根据过程编号获得年,季度,月
        /// </summary>
        /// <param name="YsgcCode"></param>
        /// <returns></returns>
        public string[] GetYsYearMonth(string YsgcCode)
        {


            string[] ret = new string[3];
            ret[0] = YsgcCode.Substring(0, 4);
            string temp = YsgcCode.Substring(4, 4);
            switch (temp)
            {
                case "0001":
                    ret[1] = "1|2|3|4";
                    ret[2] = "01|02|03|04|05|06|07|08|09|10|11|12";
                    break;
                case "0002":
                    ret[1] = "1";
                    ret[2] = "01|02|03";
                    break;
                case "0003":
                    ret[1] = "2";
                    ret[2] = "04|05|06";
                    break;
                case "0004":
                    ret[1] = "3";
                    ret[2] = "07|08|09";
                    break;
                case "0005":
                    ret[1] = "4";
                    ret[2] = "10|11|12";
                    break;
                case "0006":
                    ret[1] = "1";
                    ret[2] = "01";
                    break;
                case "0007":
                    ret[1] = "1";
                    ret[2] = "02";
                    break;
                case "0008":
                    ret[1] = "1";
                    ret[2] = "03";
                    break;
                case "0009":
                    ret[1] = "2";
                    ret[2] = "04";
                    break;
                case "0010":
                    ret[1] = "2";
                    ret[2] = "05";
                    break;
                case "0011":
                    ret[1] = "2";
                    ret[2] = "06";
                    break;
                case "0012":
                    ret[1] = "3";
                    ret[2] = "07";
                    break;
                case "0013":
                    ret[1] = "3";
                    ret[2] = "08";
                    break;
                case "0014":
                    ret[1] = "3";
                    ret[2] = "09";
                    break;
                case "0015":
                    ret[1] = "4";
                    ret[2] = "10";
                    break;
                case "0016":
                    ret[1] = "4";
                    ret[2] = "11";
                    break;
                case "0017":
                    ret[1] = "4";
                    ret[2] = "12";
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 取得月预算
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IList<Bill_Ysgc> GetYueYsByMonth(string year)
        {
            return ysDal.GetYsgcByYear(year, "2");
        }
        /// <summary>
        /// 取得年预算
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IList<Bill_Ysgc> GetNianYs()
        {
            return ysDal.GetYsgcByType("0");
        }

        /// <summary>
        /// 取得季度预算
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IList<Bill_Ysgc> GetJdYsByYear(string year)
        {
            return ysDal.GetYsgcByYear(year, "1");
        }

        /// <summary>
        /// 根据编号获取过程
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Bill_Ysgc GetYsgcByCode(string code)
        {
            return ysDal.GetYsgcByCode(code);
        }

        /// <summary>
        /// 根据过程编号,部门编号取得预算明细
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="depCode"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> GetYsmxByDeptYue(string gcbh, string depCode)
        {
            return ysDal.GetYsmxByDeptYue(gcbh, depCode);
        }
        /// <summary>
        /// 根据科目编号，部门编号获得预算明细
        /// </summary>
        /// <param name="kmCode"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> GetYsmxByKm(string kmCode, string dept)
        {
            //放弃
            string gcbh = "";
            return ysDal.GetYsmxByKm(kmCode, dept, gcbh);
        }

        /// <summary>
        /// 插入预算明细
        /// </summary>
        /// <param name="list"></param>
        /// <param name="main"></param>
        public void InsertYsmx(IList<Bill_Ysmxb> list, Bill_Main main)
        {
            ysDal.InsertYsmx(list, main);
        }
        /// <summary>
        /// 根据单号取得预算明细
        /// </summary>
        /// <param name="billCode"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> GetYsmxByCode(string billCode)
        {
            return ysDal.GetYsmxByCode(billCode);
        }
        /// <summary>
        /// 根据科目编号，得到已经做过预算的预算过程
        /// </summary>
        /// <param name="kmcode"></param>
        /// <returns></returns>
        public Boolean CheckYskm(string kmcode)
        {
            return ysDal.CheckYskm(kmcode);
        }
        /// <summary>
        /// 获取有效的费用提成(提成-报销) getSaleRebateAmount
        /// </summary>
        /// <param name="deptCode">单位code</param>
        /// <param name="kmCode">费用科目code</param>
        /// <returns></returns>
        public decimal getEffectiveSaleRebateAmount(string deptCode, string kmCode)
        {
            Bll.SaleBill.T_SaleFeeAllocationNoteBLL bllSaleFeeAllocationNote = new Bll.SaleBill.T_SaleFeeAllocationNoteBLL();
            Bll.SaleBill.T_SaleFeeSpendNoteBll bllSaleFeeSendNote = new Bll.SaleBill.T_SaleFeeSpendNoteBll();
            decimal deAllocationAmount = bllSaleFeeAllocationNote.getAllocationAmount(deptCode, kmCode);
            decimal deSpendAmount = bllSaleFeeSendNote.getSpendAmount(deptCode, kmCode);
            return deAllocationAmount - deSpendAmount;
        }

        /// <summary>
        /// 部门预算驳回
        /// </summary>
        /// <param name="lstYbbxmxb">集合</param>
        /// <param name="strErrorMsg">错误信息</param>
        /// <returns></returns>
        public int DeptBudgetRevert(string strDept, string Year, string CurrentUserCode, out string strErrorMsg)
        {
            strErrorMsg = "";
            try
            {
                sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
                return server.ExecuteNonQuery("exec DeptBudgetRevert '" + Year + "','" + strDept + "','" + CurrentUserCode + "'");
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message;
                return 0;
            }
        }

        #region 2014-03-17 edit by zyl

        public decimal GetRollSy(string gcbh, string deptCode, string kmCode)
        {
            return GetRollSyYs(gcbh, deptCode, kmCode) - GetRollSyHf(gcbh, deptCode, kmCode);
        }
        /// <summary>
        /// 根据过程编号获得该预算过程之前的花费金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetRollSyHf(string gcbh, string deptCode, string kmCode)
        {
            string[] temp = GetRollYsYearMonth(gcbh);
            if (string.IsNullOrEmpty(temp[1]))
            {
                return 0;
            }

            //生成开始结束时间
            string[] yf = temp[2].Split('|');
            DateTime begDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[0]), 1);
            DateTime endDate = new DateTime(Int32.Parse(temp[0]), Int32.Parse(yf[yf.Length - 1]), 1).AddMonths(1);
            //返回花费金额
            return ysDal.GetYueHfje(begDate, endDate, deptCode, kmCode);
        }

        /// <summary>
        /// 根据过程编号获得该预算过程之前的预算金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetRollSyYs(string gcbh, string deptCode, string kmCode)
        {
            decimal result = 0;
            string[] temp = GetRollYsYearMonth(gcbh);
            if (string.IsNullOrEmpty(temp[1]))
            {
                return result;
            }

            //生成开始结束时间
            string[] yf = temp[2].Split('|');
            int befYf = Int32.Parse(yf[0]);
            int endYf = Int32.Parse(yf[yf.Length - 1]);
            for (int i = befYf; i <= endYf; i++)
            {
                DateTime dt = DateTime.Parse(temp[0] + i + "01");
                result += ysDal.GetYueYsje(dt, deptCode, kmCode);
            }
            //返回预算金额
            return result;
        }
        /// <summary>
        /// 根据过程编号获得回滚的年,季度,月
        /// </summary>
        /// <param name="YsgcCode"></param>
        /// <returns></returns>
        public string[] GetRollYsYearMonth(string YsgcCode)
        {
            string[] ret = new string[3];
            ret[0] = YsgcCode.Substring(0, 4);
            string temp = YsgcCode.Substring(4, 4);
            switch (temp)
            {
                //年
                case "0001":
                //第1季度
                case "0002":
                    ret[1] = "";
                    break;
                //第2季度
                case "0003":
                    ret[1] = "1";
                    ret[2] = "01|03";
                    break;
                //第3季度
                case "0004":
                    ret[1] = "2";
                    ret[2] = "01|06";
                    break;
                //第4季度
                case "0005":
                    ret[1] = "3";
                    ret[2] = "01|09";
                    break;
                //1月
                case "0006":
                    ret[1] = "";
                    break;
                //2月
                case "0007":
                    ret[1] = "1";
                    ret[2] = "01";
                    break;
                //3月
                case "0008":
                    ret[1] = "1";
                    ret[2] = "01|02";
                    break;
                //4月
                case "0009":
                    ret[1] = "1";
                    ret[2] = "01|03";
                    break;
                //5月
                case "0010":
                    ret[1] = "2";
                    ret[2] = "01|04";
                    break;
                //6月
                case "0011":
                    ret[1] = "2";
                    ret[2] = "01|05";
                    break;
                //7月
                case "0012":
                    ret[1] = "2";
                    ret[2] = "01|06";
                    break;
                //8月
                case "0013":
                    ret[1] = "3";
                    ret[2] = "01|07";
                    break;
                //9月
                case "0014":
                    ret[1] = "3";
                    ret[2] = "01|08";
                    break;
                //10月
                case "0015":
                    ret[1] = "3";
                    ret[2] = "01|09";
                    break;
                //11月
                case "0016":
                    ret[1] = "4";
                    ret[2] = "01|10";
                    break;
                //12月
                case "0017":
                    ret[1] = "4";
                    ret[2] = "01|11";
                    break;
            }
            return ret;
        }
        #endregion
    }
}
