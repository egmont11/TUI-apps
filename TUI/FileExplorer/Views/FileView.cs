using FileExplorer.Models;
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

    public FileView(Window window)
    {
        _window = window;
        _currentPathLeft = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _currentPathRight = _currentPathLeft;
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

        Refresh();

        for (int i = 0; i < _directoryContentLeft.Count; i++)
        {
            var item = _directoryContentLeft[i];
            var name = item.IsDirectory ? $"{item.Name}/" : item.Name;

            _leftPanel.Add(new Label()
            {
                Text = name,
                X = 0,
                Y = i + 2,
                Width = Dim.Fill()
            });
        }
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

        Refresh();

        for (int i = 0; i < _directoryContentRight.Count; i++)
        {
            var item = _directoryContentRight[i];
            var name = item.IsDirectory ? $"{item.Name}/" : item.Name;

            _rightPanel.Add(new Label()
            {
                Text = name,
                X = 0,
                Y = i + 2,
                Width = Dim.Fill()
            });
        }
    }

    private void Refresh()
    {
        _directoryContentLeft = GetDirectoryContent(CurrentPathLeft);
        _directoryContentRight = GetDirectoryContent(CurrentPathRight);
    }
    
    public void GoToParentDirectory(bool isLeft)
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
    
    public void GoToDirectory(string path)
    {
        throw new NotImplementedException();
    }
    
    public void OpenFile(string path)
    {
        throw new NotImplementedException();
    }

    private static List<ExplorerItem> GetDirectoryContent(string path)
    {
        List<ExplorerItem> directoryContent = [];
        var directory = new DirectoryInfo(path);

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