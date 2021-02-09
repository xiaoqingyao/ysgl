using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class MessageReader
    {
        /// <summary>
        /// usercode
        /// </summary>		
        private string usercode;
        public string Usercode
        {
            get { return usercode; }
            set { usercode = value; }
        }
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
        /// isRead
        /// </summary>		
        private int isread;
        public int IsRead
        {
            get { return isread; }
            set { isread = value; }
        }        
    }
}
