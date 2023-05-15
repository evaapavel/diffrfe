using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Wraps an API response with two values: id and the input data position (left, right).
    /// </summary>
    public class IdAndPositionWrapper
    {

        /// <summary>REST API communication ID value.</summary>
        public string Id { get; set; }

        /// <summary>When posting data to the API, it is necessary to specify the position of the data ("left" / "right" ).</summary>
        public string Position { get; set; }

    }



}
