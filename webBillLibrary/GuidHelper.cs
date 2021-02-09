using System;
using System.Collections.Generic;
using System.Text;

namespace webBillLibrary
{
    public class GuidHelper
    {
        public GuidHelper()
        {
        }
        public string getNewGuid()
        {
            System.Guid guid = System.Guid.NewGuid();
            return guid.ToString().ToUpper();
        }
    }
}
