using Content.Server.Popups;
using Content.Server.Stack;
using Content.Shared._Misfits.Crafting;
using Content.Shared._Misfits.Special;
using Content.Shared.DoAfter;
using Content.Shared.Materials;
using Content.Shared.Research.Prototypes;
using Content.Shared.Stacks;
using Robust.Shared.Containers;
using Robust.Shared.Localization;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._Misfits.Crafting;

public sealed class HandCraftIntellSystem : EntitySystem
{
    [Dependency] private readonly SharedSpecialSystem _special = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly StackSystem _stack = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<TryHandCraftIntellRecipeMessage>(OnTryHandCraft);
        SubscribeLocalEvent<HandCraftIntellDoAfterEvent>(OnDoAfter);
    }

    private void OnTryHandCraft(TryHandCraftIntellRecipeMessage msg, EntitySessionEventArgs args)
    {
        var player = args.SenderSession.AttachedEntity;
        if (player == null)
            return;

        if (!_proto.TryIndex<LatheRecipePrototype>(msg.RecipeId, out var recipe))
            return;

        if (!_proto.TryIndex<HandCraftIntellRecipe>(msg.RecipeId, out var craftData))
            return;

        if (_special.GetEffective(player.Value, SpecialStat.Intelligence) < craftData.MinInt)
        {
            _popup.PopupEntity(Loc.GetString("hand-craft-intell-too-low-intelligence"), player.Value, player.Value);
            return;
        }

        if (!CheckMaterialsAvailable(player.Value, recipe.Materials))
        {
            _popup.PopupEntity(Loc.GetString("hand-craft-intell-insufficient-materials"), player.Value, player.Value);
            return;
        }

        var ev = new HandCraftIntellDoAfterEvent(msg.RecipeId);
        var doAfterArgs = new DoAfterArgs(EntityManager, player.Value, recipe.CompleteTime, ev, player.Value)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
            Broadcast = true,
        };
        _doAfter.TryStartDoAfter(doAfterArgs);
    }

    private void OnDoAfter(HandCraftIntellDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        var player = args.User;

        if (!_proto.TryIndex<LatheRecipePrototype>(args.RecipeId, out var recipe))
            return;

        if (!TryConsumeMaterials(player, recipe.Materials))
        {
            _popup.PopupEntity(Loc.GetString("hand-craft-intell-insufficient-materials"), player, player);
            return;
        }

        if (recipe.Result.HasValue)
            Spawn(recipe.Result.Value, Transform(player).Coordinates);

        args.Handled = true;
    }

    private bool CheckMaterialsAvailable(EntityUid player, Dictionary<ProtoId<MaterialPrototype>, int> required)
    {
        var available = CollectMaterials(player);
        foreach (var (material, amount) in required)
        {
            string matId = material;
            if (!available.TryGetValue(matId, out var have) || have < amount)
                return false;
        }
        return true;
    }

    private bool TryConsumeMaterials(EntityUid player, Dictionary<ProtoId<MaterialPrototype>, int> required)
    {
        var items = CollectMaterialItems(player);
        var toDelete = new List<EntityUid>();
        var toReduce = new List<(EntityUid entity, int newCount)>();

        foreach (var (material, needed) in required)
        {
            string matId = material;
            var remaining = needed;

            foreach (var (entity, matItemId, volPerUnit, count) in items)
            {
                if (matItemId != matId || remaining <= 0)
                    continue;

                var totalFromThis = volPerUnit * count;
                if (totalFromThis <= remaining)
                {
                    toDelete.Add(entity);
                    remaining -= totalFromThis;
                }
                else
                {
                    var unitsNeeded = (remaining + volPerUnit - 1) / volPerUnit;
                    var newCount = count - unitsNeeded;
                    if (newCount <= 0)
                        toDelete.Add(entity);
                    else
                        toReduce.Add((entity, newCount));
                    remaining = 0;
                }
            }

            if (remaining > 0)
                return false;
        }

        foreach (var entity in toDelete)
            QueueDel(entity);

        foreach (var (entity, newCount) in toReduce)
        {
            if (TryComp<StackComponent>(entity, out var stack))
                _stack.SetCount(entity, newCount, stack);
            else
                QueueDel(entity);
        }

        return true;
    }

    private Dictionary<string, int> CollectMaterials(EntityUid player)
    {
        var result = new Dictionary<string, int>();
        foreach (var (_, matId, volPerUnit, count) in CollectMaterialItems(player))
        {
            result[matId] = result.GetValueOrDefault(matId) + volPerUnit * count;
        }
        return result;
    }

    private List<(EntityUid entity, string matId, int volPerUnit, int count)> CollectMaterialItems(EntityUid player)
    {
        var items = new List<(EntityUid, string, int, int)>();
        var visited = new HashSet<EntityUid>();
        ScanContainers(player, items, visited);
        return items;
    }

    private void ScanContainers(EntityUid uid, List<(EntityUid, string, int, int)> items, HashSet<EntityUid> visited)
    {
        if (!visited.Add(uid))
            return;

        if (!TryComp<ContainerManagerComponent>(uid, out var containers))
            return;

        foreach (var container in containers.Containers.Values)
        {
            foreach (var contained in container.ContainedEntities)
            {
                if (HasComp<MaterialComponent>(contained) &&
                    TryComp<PhysicalCompositionComponent>(contained, out var composition))
                {
                    var count = TryComp<StackComponent>(contained, out var stack) ? stack.Count : 1;
                    foreach (var (matId, volPerUnit) in composition.MaterialComposition)
                        items.Add((contained, matId, volPerUnit, count));
                }

                ScanContainers(contained, items, visited);
            }
        }
    }
}
