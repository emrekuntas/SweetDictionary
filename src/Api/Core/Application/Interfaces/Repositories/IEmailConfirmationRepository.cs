﻿using Api.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories;


public interface IEmailConfirmationRepository: IGenericRepository<EmailConfirmation>
{
}
