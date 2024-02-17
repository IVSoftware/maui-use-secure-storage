# Maui Use Secure Storage

Your question says it's about **App Entitlements.plist in a MAUI project for iOS** but then the body of your post states the actual objective is that you would

>... like to use the SecureStorage. I read that for that, I have to create an Entitlements.plist in the iOS folder. 

It does not appear to be the case that `SecureStorage` requires that entitlement. I wanted to mention this because it may obviate the need for the rest of the question. For example, this code seems to work fine without the presence of a **Entitlements.plist** file.

```csharp
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
```

[![prompt to use count][1]][1]


  [1]: https://i.stack.imgur.com/IYLpH.png