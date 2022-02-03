using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLogTokeniser
{
    /// <summary>
    /// Similar to factory to create tokeniser to be used 
    /// </summary>
    public static class TokeniserCreator
    {
        public enum TokeniserType {STRING = 0}

        public static IHttpLogTokeniser GetTokeniser(TokeniserType tokeniserType)
        {
            IHttpLogTokeniser tokeniser = null;
            switch (tokeniserType)
            {
                case TokeniserType.STRING:
                {
                    tokeniser = new HttpLogStringTokeniser();
                    break;
                }
            }

            return tokeniser;
        }


    }
}
