#include "Game_Physics_DLL.h"

#include "Foo.h"

// Turn into singleton if wanted.
Foo* instance = 0;

int InitFoo(int newFoo)
{
	if (!instance)
	{
		instance = new Foo(newFoo);

		return 1;
	}
	
	return 0;
}

int DoFoo(int bar)
{
	if (instance)
	{
		int result = instance->foo(bar);
		
		return result;
	}
	return 0;
}

int TermFoo()
{
	if (instance)
	{
		delete instance;
		instance = 0;
		return 1;
	}

	return 0;
}