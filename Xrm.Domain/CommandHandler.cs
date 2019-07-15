﻿using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain
{
    public abstract class CommandHandler<TCommand, TPostEvent> : IHandleCommand<TCommand> where TCommand : ICommand where TPostEvent : IEvent
    {
        protected readonly IOrganizationService orgService;
        private readonly IEventBus eventBus;

        public CommandHandler(IOrganizationService orgService, IEventBus eventBus)
        {
            this.orgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void Handle(TCommand command)
        {
            if (!Validate(command)) { return; }

            TPostEvent postEvent = Execute(command);
            
            if(postEvent != null && postEvent.GetType() != typeof(Events.VoidEvent))
            { 
                eventBus.NotifyListenersAbout(postEvent);
            }
        }

        public virtual bool Validate(TCommand command) { return true; }

        public virtual TPostEvent Execute(TCommand command) { return default; }

        protected Events.VoidEvent VoidEvent => new Events.VoidEvent();
    }
}
