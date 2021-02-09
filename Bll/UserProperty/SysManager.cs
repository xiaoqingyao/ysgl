using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.SysDictionary;
using Models;
using Dal.UserProperty;
using Dal.Bills;
using Dal;

namespace Bll.UserProperty
{
    public class SysManager
    {

        DataDicDal dicDal = new DataDicDal();
        DepartmentDal deptDal = new DepartmentDal();
        YskmDal yskmDal = new YskmDal();
        XmDal xmDal = new XmDal();
        SysconfigDal configDal = new SysconfigDal();
        UsersDal userDal = new UsersDal();
        MenuDal mainDal = new MenuDal();
        NoteDal noteDal = new NoteDal();

        public IList<Bill_NotePad> GetNoteByUserDate(string userCode, DateTime dt)
        {
            return noteDal.GetNoteByUserDate(userCode, dt);
        }

        public IList<Bill_SysMenu> GetMenuAll()
        {
            return mainDal.GetMenuAll();
        }

        public IList<Bill_SysMenu> GetMenuRoot()
        {
            return mainDal.GetMenuRoot();
        }

        public IList<Bill_SysMenu> GetMenuByUser(string userCode)
        {
            return mainDal.GetMenuByUser(userCode);
        }

        public IList<Bill_SysMenu> GetMenuByUserAll(string userCode)
        {
            return mainDal.GetMenuByUserAll(userCode);
        }



        public IList<Bill_Users> GetAllUser()
        {
            return userDal.GetAllUser();
        }
        public string GetUserGroup(string code)
        {
            return userDal.GetUserGroup(code);
        }

        public IDictionary<string, string> GetsysConfig()
        {
            return configDal.GetsysConfig();
        }

        public IList<Bill_DataDic> GetDicByType(string typecode)
        {
            return dicDal.GetDicByType(typecode);
        }

        public Bill_DataDic GetDicByTypeCode(string typecode, string code)
        {
            return dicDal.GetDicByTypeCode(typecode, code);
        }

        public IList<Bill_Xm> GetXmByDep(string deptCode)
        {
            return xmDal.GetXmByDep(deptCode);
        }
        public IList<bill_xm_dept_nd> GetXmByDepNd(string deptCode, string nd)
        {
            return xmDal.GetXmByDepNd(deptCode, nd);
        }
        public IList<Bill_Xm> GetmjXmByDep(string deptCode)
        {
            return xmDal.GetmjXmByDep(deptCode);
        }
        /// <summary>
        /// 获得所有部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllDept()
        {
            return deptDal.GetAllDept();
        }
        /// <summary>
        /// 获取所有二级部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllDeptsed()
        {
            return deptDal.GetAllDeptsed();
        }

        /// <summary>
        /// 获取所有销售公司的二级部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllDeptsedsel()
        {
            return deptDal.GetAllDeptsedsale();
        }


        /// <summary>
        /// 获得所有子部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllRootDept()
        {
            ConfigDal configDal = new ConfigDal();
            //1. 判断是否预算到末级部门
            string strismj = configDal.GetValueByKey("deptjc");//是否预算到末级部门 Y 是 N或者null 否
            Bill_Departments rootDept = deptDal.GetRootDept();
            IList<Bill_Departments> rootList = new List<Bill_Departments>();
            if (!string.IsNullOrEmpty(strismj) && strismj == "Y")
            {
                rootList = deptDal.GetdeptMj(rootDept.DeptCode);
            }
            else
            {
                rootList = deptDal.GetDeptByRoot(rootDept.DeptCode);

            }
            rootList.Add(rootDept);
            return rootList;
        }
        /// <summary>
        /// 获得部门
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public Bill_Departments GetDeptByCode(string deptCode)
        {
            return deptDal.GetDeptByCode(deptCode);
        }
        /// <summary>
        /// 获得[部门编号]+部门名称
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string GetDeptCodeName(string deptCode)
        {
            Bill_Departments dept = deptDal.GetDeptByCode(deptCode);
            if (dept == null) { return ""; }
            else { return "[" + dept.DeptCode + "]" + dept.DeptName; }
        }
        /// <summary>
        /// 获得该部门的预算科目
        /// </summary>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmByDep(string depcode)
        {
            return yskmDal.GetYskmByDep(depcode);
        }
        /// <summary>
        /// 根据部门编号获得该部门可以填报/报销的预算科目
        /// </summary>
        /// <param name="depcode">部门编号</param>
        /// <param name="strdydjcode">对应单据编号 01 收入类单据 02 报销单 03固定资产购置</param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmByDep(string depcode, string strdydjcode)
        {
            return yskmDal.GetYskm(depcode, strdydjcode, "");
        }

        /// <summary>
        /// 根据部门编号获得该部门可以填报/报销的预算科目
        /// </summary>
        /// <param name="depcode">部门编号</param>
        /// <param name="strdydjcode">对应单据编号 01 收入类单据 02 报销单 03固定资产购置</param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetGkYskmByDep(string depcode, string strdydjcode)
        {
            return yskmDal.GetGkYskmByDep(depcode, strdydjcode);
        }


        /// <summary>
        /// 根据部门编号获得该部门可以填报/报销的预算科目
        /// </summary>
        /// <param name="depcode">部门编号</param>
        /// <param name="strdydjcode">对应单据编号 01 收入类单据 02 报销单 03固定资产购置</param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetGkYskmByDep(string depcode, string strdydjcode, string strdjlx)
        {
            return yskmDal.GetGkYskmByDep(depcode, strdydjcode, strdjlx);
        }
        /// <summary>
        /// 获取车辆类型
        /// </summary>
        /// <returns></returns>
        public IList<T_truckType> GetCarType()
        {
            Dal.TruckType.TruckTypeDal typedal = new Dal.TruckType.TruckTypeDal();
            return typedal.GetAllList();


        }

        /// <summary>
        /// 获得所有预算科目
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmAll()
        {
            return yskmDal.GetYskmAll();
        }

        /// <summary>
        /// 获取所有的归口预算科目
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Yskm> GetGkYskmAll()
        {
            return yskmDal.GetGkYskmAll();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<bill_ys_benefitpro> GetAllfy(string strYear, string strType)
        {
            return deptDal.GetAllfy(strYear, strType);
        }

        /// <summary>
        /// 根据科目编号获得预算科目
        /// </summary>
        /// <param name="yskmCode"></param>
        /// <returns></returns>
        public Bill_Yskm GetYskmByCode(string yskmCode)
        {
            return yskmDal.GetYskmByCode(yskmCode);
        }
        /// <summary>
        /// 为传进来的预算科目是否是末级字段赋值
        /// </summary>
        /// <param name="list"></param>
        public void SetEndYsbm(IList<Bill_Yskm> list)
        {
            foreach (Bill_Yskm yskm in list)
            {
                int temp = (from linq in list
                            where linq.YskmCode.Length > yskm.YskmCode.Length && linq.YskmCode.Substring(0, yskm.YskmCode.Length) == yskm.YskmCode
                            select linq).Count();
                if (temp > 0)
                {
                    yskm.IsEnd = "0";
                }
                else
                {
                    yskm.IsEnd = "1";
                }
            }
        }
        /// <summary>
        /// 站在所有的预算科目的角度来讲  判断某个预算科目是否是末级  区别于SetEndYsbm 方法
        /// </summary>
        /// <param name="list"></param>
        public void SetEndYskmByAll(IList<Bill_Yskm> list)
        {
            IList<Bill_Yskm> listAll = this.GetYskmAll();
            foreach (Bill_Yskm yskm in list)
            {
                int temp = (from linq in listAll
                            where linq.YskmCode.Length > yskm.YskmCode.Length && linq.YskmCode.Substring(0, yskm.YskmCode.Length) == yskm.YskmCode
                            select linq).Count();
                if (temp > 0)
                {
                    yskm.IsEnd = "0";
                }
                else
                {
                    yskm.IsEnd = "1";
                }
            }
        }
        /// <summary>
        /// 将集合中的所有的末端子集显示出来
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Bill_Yskm> GetAllChildren(List<Bill_Yskm> list, List<Bill_Yskm> listend, string deptCode)
        {
            foreach (Bill_Yskm yskm in list)
            {
                if (yskm.IsEnd.Equals("1"))
                {
                    if (!this.Exit(listend, yskm.YskmCode))
                    {
                        listend.Add(yskm);
                    }
                    continue;
                }
                else
                {
                    List<Bill_Yskm> lstChildKm = (List<Bill_Yskm>)yskmDal.GetChilds(yskm.YskmCode, deptCode);
                    var va = (from linq in lstChildKm
                              where linq.YskmCode.Length > yskm.YskmCode.Length && linq.YskmCode.Substring(0, yskm.YskmCode.Length) == yskm.YskmCode
                              select linq);
                    List<Bill_Yskm> lstRel = new List<Bill_Yskm>();
                    foreach (Bill_Yskm item in va)
                    {
                        lstRel.Add(item);
                    }
                    SetEndYskmByAll(lstRel);
                    GetAllChildren(lstRel, listend, deptCode);
                }
            }
            return listend;
        }
        private bool Exit(List<Bill_Yskm> list, string code)
        {
            bool isExit = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].YskmCode.Equals(code))
                {
                    isExit = true;
                    break;
                }
            }
            return isExit;
        }
        /// <summary>
        /// 根据部门取得预算科目编号集合
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string[] GetYskmCodeByDept(string deptcode)
        {
            return yskmDal.GetYskmCodeByDept(deptcode);
        }
        /// <summary>
        /// 根据部门和单据类型
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="strdjlx"></param>
        /// <returns></returns>
        public string[] GetYskmCodeByDept(string deptcode, string strdjlx)
        {
            return yskmDal.GetYskmCodeByDept(deptcode, strdjlx);
        }
        /// <summary>
        /// 根据部门取得guikou预算科目编号集合
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string[] GetGkYskmCodeByDept(string deptcode)
        {
            return yskmDal.GetGkYskmCodeByDept(deptcode);
        }


        public string GetYskmNameCode(string yskmCode)
        {
            return yskmDal.GetYskmNameCode(yskmCode);
        }

        public Bill_Xm GetXmByCode(string xmCode)
        {
            return xmDal.GetXmByCode(xmCode);
        }
        public string GetXmCodeName(string xmCode)
        {
            Bill_Xm xm = xmDal.GetXmByCode(xmCode);
            if (xm == null)
            {
                return null;
            }
            else
            {
                return "[" + xm.XmCode + "]" + xm.XmName;
            }
        }

        /// <summary>
        /// 获得单据编号
        /// </summary>
        /// <param name="card">单据号头</param>
        /// <param name="seed">单据日期20120101</param>
        /// <param name="type">类型0是读取1是修改</param>
        /// <returns></returns>
        public string GetYbbxBillName(string card, string seed, int type)
        {
            return dicDal.GetYbbxBillName(card, seed, type);
        }
        //判断是否末级，0，是，> 0，非
        public string GetYskmIsmj(string yskmcode)
        {
            return yskmDal.GetYskmIsmj(yskmcode);
        }
        //根据年度获取预算配置信息
        public IDictionary<string, string> GetsysConfigBynd(string nd)
        {
            return configDal.GetsysConfigBynd(nd);
        }

        public IList<Bill_Departments> GetAllDept(int pagefrm, int pageto, out int count)
        {
            return deptDal.GetAllDept(pagefrm, pageto, out count);
        }
       
    }
}


