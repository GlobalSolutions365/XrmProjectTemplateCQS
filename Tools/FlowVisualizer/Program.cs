using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xrm.Application;

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

                WriteTypeToConsole(commandHandler, parameterType, resultType, 0, parameterType);

                VisualizeEvents(domain, resultType, 1, new List<Type>());

                Console.WriteLine();
            }
        }

        private static void VisualizeEvents(Assembly domain, Type eventType, int level, List<Type> previousEventParameters)
        {
            var eventHandlers = GetTypesImplementingGeneric(domain, typeof(EventHandler<,>), eventType);

            foreach (var eventHandler in eventHandlers)
            {
                (Type parameterType, Type resultType) = GetFlowTypes(eventHandler);                

                WriteTypeToConsole(eventHandler, parameterType, resultType, level, eventType);

                if (previousEventParameters.Contains(resultType))
                {
                    using(new Color(ConsoleColor.Red))
                    { 
                        Console.WriteLine($"!!! Infinite loop detected. The result type {resultType.Name} is used as a parameter previously in the event chain.");
                    }
                    return;
                }

                previousEventParameters.Add(parameterType);

                VisualizeEvents(domain, resultType, level + 1, previousEventParameters);
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
                return types.Where(t => t.BaseType.GetGenericArguments()[0] == parameterType
                                        || t.BaseType.GetGenericArguments()[0].BaseType == parameterType).ToArray();
            }
        }

        private static (Type parameterType, Type resultType) GetFlowTypes(Type type)
        {
            var genericArguments = type.BaseType.GetGenericArguments();

            return (genericArguments[0], genericArguments[1]);
        }

        private static void WriteTypeToConsole(Type type, Type parameter, Type result, int level, Type sourceType)
        {
            Console.Write(new string(' ', level * 4));

            if (type.BaseType.GetGenericArguments()[0] != sourceType)
            {
                if(!inheritedAlternativeHandlers.Contains(sourceType))
                {
                    inheritedAlternativeHandlers.Add(sourceType);
                }

                using (new Color(ConsoleColor.DarkYellow))
                {
                    Console.Write($"(OR_{inheritedAlternativeHandlers.IndexOf(sourceType)}) ");
                }
            }

            using (new Color(ConsoleColor.Cyan))
            {
                Console.Write(type.Name);
            }

            Console.WriteLine($": { parameter.Name} -> {result.Name}");
        }
        private static List<Type> inheritedAlternativeHandlers = new List<Type>();

    }
}
