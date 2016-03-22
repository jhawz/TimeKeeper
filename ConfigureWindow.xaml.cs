using System.Windows;
using System.Windows.Controls;

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
            SharepointListTextBox.Text = Configuration.SharepointList;
            RealNameTextBox.Text = Configuration.RealName;
        }

        private void Sharepoint_Url_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration.SharepointUrl = SharepointUrlTextBox.Text;
        }

        private void Sharepoint_List_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration.SharepointList = SharepointListTextBox.Text;
        }

        private void Real_Name_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration.RealName = RealNameTextBox.Text;
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
