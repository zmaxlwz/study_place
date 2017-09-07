

namespace HelloWorld.Lib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Newtonsoft.Json.Linq;

    internal class TestUtils
    {
        private static string configPath = @".\Config";
        private static string testResultPath = @".\TestResults";

        public static string GetHostUrl()
        {
            string configFile = Path.Combine(configPath, "default.json");
            if (File.Exists(configFile))
            {
                var result = JObject.Parse(File.ReadAllText(configFile));
                var hostInfo = result["Host"];
                return hostInfo["Url"].Value<string>();
            }
            else
            {
                Console.WriteLine("The config file doesn't exists. ");
                return string.Empty;
            }
        }

        public static void GetTestResult()
        {
            //string testResultFile = Path.Combine(testResultPath, "PurchaseOrder.xml");
            string testResultFile = Path.Combine(testResultPath, "NightWatchTestResult_failed.trx");

            if (File.Exists(testResultFile))
            {
                var docString = File.ReadAllText(testResultFile);
                XElement doc = XElement.Parse(docString);
                // XElement doc = XElement.Load(testResultFile);
                //Console.WriteLine(doc);
                /*
                var testResults = from item in doc.Descendants("UnitTestResult")
                    select (string) item.Attribute("testName");
                */

                /*
                IEnumerable<XElement> address =
                    from el in doc.Elements("Address")
                    where (string)el.Attribute("Type") == "Billing"
                    select el;
                */
                /*
                var address = doc.Elements("Address");
                foreach (XElement el in address)
                    Console.WriteLine(el);
                */

                XNamespace vsTeamTest = doc.Attribute("xmlns").Value;
                var totalNumTest = (int) doc.Element(vsTeamTest + "ResultSummary").Element(vsTeamTest + "Counters").Attribute("total");
                var passedTestNum = (int) doc.Element(vsTeamTest + "ResultSummary").Element(vsTeamTest + "Counters").Attribute("passed");
                var failedTestNum = (int) doc.Element(vsTeamTest + "ResultSummary").Element(vsTeamTest + "Counters").Attribute("failed");
                Console.WriteLine("The total number of tests is: {0}", totalNumTest);
                Console.WriteLine("The number of passed tests is: {0}", passedTestNum);
                Console.WriteLine("The number of failed tests is: {0}", failedTestNum);

                var testResults = doc.Element(vsTeamTest + "Results");
                var passedTestResults =
                    from testResult in testResults?.Elements(vsTeamTest + "UnitTestResult")
                    where (string) testResult.Attribute("outcome") == "Passed"
                    select testResult;
                Console.WriteLine("The passed test names: ");
                foreach (var testResult in passedTestResults)
                {
                    Console.WriteLine((string)testResult.Attribute("testName"));
                }

                var failedTestResults =
                    from testResult in testResults?.Elements(vsTeamTest + "UnitTestResult")
                    where (string)testResult.Attribute("outcome") == "Failed"
                    select testResult;
                Console.WriteLine("The failed test names: ");
                foreach (var testResult in failedTestResults)
                {
                    var testName = (string)testResult.Attribute("testName");
                    var testOutcome = (string)testResult.Attribute("outcome");
                    var message = testResult.Element(vsTeamTest + "Output")?.Element(vsTeamTest + "ErrorInfo")?.Element(vsTeamTest + "Message")?.Value ??
                                  "Failed to extract error message";
                    var stack = testResult.Element(vsTeamTest + "Output")?.Element(vsTeamTest + "ErrorInfo")?.Element(vsTeamTest + "StackTrace")?.Value ??
                                "Failed to extract StackTrace message";
                    Console.WriteLine("{0} : {1}", testName, testOutcome);
                    Console.WriteLine("Error Message : \n {0}", message);
                    Console.WriteLine("Stack Trace : \n {0}", stack);
                }


                /*
                var testResults = doc.Element(vsTeamTest + "Results").Elements(vsTeamTest + "UnitTestResult");
                int count = 0;
                Console.WriteLine("start output...");
                foreach (var testResult in testResults)
                {
                    count++;
                    Console.WriteLine(testResult);
                }
                Console.WriteLine("count value is: {0}", count);
                */


            }
            else
            {
                Console.WriteLine("The test result file doesn't exists. ");
            }
        }

        public static void CreateFunctionalXmlElement()
        {
            XElement inventory = new XElement(
                "Inventory", 
                new XElement("Car", 
                             new XAttribute("ID", "1"),
                             new XElement("Color", "Green"),
                             new XElement("Make", "BMW"),
                             new XElement("PetName", "Stan")
                             )
            );
            Console.WriteLine(inventory);

            /*  the generated XML is like:
             *  
              <Inventory>
                 <Car ID="1">
                     <Color>Green</Color>
                     <Make>BMW</Make>
                     <PetName>Stan</PetName>
                 </Car>
              </Inventory>
            */

        }
    }
}
