using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using Newtonsoft.Json;
using static wcf_chat.Exceptions;
using System.Xml.Linq;

namespace wcf_chat
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]

    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            
            ServerUser user = new ServerUser() {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMsg(": "+user.Name+" подключился к чату!",0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user!=null)
            {
                users.Remove(user);
                SendMsg(": "+user.Name + " покинул чат!",0);
            }

        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name+" ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
        }

        public List<UserDataJS>LoadUsersFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<UserDataJS>(); // Если файл не существует, вернуть пустой список.
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

            // Запись в файл
            File.WriteAllText("Users.json", json);
            return true;
        }
    }
}
