using System;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class SetAccountNrOfContactsCommand : ICommand
    {
        public Contact FromContact { get; set; }
    }
}
