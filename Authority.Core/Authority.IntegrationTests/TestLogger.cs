using System;
using System.Collections.Generic;
using System.IO;

namespace AuthorityIdentity.IntegrationTests
{
    public sealed class TestLogger : IAuthorityLogger
    {
        private static readonly string LogFile = "testLogs.txt";

        static TestLogger()
        {
            File.Create(LogFile);
        }

        public void Info(string message)
        {
            File.AppendAllLines(LogFile, new List<string> { message });
        }

        public void Error(string message)
        {
            File.AppendAllLines(LogFile, new List<string> { message });
        }

        public void Error(string message, Exception exception)
        {
            File.AppendAllLines(LogFile, new List<string> { message });
        }
    }
}
