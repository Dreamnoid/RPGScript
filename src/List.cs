using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RPGScript
{
	public class List : Value, IEnumerable<Value>
	{
		private readonly List<Value> _values = new List<Value>();

		public int Length => _values.Count;

		#region Get

		public string GetString(int index)
		{
			var value = GetValue(index) as StringValue;
			return value;
		}

		public Table GetTable(int index) => GetValue(index) as Table;

		public List GetList(int index) => GetValue(index) as List;

		public int GetInt(int index, int def)
		{
			return (GetValue(index) is NumericValue value) ? value : def;
		}

		public float GetFloat(int index, float def)
		{
			return (GetValue(index) is NumericValue value) ? value : def;
		}

		public double GetDouble(int index, double def)
		{
			return (GetValue(index) is NumericValue value) ? value : def;
		}

		public bool GetBool(int index, bool def)
		{
			return (GetValue(index) is NumericValue value) ? value : def;
		}

        public Function GetFunction(int index) => GetValue(index) as Function;

        public Value GetValue(int index)
		{
			if (index >= 0 && index < _values.Count)
			{
				return _values[index];
			}
			return null;
		}

		#endregion

		public void Add(Value value)
		{
			_values.Add(value);
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(Syntax.StartList);
			for(int i = 0; i < _values.Count; ++i)
			{
				_values[i].Write(sb);
				if (i < (_values.Count - 1))
				{
					sb.Append(Syntax.Delimiter);
				}
			}
			sb.Append(Syntax.EndList);
		}

        public override bool IsEqual(Value other) => (this == other);

        public List<Value>.Enumerator GetEnumerator() =>  _values.GetEnumerator();

        IEnumerator<Value> IEnumerable<Value>.GetEnumerator() => _values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

        public override Value Evaluate(Runtime runtime)
        {
            var list = new List();
            foreach (var item in this)
            {
                list.Add(item.Evaluate(runtime));
            }
            return list;
        }
    }
}
