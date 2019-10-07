using System.Text;

namespace RPGScript
{
    public class Expression : Value
    {
        public readonly string Identifier;

        public readonly List Arguments = new List();

        public Expression(string identifier)
        {
            Identifier = identifier;
        }

        public override void Write(StringBuilder sb)
        {
            sb.Append(Identifier);
            sb.Append('(');
            for (int i = 0; i < Arguments.Length; ++i)
            {
                Arguments.GetValue(i).Write(sb);
                if (i < (Arguments.Length - 1))
                    sb.Append(',');
            }
            sb.Append(')');
        }

        public override bool IsEqual(Value other) => false;

        public override Value Evaluate(Runtime runtime)
        {
            return runtime.API.GetExpression(Identifier)(Arguments, runtime).Evaluate(runtime);
        }
    }
}
