using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class SetNrOfContactsCommand : ICommand
    {
        public Guid AccountId { get; set; }
    }
}
