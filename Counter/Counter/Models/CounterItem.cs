using Microsoft.Maui.Controls;

namespace Counter.Models
{
    public class CounterItem : BindableObject
    {
        private int _count;
        public string Name { get; set; }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
    }
}