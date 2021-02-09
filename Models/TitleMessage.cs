using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TitleMessage
    {
        /// <summary>
        /// code
        /// </summary>		
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// title
        /// </summary>		
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// context
        /// </summary>		
        private string context;
        public string Context
        {
            get { return context; }
            set { context = value; }
        }
        /// <summary>
        /// memo
        /// </summary>		
        private string memo;
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
        /// <summary>
        /// userCode
        /// </summary>		
        private string usercode;
        public string UserCode
        {
            get { return usercode; }
            set { usercode = value; }
        }
        /// <summary>
        /// messageDate
        /// </summary>		
        private DateTime messagedate;
        public DateTime MessageDate
        {
            get { return messagedate; }
            set { messagedate = value; }
        }
        /// <summary>
        /// msgType
        /// </summary>		
        private string msgtype;
        public string MsgType
        {
            get { return msgtype; }
            set { msgtype = value; }
        }
        /// <summary>
        /// upfile
        /// </summary>		
        private string upfile;
        public string Upfile
        {
            get { return upfile; }
            set { upfile = value; }
        }

        private IList<MessageReader> userlist = new List<MessageReader>();

        public IList<MessageReader> Userlist
        {
            get { return userlist; }
            set { userlist = value; }
        }
    }
}
