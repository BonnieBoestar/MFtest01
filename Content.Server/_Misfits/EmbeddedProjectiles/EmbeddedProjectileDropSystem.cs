using Content.Shared.Projectiles;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server._Misfits.EmbeddedProjectiles;

/// <summary>
///     #Misfits Add - Drops embedded projectiles before a mob/silicon is deleted, preventing
///     them from being silently destroyed as transform children of the slain entity.
///     This mirrors the butchering fix in SharpSystem but covers non-butchering death paths
///     (robots destroyed in combat, gibbing, admin deletion, etc.).
/// </summary>
public sealed class EmbeddedProjectileDropSystem : EntitySystem
{
    [Dependency] private readonly SharedProjectileSystem _projectile = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TransformComponent, EntityTerminatingEvent>(OnEntityTerminating);
    }

    private void OnEntityTerminating(Entity<TransformComponent> entity, ref EntityTerminatingEvent args)
    {
        // Snapshot children first because RemoveEmbed reparents the child,
        // which would modify the enumerator mid-iteration.
        var children = new List<EntityUid>();
        var childEnumerator = entity.Comp.ChildEnumerator;
        while (childEnumerator.MoveNext(out var child))
            children.Add(child);

        foreach (var child in children)
        {
            if (!TryComp<EmbeddableProjectileComponent>(child, out var embed))
                continue;

            // broken behavior;
            // EmbeddableProjectileComponent only means this item CAN embed.
            // it does not mean the item is currently embedded in this entity.
            // equipped or container items may also be transform children, so calling
            // RemoveEmbed on all embeddables can drop inventory during admin deletion.
			// also Justice, you fucking bitch. God, I'm gonna enjoy this so much. First you try to cancel me, 
			// victimize an innocent man because I guess that's just cool to do to white guys nowadays, but joke's on you: 
			// #MeToo's over, sweetheart, it didn't work. 
			// Womp, womp, womp. 
			// I do not respect your truth, 
			// I do not honor and cherish your story, 
			// and I do not fucking apologize.
            // -pierow
            // _projectile.RemoveEmbed(child, embed, null);   //<--bruh look at the top of his heeead xDDD

            // the Gigachad fix below: drops thingy that is actually embedded into the entity
            // currently being deleted, leaving it on the ground
            if (embed.Target != entity.Owner)
                continue;

            _projectile.RemoveEmbed(child, embed, null);
        }
    }
}
