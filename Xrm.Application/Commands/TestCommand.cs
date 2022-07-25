using System;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Commands
{
    public class TestCommand : ICommand
    {
        public bool IsHandled { get; set; }
        public Guid SomeId { get; set; }
    }
}
