namespace RPGScript
{
	public struct Source
	{
		public string File;
		public int Line;

		public Source(string file, int line) : this()
		{
			File = file;
			Line = line;
		}
	}
}
