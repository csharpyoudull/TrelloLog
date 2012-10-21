using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrelloLog.Test
{
    [TestClass]
    public class LogTests
    {
        private TrelloLog Log { get; set; }

        private const string ApplicationName = "Sample App";

        public LogTests()
        {
            Log = new TrelloLog();
        }

        [TestMethod]
        public void LogInfoTest()
        {
            Log.LogInfo(ApplicationName,"LogTests::LogInfoTest", "Information message.");
        }

        [TestMethod]
        public void LogWarningTest()
        {
            Log.LogWarning(ApplicationName, "LogTests::LogWarningTest", "Warning message.");
        }

        [TestMethod]
        public void LogExceptionTest()
        {
            Log.LogException(ApplicationName, "LogTests::LogExceptionTest", new Exception("Exception message."));
        }
    }
}
