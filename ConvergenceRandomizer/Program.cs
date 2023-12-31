﻿using SharpMonoInjector;
using System;
using System.IO;
using System.IO.Pipes;

namespace ConvergenceRandomizer
{
    class Program
    {
        private static void Main()
        {
            
            string processName = "Convergence";
            string assemblyPath = "Randomizer.dll";
            string @namespace = "Randomizer";
            string className = "RandomizerLoader";
            string methodName = "Init";
            string currentDirectory = Directory.GetCurrentDirectory();
            //Injector injector = new Injector(processName);

            //Inject(injector, assemblyPath, @namespace, className, methodName);

            //SendInformation(currentDirectory);
        }

        private static void Inject(Injector injector, string assemblyPath, string @namespace, string className, string methodName)
        {
            byte[] assembly;
            try
            {
                assembly = File.ReadAllBytes(assemblyPath);
            }
            catch
            {
                Console.WriteLine("Could not read the file " + assemblyPath);
                return;
            }


            using (injector)
            {
                IntPtr remoteAssembly = IntPtr.Zero;

                try
                {
                    remoteAssembly = injector.Inject(assembly, @namespace, className, methodName);
                }
                catch (InjectorException ie)
                {
                    Console.WriteLine("Failed to inject assembly: " + ie);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Failed to inject assembly (unknown error): " + exc);
                }

                if (remoteAssembly == IntPtr.Zero)
                    return;

                Console.WriteLine($"{Path.GetFileName(assemblyPath)}: " + (injector.Is64Bit ? $"0x{remoteAssembly.ToInt64():X16}" : $"0x{remoteAssembly.ToInt32():X8}"));
            }
        }


        private static void SendInformation(string information)
        {
            Console.WriteLine("Creating pipe...");
            using (NamedPipeServerStream serverPipe = new NamedPipeServerStream("PipeCR", PipeDirection.Out))
            {
                Console.WriteLine("Waiting for the DLL to connect...");
                serverPipe.WaitForConnection();

                using (StreamWriter writer = new StreamWriter(serverPipe))
                {
                    writer.WriteLine(information);
                    writer.Flush(); // Flush the data to the pipe before closing
                    serverPipe.WaitForPipeDrain(); // Wait until all data is written
                }
            }
        }
    }
}
