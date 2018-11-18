// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly : System.Diagnostics.CodeAnalysis.SuppressMessage ("Major Code Smell", "S2436:Classes and methods should not have too many generic parameters", Justification = "extra parameters are only to enforce rules on other generic types, no code acting on generica parametes", Scope = "type", Target = "~T:Copious.Application.CommandHandler`3")]
[assembly : System.Diagnostics.CodeAnalysis.SuppressMessage ("Major Code Smell", "S2436:Classes and methods should not have too many generic parameters", Justification = "<Pending>", Scope = "type", Target = "~T:Copious.Application.CrudCommandHandler`3")]