﻿using System;
using System.IO;
using MetadataDB;

namespace testExtractor
{
	class Program
	{
		static void Main(string[] args)
		{
			GenresList.GenresList.initialize(@"D:\books\_Lib.rus.ec - Официальная\genres_fb2.glst");
			MetadataList.initialize(@"D:\books\_Lib.rus.ec - Официальная\librusec_local_fb2.inpx");
			BookExtractor.BookExtractor.initialize(@"D:\books\_Lib.rus.ec - Официальная\lib.rus.ec");

			var books = MetadataList.selectBooksByTitle("potter");

			foreach (var book in books)
			{
				book.printInfoDebug();
			}
			StreamReader bookStream = new StreamReader(BookExtractor.BookExtractor.extract(books[0]));


			while (!bookStream.EndOfStream)
			{
				Console.Write("{0}", bookStream.ReadToEnd());
			}

		}
	}
}
