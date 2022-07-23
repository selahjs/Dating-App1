using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages // we are going to order by most resent MessageSent(w/c is DateTime)
                .OrderByDescending(m => m.MessageSent)
                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where( u => u.RecipientUsername == messageParams.Username
                    && u.RecipientDeleted == false),
                "Outbox" => query.Where( u => u.Sender.UserName == messageParams.Username
                    && u.SenderDeleted == false),
                _ => query.Where( u => u.RecipientUsername == messageParams.Username 
                    && u.RecipientDeleted == false && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            //we are getting the message conversation between two users
            var messages = await _context.Messages
                .Include(u=>u.Sender).ThenInclude(p=>p.Photos)
                .Include(u=>u.Recipient).ThenInclude(p=>p.Photos)
                .Where(m => m.RecipientUsername == currentUsername && m.RecipientDeleted == false
                    && m.SenderUsername == recipientUsername
                    || m.RecipientUsername == recipientUsername
                    && m.SenderUsername == currentUsername && m.SenderDeleted == false
                )
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
                
            // we are checking if ther are unRead Messages in order to mark them as read because by the time the message thread is opened 
            // ..the user will see the messages
            var unReadMessages = messages.Where(m => m.DateRead == null
                && m.RecipientUsername == currentUsername).ToList();
            
            //we are seting unread messages to read
            if(unReadMessages.Any())
            {
                foreach (var message in unReadMessages)
                {
                    message.DateRead = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0; // returns true if saved successfully
        }
    }
}