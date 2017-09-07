//----------
// some copyright information
//----------

namespace HelloWorld
{
    using System;
    using System.IO;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using HelloWorld.Lib;

    class Program
    {
        private int num = 5;

        static void checkString(string str)
        {
            if (str.Length < 3)
            {
                Console.WriteLine("less than 3");
            }
            else
            {
                Console.WriteLine(">= 3");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(nameof(Program));
            Console.WriteLine(nameof(Program.Main));
            Console.WriteLine(nameof(num));
            int? num1 = null;
            int num2 = num1 ?? - 2;
            Console.WriteLine("num1: {0}, num2: {1}", num1, num2);

            string str = string.Empty;
            Console.WriteLine("value: {0}, Any: {1}.", str.Length, str.Any());
            str = "hello";
            Console.WriteLine("value: {0}, Any: {1}.", str.Length, str.Any());

            checkString(401.ToString(CultureInfo.InvariantCulture));

            IHostingEnvironment hostingEnvironment = new HostingEnvironment(){EnvironmentName = EnvironmentName.Development};
            Console.WriteLine("Hosting Environment is Development: {0}", hostingEnvironment.IsDevelopment());

            Console.WriteLine("null value is : {0}", null);

            var url = TestUtils.GetHostUrl();
            Console.WriteLine("The URL is: {0}", url);

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine("Environment is : {0}", env);

            //TestUtils.CreateFunctionalXmlElement();

            TestUtils.GetTestResult();
            
            /*
            string p1 = @"c:\";
            string p2 = "workplace";
            string p3 = "NightWatchTestResult.trx";
            string filePath = Path.Combine(p1, p2, p3);
            Console.WriteLine(filePath);

            string text = File.ReadAllText(filePath);
            Console.WriteLine("Contents of the result file are: \n {0}", text);
            */

            /*
            Console.WriteLine("\r\nExists Certs Name and Location");
            Console.WriteLine("------ ----- -------------------------");

            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine("The current time is {0}.", currentDateTime);
            // Console.WriteLine("The current date is {0}.", currentDateTime.Date);
            // DateTime weekago = currentDateTime.AddDays(-7);
            // Console.WriteLine("The time a week ago is {0}.", weekago);

            var daysToExpire = 7;
            
            foreach (StoreName storeName in (StoreName[])
                Enum.GetValues(typeof(StoreName)))
            {
                using (X509Store store = new X509Store(storeName, StoreLocation.LocalMachine))
                {
                    // store.Open(OpenFlags.OpenExistingOnly);
                    store.Open(OpenFlags.ReadOnly);

                    X509Certificate2Collection certCollection = store.Certificates;

                    var count = 0;
                    foreach (var cert in certCollection)
                    {
                        count++;
                        // this is the expiration time in local time
                        DateTime certExpirationTime = cert.NotAfter;
                        DateTime alertTime = certExpirationTime.AddDays(-daysToExpire);
                        if (currentDateTime.CompareTo(alertTime) >= 0)
                        {
                            Console.WriteLine("cert {0}, expiration time: {1}, has expired or is to be expired. | {2} | {3}", count, certExpirationTime, cert.FriendlyName, cert.Thumbprint);
                        }
                    }

                    // Console.WriteLine("Yes    {0,4}  {1}, {2}", certCollection.Count, store.Name, store.Location);
                    Console.WriteLine("Yes    {0,4}  {1}, {2}", count, store.Name, store.Location);

                }
            }
            Console.WriteLine();
            */

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }

}