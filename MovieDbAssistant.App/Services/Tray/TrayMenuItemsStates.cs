using MediatR;

using MovieDbAssistant.App.Events;
using MovieDbAssistant.App.Features;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// menu item states setter
/// </summary>
sealed class TrayMenuItemsStates :
    IRequestHandler<BuildEndedEvent>
{
    readonly TrayMenuItems _trayMenuItems;

    public TrayMenuItemsStates(TrayMenuItems trayMenuItems)
        => _trayMenuItems = trayMenuItems;

    public async Task Handle(
        BuildEndedEvent request,
        CancellationToken _)
    {
        _trayMenuItems.SetBuildItemsEnabled(true && !ProcessInputFolder.Buzy);
        await Task.CompletedTask;
    }
}
