using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Collections;
using Dal.newysgl;
using System.Data;

namespace Bll.newysgl
{
    public class YsglMainBll
    {
        /// <summary>
        /// 获取部门年度预算表
        /// </summary>
        /// <param name="deptcode">部门编号</param>
        /// <param name="nd">年度编号</param>
        /// <param name="yskmtype">预算科目类型(01收入预算 02 费用预算)</param>
        /// <param name="tblx">填报类型01部门填报  02财务填报 默认部门填报即可</param>
        /// <param name="ystype">需要获取的预算数的类型 数组中传入1 代表只获取年初预算的 加入2代表加上调整的等等</param>
        /// <returns></returns>

        public IList<Models.YsgcTb> GetMainTable_tf(string deptcode, string nd, string yskmtype, string tblx, string[] ystype, string xmbh, string stepid)
        {
            try
            {
                bill_ysmxbBll ysmxbill = new bill_ysmxbBll();
                IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd); // 获取预算参数设置
                IList<Bill_Yskm> yskm = new Dal.SysDictionary.YskmDal().GetYskmByDeptTblx_tf(deptcode, tblx, nd, yskmtype);      //  获取预算科目   
                IList<Bill_Ysmxb> ysmxb = new Bll.newysgl.bill_ysmxbBll().GetYsmxByNian(nd, deptcode, xmbh, stepid);          //  获取预算明细表


                YsgcTb gcbh = GetgcbhByNd(nd);                                                                  // 将预算过程编号保存在一个model中
                IList<YsgcTb> Tbmain = new List<YsgcTb>();
                foreach (var i in yskm)                                                                         //将预算科目编号保存进去
                {
                    #region 预算科目中的数据添加到数据表中

                    YsgcTb ys = new YsgcTb();
                    ys.km = i.YskmMc;
                    ys.kmbh = i.YskmCode;
                    ys.iszyys = i.iszyys;
                    #region 年预算

                    if (ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                    {
                        ys.year = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                        ys.yearYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                    }

                    #endregion

                    #region 季度预算

                    if (sysConfig["MonthOrQuarter"] == "1")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0) //一月
                        {
                            ys.spring = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.springYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.summer = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.summerYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.autumn = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.autumnYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.winter = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.winterYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                    }
                    #endregion

                    #region 月度预算

                    if (sysConfig["MonthOrQuarter"] == "2")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.January = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JanuaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //ys.Januarybxje = ysmxbill.getbxje(nd, nd + "-01", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.February = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.FebruaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Februarybxje = ysmxbill.getbxje(nd, nd + "-02", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.march = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.marchYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.marchbxje = ysmxbill.getbxje(nd, nd + "-03", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.April = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AprilYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-04", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.May = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.MayYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Maybxje = ysmxbill.getbxje(nd, nd + "-05", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.June = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JuneYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Junebxje = ysmxbill.getbxje(nd, nd + "-06", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.July = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JulyYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Julybxje = ysmxbill.getbxje(nd, nd + "-07", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.August = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AugustYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-08", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.September = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.SeptemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Septemberbxje = ysmxbill.getbxje(nd, nd + "-09", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.October = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.OctoberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //ys.Octoberbxje = ysmxbill.getbxje(nd, nd + "-10", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.November = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.NovemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Novemberbxje = ysmxbill.getbxje(nd, nd + "-11", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.December = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.DecemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Decemberbxje = ysmxbill.getbxje(nd, nd + "-12", i.YskmCode, deptcode);
                        }
                    }
                    #endregion

                    Tbmain.Add(ys);
                    #endregion
                }
                return Tbmain;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }


        /// <summary>
        /// 获取部门年度预算表
        /// </summary>
        /// <param name="deptcode">部门编号</param>
        /// <param name="nd">年度编号</param>
        /// <param name="yskmtype">预算科目类型(01收入预算 02 费用预算)</param>
        /// <param name="tblx">填报类型01部门填报  02财务填报 默认部门填报即可</param>
        /// <param name="ystype">需要获取的预算数的类型 数组中传入1 代表只获取年初预算的 加入2代表加上调整的等等</param>
        /// <returns></returns>

        public IList<Models.YsgcTb> GetMainTable(string deptcode, string nd, string yskmtype, string tblx, string[] ystype, string xmbh, string stepid)
        {
            try
            {
                bill_ysmxbBll ysmxbill = new bill_ysmxbBll();
                IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd); // 获取预算参数设置
                IList<Bill_Yskm> yskm = new Dal.SysDictionary.YskmDal().GetYskmByDeptTblx(deptcode, tblx, nd, yskmtype);      //  获取预算科目   
                IList<Bill_Ysmxb> ysmxb = new Bll.newysgl.bill_ysmxbBll().GetYsmxByNian(nd, deptcode, xmbh, stepid);          //  获取预算明细表
                IList<yskmhf> ysmxb_hf = new Bll.newysgl.bill_ysmxbBll().getkmje(deptcode, nd);
                YsgcTb gcbh = GetgcbhByNd(nd);                                                                  // 将预算过程编号保存在一个model中
                IList<YsgcTb> Tbmain = new List<YsgcTb>();
                foreach (var i in yskm)                                                                         //将预算科目编号保存进去
                {
                    #region 预算科目中的数据添加到数据表中

                    YsgcTb ys = new YsgcTb();
                    ys.km = i.YskmMc;
                    ys.kmbh = i.YskmCode;
                    ys.iszyys = i.iszyys;
                    #region 年预算

                    if (ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                    {
                        ys.year = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                        ys.yearYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                    }

                    #endregion

                    #region 季度预算

                    if (sysConfig["MonthOrQuarter"] == "1")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0) //一月
                        {
                            ys.spring = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.springYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.summer = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.summerYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.autumn = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.autumnYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.winter = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.winterYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                    }
                    #endregion

                    #region 月度预算

                    if (sysConfig["MonthOrQuarter"] == "2")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.January = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JanuaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            ys.Januarybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-01").Sum(p => p.je).ToString();
                            // ys.Januarybxje = ysmxbill.getbxje(nd, nd + "-01", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.February = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.FebruaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Februarybxje = ysmxbill.getbxje(nd, nd + "-02", i.YskmCode, deptcode);
                            ys.Februarybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-02").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.march = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.marchYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.marchbxje = ysmxbill.getbxje(nd, nd + "-03", i.YskmCode, deptcode);
                            ys.marchbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-03").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.April = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AprilYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-04", i.YskmCode, deptcode);
                            ys.Augustbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-04").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.May = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.MayYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Maybxje = ysmxbill.getbxje(nd, nd + "-05", i.YskmCode, deptcode);
                            ys.Maybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-05").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.June = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JuneYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Junebxje = ysmxbill.getbxje(nd, nd + "-06", i.YskmCode, deptcode);
                            ys.Junebxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-06").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.July = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JulyYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Julybxje = ysmxbill.getbxje(nd, nd + "-07", i.YskmCode, deptcode);
                            ys.Julybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-07").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.August = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AugustYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-08", i.YskmCode, deptcode);
                            ys.Augustbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-08").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.September = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.SeptemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Septemberbxje = ysmxbill.getbxje(nd, nd + "-09", i.YskmCode, deptcode);
                            ys.Septemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-09").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.October = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.OctoberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Octoberbxje = ysmxbill.getbxje(nd, nd + "-10", i.YskmCode, deptcode);
                            ys.Octoberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-10").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.November = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.NovemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //ys.Novemberbxje = ysmxbill.getbxje(nd, nd + "-11", i.YskmCode, deptcode);
                            ys.Novemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-11").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.December = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.DecemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Decemberbxje = ysmxbill.getbxje(nd, nd + "-12", i.YskmCode, deptcode);
                            ys.Decemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-12").Sum(p => p.je).ToString();
                        }
                    }
                    #endregion

                    Tbmain.Add(ys);
                    #endregion
                }
                return Tbmain;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }
        public IList<Models.YsgcTb> GetMainTable_qt(string deptcode, string nd, string yskmtype, string tblx, string[] ystype, string xmbh, string stepid)
        {
            try
            {
                bill_ysmxbBll ysmxbill = new bill_ysmxbBll();
                IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd); // 获取预算参数设置
                IList<Bill_Yskm> yskm = new Dal.SysDictionary.YskmDal().GetYskmByDeptTblx(deptcode, tblx, nd, yskmtype);      //  获取预算科目   
                IList<Bill_Ysmxb> ysmxb = new Bll.newysgl.bill_ysmxbBll().GetYsmxByNian(nd, deptcode, xmbh, stepid);          //  获取预算明细表
                IList<yskmhf> ysmxb_hf = new Bll.newysgl.bill_ysmxbBll().getkmje(deptcode, nd);



                YsgcTb gcbh = GetgcbhByNd(nd);                                                                  // 将预算过程编号保存在一个model中
                IList<YsgcTb> Tbmain = new List<YsgcTb>();
                foreach (var i in yskm)                                                                         //将预算科目编号保存进去
                {
                    #region 预算科目中的数据添加到数据表中

                    YsgcTb ys = new YsgcTb();
                    ys.km = i.YskmMc;
                    ys.kmbh = i.YskmCode;
                    ys.iszyys = i.iszyys;
                    #region 年预算

                    if (ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                    {
                        ys.year = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                        ys.yearYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.year && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                    }

                    #endregion

                    #region 季度预算

                    if (sysConfig["MonthOrQuarter"] == "1")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0) //一月
                        {
                            ys.spring = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.springYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.spring && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.summer = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.summerYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.summer && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.autumn = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.autumnYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.autumn && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.winter = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.winterYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.winter && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                        }
                    }
                    #endregion

                    #region 月度预算

                    if (sysConfig["MonthOrQuarter"] == "2")
                    {
                        if (ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.January = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JanuaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.January && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            ys.Januarybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-01").Sum(p => p.je).ToString();
                            // ys.Januarybxje = ysmxbill.getbxje(nd, nd + "-01", i.YskmCode, deptcode);
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.February = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.FebruaryYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.February && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Februarybxje = ysmxbill.getbxje(nd, nd + "-02", i.YskmCode, deptcode);
                            ys.Februarybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-02").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.march = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.marchYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.march && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.marchbxje = ysmxbill.getbxje(nd, nd + "-03", i.YskmCode, deptcode);
                            ys.marchbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-03").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.April = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AprilYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.April && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-04", i.YskmCode, deptcode);
                            ys.Augustbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-04").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.May = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.MayYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.May && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Maybxje = ysmxbill.getbxje(nd, nd + "-05", i.YskmCode, deptcode);
                            ys.Maybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-05").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.June = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JuneYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.June && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Junebxje = ysmxbill.getbxje(nd, nd + "-06", i.YskmCode, deptcode);
                            ys.Junebxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-06").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.July = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.JulyYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.July && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Julybxje = ysmxbill.getbxje(nd, nd + "-07", i.YskmCode, deptcode);
                            ys.Julybxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-07").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.August = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.AugustYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.August && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Augustbxje = ysmxbill.getbxje(nd, nd + "-08", i.YskmCode, deptcode);
                            ys.Augustbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-08").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.September = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.SeptemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.September && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Septemberbxje = ysmxbill.getbxje(nd, nd + "-09", i.YskmCode, deptcode);
                            ys.Septemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-09").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.October = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.OctoberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.October && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            // ys.Octoberbxje = ysmxbill.getbxje(nd, nd + "-10", i.YskmCode, deptcode);
                            ys.Octoberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-10").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.November = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.NovemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.November && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //ys.Novemberbxje = ysmxbill.getbxje(nd, nd + "-11", i.YskmCode, deptcode);
                            ys.Novemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-11").Sum(p => p.je).ToString();
                        }
                        if (ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Count() > 0)
                        {
                            ys.December = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && ystype.Contains(p.YsType)).Sum(p => p.Ysje).ToString();
                            ys.DecemberYsnZj = ysmxb.Where(p => p.Gcbh == gcbh.December && p.Yskm == i.YskmCode && p.YsType == "5").Sum(p => p.Ysje).ToString();
                            //  ys.Decemberbxje = ysmxbill.getbxje(nd, nd + "-12", i.YskmCode, deptcode);
                            ys.Decemberbxje = ysmxb_hf.Where(p => p.fykm == i.YskmCode && p.cnyf == nd + "-12").Sum(p => p.je).ToString();
                        }
                    }
                    #endregion

                    Tbmain.Add(ys);
                    #endregion
                }
                return Tbmain;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }
        /// <summary>
        /// 填写预算
        /// </summary>
        /// <param name="TbMain"></param>
        /// <param name="deptcode"></param>
        /// <param name="tblx"></param>
        /// <param name="nd"></param>
        /// <param name="usercode"></param>
        /// <param name="billstatus">end or -1</param>
        /// <returns></returns>
        public bool Addtb(IList<YsgcTb> TbMain, string deptcode, string tblx, string nd, string usercode, string billstatus, string flowid, string strxmbh)
        {
            string xmcode = "";
            if (!string.IsNullOrEmpty(strxmbh))
            {
                xmcode = strxmbh;
            }
            string strystype = "1";
            if (flowid == "xmys")
            {
                strystype = "8";
            }
            billstatus = billstatus.Equals("") ? "-1" : billstatus;
            IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
            YsgcTb gcbh = GetgcbhByNd(nd); //储存的每个月的过程编号
            IList<Bill_Ysmxb> ysmxb = new List<Bill_Ysmxb>();
            IList<Bill_Main> MainList = new List<Bill_Main>();
            Bll.Bills.BillMainBLL mainbll = new Bll.Bills.BillMainBLL();
            YsgcTb BillMainCode = new YsgcTb();
            #region 如果bill_mian中的有数据 将表中的数据读出来 放到BillMainCode中 并且将需要删除的YSMX中的记录下来
            //如果没有则新增一天添加到BillMainCode中

            string strdeptzd = new Bll.UserProperty.UserMessage(usercode).GetRootDept().DeptCode;//制单部门 归口部门
            if (billstatus.Equals("end"))//如果是end  认为是归口分解
            {
                BillMainCode.year = AddMain(ref MainList, mainbll.GetBillcode(gcbh.year, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.year, usercode, billstatus, flowid, xmcode);
            }
            else
            {
                if (!string.IsNullOrEmpty(xmcode))
                {
                    BillMainCode.year = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.year, deptcode, flowid, xmcode), deptcode, gcbh.year, usercode, billstatus, flowid, xmcode);

                }
                else
                {
                    BillMainCode.year = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.year, deptcode, flowid), deptcode, gcbh.year, usercode, billstatus, flowid, xmcode);

                }
            }


            if (sysConfig["MonthOrQuarter"].ToString() == "1")
            {
                if (billstatus.Equals("end"))//如果是end  认为是归口分解
                {
                    BillMainCode.spring = AddMain(ref MainList, mainbll.GetBillcode(gcbh.spring, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.spring, usercode, billstatus, flowid, xmcode);
                    BillMainCode.summer = AddMain(ref MainList, mainbll.GetBillcode(gcbh.summer, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.summer, usercode, billstatus, flowid, xmcode);
                    BillMainCode.autumn = AddMain(ref MainList, mainbll.GetBillcode(gcbh.autumn, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.autumn, usercode, billstatus, flowid, xmcode);
                    BillMainCode.winter = AddMain(ref MainList, mainbll.GetBillcode(gcbh.winter, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.winter, usercode, billstatus, flowid, xmcode);
                }
                else
                {
                    if (!string.IsNullOrEmpty(xmcode))
                    {
                        BillMainCode.spring = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.spring, deptcode, flowid, xmcode), deptcode, gcbh.spring, usercode, billstatus, flowid, xmcode);
                        BillMainCode.summer = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.summer, deptcode, flowid, xmcode), deptcode, gcbh.summer, usercode, billstatus, flowid, xmcode);
                        BillMainCode.autumn = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.autumn, deptcode, flowid, xmcode), deptcode, gcbh.autumn, usercode, billstatus, flowid, xmcode);
                        BillMainCode.winter = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.winter, deptcode, flowid, xmcode), deptcode, gcbh.winter, usercode, billstatus, flowid, xmcode);
                    }
                    else
                    {
                        BillMainCode.spring = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.spring, deptcode, flowid), deptcode, gcbh.spring, usercode, billstatus, flowid, xmcode);
                        BillMainCode.summer = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.summer, deptcode, flowid), deptcode, gcbh.summer, usercode, billstatus, flowid, xmcode);
                        BillMainCode.autumn = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.autumn, deptcode, flowid), deptcode, gcbh.autumn, usercode, billstatus, flowid, xmcode);
                        BillMainCode.winter = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.winter, deptcode, flowid), deptcode, gcbh.winter, usercode, billstatus, flowid, xmcode);
                    }

                }

            }
            if (sysConfig["MonthOrQuarter"].ToString() == "2")
            {
                if (billstatus.Equals("end"))//如果是end  认为是归口分解 shiliyiyuan
                {
                    BillMainCode.January = AddMain(ref MainList, mainbll.GetBillcode(gcbh.January, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.January, usercode, billstatus, flowid, xmcode);
                    BillMainCode.February = AddMain(ref MainList, mainbll.GetBillcode(gcbh.February, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.February, usercode, billstatus, flowid, xmcode);
                    BillMainCode.march = AddMain(ref MainList, mainbll.GetBillcode(gcbh.march, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.march, usercode, billstatus, flowid, xmcode);
                    BillMainCode.April = AddMain(ref MainList, mainbll.GetBillcode(gcbh.April, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.April, usercode, billstatus, flowid, xmcode);
                    BillMainCode.May = AddMain(ref MainList, mainbll.GetBillcode(gcbh.May, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.May, usercode, billstatus, flowid, xmcode);
                    BillMainCode.June = AddMain(ref MainList, mainbll.GetBillcode(gcbh.June, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.June, usercode, billstatus, flowid, xmcode);
                    BillMainCode.July = AddMain(ref MainList, mainbll.GetBillcode(gcbh.July, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.July, usercode, billstatus, flowid, xmcode);
                    BillMainCode.August = AddMain(ref MainList, mainbll.GetBillcode(gcbh.August, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.August, usercode, billstatus, flowid, xmcode);
                    BillMainCode.September = AddMain(ref MainList, mainbll.GetBillcode(gcbh.September, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.September, usercode, billstatus, flowid, xmcode);
                    BillMainCode.October = AddMain(ref MainList, mainbll.GetBillcode(gcbh.October, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.October, usercode, billstatus, flowid, xmcode);
                    BillMainCode.November = AddMain(ref MainList, mainbll.GetBillcode(gcbh.November, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.November, usercode, billstatus, flowid, xmcode);
                    BillMainCode.December = AddMain(ref MainList, mainbll.GetBillcode(gcbh.December, deptcode, strdeptzd, TbMain[0].kmbh, flowid), deptcode, gcbh.December, usercode, billstatus, flowid, xmcode);
                }
                else
                {
                    if (!string.IsNullOrEmpty(xmcode))
                    {
                        BillMainCode.January = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.January, deptcode, flowid, xmcode), deptcode, gcbh.January, usercode, billstatus, flowid, xmcode);
                        BillMainCode.February = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.February, deptcode, flowid, xmcode), deptcode, gcbh.February, usercode, billstatus, flowid, xmcode);
                        BillMainCode.march = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.march, deptcode, flowid, xmcode), deptcode, gcbh.march, usercode, billstatus, flowid, xmcode);
                        BillMainCode.April = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.April, deptcode, flowid, xmcode), deptcode, gcbh.April, usercode, billstatus, flowid, xmcode);
                        BillMainCode.May = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.May, deptcode, flowid, xmcode), deptcode, gcbh.May, usercode, billstatus, flowid, xmcode);
                        BillMainCode.June = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.June, deptcode, flowid, xmcode), deptcode, gcbh.June, usercode, billstatus, flowid, xmcode);
                        BillMainCode.July = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.July, deptcode, flowid, xmcode), deptcode, gcbh.July, usercode, billstatus, flowid, xmcode);
                        BillMainCode.August = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.August, deptcode, flowid, xmcode), deptcode, gcbh.August, usercode, billstatus, flowid, xmcode);
                        BillMainCode.September = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.September, deptcode, flowid, xmcode), deptcode, gcbh.September, usercode, billstatus, flowid, xmcode);
                        BillMainCode.October = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.October, deptcode, flowid, xmcode), deptcode, gcbh.October, usercode, billstatus, flowid, xmcode);
                        BillMainCode.November = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.November, deptcode, flowid, xmcode), deptcode, gcbh.November, usercode, billstatus, flowid, xmcode);
                        BillMainCode.December = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.December, deptcode, flowid, xmcode), deptcode, gcbh.December, usercode, billstatus, flowid, xmcode);

                    }
                    else
                    {
                        BillMainCode.January = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.January, deptcode, flowid), deptcode, gcbh.January, usercode, billstatus, flowid, xmcode);
                        BillMainCode.February = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.February, deptcode, flowid), deptcode, gcbh.February, usercode, billstatus, flowid, xmcode);
                        BillMainCode.march = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.march, deptcode, flowid), deptcode, gcbh.march, usercode, billstatus, flowid, xmcode);
                        BillMainCode.April = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.April, deptcode, flowid), deptcode, gcbh.April, usercode, billstatus, flowid, xmcode);
                        BillMainCode.May = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.May, deptcode, flowid), deptcode, gcbh.May, usercode, billstatus, flowid, xmcode);
                        BillMainCode.June = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.June, deptcode, flowid), deptcode, gcbh.June, usercode, billstatus, flowid, xmcode);
                        BillMainCode.July = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.July, deptcode, flowid), deptcode, gcbh.July, usercode, billstatus, flowid, xmcode);
                        BillMainCode.August = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.August, deptcode, flowid), deptcode, gcbh.August, usercode, billstatus, flowid, xmcode);
                        BillMainCode.September = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.September, deptcode, flowid), deptcode, gcbh.September, usercode, billstatus, flowid, xmcode);
                        BillMainCode.October = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.October, deptcode, flowid), deptcode, gcbh.October, usercode, billstatus, flowid, xmcode);
                        BillMainCode.November = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.November, deptcode, flowid), deptcode, gcbh.November, usercode, billstatus, flowid, xmcode);
                        BillMainCode.December = AddMain(ref MainList, mainbll.GetBillcodeByDeptAndYsgc(gcbh.December, deptcode, flowid), deptcode, gcbh.December, usercode, billstatus, flowid, xmcode);

                    }

                }
            }
            #endregion

            foreach (var i in TbMain)
            {
                if (i.year != "")
                {
                    Bill_Ysmxb ysmx = new Bill_Ysmxb();
                    ysmx.BillCode = BillMainCode.year;
                    ysmx.Gcbh = gcbh.year;
                    ysmx.Yskm = i.kmbh;
                    ysmx.Ysje = Convert.ToDecimal(i.year);
                    ysmx.YsDept = deptcode;
                    ysmx.YsType = strystype;
                    ysmxb.Add(ysmx);
                }
                if (sysConfig["MonthOrQuarter"].ToString() == "1")
                {
                    #region 添加每一个季度的详细科目
                    if (i.spring != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.spring;
                        ysmx.Gcbh = gcbh.spring;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.spring);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.summer != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.summer;
                        ysmx.Gcbh = gcbh.summer;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.summer);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.autumn != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.autumn;
                        ysmx.Gcbh = gcbh.autumn;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.autumn);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.winter != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.winter;
                        ysmx.Gcbh = gcbh.winter;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.winter);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    #endregion

                }
                if (sysConfig["MonthOrQuarter"].ToString() == "2")
                {
                    #region 添加每一个月度的详细科目


                    if (i.January != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.January;
                        ysmx.Gcbh = gcbh.January;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.January);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }

                    if (i.February != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.February;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.February);
                        ysmx.Gcbh = gcbh.February;
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.march != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.march;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.march);
                        ysmx.YsDept = deptcode;
                        ysmx.Gcbh = gcbh.march;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.April != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.April;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.April);
                        ysmx.Gcbh = gcbh.April;
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.May != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.May;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.May);
                        ysmx.Gcbh = gcbh.May;
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.June != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.June;
                        ysmx.Gcbh = gcbh.June;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.June);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.July != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.July;
                        ysmx.Gcbh = gcbh.July;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.July);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.August != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.August;
                        ysmx.Gcbh = gcbh.August;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.August);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.September != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.September;
                        ysmx.Gcbh = gcbh.September;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.September);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.October != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.October;
                        ysmx.Gcbh = gcbh.October;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.October);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.November != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.November;
                        ysmx.Gcbh = gcbh.November;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.November);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    if (i.December != "")
                    {
                        Bill_Ysmxb ysmx = new Bill_Ysmxb();
                        ysmx.BillCode = BillMainCode.December;
                        ysmx.Gcbh = gcbh.December;
                        ysmx.Yskm = i.kmbh;
                        ysmx.Ysje = Convert.ToDecimal(i.December);
                        ysmx.YsDept = deptcode;
                        ysmx.YsType = strystype;
                        ysmxb.Add(ysmx);
                    }
                    #endregion
                }
            }
            return new Dal.Bills.MainDal().Addtb(ysmxb, MainList, tblx);
        }

        //根据年度把过程编号保存到MOdel中
        public YsgcTb GetgcbhByNd(string nd)
        {
            try
            {
                IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
                IList<Bill_Ysgc> ysgclist = new Dal.Bills.YsgcDal().GetYsgcByNian(nd);
                YsgcTb gcbh = new YsgcTb();

                //年度
                //=======================
                gcbh.year = ysgclist.Where(p => p.YsType == "0").First().Gcbh;
                if (sysConfig["MonthOrQuarter"] == "1")
                {
                    //季节
                    //=======================
                    gcbh.spring = ysgclist.Where(p => p.Yue == "一" && p.YsType == "1").First().Gcbh;
                    gcbh.summer = ysgclist.Where(p => p.Yue == "二" && p.YsType == "1").First().Gcbh;
                    gcbh.autumn = ysgclist.Where(p => p.Yue == "三" && p.YsType == "1").First().Gcbh;
                    gcbh.winter = ysgclist.Where(p => p.Yue == "四" && p.YsType == "1").First().Gcbh;
                }
                if (sysConfig["MonthOrQuarter"] == "2")
                {
                    //先将预算过程编号记录下来
                    //=======================
                    gcbh.January = ysgclist.Where(p => p.Yue == "1" && p.YsType == "2").First().Gcbh;
                    gcbh.February = ysgclist.Where(p => p.Yue == "2" && p.YsType == "2").First().Gcbh;
                    gcbh.march = ysgclist.Where(p => p.Yue == "3" && p.YsType == "2").First().Gcbh;
                    gcbh.April = ysgclist.Where(p => p.Yue == "4" && p.YsType == "2").First().Gcbh;
                    gcbh.May = ysgclist.Where(p => p.Yue == "5" && p.YsType == "2").First().Gcbh;
                    gcbh.June = ysgclist.Where(p => p.Yue == "6" && p.YsType == "2").First().Gcbh;
                    gcbh.July = ysgclist.Where(p => p.Yue == "7" && p.YsType == "2").First().Gcbh;
                    gcbh.August = ysgclist.Where(p => p.Yue == "8" && p.YsType == "2").First().Gcbh;
                    gcbh.September = ysgclist.Where(p => p.Yue == "9" && p.YsType == "2").First().Gcbh;
                    gcbh.October = ysgclist.Where(p => p.Yue == "10" && p.YsType == "2").First().Gcbh;
                    gcbh.November = ysgclist.Where(p => p.Yue == "11" && p.YsType == "2").First().Gcbh;
                    gcbh.December = ysgclist.Where(p => p.Yue == "12" && p.YsType == "2").First().Gcbh;
                }
                return gcbh;
            }
            catch { throw new Exception("预算参数设置不正确或者缺少必要的预算过程"); }
        }
        //把需要添加到bill_main中和需要删除预算明细中的记录下来 区分开
        public string AddMain(ref IList<Bill_Main> mainlist, string bh, string deptcode, string gcbh, string usercode, string billstatus, string flowid, string xmbh)
        {

            if (bh != "")
            {
                Bill_Main main = new Bill_Main();
                main.BillCode = bh;
                main.BillName = gcbh;
                main.FlowId = "DELETE";  //  创建这个实体类仅仅是为了做一个标记 删除明细表用的  看Dal中  FlowId为 “DELETE”的过滤掉了
                main.StepId = "-1";
                main.BillUser = usercode;
                main.BillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                main.BillDept = deptcode;
                main.BillJe = 0;
                main.LoopTimes = 1;
                main.BillType = "1";
                main.Note3 = xmbh;//项目编号
                mainlist.Add(main);
                return bh;
            }
            else
            {
                Bill_Main main = new Bill_Main();
                main.BillCode = System.Guid.NewGuid().ToString().ToUpper();
                main.BillName = gcbh;
                main.FlowId = flowid;//"ys";
                main.StepId = billstatus.Equals("") ? "-1" : billstatus;
                main.BillUser = usercode;
                main.BillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                main.BillDept = deptcode;
                main.BillJe = 0;
                main.LoopTimes = 1;
                main.BillType = "1";
                main.Note3 = xmbh;//项目编号
                string strdeptzd = new Bll.UserProperty.UserMessage(usercode).GetRootDept().DeptCode;//制单部门 归口部门
                main.GkDept = strdeptzd;
                mainlist.Add(main);
                return main.BillCode;
            }
        }

        /// <summary>
        /// 通过部门分解金额确认 生成预算
        /// </summary>
        private void CreateYsByBmfj(string nd, string deptcode, string usercode, string ndgcbh, string flowid)
        {
            string billcode = System.Guid.NewGuid().ToString().ToUpper();
            IList<Bill_Ysmxb> mxlist = new Dal.newysgl.bill_ysmxbDal().InsetNdysmx(nd, deptcode, ndgcbh, billcode); //数据添加到预算明细表中  返回一个预算明细表
            decimal je = mxlist.Sum(p => p.Ysje);
            if (je > 0)
            {
                Bill_Main Ndbill = new Bill_Main();
                Ndbill.BillCode = billcode;
                Ndbill.BillName = ndgcbh;
                Ndbill.FlowId = flowid;
                Ndbill.StepId = "end";
                Ndbill.BillDept = deptcode;
                Ndbill.BillUser = usercode;
                Ndbill.BillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                Ndbill.BillJe = je;
                Ndbill.LoopTimes = 1;
                Ndbill.BillType = "1";
                new Dal.Bills.MainDal().InsertMain(Ndbill);
            }
        }
        public void GetFjtbNdys(string nd, string deptcode, string usercode, string ndgcbh, string flowid)
        {
            string strbillcode = new Bll.Bills.BillMainBLL().GetBillcodeByDeptAndYsgc(ndgcbh, deptcode, flowid);
            if (strbillcode == "")  //没有生成过年度预算
            {
                CreateYsByBmfj(nd, deptcode, usercode, ndgcbh, flowid);
            }
            else
            {
                //先删除
                new Dal.Bills.MainDal().DeleteMain(strbillcode);
                new Dal.newysgl.bill_ysmxbDal().DeleteByBillCode(strbillcode);
                CreateYsByBmfj(nd, deptcode, usercode, ndgcbh, flowid);
            }
        }
    }
}
