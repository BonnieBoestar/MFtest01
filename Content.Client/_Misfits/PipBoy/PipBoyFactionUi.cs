using Content.Client.UserInterface.Fragments;
using Content.Shared._Misfits.PipBoy;
using Content.Shared.CartridgeLoader;
using Robust.Client.UserInterface;

namespace Content.Client._Misfits.PipBoy;

public sealed partial class PipBoyFactionUi : UIFragment
{
    private PipBoyFactionUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new PipBoyFactionUiFragment();

        _fragment.OnSendMessage += (type, targetNumber, groupId, content) =>
        {
            var msg = new PipBoyHubUiMessageEvent(type, targetNumber, groupId, content);
            userInterface.SendMessage(new CartridgeUiMessage(msg));
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is PipBoyHubUiState cast)
            _fragment?.UpdateState(cast);
    }
}
