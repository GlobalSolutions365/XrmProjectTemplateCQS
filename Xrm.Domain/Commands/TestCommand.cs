using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class TestCommand : ICommand
    {
        public bool IsHandled { get; set; }
        public Guid SomeId { get; set; }
    }
}
