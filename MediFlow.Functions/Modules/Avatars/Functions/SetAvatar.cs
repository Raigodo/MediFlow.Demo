using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Avatars.Functions
{
    public class SetAvatar(ISender sender)
    {
        [Function("users/current/avatar")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
