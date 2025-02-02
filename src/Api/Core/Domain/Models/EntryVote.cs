﻿using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Domain.Models;

public class EntryVote: BaseEntity
{
    public Guid EntryId { get; set; }

    public VoteType VoteType { get; set; }

    public Guid CreatedById { get; set; }


    public virtual Entry Entry { get; set; }
    public virtual User CreatedBy { get; set; }
}
