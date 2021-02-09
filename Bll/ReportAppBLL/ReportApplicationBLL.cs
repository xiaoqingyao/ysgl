using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;

namespace Bll.ReportAppBLL
{
  
    public class ReportApplicationBLL
    {
        Dal.ReportApplication.ReportApplicationDal dal = new Dal.ReportApplication.ReportApplicationDal();

        public DataTable getalltable(T_ReportApplication model)
        {
          
            return dal.getallmode(model);
     
        }
        /// <summary>
        /// 得以实体类
        /// </summary>
        /// <returns></returns>
        public T_ReportApplication getmode(string strcode) 
        {
            return dal.GetModel(strcode);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public int Addmodel(T_ReportApplication model) 
        {

            return dal.Add(model);
        }

        public int Delemode(string strcode) 
        {
            return dal.Delete(strcode);
        }

    }
}
