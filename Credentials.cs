using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper
{
    public class Credentials : Object
    {
        private String sUserName;
        private String sPassword;
        private String sDomain;
        private bool bAuthenticated;

        public String UserName
        {
            get { return sUserName; }
            set { sUserName = value; }
        }

        public String Password
        {
            get { return sPassword; }
            set { sPassword = value; }
        }

        public String Domain
        {
            get { return sDomain; }
            set { sDomain = value; }
        }

        public bool Authenticated
        {
            get { return bAuthenticated; }
            set { bAuthenticated = value; }
        }
    }
}
