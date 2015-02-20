![Icon](https://raw.github.com/kzu/SmartReferences/master/icon/32.png) SmartReferences
===============

Smarter project and assembly references with MSBuild

[![Build status](https://img.shields.io/appveyor/ci/kzu/SmartReferences.svg)](https://ci.appveyor.com/project/kzu/SmartReferences)
[![NuGet downloads](https://img.shields.io/nuget/dt/SmartReferences.svg)](https://www.nuget.org/packages/SmartReferences)
[![Version](https://img.shields.io/nuget/v/SmartReferences.svg)](https://www.nuget.org/packages/SmartReferences)

Sometimes the simple act of adding a project or assembly reference results in multiple subsequent attempts to fix the build because of the myriad indirect references that those depend on in turn, and which you are forced to add. Some other times, you just wish you were able to remove all those external projects that clutter the solution and slow down VS, but you need the project references for those tough debugging times. Welcome to Smarter Project References!

If you want to go straight to the best thing to happen to MSBuild since Shared Projects (wait, that wasn't long ago at all...), just install this package on your projects:

```
Install-Package SmartReferences
```

## Indirect Reference Annoyances

This problem manifests itself pretty easily and I'm sure we've all faced this issue at some point or another. A very simple (hypotetical) scenario where this happens is: 

- You have a shared MVVM project that has a base ViewModel class which is really awesome, since it does some logging for you via a log4net ILogger protected property it exposes, and also uses an Autofac type for doing lazy composition with metadata (or whatever). 
- You add that to your solution.
- You create the new UI project, and reference the nice MVVM library you created. 
- Next, of course, you go and create your first concrete view model based on ViewModel. 

Ooops! Now the build fails because you need to also reference log4net and Autofac. Even if you're not going to use them directly at all. Yuck!

When doing Visual Studio Extensibility, it's even worse as you keep chasing the last dependency you need in order to build (i.e. try to get IVsStatusBar from Package.GetGlobalService and see what happens ;))

It's really inconvenient since every time your library takes a new dependency, you need to update the referring projects most of the time too.

Well, no more with SmartReferences. Just install the package, and let the project figure what it needs for you. It's all there already anyway, right on the referenced project or assembly!

## Gigantic Solution Annoyances

It's not all that uncommon to have large solutions with dozens of projects organized by functional or logical area, and even maybe including third-party projects you depend on and want to build from source.

However, team members aren't expected to always touch code throughout the entire code base. Maybe they work on some specific area only. Maybe third-party projects are only needed if there's a problem that requires debugging and fixing in-place. Maybe, it's just that Visual Studio gets slow with all those projects that you're actually not currently working on!

As nice as having separate solutions with various subsets sounds, it's most of the time impractical. Whenever a project has a project reference to another, regardless of whether you want that other project in the solution or not, you NEED it in order for Visual Studio to find it and successfully build. And you have to do all that analysis yourself, looking for potential projects that can be removed from specific subsets. BOOORING! You just cope with it and decide to get a coffee while you wait for VS to load the humongous solution ;).

What if dropping projects from the solution was as easy as removing them from it, leaving all project references *intact*? That's precisely what SmartReferences does! It does so with the following heuristics:

- Before MSBuild needs to resolve references, it sees what project references aren't included in the current solution.
- It next retrieves the output assembly of that project for a configuration matching the calling project one (i.e. Debug), and if it's outdated with regards to its project file, builds it
- Finally, it replaces the project reference dynamically with the output assembly reference.

The usability improvements are massive. You can just have different solutions, and at the point where one particular project needs to be debugged with source (set breakpoints, fix the code, etc.), you just include that project back in the solution, F5 and you're done! Finished working on it? Just remove from solution again and back to depending on the last good known binary!

> Just remember NOT to save VS suggested changes to the referencing project... it will want to delete the project reference to the project you just removed from the solution).

No changes whatesoever are needed on any of the projects in order to participate, as long as they have the SmartReferences package installed.

Needless to say, you can still "smart reference" a project that does not use SmartReferences, but its own project references won't be automagically resolved when you add it to your solution.


Go ahead and install it:

```
Install-Package SmartReferences
```

It's [open source on GitHub](https://github.com/kzu/SmartReferences) and I'd gladly take PRs and issues you may find. 

Enjoy!
