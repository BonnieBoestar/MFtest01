using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._Misfits.Crafting;

[Serializable, NetSerializable]
public sealed class TryHandCraftIntellRecipeMessage : EntityEventArgs
{
    public readonly string RecipeId;

    public TryHandCraftIntellRecipeMessage(string recipeId)
    {
        RecipeId = recipeId;
    }
}

[Serializable, NetSerializable]
public sealed partial class HandCraftIntellDoAfterEvent : DoAfterEvent
{
    [DataField]
    public string RecipeId = string.Empty;

    private HandCraftIntellDoAfterEvent() { }

    public HandCraftIntellDoAfterEvent(string recipeId)
    {
        RecipeId = recipeId;
    }

    public override DoAfterEvent Clone() => this;
}
