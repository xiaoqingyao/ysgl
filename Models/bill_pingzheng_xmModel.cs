using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 凭证项目
    /// </summary>
    public class bill_pingzheng_xmModel
    {
        public bill_pingzheng_xmModel()
        { }
        #region Model
        private int _list_id;
        private string _xmcode;
        private string _xmname;
        private string _parentcode;
        private string _parentname;
        private string _isdefault;
        private string _status;
        private string _note1;
        private string _note2;
        private string _note3;
        private string _note4;
        private string _note5;
        private string _note6;
        private string _note7;
        private string _note8;
        private string _note9;
        /// <summary>
        /// 
        /// </summary>
        public int list_id
        {
            set { _list_id = value; }
            get { return _list_id; }
        }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string xmCode
        {
            set { _xmcode = value; }
            get { return _xmcode; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string xmName
        {
            set { _xmname = value; }
            get { return _xmname; }
        }
        /// <summary>
        /// 上级编号
        /// </summary>
        public string parentCode
        {
            set { _parentcode = value; }
            get { return _parentcode; }
        }
        /// <summary>
        /// 上级名称
        /// </summary>
        public string parentName
        {
            set { _parentname = value; }
            get { return _parentname; }
        }
        /// <summary>
        /// 是否默认 1-是 0-否
        /// </summary>
        public string isDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }
        /// <summary>
        /// 状态 1启用 0 禁用
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 项目大类编号
        /// </summary>
        public string Note1
        {
            set { _note1 = value; }
            get { return _note1; }
        }
        /// <summary>
        /// 项目编码 
        /// </summary>
        public string Note2
        {
            set { _note2 = value; }
            get { return _note2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note3
        {
            set { _note3 = value; }
            get { return _note3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note4
        {
            set { _note4 = value; }
            get { return _note4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note5
        {
            set { _note5 = value; }
            get { return _note5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note6
        {
            set { _note6 = value; }
            get { return _note6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note7
        {
            set { _note7 = value; }
            get { return _note7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note8
        {
            set { _note8 = value; }
            get { return _note8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note9
        {
            set { _note9 = value; }
            get { return _note9; }
        }
        #endregion Model
    }
}
