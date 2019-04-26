using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM.Model
{
    public interface IFilterMutatorPager
    {
        int ItemsPerPage { get; }

        int ItemsToSkip { get; }
    }
}
