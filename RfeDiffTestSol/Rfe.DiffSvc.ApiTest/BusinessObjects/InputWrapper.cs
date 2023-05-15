using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Wraps an API request body to be posted to the REST API server.
    /// </summary>
    public class InputWrapper
    {

        /// <summary>Input data (1st a.k.a. left, or 2nd a.k.a. right).</summary>
        public string Input { get; set; }

    }



}
