using Content.Shared.Materials;
using Robust.Shared.Prototypes;

namespace Content.Shared._Misfits.Crafting;

[Prototype]
public sealed partial class HandCraftIntellRecipePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; set; } = default!;

    [DataField(required: true)]
    public int MinInt;

    [DataField(required: true)]
    public EntProtoId Result;

    [DataField]
    public Dictionary<ProtoId<MaterialPrototype>, int> Materials = new();

    [DataField]
    public TimeSpan CompleteTime = TimeSpan.FromSeconds(5);
}
