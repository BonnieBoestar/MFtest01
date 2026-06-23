using Robust.Shared.Prototypes;

namespace Content.Shared._Misfits.Crafting;

[Prototype]
public sealed class HandCraftIntellRecipe : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField(required: true)]
    public int MinInt;
}
