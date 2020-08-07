using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace HeadWars
{
	/// Manages the database
	static class DatabaseManager
	{
		// Variables
		static string DatabasePath = "Data Source=" + Environment.CurrentDirectory + "\\Data\\Database.db;Version=3";
		static SQLiteConnection connection;

		// Properties
		static Boolean isConnected
		{
			get
			{
				return connection == null ? false : ((connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken) ? false : true);
			}
		}

		// Methods
		public static Boolean Connect()
		{
			if (!isConnected)
			{
				connection = new SQLiteConnection(DatabasePath);

				connection.Open();
			}
			return isConnected;
		}

		public static Boolean Disconnect()
		{
			if (isConnected)
				connection.Close();
			return isConnected;
		}

		/// Checks if a nickname exists
		public static Boolean PlayerExists(string username, string login = "")
		{
			Boolean Out = false;
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("select * from UserH where username='" + username + "' " + login, connection))
				{
					SQLiteDataReader reader = cmd.ExecuteReader();
					Out = reader.HasRows;
					reader.Close();
				}
			}
			return Out;
		}

		/// Creates an account
		public static Boolean CreatePlayer(string username, string password)
		{
			if (isConnected)
			{
				if (!PlayerExists(username))
				{
					using (SQLiteCommand cmd = new SQLiteCommand("insert into UserH values ('" + username + "','" + password + "');insert into PlayerH(username) values ('" + username + "')", connection))
					{
						return cmd.ExecuteNonQuery() > 0;
					}
				}
			}
			return false;
		}

		/// Gets a value in the player's table
		public static List<object> getPlayerData(string username, string column, string table = "PlayerH", string parameter = "")
		{
			List<object> Out = new List<object>();
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("select " + column + " from " + table + " where username='" + username + "' " + parameter, connection))
				{
					SQLiteDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
						if (reader.Read())
						{
							string[] columns = column.Split(new string[] { ", " }, StringSplitOptions.None);
							foreach (string s in columns)
								Out.Add(reader[s]);
						}

					reader.Close();

					return Out;
				}
			}
			return null;
		}

		/// Changes a value in the player's table
		public static Boolean alterPlayerData(string username, string column, int value, string table = "PlayerH", string parameter = "")
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("update " + table + " set " + column + " = " + value + " where username='" + username + "' " + parameter, connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}

		/// Creates the skill associated to the player
		public static Boolean newPlayerSkill(string username, string skillName)
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("insert into PlayerSkillH(username,skill) values ('" + username + "','" + skillName + "')", connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}

		/// Destroyes all the skills associated to the player
		public static Boolean destroySkillTree(string username)
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("delete from PlayerSkillH where username='" + username + "'", connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}

		/// Checks if the player has any skill
		public static Boolean hasSkills(string username)
		{
			Boolean Out = false;
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("select * from PlayerSkillH where username='" + username + "'", connection))
				{
					SQLiteDataReader reader = cmd.ExecuteReader();
					Out = reader.HasRows;
					reader.Close();
				}
			}
			return Out;
		}

		/// Deletes the account
		public static Boolean deletePlayer(string username)
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("delete from UserH where username='" + username + "'", connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}

		/// Adds a code
		public static Boolean newPrizeCode(string code, int prize)
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("insert into CodesH values('" + code + "'," + prize + ")", connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}

		/// Gets a code
		public static string getCode()
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("select code from CodesH limit 1", connection))
				{
					SQLiteDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
					{
						if (reader.Read())
						{
							return (string)reader["code"];
						}
					}
				}
			}
			return null;
		}

		/// Gets the prize of the code
		public static int getPrizeCode(string code)
		{
			int Out = 0;
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("select * from CodesH where code ='" + code + "' collate nocase", connection))
				{
					SQLiteDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
						if (reader.Read())
							Out = (int)reader["prize"];

					reader.Close();
				}
			}
			return Out;
		}

		/// Removes a code
		public static Boolean removePrizeCode(string code)
		{
			if (isConnected)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("delete from CodesH where code='" + code + "'", connection))
				{
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			return false;
		}
	}
}
