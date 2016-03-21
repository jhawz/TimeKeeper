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
            UserNameTextBox.Text = Configuration.UserName;
            PasswordBox.Password = Configuration.Password;
            DomainTextBox.Text = Configuration.Domain;
            if (Configuration.CredentialsSaved == "true")
            {
                SaveCredentialsCheckBox.IsChecked = true;
            }
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
                    if (Configuration.Configured == "true")
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        ConfigureWindow configureWindow = new ConfigureWindow();
                        configureWindow.Show();
                        Close();
                    }
                }
            }
        }

        private void SaveCredentialsCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            Configuration.CredentialsSaved = "true";
            ConfigurationManager.SaveCredentials();
        }

        private void SaveCredentialsCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Configuration.UserName = "";
            Configuration.Password = "";
            Configuration.Domain = "";
            Configuration.CredentialsSaved = "false";
            ConfigurationManager.SaveCredentials();
            UserNameTextBox.Text = "";
            PasswordBox.Password = "";
            DomainTextBox.Text = "";
        }
    }
}
