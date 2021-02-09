using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Yskm
    {
        string yskmBm;

        public string YskmBm
        {
            get { return yskmBm; }
            set { yskmBm = value; }

        }
        string yskmCode;

        public string YskmCode
        {
            get { return yskmCode; }
            set { yskmCode = value; }
        }
        string yskmMc;

        public string YskmMc
        {
            get { return yskmMc; }
            set { yskmMc = value; }
        }
        string gjfs;

        public string Gjfs
        {
            get { return gjfs; }
            set { gjfs = value; }
        }
        string tbsm;

        public string Tbsm
        {
            get { return tbsm; }
            set { tbsm = value; }
        }
        string tblx;

        public string Tblx
        {
            get { return tblx; }
            set { tblx = value; }
        }
        string kmStatus;

        public string KmStatus
        {
            get { return kmStatus; }
            set { kmStatus = value; }
        }

        string kmLx;

        public string KmLx
        {
            get { return kmLx; }
            set { kmLx = value; }
        }

        string gkFy;

        public string GkFy
        {
            get { return gkFy; }
            set { gkFy = value; }
        }

        string xmHs;

        public string XmHs
        {
            get { return xmHs; }
            set { xmHs = value; }
        }

        string bmHs;

        public string BmHs
        {
            get { return bmHs; }
            set { bmHs = value; }
        }

        string ryHs;

        public string RyHs
        {
            get { return ryHs; }
            set { ryHs = value; }
        }

        //程序使用，数据库无
        //是否末级
        string isEnd;

        public string IsEnd
        {
            get { return isEnd; }
            set { isEnd = value; }
        }
        //预算科目级次
        string grade;

        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        //程序使用，数据库无,在利润项目科目对于时使用---wzc20121019
        //是否末级
        string dept;

        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        /// <summary>
        /// 对应单据
        /// </summary>
        public string dydj
        {
            get;
            set;
        }

        string kmzg;
        public string Kmzg 
        {
            get;
            set;
        }
        /// <summary>
        /// 填报预算时 是否占用总预算限额 1是 0否
        /// </summary>
        public string iszyys { get; set; }
    }
}
