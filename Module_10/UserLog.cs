using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Module_10
{
    class UserLog
    {
        private string time;
        private long id;
        private string msg;
        private string firstName;

        public string Time
        {
            get { return time; } 
            set { time = value; } 
        }

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        

        public UserLog()
        {

        }
        public UserLog(string Time, string Msg, string FirstName, long Id)
        {
            this.Time = Time;
            this.Msg = Msg;
            this.FirstName = FirstName;
            this.Id = Id;
        }
    }
}
