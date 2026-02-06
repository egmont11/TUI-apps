using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace FileExplorer;

class Program
{
    static void Main(string[] args)
    {
        var app = Application.Create();
        app.Init();

        var window = new Window()
        {
            Title = "File Explorer"
        };
        
        app.Run(window);
        
        app.Dispose();
    }
}