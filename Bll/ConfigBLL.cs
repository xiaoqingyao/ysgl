using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal;
using Models;
using System.Data;

namespace Bll
{
     /// <summary>
    /// 系统配置表操作类 对应[t_Config]
    /// Edit By Lvcc
    /// </summary>
    public class ConfigBLL
    {

        /// <summary>
        /// 根据配置项名称获取配置项值
        /// </summary>
        /// <param name="p">配置项名称</param>
        /// <returns></returns>
        public string GetValueByKey(string p) 
        {
            ConfigDal dalConfig = new ConfigDal();
            return dalConfig.GetValueByKey(p);
        }
        public DataTable GetDtByKey(string key) {
            ConfigDal dalConfig = new ConfigDal();
            return dalConfig.GetDtByKey(key);
        }
        /// <summary>
        /// 根据配置项名称设置value值
        /// </summary>
        /// <param name="strKey">键</param>
        /// <param name="strVal">值</param>
        public int SetValueByKey(string strKey, string strVal, out string msg)
        {
            msg = "";
            ConfigDal dalConfig = new ConfigDal();
            try
            {
                int iRel = dalConfig.SetValueByKey(strKey, strVal);
                if (iRel < 1)
                {
                    throw new Exception("未知错误！");
                }
                return iRel;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 系统是否配置有某模块功能
        /// <param name="strKey">数据库config表中的配置项</param>
        /// </summary>
        public bool GetModuleDisabled(string strKey) {
            string strRel = this.GetValueByKey(strKey);
            if (!strRel.Equals("0"))
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 设置系统是否配置有财务预算的功能
        /// </summary>
        /// <param name="strVal"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int SetModuleDisable(string strKey, string strVal, out string msg)
        {
            string strAppendSql = "";
            string strMenuStatus = strVal.Equals("1") ? "1" : "D";
            switch (strKey)
            {
                case "HasBudgetControl": strAppendSql = " and left(menuid,2)='03'"; break;
                case "HasBGSQ": strAppendSql = " and menuid='0803' or menuid='0805' or menuid in(select menuid from bill_sysMenu where menuName like '%报告申请单%' or menuName like '%临时采购审核%')"; break;
                case "HasCGSP": strAppendSql = " and menuid='0804' or menuid='0806' or menuid in(select menuid from bill_sysMenu where menuName like '%采购审批单%' or menuName like '%采购审批单审核%')"; break;
                case "HasXMZF": strAppendSql = " and menuid='0807' or menuid='0808' or menuid in(select menuid from bill_sysMenu where menuName like '%项目支付申请单%' or menuName like '%项目支付申请审核%')"; break;
                case "HasCCSQ": strAppendSql = " and menuid='0809' or menuid='0810' or menuid in(select menuid from bill_sysMenu where menuName like '%出差申请单%' or menuName like '%出差申请单审核%')"; break;
                default:
                    break;
            }
            int iRel = this.SetValueByKey(strKey, strVal, out msg);
            if (iRel<1)
            {
                return -1;
            }
            //将对应模块的菜单的状态设置为D 这样在设置权限和菜单栏加载的时候就无法加载
            if (!string.IsNullOrEmpty(strAppendSql))
            {
                 string sql = "update bill_sysMenu set menustate='" + strMenuStatus + "' where 1=1 "+strAppendSql;
            try
            {
                iRel = Dal.DataHelper.ExcuteNonQuery(sql, null, false);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
            if (iRel < 1)
            {
                msg = "出现未知错误";
                return -1;
            }
            else {
                return 1;
            }
            }
            return 1;
            
        }
        //isYear 是否年度 isMonth 月度还是季度 tbfs填报方式  parList设置日期  menulist要关闭或者让开启的菜单
        public bool SetYscs(string isYear, string isMonth, string tbfs, IList<Models.bill_syspar> parList, IList<Bill_SysMenu> menulist,string nd, string ndStatus)
        {
            return new Dal.ConfigDal().SetYscs(isYear,isMonth,tbfs,parList,menulist,nd, ndStatus);
        }
        //添加年度
        public bool AddYscs(string isYear, string isMonth, string tbfs, string nd, string ndStatus)
        {
            return new Dal.ConfigDal().AddYscs(isYear,isMonth,tbfs,nd,ndStatus);
        }
    }
}
