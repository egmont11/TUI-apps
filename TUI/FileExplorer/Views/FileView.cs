using System.Collections.ObjectModel;
using FileExplorer.Models;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FileExplorer;

public class FileView
{
    private readonly Window _window;
    private string _currentPathLeft;
    private string _currentPathRight;
    
    public string CurrentPathLeft => _currentPathLeft;
    public string CurrentPathRight => _currentPathRight;
    
    private List<ExplorerItem> _directoryContentLeft;
    private List<ExplorerItem> _directoryContentRight;
    
    private View _leftPanel;
    private View _middlePanel;
    private View _rightPanel;

    private ListView _listViewLeft;
    private ListView _listViewRight;

    public FileView(Window window)
    {
        _window = window;
        _currentPathLeft = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _currentPathRight = _currentPathLeft;
        
        _directoryContentLeft = GetDirectoryContent(CurrentPathLeft);
        _directoryContentRight = GetDirectoryContent(CurrentPathRight);
    }

    public void Show()
    {
        _leftPanel = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(49),
            Height = Dim.Fill()
        };

        _middlePanel = new View()
        {
            X = Pos.Right(_leftPanel),
            Y = 0,
            Width = 2,
            Height = Dim.Fill()
        };

        _rightPanel = new View()
        {
            X = Pos.Right(_middlePanel),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        _window.Add(_leftPanel, _middlePanel, _rightPanel);

        ShowLeft();
        ShowMiddle();
        ShowRight();
    }

    private void ShowMiddle()
    {
        _middlePanel.Add(
            new Line()
            {
                X = 0, 
                Y = 0, 
                Height = Dim.Fill(),
                Orientation = Orientation.Vertical
            },
            new Line()
            {
                X = 1, 
                Y = 0, 
                Height = Dim.Fill(),
                Orientation = Orientation.Vertical
            }
        );
    }
    
    private void ShowLeft()
    {
        _leftPanel.RemoveAll();

        var pathLabel = new Label()
        {
            Text = $"Path: {_currentPathLeft}",
            X = 0,
            Y = 0,
            Width = Dim.Fill()
        };

        var line = new Line()
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill()
        };

        _leftPanel.Add(pathLabel, line);

        _listViewLeft = new ListView()
        {
            X = 0,
            Y = 2,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        _listViewLeft.Source = new ListWrapper<ExplorerItem>(
            new ObservableCollection<ExplorerItem>(_directoryContentLeft)
        );

        // triggered by enter
        _listViewLeft.Accepting += (s, e) => ItemPressed(true);
        
        _leftPanel.Add(_listViewLeft);
    }

    
    private void ShowRight()
    {
        _rightPanel.RemoveAll();

        var pathLabel = new Label()
        {
            Text = $"Path: {_currentPathRight}",
            X = 0,
            Y = 0,
            Width = Dim.Fill()
        };

        var line = new Line()
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill()
        };

        _rightPanel.Add(pathLabel, line);

        _listViewRight = new ListView()
        {
            X = 0,
            Y = 2,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        _listViewRight.Source = new ListWrapper<ExplorerItem>(
            new ObservableCollection<ExplorerItem>(_directoryContentRight)
        );
        
        _rightPanel.Add(_listViewRight);
    }

    private void Refresh()
    {
        _directoryContentLeft = GetDirectoryContent(CurrentPathLeft);
        _directoryContentRight = GetDirectoryContent(CurrentPathRight);
        
        _listViewLeft.Source = new ListWrapper<ExplorerItem>(
            new ObservableCollection<ExplorerItem>(_directoryContentLeft)
        );
        
        _listViewRight.Source = new ListWrapper<ExplorerItem>(
            new ObservableCollection<ExplorerItem>(_directoryContentRight)
        );
    }

    private void ItemPressed(bool isLeft)
    {
        var item = isLeft ? _listViewLeft.SelectedItem : _listViewRight.SelectedItem;
        var items = isLeft ? _directoryContentLeft : _directoryContentRight;

        switch (item)
        {
            case null:
            case < 0:
                return;
        }

        var selectedItem = items[(int)item];
        
        if (selectedItem.IsDirectory)
        {
            GoToDirectory(selectedItem.Path, isLeft);
        }
        else
        {
            OpenFile(selectedItem.Path);
        }
    }
    
    private void GoToParentDirectory(bool isLeft)
    {
        DirectoryInfo? parent;
        if (isLeft)
        {
            parent = Directory.GetParent(_currentPathLeft);
            if (parent is null) return;
            _currentPathLeft = parent.FullName;
        }
        else
        {
            parent = Directory.GetParent(_currentPathRight);
            if (parent is null) return;
            _currentPathRight = parent.FullName;
        }
        
        Refresh();
    }
    
    private void GoToDirectory(string path, bool isLeft)
    {
        try
        {
            if (isLeft)
            {
                _currentPathLeft = path;
            }
            else
            {
                _currentPathRight = path;
            }

            Refresh();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void OpenFile(string path)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static List<ExplorerItem> GetDirectoryContent(string path)
    {
        List<ExplorerItem> directoryContent = [];
        var directory = new DirectoryInfo(path);

        directoryContent.Add(new ExplorerItem()
        {
            Name = ".",
            Path = directory.FullName,
            IsDirectory = true,
            Size = null,
        });
        
        directoryContent.Add(new ExplorerItem()
        {
            Name = "..",
            Path = directory.Parent?.FullName ?? "",
            IsDirectory = true,
            Size = null,
        });
        
        directoryContent
            .AddRange(directory.GetFiles()
            .Select(file => new ExplorerItem()
            {
                Name = file.Name,
                Path = Path.Combine(path, file.Name),
                IsDirectory = false,
                Size = file.Length,
                LastModified = file.LastWriteTime,
                Created = file.CreationTime
            }
        ));
        
        directoryContent
            .AddRange(directory.GetDirectories()
            .Select(file => new ExplorerItem()
            {
                Name = file.Name,
                Path = Path.Combine(path, file.Name),
                IsDirectory = true,
                Size = null,
                LastModified = file.LastWriteTime,
                Created = file.CreationTime
            }
        ));

        return directoryContent;
    }
}