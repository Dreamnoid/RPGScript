using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGScript
{
	public static class Parser
	{
		public static class Syntax
		{
			public static char StartTable = '[';
			public static char EndTable = ']';
			public static char StartList = '(';
			public static char EndList = ')';
			public static char StartFunction = '{';
			public static char EndFunction = '}';
			public static char Delimiter = ',';
			public static char KeyDelimiter = '.';
			public static char Assign = '=';
			public static char Comment = '\'';
		}

		public abstract class Token
		{
			public Source Source;
		}

		public class StartTableToken : Token
		{
			public override string ToString()
			{
				return Syntax.StartTable.ToString();
			}
		}

		public class EndTableToken : Token
		{
			public override string ToString()
			{
				return Syntax.EndTable.ToString();
			}
		}

		public class StartListToken : Token
		{
			public override string ToString()
			{
				return Syntax.StartList.ToString();
			}
		}

		public class EndListToken : Token
		{
			public override string ToString()
			{
				return Syntax.EndList.ToString();
			}
		}

		public class StartFunctionToken : Token
		{
			public override string ToString()
			{
				return Syntax.StartFunction.ToString();
			}
		}

		public class EndFunctionToken : Token
		{
			public override string ToString()
			{
				return Syntax.EndFunction.ToString();
			}
		}

		public class DelimiterToken : Token
		{
			public override string ToString()
			{
				return Syntax.Delimiter.ToString();
			}
		}

		public class AssignToken : Token
		{
			public override string ToString()
			{
				return Syntax.Assign.ToString();
			}
		}

		public class KeyToken : Token
		{
			public string Key;
			public override string ToString()
			{
				return Key;
			}
		}

		public class KeyDelimiterToken : Token
		{
			public override string ToString()
			{
				return Syntax.KeyDelimiter.ToString();
			}
		}

		public class IntToken : Token
		{
			public int Value;
			public override string ToString()
			{
				return Value.ToString();
			}
		}

		public class StringToken : Token
		{
			public string Value;
			public override string ToString()
			{
				return Value;
			}
		}

		public static Queue<Token> Parse(string filename, string fullScript)
		{
			var lines = fullScript.Split('\n');
			var tokens = new Queue<Token>();
			for (int i = 0; i < lines.Length; ++i)
			{
				Parse(lines[i], new Source(filename, i + 1), tokens);
			}
			return tokens;
		}

		private static void Parse(string line, Source source, Queue<Token> tokens)
		{
			var chars = new SourceQueue(line);
			while (chars.Any())
			{
				var c = chars.Dequeue();
				if (c == Syntax.StartTable)
				{
					tokens.Enqueue(new StartTableToken() { Source = source });
				}
				else if (c == Syntax.EndTable)
				{
					tokens.Enqueue(new EndTableToken() { Source = source });
				}
				else if (c == Syntax.StartList)
				{
					tokens.Enqueue(new StartListToken() { Source = source });
				}
				else if (c == Syntax.EndList)
				{
					tokens.Enqueue(new EndListToken() { Source = source });
				}
				else if (c == Syntax.StartFunction)
				{
					tokens.Enqueue(new StartFunctionToken() { Source = source });
				}
				else if (c == Syntax.EndFunction)
				{
					tokens.Enqueue(new EndFunctionToken() { Source = source });
				}
				else if (c == Syntax.Delimiter)
				{
					tokens.Enqueue(new DelimiterToken() { Source = source });
				}
				else if (c == Syntax.KeyDelimiter)
				{
					tokens.Enqueue(new KeyDelimiterToken() { Source = source });
				}
				else if (c == Syntax.Assign)
				{
					tokens.Enqueue(new AssignToken() { Source = source });
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
					tokens.Enqueue(new StringToken() { Value = ReadString(chars), Source = source });
				}
				else if (char.IsDigit(c) || c == '+' || c == '-')
				{
					tokens.Enqueue(new IntToken() { Value = int.Parse(c + ReadDigits(chars)), Source = source });
				}
				else
				{
					var word = c + ReadLettersOrDigits(chars);
					if (word == "true")
					{
						tokens.Enqueue(new IntToken() { Value = 1, Source = source });
					}
					else if (word == "false")
					{
						tokens.Enqueue(new IntToken() { Value = 0, Source = source });
					}
					else
					{
						tokens.Enqueue(new KeyToken() { Key = word, Source = source });
					}
				}
			}
		}

		public static Value DequeueValue(this Queue<Token> tokens)
		{
			if (tokens.CheckNext<IntToken>())
			{
				var value = tokens.Dequeue<IntToken>();
				return new IntValue(value.Value);
			}
			else if (tokens.CheckNext<StringToken>())
			{
				var value = tokens.Dequeue<StringToken>();
				return new StringValue(value.Value);
			}
			else if (tokens.CheckNext<StartTableToken>())
			{
				var value = Table.Parse(tokens);
				return value;
			}
			else if (tokens.CheckNext<StartListToken>())
			{
				var value = List.Parse(tokens);
				return value;
			}
			else if (tokens.CheckNext<StartFunctionToken>())
			{
				var value = Function.Parse(tokens);
				return value;
			}
			else if (tokens.CheckNext<KeyToken>())
			{
				var parts = new List<string>();
				parts.Add(tokens.Dequeue<KeyToken>().Key);
				while (tokens.CheckNext<KeyDelimiterToken>())
				{
					tokens.Dequeue<KeyDelimiterToken>();
					parts.Add(tokens.Dequeue<KeyToken>().Key);
				}
				return new Variable(parts.ToArray());
			}
			else
			{
				var token = tokens.Peek();
				throw new ParserScriptException(string.Format("{0} is not a value", token.GetType().Name), token.Source);
			}
		}

		private static string ReadDigits(SourceQueue characters)
		{
			var buffer = new StringBuilder();
			while (characters.Any() && char.IsDigit(characters.Peek()))
			{
				buffer.Append(characters.Dequeue());
			}
			return buffer.ToString();
		}

		private static string ReadLettersOrDigits(SourceQueue characters)
		{
			var buffer = new StringBuilder();
			while (characters.Any() && char.IsLetterOrDigit(characters.Peek()))
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

		public static TToken Dequeue<TToken>(this Queue<Token> tokens) where TToken : Token
		{
			var token = tokens.Dequeue();
			if (!(token is TToken))
			{
				throw new ParserScriptException(string.Format("Expected {0}, received '{1}'", typeof(TToken).Name, token.ToString()), token.Source); 
			}
			return ((TToken)token);
		}

		public static bool CheckNext<TToken>(this Queue<Token> tokens) where TToken : Token
		{
			var token = tokens.Peek();
			return (token is TToken);
		}
	}
}
