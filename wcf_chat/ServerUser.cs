using System.Collections.Generic;
using System.ServiceModel;
namespace wcf_chat
{
    public class ServerUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public OperationContext operationContext { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserDataJS
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

}
