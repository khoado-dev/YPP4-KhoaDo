using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models
{
    public sealed class Request
    {
        public string Method { get; }
        public string Path { get; }

        public Request(string method, string path)
        {
            Method = method.ToUpperInvariant();
            Path = path;
        }
    }
}
