using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class LastNameUpdated : IEvent
    {
        public Guid ContactId { get; set; }
        public string NewLastnameName { get; set; }
    }
}
