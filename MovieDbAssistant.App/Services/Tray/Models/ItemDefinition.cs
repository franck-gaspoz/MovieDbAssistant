namespace MovieDbAssistant.App.Services.Tray.Models;

/// <summary>
/// The item definition.
/// </summary>
public record ItemDefinition(ToolStripItem Item, Action<ToolStripItem>? Init)
{
    public static implicit operator (ToolStripItem item, Action<ToolStripItem>? init)(ItemDefinition value) => (value.Item, value.Init);
    public static implicit operator ItemDefinition((ToolStripItem item, Action<ToolStripItem>? init) value) => new(value.item, value.init);
}