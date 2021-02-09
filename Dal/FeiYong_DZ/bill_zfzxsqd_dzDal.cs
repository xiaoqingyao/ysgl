using Dal.Bills;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dal.FeiYong_DZ
{
    public class bill_zfzxsqd_dzDal
    {
        public bill_zfzxsqd_dz GetmodelByCode(string billcode)
        {
            string sql = " select * from bill_zfzxsqd_dz where billcode=@billcode ";
            SqlParameter[] sps = { new SqlParameter("@billcode", billcode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    bill_zfzxsqd_dz zfzxsqd = new bill_zfzxsqd_dz();
                    zfzxsqd.billcode = Convert.ToString(dr["billcode"]);
                    zfzxsqd.sqrq = Convert.ToString(dr["sqrq"]);
                    zfzxsqd.zcfx = Convert.ToString(dr["zcfx"]);
                    zfzxsqd.zrfx = Convert.ToString(dr["zrfx"]);
                    zfzxsqd.xyxm = Convert.ToString(dr["xyxm"]);
                    zfzxsqd.nianji = Convert.ToString(dr["nianji"]);
                    zfzxsqd.yxyfdfy = decimal.Parse(dr["yxyfdfy"].ToString());


                    zfzxsqd.ybmkc = Convert.ToString(dr["ybmkc"]);
                    zfzxsqd.ykcxsyh = Convert.ToString(dr["ykcxsyh"]);
                    zfzxsqd.yxfks = decimal.Parse(dr["yxfks"].ToString());
                    zfzxsqd.dyksdj = decimal.Parse(dr["dyksdj"].ToString());
                    zfzxsqd.yxffy = decimal.Parse(dr["yxffy"].ToString());
                    zfzxsqd.ykqtfy = decimal.Parse(dr["ykqtfy"].ToString());
                    zfzxsqd.syjexx = decimal.Parse(dr["syjexx"].ToString());
                    zfzxsqd.syjedx = Convert.ToString(dr["syjedx"]);
                    zfzxsqd.zfyy = Convert.ToString(dr["zfyy"]);
                    zfzxsqd.xbxs = Convert.ToString(dr["xbxs"]);
                    zfzxsqd.xbjje = decimal.Parse(dr["xbjje"].ToString());
                    zfzxsqd.xbjjedx = Convert.ToString(dr["xbjjedx"]);
                    zfzxsqd.note1 = Convert.ToString(dr["note1"]);
                    zfzxsqd.note2 = Convert.ToString(dr["note2"]);
                    zfzxsqd.note3 = Convert.ToString(dr["note3"]);
                    zfzxsqd.note4 = Convert.ToString(dr["note4"]);
                    zfzxsqd.note5 = Convert.ToString(dr["note5"]);
                    return zfzxsqd;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="MainCode"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete(string MainCode)
        {
            MainDal mdal = new MainDal();
            mdal.DeleteMain(MainCode);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_zfzxsqd_dz ");
            strSql.Append(" where billcode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = MainCode;

            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);
        }

    }
}
