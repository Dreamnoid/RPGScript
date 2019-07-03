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

		public Table GetTable(int index)
		{
			return GetValue(index) as Table;
		}

		public int GetInt(int index, int def)
		{
			return (GetValue(index) is IntValue value) ? value : def;
		}

		public bool GetBool(int index, bool def)
		{
			return (GetValue(index) is IntValue value) ? value : def;
		}

		public Value GetValue(int index)
		{
			if (index >= 0 && index < _values.Count)
			{
				return _values[index];
			}
			return null;
		}

		#endregion

		public static List Parse(Queue<Parser.Token> tokens)
		{
			var list = new List();
			tokens.Dequeue<Parser.StartListToken>();
			while (!tokens.CheckNext<Parser.EndListToken>())
			{
				list._values.Add(tokens.DequeueValue());
				if (!tokens.CheckNext<Parser.DelimiterToken>())
				{
					break;
				}
				else
				{
					tokens.Dequeue<Parser.DelimiterToken>();
				}
			}
			tokens.Dequeue<Parser.EndListToken>();
			return list;
		}

		public override void Write(StringBuilder sb)
		{
			sb.Append(Parser.Syntax.StartList);
			foreach (var value in _values)
			{
				value.Write(sb);
				sb.Append(Parser.Syntax.Delimiter);
			}
			sb.Append(Parser.Syntax.EndList);
		}

		public override bool IsEqual(Value other)
		{
			return (this == other);
		}

		public IEnumerator<Value> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _values.GetEnumerator();
		}
	}
}
