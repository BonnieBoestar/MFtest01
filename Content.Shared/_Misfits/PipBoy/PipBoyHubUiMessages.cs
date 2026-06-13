// #Misfits Add - PipBoy Hub UI message events (client → server).
using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._Misfits.PipBoy;

/// <summary>
/// Message sent from the PipBoy Hub UI to the server.
/// </summary>
[Serializable, NetSerializable]
public sealed class PipBoyHubUiMessageEvent : CartridgeMessageEvent
{
    public readonly PipBoyHubMessageType Type;

    /// <summary>Target PipBoy/NanoChat number (for contacts/directory actions).</summary>
    public readonly uint? TargetNumber;

    /// <summary>Group ID (for group actions).</summary>
    public readonly uint? GroupId;

    /// <summary>Text content (group name, message text, password, etc.).</summary>
    public readonly string? Content;

    public PipBoyHubUiMessageEvent(PipBoyHubMessageType type,
        uint? targetNumber = null,
        uint? groupId = null,
        string? content = null)
    {
        Type = type;
        TargetNumber = targetNumber;
        GroupId = groupId;
        Content = content;
    }
}

[Serializable, NetSerializable]
public enum PipBoyHubMessageType : byte
{
    // ── Contacts ──
    /// <summary>Send a contact request to TargetNumber.</summary>
    ContactAdd,
    /// <summary>Accept an incoming contact request from TargetNumber.</summary>
    ContactAccept,
    /// <summary>Reject / remove a contact by TargetNumber.</summary>
    ContactReject,
    /// <summary>Toggle whether we share our location with TargetNumber.</summary>
    ContactToggleLocation,
    /// <summary>Quick-message a contact. Content = message text, TargetNumber = recipient.</summary>
    ContactSendMessage,
    /// <summary>Set personal notes on a contact. TargetNumber + Content = notes text.</summary>
    ContactSetNote,

    // ── Groups ──
    /// <summary>Create a new group. Content = group name.</summary>
    GroupCreate,
    /// <summary>Join a group by GroupId. Content = password (for encrypted groups).</summary>
    GroupJoin,
    /// <summary>Leave a group by GroupId.</summary>
    GroupLeave,
    /// <summary>Toggle our map tracking for GroupId.</summary>
    GroupToggleMapTracking,
    /// <summary>Send a group message. GroupId + Content.</summary>
    GroupSendMessage,
    /// <summary>Select a group to view its messages/tracking. GroupId.</summary>
    GroupSelect,
    /// <summary>Add a waypoint at sender's position. GroupId + Content = label.</summary>
    GroupAddWaypoint,
    /// <summary>Remove a waypoint by index. GroupId + Content = index string.</summary>
    GroupRemoveWaypoint,
    /// <summary>Set or clear group password. GroupId + Content = password (empty to remove).</summary>
    GroupSetPassword,

    // ── Directory ──
    /// <summary>Toggle our own visibility in the directory.</summary>
    DirectoryToggleVisibility,
    /// <summary>Send message to a directory entry. TargetNumber + Content.</summary>
    DirectorySendMessage,

    // ── Password / Lock ──
    /// <summary>Set or change password. Content = new password (empty to remove).</summary>
    PasswordSet,
    /// <summary>Attempt to unlock. Content = password attempt.</summary>
    PasswordUnlock,
    /// <summary>Lock the PipBoy Hub manually.</summary>
    Lock,

    // ── SOS Beacon ──
    /// <summary>Broadcast SOS emergency beacon to all accepted contacts.</summary>
    SosBeacon,

    // ── Status ──
    /// <summary>Set presence status. Content = "statusEnum|optional status message".</summary>
    SetStatus,

    // ── Dead Drops ──
    /// <summary>Create a dead drop at current position. Content = message, TargetNumber = optional recipient.</summary>
    DeadDropCreate,
    /// <summary>Collect (read and remove) a nearby dead drop. Content = drop ID string.</summary>
    DeadDropCollect,
    /// <summary>Send a faction communications message. Content = message text.</summary>
    FactionSendMessage,
    /// <summary>Send a faction announcement. Content = message text.</summary>
    FactionSendAnnouncement,
    /// <summary>Acknowledge a faction alert. Content = alert ID.</summary>
    FactionAcknowledgeAlert,
    /// <summary>Create a faction task. Content = title or title\0assignee number.</summary>
    FactionCreateTask,
    /// <summary>Toggle task completion. Content = task ID.</summary>
    FactionToggleTask,
}
