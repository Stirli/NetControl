using System;
using System.Collections.Generic;
using System.Linq;
using NetControlCommon.Interfaces;

namespace NetControlCommon
{
    public class ControllerFactory
    {
        private readonly IEnumerable<Type> controllers;

        public ControllerFactory()
        {
            //AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName());
            controllers = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.Contains("NetControl"))
                .SelectMany(ass => ass.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Controller")));
        }

        public IController GetController(string method)
        {
            var controllerClass = controllers.FirstOrDefault(c => c.Name.ToLowerInvariant().StartsWith(method));
            var ctor = controllerClass.GetConstructor(new Type[0]);
            return ctor.Invoke(new object[0]) as IController;
        }
    }
}