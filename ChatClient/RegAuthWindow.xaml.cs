using System.Windows;
using System;
using System.Windows.Controls;
using ChatClient.ServiceChat;
using wcf_chat;
using System.ServiceModel;

namespace ChatClient
{
    public partial class RegAuthWindow : Window,IServiceChatCallback
    {
        // Действия при инициализации
        public RegAuthWindow()
        {
            InitializeComponent();
        }
        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBoxA.Text;
            string password = PasswordBoxA.Password;
            UserDataJS foundUser;
            try
            {
                ServiceChatClient client = new ServiceChatClient(new InstanceContext(this));
                foundUser = client.Authorization(login, password);
                MainWindow mainWindow = new MainWindow(foundUser.Name);
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex) // Обработка всех остальных исключений
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceChatClient client = new ServiceChatClient(new InstanceContext(this));
            string login = LoginTextBoxR.Text;
            string password = PasswordBoxR.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string name = NameTextBoxR.Text;

            var userData = new UserDataJS
            {
                Name = name,
                Login = login,
                Password = password
            };
            try
            {
                bool res = client.Registration(userData, confirmPassword);
                if (res)
                {
                    MessageBox.Show(" Пользователь успешно зарегистрирован!\nПожалуйста, пройдите процедуру входа в форме входа");
                }
            }
            catch (Exception ex) // Обработка всех остальных исключений
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            // Логика для обработки изменений текста
        }
        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)

        {

            // Логика для обработки изменений текста

        }
        public void MsgCallback(string msg)
        {
            MessageBox.Show(msg);
        }

    }



}

