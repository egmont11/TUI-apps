namespace TodoList.Models;

public class TodoItem
{
    public string Text { get; set; }
    public bool Marked  { get; set; }
    
    public override string ToString()
    {
        return Marked ? $"[X]  {Text} " : $"[ ] {Text}";
    }
}