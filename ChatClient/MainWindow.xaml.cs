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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int ID;
        public MainWindow(string name)
        {
            InitializeComponent();
            tbUserName.Text = name;
            client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
            ID = client.Connect(tbUserName.Text);
            isConnected = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
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
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);
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
                    client.SendMsg(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                }               
            }
        }
    }
}
