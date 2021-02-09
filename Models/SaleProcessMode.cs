using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    //
    /// <summary>
    /// T_SaleProcess
    /// 预算过程表
    /// </summary>
     public   class SaleProcessMode
    {
         public SaleProcessMode() { }
            /// <summary>
            ///Code 主键
            /// </summary>	
             private string code;
            public string Code
             {
                 get { return code; }
                 set { code = value; }
             }

            /// <summary>
            ///PName 名称
            /// </summary>		
            private string pname;
            public string PName
            {
                get { return pname; }
                set {  pname=value; }
            }

            /// <summary>
            ///Status 状态
            /// </summary>	
            private string status;
            public string Status 
            {
                get { return status; }
                set {  status=value; }
            }

            /// <summary>
            ///Note1
            /// </summary>	
            private string note1;
            public string Note1
            {
                get { return note1; }
                set {  note1=value; }
            }

            /// <summary>
            ///Note2
            /// </summary>		
            private string note2;
            public string Note2
            {
                get { return note2; }
                set {  note2=value; }
            }

            /// <summary>
            ///Note3
            /// </summary>		
            private string note3;
            public string Note3
            {
                get { return note3; }
                set { note3=value; }
            }

            /// <summary>
            ///Note4
            /// </summary>		
            private string note4;
            public string Note4
            {
                get { return note4; }
                set { note4=value; }
            }

            /// <summary>
            ///Note5
            /// </summary>		
            private string note5;
            public string Note5
            {
                get { return note5; }
                set { note5=value; }
            }


        }
 
}
