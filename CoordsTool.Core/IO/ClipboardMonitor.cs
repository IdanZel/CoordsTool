using TextCopy;

namespace CoordsTool.Core.IO;

public class ClipboardMonitor : IDisposable
{
    private static readonly TimeSpan ReadingInterval = TimeSpan.FromMilliseconds(500);

    private readonly Timer _readTimer;
    private readonly Action<string> _onClipboardUpdated;

    private string _lastClipboardText;
    private bool _isEnabled;

    public ClipboardMonitor(Action<string> onClipboardUpdated)
    {
        _readTimer = new Timer(ReadFromClipboard, null, ReadingInterval, ReadingInterval);
        _onClipboardUpdated = onClipboardUpdated;
        _lastClipboardText = string.Empty;
        _isEnabled = true;
    }

    public void Enable()
    {
        if (_isEnabled)
        {
            return;
        }

        _readTimer.Change(ReadingInterval, ReadingInterval);
        _isEnabled = true;
    }

    public void Disable()
    {
        if (!_isEnabled)
        {
            return;
        }

        _readTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        _isEnabled = false;
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