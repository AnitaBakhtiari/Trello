﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Models;
using MediatR;

namespace Application.Queries
{
    public class GetArchiveListQuery:IRequest<IEnumerable<UserTask>>
    {

    }
}
