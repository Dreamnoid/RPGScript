﻿using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RPGScript
{
	internal static class Lexer
	{
		public static Queue<Token> Parse(string fullScript, string filename)
		{
			var lines = fullScript.Split('\n');
			var tokens = new Queue<Token>();
			for (int i = 0; i < lines.Length; ++i)
			{
				Parse(lines[i], new Source(filename, i + 1), tokens);
			}
			return tokens;
		}

		public static void Parse(string line, Source source, Queue<Token> tokens)
		{
			var chars = new SourceQueue(line);
			while (chars.Any())
			{
				var c = chars.Dequeue();
				if (c == Syntax.StartTable)
				{
					tokens.Enqueue(new Token.StartTable() { Source = source });
				}
				else if (c == Syntax.EndTable)
				{
					tokens.Enqueue(new Token.EndTable() { Source = source });
				}
				else if (c == Syntax.StartList)
				{
					tokens.Enqueue(new Token.StartList() { Source = source });
				}
				else if (c == Syntax.EndList)
				{
					tokens.Enqueue(new Token.EndList() { Source = source });
				}
				else if (c == Syntax.StartFunction)
				{
					tokens.Enqueue(new Token.StartFunction() { Source = source });
				}
				else if (c == Syntax.EndFunction)
				{
					tokens.Enqueue(new Token.EndFunction() { Source = source });
				}
				else if (c == Syntax.Delimiter)
				{
					tokens.Enqueue(new Token.Delimiter() { Source = source });
				}
				else if (c == Syntax.KeyDelimiter)
				{
					tokens.Enqueue(new Token.KeyDelimiter() { Source = source });
				}
				else if (c == Syntax.Assign)
				{
					tokens.Enqueue(new Token.Assign() { Source = source });
				}
				else if (c == Syntax.MacroPrefix)
				{
					tokens.Enqueue(new Token.MacroPrefix() { Source = source });
				}
				else if (c == Syntax.VariablePrefix)
                {
                    tokens.Enqueue(new Token.VariablePrefix() { Source = source });
                }
                else if (c == ' ' || c == '\n' || c == '\r' || c == '\t')
				{
					// Whitespace
				}
				else if (c == Syntax.Comment)
				{
					ReadToEOL(chars);
				}
				else if (c == '"')
				{
					tokens.Enqueue(new Token.String() { Value = ReadString(chars), Source = source });
				}
				else if (char.IsDigit(c) || c == '+' || c == '-')
				{
					tokens.Enqueue(new Token.Number() { Value = double.Parse(c + ReadDigits(chars), CultureInfo.InvariantCulture), Source = source });
				}
				else
				{
					var word = c + ReadLettersOrDigits(chars);
					if (word == "true")
					{
						tokens.Enqueue(new Token.Number() { Value = 1, Source = source });
					}
					else if (word == "false")
					{
						tokens.Enqueue(new Token.Number() { Value = 0, Source = source });
					}
					else
					{
						tokens.Enqueue(new Token.Identifier() { Name = word, Source = source });
					}
				}
			}
		}

		private static string ReadDigits(SourceQueue characters)
		{
			var buffer = new StringBuilder();
			int separatorsBudget = 1;
			while (characters.Any() && (char.IsDigit(characters.Peek()) || ((separatorsBudget > 0) && (characters.Peek() == '.'))))
			{
				var c = characters.Dequeue();
				buffer.Append(c);
				if (c == '.') separatorsBudget -= 1;
			}
			return buffer.ToString();
		}

		private static string ReadLettersOrDigits(SourceQueue characters)
		{
			var buffer = new StringBuilder();
			while (characters.Any() && (char.IsLetterOrDigit(characters.Peek()) || (characters.Peek() == '_')))
			{
				buffer.Append(characters.Dequeue());
			}
			return buffer.ToString();
		}

		private static string ReadString(SourceQueue characters)
		{
			StringBuilder buffer = new StringBuilder();
			bool escaped = false;
			while (characters.Any())
			{
				var c = characters.Dequeue();
				if (c == '"' && !escaped)
				{
					break;
				}
				if (c == '\\' && !escaped)
				{
					escaped = true;
				}
				else if (c == 'n' && escaped)
				{
					buffer.Append('\n');
					escaped = false;
				}
				else
				{
					escaped = false;
					buffer.Append(c);
				}
			}
			return buffer.ToString();
		}

		private static void ReadToEOL(SourceQueue characters)
		{
			while (characters.Any())
			{
				var c = characters.Dequeue();
				if (c == '\r' || c == '\n')
				{
					return;
				}
			}
		}
	}
}
