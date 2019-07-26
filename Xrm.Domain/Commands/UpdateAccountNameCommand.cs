using System;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class UpdateAccountNameCommand : ICommand
    {
        public Account TargetAccount { get; set; }
        public string Prefix { get; set; }
    }
}
