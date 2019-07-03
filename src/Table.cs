using System.Collections.Generic;
using System.Text;

namespace RPGScript
{
	public class Table : Value
	{
		private readonly Dictionary<string, Value> _values = new Dictionary<string, Value>();

		public IEnumerable<string> Keys => _values.Keys;

		#region Set

		public void Set(string key, int value)
		{
			_values[key] = new IntValue(value);
		}

		public void Set(string key, string value)
		{
			_values[key] = new StringValue(value);
		}

		public void Set(string key, bool value)
		{
			_values[key] = new IntValue(value);
		}

		public void Set(string key, Value value)
		{
			_values[key] = value;
		}

		#endregion

		#region Get

		public int GetInt(string key, int def)
		{
			return (GetValue(key) is IntValue value) ? value : def;
		}

		public string GetString(string key, string def)
		{
			var value = GetValue(key) as StringValue;
			return value ?? def;
		}

		public bool GetBool(string key, bool def)
		{
			return (GetValue(key) is IntValue value) ? value : def;
		}

		public Table GetTable(string key)
		{
			return GetValue(key) as Table;
		}

		public Table GetOrCreateTable(string key)
		{
			var table = GetValue(key) as Table;
			table = table ?? new Table();
			_values[key] = table;
			return table;
		}

		public Function GetFunction(string key)
		{
			return GetValue(key) as Function;
		}

		public List GetList(string key)
		{
			return GetValue(key) as List;
		}

		public List GetOrCreateList(string key)
		{
			var list = GetValue(key) as List;
			list = list ?? new List();
			_values[key] = list;
			return list;
		}

		public Value GetValue(string key)
		{
			if ((key != null) && _values.ContainsKey(key))
			{
				return _values[key];
			}
			return null;
		}

		#endregion

		public override void Write(StringBuilder sb)
		{
			sb.Append(Parser.Syntax.StartTable);
			foreach (var kvp in _values)
			{
				sb.Append(kvp.Key);
				sb.Append(Parser.Syntax.Assign);
				kvp.Value.Write(sb);
				sb.Append(Parser.Syntax.Delimiter);
			}
			sb.Append(Parser.Syntax.EndTable);
		}

		public override bool IsEqual(Value other)
		{
			return (this == other);
		}

		public void Append(Table table)
		{
			foreach (var kvp in table._values)
			{
				_values[kvp.Key] = kvp.Value;
			}
		}

		#region Parse

		public static Table Parse(string filename, byte[] buffer)
		{
			return Parse(filename, Encoding.UTF8.GetString(buffer));
		}

		public static Table Parse(string filename, string fullScript)
		{
			return Parse(Parser.Parse(filename, fullScript));
		}

		public static Table Parse(Queue<Parser.Token> tokens)
		{
			var table = new Table();
			tokens.Dequeue<Parser.StartTableToken>();
			while (!tokens.CheckNext<Parser.EndTableToken>())
			{
				var key = tokens.Dequeue<Parser.KeyToken>();
				tokens.Dequeue<Parser.AssignToken>();
				table._values[key.Key] = tokens.DequeueValue();
				if (!tokens.CheckNext<Parser.DelimiterToken>())
				{
					break;
				}
				else
				{
					tokens.Dequeue<Parser.DelimiterToken>();
				}
			}
			tokens.Dequeue<Parser.EndTableToken>();
			return table;
		}

		#endregion

	}
}
