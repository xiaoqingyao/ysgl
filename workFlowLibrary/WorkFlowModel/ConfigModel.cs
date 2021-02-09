using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowModel
{
    public class ConfigModel
    {
        private string typecode;

        public string Typecode
        {
            get { return typecode; }
            set { typecode = value; }
        }
        private string tabname;

        public string Tabname
        {
            get { return tabname; }
            set { tabname = value; }
        }
        private string filter;

        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }
        private string codecolum;

        public string Codecolum
        {
            get { return codecolum; }
            set { codecolum = value; }
        }
        private string usercolum;

        public string Usercolum
        {
            get { return usercolum; }
            set { usercolum = value; }
        }
        private string typename;

        public string Typename
        {
            get { return typename; }
            set { typename = value; }
        }
    }
}
