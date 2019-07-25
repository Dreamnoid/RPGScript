using System;

namespace RPGScript
{
	public class RuntimeScriptException : Exception
	{
		public RuntimeScriptException(string msg) : base(msg) { }
	}

	public class ParserScriptException : Exception
	{
		public ParserScriptException(string msg, Source source) : base(string.Format("Syntax error: {0} ({1} line {2})", msg, source.File, source.Line)) { }
	}
}
