using System;
using Mono.Cecil;

namespace GlobalConfigureAwait.Extensions
{
    public static class Assert
    {
        public static void IsType(TypeReference typeReference, Type expectedType)
        {
            if (typeReference.Name != expectedType.Name)
                throw new InvalidOperationException(
                    $"Invalid type: expected {expectedType.FullName}, got {typeReference.FullName}.");
        }
    }
}