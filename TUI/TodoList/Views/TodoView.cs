using System.Collections.ObjectModel;
using System.Drawing;
using Terminal.Gui;
using Terminal.Gui.Input;
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
    private TextField _newItemTextField;
    private Label _newItemLabel;
    
    public TodoView(Window window)
    {
        _window = window;
        _loadDataService = new LoadDataService();
        _todoItems = [];
        _listView = new ListView
        {
            X = 0,
            Y = 2,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 2,
            ShowMarks = false
        };
        _newItemLabel = new Label
        {
            X = 0,
            Y = 0,
            Text = "New Item: "
        };
        _newItemTextField = new TextField
        {
            X = Pos.Right(_newItemLabel),
            Y = 0,
            Width = Dim.Fill()
        };
    }

    public void Run()
    {
        _todoItems = LoadDataService.GetItems();
        
        UpdateListView();
        
        _listView.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Key.Delete)
            {
                if (!(_listView.SelectedItem >= 0)) return;
                var item = _todoItems[(int)_listView.SelectedItem!];
                _todoItems.Remove(item);
                UpdateListView();
                SetTitle();
                e.Handled = true;
            }
        };

        _listView.Accepting += (s, e) =>
        {
            if (!(_listView.SelectedItem >= 0)) return;

            var item = _todoItems[(int)_listView.SelectedItem!];
            item.Marked = !item.Marked;

            _listView.SetNeedsDraw();
            e.Handled = true;

            SetTitle();
            _listView.HasFocus = true;
        };
        
        var saveButton = new Button
        {
            Text = "Save",
            X = Pos.AnchorEnd(12),
            Y = Pos.AnchorEnd(1)
        };
        saveButton.Accepting += (s, e) => SaveList();
        _window.Add(saveButton);
        
        _window.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Key.S.WithCtrl)
            {
                SaveList();
                e.Handled = true;
            }
        };
        
        _window.Add(_listView);
        _window.Add(_newItemLabel);
        _window.Add(_newItemTextField);
        
        _newItemTextField.Accepting += (s, e) =>
        {
            var text = _newItemTextField.Text;
            if (string.IsNullOrWhiteSpace(text)) return;
            
            _todoItems.Add(new TodoItem { Text = text, Marked = false });
            _newItemTextField.Text = "";
            UpdateListView();
            SetTitle();
            
            e.Handled = true;
        };
    }

    private void UpdateListView()
    {
        _listView.SetSource(_todoItems);
    }

    private void SaveList()
    {
        LoadDataService.SaveItems(_todoItems);
    }

    private void SetTitle()
    {
        _window.Title = $"{_todoItems.Count(i => !i.Marked)} Todos";
    }
}