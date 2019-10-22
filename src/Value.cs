using System.IO;
using System.Text;

namespace RPGScript
{
	public abstract class Value
	{
		public abstract void Write(StringBuilder sb);

		public abstract bool IsEqual(Value other);

        public virtual Value Evaluate(Runtime runtime) => this;

        public virtual bool AsBool() => throw new RuntimeScriptException($"{GetType().Name} cannot be evaluated as a boolean value");

        public virtual double AsNumeric() => throw new RuntimeScriptException($"{GetType().Name} cannot be evaluated as a numeric value");

        public virtual string AsString() => throw new RuntimeScriptException($"{GetType().Name} cannot be evaluated as a string value");

        public static Value Load(string filename, Preprocessor preprocessor)
        {
            return Load(filename, File.ReadAllText(filename), preprocessor);
        }

        public static Value Load(string filename, string fullScript, Preprocessor preprocessor)
        {
            return Lexer.Parse(fullScript, filename).DequeueValue(preprocessor);
        }

        public void Save(string filename)
        {
            var sb = new StringBuilder();
            Write(sb);
            File.WriteAllText(filename, sb.ToString());
        }
    }
}
