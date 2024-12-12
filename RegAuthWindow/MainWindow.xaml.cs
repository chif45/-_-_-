using System.Windows;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace RegAuthWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private List<UserData> LoadUsersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<UserData>(); // Если файл не существует, вернуть пустой список.

            string json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<UserData>>(json);
            return users;
        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBoxA.Text;
            string password = PasswordBoxA.Password;

            foreach (var user in LoadUsersFromFile("Users.json"))
            {
                if (user.Login != login)
                {
                    MessageBox.Show("Пользователь с таким логином не найден.");
                    return;
                }
                else if (user.Password != password)
                {
                    MessageBox.Show("Не верный пароль");
                    return;
                }
            }


        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBoxR.Text;
            string password = PasswordBoxR.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Сравнение паролей
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте снова.");
                return;
            }

            foreach (var user in LoadUsersFromFile("Users.json"))
            {
                if (user.Login == login)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }
            }

            // Заносим данные в JSON формат
            var userData = new UserData
            {
                Login = login,
                Password = password
            };

            string json = JsonConvert.SerializeObject(userData, Formatting.Indented);

            // Запись в файл
            File.AppendAllText("Users.json", json + Environment.NewLine);
            MessageBox.Show("Пользователь успешно зарегистрирован!\n Пожалуйста, пройдите процедуру входа в форме входа");
        }

        public class UserData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            // Логика для обработки изменений текста
        }
        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)

        {

            // Логика для обработки изменений текста

        }
    }
}

