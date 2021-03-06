﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GenresList
{
	[DataContract]
	public class GenresListEntity
	{
		[DataMember]
		public int genreGroupNumber { get; private set; }
		[DataMember]
		public int genreNumber { get; private set; }
		[DataMember]
		public int subgenreNumber { get; private set; }
		[DataMember]
		public string name { get; private set; }
		[DataMember]
		public string description { get; private set; }

		public GenresListEntity(string genreString)
		{
			parse(genreString);
		}

		private void parse(string genreString)
		{
			splitAndSetFields(genreString);
		}

		private const char numberAndNameDelimiter = ' ';
		private const int genreAndSubgenrePosition = 0;		//формат строки жанра:
		private const int nameAndDescriptionPosition = 1;	//<genreGroupNumber>.<genreNumber>.<subgenreNumber> <name>;<description>
		//разделим сначала по пробелу
		//потом куски будем делить по точке и по точке с запятой
		private void splitAndSetFields(string genreString)
		{
			string[] genreStringSplitted = genreString.Split(numberAndNameDelimiter);
			setGenreAndSubgenreNumber(genreStringSplitted[genreAndSubgenrePosition]);
			setNameAndDescription(genreStringSplitted[nameAndDescriptionPosition]);

			//костыль: описание может быть через пробелы, надо добавить все то, что было упущено
			addLastWordsOfDescription(genreStringSplitted);
		}

		private const char genreAndSubgenreDelimiter = '.';
		private const int genreGroupPosition = 0;
		private const int genrePosition = 1;
		private const int subgenrePosition = 2;
		private void setGenreAndSubgenreNumber(string genreAndSubgenre)
		{
			string[] genreAndSubgenreSplitted = genreAndSubgenre.Split(genreAndSubgenreDelimiter);
			genreGroupNumber = Convert.ToInt32(genreAndSubgenreSplitted[genreGroupPosition]);
			genreNumber = Convert.ToInt32(genreAndSubgenreSplitted[genrePosition]);
			subgenreNumber = Convert.ToInt32(genreAndSubgenreSplitted[subgenrePosition]);
		}

		private const char nameAndDescriptionDelimiter = ';';
		private const int namePosition = 0;
		private const int descriptionPosition = 1;
		private void setNameAndDescription(string nameAndDescription)
		{
			string[] nameAndDescriptionSplitted = nameAndDescription.Split(nameAndDescriptionDelimiter);
			name = nameAndDescriptionSplitted[namePosition];
			description = nameAndDescriptionSplitted[descriptionPosition];
		}


		private const int lastWordsOfDescriptionPosition = nameAndDescriptionPosition + 1;
		private void addLastWordsOfDescription(IReadOnlyList<string> genreStringSplitted)
		{
			for (int i = lastWordsOfDescriptionPosition; i < genreStringSplitted.Count; i++)
			{
				description += ' ' + genreStringSplitted[i];
			}
		}

	}
}
