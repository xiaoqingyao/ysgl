using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Msg
    {
        /// <summary>
        /// bill_msg 发布消息表
        /// </summary>
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private string title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string contents;
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }
        private string writer;
        /// <summary>
        /// 发布人
        /// </summary>
        public string Writer
        {
            get { return writer; }
            set { writer = value; }
        }
        private DateTime date;
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        private string readTimes;
        /// <summary>
        /// 阅读次数
        /// </summary>
        public string ReadTimes
        {
            get { return readTimes; }
            set { readTimes = value; }
        }

        private string mstype;
        /// <summary>
        /// 消息类型--消息---通知
        /// </summary>
        public string Mstype
        {
            get { return mstype; }
            set { mstype = value; }
        }

        private string notifierid;
        /// <summary>
        /// 如果是通知 通知人id
        /// </summary>
        public string Notifierid
        {
            get { return notifierid; }
            set { notifierid = value; }
        }
        private string notifiername;
        /// <summary>
        /// 被通知人姓名
        /// </summary>
        public string Notifiername
        {
            get { return notifiername; }
            set { notifiername = value; }
        }

        private string endtime;
        /// <summary>
        /// 通知截止时间
        /// </summary>
        public string Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }
        private string accessories;
        /// <summary>
        /// 附件路径
        /// </summary>
        public string Accessories 
        {
            get { return accessories; }
            set { accessories = value; }
        }
    }
}
