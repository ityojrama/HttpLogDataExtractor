using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLogDataExtractor;

namespace HttpLogDataExtractorDemoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpLogDataExtractor.HttpLogDataInfo httpLogDataInfo = new HttpLogDataExtractor.HttpLogDataInfo();
            HttpLogDataExtractor.HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            string[] logLines =
                File.ReadAllLines(
                    "E:\\Programming_Task_-_Azenix_\\Programming_Task_-_Azenix_\\programming-task-example-data.log");
            retCode = httpLogDataInfo.Process(logLines);

            Console.WriteLine($"Number of Unique Ip Addresses - {httpLogDataInfo.GetUniqueIPAddressesCount()}");

            List<string> top3Urls = httpLogDataInfo.GetTopUrls(3);
            Console.WriteLine($"Top 3 Urls:");
            foreach (var url in top3Urls)
            {
                Console.WriteLine(url);
            }

            List<string> top3ActiveIps = httpLogDataInfo.GetTopActiveIPAddresses(3);
            Console.WriteLine($"Top 3 Active IP Addresses:");
            foreach (var ip in top3ActiveIps)
            {
                Console.WriteLine(ip);
            }
        }
    }
}
