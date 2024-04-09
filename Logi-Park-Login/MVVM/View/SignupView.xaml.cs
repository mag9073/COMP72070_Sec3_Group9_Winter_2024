using LogiPark.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        ProgramClient client;

        public SignupView()
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

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            StringBuilder sb = new StringBuilder();

            // Define username and password -> get it from textbox (check xaml code)
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            Console.WriteLine($"username: {username}");
            Console.WriteLine($"password: {password}");

           
         



            // Verify Username -- This can be separated to a helper method
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

                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData
                {
                    username = username,
                    password = password
                };

                client.SendSignUpRequest(signUpData);
                string response = client.ReceiveServerResponse();

                Dispatcher.Invoke(() =>
                {
                    if (response.Contains("Username already exists please try another!!! /o\\"))
                    {
                        messageTextBlock.Text = response;
                        messageTextBlock.Foreground = Brushes.Red;
                    }
                    else
                    {
                        messageTextBlock.Text = response;
                        messageTextBlock.Foreground = Brushes.Green;
                        
                    }
                    
                });

                LoginView loginView = new LoginView();

                Thread.Sleep(2000);
                loginView.Show();

                this.Close();
            }

            // If the username and password are filled, then can proceed to sign up process


            // Initialize it to the SignUp Data Class

            // Send info to the sign up class 

            // Need to implement a method to send info

            // Need to implement a method to receive data back?

            // its it is good then it can proceed, take them back to the login page and current view 
        }

        private void Login_Handler(object sender, MouseButtonEventArgs e)
        {

            // Handle login label clicked

            LoginView loginView = new LoginView();

            loginView.Show();

            this.Close();
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
