using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace CimPointConv.Tests
{
    [TestClass()]
    public class ProcessorTests
    {
        [TestMethod()]
        public void RegexReplaceTest()
        {
            var regex = new Regex(@"(DB\d+\.DB.)(\d+)");
            
            Assert.AreEqual("DB123.DBB11", Processor.RegexReplace(regex,"DB123.DBB1","${1}<$2+10>"));
            Assert.AreEqual("DB123.DBB555", Processor.RegexReplace(regex, "DB123.DBB1", @"${1}555"));

            regex = new Regex(@"I(\d+)");
            Assert.AreEqual("I201", Processor.RegexReplace(regex, "I151", @"I<$1+50>"));
        }
    }
}