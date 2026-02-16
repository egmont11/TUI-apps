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
    private readonly Window _window;
    private ObservableCollection<TodoItem> _todoItems;
    private readonly ListView _listViewUnfinished;
    private readonly ListView _listViewFinished;
    private readonly TextField _newItemTextField;
    private readonly Label _newItemLabel;
    
    public TodoView(Window window)
    {
        _window = window;
        _todoItems = [];
        _listViewUnfinished = new ListView
        {
            X = 0,
            Y = 2,
            Width = Dim.Percent(50) - 1,
            Height = Dim.Fill(1),
            ShowMarks = false
        };
        _listViewFinished = new ListView
        {
            X = Pos.Percent(50),
            Y = 2,
            Width = Dim.Fill(),
            Height = Dim.Fill(1),
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

        void HandleKeyDown(ListView listView, Key e)
        {
            if (e == Key.Delete)
            {
                if (listView.Source is null || !(listView.SelectedItem >= 0)) return;
                var items = (IList<TodoItem>)listView.Source.ToList();
                var item = items[listView.SelectedItem ?? 0];
                _todoItems.Remove(item);
                UpdateListView();
                SetTitle();
                e.Handled = true;
            }
            if (e == Key.Tab)
            {
                if (listView == _listViewUnfinished)
                {
                    _listViewFinished.SetFocus();
                }
                else
                {
                    _listViewUnfinished.SetFocus();
                }
                e.Handled = true;
            }
        }

        void HandleAccepting(ListView listView, CommandEventArgs e)
        {
            if (listView.Source is null || !(listView.SelectedItem >= 0)) return;
            var items = (IList<TodoItem>)listView.Source.ToList();
            var item = items[listView.SelectedItem ?? 0];
            item.Marked = !item.Marked;

            UpdateListView();
            e.Handled = true;

            SetTitle();
            listView.HasFocus = true;
        }

        _listViewUnfinished.KeyDown += (s, e) => HandleKeyDown(_listViewUnfinished, e);
        _listViewFinished.KeyDown += (s, e) => HandleKeyDown(_listViewFinished, e);

        _listViewUnfinished.Accepting += (s, e) => HandleAccepting(_listViewUnfinished, e);
        _listViewFinished.Accepting += (s, e) => HandleAccepting(_listViewFinished, e);
        
        var saveButton = new Button
        {
            Text = "Save",
            X = Pos.AnchorEnd(12),
            Y = Pos.AnchorEnd(1)
        };
        saveButton.Accepting += (s, e) => SaveList();
        
        _window.KeyDown += (s, e) =>
        {
            if (e.KeyCode != Key.S.WithCtrl) return;
            
            SaveList();
            e.Handled = true;
        };

        _window.Add(_listViewUnfinished);
        _window.Add(_listViewFinished);
        _window.Add(_newItemLabel);
        _window.Add(_newItemTextField);
        _window.Add(saveButton);
        _window.Add(new Line()
        {
            X = Pos.Percent(50)-1,
            Y = 2,
            Height = Dim.Fill(1),
            Orientation = Orientation.Vertical
        });
        
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
        var finishedItems = new ObservableCollection<TodoItem>(_todoItems.Where(i => i.Marked));
        var unfinishedItems = new ObservableCollection<TodoItem>(_todoItems.Where(i => !i.Marked));
        
        _listViewUnfinished.SetSource<TodoItem>(unfinishedItems);
        _listViewFinished.SetSource<TodoItem>(finishedItems);
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