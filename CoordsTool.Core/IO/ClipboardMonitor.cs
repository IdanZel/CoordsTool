using TextCopy;

namespace CoordsTool.Core.IO;

public class ClipboardMonitor : IDisposable
{
    private static readonly TimeSpan ReadingInterval = TimeSpan.FromMilliseconds(500);

    private readonly Timer _readTimer;
    private readonly Action<string> _onClipboardUpdated;

    private string _lastClipboardText;

    public ClipboardMonitor(Action<string> onClipboardUpdated)
    {
        _readTimer = new Timer(ReadFromClipboard, null, ReadingInterval, ReadingInterval);
        _onClipboardUpdated = onClipboardUpdated;
        _lastClipboardText = string.Empty;
    }

    public void Enable()
    {
        _readTimer.Change(ReadingInterval, ReadingInterval);
    }

    public void Disable()
    {
        _readTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    private async void ReadFromClipboard(object? state)
    {
        var clipboardText = await ClipboardService.GetTextAsync();

        if (clipboardText is null || clipboardText.Equals(_lastClipboardText, StringComparison.Ordinal))
        {
            return;
        }

        _onClipboardUpdated(clipboardText);
        _lastClipboardText = clipboardText;
    }

    public void Dispose()
    {
        _readTimer.Dispose();
        GC.SuppressFinalize(this);
    }
}