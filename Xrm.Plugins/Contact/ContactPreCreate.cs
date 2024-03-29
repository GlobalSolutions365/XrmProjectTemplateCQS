﻿using Xrm.Application.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Domain.Crm;

namespace Xrm.Plugin.Contact
{
    public class ContactPreCreate : Base.Plugin
    {
        public ContactPreCreate() : base(typeof(ContactPreCreate))
        {
            RegisterPluginStep<Ctx.Contact>(EventOperation.Create, ExecutionStage.PreOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Contact targetContact = localContext.GetTarget<Ctx.Contact>();

            var setAccountNrOfContactsCommand = new SetAccountNrOfContactsCommand
            {
                FromContact = targetContact
            };
            localContext.Handle(setAccountNrOfContactsCommand);
        }
    }
}
