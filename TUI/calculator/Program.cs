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

        var plusButton = new Button("Plus")
        {
            X = 2,
            Y = 8,
        };
        window.Add(plusButton);
        var minusButton = new Button("Minus")
        {
            X = 10,
            Y = 8
        };
        window.Add(minusButton);

        var multiplyButton = new Button("Multiply")
        {
            X = 22,
            Y = 8
        };
        window.Add(multiplyButton);

        var divisionButton = new Button("Division")
        {
            X = 35,
            Y = 8
        };
        window.Add(divisionButton);
        
        var resultText = new Label("Result: ")
        {
            X = 2,
            Y = 6,
        };
        window.Add(resultText); 
        
        // events
        minusButton.Clicked += () =>
        {
            try
            {
                var numberOne = Convert.ToInt32(inputOne.Text);
                var numberTwo = Convert.ToInt32(inputTwo.Text);

                resultText.Text = $"Result: {numberOne - numberTwo}";
            }
            catch (Exception ex)
            {
                resultText.Text = ex.Message;
            }
        };

        plusButton.Clicked += () =>
        {
            try {
                var numberOne = Convert.ToInt32(inputOne.Text);
                var numberTwo = Convert.ToInt32(inputTwo.Text);
                resultText.Text = $"Result: {numberOne + numberTwo}";
            }
            catch (Exception ex)
            {
                resultText.Text = ex.Message;
            }
        };

        multiplyButton.Clicked += () =>
        {
            try
            { 
                var numberOne = Convert.ToInt32(inputOne.Text);
                var numberTwo = Convert.ToInt32(inputTwo.Text);
                resultText.Text = $"Result: {numberOne * numberTwo}";
            }
            catch (Exception ex)
            {
                resultText.Text = ex.Message;
            }
        };

        divisionButton.Clicked += () =>
        {
            try
            {
                var numberOne = Convert.ToInt32(inputOne.Text);
                var numberTwo = Convert.ToInt32(inputTwo.Text);
                resultText.Text = $"Result: {numberOne / numberTwo}";
            }
            catch (Exception ex)
            {
                resultText.Text = ex.Message;
            }
            
        };
        
        Application.Run();
        Application.Shutdown();
    }
}