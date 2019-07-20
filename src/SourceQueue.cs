using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGScript
{
	internal class SourceQueue
	{
		private readonly string _sourceCode;
		private int _cursor = 0;

		public SourceQueue(string sourceCode)
		{
			_sourceCode = sourceCode;
		}

		public bool Any()
		{
			return (_cursor < _sourceCode.Length);
		}

		public char Dequeue()
		{
			return _sourceCode[_cursor++];
		}

		public char Peek()
		{
			return _sourceCode[_cursor];
		}
	}
}
