using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BookEntity;

namespace MetadataDB
{
	public static class MetadataQuery
	{
		public static List<BookEntity.BookEntity> selectBooksByAuthor
													(this List<BookEntity.BookEntity> metadataList, string authorNameToSearch)
		{
			return metadataList.Where(
				book => book.authors.Any(author => (author.fullName)
					.Contains(authorNameToSearch, StringComparison.OrdinalIgnoreCase))).ToList();
		}

		public static List<BookEntity.BookEntity> selectBooksByTitle
													(this List<BookEntity.BookEntity> metadataList, string titleToSearch)
		{
			return metadataList.Where(
				book => book.title.Contains(titleToSearch, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}

		public static List<BookEntity.BookEntity> selectBooksByGenre
													(this List<BookEntity.BookEntity> metadataList, int genreIDToSearch)
		{
			if (genreIDToSearch < 0)
				return metadataList;
			return metadataList.Where(book => book.genres.Any(genreID => genreID == genreIDToSearch)).ToList();
		}

		public static List<BookEntity.BookEntity> selectBooksByGenres
			(this List<BookEntity.BookEntity> metadataList, Genres genres)
		{
			return genres.Aggregate(metadataList, (current, i) => current.selectBooksByGenre(i));
		}

		public static BookEntity.BookEntity selectBookByID(this List<BookEntity.BookEntity> metadataList, int bookID)
		{																//it is guaranteed that books have unique IDs
			return metadataList.FirstOrDefault(book => book.bookID == bookID);
		}

		public static List<BookEntity.BookEntity> selectBooksByTemplate(this List<BookEntity.BookEntity> metadataList,
			BookEntity.BookEntity template)
		{
			List<BookEntity.BookEntity> books = metadataList;

			books = books.selectBooksByAuthor(template.authors[0].fullName);

			books = books.selectBooksByTitle(template.title);

			books = books.selectBooksByGenres(template.genres);

			return books;
		}


		//Had to reimplement string.Contains method cause it does not work with case insensitive option
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private static bool Contains(this string source, string toCheck, StringComparison stringComparison)
		{
			if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
				return true;	//behaviour of those official string methods is the same
			return source.IndexOf(toCheck, stringComparison) >= 0;
		}
	}
}