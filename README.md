UnityAutoMoq
============
Automocking container using Microsoft Unity and Moq.

	// Creating a new instance of the auto mock container
	var container = new UnityAutoMoqContainer();
	
	// Resolving a concrete class automatically creates
	// mocks for the class dependencies and injects them
	// before returning an instance of the class
	Service service = container.Resolve<Service>();
	
	// Resolving an interface, returns a mocked
	// instance of that interface
	IService mocked = container.Resolve<IService>();
	
	// GetMock returns the mock on which you can do setup etc.
	Mock<IService> mock = container.GetMock<IService>();
	
	// Sometimes you need to cast your interface to some other type
	// This is how that is done
	container.ConfigureMock<IService>().As<IDisposable>();
	Mock<IDisposable> disposable = container.GetMock<IService>().As<IDisposable>();
	
License
-------
The MIT License

Copyright (c) 2011 Thomas Pedersen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.