using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class AndCriteria : SingleCriteria
    {
        public AndCriteria(in QueryCondition condition)
            : base(condition)
        {
        }
    }
}
