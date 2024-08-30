# Monogame RPG Engine

## What is this?

I made a game engine and built an RPG on top of it in Java for a course I teach, which can be found here: https://github.com/a-r-t/SER-225-Game-RPG.
That Java game is built using Java Swing, which isn't an actual game framework.
A lot of my inspiration for it came from my past experiences with Monogame, so I figured why not port it over to Monogame itself.
I might use it in the future to make a game...if I ever have free time.
But it is free and open for anyone else to use.

## How to run this project?

Install .NET 8.0 from [here](https://dotnet.microsoft.com/en-us/download) if not already installed.

* [Windows](#for-windows-developers)
* [Mac](#for-mac-developers)

### For Windows Developers

#### Setup

1. Install Visual Studio Community Edition, NOT Visual Studio Code, from [here](https://visualstudio.microsoft.com/vs/) if not already installed.
2. Open the project solution file (`Monogame-RPG-Engine.sln`) in Visual Studio and it should handle everything else from there for you as far as opening up the project, restoring external libraries, etc.

#### Run the project for development

After opening the project successfully, you should be able to run the game and play my fun little cat RPG identical to the Java version. Just make sure the dropdown says 'Debug' instead of 'Release' for shorter build times.

#### Building the project (WIP)

In the same dropdown where you selected 'Debug', select 'Release' and run the game again.

### For Mac Developers

Since Visual Studio is now unsupported by Microsoft on MacOS, we have to get a little creative when running the project. By getting creative, I really mean using the command line.

#### Setup

1. Run `dotnet restore` to download all of the necessary libraries and tools.

#### Run the project for development

1. Run `dotnet build`. This will place a development build in `bin/Debug/net6.0`.
2. Ensure you're in the `net6.0` directory, then run `./Monogame-RPG-Engine`.

#### Building the project (WIP)

Currently, the project builds in a mess of folders and directories, but it runs! This will guide you through the current process of getting an almost distributable version of the project built.

1. Run the command `dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true`. This will place the final set of files to `bin/Release/net6.0/osx-x64/publish`.
2. Ensure you are in the `publish` directory, then run `./Monogame-RPG-Engine`. This will run the executable in the correct folder, so all of the references within the code work properly.

## How is this project structured?

There are two main parts of this project: the game engine and the game that's built on top of it.
The game engine is inside the `Engine` folder, while the game itself is in the `App` folder.
All content (graphics, font, etc.) is inside the `Content` folder.

## How to create a new game using this code?

If you were to want to create your own game out of this, you can pattern match against the way my game in the `App` folder is made and the way it uses the engine,
and then once remove all of my stuff and do your own thing.
I recommend leaving the `Engine` folder alone until you get to the point where you need to add support for something.
This is a BASIC 2D RPG game engine with a fixed timestep; a lot can be done with it and it is very flexible/expandable, but it is not going to pump out an AAA game out-of-the-box.

For documentation on how the engine/game works, the Java version's docs [here](https://a-r-t.github.io/SER-225-Game-RPG/) should suffice.
There are several differences/inconsistencies here and there due to the different tech stack, but the vast majority of it is still relevant and useful.

## Differences between this version of the game and the Java version

The actual game itself is the same as the Java version, but I had to change the engine here and there to be more compatible with Monogame.
Frankly the engine runs so much better now that it is actually backed by an actual game framework hahaha.

If you are new to Monogame or game dev in general, the patterns around content (loading, pipeline, unloading, etc.) may be confusing at first, but essentially the goal of any game is to manage content (e.g. graphics, sounds, fonts) as efficiently as possible.
This includes aiming for the fastest load time possible, only load what is needed, leaving the lowest memory footprint possible, and just overall being strategic with how/where/when content is being managed.

This project uses a library called [Nopipeline](https://github.com/Martenfur/Nopipeline), which allows for writing a config file and automatically configuring the content pipeline vs having to use the terrible built-in content poipeline tool.
The configuration for the "no pipeline" library is the `Content/Content.npl` file.

This project also uses a font library [FontStashSharp](https://github.com/FontStashSharp/FontStashSharp) for loading/rendering fonts, because...well Monogame's SpriteFont leaves a lot to be desired.
This engine has built-in support for FontStashSharp type fonts.
I also implemented SpriteFont support into the engine just because they are "tried and true", but the FontStashSharp font rending is just so much better, I highly recommend using it over generic SpriteFonts.

The actual "main" method is inside the `App/Main/Run.cs` file.

Inside the `Engine/Core` folder is a `GameLoop.cs` file which is essentially Monogame's "entry point".
This is what kicks off the game loop cycle (update/draw).

Oh and I guess I should mention that this project is written in C#, not Java, so yeah that's a pretty big difference.

## What's next?

There is still a lot of refactoring I need to do to the engine, and improvements/more features I want to work on as well.
...And some project/code clean up.
We'll see if I ever get around to it, but feel free to contribute if you are interested!