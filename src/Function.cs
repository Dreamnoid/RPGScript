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

        public void AddCommand(string identifier, List args)
        {
            _commands.Add(new Command() { Identifier = identifier, Args = args });
        }
    }
}
