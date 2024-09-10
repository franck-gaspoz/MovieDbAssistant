using MovieDbAssistant.App.Services.Build;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// menu item states setter
/// </summary>
[Singleton]
sealed class TrayMenuItemsStates :
    ISignalHandler<BuildCompletedEvent>
{
    readonly TrayMenuItems _trayMenuItems;
    readonly BuiIdInputFolderUIService _buildInputFolderService;

    public TrayMenuItemsStates(
        TrayMenuItems trayMenuItems,
        BuiIdInputFolderUIService buildInputFolderService)
    {
        _trayMenuItems = trayMenuItems;
        _buildInputFolderService = buildInputFolderService;
    }

    public void Handle(object sender, BuildCompletedEvent @event) 
        => _trayMenuItems.SetBuildItemsEnabled(
            true && !_buildInputFolderService.Buzy);
}
