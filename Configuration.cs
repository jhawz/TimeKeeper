using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper
{
    public static class Configuration
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string Domain { get; set; }
        public static string SharepointUrl { get; set; }
        public static bool Authenticated { get; set; }
    }
}
