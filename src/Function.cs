using System.Collections.Generic;
using System.Text;

namespace RPGScript
{
	public class FunctionContext
	{
		public bool BoolFlag;
		public readonly Table Globals;
		public readonly API API;
		public FunctionContext(Table globals, API api)
		{
			Globals = globals ?? new Table();
			API = api;
		}
	}

	public class Function : Value
	{
		struct Op
		{
			public string Cmd;
			public List Args;
			public override string ToString()
			{
				return Cmd;
			}
		}
		private readonly List<Op> _ops = new List<Op>();

		public bool Evaluate(FunctionContext ctx)
		{
			foreach (var op in _ops)
			{
				var cmd = ctx.API.GetCommand(op.Cmd);
				cmd.Evaluate(op.Args, ctx);
			}
			return ctx.BoolFlag;
		}

		public IEnumerable<object> Execute(FunctionContext ctx)
		{
			foreach (var op in _ops)
			{
				var cmd = ctx.API.GetCommand(op.Cmd);
				foreach (var o in cmd.Execute(op.Args, ctx))
				{
					yield return o;
				}
			}
		}

		internal static Function Parse(Queue<Token> tokens)
		{
			var function = new Function();
			tokens.Dequeue<Token.StartFunction>();
			while (!tokens.CheckNext<Token.EndFunction>())
			{
				var cmd = tokens.Dequeue<Token.Key>();
				var args = Parser.ParseList(tokens, null);
				function._ops.Add(new Op() { Cmd = cmd.Name, Args = args });
			}
			tokens.Dequeue<Token.EndFunction>();
			return function;
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(Syntax.StartFunction);
			foreach (var op in _ops)
			{
				sb.Append(op.Cmd);
				op.Args.Write(sb);
				sb.AppendLine();
			}
			sb.Append(Syntax.EndFunction);
		}

		public override bool IsEqual(Value other)
		{
			return (this == other);
		}
	}
}
