using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.RequestDecompression;
using System.IO;
using System.Security.Cryptography;


namespace Rfe.DiffSvc.WebApi.Middleware
{
    
    
    
    /// <summary>
    /// This class serves as middleware (interceptor) to decode the request body from Base64 to plain text.
    /// Its name should be rather Base64DecodingProvider.
    /// </summary>
    /// <remarks>
    /// I have never implemented such a thing.
    /// Inspiration: https://stackoverflow.com/questions/42792099/request-content-decompression-in-asp-net-core
    /// And more on stackoverflow.com.
    /// </remarks>
    public class Base64DecompressionProvider : IDecompressionProvider
    {
        
        
        
        // Implement the "decompression"/"decoding" method.
        public Stream GetDecompressionStream(Stream stream)
        //public async Stream GetDecompressionStream(Stream stream)
        //public async Task<Stream> GetDecompressionStream(Stream stream)
        {
            // Perform custom decompression logic here.
            //return stream;

            //MemoryStream decompressed = new MemoryStream();
            //StringWriter writer = new StringWriter(stream);
            //CryptoStream inStream = new CryptoStream(stream, new FromBase64Transform(), CryptoStreamMode.Read);
            
            // Prepare an output stream.
            MemoryStream outStream = new MemoryStream();

            // Decode from Base64.
            // Use CryptoStream as an input stream.
            using (CryptoStream inStream = new CryptoStream(stream, new FromBase64Transform(), CryptoStreamMode.Read))
            {
                //inStream.CopyTo(outStream);
                //inStream.CopyToAsync(outStream);
                //await inStream.CopyToAsync(outStream);
                Task result = inStream.CopyToAsync(outStream);
                //Task.WaitAll(result);
                //result.Start();
                result.Wait();
            }
            
            // You may need to "rewind".
            outStream.Seek(0, SeekOrigin.Begin);

            // Return the output stream.
            return outStream;
        }
        
        
        
    }
    
    
    
}
