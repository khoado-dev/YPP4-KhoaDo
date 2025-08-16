using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureDI.Demo
{
    public class GreetingService : IGreetingService
    {
        private readonly IClock _clock;

        // Constructor injection
        public GreetingService(IClock clock)
        {
            _clock = clock;
        }

        public string Greet(string name)
        {
            // Simple business logic using dependency
            return $"Hello, {name}! Time (UTC): {_clock.UtcNow:O}";
        }
    }
}
