namespace RPGScript
{
	internal abstract class Token
	{
		public Source Source;

		public class StartTable : Token
		{
			public override string ToString() => Syntax.StartTable.ToString();
		}

		public class EndTable : Token
        {
            public override string ToString() => Syntax.EndTable.ToString();
		}

		public class StartList : Token
        {
            public override string ToString() => Syntax.StartList.ToString();
        }

		public class EndList : Token
		{
			public override string ToString() => Syntax.EndList.ToString();
		}

		public class StartFunction : Token
		{
			public override string ToString() => Syntax.StartFunction.ToString();
		}

		public class EndFunction : Token
		{
			public override string ToString() => Syntax.EndFunction.ToString();
		}

		public class Delimiter : Token
		{
			public override string ToString() => Syntax.Delimiter.ToString();
		}

		public class Assign : Token
		{
			public override string ToString() => Syntax.Assign.ToString();
		}

		public class Identifier : Token
		{
			public string Name;
            public override string ToString() => Name;
        }

		public class KeyDelimiter : Token
		{
			public override string ToString() => Syntax.KeyDelimiter.ToString();
		}

		public class Number : Token
		{
			public double Value;
			public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		public class String : Token
		{
			public string Value;
            public override string ToString() => Value;
        }

        public class VariablePrefix : Token
        {
            public override string ToString() => Syntax.VariablePrefix.ToString();
        }

		public class MacroPrefix : Token
		{
			public override string ToString() => Syntax.MacroPrefix.ToString();
		}
	}
}
