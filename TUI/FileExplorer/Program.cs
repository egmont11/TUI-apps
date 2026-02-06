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
        var shortCutsBar = new ShortCutsBar(window);
        
        shortCutsBar.Show();
        app.Run(window);
        
        app.Dispose();
    }
}