using System.Text;

namespace RPGScript
{
	public class DoubleValue : Value
	{
		private readonly double _value;

		public DoubleValue(double value)
		{
			_value = value;
		}

		public DoubleValue(float value)
		{
			_value = value;
		}

		public static implicit operator float(DoubleValue v)
		{
			return (float)v._value;
		}

		public static implicit operator double(DoubleValue v)
		{
			return v._value;
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(_value);
		}

		public override string ToString()
		{
			return _value.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		public override bool IsEqual(Value other)
		{
			return ((other is DoubleValue) && (((DoubleValue)other)._value == _value));
		}
	}
}
