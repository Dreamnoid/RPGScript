using System.IO;

namespace RPGScript
{
	public interface ISourceProvider
	{
		string GetSource(string filename);
	}

	public struct DefaultSourceProvider : ISourceProvider
	{
		public string GetSource(string filename)
		{
			return File.ReadAllText(filename);
		}
	}

	internal struct InMemorySourceProvider : ISourceProvider
	{
		private readonly string _source;
		public InMemorySourceProvider(string source) : this()
		{
			_source = source;
		}

		public string GetSource(string filename)
		{
			return _source;
		}
	}

}
