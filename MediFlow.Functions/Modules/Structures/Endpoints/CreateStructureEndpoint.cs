namespace MediFlow.Functions.Modules.Structures.Endpoints;

public record CreateStructureRequest(string StructureName);

public static class CreateStructureEndpoint
{
    public static IEndpointRouteBuilder MapCreateStructureEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/structures", Handle)
            .RequireAuthorization(AuthPolicies.ManagerPlus);
        return routes;
    }

    public static async Task<IResult> Handle(
        CreateStructureRequest req,
        ICurrentUserAccessor currentUser,
        IUserRepository userRepository,
        IStructureRepository structureRepository,
        IManagerRepository managerRepository,
        IDeviceKeyRepository deviceKeyRepository,
        IUnitOfWork unitOfWork,
        ResponseFactory responseFactory)
    {
        var user = await userRepository.GetOneAsync(currentUser.UserId);
        if (user is null)
        {
            return responseFactory.NotFound<User>();
        }

        var structure = new Structure
        {
            Name = req.StructureName,
            ManagerId = currentUser.UserId,
        };
        var structureManager = new StructureManager()
        {
            StructureId = structure.Id,
            ManagerId = currentUser.UserId,
        };
        var deviceKey = new DeviceKey()
        {
            StructureId = structure.Id,
            KeyValue = Guid.NewGuid(),
            KeyUpdatedOn = DateOnly.FromDateTime(DateTime.UtcNow),
        };
        structureRepository.Add(structure);
        managerRepository.Add(structureManager);
        deviceKeyRepository.Add(deviceKey);
        await unitOfWork.SaveChangesAsync();

        return responseFactory.Ok(structure.ToResponseDto());
    }
}
