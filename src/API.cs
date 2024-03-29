﻿using System;
using System.Collections.Generic;

namespace RPGScript
{
    public delegate Value UserExpression(List arguments, Runtime runtime);
    public delegate IEnumerable<object> UserCommand(List args, Runtime runtime);

    public class API
	{
		private readonly Dictionary<string, UserCommand> _commands = new Dictionary<string, UserCommand>();
        private readonly Dictionary<string, UserExpression> _expressions = new Dictionary<string, UserExpression>();

		private static readonly API FrameworkAPI = new API();
		static API()
		{
			FrameworkAPI.RegisterStaticClass(typeof(Framework));
		}

        public API()
        {
			Append(FrameworkAPI);
        }

		public void RegisterStaticClass(Type type)
		{
			var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			foreach (var method in methods)
			{
				if (Delegate.CreateDelegate(typeof(UserCommand), method, false) is UserCommand cmd)
                {
                    RegisterCommand(method.Name, cmd);
                }
                else if (Delegate.CreateDelegate(typeof(UserExpression), method, false) is UserExpression exp)
                {
                    RegisterExpression(method.Name, exp);
                }
			}
		}

        public void RegisterCommand(string name, UserCommand cmd)
		{
			_commands[name] = cmd;
		}

        public void RegisterExpression(string name, UserExpression exp)
        {
            _expressions[name] = exp;
        }

        public UserCommand GetCommand(string name)
		{
			if (_commands.ContainsKey(name))
			{
				return _commands[name];
			}
            throw new RuntimeScriptException($"Unknown command: {name}");
		}

        public UserExpression GetExpression(string name)
        {
            if (_expressions.ContainsKey(name))
            {
                return _expressions[name];
            }
            throw new RuntimeScriptException($"Unknown expression: {name}");
        }

        public void Append(API api)
		{
			if (api == null) return;

			foreach (var kvp in api._commands)
			{
				_commands[kvp.Key] = kvp.Value;
			}
            foreach (var kvp in api._expressions)
            {
                _expressions[kvp.Key] = kvp.Value;
            }
        }
    }
}
