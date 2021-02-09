using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

namespace my_fzl
{
    /// <summary>
    /// bindClss 的摘要说明
    /// </summary>
    public class bindClss : Page
    {  
        public SqlDataReader myReader;
        public SqlDataAdapter myAdapter;
        public SqlCommand myCommand;
        public DataSet myDataSet;
        SqlConnection myCon = null;

        public bindClss()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            myCon = new  SqlConnection(this.GetConnStr());
        }

        #region 得到数据库的连接字符串
        /// <summary>
        /// 得到数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnStr()
        {
            //需要得到一个数据库的连接字符串
            string strConn = ConfigurationSettings.AppSettings["ConnectionStringvUnionDataBase"];
            return strConn;
        }
        #endregion

        #region 绑定ddl并选定默认
        /// <summary>
        /// 根据选定的值绑定下拉框
        /// </summary>
        /// <param name="Htext">显示值</param>
        /// <param name="Hvalue">value值</param>
        /// <param name="HdropList">下拉框</param>
        /// <param name="Hsql">sql</param>
        /// <param name="Htablename">表</param>
        /// <param name="CsValue">默认选定值</param>
        public void assetDropDownListBindSelected(string Htext, string Hvalue, System.Web.UI.WebControls.DropDownList HdropList, string Hsql, string Htablename, string CsValue)
        {
            DataSet myDs = new DataSet();
            SqlDataAdapter myAdapter = new SqlDataAdapter(Hsql, myCon);
            string space = "";
            try
            {
                myCon.Open();
                myAdapter.Fill(myDs, Htablename);
                HdropList.Items.Clear();
                if (myDs.Tables[0].Rows.Count > 0)
                {
                    for (int y = 0; y < myDs.Tables[0].Rows.Count; y++)
                    {
                        ListItem Templi = new ListItem();
                        if (HdropList.ID.IndexOf("jfkm") >= 0 || HdropList.ID.IndexOf("syfx") >= 0)
                        {
                            if (myDs.Tables[0].Rows[y][Hvalue].ToString().Trim().Length >= 4)
                            {
                                for (int s = 4; s <= myDs.Tables[0].Rows[y][Hvalue].ToString().Trim().Length; s = s + 2)
                                {
                                    space = "　" + space;
                                }
                                Templi.Text = space + myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                                Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                                space = "";
                            }
                            else
                            {
                                Templi.Text = myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                                Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                            }
                        }
                        else
                        {
                            Templi.Text = myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                            Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                        }
                        HdropList.Items.Add(Templi);
                        if (myDs.Tables[0].Rows[y][Hvalue].ToString().Trim() == CsValue)
                        {
                            Templi.Selected = true;
                        }
                    }
                    //					HdropList.DataSource=myDs.Tables[0].DefaultView;
                    //					HdropList.DataTextField=myDs.Tables[0].Columns[Htext].ToString().Trim();
                    //					HdropList.DataValueField=myDs.Tables[0].Columns[Hvalue].ToString().Trim();
                    //					HdropList.DataBind();
                }
                myAdapter.Dispose();
                myDs.Dispose();
            }
            catch
            {
                RegisterStartupScript("btn", "<script language=\"javascript\">alert('" + this.GetErrorDis() + "');</" + "script>");
            }
            finally
            {
                myCon.Close();
            }
        }

        #endregion

        #region 绑定ddl
        /// <summary>
        /// 根据sql语句绑定代码的下拉信息
        /// </summary>
        /// <param name="HdropList"></param>
        /// <param name="Hsql"></param>
        public void assetDropDownListBind(string Htext, string Hvalue, System.Web.UI.WebControls.DropDownList HdropList, string Hsql, string Htablename)
        {
            if (Hsql.IndexOf("order by") < 0 && Hsql.IndexOf("databak") < 0)
            {
                Hsql += " order by diccode asc" ;
            }
            HdropList.Items.Clear();
            DataSet myDs = this.GetDataSet(Hsql);
            string space = "";
            try
            {
                if (myDs.Tables[0].Rows.Count > 0)
                {

                    for (int y = 0; y < myDs.Tables[0].Rows.Count; y++)
                    {
                        ListItem Templi = new ListItem();
                        if (HdropList.ID.IndexOf("jfkm") >= 0 || HdropList.ID.IndexOf("syfx") >= 0 || HdropList.ID.IndexOf("drp_djh_7") >= 0)
                        {
                            if (myDs.Tables[0].Rows[y][Hvalue].ToString().Trim().Length >= 4)
                            {
                                for (int s = 4; s <= myDs.Tables[0].Rows[y][Hvalue].ToString().Trim().Length; s = s + 2)
                                {
                                    space = "　" + space;
                                }
                                Templi.Text = space + myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                                Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                                space = "";
                            }
                            else
                            {
                                Templi.Text = myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                                Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                            }
                        }
                        else
                        {
                            Templi.Text = myDs.Tables[0].Rows[y][Htext].ToString().Trim();
                            Templi.Value = myDs.Tables[0].Rows[y][Hvalue].ToString().Trim();
                        }
                        HdropList.Items.Add(Templi);
                    }
                }
                myAdapter.Dispose();
                myDs.Dispose();
            }
            catch (Exception ex)
            {
                RegisterStartupScript("btn", "<script language=\"javascript\">alert('" + this.GetErrorDis() + "');</" + "script>");
            }
            finally
            {
            }
        }

        #endregion

        #region 得到错误的提示
        /// <summary>
        /// 得到数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetErrorDis()
        {
            string strConn = "分页大小输入有误！";// "系统出现异常，存在的原因可能是：" + "\\n\\n" + "数据库连接有误！" + "\\n\\n" + "加密狗有误！" + "\\n\\n" + "系统的审核流程没有设置！" + "\\n\\n" + "系统初始化过程中出现了异常！" + "\\n\\n" + "出现了其他不可预知的错误！" + "\\n\\n" + "请与管理员联系！";
            //try
            //{
            //    TextBox txtsize = (TextBox)Page.FindControl("txt_size");
            //    int.Parse(txtsize.Text);
            //    if(int.Parse(txtsize.Text)>30000)
            //    {
            //        strConn="f";
            //    }
            //    else
            //    {
            //        strConn = "";
            //    }
            //}
            //catch
            //{ 
            //    strConn="分页大小输入有误！";
            //}
            return strConn;
        }
        #endregion

        #region 根据Hsql语句得到查询的数据个数，返回dataset
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hsql"></param>
        public int GetRecordCount(string Hsql, string Htablename)
        {
            int returnStr = 0;

            try
            {
                if (myCon.State == ConnectionState.Closed)
                {
                    myCon.Open();
                }
                DataSet myDs = new DataSet();
                SqlDataAdapter myAdapter = new SqlDataAdapter(Hsql, myCon);
                myAdapter.Fill(myDs, Htablename);
                returnStr = myDs.Tables[0].Rows.Count;
                myAdapter.Dispose();
                myDs.Dispose();
                myCon.Close();
            }
            catch (Exception ex)
            {
                returnStr = 0;
            }
            return returnStr;
        }

        /// <summary>
        /// 根据sql语句返回DataSet
        /// </summary>
        /// <param name="Hsql"></param>
        /// <param name="Htablename"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string Hsql, string Htablename)
        {
            if (myCon.State == ConnectionState.Closed)
            {
                myCon.Open();
            }
            DataSet myDs = new DataSet();
            SqlDataAdapter myAdapter = new SqlDataAdapter(Hsql, myCon);
            myAdapter.Fill(myDs, Htablename);
            myCon.Close();
            return myDs;
        }
        public DataSet GetDataSet(string Hsql)
        {
            if (myCon.State == ConnectionState.Closed)
            {
                myCon.Open();
            }
            DataSet myDs = new DataSet();
            SqlDataAdapter myAdapter = new SqlDataAdapter(Hsql, myCon);
            myAdapter.Fill(myDs);
            myCon.Close();
            return myDs;
        }
                #endregion
         
        #region 转换金额成大写 
        
        /**/
        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /**/
        /// <summary> 
        /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num) 
        /// </summary> 
        /// <param name="num">用户输入的金额，字符串形式未转成decimal</param> 
        /// <returns></returns> 
        public static string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "非数字形式！";
            }
        } 

        #endregion

        #region 公用存储过程分页相关操作
        /// <summary>
        /// 得到记录总数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public   int GetRecordCount2(string tableName)
        {
            return GetRecordCount(tableName, "");
        }
        /// <summary>
        /// 得到记录总数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strWhere">筛选条件（可以不用加where）</param>
        /// <returns></returns>
        public   int GetRecordCount2(string tableName, string strWhere)
        {
            string strsql = string.Empty;
            if (string.IsNullOrEmpty(strWhere))
                strsql = "select count(*) from " + tableName;
            else
                strsql = "select count(*) from " + tableName + " where " + strWhere;
            object obj = new  sqlHelper.sqlHelper().GetCellValue(strsql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 分页获取数据列表（分页）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderField">排序字段(必须!支持多字段，PS：多个字段用","分割，一般不为空，为空时默认为"ID")</param>
        /// <param name="sqlWhere">附加条件语句(不用加where)，可为空</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        /// <param name="pageIndex">指定当前为第几页</param>
        /// <param name="OrderType">排序方式，非0则降序</param>
        /// <returns></returns>
        public   DataSet GetList(string tableName, string orderField, string sqlWhere, int pageSize, int pageIndex, int OrderType)
        {
            return GetList(tableName, "*", orderField, sqlWhere, pageSize, pageIndex, OrderType);
        }


        /// <summary>
        /// 分页获取数据列表（分页）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段名(PS：可为空，为空时显示全部字段，显示全部字段为"*")</param>
        /// <param name="orderField">排序字段(必须!支持多字段，PS：多个字段用","分割，一般不为空，为空时默认为"ID")</param>
        /// <param name="sqlWhere">附加条件语句(不用加where)，可为空</param>
        /// <param name="pageSize">每页显示多少条记录</param>
        /// <param name="pageIndex">指定当前为第几页</param>
        /// <param name="OrderType">排序方式，非0则降序</param>
        /// <returns></returns>
        public   DataSet GetList(string tableName, string fields, string orderField, string sqlWhere, int pageSize, int pageIndex, int OrderType)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@tableName", SqlDbType.VarChar, 50),
					new SqlParameter("@fields", SqlDbType.VarChar, 2000),
                    new SqlParameter("@orderField", SqlDbType.VarChar, 500),
                    new SqlParameter("@sqlWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@pageSize", SqlDbType.Int),
					new SqlParameter("@pageIndex", SqlDbType.Int),
                    new SqlParameter("@orderType", SqlDbType.Int),
                    new SqlParameter("@TotalPage", SqlDbType.Int)
					};
            parameters[0].Value = tableName;
            parameters[1].Value = string.IsNullOrEmpty(fields) ? "*" : fields;
            parameters[2].Value = string.IsNullOrEmpty(orderField) ? "billdate" : orderField;
            parameters[3].Value = string.IsNullOrEmpty(sqlWhere) ? "" : sqlWhere;
            parameters[4].Value = pageSize;
            parameters[5].Value = pageIndex;
            parameters[6].Value = OrderType;
            parameters[7].Direction = ParameterDirection.Output;

            return new sqlHelper.sqlHelper().ExecuteProcedure("BILL_PROGetRecordPage", parameters);
        }
        #endregion
    }
}