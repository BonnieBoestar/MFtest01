using Content.Shared.Tag;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Misfits.PipBoy;

[Prototype("pipBoyFactionChannel")]
public sealed partial class PipBoyFactionChannelPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string Name { get; private set; } = string.Empty;

    [DataField(required: true, customTypeSerializer: typeof(PrototypeIdSerializer<TagPrototype>))]
    public string IdCardTag { get; private set; } = string.Empty;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<JobPrototype>))]
    public string? AnnouncerJob { get; private set; }

    public string LocalizedName => Loc.GetString(Name);
}
