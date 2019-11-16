#include "Foo.h"

Foo::Foo(int newFoo)
	:f(newFoo)
{
	return;
}

int Foo::foo(int bar)
{
	return (bar + f);
}