using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpLogTokeniser;

namespace HttpLogDataExtractor
{
    /// <summary>
    /// This class handles getting log file path or lines through Process api
    /// The the following can be retrieved:
    /// 1. The number of unique IP addresses - GetUniqueIPAddressesCount
    /// 2. The top 3 most visited URLs - GetTopUrls
    /// 3. The top 3 most active IP addresses - GetTopActiveIPAddresses
    /// </summary>
    public class HttpLogDataInfo
    {
        public enum DataInfoState { NO_LOGS_LOADED = 0, LOGS_LOADED}

        private enum FieldIndices {IP_ADDR = 0, URL = 4}

        private List<List<string>> logDataTokenList;
        private DataInfoState currentState = DataInfoState.NO_LOGS_LOADED;

        private Dictionary<string, int> ipAddressFrequencies = new Dictionary<string, int>();
        private Dictionary<string, int> urlFrequencies = new Dictionary<string, int>();

        void loadIPFrequencies()
        {
            if (ipAddressFrequencies.Count == 0)
            {
                foreach (var tokens in logDataTokenList)
                {
                    if (!ipAddressFrequencies.ContainsKey(tokens[(int)FieldIndices.IP_ADDR]))
                    {
                        ipAddressFrequencies[tokens[(int)FieldIndices.IP_ADDR]] = 1;
                    }
                    else
                    {
                        ipAddressFrequencies[tokens[(int)FieldIndices.IP_ADDR]]++;
                    }
                }
            }

        }

        void loadUrlFrequencies()
        {
            const int URL_INDEX = 1;
            if (urlFrequencies.Count == 0)
            {
                foreach (var tokens in logDataTokenList)
                {
                    var urlSection = tokens[(int) FieldIndices.URL];
                    char[] separatorChars = new char[] {' '};
                    string[] urlSectionTokens = urlSection.Split(separatorChars);

                    if (!urlFrequencies.ContainsKey(urlSectionTokens[URL_INDEX]))
                    {
                        urlFrequencies[urlSectionTokens[URL_INDEX]] = 1;
                    }
                    else
                    {
                        urlFrequencies[urlSectionTokens[URL_INDEX]]++;
                    }
                }
            }
        }

        /// <summary>
        /// Reads the log file and parses the log data line by line into tokens/sections
        /// </summary>
        /// <param name="filePath">Full file path of log file</param>
        /// <returns>HttpLogOpRetCode</returns>
        public HttpLogOpRetCode Process(string filePath)
        {
            HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            
            if (File.Exists(filePath))
            {
                string []logLines = File.ReadAllLines(filePath);
                retCode = Process(logLines);
            }
            else
            {
                retCode = HttpLogOpRetCode.INVALID_FILE_PATH;
            }

            return retCode;
        }

        /// <summary>
        /// Parses the provided lines, tokenising each line
        /// </summary>
        /// <param name="logLines">Array of log lines loaded from log file</param>
        /// <returns>HttpLogOpRetCode</returns>
        public HttpLogOpRetCode Process(string[] logLines)
        {
            HttpLogOpRetCode retCode = HttpLogOpRetCode.SUCCESS;
            currentState = DataInfoState.NO_LOGS_LOADED;
            
            ipAddressFrequencies.Clear();
            urlFrequencies.Clear();

            try
            {
                IHttpLogTokeniser tokeniser = TokeniserCreator.GetTokeniser(TokeniserCreator.TokeniserType.STRING);

                logDataTokenList = tokeniser.tokenise(logLines);

                currentState = DataInfoState.LOGS_LOADED;
            }
            catch (Exception)
            {
                retCode = HttpLogOpRetCode.LOG_PARSING_FAILED;
            }
            

            return retCode;
        }

        /// <summary>
        /// Gets the number of unique IP addresses in the logs
        /// Any of the relevant Process methods must be called before this method
        /// </summary>
        /// <returns>Success or Fail</returns>
        /// <exception cref="Exception"></exception>
        public int GetUniqueIPAddressesCount()
        {
            int uniqueIPsCount = 0;
            if (currentState == DataInfoState.LOGS_LOADED)
            {
                loadIPFrequencies();
                uniqueIPsCount = ipAddressFrequencies.Count;
            }
            else
            {
                throw new Exception("No log file provided!");
            }

            return uniqueIPsCount;
        }

        /// <summary>
        /// Gets the top urls indicated by count, i.e. if count is 3 it will
        /// retrieve top 3 urls
        /// </summary>
        /// <param name="count">The number of top urls to retrieve</param>
        /// <returns>List of the top urls</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetTopUrls(int count)
        {
            List<string> topUrls = new List<string>();
            if (currentState == DataInfoState.LOGS_LOADED)
            {
                if (count < 1)
                {
                    throw new Exception("count parameter cannot be less than 1!");
                }

                loadUrlFrequencies();

                List<KeyValuePair<string, int>> urlFrequencyList = urlFrequencies.ToList();

                urlFrequencyList.Sort((KeyValuePair<string, int> lhs, KeyValuePair<string, int> rhs) =>
                {
                    if (lhs.Value == rhs.Value)
                    {
                        return lhs.Key.CompareTo(rhs.Key); //Sorted alphabetically if same frequency
                    }
                    else
                    {
                        return rhs.Value - lhs.Value;
                    }
                    
                });
            
                count = Math.Min(count, urlFrequencies.Count);

                for (int idx = 0; idx < count; idx++)
                {
                    topUrls.Add(urlFrequencyList[idx].Key);
                }
            }
            else
            {
                throw new Exception("No log file provided!");
            }

            return topUrls;
        }

        /// <summary>
        /// Gets the top active ip addresses indicated by count, i.e. if count is 3 it will
        /// retrieve the top 3 active ip addresses
        /// </summary>
        /// <param name="count">The number of top active ip addresses to retrieve</param>
        /// <returns>List of top active ip addresses</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetTopActiveIPAddresses(int count)
        {
            List<string> topActiveIPs = new List<string>();
            if (currentState == DataInfoState.LOGS_LOADED)
            {
                if (count < 1)
                {
                    throw new Exception("count parameter cannot be less than 1!");
                }
                
                
                loadIPFrequencies();

                List<KeyValuePair<string, int>> ipFrequencyList = ipAddressFrequencies.ToList();

                ipFrequencyList.Sort((KeyValuePair<string, int> lhs, KeyValuePair<string, int> rhs) =>
                {
                    if (lhs.Value == rhs.Value)
                    {
                        return lhs.Key.CompareTo(rhs.Key);  //Sorted alphabetically if same frequency
                    }
                    else
                    {
                        return rhs.Value - lhs.Value;
                    }
                });

                count = Math.Min(count, ipAddressFrequencies.Count);

                for (int idx = 0; idx < count; idx++)
                {
                    topActiveIPs.Add(ipFrequencyList[idx].Key);
                }
            }
            else
            {
                throw new Exception("No log file provided!");
            }

            return topActiveIPs;
        }
    }
}
