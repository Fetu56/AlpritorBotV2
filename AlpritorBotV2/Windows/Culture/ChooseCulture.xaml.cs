using AlpritorBotV2.Logic.ClassesModule;
using AlpritorBotV2.LogModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlpritorBotV2.Windows.Culture
{
    /// <summary>
    /// Interaction logic for ChooseCulture.xaml
    /// </summary>
    public partial class ChooseCulture : Window
    {
        public List<CultureProps> CultureList { get; set; } = CultureProps.GetSupportedCultures();
        public CultureProps? SelectedCulture;
        public string? ChannelName;
        public ChooseCulture()
        {
            InitializeComponent();

            //Binding binding = new Binding
            //{
            //    Source = CulturesContainer.Cultures.ConvertAll(x => x.ToUpper()),
            //    Path = new PropertyPath("CultureList")
            //};
            //CultureBox.SetBinding(ContentControl.ContentProperty, binding);

            //new CountryFlag.CountryFlag()
            //CultureBox.ItemsSource = CulturesContainer.Cultures.ConvertAll(x => x.ToUpper());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if(CultureBox.SelectedItem == null)
            //{
            //    HightLightRed(CultureBox);
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(ChannelTextBox.Text))
            //{
            //    HightLightRed(ChannelTextBox);
            //    return;
            //}
            SelectedCulture = (CultureProps)CultureBox.SelectedItem;
            ChannelName = ChannelTextBox.Text;
            Close();
        }

        //private void HightLightRed(Control control)
        //{
        //    control.BorderBrush = Brushes.Red;
        //}

        private void Start_Settings_Closed(object sender, EventArgs e)
        {
            SimpleLogger.WriteToLog($"Closed with: culture - \"{SelectedCulture}\", channel - \"{ChannelName}\"", "ChooseCulture Window");
        }

        public new Pair<CultureProps?, string?>? ShowDialog()
        {
            base.ShowDialog();
            return new Pair<CultureProps?, string?>(SelectedCulture, ChannelName);
        }
    }
}
