namespace MediFlow.Functions.Data.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Veriffy(string password, string hashedPassword);
    }
}