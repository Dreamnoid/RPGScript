using System.Collections.Generic;

namespace RPGScript
{
	internal static class Parser
	{
		public static Value DequeueValue(this Queue<Token> tokens, ISourceProvider provider)
		{
			if (tokens.CheckNext<Token.Number>())
			{
				var value = tokens.Dequeue<Token.Number>();
				return new NumericValue(value.Value);
			}
			else if (tokens.CheckNext<Token.String>())
			{
				var value = tokens.Dequeue<Token.String>();
				return new StringValue(value.Value);
			}
			else if (tokens.CheckNext<Token.StartTable>())
			{
				return ParseTable(tokens, provider);
			}
			else if (tokens.CheckNext<Token.StartList>())
			{
				return ParseList(tokens, provider);
			}
			else if (tokens.CheckNext<Token.StartFunction>())
			{
				var value = Function.Parse(tokens);
				return value;
			}
			else if (tokens.CheckNext<Token.VariablePrefix>())
            {
                tokens.Dequeue<Token.VariablePrefix>();
                var parts = new List<string>();
                parts.Add(tokens.Dequeue<Token.Identifier>().Name);
                while (tokens.CheckNext<Token.KeyDelimiter>())
                {
                    tokens.Dequeue<Token.KeyDelimiter>();
                    parts.Add(tokens.Dequeue<Token.Identifier>().Name);
                }
                return new Variable(parts.ToArray());
            }
			else if (tokens.CheckNext<Token.At>())
			{
				if (provider == null)
				{
					throw new ParserScriptException("Trying to import a file when no source provider was specified", tokens.Peek().Source);
				}
				tokens.Dequeue<Token.At>();
				var value = tokens.Dequeue<Token.String>();
				return Table.Load(value.Value, provider);
			}
            else if (tokens.CheckNext<Token.Identifier>())
            {
                return ParseExpression(tokens, provider);
            }
			else
			{
				var token = tokens.Peek();
                throw new ParserScriptException($"{token.GetType().Name} is not a value", token.Source);
            }
		}

        private static Expression ParseExpression(Queue<Token> tokens, ISourceProvider provider)
        {
            var identifier = tokens.Dequeue<Token.Identifier>();
            var expression = new Expression(identifier.Name);
            tokens.Dequeue<Token.StartList>();
            while (!tokens.CheckNext<Token.EndList>())
            {
                expression.Arguments.Add(tokens.DequeueValue(provider));
				if (!tokens.CheckNext<Token.Delimiter>())
				{
					break;
				}
				else
				{
					tokens.Dequeue<Token.Delimiter>();
				}
			}
            tokens.Dequeue<Token.EndList>();
            return expression;
        }

        public static Table ParseTable(Queue<Token> tokens, ISourceProvider provider)
		{
			var table = new Table();
			tokens.Dequeue<Token.StartTable>();
			while (!tokens.CheckNext<Token.EndTable>())
			{
				var key = tokens.Dequeue<Token.Identifier>();
				tokens.Dequeue<Token.Assign>();
				table.Set(key.Name, tokens.DequeueValue(provider));
				if (!tokens.CheckNext<Token.Delimiter>())
				{
					break;
				}
				else
				{
					tokens.Dequeue<Token.Delimiter>();
				}
			}
			tokens.Dequeue<Token.EndTable>();
			return table;
		}

		public static List ParseList(Queue<Token> tokens, ISourceProvider provider)
		{
			var list = new List();
			tokens.Dequeue<Token.StartList>();
			while (!tokens.CheckNext<Token.EndList>())
			{
				list.Add(tokens.DequeueValue(provider));
				if (!tokens.CheckNext<Token.Delimiter>())
				{
					break;
				}
				else
				{
					tokens.Dequeue<Token.Delimiter>();
				}
			}
			tokens.Dequeue<Token.EndList>();
			return list;
		}

		public static TToken Dequeue<TToken>(this Queue<Token> tokens) where TToken : Token
		{
			if (tokens.Count > 0)
			{
				var token = tokens.Dequeue();
				if (!(token is TToken))
				{
					throw new ParserScriptException(string.Format("Expected {0}, received '{1}'", typeof(TToken).Name, token.ToString()), token.Source);
				}
				return ((TToken)token);
			}
			else
			{
				throw new ParserScriptException(string.Format("Expected {0}, received EOF", typeof(TToken).Name), new Source());
			}
		}

		public static bool CheckNext<TToken>(this Queue<Token> tokens) where TToken : Token
		{
			var token = tokens.Peek();
			return (token is TToken);
		}
	}
}
