using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Models
{
    public class ProfanityException : Exception
    {
        public ProfanityException(string message, ProfanityErrorOperation errorType) : base(message)
        {
            ProfanityErrorType = errorType;
        }

        public ProfanityErrorOperation ProfanityErrorType;
    }

    public enum ProfanityErrorOperation
    {
        ProfanityRun,
        ProfanityWordList
    }
}
