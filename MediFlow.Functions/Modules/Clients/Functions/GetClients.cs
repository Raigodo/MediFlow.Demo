using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Clients.Functions
{
    public class GetClients(ISender sender)
    {
        [Function("clients")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
