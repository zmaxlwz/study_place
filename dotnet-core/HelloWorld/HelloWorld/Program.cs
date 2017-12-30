//----------
// some copyright information
//----------

namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using HelloWorld.Lib;

    class Program
    {
        private int num = 5;
        private Process currentProcess;
        private Timer processCounterMonitorTimer;
        private const int counterMonitorTimerInterval = 5000;

        public Program()
        {
            // this.currentProcess = Process.GetCurrentProcess();
            this.currentProcess = new Process()
            {
                StartInfo = new ProcessStartInfo("C:\\Windows\\Notepad.exe")
            };

            this.processCounterMonitorTimer = new Timer(this.ProcessCountersMonitor, null, Timeout.Infinite, Timeout.Infinite);
        }

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
            /*
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

            //TestUtils.GetTestResult();

            //TestUtils.GetFileNames();

            TestUtils.CreateTimeFolder();

            string host = "https://10.25.245.4";
            var uri = new Uri(host);
            var applicationEndPoint = uri.Host;
            Console.WriteLine("uri: {0}, appEndPoint: {1}", uri, applicationEndPoint);

            string name = "10.25.3.1";
            Console.WriteLine("prev: {0}, after: {1}", name, name.Replace(':', '-'));

            Console.WriteLine("https://serviceprober.asgfalcon.io" + "/keepalive" + "?setapplicationendpoint=" + name);

            var newURI = new Uri("https://serviceprober.asgfalcon.io/keepalive/?setapplicationendpoint=23.12.45.13");
            Console.WriteLine(newURI.AbsolutePath);

            string str1 = "staging";
            string str2 = null;
            Console.WriteLine(string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase));

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://serviceprober.asgfalcon.io");
            var response = client.GetAsync("keepalive?setapplicationendpoint=23.12.45.13").Result;
            Console.WriteLine("status: {0}, content: {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "keepalive");
            Console.WriteLine("request uri: {0}, hostHeader: {1}", requestMessage.RequestUri, requestMessage.Headers.Host);

            Uri baseUri = new Uri("http://www.contoso.com/");
            Uri myUri = new Uri(baseUri, "catalog/shownew.htm?foo=bar");
            Console.WriteLine("query: {0}, is null: {1}, is empty: {2}", myUri.Query, myUri.Query == null, string.Equals(myUri.Query, string.Empty));
            Console.WriteLine(myUri.AbsolutePath);
            Console.WriteLine(myUri.PathAndQuery);

            var relativeUri = new Uri("/keepalive", UriKind.Relative);
            Console.WriteLine(relativeUri.ToString());

            Console.WriteLine(Environment.GetEnvironmentVariable("hosting__environment") == null);

            string strnull = "my";
            Console.WriteLine("true of false: {0}", strnull is string);
            */

            /*
            Program pg = new Program();
            pg.currentProcess.Start();
            Console.WriteLine("process ID: {0}", pg.currentProcess.Id);
            pg.StartMonitoring();
            pg.currentProcess.WaitForExit();
            */

            HttpStatusCode? status1 = HttpStatusCode.Accepted;
            HttpStatusCode status2 = status1 ?? throw new ArgumentException("status is null.");
            Console.WriteLine("The status has value: {0}, the value is: {1}", status1.HasValue, status2);

            var value = LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE | LimitFlags.JOB_OBJECT_LIMIT_JOB_MEMORY;
            Console.WriteLine("flag type: {0}, value is: {1}", value.GetType(), value);

            Console.WriteLine("sizeof(int) is {0}", sizeof(int));
 
            List<int> list = new List<int>(){1,2,3};
            Console.WriteLine("The list is: {0}", string.Join(",", list));

#if DEBUG
            Console.WriteLine("in debug mode.");
#else
            Console.WriteLine("in release mode.");
#endif

            double dnum = 2000.5413;
            Console.WriteLine("round result: {0}", Math.Round(dnum));
            Console.WriteLine("convert to ulong is: {0}", (ulong)dnum);

            long lnum = 1;
            var result = lnum * 1.0 / 129836;
            Console.WriteLine("The result is {0}, result is greater than 0: {1}", result, result > 0);

            // CheckCertificateToExpire();
            // CheckRequiredCertificate();
            /*
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://helloworld.asgfalcon.io/keepalive").Result;
            Console.WriteLine("The status code is {0}.", response.StatusCode);
            var httpContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("HttpContent is: {0}.", httpContent);
            */

            SampleInterface sc = new SampleClass();
            Console.WriteLine(sc.Age);
            sc.PrintName(10);
            Console.WriteLine(sc.Age);

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        private static void CheckRequiredCertificate()
        {
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var storeCertificateCollection = store.Certificates;
                var thumbprint = "b552d7ae54e86551e31e270598cf596d70537a14";

                try
                {
                    var foundCertificateCollection =
                        storeCertificateCollection.Find(X509FindType.FindByThumbprint, thumbprint, false);
                    if (foundCertificateCollection == null || foundCertificateCollection.Count == 0)
                    {
                        var description = string.Format(CultureInfo.InvariantCulture, "The following required certificates are not installed: ");
                        description += string.Format(CultureInfo.InvariantCulture, " ({0} : {1}) ", "cert thumprint", thumbprint);
                        Console.WriteLine(description);
                    }
                    else
                    {
                        Console.WriteLine("All required certificates have already been installed.");
                        var certificate = foundCertificateCollection[0];
                        using (var chain = new X509Chain())
                        {
                            chain.Build(certificate);
                            // Console.WriteLine("Chain policy: {0}", chain.ChainPolicy);
                            foreach (var status in chain.ChainStatus)
                            {
                                Console.WriteLine("Chain status: {0} -- {1}", status.Status, status.StatusInformation);
                            }

                            foreach (var element in chain.ChainElements)
                            {
                                Console.WriteLine("Chain element information: {0}", element.Information);
                                foreach (var status in element.ChainElementStatus)
                                {
                                    Console.WriteLine("element status: {0} -- {1}", status.Status, status.StatusInformation);
                                }
                                Console.WriteLine("Chain element certificate: {0}", element.Certificate);
                            }                          
                        }
                    }
                }
                catch (CryptographicException e)
                {
                    var desc = string.Format(
                        CultureInfo.InvariantCulture,
                        "Exception while trying to find the certificate by Thumbprint from the LocalMachine Store: {0}.",
                        e);
                    Console.WriteLine(desc);
                }
            }
        }

        private static void CheckCertificateToExpire()
        {
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates;
                var currentDateTime = DateTime.Now;
                var certificateToExpireList = new List<X509Certificate2>();
                foreach (var certificate in certCollection)
                {
                    var certExpirationTime = certificate.NotAfter;
                    var alertTime = certExpirationTime.AddDays(-90);
                    if (currentDateTime.CompareTo(alertTime) >= 0 && currentDateTime.CompareTo(certExpirationTime) < 0)
                    {
                        certificateToExpireList.Add(certificate);
                    }
                }

                if (certificateToExpireList.Any())
                {
                    var description = string.Format(CultureInfo.InvariantCulture, "The following certificates are to expire: ");
                    foreach (var certificate in certificateToExpireList)
                    {
                        description += string.Format(CultureInfo.InvariantCulture, " ({0} : {1}) ", certificate.FriendlyName, certificate.Thumbprint);
                    }

                    Console.WriteLine(description);
                }
                else
                {
                    Console.WriteLine("No certificates are to expire.");
                }
            }
        }

        private void StartMonitoring()
        {           
            this.processCounterMonitorTimer.Change(0, counterMonitorTimerInterval);
        }

        private void ProcessCountersMonitor(object state)
        {
            this.currentProcess.Refresh();

            // set the total CPU time counter in seconds for the process
            Console.WriteLine("Console: CPU time usage {0} milliseconds", (ulong)this.currentProcess.TotalProcessorTime.TotalMilliseconds);

            // set the amount of physical memory usage in KB for the process
            Console.WriteLine("Console: physical memory usage: {0} KB.", (ulong)(this.currentProcess.WorkingSet64 / 1024));
        }
    }

    [Flags]
    internal enum LimitFlags
    {
        JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
        JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
        JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
        JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
        JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
        JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
        JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,
        JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
        JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
        JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
        JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
        JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
        JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
        JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001
    }

}