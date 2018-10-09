using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetProjectUwp.Models.GamesInfo;

namespace DotnetProjectUwp.Models
{
    public class UserRank
    {
        public string Rank { get; set; }

        public string MainAccount { get; set; }

        public GameInfo Game { get; set; }
    }
}
