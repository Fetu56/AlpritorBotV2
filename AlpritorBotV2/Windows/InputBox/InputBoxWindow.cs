using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace AlpritorBotV2.Windows.InputBox
{
    public class InputBoxWindow//locale? TBH got code from https://stackoverflow.com/a/32780301/18599560 and minor edited, cuz of lazy :)
    {
        Window Box = new Window();
        FontFamily font = new FontFamily("Tahoma");
        int FontSize = 20;
        StackPanel sp1 = new StackPanel();
        string title = "InputBox";
        string boxcontent;
        string errormessage = "Invalid answer";
        string errortitle = "Error";
        string okbuttontext = "OK";
        Brush InputBackgroundColor = Brushes.Ivory;
        bool clicked = false;
        TextBox input = new TextBox();
        Button ok = new Button();

        public InputBoxWindow(string content)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            windowdef();
        }

        public InputBoxWindow(string content, string Htitle, string DefaultText)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            try
            {
                title = Htitle;
            }
            catch
            {
                title = "Error!";
            }
            windowdef();
        }

        public InputBoxWindow(string content, string Htitle, string Font, int Fontsize)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            try
            {
                font = new FontFamily(Font);
            }
            catch { font = new FontFamily("Tahoma"); }
            try
            {
                title = Htitle;
            }
            catch
            {
                title = "Error!";
            }
            if (Fontsize >= 1)
                FontSize = Fontsize;
            windowdef();
        }

        private void windowdef()
        {
            Box.Height = 165;
            Box.Width = 250;
            Box.Title = title;
            Box.Content = sp1;
            TextBlock content = new TextBlock();
            content.TextWrapping = TextWrapping.Wrap;
            content.Background = null;
            content.HorizontalAlignment = HorizontalAlignment.Center;
            content.Text = boxcontent;
            content.FontFamily = font;
            content.FontSize = FontSize;
            sp1.Children.Add(content);

            input.Background = InputBackgroundColor;
            input.FontFamily = font;
            input.FontSize = FontSize;
            input.MinWidth = 200;
            input.VerticalContentAlignment = VerticalAlignment.Center;
            input.Height = 30;
            input.FontSize = 16;
            input.Margin = new Thickness(5);
            sp1.Children.Add(input);
            ok.Margin = new Thickness(5);
            ok.Width = 70;
            ok.Height = 30;
            ok.Click += ok_Click;
            ok.Content = okbuttontext;
            ok.HorizontalAlignment = HorizontalAlignment.Center;
            sp1.Children.Add(ok);

        }

        void Box_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!clicked)
                e.Cancel = true;
        }

        void ok_Click(object sender, RoutedEventArgs e)
        {
            clicked = true;
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                MessageBox.Show(errormessage, errortitle);
            }
            else
            {
                Box.Close();
            }
            clicked = false;
        }

        public string ShowDialog()
        {
            Box.ShowDialog();
            return input.Text;
        }
    }
}
