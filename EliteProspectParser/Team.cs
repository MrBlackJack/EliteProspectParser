using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteProspectParser
{
    public class Team
    {
        public string name { get; set; }
        public string nameRus { get; set; }
        public League league { get; set; }
        public string href { get; set; }
        public string urlLogo { get; set; }
        public string arena { get; set; }
    }
}
