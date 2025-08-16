using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureDI.Demo
{
    // Returns current time (abstractable for testing).
    public interface IClock
    {
        DateTime UtcNow { get; }
    }

}
