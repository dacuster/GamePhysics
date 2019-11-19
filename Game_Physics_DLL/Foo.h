#ifndef FOO_H
#define FOO_H

class Foo
{
public:
	Foo(int newFoo = 0);
	int foo(int bar = 0);

private:
	int f;
};


class CppClass
{
public:
	CppClass(int newInt);
	~CppClass();

	int add(int newInt);

private:
	int id;
};

#endif // !FOO_H