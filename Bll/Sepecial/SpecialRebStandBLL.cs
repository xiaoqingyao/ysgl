using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;

namespace Bll.Sepecial
{
    public class SpecialRebStandBLL
    {

        Dal.TSFLSPZD.SpeRebStandardDal spedal = new Dal.TSFLSPZD.SpeRebStandardDal();
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        public int Insert(IList<T_SpecialRebatesStandardmodel> Ilistmode, out string msg)
        {
            try
            {
                msg = "";
                spedal.Delete(Ilistmode[0].AppBillCode.ToString());
                for (int i = 0; i < Ilistmode.Count; i++)
                {
                  
                    int iRel = spedal.Add(Ilistmode[i]);
                    if (iRel <= 0)
                    {
                        throw new Exception("未知错误！");
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }

        }

        public DataTable getTablepro(string strdeptcode, string strappcode, string strcarcode)
        {

            return spedal.getexpro(strdeptcode, strappcode, strcarcode);
        }
       
    }
}
