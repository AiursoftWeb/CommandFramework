using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Aiursoft.CommandFramework.ProgressBar;

/// <summary>
/// An ASCII progress bar with elapsed time and remaining time (ETA as TimeSpan).
/// </summary>
[ExcludeFromCodeCoverage]
public class ProgressBar : IDisposable, IProgress<double>
{
    private const int BlockCount = 50;
    private const string Animation = @"|/-\";
    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);
    private readonly TimeSpan _sampleWindow = TimeSpan.FromSeconds(10);

    private readonly Timer _timer;
    private readonly DateTime _startTime;
    private readonly List<(DateTime Timestamp, double Progress)> _samples = new();

    private double _currentProgress;
    private string _currentText = string.Empty;
    private bool _disposed;
    private int _animationIndex;

    public ProgressBar()
    {
        _startTime = DateTime.UtcNow;
        _timer = new Timer(TimerHandler);

        if (!Console.IsOutputRedirected)
        {
            ResetTimer();
        }
    }

    /// <summary>
    /// Report a progress value in [0..1].
    /// </summary>
    public void Report(double value)
    {
        value = Math.Max(0, Math.Min(1, value));
        Interlocked.Exchange(ref _currentProgress, value);
    }

    private void TimerHandler(object? state)
    {
        lock (_timer)
        {
            if (_disposed) return;

            var now = DateTime.UtcNow;
            var progress = Interlocked.CompareExchange(ref _currentProgress, 0.0, 0.0);

            _samples.Add((now, progress));
            _samples.RemoveAll(s => (now - s.Timestamp) > _sampleWindow);

            double speed = 0;
            if (_samples.Count >= 2)
            {
                var first = _samples[0];
                var last = _samples[^1];
                var dt = (last.Timestamp - first.Timestamp).TotalSeconds;
                var dp = last.Progress - first.Progress;
                if (dt > 0) speed = dp / dt;
            }

            string remainingText;
            if (speed > 0 && progress < 1.0)
            {
                var secsRemaining = (1.0 - progress) / speed;
                var remaining = TimeSpan.FromSeconds(secsRemaining);
                remainingText = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
            else
            {
                remainingText = "--:--:--";
            }

            var elapsed = now - _startTime;
            var elapsedText = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            var filled = (int)(progress * BlockCount);
            var text =
                $"[{new string('#', filled)}{new string('-', BlockCount - filled)}] " +
                $"{(int)(progress * 100),3}% {Animation[_animationIndex++ % Animation.Length]} " +
                $"[Elapsed: {elapsedText}] [Remaining: {remainingText}]";

            UpdateText(text);
            ResetTimer();
        }
    }

    private void UpdateText(string text)
    {
        var common = 0;
        var max = Math.Min(_currentText.Length, text.Length);
        while (common < max && _currentText[common] == text[common])
        {
            common++;
        }

        var output = new StringBuilder();
        output.Append('\b', _currentText.Length - common);
        output.Append(text[common..]);

        var overlap = _currentText.Length - text.Length;
        if (overlap > 0)
        {
            output.Append(' ', overlap)
                .Append('\b', overlap);
        }

        Console.Write(output);
        _currentText = text;
    }

    private void ResetTimer()
        => _timer.Change(_animationInterval, Timeout.InfiniteTimeSpan);

    public void Dispose()
    {
        lock (_timer)
        {
            _disposed = true;
            UpdateText(string.Empty);
        }

        _timer.Dispose();
    }
}
