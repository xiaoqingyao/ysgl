<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        
        //bool boControlPoint = new Bll.ConfigBLL().GetValueByKey("ISControlPoint").Equals("1");
        //if (boControlPoint)
        //{
        //    //在应用程序启动时运行的代码
        //    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(refreshUserOnlinetime));
        //    thread.Name = "定时清理下线人员";
        //    thread.Start();
        //}
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码
        Session.Timeout = 200; 
    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
    /// <summary>
    /// 定时踢出下线了的人员
    /// </summary>
    void refreshUserOnlinetime()
    {
        //DateTime nowTime = DateTime.Now;
        //System.Data.DataTable dtRel = new Bll.OnlineBLL().GetAll();
        //for (int i = 0; i < dtRel.Rows.Count; i++)
        //{
        //    System.Data.DataRow dr = dtRel.Rows[i];
        //    string currentCode = dr["usercode"].ToString();
        //    string lasttime = dr["lastonlinetime"].ToString();
        //    DateTime lastTime = DateTime.Parse(lasttime);
        //    if (lastTime.AddSeconds(40) < nowTime)
        //    {
        //        new Bll.OnlineBLL().UnOnlineUser(currentCode);
        //    }
        //}
        while (true)
        {
            //检测是否存在狗
           int icount = new Bll.OnlineBLL().GetMaxOnlineCount();
            if (icount <= 0)
            {
                //Session.Abandon();
                Session.Abandon();
            }
            else
            {
                new Bll.OnlineBLL().refresh();
            }
            System.Threading.Thread.CurrentThread.Join(1000 * 200);
        }

    }
    
</script>

