using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Reporting
{
    public interface IReportDocument
    {
        IReportFilter CustomFilter { get; set; }
        void LoadData();
    }
}
