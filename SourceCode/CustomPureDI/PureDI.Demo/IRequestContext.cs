using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureDI.Demo
{
    // Data that belongs to a single request (Scoped).
    public interface IRequestContext
    {
        Guid CorrelationId { get; }
    }

}
