// #Misfits Add - PipBoy Network data structures for contacts, groups, directory, and group messages.
using Content.Shared.Mobs;
using Robust.Shared.Serialization;

namespace Content.Shared._Misfits.PipBoy;

/// <summary>
/// Status of a contact relationship between two PipBoy users.
/// </summary>
[Serializable, NetSerializable]
public enum PipBoyContactStatus : byte
{
    /// <summary>We sent the request; waiting for their acceptance.</summary>
    PendingOutgoing,
    /// <summary>They sent us a request; waiting for our response.</summary>
    PendingIncoming,
    /// <summary>Both parties accepted.</summary>
    Accepted,
}

/// <summary>
/// Presence status shown in the directory and contact list.
/// </summary>
[Serializable, NetSerializable]
public enum PipBoyPresenceStatus : byte
{
    Available,
    Busy,
    DoNotDisturb,
    Away,
}

/// <summary>
/// A contact entry stored on a PipBoy network component.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyContact
{
    public uint Number;
    public string Name;
    public string? JobTitle;
    public PipBoyContactStatus Status;
    /// <summary>Whether WE share our location with them.</summary>
    public bool SharingLocation;
    /// <summary>Whether THEY share their location with us (mirror of their SharingLocation toward us).</summary>
    public bool TheyShareLocation;
    /// <summary>Personal notes about this contact (only visible to the owner).</summary>
    public string? Notes;

    public PipBoyContact(uint number, string name, string? jobTitle, PipBoyContactStatus status,
        bool sharingLocation = false, bool theyShareLocation = false, string? notes = null)
    {
        Number = number;
        Name = name;
        JobTitle = jobTitle;
        Status = status;
        SharingLocation = sharingLocation;
        TheyShareLocation = theyShareLocation;
        Notes = notes;
    }
}

/// <summary>
/// Tracks a player's membership and preferences in a specific group.
/// Stored per-member on <see cref="PipBoyNetworkComponent"/>.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyGroupMembership
{
    public uint GroupId;
    public string GroupName;
    public bool MapTrackingEnabled;

    public PipBoyGroupMembership(uint groupId, string groupName, bool mapTrackingEnabled = false)
    {
        GroupId = groupId;
        GroupName = groupName;
        MapTrackingEnabled = mapTrackingEnabled;
    }
}

/// <summary>
/// Summary sent to the client about a group the player belongs to.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyGroupInfo
{
    public uint GroupId;
    public string GroupName;
    public int MemberCount;
    public bool MapTrackingEnabled;
    public bool IsEncrypted;

    public PipBoyGroupInfo(uint groupId, string groupName, int memberCount, bool mapTrackingEnabled, bool isEncrypted = false)
    {
        GroupId = groupId;
        GroupName = groupName;
        MemberCount = memberCount;
        MapTrackingEnabled = mapTrackingEnabled;
        IsEncrypted = isEncrypted;
    }
}

/// <summary>
/// A world-position entry for a tracked entity shown on the map or coordinate readout.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyTrackedLocation
{
    public uint Number;
    public string Name;
    public float X;
    public float Y;

    public PipBoyTrackedLocation(uint number, string name, float x, float y)
    {
        Number = number;
        Name = name;
        X = x;
        Y = y;
    }
}

/// <summary>
/// An entry in the PipBoy directory/masterlist showing a visible PipBoy.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyDirectoryEntry
{
    public uint Number;
    public string Name;
    public string? JobTitle;
    public PipBoyPresenceStatus PresenceStatus;
    public string? StatusMessage;

    public PipBoyDirectoryEntry(uint number, string name, string? jobTitle = null,
        PipBoyPresenceStatus presenceStatus = PipBoyPresenceStatus.Available, string? statusMessage = null)
    {
        Number = number;
        Name = name;
        JobTitle = jobTitle;
        PresenceStatus = presenceStatus;
        StatusMessage = statusMessage;
    }
}

/// <summary>
/// A group chat message.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyGroupMessage
{
    // #Misfits Change - Increased from 256 to 1024 so longer RP messages aren't truncated
    public const int MaxContentLength = 1024;

    public TimeSpan Timestamp;
    public string Content;
    public uint SenderId;
    public string SenderName;

    public PipBoyGroupMessage(TimeSpan timestamp, string content, uint senderId, string senderName)
    {
        Timestamp = timestamp;
        Content = content;
        SenderId = senderId;
        SenderName = senderName;
    }
}

/// <summary>
/// Server-side group state stored in <see cref="Content.Server._Misfits.PipBoy.PipBoyNetworkSystem"/>.
/// NOT serialized to the network — only <see cref="PipBoyGroupInfo"/> is sent to clients.
/// </summary>
public sealed class PipBoyGroupData
{
    public uint GroupId;
    public string GroupName = string.Empty;
    public uint CreatorNumber;
    public HashSet<uint> Members = new();
    public List<PipBoyGroupMessage> Messages = new();
    /// <summary>Encrypted channel password. Null = open group.</summary>
    public string? Password;
    public List<PipBoyWaypoint> Waypoints = new();
}

/// <summary>
/// An SOS emergency beacon alert received from a contact.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoySosAlert
{
    public uint SenderNumber;
    public string SenderName;
    public float X;
    public float Y;
    public TimeSpan Timestamp;

    public PipBoySosAlert(uint senderNumber, string senderName, float x, float y, TimeSpan timestamp)
    {
        SenderNumber = senderNumber;
        SenderName = senderName;
        X = x;
        Y = y;
        Timestamp = timestamp;
    }
}

/// <summary>
/// A named waypoint placed by a group member, visible to all members.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyWaypoint
{
    public string Label;
    public float X;
    public float Y;
    public string SetByName;

    public PipBoyWaypoint(string label, float x, float y, string setByName)
    {
        Label = label;
        X = x;
        Y = y;
        SetByName = setByName;
    }
}

/// <summary>
/// A virtual dead drop message left at a world position.
/// </summary>
[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyDeadDrop
{
    // #Misfits Change - Increased from 256 to 1024 so longer RP messages aren't truncated
    public const int MaxContentLength = 1024;
    public const float PickupRange = 10f;

    public uint Id;
    public uint SenderNumber;
    public string SenderName;
    public string Content;
    public float X;
    public float Y;
    public TimeSpan Timestamp;
    /// <summary>If set, only this PipBoy number can see the drop.</summary>
    public uint? TargetNumber;

    public PipBoyDeadDrop(uint id, uint senderNumber, string senderName, string content,
        float x, float y, TimeSpan timestamp, uint? targetNumber = null)
    {
        Id = id;
        SenderNumber = senderNumber;
        SenderName = senderName;
        Content = content;
        X = x;
        Y = y;
        Timestamp = timestamp;
        TargetNumber = targetNumber;
    }
}

[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyFactionMember
{
    public uint Number;
    public string Name;
    public string? JobTitle;
    public float Health;
    public MobState State;
    public string LocationName;
    public bool IsOnline;
    public TimeSpan LastSeenAt;

    public PipBoyFactionMember(
        uint number,
        string name,
        string? jobTitle,
        float health,
        MobState state,
        string locationName,
        bool isOnline,
        TimeSpan lastSeenAt)
    {
        Number = number;
        Name = name;
        JobTitle = jobTitle;
        Health = health;
        State = state;
        LocationName = locationName;
        IsOnline = isOnline;
        LastSeenAt = lastSeenAt;
    }
}

[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyFactionAlert
{
    public uint Id;
    public TimeSpan Timestamp;
    public string Message;
    public string LocationName;

    public PipBoyFactionAlert(uint id, TimeSpan timestamp, string message, string locationName)
    {
        Id = id;
        Timestamp = timestamp;
        Message = message;
        LocationName = locationName;
    }
}

[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyFactionMessage
{
    public const int MaxContentLength = 512;

    public uint Id;
    public TimeSpan Timestamp;
    public uint SenderNumber;
    public string SenderName;
    public string Content;
    public bool IsAnnouncement;

    public PipBoyFactionMessage(
        uint id,
        TimeSpan timestamp,
        uint senderNumber,
        string senderName,
        string content,
        bool isAnnouncement)
    {
        Id = id;
        Timestamp = timestamp;
        SenderNumber = senderNumber;
        SenderName = senderName;
        Content = content;
        IsAnnouncement = isAnnouncement;
    }
}

[Serializable, NetSerializable, DataRecord]
public partial struct PipBoyFactionTask
{
    public const int MaxTitleLength = 96;

    public uint Id;
    public TimeSpan CreatedAt;
    public string Title;
    public uint? AssigneeNumber;
    public string? AssigneeName;
    public bool Completed;

    public PipBoyFactionTask(
        uint id,
        TimeSpan createdAt,
        string title,
        uint? assigneeNumber,
        string? assigneeName,
        bool completed)
    {
        Id = id;
        CreatedAt = createdAt;
        Title = title;
        AssigneeNumber = assigneeNumber;
        AssigneeName = assigneeName;
        Completed = completed;
    }
}
