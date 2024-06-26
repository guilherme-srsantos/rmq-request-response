using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using MassTransit;

namespace Server
{
    public class RequestConsumer : IConsumer<GetDataRequest>
    {
        private readonly ILogger _logger;
        public RequestConsumer(ILogger<RequestConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<GetDataRequest> context)
        {
            _logger.LogInformation("RequisicaoRecebida às {Date}", DateTime.Now);

            await context.RespondAsync<GetDataResponse>( new {
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
            });
        }
    }
}