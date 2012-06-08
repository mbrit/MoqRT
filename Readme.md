The principle
===
Dynamic code generation is gone in WinRT (see [http://geekswithblogs.net/mbrit/archive/2012/06/05/say-goodbye-to-system.reflection.emit-any-dynamic-proxy-generation-in-winrt.aspx]).
MoqRT uses a port of the Moq library and Castle Code Dynamic Proxy component to re-establish mocking
when executing TDD in Metro-style.

The idea is that you create your mocks as normal, but rather than generating them on-the-fly they are
"baked" into a normal assembly that you reference in your project. A separate executable listens
for new builds happening of your test assembly, whereupon it takes your tests, runs them and stores
the created dynamic proxies for later use.

Under the good, it's exactly the same Moq that you're used to using, only compiled against WinRT. 
Similarly, rather than using all of Castle Core, just the Dynamic Proxy components have been brought 
in and ported to WinRT. 

This only works against the Visual Studio 2012 test runner. But that's OK as of the time of writing,
the only testing framework that supports Metro-style is VS's.

Getting started
===
_CAUTION: This is a reasonably fiddly collection of steps. Please follow them carefully!_

MORE CAUTION: There is a bug in this current version that stops integer return values
from being processed property. (e.g. `Setup(...)...Returns(...)` where `Returns` is an
integer type.) Use strings at this point. This will obviously be fixed as a matter of 
urgency.

The steps...

* Clone the repository.

* You'll find separate Metro-style and .NET solutions. Build both of these solutions. Do not 
run the unit tests of the Metro-style project at this point. (You can later, but they rely on
you understanding the process you're about to learn now!)

* The .NET solution contains the "Baker". This comprises a Windows Forms client application and 
a console application - both of which we'll come onto.

* Create a new test project. Reference the MoqRT.MetroStyle.dll assembly in you project. Add
a new test class that uses a mock. (See Moq's documentation for this.) This should compile OK.

* Sign you project. This is an important step - there's a current bug where MoqRT assumes
that your test project is signed. (This will be fixed in a future release as it's possible that
your assembly tree is not signed.)

* MoqRT uses a SQLite database to keep track of which mocks have been baked. You'll need to add
`sqlite3.dll` to your test project. There is a special version of SQLite for use in Metro-style
apps. You'll find this in the `~/Lib` folder in the repo. To add this to the test project,
right-click, select Add - Existing Item and navigate to the `sqlite3.dll` file. You don't need
to do anyting else other than just include it in the project. VS will package it up as a 
static dependency of the test executable.

* Open the `MoqRT.Baker.Client` executable. The client uses .NET Remoting to listen for instructions
to bake proxies, so if prompted accept any firewall changes.

* You may not want to run the client executable in the debugger as any normal test failures will
break the client and bring VS to the foreground. Run the client detached from any debugger.

* Click the Browse button and select your test assembly. This should update the fields on the screen.

* Click the Run button. This puts the client in monitoring mode. This will detect any test classes
in your application and present them in a tree. (More on this in a moment.)

* At this point you will need an assembly to refrence. Click the Force Baking button and a new
`MoqRT.Baked.dll` assembly will be created in the `~/bin/Baking` folder in your project. Reference
this assembly in your project and re-build. (You only need to do this once.) The baking operation
will fail if you have not signed your test project, as discussed earlier.

* To tell the Baker that your test has been built you need to run a post-build event. Edit the 
test project properties, and add a new Post-build Event. This just has to reference the `MoqRT.Baker.Console.exe`.
No parameters are required. All this console application does is open a .NET Remoting connection to the
main client and ask it to "bake". It will then block until baking is complete.

* Now rebuild (not "build") your test project. You'll see a message in the VS Output window saying that
baking has occurred. Check in the client application and this should be updated.

* Run your tests. Your mock should work correctly.

* Change your test and run them again. The Baker should recreate your tests automatically.

If you get errors regarding "not being able to access the SQLite database", go back to the client and
"Force Baking". Check that `MoqRT.Baked.dll.db` is in the `AppX` folder.

Excluding tests
---
The reason why there is a client application is to address a problem with this approach in that
tests have to run in order to create the mocks. The ultimate extension of this is where you can 
end up running the entire test suite just to run one test.

On the client, you can use the tree to check on or off classes and methods that you do or don't 
want to include in a run. For example, if you have 50 test classes in your project and you're only
working on one, you can turn off all but the one that you're using.

Another thing that you can do is tell the Baker that mock generation has finished for a certain
class. For example, if mock setup takes milliseconds but the test takes several seconds, you want
to abort the baking once the mocks are setup. To do this, call the `MoqRTRuntime.StopIfBaking`
method. This will have no effect on the test, but will abort the method if it's being used
by the Baker.

You can also use the `IgnoreBaking` attribute to indicate that a method does not use mocks
and should not be included in the baking process.

Feedback
===
This is very much an "alpha release". Please ping me on Twitter (@mbrit) with feedback.

