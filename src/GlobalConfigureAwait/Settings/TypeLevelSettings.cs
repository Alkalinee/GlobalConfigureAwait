using GlobalConfigureAwait.Extensions;
using Mono.Cecil;

namespace GlobalConfigureAwait.Settings
{
    public class TypeLevelSettings : AssemblyLevelSettings
    {
        public TypeLevelSettings(ICustomAttributeProvider customAttributeProvider,
            AssemblyLevelSettings assemblySettings) : base(assemblySettings)
        {
            TypeConfigureAwait = customAttributeProvider.GetConfigureAwaitAttributeValue();
            if (TypeConfigureAwait.HasValue)
                customAttributeProvider.RemoveConfigureAwaitAttribute();
        }

        protected TypeLevelSettings(TypeLevelSettings settings) : base(settings)
        {
            TypeConfigureAwait = settings.TypeConfigureAwait;
        }

        public bool? TypeConfigureAwait { get; }

        public override bool? GetConfigureAwait()
        {
            if (TypeConfigureAwait != null)
                return TypeConfigureAwait;

            return base.GetConfigureAwait();
        }
    }
}