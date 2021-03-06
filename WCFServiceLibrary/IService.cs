﻿using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using BookEntity;
using GenresList;

namespace WCFServiceLibrary
{
	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByGenres(Genres genres);

		[OperationContract]
		BookEntity.BookEntity selectBookByID(int bookID);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByTemplate(BookEntity.BookEntity template);

		[OperationContract]
		Stream extractBook(BookEntity.BookEntity book);

		[OperationContract]
		Stream extractBookByID(int bookID);

		[OperationContract]
		List<GenresListEntity> getAvailableGenres();
	}



}
