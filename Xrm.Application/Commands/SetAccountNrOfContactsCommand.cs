using System;
using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Commands
{
    public class SetAccountNrOfContactsCommand : ICommand
    {
        public Contact FromContact { get; set; }
    }
}
