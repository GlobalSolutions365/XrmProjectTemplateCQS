using System;
using System.Linq;
using System.Reflection;
using Xrm.Domain;

namespace FlowVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly domain = typeof(Locator).Assembly;

            VisualizeCommandHandlers(domain);
        }

        private static void VisualizeCommandHandlers(Assembly domain)
        {
            var commandHandlers = GetTypesImplementingGeneric(domain, typeof(CommandHandler<,>));

            foreach (var commandHandler in commandHandlers)
            {
                (Type parameterType, Type resultType) = GetFlowTypes(commandHandler);

                Console.WriteLine(Format(commandHandler, parameterType, resultType, 0));

                VisualizeEvents(domain, resultType, 1);

                Console.WriteLine();
            }
        }

        private static void VisualizeEvents(Assembly domain, Type eventType, int level)
        {
            var eventHandlers = GetTypesImplementingGeneric(domain, typeof(EventHandler<,>), eventType);

            foreach (var eventHandler in eventHandlers)
            {
                (Type parameterType, Type resultType) = GetFlowTypes(eventHandler);

                Console.WriteLine(Format(eventHandler, parameterType, resultType, level));

                VisualizeEvents(domain, resultType, level + 1);
            }
        }

        private static Type[] GetTypesImplementingGeneric(Assembly assembly, Type genericType, Type parameterType = null)
        {
            var types = (from type in assembly.GetTypes()
                         let baseType = type.BaseType
                         where
                             !type.IsAbstract && !type.IsInterface &&
                             baseType != null && baseType.IsGenericType &&
                             baseType.GetGenericTypeDefinition() == genericType
                         orderby type.Name
                         select type
                       ).ToArray();

            if (parameterType == null)
            {
                return types;
            }
            else
            {
                return types.Where(t => t.BaseType.GetGenericArguments()[0] == parameterType).ToArray();
            }
        }

        private static (Type parameterType, Type resultType) GetFlowTypes(Type type)
        {
            var genericArguments = type.BaseType.GetGenericArguments();

            return (genericArguments[0], genericArguments[1]);
        }

        private static string Format(Type type, Type parameter, Type result, int level)
        {
            string padding = new string(' ', level * 4);

            return $"{padding}{type.Name}: {parameter.Name} -> {result.Name}";
        }
    }
}
