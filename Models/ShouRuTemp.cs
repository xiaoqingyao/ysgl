using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ShouRuTemp : IComparable<ShouRuTemp>
    {
        /// <summary>
        /// 项目类型：1为课程，2为物品
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        public string CampusName { get; set; }
        /// <summary>
        /// 收据号
        /// </summary>
        public string ReceiptNo { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 收费日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string TotalMoney { get; set; }
        /// <summary>
        /// 业绩归属人姓名（如果有多个用逗号分隔
        /// </summary>
        public string EmployeeNames { get; set; }
        /// <summary>
        /// 收费确认的用户名
        /// </summary>
        public string ConfirmUserName { get; set; }
        /// <summary>
        /// 收费确认的时间
        /// </summary>
        public string ConfirmTime { get; set; }

        public int CompareTo(ShouRuTemp other)
        {
            return (int)(DateTimeToUnixTimestamp(Convert.ToDateTime(this.Date)) - DateTimeToUnixTimestamp(Convert.ToDateTime(other.Date)));
        }
        public long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
    }
}
