namespace GlobalConfigureAwait.Test.Helpers
{
    public class NetStandardAssemblyWeaver : AssemblyWeaver
    {
        public NetStandardAssemblyWeaver() : base("AssemblyToProcess.NetStandard.dll")
        {
        }
    }
}