using System;

namespace RPGScript
{
	public class RuntimeScriptException : Exception
	{
		public RuntimeScriptException(string msg) : base(msg) { }
	}

	public class ParserScriptException : Exception
	{
		public ParserScriptException(string msg, Source source) : base($"Syntax error: {msg} ({source.File} line {source.Line})") { }
	}
}
