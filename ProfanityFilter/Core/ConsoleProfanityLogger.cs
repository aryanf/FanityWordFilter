using ProfanityFilter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Core
{
    public class ConsoleProfanityLogger : IProfanityLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
