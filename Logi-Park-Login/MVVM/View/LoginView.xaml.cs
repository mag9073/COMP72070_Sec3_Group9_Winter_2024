using LogiPark.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogiPark.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private ProgramClient client;
        //private string address = "127.0.0.1";
        //private int portnumber = 13000;
        private int attempts = 0;
        const int maxAttempts = 3;

        public LoginView()
        {
            this.client = new ProgramClient();
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (attempts >= maxAttempts)
            {
                messageTextBlock.Text = "Maximum login attempts exceeded.";
                messageTextBlock.Foreground = Brushes.Red;
                return;
            }

            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            bool isValid = ValidateCredentials(username, password);
            if (isValid)
            {
                attempts++;
                SendCredentials(username, password); 
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(username) || username == "Username or email")
            {
                isValid = false;
                sb.AppendLine("Please enter your username or email");
            }

            if (string.IsNullOrWhiteSpace(password) || password == "Password")
            {
                isValid = false;
                sb.AppendLine("Please enter your password");
            }

            if (!isValid)
            {
                MessageBox.Show(sb.ToString(), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isValid;
        }

        private void SendCredentials(string username, string password)
        {
            UserDataManager.LoginData loginData = new UserDataManager.LoginData
            {
                username = username,
                password = password
            };

            client.SendLoginRequest(loginData);
            string response = client.ReceiveServerResponse();

            if (response.Contains("Username and password are Correct!!! \\o/"))
            {
                Dispatcher.Invoke(() =>
                {
                    messageTextBlock.Text = "Login Successful";
                    messageTextBlock.Foreground = Brushes.Green;

                    //AdminHomeView adminHomeView = new AdminHomeView();
                    //adminHomeView.Show();
                    //this.Close();

                    ClientHomeView clientHomeView = new ClientHomeView();
                    clientHomeView.Show();
                    this.Close();
                });
            }
            else
            {
                // deals with run time thread safe
                Dispatcher.Invoke(() =>
                {
                    if (attempts < maxAttempts)
                    {
                        messageTextBlock.Text = $"Login Failed. Attempt {attempts} of {maxAttempts}. Please try again.";
                        messageTextBlock.Foreground = Brushes.Red;
                    }
                    else
                    {
                        messageTextBlock.Text = "Login Failed. Maximum attempts reached.";
                        messageTextBlock.Foreground = Brushes.Red;
                        client.CloseConnection();
                    }
                });
            }
        }

        private void Signup_Handler(object sender, MouseButtonEventArgs e)
        {

            // Handle signup label clicked
            SignupView signupView = new SignupView();

            signupView.Show();

            this.Close();
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminHomeView HomeView = new AdminHomeView();

            HomeView.Show();

            this.Close();
        }
    }
}
