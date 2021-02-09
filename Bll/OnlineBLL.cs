using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dal;
using System.Security.Cryptography;

namespace Bll
{
    /// <summary>
    /// 控制同时在线人数点数服务类
    /// </summary>
    public class OnlineBLL
    {
        public DataTable GetAll() {
            string strsql = "select * from t_online";
            DataTable dt = DataHelper.GetDataTable(strsql, null, false);
            if (dt == null)
            {
                return new DataTable();
            }
            else {
                return dt;
            }
        }
        /// <summary>
        /// 使人员下线（在表中踢出）
        /// </summary>
        /// <param name="currentCode"></param>
        public void UnOnlineUser(string userCode)
        {
            string strsql = "delete from t_online where usercode='"+userCode+"'";
            DataHelper.ExcuteNonQuery(strsql, null, false);
        }
        public bool IsExit(string userCode) { 
            string strsql="select count(*) from t_online where usercode='"+userCode+"'";
            object objrel = DataHelper.ExecuteScalar(strsql, null, false);
            int icount = objrel == null ? 0 : int.Parse(objrel.ToString());
            return icount > 0 ? true : false;
        }

        public void UpLastTime(string usercode) {
            string strdate = DateTime.Now.ToString();
            string strsql = "update t_online set lastonlinetime='"+strdate+"' where usercode='"+usercode+"'";
            DataHelper.ExcuteNonQuery(strsql, null, false);
        }

        public void AddUser(string usercode) {
            string strdate = DateTime.Now.ToString();
            string strsql = "insert into t_online(usercode,online,lastonlinetime) values('"+usercode+"','1','"+strdate+"')";
            DataHelper.ExcuteNonQuery(strsql, null, false);
        }

        public int GetOnlineCount() {
            string strsql = "select count(*) from t_online";
            object objrel = DataHelper.ExecuteScalar(strsql, null, false);
            int icount = objrel == null ? 0 : int.Parse(objrel.ToString());
            return icount;
        }

        public int GetMaxOnlineCount() {
            string strsql = "select block0 from t_zcxx2";
            object objrel = DataHelper.ExecuteScalar(strsql, null, false);
            if (objrel==null)
            {
                return 0;
            }
            string strMsg = this.Decrypt(objrel.ToString(), "vsoft");
            string[] arrrel = strMsg.Split(new char[]{'@'}, StringSplitOptions.RemoveEmptyEntries);
            if (arrrel.Length<=2)
	        {
                return 0;
	        }
            string strpoint = arrrel[2];
            return int.Parse(strpoint);
        }
        public void refresh() {
            string strDateTime="";
            strDateTime = DateTime.Now.AddSeconds(-20).ToString();
            string strsql = "delete from t_online where lastonlinetime<'"+strDateTime+"'";
            DataHelper.ExcuteNonQuery(strsql, null, false);
        }
        private string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            if (stringToDecrypt == null)
            {
                return string.Empty;
            }

            sEncryptionKey = sEncryptionKey.PadLeft(16, '0');

            byte[] key = { };
            byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);
            try
            {
                key = Encoding.Unicode.GetBytes(sEncryptionKey.Substring(0, 16));

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = key;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);

                return Encoding.Unicode.GetString(resultArray);
            }
            catch
            {
                return (string.Empty);
            }
        }
    }
}
