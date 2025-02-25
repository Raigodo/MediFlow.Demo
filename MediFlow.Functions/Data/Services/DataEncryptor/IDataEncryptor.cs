namespace MediFlow.Functions.Data.Services.DataEncryptor;

public interface IDataEncryptor
{
    T Decrypt<T>(string data);
    string Encrypt<T>(T data);
}
