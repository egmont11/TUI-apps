using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TodoList.Views;

namespace TodoList;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        var window = new Window()
        {
            Title = "Todo list",
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        var view = new TodoView(window);
        view.Run();
        
        Application.Run(window);
        Application.Shutdown();
    }
}