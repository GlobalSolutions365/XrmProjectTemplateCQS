using Xrm.Application.Commands;
using Xrm.Plugin.Base;
using Ctx = Xrm.Domain.Crm;

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
            localContext.Handle(setAccountNrOfContactsCommand);
        }
    }
}
