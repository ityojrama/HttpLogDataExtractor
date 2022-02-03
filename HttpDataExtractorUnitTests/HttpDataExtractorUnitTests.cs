using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using HttpLogDataExtractor;

namespace HttpDataExtractorUnitTests
{
    [TestClass]
    public class HttpDataExtractorUnitTests
    {
        static readonly string LogFilePath = $"E:\\TestInput\\example-data.log";
        [TestMethod]
        public void ProcessWithValidFilePath()
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            retCode = httpLogDataInfo.Process(LogFilePath);

            Assert.AreEqual(retCode, HttpLogOpRetCode.SUCCESS);
        }

        [TestMethod]
        public void ProcessWithInValidFilePath()
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            retCode = httpLogDataInfo.Process("X:\\example-data.log");

            Assert.AreEqual(retCode, HttpLogOpRetCode.INVALID_FILE_PATH);
        }

        [TestMethod]
        public void ProcessWithArrayOfLines()
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);

            Assert.AreEqual(retCode, HttpLogOpRetCode.SUCCESS);
        }

        [TestMethod]
        public void GetUniqueIPAddressesCountTest()
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);

            int uniqueIpAddresses = httpLogDataInfo.GetUniqueIPAddressesCount();

            Assert.AreEqual(uniqueIpAddresses, 11);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No log file provided!")]
        public void GetUniqueIPAddressesCountLogNotLoadedTest()
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            
            int uniqueIpAddresses = httpLogDataInfo.GetUniqueIPAddressesCount();

            Assert.AreEqual(uniqueIpAddresses, 11);
        }

        [TestMethod]
        public void GetTop3UrlsTest()
        {
            List<string> expectedTop3Urls = new List<string>();
            expectedTop3Urls.Add("/docs/manage-websites/");
            expectedTop3Urls.Add("/");
            expectedTop3Urls.Add("/asset.css");
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);

            List<string> top3Urls = httpLogDataInfo.GetTopUrls(3);


            for (int idx = 0; idx < top3Urls.Count; idx++)
            {
                Assert.AreEqual(expectedTop3Urls[idx], top3Urls[idx]);
            }

            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No log file provided!")]
        public void GetTop3UrlsFileNotLoadedTest()
        {
            List<string> expectedTop3Urls = new List<string>();
            expectedTop3Urls.Add("/docs/manage-websites/");
            expectedTop3Urls.Add("/");
            expectedTop3Urls.Add("/asset.css");
            
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            
            
            List<string> top3Urls = httpLogDataInfo.GetTopUrls(3);


            for (int idx = 0; idx < top3Urls.Count; idx++)
            {
                Assert.AreEqual(expectedTop3Urls[idx], top3Urls[idx]);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "count parameter cannot be less than 1")]
        public void GetTop3UrlsCountLessThanOneTest()
        {
            List<string> expectedTop3Urls = new List<string>();
            expectedTop3Urls.Add("/docs/manage-websites/");
            expectedTop3Urls.Add("/");
            expectedTop3Urls.Add("/asset.css");
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);
            List<string> top3Urls = httpLogDataInfo.GetTopUrls(0);


            for (int idx = 0; idx < top3Urls.Count; idx++)
            {
                Assert.AreEqual(expectedTop3Urls[idx], top3Urls[idx]);
            }


        }

        [TestMethod]
        public void GetTop3ActiveIPAddressesTest()
        {
            List<string> expectedTop3ActiveIPAddresses = new List<string>();
            expectedTop3ActiveIPAddresses.Add("168.41.191.40");
            expectedTop3ActiveIPAddresses.Add("177.71.128.21");
            expectedTop3ActiveIPAddresses.Add("50.112.00.11");
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);

            List<string> top3ActiveIPAddresses = httpLogDataInfo.GetTopActiveIPAddresses(3);


            for (int idx = 0; idx < top3ActiveIPAddresses.Count; idx++)
            {
                Assert.AreEqual(expectedTop3ActiveIPAddresses[idx], top3ActiveIPAddresses[idx]);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No log file provided!")]
        public void GetTop3ActiveIPAddressesFileNotLoadedTest()
        {
            List<string> expectedTop3ActiveIPAddresses = new List<string>();
            expectedTop3ActiveIPAddresses.Add("168.41.191.40");
            expectedTop3ActiveIPAddresses.Add("177.71.128.21");
            expectedTop3ActiveIPAddresses.Add("50.112.00.11");
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            
            List<string> top3ActiveIPAddresses = httpLogDataInfo.GetTopActiveIPAddresses(3);


            for (int idx = 0; idx < top3ActiveIPAddresses.Count; idx++)
            {
                Assert.AreEqual(expectedTop3ActiveIPAddresses[idx], top3ActiveIPAddresses[idx]);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "count parameter cannot be less than 1!")]
        public void GetTop3ActiveIPAddressesCountLessThanOneTest()
        {
            List<string> expectedTop3ActiveIPAddresses = new List<string>();
            expectedTop3ActiveIPAddresses.Add("168.41.191.40");
            expectedTop3ActiveIPAddresses.Add("177.71.128.21");
            expectedTop3ActiveIPAddresses.Add("50.112.00.11");
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;

            string[] logLines = File.ReadAllLines(LogFilePath);
            retCode = httpLogDataInfo.Process(logLines);

            List<string> top3ActiveIPAddresses = httpLogDataInfo.GetTopActiveIPAddresses(-3);


            for (int idx = 0; idx < top3ActiveIPAddresses.Count; idx++)
            {
                Assert.AreEqual(expectedTop3ActiveIPAddresses[idx], top3ActiveIPAddresses[idx]);
            }


        }
    }
}
