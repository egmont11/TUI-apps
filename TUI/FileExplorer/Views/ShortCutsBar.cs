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
        
        // TODO: implement these shortcuts
        var shortcuts = new Label()
        {
            Text = "Switch views: Tab | Copy: Shift+C | Paste: Shift+V | Delete: Delete",
            X = 0,
            Y = Pos.AnchorEnd(1)
        };
        
        _window.Add(shortcuts);
        _window.Add(border);
    }
}