using System;
using System.Collections.Generic;

namespace RPGScript
{
	public class API
	{
		private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

		public static API FromStaticClass(Type type)
		{
			var api = new API();
			var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			foreach (var method in methods)
			{
				if (Delegate.CreateDelegate(typeof(Command.Callback), method, false) is Command.Callback d)
				{
					api._commands[method.Name] = new Command(d);
				}
			}
			return api;
		}

		public static API FromStaticClass<T>(Type type)
		{
			var api = new API();
			var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			foreach (var method in methods)
			{
				if (Delegate.CreateDelegate(typeof(Command.Callback), method, false) is Command.Callback d)
				{
					api._commands[method.Name] = new Command(d);
				}
				else if (Delegate.CreateDelegate(typeof(YieldCommand<T>.Callback), method, false) is YieldCommand<T>.Callback yd)
				{
					api._commands[method.Name] = new YieldCommand<T>(yd);
				}
			}
			return api;
		}

		public void Register(string name, ICommand cmd)
		{
			_commands.Add(name, cmd);
		}

		public ICommand GetCommand(string name)
		{
			if (_commands.ContainsKey(name))
			{
				return _commands[name];
			}
			else
				throw new RuntimeScriptException(string.Format("Unknown command: {0}", name));
		}

		public void Append(API api)
		{
			foreach (var kvp in api._commands)
			{
				_commands[kvp.Key] = kvp.Value;
			}
		}
	}
}
