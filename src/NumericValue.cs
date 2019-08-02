using System.Text;

namespace RPGScript
{
	public class NumericValue : Value
	{
		private readonly double _value;

		public NumericValue(int value)
		{
			_value = value;
		}

		public NumericValue(float value)
		{
			_value = value;
		}

		public NumericValue(double value)
		{
			_value = value;
		}

		public NumericValue(bool value)
		{
			_value = value ? 1 : 0;
		}

		public static implicit operator int(NumericValue v)
		{
			return (int)v._value;
		}

		public static implicit operator float(NumericValue v)
		{
			return (float)v._value;
		}

		public static implicit operator double(NumericValue v)
		{
			return v._value;
		}

		public static implicit operator bool(NumericValue v)
		{
			return (v._value > 0);
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
			return ((other is NumericValue) && (((NumericValue)other)._value == _value));
		}
	}
}
