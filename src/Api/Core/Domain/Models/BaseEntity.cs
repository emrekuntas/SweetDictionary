﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Domain.Models;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
}
