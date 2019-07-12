using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class UpdateLastName : ICommand
    {
        public Guid ContactId { get; set; }
        public string Prefix { get; set; }
    }
}
