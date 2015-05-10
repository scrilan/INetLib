﻿using System;
using System.Net;
using System.ServiceModel;
using WCFServiceLibrary;

namespace INetLibServiceHost
{
	static class Program
	{
		static void Main(string[] args)
		{
			ServerInitialization.initialize();

			tryStartService();
		}

		private static void tryStartService()
		{
			try
			{
				startService();
			}
			catch (AddressAccessDeniedException)
			{
				Console.WriteLine("Failed to start the service. Try run it as an administrator.");
				Console.WriteLine("Terminating.");
			}
			catch (AddressAlreadyInUseException)
			{
				Console.WriteLine("Failed to start the service. Is another instance running?");
				Console.WriteLine("Terminating.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to start the service.");
				Console.WriteLine(e.Message);
				Console.WriteLine("Terminating.");
			}
		}

		private static void startService()
		{
			ServiceHost host = new ServiceHost(typeof (Service));

			host.addINetLibServiceEndpoint();

			host.Open();
			Console.WriteLine("INetLib host service started at " + DateTime.Now);
			Console.WriteLine("Press any key to stop the host service.");
			Console.ReadKey();
			host.Close();
		}

		private static void addINetLibServiceEndpoint(this ServiceHost host)
		{
			string hostURI = "net.tcp://" + Dns.GetHostName() + ":14141/INetLib";
			host.AddServiceEndpoint(typeof (IService), new NetTcpBinding
			{
				Security =
				{
					Mode = SecurityMode.None,
					Transport = {ClientCredentialType = TcpClientCredentialType.None},
					Message = {ClientCredentialType = MessageCredentialType.None}
				}
			}, hostURI);
		}
	}
}