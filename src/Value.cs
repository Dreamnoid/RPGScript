using System.Text;

namespace RPGScript
{
	public abstract class Value
	{
		public abstract void Write(StringBuilder sb);
		public abstract bool IsEqual(Value other);
	}
}
