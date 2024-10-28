using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;

namespace Counter;

public partial class MainPage : ContentPage
{
    private const string FileName = "counters.txt";
    private List<CounterItem> counters = new List<CounterItem>();

    public MainPage()
    {
        InitializeComponent();
        LoadCounters();
        DisplayCounters();
    }

    private void OnAddCounterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CounterNameEntry.Text) ||
            !int.TryParse(CounterValueEntry.Text, out int initialValue))
        {
            DisplayAlert("Error", "Please enter a valid name and initial value.", "OK");
            return;
        }

        var newCounter = new CounterItem
        {
            Name = CounterNameEntry.Text,
            Count = initialValue
        };

        counters.Add(newCounter);
        SaveCounters();
        DisplayCounters();

        CounterNameEntry.Text = string.Empty;
        CounterValueEntry.Text = string.Empty;
    }

    private void DisplayCounters()
    {
        CountersStack.Children.Clear();
        foreach (var counter in counters)
        {
            var counterLayout = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
            },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 5)
            };

            var counterLabel = new Label
            {
                Text = $"{counter.Name}\n {counter.Count}",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var addButton = new Button
            {
                Text = "+",
                CommandParameter = counter,
                HorizontalOptions = LayoutOptions.End
            };
            addButton.Clicked += OnCounterAddClicked;

            var deleteButton = new Button
            {
                Text = "-",
                CommandParameter = counter,
                HorizontalOptions = LayoutOptions.Start
            };
            deleteButton.Clicked += OnCounterDeleteClicked;

            counterLayout.Children.Add(deleteButton);
            Grid.SetColumn(deleteButton, 0);
            counterLayout.Children.Add(counterLabel);
            Grid.SetColumn(counterLabel, 1);
            counterLayout.Children.Add(addButton);
            Grid.SetColumn(addButton, 2);

            CountersStack.Children.Add(counterLayout);
        }
    }

    private void OnCounterAddClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CounterItem counter)
        {
            counter.Count++;
            SaveCounters();
            DisplayCounters();
        }
    }

    private void OnCounterDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CounterItem counter)
        {
            counter.Count--;
            SaveCounters();
            DisplayCounters();
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
                    if (parts.Length == 2 &&
                        int.TryParse(parts[1], out int count))
                    {
                        counters.Add(new CounterItem
                        {
                            Name = parts[0],
                            Count = count
                        });
                    }
                }
            }
        }
    }
}

public class CounterItem
{
    public string Name { get; set; }
    public int Count { get; set; }
}