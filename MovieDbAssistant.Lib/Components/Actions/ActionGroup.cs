using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// a group of actions
/// </summary>
[Transient]
public sealed class ActionGroup : IActionFeature
{
    #region fields & properties

    readonly Dictionary<ActionFeatureCommandBase, ActionItem> _actionsStates = [];

    #endregion

    #region create & setup

    public ActionGroup() => InstanceId = new(this);

    #endregion

    #region /**----- interface IActionFeature -----*/

    /// <inheritdoc/>
    public string Id => this.Id();

    /// <inheritdoc/>
    public bool RunInBackground => false;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    /// <inheritdoc/>
    public void End(ActionContext context, bool error = false)
        => SetActionState(context, true);

    /// <inheritdoc/>
    public void Error(ActionErroredEvent @event)
        => SetActionState(@event.Context, true);

    /// <inheritdoc/>
    public void OnFinally(ActionContext context)
    {
    }

    #endregion /----- -----/

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
    /// add an action command instance for tracking later
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
        bool end = true;
        while (!end)
        {
            end = true;
            foreach (var kvp in _actionsStates)
                end &= kvp.Value.IsEnded;
            if (!end)
            {
                Thread.Yield();                
            }
        }
    }

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
