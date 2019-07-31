# XRM Project Template CQS

## Purpose

The main purpose of this repository is to provide a clean solution template for building XRM (Dynamics CRM / Dynamics 365 CE) projects. Inspired by the <a href="https://en.wikipedia.org/wiki/Command–query_separation" target="_blank">CQS (Command-Query-Seperation)</a> principle and the <a href="https://en.wikipedia.org/wiki/SOLID" target="_blank">SOLID</a> principles. The aim is to build the solution with small, testable, maintainable and non-conflicting (in sense of source control) blocks. At the same time the amount of non business logic related code should be minimized if not avoided completely.

As the name suggests the template enforces an CQS inspired coding style. There should be a clear separation between code that modifies state (Commands) and code that reads state (Queries). No single method (or in this case even class) should do both. Additionally having separate classes for handling each command results in small, single purpose, classes which are easy to maintain and test.

## Commands, events and queries

The program flow is created using 3 main building blocks:
1. Commands - telling the system to do something, for example "UpdateChildContactCount" command in the context of an Account. Each **command** should by design have exactly one **command handler**, which contains the implementation of handling the command.
2. Events - telling the system that something has happened, for example "ChildContactCountUpdated". Each **event** can have any number (0..N) subscribed **event handlers**, which contain the implementation of handling the event.
3. Queries - used to retrieve data from CRM

Commands and events depend on the queries (since often the decision what to do depends on data we need to retrieve). 

**Queries** depend only on CRM. Commands and events don't do any query data directly, instead they do it through the queries. **Commands**, when done, can raise events. **Events**, when done, can raise events. The image below depicts their interactions.

![Components](Docs/Images/Components.svg)

## How does it work?

Let's assume we want to implement a flow like this: Whenever a contact is created or updated it's parent accounts name should be updated to reflect how many child contacts it has - something like "I have {N} contacts". After this is done, the contacts last name should be updated to something like "My parent account has {N} contacts". Then after this is done a not should be created on the contact with text like "I am a note".

The solution involves a new component, that has previously not been mentioned - the command and event **bus**. Without going into details - it's something that's responsible for executing command handlers for commands and event handlers for events. Additionally it takes care of injecting all required dependencies.

The flow described above should look something like this:

```
SetAccountNrOfContactsCommand
   -> AccountNrOfContactsSetEvent
       -> AccountChildContactCountSetInLastNameEvent
```

Because the trigger will be an create or update of a Contact and part of the flow touches the same contact that triggered the flow, we'll implement it using 2 pre-operation plugins: ContactPreCreate and ContactPreUpdate.

We start by defining the **Command** the plugins will be issuing. This can be something like:

```csharp
public class SetAccountNrOfContactsCommand : ICommand
{
    public Contact FromContact { get; set; }
}
```

This is just a simple POCO class, implementing an empty ```ICommand``` interface.

Next let's create our two plugins:

```csharp
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
        CommandBus.Handle(setAccountNrOfContactsCommand);
    }
}
```

```csharp
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
        CommandBus.Handle(setAccountNrOfContactsCommand);
    }
}
```

If you don't recognize the above code, this is because the **XRM Project Template** uses a slightly customized version of the <a href="https://github.com/delegateas/Daxif/" target="_blank">Delegate DAXIF#</a> framework. The main feature we utilize here is automatic plugin registration, based on metadata in the code itself - the ```RegisterPluginStep``` method. Also all the things you would normally expect in the plugin, like obtaining the CRM's ``IOrganizationService`` reference, ``ITracingService`` and ``IPluginExecutionContext`` are already handled in the ```Base.Plugin```. This follows the philosophy, explained on top of this page, of avoiding any unnecessary filler code and focusing only on what is absolutely required. 

All this plugin does is setup a simple **Command** a  and pass it over to the **Command bus**, which will try to find a suitable **Command handler** and execute it.

Let's leave the **Command bus** implementation a black box. Ideally you will never need to touch it (except for maybe adding new dependencies into the DI Container - described later). Right now we have a **Command** so we need an corresponding **Command handler**. But we can also foresee it will need some way to get the number of child contacts of a certain account. So let's start by creating a query:

```csharp
public class AccountQueries : CrmQuery<Account>
{
    public AccountQueries(IOrganizationService orgService) : base(orgService) { }

    public int GetNrOfContacts(Guid accountId)
    {
        using (XrmContext xrm = new XrmContext(OrgService))
        {
            return xrm.ContactSet
                      .Where(c => c.ParentCustomerId != null && c.ParentCustomerId.Id == accountId)
                      .Select(c => c.Id)
                      .ToArray()
                      .Length;
        }
    }
}
```

Any **Query** should be a class extending the abstract ```CrmQuery<TEntity>``` class. The command bus will automatically set any constructor injected dependencies. In this case we only need a reference to the ```IOrganizationService```. Next we just implement the query method we need, in this case ```GetNrOfContacts```.

Having the required query, we can implement the **Command handler**:

```csharp
public class SetAccountNrOfContactsCommandHandler : CommandHandler<SetAccountNrOfContactsCommand, AccountNrOfContactsSetEvent>
{
    private readonly AccountQueries accountQueries;

    public SetAccountNrOfContactsCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus,
        AccountQueries accountQueries)
        : base(orgServiceWrapper, eventBus)
    {
        this.accountQueries = accountQueries ?? throw new ArgumentNullException(nameof(accountQueries));
    }

    public override AccountNrOfContactsSetEvent Execute(SetAccountNrOfContactsCommand command)
    {
        command.FromContact = command.FromContact ?? throw new ArgumentNullException(nameof(command.FromContact));

        if (command.FromContact.ParentCustomerId == null)
        {
            return null;
        }

        int nrOfContacts = accountQueries.GetNrOfContacts(command.FromContact.ParentCustomerId.Id);

        Account account = new Account
        {
            Id = command.FromContact.ParentCustomerId.Id,
            Name = $"I have {nrOfContacts} contacts"
        };
        orgServiceWrapper.OrgServiceAsSystem.Update(account);

        return new AccountNrOfContactsSetEvent { TargetContact = command.FromContact };
    }
}
```

A command handler is a class inheriting from ```CommandHandler<TCommand, TResultEvent>```. In other words it handler a command of type ```TCommand``` and when done produces an event of type ```TResultEvent```. In this case, according to the requirements we handle a ```SetAccountNrOfContactsCommand``` and raise an ```AccountNrOfContactsSetEvent```. All the required dependencies should be defines in the constructor. Two of them are required (by the base class) - ```IOrganizationServiceWrapper orgServiceWrapper``` (specific to plugins and custom workflow activities - aggregates the service in the context of the current user and the SYSTEM user) and ```IEventBus eventBus``` (for handling events), the rest depends only on what you need in the command handler. 

> In the constructor take in only the dependencies you require in this specific command handler. Every command and event handler is constructed independently and on demand by the event bus. 
> For passing around state use the Command and Event POCO objects.

How does a command handler work? If you look at the source of ```CommandHandler<TCommand, TResultEvent>``` you'll see it's a simple implementation of the <a href="https://en.wikipedia.org/wiki/Template_method_pattern" target="_blank">template method pattern</a>.

```csharp
public abstract class CommandHandler<TCommand, TPostEvent>
{
    public CommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus)
    {
        // ...
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

    public abstract TPostEvent Execute(TCommand command);
}
```
(Simplified a little but compared to the actual implementation, but not much ;))

1. It calls the virtual ```Validate``` method, with the command as argument. Because the method is virtual it's not required to be implemented in classes extending ```CommandHandler<TCommand, TResultEvent>```. If you want to implement your own validation it should be here. Either return false if you want it to silently fail (stop the flow) or throw an exception if you want it to be loud :bomb:.
2. It calls the abstract ```Execute``` method, which takes in the command and returns an event. This method is abstract, so it's required to be implemented (else the whole command handler wouldn't make much sense).
3. If the event is not-null and not an ```VoidEvent``` it will ask the event bus to notify all listeners about it (0 or more). Conversely if you return null or a ```VoidEvent``` the flow will stop at this command handler. 

To finish up, we need to create the two events and event handlers. How this works is almost the same as for commands and command handlers.

```csharp
public class AccountNrOfContactsSetEvent : IEvent
{
    public Contact TargetContact { get; set; }
}

public class AccountNrOfContactsSetEventHandler : EventHandler<AccountNrOfContactsSetEvent, AccountChildContactCountSetInLastNameEvent>
{
    public AccountNrOfContactsSetEventHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
    {
    }

    public override AccountChildContactCountSetInLastNameEvent Execute(AccountNrOfContactsSetEvent @event)
    {
        // ...

        return new AccountChildContactCountSetInLastNameEvent
        {
            // ...
        };
    }
}

public class AccountChildContactCountSetInLastNameEvent : IEvent
{
    // ...
}

public class AccountChildContactCountSetInLastNameEventHandler : EventHandler<AccountChildContactCountSetInLastNameEvent, VoidEvent>
{
    public AccountChildContactCountSetInLastNameEvent(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
    {
    }

    public override VoidEvent Execute(AccountChildContactCountSetInLastNameEvent @event)
    {
        // ...

        return VoidEvent;
    }
}
```

We didn't fill in all the code above because it's mostly implementation detail, but it shows how a flow should be built out of it's simple building blocks.

You might wander about this line: ```return VoidEvent;```. Fortunately the is nothing magical about it. There is a class called ``VoidEvent`` so this could be written like so ```return new VoidEvent()```, but that's a bit ugly. Similar on how ASP.NET MVC has factory methods inside the Controler class (like ```return View()```, instead of ```return new View()``` we have a simple factory property in both the ```CommandHandler<T1,T2>``` and ```EventHandler<T1,T2>``` base classes.

```protected Events.VoidEvent VoidEvent => new Events.VoidEvent();```

That's it.

The example above might seem a bit complex at first, but bear in mind all you really need to do is create the command / event POCO classes and implement the ```Execute``` methods of their respective handlers. When you create a new handler and inherti from ```CommandHandler<T1,T2>``` or ```EventHandler<T1,T2>``` most of the code (including the minimum required constructor) will be generated automatically by Visual Studio if you Ctr+Space a few time on the red squiggles.



## Leveraging on other Open Source projects