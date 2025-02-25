using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Clients.Functions
{
    public class UpdateClient(ISender sender)
    {
        [Function("clients/{clientId}")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "patch")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
