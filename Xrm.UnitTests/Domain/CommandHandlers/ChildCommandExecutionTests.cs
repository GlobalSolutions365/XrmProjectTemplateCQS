using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Crm;


namespace Xrm.UnitTests.Domain.CommandHandlers
{
    [TestClass]
    public class ChildCommandExecutionTests : BaseCrmTest
    {
        private readonly Guid accountId = Guid.NewGuid();
        private readonly Guid triggerContactId = Guid.NewGuid();

        public ChildCommandExecutionTests()
        {
            Account account = new Account { Id = accountId, Name = "" };
            Contact[] contacts = new[]
            {
                new Contact { Id = Guid.NewGuid(), ParentCustomerId = null },
                new Contact { Id = triggerContactId, ParentCustomerId = new EntityReference(Account.EntityLogicalName, accountId) },
                new Contact { Id = Guid.NewGuid(), ParentCustomerId = new EntityReference(Account.EntityLogicalName, Guid.NewGuid() ) },
                new Contact { Id = Guid.NewGuid(), ParentCustomerId = new EntityReference(Account.EntityLogicalName, accountId) },
                new Contact { Id = Guid.NewGuid(), ParentCustomerId = null },
            };

            Context.Initialize(new Entity[] { account }.Union(contacts));
        }

        [TestMethod]
        public void CanRunCommandFromCommand()
        {
            CommandTriggeringCommand cmd = new CommandTriggeringCommand { FromContact = GetTriggeringContact() };

            CmdBusWithNoEventPropagation.Handle(cmd, FlowArgs);

            Account account = GetTargetAccount();

            Assert.AreEqual($"I have 2 contacts", account.Name);
        }

        [TestMethod]
        public void CanRunCommandFromEvent()
        {
            EventTriggeringCommand @event = new EventTriggeringCommand { FromContact = GetTriggeringContact() };

            EventBus.NotifyListenersAbout(@event, FlowArgs);

            Account account = GetTargetAccount();

            Assert.AreEqual($"I have 2 contacts", account.Name);
        }

        private Contact GetTriggeringContact()
        {
            return OrgService.Retrieve(Contact.EntityLogicalName, triggerContactId, new ColumnSet(true))
                             .ToEntity<Contact>();
        }

        private Account GetTargetAccount()
        {
            return OrgService.Retrieve(Account.EntityLogicalName, accountId, new ColumnSet(true))
                          .ToEntity<Account>();
        }
    }
}
