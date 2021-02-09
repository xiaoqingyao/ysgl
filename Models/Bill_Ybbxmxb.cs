using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 一般报销明细表
    /// </summary>
    public class Bill_Ybbxmxb
    {
        string billCode;
        /// <summary>
        /// 报销单号
        /// </summary>
        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        string bxr;
        /// <summary>
        /// 报销人
        /// </summary>
        public string Bxr
        {
            get { return bxr; }
            set { bxr = value; }
        }

        string bxrzh;
        /// <summary>
        /// 报销账号
        /// </summary>
        public string Bxrzh
        {
            get { return bxrzh; }
            set { bxrzh = value; }
        }

        string bxrphone;
        /// <summary>
        /// 报销电话
        /// </summary>
        public string Bxrphone
        {
            get { return bxrphone; }
            set { bxrphone = value; }
        }
        /// <summary>
        /// 报销人（在市里医院项目里 做报销人  该报销人只是记个名字而已 不影响原来的数据结构）
        /// </summary>
        public string Bxr2 { get; set; }
        string bxzy;
        /// <summary>
        /// 报销摘要
        /// </summary>
        public string Bxzy
        {
            get { return bxzy; }
            set { bxzy = value; }
        }
        string bxsm;
        /// <summary>
        /// 报销说明
        /// </summary>
        public string Bxsm
        {
            get { return bxsm; }
            set { bxsm = value; }
        }
        string sfdk;

        public string Sfdk
        {
            get { return sfdk; }
            set { sfdk = value; }
        }
        decimal ybje;
        /// <summary>
        /// 给付金额
        /// </summary>
        public decimal Ybje
        {
            get { return ybje; }
            set { ybje = value; }
        }
        decimal ytje;

        public decimal Ytje
        {
            get { return ytje; }
            set { ytje = value; }
        }
        string sfgf;
        /// <summary>
        /// 是否给付
        /// </summary>
        public string Sfgf
        {
            get { return sfgf; }
            set { sfgf = value; }
        }
        string bxmxlx;
        /// <summary>
        /// 报销明细类型
        /// </summary>
        public string Bxmxlx
        {
            get { return bxmxlx; }
            set { bxmxlx = value; }
        }
        string gfr;
        /// <summary>
        /// 给付人
        /// </summary>
        public string Gfr
        {
            get { return gfr; }
            set { gfr = value; }
        }
        DateTime? gfsj;
        /// <summary>
        /// 给付时间
        /// </summary>
        public DateTime? Gfsj
        {
            get { return gfsj; }
            set { gfsj = value; }
        }

        private int bxdjs;
        /// <summary>
        /// 报销单据数
        /// </summary>
        public int Bxdjs
        {
            get { return bxdjs; }
            set { bxdjs = value; }
        }

        private string _pzcode = "";
        /// <summary>
        /// 凭证号
        /// </summary>
        public string Pzcode
        {
            get { return _pzcode; }
            set { _pzcode = value; }
        }

        private string _pzdate = "";
        /// <summary>
        /// 凭证日期
        /// </summary>
        public string Pzdate
        {
            get { return _pzdate; }
            set { _pzdate = value; }
        }

        private string _guazhang = "";
        /// <summary>
        /// 是否挂账
        /// </summary>
        public string Guazhang
        {
            get { return _guazhang; }
            set { _guazhang = value; }
        }
        private string _sqlx = "";
        /// <summary>
        /// 申请类型
        /// </summary>
        public string Sqlx
        {
            get { return _sqlx; }
            set { _sqlx = value; }
        }

        private string _ykfs;
        /// <summary>
        /// 用款方式
        /// </summary>
        public string Ykfs
        {
            get { return _ykfs; }
            set { _ykfs = value; }
        }
        private string _note0;
        /// <summary>
        ///  所在分校 |&|学员姓名|&|所在年级|&|协议编号|&|签单时间
        /// </summary>
        public string note0
        {
            get { return _note0; }
            set { _note0 = value; }
        }
        private string _note1;
        /// <summary>
        ///  协议辅导费用|&|已消费课时|&|对应课时单价|&|已消费费用|&|应扣其他费用
        /// </summary>
        public string note1
        {
            get { return _note1; }
            set { _note1 = value; }
        }
        private string _note2;
        public string note2
        {
            get { return _note2; }
            set { _note2 = value; }
        }
        private string _note3;
        public string note3
        {
            get { return _note3; }
            set { _note3 = value; }
        }
        private string _note4;
        public string note4
        {
            get { return _note4; }
            set { _note4 = value; }
        }
        private string _note5;
        public string note5
        {
            get { return _note5; }
            set { _note5 = value; }
        }

        private IList<Bill_Ybbxmxb_Fykm> kmList;

        public IList<Bill_Ybbxmxb_Fykm> KmList
        {
            get { return kmList; }
            set { kmList = value; }
        }

        private IList<Bill_Ybbx_Fysq> fysqList;

        public IList<Bill_Ybbx_Fysq> FysqList
        {
            get { return fysqList; }
            set { fysqList = value; }
        }
        public string fujian { get; set; }
    }
}
