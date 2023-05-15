using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Wraps a simple API response with the "communication" id.
    /// </summary>
    public class IdWrapper
    {

        /// <summary>REST API communication ID value.</summary>
        public string Id { get; set; }

    }



}
