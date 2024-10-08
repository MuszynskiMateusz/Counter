namespace Counter;
public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClickedAdd(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterValue.Text = $"Clicked {count} time";
        else
            CounterValue.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterValue.Text);
    }

    private void OnCounterClickedDelete(object sender, EventArgs e)
    {
        count--;

        if (count == 1)
            CounterValue.Text = $"Clicked {count} time";
        else
            CounterValue.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterValue.Text);
    }
}