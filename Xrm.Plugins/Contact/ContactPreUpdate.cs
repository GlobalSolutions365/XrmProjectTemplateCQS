using Xrm.Domain.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Models.Crm;

namespace Xrm.Plugin.Contact
{
    public class ContactPreUpdate : Base.Plugin
    {
        public ContactPreUpdate() : base(typeof(ContactPreUpdate))
        {
            RegisterPluginStep<Ctx.Contact>(EventOperation.Update, ExecutionStage.PreOperation, Execute);
        }

        private void Execute(LocalPluginContext localContext)
        {
            Ctx.Contact targetContact = localContext.GetTarget<Ctx.Contact>();

            var setAccountNrOfContactsCommand = new SetAccountNrOfContactsCommand
            {
                FromContact = targetContact
            };
            localContext.CommandBus.Handle(setAccountNrOfContactsCommand);
        }
    }
}
