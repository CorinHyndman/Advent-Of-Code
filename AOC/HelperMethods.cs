using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOC
{
	public static class HelperMethods
	{
		public static int Year { get; set; }
		public static Assembly ExectuingAssembly { get; set; }

		#region General
		/// <summary>Gets the input data for a specified day</summary>
		public static string GetInput(int day)
		{
			string resourceName = $"_{Year}.Input.{day}.txt";

			string[] embeddedResources = ExectuingAssembly.GetManifestResourceNames();
			if (!embeddedResources.Contains(resourceName))
			{
				Console.WriteLine(@$"Could Not Find ""{resourceName}""");
				return null;
			}

			using (var reader = new StreamReader(stream: ExectuingAssembly.GetManifestResourceStream(resourceName)))
			{
				return reader.ReadToEnd();
			}
		}
		public static bool AnyElementMeetsCriteria<T>(this T[,] array, Func<T, bool> criteria)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (criteria(array[i,j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Returns a bool indicating whether the element has been successfully accessed</summary>
		public static bool TryGetElementAt<T>(this T[] array, int index, out T result)
		{
			if (-1 < index && index < array.Length)
			{
				result = array[index];
				return true;
			}
			result = default;
			return false;
		}
		/// <summary>Returns a bool indicating whether the element has been successfully accessed</summary>
		public static bool TryGetElementAt<T>(this T[,] array, int Col, int Row, out T result)
		{
			if (-1 < Col && -1 < Row && Col < array.GetLength(0) && Row < array.GetLength(1))
			{
				result = array[Col,Row];
				return true;
			}
			result = default;
			return false;
		}
#if DEBUG
		public static void Draw<T>(T[] array, string prefix = null, string suffix = null)
		{
			Console.WriteLine(prefix);
			foreach (T item in array)
			{
				Console.WriteLine(item);
			}
			Console.WriteLine(suffix);
		}
		public static void Draw<T>(T[,] array, string prefix = null, string suffix = null)
		{
			Console.WriteLine(prefix);
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					Console.Write(array[i,j]);
					if (j != array.GetLength(1) - 1)
					{
						Console.Write(", ");
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine(suffix);
		}
#endif
#endregion

#region DaySpecific
		public static bool ForeachElementThatMeetsCriteria<T>(this T[,] array, Func<(T value, (int, int) position), bool> criteria, Action<int, int> action)
		{
			//Day 11
			bool flag = false;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (criteria((array[i, j], (i, j))))
					{
						action(i, j);
						flag = true;
					}
				}
			}
			return flag;
		}
#endregion
	}
}
