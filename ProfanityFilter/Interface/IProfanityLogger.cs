using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Interface
{
    public interface IProfanityLogger
    {
        public void Log(string message);
    }
}
