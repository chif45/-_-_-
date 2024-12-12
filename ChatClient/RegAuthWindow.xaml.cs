using System.Windows;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Xml;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для RegAuthWindow.xaml
    /// </summary>
    public partial class RegAuthWindow : Window
    {
        public RegAuthWindow()
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
            string name = string.Empty;
            UserData foundUser = null;

            foreach (var user in LoadUsersFromFile("Users.json"))
            {
                if (user.Login == login)
                {
                    foundUser = user; 
                    break; 
                }
            }
            
            if (foundUser == null)
            {
                MessageBox.Show("Пользователь с таким логином не найден.");
                return; 
            }

            if (foundUser.Password != password)
            {
                MessageBox.Show("Не верный пароль");
                return; // Завершаем выполнение метода
            }

            MainWindow mainWindow = new MainWindow(foundUser.Name);
            mainWindow.Show();
            this.Close();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBoxR.Text;
            string password = PasswordBoxR.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string name = NameTextBoxR.Text;

            var users = LoadUsersFromFile("Users.json");

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте снова.");
                return;
            }

            foreach (var user in users)
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
                Name = name,
                Login = login,
                Password = password
            };
            users.Add(userData);
            string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);

            // Запись в файл
            File.WriteAllText("Users.json", json);
            MessageBox.Show("Пользователь успешно зарегистрирован!\n Пожалуйста, пройдите процедуру входа в форме входа");
        }

        public class UserData
        {
            public string Name { get; set; }
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

