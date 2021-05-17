using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Core
{
    public class ProfanityResult
    {
        public string Text { get; set; }
        public bool IsProfane { get; set; }
        public int NumberOfProfaneWords { get; set; }
        public long ProcessTimeInMilliseconds { get; set; }
        public List<string> ProfaneWords { get; set; }
    }
}
