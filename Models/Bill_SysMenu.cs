using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_SysMenu
    {


        

        private string menuId;

        public string MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        private string menuName;
        /// <summary>
        /// 菜单名（不允许用户修改的）
        /// </summary>
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        
      
        /// <summary>
        ///用于显示的菜单名(允许用户修改的)
        /// </summary>
        public string ShowName
        {
            get;
            set;
        }
        private string menuUrl;

        public string MenuUrl
        {
            get { return menuUrl; }
            set { menuUrl = value; }
        }
        private string menuOrder;

        public string MenuOrder
        {
            get { return menuOrder; }
            set { menuOrder = value; }
        }
        private string menuSm;

        public string MenuSm
        {
            get { return menuSm; }
            set { menuSm = value; }
        }
        private string menuState;

        public string MenuState
        {
            get { return menuState; }
            set { menuState = value; }
        }
    }
}
