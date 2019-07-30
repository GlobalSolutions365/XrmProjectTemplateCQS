using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using Xrm.Domain.Commands;
using Xrm.Models.Crm;

namespace Xrm.UnitTests.Domain.CommandHandlers
{
    [TestClass]
    public class SetAccountNrOfContactsCommandHandlerTests : BaseCrmTest
    {
        private readonly Guid accountId = Guid.NewGuid();
        private readonly Guid triggerContactId = Guid.NewGuid();

        public SetAccountNrOfContactsCommandHandlerTests()
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
        public void AccountsNameIsCorrectlySet()
        {
            SetAccountNrOfContactsCommand cmd = new SetAccountNrOfContactsCommand { FromContact = GetTriggeringContact() };

            CmdBus.Handle(cmd);

            Account account = GetTargetAccount();

            Assert.AreEqual($"I have 2 contacts", account.Name);
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenPassingNullContact()
        {
            SetAccountNrOfContactsCommand cmd = new SetAccountNrOfContactsCommand { FromContact = null };

            Assert.ThrowsException<ArgumentNullException>(() => CmdBus.Handle(cmd));
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
