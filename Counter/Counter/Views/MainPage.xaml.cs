using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Controls;
using Counter.Models;

namespace Counter.Views;

public partial class MainPage : ContentPage
{
    private const string FileName = "counters.txt";
    public ObservableCollection<CounterItem> counters { get; set; } = new ObservableCollection<CounterItem>();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadCounters();
    }

    private void OnCounterAddClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CounterItem counter)
        {
            counter.Count++;
            SaveCounters();
        }
    }

    private void OnCounterDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CounterItem counter && counter.Count > 0)
        {
            counter.Count--;
            SaveCounters();
        }
    }

    private void SaveCounters()
    {

        var filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var counter in counters)
            {
                writer.WriteLine($"{counter.Name},{counter.Count}");
            }
        }
    }

    private void LoadCounters()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
        if (File.Exists(filePath))
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                    {
                        counters.Add(new CounterItem { Name = parts[0], Count = count });
                    }
                }
            }
        }
    }

    private async void OnNavigateToAddCounterPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCounterPage(counters));
    }
}