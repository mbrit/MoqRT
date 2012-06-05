The principle
===
Dynamic code generation is gone in WinRT (see [http://geekswithblogs.net/mbrit/archive/2012/06/05/say-goodbye-to-system.reflection.emit-any-dynamic-proxy-generation-in-winrt.aspx]).
MoqRT uses a port of the Moq library and Castle Code Dynamic Proxy component to re-establish mocking
when executing TDD in Metro-style.

The idea is that you create your mocks as normal, but rather than generating them on-the-fly they are
"baked" into a proper assembly that you use in your project. It's exactly the same Moq that you're
used to using, only compiled against the WinRT .NET Core project. Similarly, rather than using all of
Castle Core, just the Dynamic Proxy components hae been brought in and tweaked.

This only works against the Visual Studio 2012 test runner. But that's OK as of the time of writing,
the only testing framework that supports Metro-style is VS's.

Getting started
===
* Clone the repository.

* You'll find separate Metro-style and .NET solutions. Use MoqRT.MetroStyle.dll in your Metro-style
tests project.

* Build and compile the project as normal.

* In the .NET project, run the MoqRT.DotNet.Baker.Client executable.

* Browse to the assembly that you wish to use and click the Run button. This will load the assembly
and look for TypeClass and TypeMethod declarations. 

* When it finds them, it will run each method. Ultimately it will create an assembly called
`MoqRT.Baked.dll` in a folder called `~\bin\Baking` in your project. 

* Reference `MoqRT.Baked.dll` from that folder into your project.

* You should now be able to run your tests.

* Everytime you build the project, the baking client will recreate the assembly both in the `\bin\Baking`
folder and in the `AppX` folder of your project. 

* It will also create a SQLite database fail called `MoqRT.Baked.dll.db`. This holds a register of the
mocked types and their proxies.

Gotchas
===
* You may find getting the `sqlite3.dll` files in the correct place tricky. If you get errors about
SQLite, check those files exist where the components can reach them.

* The client is a bit compile happy, and probably a bit flaky. You need to be quite explicit with it - e.g.
wait until it's finished building the new assembly, confirm it's in the correct location, etc.

* This is very much an "alpha release". Please ping me on Twitter (@mbrit) with feedback.