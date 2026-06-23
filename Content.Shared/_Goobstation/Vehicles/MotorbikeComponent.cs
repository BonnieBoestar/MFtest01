using System;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Vehicles;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MotorbikeComponent : Component
{
    [DataField]
    public string FuelSolution = "motorbikeFuel";

    [DataField]
    public ProtoId<ReagentPrototype> FuelReagent = "WeldingFuel";

    [DataField]
    public FixedPoint2 FuelUsePerSecond = FixedPoint2.New(2.5f);

    [DataField]
    public FixedPoint2 RefillAmount = FixedPoint2.New(100f);

    [DataField]
    public FixedPoint2 MaxIntegrity = FixedPoint2.New(250f);

    [DataField]
    public TimeSpan RefillDelay = TimeSpan.FromSeconds(10);

    [DataField]
    public TimeSpan ExplosionDelay = TimeSpan.FromSeconds(5);

    [DataField]
    public string ExplosionType = "Default";

    [DataField]
    public float ExplosionTotalIntensity = 30f;

    [DataField]
    public float ExplosionSlope = 10f;

    [DataField]
    public float ExplosionMaxTileIntensity = 10f;

    [DataField]
    public SoundSpecifier RefillSound = new SoundPathSpecifier("/Audio/Effects/refill.ogg");

    [DataField]
    public SoundSpecifier FuseSound = new SoundPathSpecifier("/Audio/_Nuclear14/Weapons/Effects/Wpn_dynam_fuse.ogg");

    [ViewVariables]
    public FixedPoint2 FuelAccumulator;

    [ViewVariables]
    public DoAfterId? RefuelDoAfter;

    [ViewVariables, AutoNetworkedField]
    public bool Burning;

    [ViewVariables, AutoNetworkedField]
    public TimeSpan? ExplodeAt;
}

[Serializable, NetSerializable]
public enum MotorbikeVisuals : byte
{
    Burning,
}

[Serializable, NetSerializable]
public sealed partial class MotorbikeRefuelDoAfterEvent : SimpleDoAfterEvent;
