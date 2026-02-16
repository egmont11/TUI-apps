using System.Collections.ObjectModel;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Views;

public class TodoView
{
    private Window _window;
    private LoadDataService _loadDataService;
    private ObservableCollection<TodoItem> _todoItems;
    private ListView _listView;
    
    public TodoView(Window window)
    {
        _window = window;
        _loadDataService = new LoadDataService();
        _todoItems = [];
        _listView = new ListView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ShowMarks = false
        };
        
        _listView.Accepting += (s, e) => {
            if (_listView.SelectedItem >= 0)
            {
                var item = _todoItems[(int)_listView.SelectedItem!];
                item.Marked = !item.Marked;
                _listView.SetNeedsDraw();
                e.Handled = true;
            }
        };
        
        // testing data
        for (byte i = 0; i < 10; i++)
        {
            _todoItems.Add(new TodoItem()
            {
                Marked = false, Text = "Testing Todo Render"
            });
        }
    }

    public void Run()
    {
        UpdateListView();
        _window.Add(_listView);
    }

    private void UpdateListView()
    {
        _listView.SetSource(_todoItems);
    }
}