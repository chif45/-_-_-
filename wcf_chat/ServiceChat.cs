using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using Newtonsoft.Json;
using static wcf_chat.Exceptions;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace wcf_chat
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]

    public class ServiceChat : IServiceChat
    {

        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public async Task<int> ConnectAsync(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            await SendMsgAsync(": " + user.Name + " подключился к чату!", 0, -1);
            users.Add(user);
            await UpdateOnlineUserAsync(name, true, user.ID);
            await ReturnOnlineUsers(user.ID);
            return user.ID;
        }

        public async void DisconnectAsync(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user!=null)
            {
                users.Remove(user);

                await SendMsgAsync(": "+user.Name + " покинул чат!",0, -1);
                await UpdateOnlineUserAsync(user.Name, false, id);
            }

        }

        public List<UserDataJS>LoadUsersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<UserDataJS>();
            string json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<UserDataJS>>(json);
            return users;
        }

        public UserDataJS Authorization(string login, string password)
        {
            UserDataJS foundUser = null;

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
                throw new NotFound("Пользователь не найден");
            }

            if (foundUser.Password != password)
            {
                throw new WrongPassword("Не верный пароль");
            }
            return foundUser;
        }

        public bool Registration(UserDataJS data, string confirmPassword)
        {
            var users = LoadUsersFromFile("Users.json");


            if (data.Password != confirmPassword)
            {
                throw new WrongPassword("Пароли не совпадают. Пожалуйста, попробуйте снова.");
                
            }

            foreach (var user in users)
            {
                if (user.Login == data.Login)
                {
                    throw new AlreadyExsist("Пользователь с таким логином уже существует.");

                }
            }

            users.Add(data);
            string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText("Users.json", json);
            return true;
        }

        public async Task SendMsgAsync(string msg, int id_me, int id_for)
        {
            List<Task> tasks = new List<Task>();
            if (id_for == -1)
            {
                foreach (var item in users)
                {
                    string answer = DateTime.Now.ToShortTimeString();
                    var user = users.FirstOrDefault(i => i.ID == id_me);
                    if (user != null)
                    {
                        answer += " " + user.Name + ": ";
                    }
                    answer += msg;
                    tasks.Add(Task.Run(() =>
                        item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer)));
                }
            }
            else
            {
                var user_for = users.FirstOrDefault(j => j.ID == id_for);
                var user_me = users.FirstOrDefault(i => i.ID == id_me);
                string answer = DateTime.Now.ToShortTimeString() + " " + user_me.Name + ": " + msg;
                tasks.Add(Task.Run(() =>
                    user_for.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer)));
                tasks.Add(Task.Run(() =>
                    user_me.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer)));
            }
            await Task.WhenAll(tasks); 
        }

        public async Task UpdateOnlineUserAsync(string NewUser, bool status, int currentUserId)
        {
            List<Task> tasks = new List<Task>();
            foreach (var user in users)
            {
                if (user.ID != currentUserId)
                {
                    tasks.Add(Task.Run(() =>
                    user.operationContext.GetCallbackChannel<IServerChatCallback>().UpdateOnlineUserCallback(NewUser, status, currentUserId)));
                }
            }
            await Task.WhenAll(tasks); // Ждем выполнения всех задач
        }

        public async Task ReturnOnlineUsers(int ID)
        {
            List<Task> tasks = new List<Task>();
            Dictionary<int, string> OnlineUsers = new Dictionary<int, string>();
            ServerUser currentUser = new ServerUser();
            foreach (var user in users)
            {
                if (user.ID != ID)
                {
                    OnlineUsers.Add(user.ID,user.Name);
                }
                else
                {
                    currentUser = user;
                }
            }

            tasks.Add(Task.Run(() => currentUser.operationContext.GetCallbackChannel<IServerChatCallback>().ReturnOnlineUsersCallback(OnlineUsers)));
            await Task.WhenAll(tasks);
        }

        private static readonly string filePath = "C:\\UploadedFiles\\";

        public void UploadFile(byte[] fileData, string fileName)
        {

            string path = Path.Combine(filePath);


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                File.WriteAllBytes(Path.Combine(path, fileName), fileData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }

        }
        public byte[] DownloadFile(string fileName)
        {
            return File.ReadAllBytes(Path.Combine(filePath + "\\", fileName));
        }

    }

}
