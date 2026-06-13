// #Misfits Add - PipBoy Hub cartridge UI state sent from server to client.
using Robust.Shared.Serialization;

namespace Content.Shared._Misfits.PipBoy;

[Serializable, NetSerializable]
public sealed class PipBoyHubUiState : BoundUserInterfaceState
{
    // Identity
    public readonly uint OwnNumber;

    // Lock / password
    public readonly bool HasPassword;
    public readonly bool IsLocked;

    // Directory visibility
    public readonly bool IsVisible;

    // Contacts tab
    public readonly Dictionary<uint, PipBoyContact> Contacts;
    /// <summary>World positions for contacts who share location with us (keyed by their number).</summary>
    public readonly List<PipBoyTrackedLocation> ContactLocations;

    // Groups tab
    public readonly List<PipBoyGroupInfo> Groups;
    /// <summary>Messages for the currently-viewed group (empty if none selected).</summary>
    public readonly List<PipBoyGroupMessage> GroupMessages;
    public readonly uint? SelectedGroupId;
    /// <summary>World positions of group members who share map tracking in the selected group.</summary>
    public readonly List<PipBoyTrackedLocation> GroupLocations;
    /// <summary>Waypoints for the currently-viewed group.</summary>
    public readonly List<PipBoyWaypoint> GroupWaypoints;

    // Directory tab
    public readonly List<PipBoyDirectoryEntry> Directory;

    // SOS Alerts
    public readonly List<PipBoySosAlert> SosAlerts;

    // Status
    public readonly PipBoyPresenceStatus PresenceStatus;
    public readonly string? StatusMessage;

    // Dead Drops
    public readonly List<PipBoyDeadDrop> NearbyDeadDrops;

    // Radio relay
    public readonly bool HasRadioConnection;

    // Faction link
    public readonly string? FactionName;
    public readonly bool CanFactionAnnounce;
    public readonly List<PipBoyFactionMember> FactionRoster;
    public readonly List<PipBoyFactionAlert> FactionAlerts;
    public readonly List<PipBoyFactionMessage> FactionMessages;
    public readonly List<PipBoyFactionTask> FactionTasks;

    public PipBoyHubUiState(
        uint ownNumber,
        bool hasPassword,
        bool isLocked,
        bool isVisible,
        Dictionary<uint, PipBoyContact> contacts,
        List<PipBoyTrackedLocation> contactLocations,
        List<PipBoyGroupInfo> groups,
        List<PipBoyGroupMessage> groupMessages,
        uint? selectedGroupId,
        List<PipBoyTrackedLocation> groupLocations,
        List<PipBoyDirectoryEntry> directory,
        List<PipBoyWaypoint> groupWaypoints,
        List<PipBoySosAlert> sosAlerts,
        PipBoyPresenceStatus presenceStatus,
        string? statusMessage,
        List<PipBoyDeadDrop> nearbyDeadDrops,
        bool hasRadioConnection,
        string? factionName,
        bool canFactionAnnounce,
        List<PipBoyFactionMember> factionRoster,
        List<PipBoyFactionAlert> factionAlerts,
        List<PipBoyFactionMessage> factionMessages,
        List<PipBoyFactionTask> factionTasks)
    {
        OwnNumber = ownNumber;
        HasPassword = hasPassword;
        IsLocked = isLocked;
        IsVisible = isVisible;
        Contacts = contacts;
        ContactLocations = contactLocations;
        Groups = groups;
        GroupMessages = groupMessages;
        SelectedGroupId = selectedGroupId;
        GroupLocations = groupLocations;
        Directory = directory;
        GroupWaypoints = groupWaypoints;
        SosAlerts = sosAlerts;
        PresenceStatus = presenceStatus;
        StatusMessage = statusMessage;
        NearbyDeadDrops = nearbyDeadDrops;
        HasRadioConnection = hasRadioConnection;
        FactionName = factionName;
        CanFactionAnnounce = canFactionAnnounce;
        FactionRoster = factionRoster;
        FactionAlerts = factionAlerts;
        FactionMessages = factionMessages;
        FactionTasks = factionTasks;
    }
}
