using System.Collections.Generic;
using System.Text;

namespace RPGScript
{
	public class Function : Value
	{
		struct Command
		{
			public string Identifier;
			public List Args;
			public override string ToString()
			{
				return Identifier;
			}
		}
		private readonly List<Command> _commands = new List<Command>();

		public IEnumerable<object> Execute(Runtime runtime)
		{
			foreach (var command in _commands)
			{
				var cmd = runtime.API.GetCommand(command.Identifier);
				foreach (var op in cmd(command.Args, runtime))
				{
					yield return op;
				}
			}
		}

		internal static Function Parse(Queue<Token> tokens)
		{
			var function = new Function();
			tokens.Dequeue<Token.StartFunction>();
			while (!tokens.CheckNext<Token.EndFunction>())
			{
				var cmd = tokens.Dequeue<Token.Identifier>();
				var args = Parser.ParseList(tokens, null);
				function._commands.Add(new Command() { Identifier = cmd.Name, Args = args });
			}
			tokens.Dequeue<Token.EndFunction>();
			return function;
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(Syntax.StartFunction);
			foreach (var op in _commands)
			{
				sb.Append(op.Identifier);
				op.Args.Write(sb);
				sb.AppendLine();
			}
			sb.Append(Syntax.EndFunction);
		}

		public override bool IsEqual(Value other)
		{
			return (this == other);
		}

		public static Function Load(string filename)
		{
			return Load(filename, new DefaultSourceProvider());
		}

		public static Function Load(string filename, string fullScript)
		{
			return Load(filename, new InMemorySourceProvider(fullScript));
		}

		public static Function Load(string filename, ISourceProvider provider)
		{
			return Parse(Lexer.Parse(filename, provider));
		}
	}
}
