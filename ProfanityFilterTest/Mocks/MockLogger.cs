using ProfanityFilter.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProfanityFilterTest.Mocks
{
    public class MockLogger : IProfanityLogger
    {
        private List<string> logs;

        public MockLogger()
        {
            logs = new List<string>();
        }
        public void Log(string message)
        {
            logs.Add(message);
        }
    }
}
