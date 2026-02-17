using SystemMonitor.Models;
using Terminal.Gui.Views;

namespace SystemMonitor.Views;

public class SystemMonitorView : Runnable
{
    private readonly DateTime _startTime = DateTime.UtcNow;
    private Models.SystemMonitor _systemResources;
    
    public SystemMonitorView()
    {
        Title = "System Monitor";
        _systemResources = new Models.SystemMonitor();
        
        // Add the views to the Window
        Add();
    }
}