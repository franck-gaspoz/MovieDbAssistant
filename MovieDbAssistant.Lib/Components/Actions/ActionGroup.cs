using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// a group of actions
/// </summary>
[Transient]
public sealed class ActionGroup : IIdentifiable,
    ISignalMethodHandler<ActionErroredEvent>,
    ISignalMethodHandler<ActionEndedEvent>
{
    #region fields & properties

    readonly Dictionary<ActionFeatureCommandBase, ActionItem> _actionsStates = [];

    #endregion

    #region create & setup

    public ActionGroup(ISignalR signal)
    {
        var type = GetType();
        signal.Register<ActionErroredEvent>(this);
        signal.Register<ActionEndedEvent>(this);
        InstanceId = new(this);
    }

    #endregion

    #region /**----- interface IIdentifiable -----*/

    /// <inheritdoc/>
    public string Id => this.Id();

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    #endregion /----- -----/

    #region /----- dispatched signals handlers (ActionContext) ----*/

    /// <summary>
    /// handler an action errored event
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="event">event</param>
    public void Handle(object sender, ActionErroredEvent @event)
    {
        _ = sender;
        SetActionState(@event.Context, true);
    }

    /// <summary>
    /// handle an action ended event
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="event">event</param>
    public void Handle(object sender, ActionEndedEvent @event)
    {
        _ = sender;
        SetActionState(@event.Context, true);
    }

    #endregion

    #region operations

    void SetActionState(ActionContext context, bool state)
    {
        if (_actionsStates.TryGetValue(
            context.Command, out var action))
            action.IsEnded = state;
    }

    /// <summary>
    /// clear actions states
    /// </summary>
    public void Clear() => _actionsStates.Clear();

    /// <summary>
    /// add an action command instance for later tracking
    /// </summary>
    public void Add(string key, ActionFeatureCommandBase command)
        => _actionsStates.Add(
            command,
            new ActionItem(key, command, false));

    /// <summary>
    /// Wait all actions has ended (with sucess or error)
    /// </summary>
    public void WaitAll()
    {
        var end = false;
        while (!end)
        {
            end = true && _actionsStates.Count > 0;
            foreach (var kvp in _actionsStates)
                end &= kvp.Value.IsEnded;
            if (!end)
            {
                Thread.Yield();
            }
        }
    }

    /// <inheritdoc/>
    public string GetNamePrefix() => string.Empty;

    record class ActionItem
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>A <see cref="string? "/></value>
        public string? Key { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>An <see cref="ActionFeatureCommandBase? "/></value>
        public ActionFeatureCommandBase? Command { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ended.
        /// </summary>
        /// <value>A <see cref="bool"/></value>
        public bool IsEnded { get; set; }

        public ActionItem(string? key, ActionFeatureCommandBase? command, bool isEnded)
        {
            Key = key;
            Command = command;
            IsEnded = isEnded;
        }
    }

    #endregion
}
