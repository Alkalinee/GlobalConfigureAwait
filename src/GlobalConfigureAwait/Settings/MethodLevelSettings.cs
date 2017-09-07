using GlobalConfigureAwait.Extensions;
using Mono.Cecil;

namespace GlobalConfigureAwait.Settings
{
    public class MethodLevelSettings : TypeLevelSettings
    {
        public MethodLevelSettings(ICustomAttributeProvider customAttributeProvider, TypeLevelSettings typeLevelSettings) : base(typeLevelSettings)
        {
            MethodConfigureAwait = customAttributeProvider.GetConfigureAwaitAttributeValue();
            if (MethodConfigureAwait.HasValue)
                customAttributeProvider.RemoveConfigureAwaitAttribute();
        }

        public bool? MethodConfigureAwait { get; }

        public override bool? GetConfigureAwait()
        {
            if (MethodConfigureAwait != null)
                return MethodConfigureAwait;

            return base.GetConfigureAwait();
        }
    }
}