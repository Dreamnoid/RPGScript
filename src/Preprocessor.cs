using System.Collections.Generic;

namespace RPGScript
{
	public delegate Value UserMacro(List args, Preprocessor preprocessor);

	public class Preprocessor
	{
		private readonly Dictionary<string, UserMacro> _macros = new Dictionary<string, UserMacro>();

		public Preprocessor()
		{
			Register("Include", Include);
		}

		public void Register(string name, UserMacro macro)
		{
			_macros[name] = macro;
		}

		public UserMacro GetMacro(string name, Source source)
		{
			if (_macros.ContainsKey(name))
			{
				return _macros[name];
			}
			throw new ParserScriptException($"Unknown preprocessor macro: {name}", source);
		}

		public static Value Include(List args, Preprocessor preprocessor)
		{
			return Value.Load(args.GetValue(0).AsString(), preprocessor);
		}
	}
}
