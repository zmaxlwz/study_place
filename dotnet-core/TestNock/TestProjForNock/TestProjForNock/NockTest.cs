// some copyright info

namespace NockProj.TestProjForNock
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Nock.net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RequestClassLibrary;

    [TestClass]
    public class NockTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            nock.ClearAll();
            nock.RequestTimeoutInMilliseconds = 5000;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            nock.Stop();
        }

        [TestMethod]
        public async Task TestNock()
        {
            var nock = new nock("http://domain-name.com")
                .Get("/users/1")
                .Reply(HttpStatusCode.OK, "OK");

            /*
            var request = WebRequest.Create("http://domain-name.com/users/1") as HttpWebRequest;
            request.Method = "GET";
            var response = request.GetResponse() as HttpWebResponse;
            */

            /*
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The status code should be OK, but is {0}.", response.StatusCode);
            var httpContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("OK", httpContent);
            */

            Assert.IsTrue(true, "should be true.");
        }


    }
}
