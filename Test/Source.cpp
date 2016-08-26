#include "Header.h"
A::A()
{
s = "string";
        i = 7;
    }

void A::m1()
{
printf("%s %d\n", s.c_str(), i);
    }

B::B()
{
f = 1.1;
        v.push_back(new A());
    }

void B::m1()
{
v[0]->m1();
    }

void B::m2()
{
printf("%f\n", f);
    }

C::C(int a)
:val(a)  {
}

C1::C1(int a)
:C(a * 2), val2(a)
    {
}

bool N::Hip::Test(bool b)
{
return !b;
        }

M::L::K::FF::FF()
:a(6) {
}

int M::L::K::FF::get_a()
{
return a;
                }

