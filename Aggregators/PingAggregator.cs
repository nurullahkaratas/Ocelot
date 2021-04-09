using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ocelott.Aggregators
{
    public  class PingAggregator : IDefinedAggregator
    {
     
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            var contentBuilder = new StringBuilder();
            foreach (var context in responses)
            {
                contentBuilder.Append(await context.DownstreamResponse.Content.ReadAsStringAsync());
                contentBuilder.Append(",");
            }

            var stringContent = new StringContent(contentBuilder.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK,
            responses.SelectMany(x => x.DownstreamResponse.Headers).ToList(), "OK");

        }

    }
}
