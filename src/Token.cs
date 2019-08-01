namespace RPGScript
{
	internal abstract class Token
	{
		public Source Source;

		public class StartTable : Token
		{
			public override string ToString()
			{
				return Syntax.StartTable.ToString();
			}
		}

		public class EndTable : Token
		{
			public override string ToString()
			{
				return Syntax.EndTable.ToString();
			}
		}

		public class StartList : Token
		{
			public override string ToString()
			{
				return Syntax.StartList.ToString();
			}
		}

		public class EndList : Token
		{
			public override string ToString()
			{
				return Syntax.EndList.ToString();
			}
		}

		public class StartFunction : Token
		{
			public override string ToString()
			{
				return Syntax.StartFunction.ToString();
			}
		}

		public class EndFunction : Token
		{
			public override string ToString()
			{
				return Syntax.EndFunction.ToString();
			}
		}

		public class Delimiter : Token
		{
			public override string ToString()
			{
				return Syntax.Delimiter.ToString();
			}
		}

		public class Assign : Token
		{
			public override string ToString()
			{
				return Syntax.Assign.ToString();
			}
		}

		public class Key : Token
		{
			public string Name;
			public override string ToString()
			{
				return Name;
			}
		}

		public class KeyDelimiter : Token
		{
			public override string ToString()
			{
				return Syntax.KeyDelimiter.ToString();
			}
		}

		public class Int : Token
		{
			public int Value;
			public override string ToString()
			{
				return Value.ToString();
			}
		}

		public class Double : Token
		{
			public double Value;
			public override string ToString()
			{
				return Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}
		}

		public class String : Token
		{
			public string Value;
			public override string ToString()
			{
				return Value;
			}
		}

		public class At : Token
		{
			public override string ToString()
			{
				return Syntax.At.ToString();
			}
		}
	}
}
