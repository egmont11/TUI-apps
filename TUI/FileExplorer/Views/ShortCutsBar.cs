using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FileExplorer;

public class ShortCutsBar
{
    private readonly Window _window;

    public ShortCutsBar(Window window)
    {
        _window = window;
    }

    public void Show()
    {
        var border = new Line()
        {
            X = 0,
            Y = Pos.AnchorEnd(2)
        };
        
        // TODO: implement this shortcut
        var exampleText = new Label()
        {
            Text = "Switch views: Tab",
            X = 1,
            Y = Pos.AnchorEnd(1)
        };
        
        _window.Add(exampleText);
        _window.Add(border);
    }
}