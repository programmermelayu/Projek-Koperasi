using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Entity
{
    public interface IEntity
    {
        bool Delete();
        bool Create();
        string Sql { get; set; }
        int ID { get; set; }
    }

}
