using System;
using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Commands
{
    public class UpdateAccountNameCommand : ICommand
    {
        public Account TargetAccount { get; set; }
        public string Prefix { get; set; }
    }
}
