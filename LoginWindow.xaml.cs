using System;
using System.Windows;
using System.Windows.Controls;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly LdapAuthentication _ldapAuthentication;
        public LoginWindow()
        {
            InitializeComponent();
            string adPath = DomainManager.RootPath;
            _ldapAuthentication = new LdapAuthentication(adPath);
            ConfigurationManager.LoadConfiguration();
        }

        private void TextBox_UserName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration.UserName = UserNameTextBox.Text;
        }

        private void TextBox_Domain_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration.Domain = DomainTextBox.Text;
        }

        private void PasswordBox_PasswordChanged_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Configuration.Password = PasswordBox.Password;
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration.Authenticated = _ldapAuthentication.IsAuthenticated(Configuration.Domain, Configuration.UserName, Configuration.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Configuration.Authenticated.Equals(true))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
            }
        }
    }
}
