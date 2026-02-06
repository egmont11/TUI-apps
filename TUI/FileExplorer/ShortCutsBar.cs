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
        var lastRow = _window.Frame.Bottom - 2;

        var border = new Line()
        {
            X = 0,
            Y = lastRow - 1
        };
        
        var exampleText = new Label()
        {
            X = 12,
            Y = lastRow,
        };
        
        _window.Add(exampleText);
        _window.Add(border);
    }
}