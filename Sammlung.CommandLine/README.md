# Sammlung.CommandLine

Sammlung.CommandLine is a rich-featured command line parsing tool. 

## Concept
The core concept
of the parser forms the idea, that each argument of a command will be processed
by a pipeline. We're using Sammlung.Pipes for this matter.

Each stage in the pipeline provides a transformation or validation mechanism.
The output of a previous stage flows into the next stage which either succeeds
or throws a well-known exception signalling that the value could not be processed.

### Example

Let's jump right into the matter. We will show you the complete example and then 
we take it apart.

```C#
var flag = _pipeFactory.BoolPipe(CultureInfo.InvariantCulture)
        .WithMultiInput()
        .WithUniqueOutput()
        .AsValueHandler((Parameters p, bool v) => p.Flag = v)
        .AsFlag("-f", "--force");

var stringOpt = _pipeFactory
        .StringPipe()
        .WithMultiInput()
        .WithUniqueOutput()
        .AsValueHandler((Parameters p, string v) => p.Hello = v)
        .AsOption("-s", "--string");

var argument = _pipeFactory.StringPipe().WithMultiInput().WithUniqueOutput()
        .AsValueHandler((Parameters p, string v) => p.World = v).AsArgument();

var command = new Command<SubParameters>(new [] {"first_command"}, () => new SubParameters());
            
return RootCommand.Create<Parameters>()
    .SetApplicationName("HalloWelt")
    .AddFlag(flag)
    .AddOption(stringOpt)
    .AddArgument(argument)
    .AddCommand(command);
```

This is a usual CLI definition written with this library.

```C#
var flag = _pipeFactory.BoolPipe(CultureInfo.InvariantCulture)
        .WithMultiInput()
        .WithUniqueOutput()
        .AsValueHandler((Parameters p, bool v) => p.Flag = v)
        .AsFlag("-f", "--force");
```

The flag is stitched together. As follows:
```C#
_pipeFactory.BoolPipe(CultureInfo.InvariantCulture)
```
Creates a pipe which does the following mapping `string -> bool`.
```C#
.WithMultiInput()
```
Feeds each element in an `IEnumerable<string>` into the previous function.
Thus the resulting pipe is a mapping like 
`IEnumerable<string> -> IEnumerable<bool>`.
```C#
.WithUniqueOutput()
```
Projects the `IEnumerable<string>` down to a single `bool`.
```C#
.AsValueHandler((Parameters p, bool v) => p.Flag = v)
```
Wraps the pipeline into a `IValueHandler<Parameters>` which ultimately sets the property
on the `Parameters` object of the command.
```C#
.AsFlag("-f", "--force")
```
Wraps the `IValueHandler<Parameters>` into a `Flag<Parameters>` and assigns the
keywords `-f` and `--force` to it.

If the concept of `Sammlung.Pipes` reminds you of `System.Linq` this is no
coincidence it is inspired by it. But pipes are reaching far beyond
Linq, that's because if functions manifested by the pipes are symmetrical
pipes can be executed in the other direction - but that is a whole another story.
For now, it's simply a lazy Linq transformation chain but mostly without the
functional part.

### Parsing rules

Please read the [Best Practices for CLIs](https://clig.dev/) to provide a
marvelous user experience for your CLI. Some design decisions are derived
from there.

#### General
- If there is a string token which coincidentally matches a option keyword
  or a command it can be set in double quotes the be recognized as an argument
  or an option argument.
- If the command is not correct, the error and the help is shown
- The order in which the arguments, options and commands are parsed:
  - Get a new token
  - Check if a command keyword matches
  - If it matches: It's a command.
  - Else: check if a options keyword matches
  - If it matches: It's an option.
  - Else: It's an argument
- The parsing is done by a state-machine.

#### Arguments
- Each instance of an argument is unary by default or has a fixed arity.
- Every argument before the last argument has only one occurrence.
- The last argument is allowed to have unlimited occurrences.
- There are no optional arguments. Optional arguments are options.

#### Options
- Each instance of an option is unary by default or has a fixed arity.
- Options can occur multiple times.
- The number of minimum and maximum occurrences can be configured.
- Options may be optional.

#### Commands
- On each command level there can be only one sub-command level.
- If there are commands at least one is required.

#### Examples

The application has the following syntax:
```
$ MyApplication <ARG1> <ARG2> [<ARG2> ...] [-f|--force] [-v|--verbosity <VERBOSITY>] <COMMAND>

OPTIONS
    -v|--verbosity    Options: [None, Low, Medium, High], Default: None
        Sets the verbosity level of the application.

COMMANDS
    ls    Lists the current directory contents.
    cd    Changes the current directory.
```

Valid calls are:
```
$ MyApplication arg1 arg2 arg3 arg4 -f -v High ls 
$ MyApplication arg1 arg2 -f -v High ls 
$ MyApplication arg1 arg2 -v High ls 
$ MyApplication arg1 arg2 ls 
```

Invalid calls are: