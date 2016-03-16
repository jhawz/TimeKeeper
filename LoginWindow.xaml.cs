using System;
using System.Collections.Generic;
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
using System.DirectoryServices;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static Credentials credentials;
        private LdapAuthentication ldapAuthentication;

        public LoginWindow()
        {
            InitializeComponent();
            credentials = new Credentials();
            string adPath = DomainManager.RootPath;
            ldapAuthentication = new LdapAuthentication(adPath);
        }

        private void TextBox_UserName(object sender, TextChangedEventArgs e)
        {
            credentials.UserName = UserNameTextBox.Text;
        }

        private void TextBox_Domain(object sender, TextChangedEventArgs e)
        {
            credentials.Domain = DomainTextBox.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            credentials.Password = PasswordBox.Password;
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                credentials.Authenticated = ldapAuthentication.IsAuthenticated(credentials.Domain, credentials.UserName, credentials.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (credentials.Authenticated.Equals(true))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
