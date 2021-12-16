using System.Diagnostics;

namespace SharpCrokite.Core.StaticDataUpdater
{
    [DebuggerDisplay("{Type} - {Name}")]
    public class MaterialDto
    {
        public int MaterialId { get; init; }
        public string Type { get; init; }
        public string Name { get; init; }
        public byte[] Icon { get; init; }
        public string Description { get; init; }
    }
}
