using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace Dal.SysDictionary
{
    public class MenuDal
    {
        public IList<Bill_SysMenu> GetMenuAll()
        {
            string sql = "select * from bill_sysMenu order by menuid where menustate<>'D'";
            return ListMaker(sql, null); 
        }

        public IList<Bill_SysMenu> GetMenuByUser(string userCode)
        {
            string sql = @" select * from bill_sysMenu where menuid in
                            (select objectID from dbo.bill_userRight where righttype = '1' and userCode=@userCode) and isnull(menustate,0)!='D'
                            order by menuid";
     
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(sql, sps);
        }

        public IList<Bill_SysMenu> GetMenuByRole(string roleCode)
        {
            string sql = @" select * from bill_sysMenu where menuid in
                            (select objectID from dbo.bill_userRight where righttype = '3' and userCode=@userCode)  and isnull(menustate,0)!='D'
                            order by menuid";

            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",roleCode)
                                 };
            return ListMaker(sql, sps);
        }

        public IList<Bill_SysMenu> GetMenuByUserAll(string userCode)
        {
            string sql = @" select * from bill_sysMenu where menuid in
                            (select objectID from dbo.bill_userRight where userCode=@userCode)  and isnull(menustate,0)!='D'
                            order by menuorder";

            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(sql, sps);
        }



        public IList<Bill_SysMenu> GetMenuRoot()
        {
            string sql = @"select * from bill_sysMenu where len(menuid)=2  and isnull(menustate,0)!='D' order by menuorder";

            return ListMaker(sql, null);
        }


        private IList<Bill_SysMenu> ListMaker(string sql,SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_SysMenu> list = new List<Bill_SysMenu>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_SysMenu menu = new Bill_SysMenu();
                menu.MenuId = Convert.ToString(dr["MenuId"]);
                menu.MenuName = Convert.ToString(dr["MenuName"]);
                menu.ShowName = Convert.ToString(dr["ShowName"]);
                menu.MenuOrder = Convert.ToString(dr["MenuOrder"]);
                menu.MenuSm = Convert.ToString(dr["MenuSm"]);
                menu.MenuState = Convert.ToString(dr["MenuState"]);
                menu.MenuUrl = Convert.ToString(dr["MenuUrl"]);
                list.Add(menu);
            }
            return list;
        }
    }
}
