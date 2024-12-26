using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ChatClient.ServiceChat;
using wcf_chat;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.ServiceModel;
using System.Linq;

namespace ChatClient
{
    public class ListenItem
    {
        public string DisplayText { get; set; }
        public int HiddenValue { get; set; }
        public ListenItem(string displayText, int hiddenValue)
        {
            DisplayText = displayText;
            HiddenValue = hiddenValue;
        }
        public override string ToString()
        {
            return DisplayText;
        }
    }
        public partial class MainWindow : Window, IServiceChatCallback
    {
        private ServiceChatClient client;
        private int ID;
        private bool isConnected;
        private readonly string Login;
        private int ID_dialog = -1;

        public MainWindow(string name, string login)
        {
            InitializeComponent();
            tbUserName.Text = name;
            Login = login;
            client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            try
            {
                ID = await client.ConnectAsync(tbUserName.Text);
                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
                isConnected = false;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.DisconnectAsync(ID);
                client = null;
                isConnected = false;
                RegAuthWindow RegAuthWindow = new RegAuthWindow();
                RegAuthWindow.Show();
            }

        }
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            DisconnectUser();
            this.Close();
        }
        public void MsgCallback(string msg)
        {
             lbChat.Items.Add(msg);
             lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }
        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client!=null)
                {
                    client.SendMsgAsync(tbMessage.Text, ID, ID_dialog);
                    tbMessage.Text = string.Empty;
                }               
            }
        }
        public void UpdateOnlineUserCallback(string UserName, bool status, int ID)
        {
            if (status)
            {
                if (!lbOnlineUsers.Items.Cast<ListenItem>().Any(item =>
                    item.DisplayText == UserName && item.HiddenValue == ID))
                {
                    lbOnlineUsers.Items.Add(new ListenItem(UserName, ID));
                }
            }
            else 
            {
                var itemToRemove = lbOnlineUsers.Items
                    .Cast<ListenItem>()
                    .FirstOrDefault(item => item.DisplayText == UserName && item.HiddenValue == ID);
                if (itemToRemove != null)
                {
                    lbOnlineUsers.Items.Remove(itemToRemove);
                }
            }
        }
        public void ReturnOnlineUsersCallback(Dictionary<int,string> OnlineUsers)
        {
            foreach (var user in OnlineUsers)
            {
                lbOnlineUsers.Items.Add(new ListenItem(user.Value, user.Key));
            }
        }
        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files|*.*", 
                Title = "Выберите файл для загрузки"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                UploadFile(filePath);
            }
        }
        public void UploadFile(string filePath)
        {
            try
            {
                var fileBytes = File.ReadAllBytes(filePath);
                var fileName = Path.GetFileName(filePath);
                client.UploadFile(fileBytes, fileName);
                client.SendMsg(fileName, ID, ID_dialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
        }
        private void DownloadFile(string fileName)
        {
            try
            {
                byte[] fileData = client.DownloadFile(fileName); 
                string savePath = Path.Combine("C:\\DownloadedFiles", fileName);

                string directoryPath = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Сохранение файла
                File.WriteAllBytes(savePath, fileData);
                MessageBox.Show("Файл успешно загружен: " + savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при скачивании файла: {ex.Message}");
            }
        }
        private void bOpenGlobalChat_Click(object sender, RoutedEventArgs e)
        {
            WhatIsTheChat.Text = "Общий чат";
            lbChat.Items.Clear();
            ID_dialog = -1;
            lbOnlineUsers.SelectedIndex = -1;
        }
        private void lbOnlineUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lbOnlineUsers.SelectedItem is ListenItem selecteditem && lbOnlineUsers.SelectedItem != null)
            {
                WhatIsTheChat.Text = selecteditem.DisplayText;
            }
            if (lbOnlineUsers.SelectedItem is ListenItem selectedItem)
            {
                ID_dialog = selectedItem.HiddenValue;
                lbChat.Items.Clear();
            }
        }
        private void lbChat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbChat.SelectedItem != null)
            {
                string selectedItem = lbChat.SelectedItem.ToString();
                int lastSpaceIndex = selectedItem.LastIndexOf(' ');
                string selectedFileName;

                if (lastSpaceIndex != -1)
                {
                    selectedFileName = selectedItem.Substring(lastSpaceIndex + 1);
                }
                else
                {
                    selectedFileName = selectedItem;
                }

                DownloadFile(selectedFileName);
            }
        }
        private void SendMsg_Click(object sender, RoutedEventArgs e)
        {
            client.SendMsgAsync(tbMessage.Text, ID, ID_dialog);
            tbMessage.Text = string.Empty;
        }
    }

}
