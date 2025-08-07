using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCoreByHand
{
    public class Workspace
    {
        public int Id { get; set; }
        public string WorkspaceName { get; set; }
        public int CreatedBy { get; set; }  // FK -> User.Id
    }

}
