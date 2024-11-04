using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Counter.Models;

namespace Counter.Views
{
    public partial class AddCounterPage : ContentPage
    {
        public ObservableCollection<CounterItem> Counters { get; set; }

        public AddCounterPage(ObservableCollection<CounterItem> counters)
        {
            InitializeComponent();
            Counters = counters;
            BindingContext = this;
        }

        private void OnAddCounterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CounterNameEntry.Text) || !int.TryParse(CounterValueEntry.Text, out int initialValue))
            {
                DisplayAlert("Error", "Please enter a valid name and initial value.", "OK");
                return;
            }

            var newCounter = new CounterItem
            {
                Name = CounterNameEntry.Text,
                Count = initialValue
            };

            Counters.Add(newCounter);

            CounterNameEntry.Text = string.Empty;
            CounterValueEntry.Text = string.Empty;
        }
    }
}