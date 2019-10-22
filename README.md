# RPGScript
A very simple scripting language, written in C# and embeddable in .NET applications (Framework or Core).
Be advised: RPGScript is a hobby project and is probably not ready for production.

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
* Expressions
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

## Expressions
Expressions are special values that can be evaluated at runtime.

The simplest expression is a 'variable': a path relative to the global table. Variables start with a `$` prefix:
``` C
Msg($Save.Player.Name)
```
At runtime, when the `Msg` command is called, the variable will be replaced by the value yielded by `globalTable.GetTable("Save").GetTable("Player").GetValue("Name")`.

More complex expressions exist, mainly unary and binary expressions:
``` C
PlayerIsAlive = Not(Equals($Save.Player.HP, 0))
```
They can the be evaluated at runtime, from the host application:
``` csharp
if(table.GetExpression("PlayerIsAlive").Evaluate(runtime).AsBool())
{
	// Player is alive!
}
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

Every function call can use its own specific API and global table, similar to the `setfenv()` feature in Lua.

Executing a function simply calls the commands sequentially, but allow yielding from the C# code.
Exemple:
``` C
OnTalk = 
{
	ShowDB()
	Print("Hello, do you like RPGScript?")
	Choice("Yes","No")
	If(Equals($Temp.Choice, 0),
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

RPGScript comes with a few built-in commands and expressions, available in the `Framework` class.

## Comments
Only single-line comments are featured. Starting with a `'` character, the rest of the line will be commented out.

## Preprocessor
It is possible to provide user-defined expressions that are evaluated during the parsing of the document, acting like preprocessor macro. They always start with the `#` prefix:
``` C
[
	Characters = #Include("characters.rpgs")
]
```
`#Include` is a built-in command that will parse a different file and 'paste' the value in-place of the preprocessor macro.
