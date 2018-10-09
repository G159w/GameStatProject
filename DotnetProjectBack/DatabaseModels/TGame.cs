using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TGame
    {
        public TGame()
        {
            TUserGame = new HashSet<TUserGame>();
        }

        public long Id { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public bool ApiKeyRequired { get; set; }

        public ICollection<TUserGame> TUserGame { get; set; }
    }
}
