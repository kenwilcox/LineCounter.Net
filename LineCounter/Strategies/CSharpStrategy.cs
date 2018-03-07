﻿using System;
using System.IO;
using LineCounter;

namespace TeamBinary.LineCounter
{
	public class CSharpStrategy : IStrategy
	{
		static TrimStringLens l = new TrimStringLens();
		public Statistics Count(string path)
		{
			var lines = File.ReadAllLines(path);

			return Count(lines);
		}

		public Statistics Count(string[] lines)
		{
			var res = new Statistics();

			foreach (var line in lines)
			{
				l.SetValue(line);
				if (l.IsWhitespace())
					continue;

				if (l == "{" || l == "}" || l == ";" || l == "};")
					continue;

				if (l.StartsWithOrdinal("/"))
				{
					if (l.StartsWithOrdinal("/// "))
					{
						if (l == "/// <summary>" || l == "/// </summary>")
							continue;

						res.DocumentationLines++;
						continue;
					}

					if (l.StartsWithOrdinal("//"))
						continue;
				}

				res.CodeLines++;
			}

			return res;
		}
	}
}