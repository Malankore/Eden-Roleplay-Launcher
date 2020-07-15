using System;
using System.Collections.Generic;
using System.Text;

namespace Eden_Roleplay.Model
{
    public class ChangeLog
    {
        private string _title;
        private string _details;
        private DateTime _date;

        public string Title { 
            get {
                return _title;
            }
            set 
            {
                _title = value;
            } 
        }

        public string Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }
    }
}
