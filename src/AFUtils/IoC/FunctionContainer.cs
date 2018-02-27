﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using AFUtils.IoC;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AFUtils.Tests")]
namespace AFUtils
{  
    public class FunctionContainer : IFunctionContainer
    {
       
        internal Dictionary<Type, List<Type>> messageHandlersDictionary = new Dictionary<Type, List<Type>>();


        /// <summary>
        /// Scans the dlls and creates a dictionary in which each message in IFunctions is referenced to each function.
        /// </summary>
        public FunctionContainer()
        {

            var ifunctionTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition()==typeof(IFunction<>)));



            foreach (var t in ifunctionTypes)
            {
                var interfaceType = t.GetInterfaces()[0];
                var messageType = interfaceType.GetGenericArguments()[0];

                List<Type> handlerTypeList;

                if (!messageHandlersDictionary.ContainsKey(messageType))
                {
                    handlerTypeList = new List<Type>();
                    handlerTypeList.Add(t);
                    messageHandlersDictionary.Add(messageType, handlerTypeList);

                }
                else
                {
                    handlerTypeList = messageHandlersDictionary[messageType];
                    handlerTypeList.Add(t);
                    messageHandlersDictionary[messageType]= handlerTypeList;
                }


                

            }
                                                  
        }

        /// <summary>
        /// Calls each function referenced by each message in the dictionary.
        /// </summary>
        public Task InvokeAsync<T>(T message, ITraceWriter log)
        {
            var handlerTypeList = messageHandlersDictionary[message.GetType()];

            foreach (var t in handlerTypeList)
            {
                var handler = Activator.CreateInstance(t);

                object[] parametersArray = new object[] { message, log };

                //var asdf = t.GetMethods()[0];
                //t.GetMethods()[0].GetParameters()[0].ParameterType
                var methodsToInvoke = t.GetMethods().Where(m => m.GetParameters().Any(p => p.ParameterType == message.GetType()));

               /* foreach(var m in t.GetMethods())
                {
                    vvar asdf = m.GetParameters().Any(p => p.ParameterType == message.GetType());
                }*/
                methodsToInvoke.ToList().ForEach(m=> m.Invoke(handler, parametersArray));
            }

            return Task.CompletedTask;

        }

        public void Dispose()
        {
            
        }


    }
}
