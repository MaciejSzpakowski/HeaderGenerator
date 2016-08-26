#pragma once
#include <cstdio>
#include <vector>
#include <cstdio>
#include <string>
class A;
class B;
class C;
class C1;
class Hip;
class FF;

class A
{
private:
B* b;
std::string s;
int i;
protected:
public:
A();
void m1();
};

class B
{
private:
std::vector<A*> v;
float f;
protected:
public:
B();
void m1();
void m2();
};

class C
{
private:
protected:
int val;
public:
C(int a);
};

class C1 : public C
{
private:
int val2;
protected:
public:
C1(int a);
};

namespace N
{

class Hip
{
private:
protected:
public:
bool Test(bool b);
};
}

namespace M
{

namespace L
{

namespace K
{

class FF
{
private:
int a;
protected:
public:
FF();
int get_a();
};
}
}
}
