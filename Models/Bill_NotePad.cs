using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_NotePad
    {
        private string userCode;

        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }
        private DateTime noteDate;

        public DateTime NoteDate
        {
            get { return noteDate; }
            set { noteDate = value; }
        }
        private string context;

        public string Context
        {
            get { return context; }
            set { context = value; }
        }
        private string noteType;

        public string NoteType
        {
            get { return noteType; }
            set { noteType = value; }
        }
    }
}
