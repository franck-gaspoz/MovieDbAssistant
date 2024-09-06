﻿using MediatR;

namespace MovieDbAssistant.App.Components;

class CommandHandlerBase<TCommand> : IRequestHandler<TCommand>
    where TCommand : IRequest
{
    protected Action<TCommand, CancellationToken>? Handler { get; set; }

    /// <summary>
    /// handle command
    /// </summary>
    /// <param name="com">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>    
    /// <returns>A <see cref="Task"/></returns>
    public async Task Handle(
        TCommand com,
        CancellationToken cancellationToken)
    {
        Handler?.Invoke(com, cancellationToken);
        await Task.CompletedTask;
    }
}