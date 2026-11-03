using System;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;

namespace SCNET_Restart_Tool.Uno;

/// <summary>
/// DispatcherQueue 异步扩展
/// </summary>
public static class DispatcherQueueExtensions
{
    /// <summary>在 UI 线程上异步执行操作</summary>
    public static Task EnqueueAsync(this DispatcherQueue dispatcher, Func<Task> action)
    {
        var tcs = new TaskCompletionSource<bool>();
        dispatcher.TryEnqueue(async () =>
        {
            try
            {
                await action();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }

    /// <summary>在 UI 线程上异步执行操作</summary>
    public static Task EnqueueAsync(this DispatcherQueue dispatcher, Action action)
    {
        var tcs = new TaskCompletionSource<bool>();
        dispatcher.TryEnqueue(() =>
        {
            try
            {
                action();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }
}
