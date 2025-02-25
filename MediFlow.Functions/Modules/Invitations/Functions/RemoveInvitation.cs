using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Invitations.Functions
{
    public class RemoveInvitation(ISender sender)
    {
        [Function("employees/invitations/{invitationId}")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
