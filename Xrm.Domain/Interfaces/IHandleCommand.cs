namespace Xrm.Domain.Interfaces
{
    public interface IHandleCommand<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
