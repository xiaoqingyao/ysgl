using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;

namespace Bll.PingZheng
{
    /// <summary>
    /// 凭证项目BLL
    /// Edit by Lvcc
    /// </summary>
    public class PingZheng_XMBLL
    {
        Dal.PingZheng.bill_pingzheng_xm dal = new Dal.PingZheng.bill_pingzheng_xm();
        public int Delete(string strCode, out string msg)
        {
            try
            {
                msg = "";
                return dal.Delete(int.Parse(strCode));
            }
            catch (Exception ex)
            {
                msg = ex.Message ;
                return -1;
            }
        }
        public IList<bill_pingzheng_xmModel> GetAll()
        {
            return dal.GetAllList();
        }
        public DataSet GetAlltb() {
            DataSet ds = new DataSet();
            ds.Tables.Add(dal.GelAllTable());
            return ds;
        }

        public int Add(bill_pingzheng_xmModel modelPingZhengXmModel, out string strMsg)
        {
            try
            {
                strMsg = "";
                return dal.Add(modelPingZhengXmModel);
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return -1;
            }
        }

        public bill_pingzheng_xmModel GetModel(string strCode)
        {
            return dal.GetModel(int.Parse(strCode));
        }
        public bill_pingzheng_xmModel GetModelByName(string strName) {
            return dal.GetModelByName(strName);
        }
        public IList<bill_pingzheng_xmModel> GetChildsByName(string strName)
        {
            return dal.GetChildsByName(strName);
        }
        public bill_pingzheng_xmModel GetChildByName(string strName) {
            return dal.GetChildByName(strName);
        }

        public IList<bill_pingzheng_xmModel> GetAllParent()
        {
            return dal.GetAllParent();
        }

        public bool ExistsNext(string code)
        {
            return dal.ExistsNext(code);
        }
    }
}
