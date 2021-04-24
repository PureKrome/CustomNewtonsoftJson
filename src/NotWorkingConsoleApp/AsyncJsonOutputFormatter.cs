using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;

namespace NotWorkingConsoleApp
{
    public class AsyncJsonOutputFormatter : NewtonsoftJsonOutputFormatter
    {
        public AsyncJsonOutputFormatter(JsonSerializerSettings serializerSettings,
            ArrayPool<char> charPool,
            MvcOptions options) : base(serializerSettings, charPool, options)
        {
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            throw new NotImplementedException();
        }
    }
}
