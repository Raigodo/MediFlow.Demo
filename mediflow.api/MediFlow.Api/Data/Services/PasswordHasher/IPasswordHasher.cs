namespace MediFlow.Api.Data.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Veriffy(string password, string hashedPassword);
    }
}