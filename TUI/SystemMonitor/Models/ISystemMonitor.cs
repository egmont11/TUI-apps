namespace SystemMonitor.Models;

public interface ISystemMonitor
{
    double CpuUsage { get; }
    void Start(int intervalMs);
    void Stop();
}