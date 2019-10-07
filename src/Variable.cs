using System.Linq;
using System.Text;

namespace RPGScript
{
	public class Variable : Value
    {
		private readonly string[] _parts;

		public Variable(string[] parts)
		{
			_parts = parts;
		}

		public override bool IsEqual(Value other)
		{
			if (other is Variable otherv)
			{
				if (_parts.Length == otherv._parts.Length)
				{
					for (int i = 0; i < _parts.Length; ++i)
					{
						if (_parts[i] != otherv._parts[i]) return false;
					}
					return true;
				}
			}
			return false;
		}

        public override Value Evaluate(Runtime runtime) => Get(runtime.Globals).Evaluate(runtime);

        public override void Write(StringBuilder sb)
		{
			sb.Append(string.Join(".", _parts));
		}

		public void Set(Table globals, int value)
		{
			Set(globals, (Value)new NumericValue(value));
		}

		public void Set(Table globals, string value)
		{
			Set(globals, (Value)new StringValue(value));
		}

		public void Set(Table globals, bool value)
		{
			Set(globals, (Value)new NumericValue(value));
		}

		public void Set(Table globals, Value value)
		{
			var table = globals;
			for (int i = 0; i < _parts.Length - 1; ++i)
			{
				table = globals.GetOrCreateTable(_parts[i]);
			}
			var key = _parts.Last();
			table.Set(key, value);
		}

		public Value Get(Table globals)
		{
			var table = globals;
			for (int i = 0; i < _parts.Length - 1; ++i)
			{
				table = globals.GetOrCreateTable(_parts[i]);
			}
			var key = _parts.Last();
			return table.GetValue(key);
		}

		public int GetInt(Table globals, int def)
		{
			return (Get(globals) as NumericValue) ?? def;
		}

		public string GetString(Table globals, string def)
		{
			return (Get(globals) as StringValue) ?? def;
		}
	}
}
