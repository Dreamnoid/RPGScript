using System.Collections.Generic;
using System.Linq;

namespace RPGScript
{
    public static  class Framework
    {
        #region "Commands"

        public static IEnumerable<object> If(List args, Runtime runtime)
        {
            var condition = args.GetValue(0);
            var trueBlock = args.GetValue(1) as Function;
            var falseBlock = args.GetValue(2) as Function;
            if (condition.Evaluate(runtime).AsBool())
            {
                if (trueBlock != null)
                {
                    foreach (var op in trueBlock.Execute(runtime))
                    {
                        yield return op;
                    }
                }
            }
            else if (falseBlock != null)
            {
                foreach (var op in falseBlock.Execute(runtime))
                {
                    yield return op;
                }
            }
        }

        public static IEnumerable<object> While(List args, Runtime runtime)
        {
            var condition = args.GetValue(0);
            var block = args.GetValue(1) as Function;
            if (block == null)
                yield break;

            while (condition.Evaluate(runtime).AsBool())
            {
                foreach (var op in block.Execute(runtime))
                {
                    yield return op;
                }
            }
        }

        public static IEnumerable<object> Set(List args, Runtime runtime)
        {
            var variable = (Variable) args.GetValue(0);
            var value = args.GetValue(1).Evaluate(runtime);
            variable.Set(runtime.Globals, value);
            yield break;
        }

        #endregion

        #region "Expressions"

        public static Value Not(List args, Runtime runtime) =>
            new NumericValue(!args.First().Evaluate(runtime).AsBool());

        public static Value And(List args, Runtime runtime) =>
            new NumericValue(args.All(a => a.Evaluate(runtime).AsBool()));

        public static Value Or(List args, Runtime runtime) =>
            new NumericValue(args.Any(a => a.Evaluate(runtime).AsBool()));

        public static Value Add(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Sum());

        public static Value Sub(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Aggregate((a, b) => a - b));

        public static Value Mult(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Aggregate((a, b) => a * b));

        public static Value Div(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Aggregate((a, b) => a / b));

        public static Value Max(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Max());

        public static Value Min(List args, Runtime runtime) =>
            new NumericValue(args.Select(a => a.Evaluate(runtime).AsNumeric()).DefaultIfEmpty(0).Min());

        public static Value Equals(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).IsEqual(args.GetValue(1).Evaluate(runtime)));

        public static Value GT(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).AsNumeric() > args.GetValue(1).Evaluate(runtime).AsNumeric());

        public static Value LT(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).AsNumeric() < args.GetValue(1).Evaluate(runtime).AsNumeric());

        public static Value GTE(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).AsNumeric() >= args.GetValue(1).Evaluate(runtime).AsNumeric());

        public static Value LTE(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).AsNumeric() <= args.GetValue(1).Evaluate(runtime).AsNumeric());

        public static Value StrLen(List args, Runtime runtime) =>
            new NumericValue(args.GetValue(0).Evaluate(runtime).AsString().Length);

        public static Value Concat(List args, Runtime runtime) =>
            new StringValue(args.Select(a => a.Evaluate(runtime).AsString()).Aggregate((a, b) => a + b));

        public static Value Format(List args, Runtime runtime) =>
            new StringValue(string.Format(
                args.GetValue(1).Evaluate(runtime).AsString(),
                args.Skip(1).Select(a => (object) a.Evaluate(runtime)).ToArray()));

        #endregion
    }
}
