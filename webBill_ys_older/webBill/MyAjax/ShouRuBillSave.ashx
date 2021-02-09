<%@ WebHandler Language="C#" Class="ShouRuBillSave" %>

using System;
using System.Web;

public class ShouRuBillSave : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}