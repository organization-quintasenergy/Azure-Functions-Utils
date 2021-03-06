using System;
using System.Threading.Tasks;
using AFBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PaymentService
{
    public static class CommandsReceiverFunction
    {
        const string SERVICENAME = "paymentservice";
        public static HandlersContainer container = new HandlersContainer(SERVICENAME);

        static CommandsReceiverFunction()
        {
            HandlersContainer.AddDependency<IPaymentsRepository, InMemoryPaymentsRepository>();
        }


        [FunctionName("PaymentServiceEndpointFunction")]
        public static async Task Run([QueueTrigger("paymentservice")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"PaymentServiceCommandReceiverFunction received a message: {myQueueItem}");

            await container.HandleAsync(myQueueItem, log);
        }
    }
}
