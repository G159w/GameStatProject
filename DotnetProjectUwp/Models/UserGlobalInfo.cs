using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetProjectUwp.Models.GamesInfo;

namespace DotnetProjectUwp.Models
{
    public class UserGlobalInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<GameInfo> Games { get; set; }

    }
}
