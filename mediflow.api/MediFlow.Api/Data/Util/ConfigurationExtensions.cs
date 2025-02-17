using MediFlow.Api.Data.Services.DataEncryptor;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFlow.Api.Data.Util;

public static class ConfigurationExtensions
{
    public static PropertyBuilder<T> IsEnrypted<T>(this PropertyBuilder<T> builder, IDataEncryptor encryptor) =>
        builder.HasConversion(
            data => encryptor.Encrypt(data),
            hash => encryptor.Decrypt<T>(hash));
}
