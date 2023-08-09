using AlpritorBotV2.ListenerModule;
using AlpritorBotV2.TwitchModule;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace AlpritorBotV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Windows.Culture.ChooseCulture culture = new();
            culture.ShowDialog();

            if(culture.SelectedCulture!=null && !string.IsNullOrEmpty(culture.ChannelName))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture.SelectedCulture.CultureName);
                //MessageBox.Show(Localization.Bot.Resources.CurrentLocale);

                BotBase.Initialize(culture.ChannelName);

                //MessageBox.Show(Localization.Bot.Resources.GiveMeModeMsg);
                //MessageBox.Show(Localization.Bot.Resources.HelloMsg);
            }

            InitializeComponent();
        }

        private async void ButtonGetToken_Click(object sender, RoutedEventArgs e)
        {
            ButtonGetToken.IsEnabled = false;
            try
            {
                BotBase.SetUserAccessToken(await TokenListener.GetAccessToken());
            }
            catch (System.Exception ex)
            {

                ButtonGetToken.IsEnabled = true;
                MessageBox.Show(ex.Message, "ERROR");//locale AND BUTTON TEXT TOO
            }
        }
    }
}
