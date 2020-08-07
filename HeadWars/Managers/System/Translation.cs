using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace HeadWars
{
	// Deals with the translation strings
	static class Translation
	{
		// Variables
		/// Available langues
		public static List<string> langues = new List<string> {
			"en",
			"pt",
			"es",
			"fr"
		};

		private static string currentLanguage;
		private static ResourceManager manager;
		private static CultureInfo langue;

		// Properties
		public static string CurrentLanguage
		{
			get
			{
				return currentLanguage;
			}
		}
		public static string WindowsLanguage
		{
			get
			{
				return CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
			}
		}

		// Methods
		/// Constructor
		static Translation()
		{
			manager = new ResourceManager("HeadWars.Translations.lang", typeof(HeadWars).Assembly);

			currentLanguage = currentLanguage == null ? "en" : currentLanguage;
			createLanguage(currentLanguage);
		}

		/// Creates the language manager
		private static void createLanguage(string language)
		{
			langue = CultureInfo.CreateSpecificCulture(language);
		}

		/// Sets the language
		public static void setLanguage(string language)
		{
			language = (langues.Contains(language)) ? language : "en";

			if (language != currentLanguage)
				createLanguage(currentLanguage = language);
		}

		/// Gets the string
		public static string GetString(string index)
		{
			return manager.GetString(index, langue) != null ? manager.GetString(index, langue) : "TRANSLATION ERROR ( 0 ~> Translation index not found. ) : [" + index + "] [" + currentLanguage + "]";
		}
	}
}