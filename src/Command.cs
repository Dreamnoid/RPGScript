using System.Collections.Generic;

namespace RPGScript
{
	public interface ICommand
	{
		void Evaluate(List args, FunctionContext ctx);
		IEnumerable<object> Execute(List args, FunctionContext ctx);
	}

	public class Command : ICommand
	{
		public delegate void Callback(List args, FunctionContext ctx);

		private readonly Callback _action;

		private readonly static object[] _noYield = new object[] { };

		public Command(Callback action)
		{
			_action = action;
		}

		public void Evaluate(List args, FunctionContext ctx)
		{
			_action(args, ctx);
		}

		public IEnumerable<object> Execute(List args, FunctionContext ctx)
		{
			_action(args, ctx);
			return _noYield;
		}
	}

	public class YieldCommand<T> : ICommand
	{
		public delegate IEnumerable<T> Callback(List args, FunctionContext ctx);

		private readonly Callback _action;

		public YieldCommand(Callback action)
		{
			_action = action;
		}

		public void Evaluate(List args, FunctionContext ctx)
		{
			throw new RuntimeScriptException("Yieldable commands can't be used in evaluations");
		}

		public IEnumerable<object> Execute(List args, FunctionContext ctx)
		{
			foreach (var op in _action(args, ctx))
			{
				yield return op;
			}
		}
	}
}
