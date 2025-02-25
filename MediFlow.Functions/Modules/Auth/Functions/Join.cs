using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Auth.Functions
{
    public class Join(ISender sender)
    {
        [Function("auth/join")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
