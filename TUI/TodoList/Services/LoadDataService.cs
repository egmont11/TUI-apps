using System.Collections.ObjectModel;
using System.Text.Json;
using TodoList.Models;

namespace TodoList.Services;

public class LoadDataService
{
    public LoadDataService()
    {
    }

    public static ObservableCollection<TodoItem> GetItems()
    {
        if (!File.Exists("./data.json"))
        {
            File.Create("./data.json").Close();
            File.WriteAllText("./data.json", "[]");
            return [];
        }

        try
        {
            var rawText = File.ReadAllText("./data.json");
            var items =
                JsonSerializer.Deserialize<ObservableCollection<TodoItem>>(rawText)
                ?? [];

            return items;
        }
        catch (Exception e)
        {
            return [];
        }
    }

    public static void SaveItems(ObservableCollection<TodoItem> items)
    {
        var rawText = JsonSerializer.Serialize(items);
        File.WriteAllText("./data.json", rawText);
    }
}