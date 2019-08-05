using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Infrastructure;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;
using Xrm.UnitTests.Fakes;

namespace Xrm.UnitTests.Resolver
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public void CanResolveCommandHandler()
        {
            var context = new XrmFakedContext();

            ICommandBus cmdBus = new Bus(new OrganizationServiceWrapper(context.GetOrganizationService()), new FakeTracingService());

            TestCommand cmd = new TestCommand { IsHandled = false };

            cmdBus.Handle(cmd);

            Assert.IsTrue(cmd.IsHandled);
        }

        [TestMethod]
        public void CanResolveCommandHandlerWithUserContextRepo()
        {
            var context = new XrmFakedContext();

            ICommandBus cmdBus = new Bus(new OrganizationServiceWrapper(context.GetOrganizationService(), context.GetOrganizationService()), new FakeTracingService());

            SetAccountNrOfContactsCommand cmd = new SetAccountNrOfContactsCommand { FromContact = new Contact() };

            cmdBus.Handle(cmd);
        }

        [TestMethod]
        public void CanResolveEventHandlers()
        {
            var context = new XrmFakedContext();

            IEventBus cmdBus = new Bus(new OrganizationServiceWrapper(context.GetOrganizationService()), new FakeTracingService());

            TestEvent @event = new TestEvent();

            cmdBus.NotifyListenersAbout(@event);

            Assert.IsTrue(@event.IsHandled1);
            Assert.IsTrue(@event.IsHandled2);
        }

        [TestMethod]
        public void EventIsExecutedAfterCommand()
        {
            var context = new XrmFakedContext();

            ICommandBus cmdBus = new Bus(new OrganizationServiceWrapper(context.GetOrganizationService()), new FakeTracingService());

            TestCommand cmd = new TestCommand { IsHandled = false };

            cmdBus.Handle(cmd);

            Assert.IsTrue(TestCommandExecutedEvent.IsHandled);
        }
    }
}

