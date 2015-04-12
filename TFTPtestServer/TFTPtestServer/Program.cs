﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tftp.Net;

namespace TFTPtestServer
{
	internal class Program
	{
		private static String ServerDirectory;

		private static void Main(string[] args)
		{
			ServerDirectory = Environment.CurrentDirectory;

			Console.WriteLine("Running TFTP server for directory: " + ServerDirectory);
			Console.WriteLine();
			Console.WriteLine("Press any key to close the server.");

			using (var server = new TftpServer())
			{
				server.OnReadRequest += new TftpServerEventHandler(server_OnReadRequest);
				server.OnWriteRequest += new TftpServerEventHandler(server_OnWriteRequest);
				server.Start();
				Console.Read();
			}
		}

		private static void server_OnWriteRequest(ITftpTransfer transfer, EndPoint client)
		{
			String file = Path.Combine(ServerDirectory, transfer.Filename);

			if (File.Exists(file))
			{
				Console.WriteLine("[" + transfer.Filename + "] Rejecting Write request from " + client + " for " + transfer.Filename +
				                  ": File already exists");
				transfer.Cancel(TftpErrorPacket.FileAlreadyExists);
			}
			else
			{
				Console.WriteLine("[" + transfer.Filename + "] Write request from " + client + " for " + transfer.Filename);
				transfer.Start(new FileStream(file, FileMode.CreateNew));
			}
		}

		private static void server_OnReadRequest(ITftpTransfer transfer, EndPoint client)
		{
			Console.WriteLine("[" + transfer.Filename + "] Read request from " + client + " for " + transfer.Filename);

			String path = Path.Combine(ServerDirectory, transfer.Filename);
			FileInfo file = new FileInfo(path);

			//Is the file within the server directory?
			if (!file.FullName.ToLower().StartsWith(ServerDirectory.ToLower()))
			{
				Console.WriteLine("[" + transfer.Filename + "] Denying request because the file is outside the server directory.");
				transfer.Cancel(TftpErrorPacket.AccessViolation);
				return;
			}

			//Does the file exist at all?
			if (!file.Exists)
			{
				Console.WriteLine("[" + transfer.Filename + "] Denying request because the file does not exist.");
				transfer.Cancel(TftpErrorPacket.FileNotFound);
				return;
			}

			//Ok, start the transfer
			Stream str = new FileStream(file.FullName, FileMode.Open);
			Console.WriteLine("[" + transfer.Filename + "] Accepting request");
			transfer.OnProgress += new TftpProgressHandler(transfer_OnProgress);
			transfer.OnError += new TftpErrorHandler(transfer_OnError);
			transfer.OnFinished += new TftpEventHandler(transfer_OnFinished);
			transfer.Start(str);
		}

		private static void transfer_OnError(ITftpTransfer transfer, TftpTransferError error)
		{
			Console.WriteLine("[" + transfer.Filename + "] Error: " + error);
		}

		private static void transfer_OnFinished(ITftpTransfer transfer)
		{
			Console.WriteLine("[" + transfer.Filename + "] Finished.");
		}

		private static void transfer_OnProgress(ITftpTransfer transfer, TftpTransferProgress progress)
		{
			Console.WriteLine("[" + transfer.Filename + "] Progress: " + progress);
		}
	}

}