using System.Windows;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for ConfigureWindow.xaml
    /// </summary>
    public partial class ConfigureWindow
    {
        public ConfigureWindow()
        {
            InitializeComponent();

            SharepointUrlTextBox.Text = Configuration.SharepointUrl;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Configured = "true";
            ConfigurationManager.SaveConfiguration();
            if (Configuration.SharepointUrl != "")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("No Configuration Information");
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Configuration.SharepointUrl = SharepointUrlTextBox.Text;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Configuration.Configured == "true")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        }
    }
}
