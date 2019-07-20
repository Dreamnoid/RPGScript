# RPGScript
A very simple scripting language, written in C# and embeddable in .NET applications (Framework or Core).
Be advised: right now, it's only the product of an afternoon of work. RPGScript is probably not ready for production.

## Uses
RPGScript was written with RPG and adventure games in mind. Like most scripting languages for games, it serves two purposes:
* Defining data
* Defining simple behavior

Its philosophy is very similar to Lua, as I started writing it as a simpler, flexible and fully-managed replacement for it.

## Data types
The following data types are supported:
* Integers (bools are treated as integers)
* Strings
* Lists (a simple collection of values)
* Tables (a key/value dictionary. Keys are strings, values can be anything in this list)
* Functions

## Tables
Tables are the bread-and-butter of RPGScript. They are declared between the `[` and `]` characters, with a `key = value` syntax:
``` C
[
	Alex = 
	[
		Name = "Alex",
		Sprite = 0,
		Face = 0,
		Stats =
		[
			HP = 200,
			MP = 30
		]
	]
]
```
(If you are familiar with Lua or JSON, you should be right at home with RPGScript's tables.)

Tables can be parsed from a .NET string, modified and then written back to one. It makes save files very easy to implement.

Ususally, the host application will load a table, and look for variables and functions in it. In this regard, it can be used as a kind of module.

## Lists
Lists are pretty much like tables, without the keys. They are declared between `(` and `)`:
``` C
Party = ( [ Name = "Alex" , HP = 100, MP = 20 ] , [ Name = "Brian" , HP = 150, MP = 5 ] )
```

## Functions
Functions are first-class citizens in RPGScript. They are declared between `{` and `}`:
``` C
OnStart = 
{
	Unfade()
}
```
Functions are essentially a list of command calls. Commands are .NET functions defined and exposed by the host application.

Every function call can use its own specific API and global tables, similar to the `setfenv()` feature in Lua.

There's two ways to call a function:
* Evaluation
* Execution

Evaluation calls every commands and check the `BoolFlag` at the end of the execution. Exemple:
``` C
Condition = { Equals(Temp.Choice, 0) }
```
(There's no stack in RPGScript (yet), so the `BoolFlag` acts as a kind of register.)

On the other hand, execution does not return a result.
It simply calls the commands sequentially, but allow yielding from the C# code.
Exemple:
``` C
OnTalk = 
{
	ShowDB()
	Print("Hello, do you like RPGScript?")
	Choice("Yes","No")
	If({Equals(Temp.Choice, 0)},
	{
		Print("Yes!!")
	},
	{
		Print("Noo...")
	})
	HideDB()
}
```
Here, the `ShowDB`, `HideDB`, `Print` and `Choice` commands yield while the host application animates the dialog box and wait for user input.

As you can see from the previous sample, RPGScript does not yet support branching, or loops. It may come later.
But right now, you can write a simple `If` command this way:
``` C#
public static IEnumerable<object> If(List args, FunctionContext ctx)
{
	var condition = (Function)args.GetValue(0);
	var trueBlock = args.GetValue(1) as Function;
	var falseBlock = args.GetValue(2) as Function;
	if (condition.Evaluate(ctx))
	{
		if (trueBlock != null)
		{
			foreach (var op in trueBlock.Execute(ctx))
			{
				yield return (op;
			}
		}
	}
	else
	{
		if (falseBlock != null)
		{
			foreach (var op in falseBlock.Execute(ctx))
			{
				yield return op;
			}
		}
	}
}
```

## Variables
Variables exist in RPGScript, but are subject to change, so I won't document them yet.

## Comments
Only single-line comments are featured. Starting with a `'` character, the rest of the line will be commented out.