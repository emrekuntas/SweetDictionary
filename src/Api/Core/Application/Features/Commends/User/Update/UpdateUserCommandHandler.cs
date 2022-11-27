using Application.Interfaces.Repositories;
using AutoMapper;
using Common.Events.User;
using Common.Infrastructure.Exceptions;
using Common.Infrastructure;
using Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Application.Features.Commends.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser is null)
                throw new DatabaseValidationException("User not found!");

            string dbEmailAddress = dbUser.EmailAddress;
            bool emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            dbUser = mapper.Map(request, dbUser);

            int rows = await userRepository.UpdateAsync(dbUser);

            if (emailChanged && rows > 0)
            {
                UserEmailChangedEvent @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.UserExchangeName,
                                                   exchangeType: DictionaryConstants.DefaultExchangeType,
                                                   queueName: DictionaryConstants.UserEmailChangedQueueName,
                                                   obj: @event);

                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
