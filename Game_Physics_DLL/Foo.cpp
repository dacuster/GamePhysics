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

CppClass::CppClass(int newInt)
{
	id = newInt;

	return;
}

CppClass::~CppClass()
{
}

int CppClass::add(int newId)
{
	return id + newId;
}