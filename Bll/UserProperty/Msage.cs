using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.NewsDictionary;
using Models;

namespace Bll.UserProperty
{
    public class Msage
    {
     
        MsgDal msgdal = new MsgDal();
      
       



        /// <summary>
        /// 得到所有消息
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<Bill_Msg> GetNews()
        {
            return msgdal.GetNews();
        }

        /// <summary>
        /// 获得usercode发布的通知
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<Bill_Msg> GetMessageByMaker(string userCode)
        {
            return msgdal.GetMessageByMaker(userCode);
        }

       
        
    }
}
