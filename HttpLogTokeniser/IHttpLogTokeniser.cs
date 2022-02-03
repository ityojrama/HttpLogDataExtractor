using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLogTokeniser
{
    public interface IHttpLogTokeniser
    {
        List<List<string>> tokenise(string[] lines);
    }
}
