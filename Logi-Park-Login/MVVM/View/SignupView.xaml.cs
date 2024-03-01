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

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignupView.xaml
    /// </summary>
    public partial class SignupView : Window
    {
        public SignupView()
        {
            InitializeComponent();
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Username or email")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Username or email";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Password")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Password";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            StringBuilder sb = new StringBuilder();

            // Verify Username
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) || usernameTextBox.Text == "Username or email")
            {
                isValid = false;
                sb.AppendLine("Please enter your username or email");
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Text) || passwordTextBox.Text == "Password")
            {
                isValid = false;
                sb.AppendLine("Please enter your password");
            }

            if (!isValid)
            {
                MessageBox.Show(sb.ToString(), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // If valid then open a home view
                //HomeView homeView = new HomeView();

                //homeView.Show();

                //this.Close();
            }

        }

        private void Login_Handler(object sender, MouseButtonEventArgs e)
        {

            // Handle login label clicked

            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void usernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void passwordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
