using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Xrm.Infrastructure;
using Xrm.Domain.Interfaces;
using ExtendedStepConfig = System.Tuple<int, int, string, int, string, string>;
using ImageTuple = System.Tuple<string, string, int, string>;
// StepConfig           : className, ExecutionStage, EventOperation, LogicalName
// ExtendedStepConfig   : Deployment, ExecutionMode, Name, ExecutionOrder, FilteredAttributes, UserContext
// ImageTuple           : Name, EntityAlias, ImageType, Attributes
using StepConfig = System.Tuple<string, int, string, string>;
using DateProvider;

namespace Xrm.Plugin.Base
{
    /// <summary>
    /// Base class for all Plugins.
    /// </summary>    
    public abstract class Plugin : IPlugin
    {
        private Collection<Tuple<int, string, string, Action<LocalPluginContext>>> registeredEvents;

        /// <summary>
        /// Gets the List of events that the plug-in should fire for. Each List
        /// Item is a <see cref="System.Tuple"/> containing the Pipeline Stage, Message and (optionally) the Primary Entity. 
        /// In addition, the fourth parameter provide the delegate to invoke on a matching registration.
        /// </summary>
        internal Collection<Tuple<int, string, string, Action<LocalPluginContext>>> RegisteredEvents
        {
            get
            {
                if (this.registeredEvents == null)
                {
                    this.registeredEvents = new Collection<Tuple<int, string, string, Action<LocalPluginContext>>>();
                }

                return this.registeredEvents;
            }
        }

        /// <summary>
        /// Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string ChildClassName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref="" cred="Type"/> of the derived class.</param>
        internal Plugin(Type childClassName)
        {
            this.ChildClassName = childClassName.ToString();
        }

        #region Xrm Project Template CQS
        private Bus bus = new Bus();
        #endregion


        /// <summary>
        /// Executes the plug-in.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics CRM caches plug-in instances. 
        /// The plug-in's Execute method should be written to be stateless as the constructor 
        /// is not called for every invocation of the plug-in. Also, multiple system threads 
        /// could execute the plug-in at the same time. All per invocation state information 
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            // Construct the Local plug-in context.
            LocalPluginContext localcontext = new LocalPluginContext(serviceProvider, bus);

            localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));

            try
            {
                // Iterate over all of the expected registered events to ensure that the plugin
                // has been invoked by an expected event
                // For any given plug-in event at an instance in time, we would expect at most 1 result to match.
                Action<LocalPluginContext> entityAction =
                    (from a in this.RegisteredEvents
                     where (
                     a.Item1 == localcontext.PluginExecutionContext.Stage &&
                     a.Item2 == localcontext.PluginExecutionContext.MessageName &&
                     (string.IsNullOrWhiteSpace(a.Item3) ? true : a.Item3 == localcontext.PluginExecutionContext.PrimaryEntityName)
                     )
                     select a.Item4).FirstOrDefault();

                if (entityAction != null)
                {
                    localcontext.Trace(string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} is firing for Entity: {1}, Message: {2}",
                        this.ChildClassName,
                        localcontext.PluginExecutionContext.PrimaryEntityName,
                        localcontext.PluginExecutionContext.MessageName));

                    entityAction.Invoke(localcontext);

                    // now exit - if the derived plug-in has incorrectly registered overlapping event registrations,
                    // guard against multiple executions.
                    return;
                }
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e.ToString()));

                // Handle the exception.
                throw;
            }
            finally
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", this.ChildClassName));
            }
        }


        // Delegate A/S added:
        /// <summary>
        /// The methods exposes the RegisteredEvents as a collection of tuples
        /// containing:
        /// - The full assembly name of the class containing the RegisteredEvents
        /// - The Pipeline Stage
        /// - The Event Operation
        /// - Logical Entity Name (or empty for all)
        /// This will allow to instantiate each plug-in and iterate through the 
        /// PluginProcessingSteps in order to sync the code repository with 
        /// MS CRM without have to use any extra layer to perform this operation
        /// </summary>
        /// <returns></returns>
        /// 

        public IEnumerable<Tuple<string, int, string, string>> PluginProcessingSteps()
        {
            var className = this.ChildClassName;
            foreach (var events in this.RegisteredEvents)
            {
                yield return new Tuple<string, int, string, string>
                    (className, events.Item1, events.Item2, events.Item3);
            }
        }

        #region Additional helper methods

        protected static T GetImage<T>(LocalPluginContext context, ImageType imageType, string name) where T : Entity
        {
            EntityImageCollection collection = null;
            if (imageType == ImageType.PreImage)
            {
                collection = context.PluginExecutionContext.PreEntityImages;
            }
            else if (imageType == ImageType.PostImage)
            {
                collection = context.PluginExecutionContext.PostEntityImages;
            }

            Entity entity;
            if (collection != null && collection.TryGetValue(name, out entity))
            {
                return entity.ToEntity<T>();
            }
            else
            {
                return null;
            }
        }

        protected static T GetImage<T>(LocalPluginContext context, ImageType imageType) where T : Entity
        {
            return GetImage<T>(context, imageType, imageType.ToString());
        }

        protected static T GetPreImage<T>(LocalPluginContext context, string name = "PreImage") where T : Entity
        {
            return GetImage<T>(context, ImageType.PreImage, name);
        }

        protected static T GetPostImage<T>(LocalPluginContext context, string name = "PostImage") where T : Entity
        {
            return GetImage<T>(context, ImageType.PostImage, name);
        }

        #endregion


        #region PluginStepConfig retrieval
        /// <summary>
        /// Made by Delegate A/S
        /// Get the plugin step configurations.
        /// </summary>
        /// <returns>List of steps</returns>
        public IEnumerable<Tuple<StepConfig, ExtendedStepConfig, IEnumerable<ImageTuple>>> PluginProcessingStepConfigs()
        {
            var className = this.ChildClassName;
            foreach (var config in this.PluginStepConfigs)
            {
                yield return
                    new Tuple<StepConfig, ExtendedStepConfig, IEnumerable<ImageTuple>>(
                        new StepConfig(className, config._ExecutionStage, config._EventOperation, config._LogicalName),
                        new ExtendedStepConfig(config._Deployment, config._ExecutionMode, config._Name, config._ExecutionOrder, config._FilteredAttributes, config._UserContext.ToString()),
                        config.GetImages());
            }
        }


        protected PluginStepConfig<T> RegisterPluginStep<T>(
            EventOperation eventOperation, ExecutionStage executionStage, Action<LocalPluginContext> action)
            where T : Entity
        {
            PluginStepConfig<T> stepConfig = new PluginStepConfig<T>(eventOperation, executionStage);
            this.PluginStepConfigs.Add(stepConfig);

            this.RegisteredEvents.Add(
                new Tuple<int, string, string, Action<LocalPluginContext>>(
                    stepConfig._ExecutionStage,
                    stepConfig._EventOperation,
                    stepConfig._LogicalName,
                    new Action<LocalPluginContext>(action)));

            return stepConfig;
        }


        private Collection<IPluginStepConfig> pluginConfigs;
        private Collection<IPluginStepConfig> PluginStepConfigs
        {
            get
            {
                if (this.pluginConfigs == null)
                {
                    this.pluginConfigs = new Collection<IPluginStepConfig>();
                }
                return this.pluginConfigs;
            }
        }
        #endregion

    }


    class AnyEntity : Entity
    {
        public AnyEntity() : base("") { }
    }
}
