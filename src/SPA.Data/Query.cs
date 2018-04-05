using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Data
{
    public abstract class Query 
    {
        public virtual string Sql { get; set; } 
        public virtual string Table{ get; set;}   
    }
}
