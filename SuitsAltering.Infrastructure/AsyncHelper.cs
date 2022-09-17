using System.Globalization;

namespace SuitsAltering.Infrastructure;

public class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        CultureInfo cultureUi = CultureInfo.CurrentUICulture;
        CultureInfo culture = CultureInfo.CurrentCulture;
        return AsyncHelper.TaskFactory.StartNew<Task<TResult>>((Func<Task<TResult>>)(() =>
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = cultureUi;
            return func();
        })).Unwrap<TResult>().GetAwaiter().GetResult();
    }

    public static void RunSync(Func<Task> func)
    {
        CultureInfo cultureUi = CultureInfo.CurrentUICulture;
        CultureInfo culture = CultureInfo.CurrentCulture;
        AsyncHelper.TaskFactory.StartNew<Task>((Func<Task>)(() =>
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = cultureUi;
            return func();
        })).Unwrap().GetAwaiter().GetResult();
    }
}