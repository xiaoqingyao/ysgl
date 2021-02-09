using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.NewsDictionary;
using Models;

namespace Bll.UserProperty
{
    public class DeskManager
    {
        MessageDal msgDal = new MessageDal();

        /// <summary>
        /// 增加一条新闻或通知
        /// </summary>
        public void Add(TitleMessage model)
        {
            msgDal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string code)
        {
            msgDal.Delete(code);
        }
        /// <summary>
        /// 获得行数
        /// </summary>
        /// <returns></returns>
        public int GetNewsCount()
        {
            return msgDal.GetNewsCount("1");
        }
        /// <summary>
        /// 获得通知行数
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int GetMessageCount(string userCode)
        {
            return msgDal.GetMessageCount(userCode);
        }

        /// <summary>
        /// 获得可读通知
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int GetReaderCount(string userCode)
        {
            return msgDal.GetReaderCount(userCode);
        }



        /// <summary>
        /// 根据开始行结束行获得数据
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetNews(int beg, int end)
        {
            return msgDal.GetNews(beg, end);
        }

        /// <summary>
        /// 获得usercode发布的信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetMessageByMaker(string userCode, int beg, int end)
        {
            return msgDal.GetMessageByMaker(userCode, beg, end);
        }

        /// <summary>
        /// 获得usercode可以读的通知
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetMessageByReader(string userCode, int beg, int end)
        {
            return msgDal.GetMessageByReader(userCode, beg, end);
        }
        /// <summary>
        /// 获得单据状态，是否已读
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public string GetMessageState(string code, string userCode)
        {
            int isread = msgDal.GetMessageState(code, userCode);
            if (isread == 1)
            {
                return "已读";
            }
            else
            {
                return "未读";
            }
        }

        /// <summary>
        /// 根据单号获得新闻
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public TitleMessage GetNewsByCode(string code)
        {
            return msgDal.GetNewsByCode(code);
        }

        /// <summary>
        /// 更新信息状况已读
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="code"></param>
        /// <param name="val"></param>
        public void UpReaded(string userCode, string code)
        {
            msgDal.UpdateMessageState(userCode, code, 1);
        }
        /// <summary>
        /// 更新信息状况未读
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="code"></param>
        /// <param name="val"></param>
        public void UpUnreaded(string userCode, string code)
        {
            msgDal.UpdateMessageState(userCode, code, 0);
        }
        
    }
}
