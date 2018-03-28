using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IMessagesAppService
    {
        List<MessageModel> GetMessages();
        MessageModel GetMessage(int MessageId);
        void UpsertMessage(MessageModel input);
        void RemoveMessage(int MessageId);
    }
}
