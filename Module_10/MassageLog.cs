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
    class MassageLog
    {
        private string time;
        private string msg;
        private long id;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }


        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public MassageLog() 
        { 
        }
        
        public MassageLog(string Time, string Msg, long Id)
        {
            this.Time = Time;
            this.Msg = Msg;
            this.Id = Id;
        }
    }
}
