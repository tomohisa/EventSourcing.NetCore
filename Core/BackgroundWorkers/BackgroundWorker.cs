﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.BackgroundWorkers;

public class BackgroundWorker: BackgroundService
{
//     private Task? executingTask;
//     private CancellationTokenSource? cts;
    private readonly ILogger<BackgroundWorker> logger;
    private readonly Func<CancellationToken, Task> perform;

    public BackgroundWorker(
        ILogger<BackgroundWorker> logger,
        Func<CancellationToken, Task> perform
    )
    {
        this.logger = logger;
        this.perform = perform;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            await Task.Yield();
            logger.LogInformation("Background worker stopped");
            await perform(stoppingToken);
            logger.LogInformation("Background worker stopped");
        }, stoppingToken);
    }

    // public Task StartAsync(CancellationToken cancellationToken)
    // {
    //     // Create a linked token so we can trigger cancellation outside of this token's cancellation
    //     cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    //
    //     executingTask = Task.Run(() => perform(cts.Token), cancellationToken);
    //
    //     return executingTask;
    // }
    //
    // public async Task StopAsync(CancellationToken cancellationToken)
    // {
    //     // Stop called without start
    //     if (executingTask == null)
    //         return;
    //
    //     // Signal cancellation to the executing method
    //     cts?.Cancel();
    //
    //     // Wait until the issue completes or the stop token triggers
    //     await Task.WhenAny(executingTask, Task.Delay(-1, cancellationToken));
    //
    //     // Throw if cancellation triggered
    //     cancellationToken.ThrowIfCancellationRequested();
    //
    //     logger.LogInformation("Background worker stopped");
    // }
    //
    // public void Dispose()
    // {
    //     cts?.Dispose();
    // }
}
