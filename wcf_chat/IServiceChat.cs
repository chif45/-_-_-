using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace wcf_chat
{

    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IServiceChat
    {
        [OperationContract]
        Task<int> ConnectAsync(string name);

        [OperationContract]
        void DisconnectAsync(int id);

        [OperationContract(IsOneWay = true)]
        Task SendMsgAsync(string msg, int id_me, int id_for);

        List<UserDataJS> LoadUsersFromFile(string filePath);

        [OperationContract]
        UserDataJS Authorization(string login, string password);

        [OperationContract]
        bool Registration(UserDataJS data, string confirmPassword);

        [OperationContract(IsOneWay = true)]
        Task UpdateOnlineUserAsync(string NewUser, bool status, int currentUserId);

        [OperationContract]
        Task ReturnOnlineUsers(int ID);

        [OperationContract]
        void UploadFile(byte[] fileData, string fileName);

        [OperationContract]
        byte[] DownloadFile(string fileName);
    }

    public interface IServerChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);

        [OperationContract(IsOneWay = true)]
        void UpdateOnlineUserCallback(string UserName, bool status, int ID);

        [OperationContract(IsOneWay = true)]
        void ReturnOnlineUsersCallback(Dictionary<int,string> OnlineUsers);

    }





}
