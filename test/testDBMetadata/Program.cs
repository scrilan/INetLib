﻿using System;
using System.Diagnostics;
using MetadataDB;

namespace testDBMetadata
{
	class Program
	{
		static void Main(string[] args)
		{
			//These two initializations are always mandatory 
			Stopwatch timer = new Stopwatch();

			timer.Start();
			GenresList.GenresList.initialize(@"D:\books\_Lib.rus.ec - Официальная\genres_fb2.glst");
			MetadataList.initialize(@"D:\books\_Lib.rus.ec - Официальная\librusec_local_fb2.inpx");
			timer.Stop();
			Console.WriteLine("Initialization time:	{0}", timer.Elapsed);

			timer.Restart();
			var books = MetadataList.selectBooksByAuthor("Линдгрен");
			timer.Stop();
			Console.WriteLine("Author search time:	{0}", timer.Elapsed);

			timer.Restart();
			books = MetadataList.selectBooksByTitle("Поттер");
			timer.Stop();
			Console.WriteLine("Title search time:	{0}", timer.Elapsed);
			foreach (var book in books)
			{
				book.printInfoDebug();
			}
		}
	}
}
