using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class TestTransactionalEvent1 : IEvent
    {
        public Guid ContactFromCommandId { get; set; }
    }
}
