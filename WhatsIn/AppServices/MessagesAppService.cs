using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class MessagesAppService : IMessagesAppService
    {
        IWhatsInDBContext _context;
        public MessagesAppService(IWhatsInDBContext context)
        {
            _context = context;
        }

        public List<MessageModel> GetMessages()
        {
            try
            {
                var query = from q in _context.Messages
                            where q.IsDeleted == false || q.IsDeleted == null
                            select q;

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MessageModel GetMessage(int MessageId)
        {
            try
            {
                if (MessageId == 0)
                    throw new NullReferenceException();

                var query = from q in _context.Messages
                            where q.Id == MessageId
                            select q;

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertMessage(MessageModel input)
        {
            try
            {
                if (input.Id == 0)
                    _context.Messages.Add(input);
                else
                    _context.Entry(input).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveMessage(int MessageId)
        {
            try
            {
                if (MessageId == 0)
                    throw new NullReferenceException();
                var Message = GetMessage(MessageId);
                Message.IsDeleted = true;
                _context.Entry(Message).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}