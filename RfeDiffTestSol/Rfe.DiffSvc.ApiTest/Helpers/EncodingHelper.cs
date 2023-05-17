using System;
using System.Text;


namespace Rfe.DiffSvc.ApiTest.Helpers
{



    /// <summary>
    /// Exposes methods to encode/decode strings with Base64.
    /// </summary>
    public static class EncodingHelper
    {



        /// <summary>
        /// Gets a plain text as an input parameter
        /// and encodes it into a Base64 string.
        /// </summary>
        /// <param name="plainText">Plain text to convert.</param>
        /// <returns>Returns a Base64 equivalent of the given text.</returns>
        public static string Base64Encode(string plainText)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            string result = Convert.ToBase64String(bytes);
            return result;
        }



        /// <summary>
        /// Gets a Base64 encoded data as input
        /// and decodes it into a plain text string. 
        /// </summary>
        /// <param name="base64Data">Base64 encoded data to convert.</param>
        /// <returns>Returns a plain text string equivalent of the given input.</returns>
        public static string Base64Decode(string base64Data)
        {
            Byte[] bytes = Convert.FromBase64String(base64Data);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }



    }



}