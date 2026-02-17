using SystemMonitor.Views;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;

namespace SystemMonitor;

class Program
{
    // trying the correct "modern" way of doing terminal.gui v2
    static void Main(string[] args)
    {
        ConfigurationManager.RuntimeConfig = """{ "Theme": "Amber Phosphor" }""";
        ConfigurationManager.Enable(ConfigLocations.All);
        
        var app = Application.Create().Init();
        app.Run<SystemMonitorView>();
        app.Dispose();
    }
}