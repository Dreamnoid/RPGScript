using System.Text;

namespace RPGScript
{
	public class IntValue : Value
	{
		private readonly int _value;

		public IntValue(int value)
		{
			_value = value;
		}

		public IntValue(bool value)
		{
			_value = value ? 1 : 0;
		}

		public static implicit operator int(IntValue v)
		{
			return v._value;
		}

		public static implicit operator bool(IntValue v)
		{
			return (v._value > 0);
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(_value);
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		public override bool IsEqual(Value other)
		{
			return ((other is IntValue) && (((IntValue)other)._value == _value));
		}
	}
}
