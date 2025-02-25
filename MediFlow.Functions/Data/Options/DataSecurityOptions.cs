namespace MediFlow.Functions.Data.Options;

public sealed class DataSecurityOptions
{
    public EncryptionKeysOptions EncryptionKeys { get; set; } = new();

    public sealed class EncryptionKeysOptions
    {
        public string Current { get; set; } = string.Empty;
    }
}

