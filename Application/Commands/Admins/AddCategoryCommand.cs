﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Commands
{
  public  class AddCategoryCommand:IRequest<int>
    {
        public string Name { get; set; }
    }
}