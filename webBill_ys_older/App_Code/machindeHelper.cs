using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Management;

/// <summary>
/// machindeHelper 的摘要说明
/// </summary>
public class machindeHelper
{
    public machindeHelper()
    { }

    public string GetMachineGuid()
    {
        string _cpuInfo = "";//cpu信息 
        ManagementClass cimobject = new ManagementClass("Win32_Processor");
        ManagementObjectCollection moc = cimobject.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            _cpuInfo = mo.Properties["ProcessorId"].Value.ToString();

        }
        return _cpuInfo;
    }
}
