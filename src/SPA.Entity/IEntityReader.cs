using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPA.Entity
{
    public interface IEntityReader<T>
    {

        string ErrorMessage { get; set; }

        string SystemErrorMessage { get; set; }

        T SingleRecord { get; set; }

        List<T> MultipleRecords { get; set; }

         //IEntity  SingleRecord { get; set; }
         //IEntity MultipleRecord { get; set; }
        /// <summary>
        /// Retrieve single record by passing multiple filter from the caller/client.
        /// </summary>
        /// <param name="searchKeyCollection">collection of filter</param>
        /// <returns></returns>
        bool ReadSingle(FilterElement filter);

        /// <summary>
        /// Retrieve single record by passing multiple filter from the caller/client.
        /// </summary>
        /// <param name="searchKeyCollection">collection of filter</param>
        /// <returns></returns>
        bool ReadSingle(List<FilterElement> filterCollection);

        /// <summary>
        /// Retrieve multiple records without any filter from the caller/client.
        /// </summary>
        /// <param name="searchKeyCollection">collection of filter</param>
        /// <returns></returns>
        bool ReadMultiple();

        /// <summary>
        /// Retrieve multiple records by passing multiple filter from the caller/client.
        /// </summary>
        /// <param name="searchKeyCollection">collection of filter</param>
        /// <returns></returns>
        bool ReadMultiple(List<FilterElement> filterCollection);

        /// <summary>
        ///  Retrieve multiple records passing single filter from the caller/cient.
        /// </summary>
        /// <param name="searchKeyCollection">collection of filter</param>
        /// <returns></returns>
        bool ReadMultiple(FilterElement filter);

    }


    

}
