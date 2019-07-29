using Xrm.Domain.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Models.Crm;

namespace Xrm.Plugin.Contact
{
    public class ContactPostCreate : BasePlugin
    {
        public ContactPostCreate() : base(typeof(ContactPostCreate))
        {
            RegisterPluginStep<Ctx.Contact>(EventOperation.Create, ExecutionStage.PostOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Contact targetContact = localContext.GetTarget<Ctx.Contact>();

            var setAccountNrOfContactsCommand = new SetAccountNrOfContactsCommand
            {
                FromContact = targetContact
            };
            CommandBus.Handle(setAccountNrOfContactsCommand);
        }
    }
}
