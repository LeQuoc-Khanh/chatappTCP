using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatapp.Models
{
    public class StatusDataModel
    {
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public Uri StatusImage { get; set; }
        public bool IsMeAddSatus { get; set; }
        //public string StatusMessage { get; set; }

    }
}
