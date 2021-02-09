using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Ysmxb
    {
        private string gcbh;

        public string Gcbh
        {
            get { return gcbh; }
            set { gcbh = value; }
        }
        private string billCode;

        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        private string yskm;

        public string Yskm
        {
            get { return yskm; }
            set { yskm = value; }
        }
        private decimal ysje;

        public decimal Ysje
        {
            get { return ysje; }
            set { ysje = value; }
        }
        private string ysDept;

        public string YsDept
        {
            get { return ysDept; }
            set { ysDept = value; }
        }

        /// <summary>
        /// 说明
        /// </summary>
        private string sm = "";
        public string Sm
        {
            get { return sm; }
            set { sm = value; }
        }

        private string ysType;

        /// <summary>
        /// 预算类型 1，一般预算，2，追加，3预算调整，4科目之间调整 5预算内追加
        /// </summary>
        public string YsType
        {
            get { return ysType; }
            set { ysType = value; }
        }
        /// <summary>
        /// 预算科目名称 非数据库字段
        /// </summary>
        public string YskmMc { get; set; }
        /// <summary>
        /// 预算过程名称 非数据库字段
        /// </summary>
        public string GcMc { get; set; }
    }
}
