using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLogTokeniser
{
    internal class HttpLogStringTokeniser : IHttpLogTokeniser
    {
        enum TokeniserState { TOKEN_START, TOKEN_BRACKET_MARKER_START, TOKEN_QUOTES_MARKER_START, TOKEN_NORMAL_INPROGRESS, TOKEN_MARKER_END }
        
        /// <summary>
        /// Parses the log line and tokenises based log format
        /// </summary>
        /// <param name="logLine">line to parse into tokens</param>
        /// <returns>List of tokens from parsing</returns>
        private List<string> lineTokeniser(string logLine)
        {
            TokeniserState curState = TokeniserState.TOKEN_START;
            StringBuilder curToken = new StringBuilder();
            List<string> tokens = new List<string>();
            foreach (char c in logLine)
            {
                switch (c)
                {
                    case '"':
                        {
                            if (curState == TokeniserState.TOKEN_START)
                            {
                                curState = TokeniserState.TOKEN_QUOTES_MARKER_START;
                            }
                            else if (curState == TokeniserState.TOKEN_QUOTES_MARKER_START)
                            {
                                curState = TokeniserState.TOKEN_MARKER_END;
                            }
                            break;
                        }
                    case '[':
                        {
                            if (curState == TokeniserState.TOKEN_START)
                            {
                                curState = TokeniserState.TOKEN_BRACKET_MARKER_START;
                            }
                            else
                            {
                                curToken.Append(c);
                            }
                            break;
                        }
                    case ']':
                        {
                            if (curState == TokeniserState.TOKEN_BRACKET_MARKER_START)
                            {
                                curState = TokeniserState.TOKEN_MARKER_END;
                            }
                            else
                            {
                                curToken.Append(c);
                            }
                            break;
                        }
                    case ' ':
                        {
                            if (curState == TokeniserState.TOKEN_BRACKET_MARKER_START
                                || curState == TokeniserState.TOKEN_QUOTES_MARKER_START)
                            {
                                curToken.Append(c);
                            }
                            else if (curState == TokeniserState.TOKEN_MARKER_END
                                     || curState == TokeniserState.TOKEN_NORMAL_INPROGRESS)
                            {
                                tokens.Add(curToken.ToString());
                                curToken.Clear();

                                curState = TokeniserState.TOKEN_START;
                            }
                            break;
                        }
                    default:
                        {
                            if (curState != TokeniserState.TOKEN_MARKER_END)
                            {
                                curToken.Append(c);

                                if (curState == TokeniserState.TOKEN_START)
                                {
                                    curState = TokeniserState.TOKEN_NORMAL_INPROGRESS;
                                }
                            }

                            break;
                        }

                }
            }

            //Handle end of line
            if (curState == TokeniserState.TOKEN_NORMAL_INPROGRESS
                || curState == TokeniserState.TOKEN_MARKER_END)
            {
                tokens.Add(curToken.ToString());
                curToken.Clear();
            }
            
            return tokens;
        }

        /// <summary>
        /// Parses the lines received into tokens
        /// </summary>
        /// <param name="lines">Array of log lines to be parsed into tokens</param>
        /// <returns>List of tokens for each line</returns>
        public List<List<string>> tokenise(string[] lines)
        {
            List<List<string>> tokensList = new List<List<string>>();

            foreach (var line in lines)
            {
                tokensList.Add(lineTokeniser(line));
            }

            return tokensList;
        }
    }
}
