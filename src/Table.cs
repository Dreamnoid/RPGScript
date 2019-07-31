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
			if (value != null)
			{
				_values[key] = new StringValue(value);
			}
			else if (_values.ContainsKey(key))
			{
				_values.Remove(key);
			}
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
			sb.Append(Syntax.StartTable);
			int i = 0;
			foreach (var kvp in _values)
			{
				sb.Append(kvp.Key);
				sb.Append(Syntax.Assign);
				kvp.Value.Write(sb);
				if (i < (_values.Count - 1))
				{
					sb.Append(Syntax.Delimiter);
				}
				i += 1;
			}
			sb.Append(Syntax.EndTable);
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

		public static Table Load(string filename)
		{
			return Load(filename, new DefaultSourceProvider());
		}

		public static Table Load(string filename, string fullScript)
		{
			return Load(filename, new InMemorySourceProvider(fullScript));
		}

		public static Table Load(string filename, ISourceProvider provider)
		{
			return Parser.ParseTable(Lexer.Parse(filename, provider), provider);
		}

		public void Save(string filename)
		{
			var sb = new StringBuilder();
			Write(sb);
			System.IO.File.WriteAllText(filename, sb.ToString());
		}
	}
}
