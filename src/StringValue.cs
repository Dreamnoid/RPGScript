using System.Text;

namespace RPGScript
{
	public class StringValue : Value
	{
		private readonly string _value;

		public StringValue(string value)
		{
			_value = value;
		}

		public static implicit operator string(StringValue v)
		{
			return v?._value;
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append("\"").Append(_value.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append("\"");
		}

        public override string ToString() => _value;

		public override bool IsEqual(Value other)
		{
			return ((other is StringValue value) && (value._value == _value));
		}

        public override string AsString() =>_value;
    }
}
