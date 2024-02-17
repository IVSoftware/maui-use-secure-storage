using CoreText;

namespace maui_use_secure_storage
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        bool _isLoaded = false;
        public MainPage()
        {
            InitializeComponent();
            Appearing += async(sender, e) =>
            {
                if(!_isLoaded)
                {
                    _isLoaded = true;
                    await Task.Delay(1); // Avoid startup timeout if we want to set a breakpoint below.
                    if(
                        await SecureStorage.GetAsync(key: nameof(count)) is string value 
                        && 
                        int.TryParse(value, out var countPrev)
                        && countPrev > 0)
                    {
                        if(await DisplayAlert("Secure Storage", $"Use prev count {countPrev}?", cancel:"No", accept:"Yes"))
                        {
                            count = countPrev; 
                            CounterBtn.Text = $@"Clicked {count} time{(countPrev == 0 ? string.Empty : 's')}";
                        }
                        else
                        {
                            await SecureStorage.SetAsync(key: nameof(count), "0");
                        }
                    }
                }
            };
        }
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            if (count == 1) CounterBtn.Text = $"Clicked {count} time";
            else CounterBtn.Text = $"Clicked {count} times";
            SemanticScreenReader.Announce(CounterBtn.Text);

            await SecureStorage.SetAsync(key: nameof(count), $"{count}");
        }
    }
}
