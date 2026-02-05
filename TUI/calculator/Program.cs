using Terminal.Gui;

namespace calculator;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        
        var top = Application.Top;
        var window = new Window("Calculator")
        {
            X = Pos.Center(), Y = Pos.Center(),
            Width = Dim.Fill(), Height = Dim.Fill()
        };
        top.Add(window);
        
        var inputOneText = new Label("Input One: ")
        {
            X = 2, 
            Y = 2
        };
        var inputOne = new TextField("")
        {
            X = 14, 
            Y = 2,
            Width = 20
        };
        window.Add(inputOneText);
        window.Add(inputOne);

        var inputTwoText = new Label("Input Two: ")
        {
            X = 2,
            Y = 4
        };
        var inputTwo = new TextField("")
        {
            X = 14, 
            Y = 4,
            Width = 20
        };
        
        window.Add(inputTwoText);
        window.Add(inputTwo);
        
        Application.Run();
        Application.Shutdown();
    }
}