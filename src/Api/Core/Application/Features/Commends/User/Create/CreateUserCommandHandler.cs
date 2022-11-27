using Api.Core.Domain.Models;
using Application.Interfaces.Repositories;
using AutoMapper;
using Common.Events.User;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models;
using Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commends.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);
            if (existUser is not null)
            {
                throw new DatabaseValidationException("User already exists!");
            }
            var dbuser = _mapper.Map<Api.Core.Domain.Models.User>(request);

            var rows = await _userRepository.AddAsync(dbuser);


            // Email Changed/Created
            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbuser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: DictionaryConstants.UserExchangeName,
                                                   exchangeType: DictionaryConstants.DefaultExchangeType,
                                                   queueName: DictionaryConstants.UserEmailChangedQueueName,
                                                   obj: @event);
            }

            return dbuser.Id;
        }
    }
}
