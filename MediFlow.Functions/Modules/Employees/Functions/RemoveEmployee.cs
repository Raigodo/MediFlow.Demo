using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace MediFlow.Functions.Modules.Employees.Functions
{
    public class RemoveEmployee(ISender sender)
    {
        [Function("employees/{employeeId}")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
