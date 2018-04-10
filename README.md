# JSDialogueSystem

A prototype of a game dialogue system base on JSON conversation trees.
You can find more using the project [Wiki] (https://github.com/AMDevIT/JSDialogueSystem/wiki)

## Abstract

The goal is to allow a coder to generate a conversation graph that can be scripted using referenced C# methods, instead of runtime parsed methods.
The drawbacks of the use of precompiled scripts theoretically are balanced by the speed of the code against interpreted languages like Javascript or LUA, and the overhead of another virtual machine like V8 running in the game process.

## The idea in a few words

The main idea is that JSON will decribe the graph nodes, while a set of properties references the IDs of the delegates to call on predetermined events, like "OnEnter", "OnLeave", "OnSelected".
Simple conversation flows can be described just using node default handlers and properties, like "navigateTo" will instruct default choice handler to navigate to the specified node.
This will allow "forward-only" conversations models to be easily implemented.

Also, the goal is to obtain platform-agnostic behaviours, using interfaces to describe operations like JSON parsing or localized strings.

## Credits

Assets found mostly on https://opengameart.org/

Portraits in the test application:

* http://justinnichol.blogspot.it
* https://www.kickstarter.com/projects/1318318905/creative-commons-fantasy-portrait-marathon


