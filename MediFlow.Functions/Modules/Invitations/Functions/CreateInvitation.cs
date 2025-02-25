using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Invitations.Functions
{
    public class CreateInvitation(ISender sender)
    {
        [Function("employees/invitations")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
