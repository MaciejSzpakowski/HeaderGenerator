#pragma once
#include <cstdio>
#include <vector>
#include <cstdio>
#include <string>
class Test9;
class Test1;
class Test2;
class Test3;
class Test4;
class Test5;
class Test6;
class Test7;
class Nest;
struct Test8;
struct Num;
struct TestStruct;

namespace N
{

class Test9
{
private:
Test1 t1;
Num num;
M::TestStruct ts;
public:
Test9();
);
};

class Test7
{
private:

class Nest
{
public:
std::string str;
void operator()();
};
Nest str[3];
public:
Test7();
};

struct Test8
{
std::string str;
Test8();
};

struct Num
{
int i;
};
}

class Test1
{
private:
Test2* t;
std::string s;
int i;
public:
Test1();
void m1();
int m2(int k);
void m3(int k, std::string&& str);
};

class Test2
{
private:
std::vector<Test1*> v;
float f;
public:
Test2();
void m1();
void m2();
};

class Test3
{
protected:
int val;
public:
Test3(int a);
void m1();
};

class Test4 : public Test3
{
private:
int val2;
public:
Test4(int a);
};

namespace NamespaceTest5
{

class Test5
{
public:
Test5();
bool otherm(bool b);
};
}

namespace NamespaceTest6
{

namespace O
{

namespace P
{

class Test6
{
private:
int a;
public:
Test6();
int get_a();
};
}
}
}

namespace N
{
}

namespace M
{

struct TestStruct
{
int j;
};
}
