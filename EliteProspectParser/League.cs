using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteProspectParser
{
    public class League
    {
        public string Part { get; set; }
        public string Name { get; set; }
        public string href { get; set; }
        public int group { get; set; }
        public bool isChecked { get; set; }
    }
}
