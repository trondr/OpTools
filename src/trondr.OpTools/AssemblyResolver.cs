// File: AssemblyResolver.cs
// Project Name: NMultiTool.Console
// Project Home: https://github.com/trondr
// License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
// Credits: See the Credit folder in this project
// Copyright © <github.com/trondr> 2013 
// All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace trondr.OpTools
{
    /// <summary>
    /// 
    /// The assembly resolver enables deployment of an executable with all referenced dlls included as embedded 
    /// resources in the executable. To achieve this. 1. Include this AssemblyResolver.cs in the in your console 
    /// or windows project. 2. Create a 'Libs' folder in the project. 3. Copy the referenced dlls to the 
    /// 'Libs' folder and include the dlls in the project. 4. Set build action to 'Embedded Resource' for each dll.
    /// 5. Add a static constructor to your Program.cs as shown in the example below to activate the assembly resolver.
    /// 
    /// </summary>
    ///  
    /// <example>
    /// 
    /// <code>
    /// class Program
    /// {
    ///    #region Constructor
    ///    static Program()
    ///    {         
    ///       AppDomain.CurrentDomain.AssemblyResolve += new AssemblyResolver().ResolveHandler;
    ///    }
    ///    
    ///
    ///   [STAThread]
    ///   static void Main(string[] args)
    ///   {
    ///      //Your code
    ///   }
    /// }
    /// </code>
    /// #endregion
    /// 
    /// </example>
    internal class AssemblyResolver
    {
        private readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// Custom assembly resolver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly ResolveHandler(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            if (_assemblies.ContainsKey(assemblyName))
            {
                return _assemblies[assemblyName];
            }

            var resourceAssembly = Assembly.GetExecutingAssembly();
            var resourceNameSpace = this.GetType().Namespace;
            var resourceName = $"{resourceNameSpace}.Libs.{assemblyName}.dll";
            if (resourceName.EndsWith("XmlSerializers.dll"))
            {
                return null;
            }
            var assembly = LoadAssemblyFromResource(assemblyName, resourceName, resourceAssembly);
            if (assembly == null)
            {
                //System.Console.WriteLine("");
                resourceAssembly = Assembly.GetEntryAssembly();
                resourceNameSpace = resourceAssembly.GetTypes()[0].Namespace;
                resourceName = $"{resourceNameSpace}.Libs.{assemblyName}.dll";
                assembly = LoadAssemblyFromResource(assemblyName, resourceName, Assembly.GetEntryAssembly());
            }
            _assemblies.Add(assemblyName, assembly);
            return assembly;
        }

        public Assembly LoadAssemblyFromResource(string assemblyName, string resourceName, Assembly resourceAssembly)
        {
#if DEBUG
            System.Console.WriteLine($"Trying to load assembly '{assemblyName}.dll' from embedded resource '{resourceName}'...");
#endif
            byte[] assemblyData;
            using (var dllStream = resourceAssembly.GetManifestResourceStream(resourceName))
            {
                if (dllStream == null)
                {
#if DEBUG
                    System.Console.WriteLine("Failed to load assembly '{0}.dll' from embedded resource '{1}'. Assembly not found.",assemblyName, resourceName);
#endif
                    return null;
                }
                assemblyData = new BinaryReader(dllStream).ReadBytes((int)dllStream.Length);
            }
            var assembly = Assembly.Load(assemblyData);
#if DEBUG
            System.Console.WriteLine("Sucessfully loaded assembly '{assemblyName}.dll' from embedded resource '{resourceName}'.");
#endif
            return assembly;
        }
    }
}