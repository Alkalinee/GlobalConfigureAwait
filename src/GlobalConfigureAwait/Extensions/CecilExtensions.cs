using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace GlobalConfigureAwait.Extensions
{
    public static class CecilExtensions
    {
        private const string ConfigureAwaitAttributeName = "Fody.ConfigureAwaitAttribute";

        public static bool? GetConfigureAwaitAttributeValue(this ICustomAttributeProvider value)
        {
            var attribute =
                value.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == ConfigureAwaitAttributeName);
            return (bool?) attribute?.ConstructorArguments[0].Value;
        }

        public static bool IsCompilerGenerated(this ICustomAttributeProvider provider)
        {
            if (provider == null || !provider.HasCustomAttributes)
                return false;

            return provider.CustomAttributes
                .Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
        }

        public static bool IsIAsyncStateMachine(this TypeDefinition typeDefinition)
        {
            if (typeDefinition == null || !typeDefinition.HasInterfaces)
                return false;

            return typeDefinition.Interfaces
                .Any(x => x.InterfaceType.FullName == "System.Runtime.CompilerServices.IAsyncStateMachine");
        }

        public static TypeDefinition GetAsyncStateMachineType(this ICustomAttributeProvider provider)
        {
            if (provider == null || !provider.HasCustomAttributes)
                return null;

            return (TypeDefinition)provider.CustomAttributes
                .FirstOrDefault(a =>
                    a.AttributeType.FullName == "System.Runtime.CompilerServices.AsyncStateMachineAttribute")
                ?.ConstructorArguments[0].Value;
        }

        public static void InsertBefore(this ILProcessor processor, Instruction target, params Instruction[] instructions)
        {
            foreach (var instruction in instructions)
                processor.InsertBefore(target, instruction);
        }

        public static void RemoveConfigureAwaitAttribute(this ICustomAttributeProvider definition)
        {
            for (var i = definition.CustomAttributes.Count - 1; i >= 0; i--)
            {
                var attribute = definition.CustomAttributes[i];
                if (attribute.AttributeType.FullName == ConfigureAwaitAttributeName)
                {
                    definition.CustomAttributes.Remove(attribute);
                    break;
                }
            }
        }
    }
}