using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_10
{
    class FilesLog
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public FilesLog()
        {

        }
        public FilesLog(string Name)
        {
            this.Name = Name;
        }
    }
}

