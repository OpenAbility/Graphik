# ReplaceShade
I can't be bothered to make a proper readme, so have
some quotes.

> I'll just rant about some ideas that I got in the shower for the shader language I've been thinking of implementing, to get them ingrained.
>
> The general idea is to build a JSON file which contains regex statements, and use that to sloppily put together some sort of valid GLSL/HLSL/MSL/PSSL etc.
>
> These JSON files would to some extent steal ideas from TextMate, you define scopes, each scope is selected via regex(possibly procedural regex too), then each scope can have a child scope which selects from the scope data etc etc. The JSON is basically a big scope definition.
>
> Each scope then specifies how to output itself as the target language(e.g glsl), which can be done by regex-style building/group inserts.
>
> In order to make this all work a bit better tho, we'll need to find a way to split and store values, so each scope would need to store a set of flags, e.g "store", and we'll need to specify some basic syntax for how procedural regex should be built.
>
> There are some character sets we shouldn't see very often in regex, but I'll need to take a look around to see what actually works and what doesn't(ah, the pain of frankenprojects).
>
> This concludes the first part of Jimmy's insane ramblings about how he will disassemble and CTRL+R an entire language
> 
-- Jimmy, 24th October 2023 in a discord channel


And for the example syntax:

> Managed to slap together a basic "idea" for now. We'll see how I go about changing this as I implement it lol.
> ```json5
>{
>  "name": "program",
>  "regex": ".*",
>  // This would be concatinated into
>  // using "regexType": "procedural" I'd do some magix to the regex, it is assumed to be "regular" for now :)
>  "flags": [
>    // Yessir
>  ],
>  "children": [
>    {
>      "name": "function",
>      "regex": "function ($[program.types;join;|]$) (\w*)\(\)"
>      "children": [],
>      // Syntax mAAAgiC
>      "output": "$1 $2() {\n$[programLinesOrWhatever]$\n}"
>    }
>  ],
>  "output": ""
>  // TODO: this
>}
> ```
-- Also Jimmy, like 10 minutes later in the same channel
