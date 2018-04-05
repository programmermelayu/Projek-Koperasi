using System;
namespace SPA.Data
{
    interface IQuerySelect
    {
        bool Distinct { get; set; }
        System.Collections.ArrayList Fields { get; set; }
        string On { get; set; }
        string OrderBy { get; set; }
        int Top { get; set; }
    }
}
