using System.Diagnostics;
using System.Runtime.InteropServices;
using NickStrupat;

namespace SystemMonitor.Models;

public class SystemMonitor : ISystemMonitor, IDisposable
{
    private OSPlatform _osPlatform;
    private Timer? _timer;
    private readonly object _lock = new();
    private long _prevIdleTime;
    private long _prevTotalTime;
    private PerformanceCounter? _counter;

    public double CpuUsage
    {
        get { lock (_lock) return field; }
        private set { lock (_lock) field = value; }
    }

    // Total physical RAM installed
    public double TotalAvailableMemory
    {
        get { lock (_lock) return field; }
        private set { lock (_lock) field = value; }
    }

    // Currently used RAM
    public double TotalUsedMemory
    {
        get { lock (_lock) return field; }
        private set { lock (_lock) field = value; }
    }

    public SystemMonitor()
    {
        _osPlatform = OperatingSystem.IsWindows() ? OSPlatform.Windows : OSPlatform.Linux;

        if (_osPlatform == OSPlatform.Windows)
        {
            var computerInfo = new ComputerInfo();
            TotalAvailableMemory = computerInfo.TotalPhysicalMemory;
            _counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }
        else if (_osPlatform == OSPlatform.Linux)
        {
            foreach (var line in File.ReadLines("/proc/meminfo"))
            {
                if (!line.StartsWith("MemTotal:")) continue;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var kb = ulong.Parse(parts[1]);
                TotalAvailableMemory = kb * 1024;
                break;
            }
        }
    }

    public void Start(int intervalMs = 1000)
    {
        _timer = new Timer(UpdateSystemUsage, null, 0, intervalMs);
    }

    public void Stop()
    {
        _timer?.Dispose();
        _timer = null;
    }

    public void Dispose()
    {
        Stop();
    }

    private void UpdateSystemUsage(object? state)
    {
        UpdateCpuUsage();
        UpdateRamUsage();
    }

    // ---------------- CPU ----------------

    private void UpdateCpuUsage()
    {
        if (_osPlatform == OSPlatform.Linux)
            UpdateLinuxCpuUsage();
        else
            UpdateWindowsCpuUsage();
    }

    private void UpdateWindowsCpuUsage()
    {
        _counter.NextValue();
        Thread.Sleep(500);
        CpuUsage = Math.Round(_counter.NextValue(), 2);
    }

    private void UpdateLinuxCpuUsage()
    {
        var parts = File.ReadAllText("/proc/stat")
                        .Split('\n')[0]
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var user = long.Parse(parts[1]);
        var nice = long.Parse(parts[2]);
        var system = long.Parse(parts[3]);
        var idle = long.Parse(parts[4]);

        var total = user + nice + system + idle;

        var totalDiff = total - _prevTotalTime;
        var idleDiff = idle - _prevIdleTime;

        if (_prevTotalTime != 0 && totalDiff > 0)
        {
            CpuUsage = Math.Round((1.0 - ((double)idleDiff / totalDiff)) * 100, 2);
        }

        _prevTotalTime = total;
        _prevIdleTime = idle;
    }

    // ---------------- RAM ----------------

    private void UpdateRamUsage()
    {
        if (_osPlatform == OSPlatform.Linux)
            UpdateLinuxRamUsage();
        else
            UpdateWindowsRamUsage();
    }

    private void UpdateWindowsRamUsage()
    {
        var computerInfo = new ComputerInfo();
        TotalUsedMemory =
            computerInfo.TotalPhysicalMemory -
            computerInfo.AvailablePhysicalMemory;
    }

    private void UpdateLinuxRamUsage()
    {
        ulong memTotalKb = 0;
        ulong memAvailableKb = 0;

        foreach (var line in File.ReadLines("/proc/meminfo"))
        {
            if (line.StartsWith("MemTotal:"))
                memTotalKb = ulong.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

            else if (line.StartsWith("MemAvailable:"))
                memAvailableKb = ulong.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

            if (memTotalKb != 0 && memAvailableKb != 0)
                break;
        }

        if (memTotalKb == 0 || memAvailableKb == 0)
            return;

        TotalUsedMemory = (memTotalKb - memAvailableKb) * 1024;
    }
}
