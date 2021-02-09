using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Xm
    {
        private string xmCode;

        public string XmCode
        {
            get { return xmCode; }
            set { xmCode = value; }
        }
        private string xmName;

        public string XmName
        {
            get { return xmName; }
            set { xmName = value; }
        }
        private string sjXm;

        public string SjXm
        {
            get { return sjXm; }
            set { sjXm = value; }
        }
        private string xmDept;

        public string XmDept
        {
            get { return xmDept; }
            set { xmDept = value; }
        }
        private string xmStatus;

        public string XmStatus
        {
            get { return xmStatus; }
            set { xmStatus = value; }
        }
    }
}
