using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Employees.Functions
{
    public class GetEmployees(ISender sender)
    {
        [Function("employees")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
