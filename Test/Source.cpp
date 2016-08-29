#include "Header.h"
N::Test9::Test9()
:num({
9 }

N::Test9::)
{
t1.m3(num.i, "back reference passed\n");
        }

void N::Test7::Nest::operator()()
{
printf("%s", str.c_str());
            }

N::Test7::Test7()
{
str[0].str = "Test 7 ";
            str[1].str = "nested class and opeartor ";
            str[2].str = "passed\n";

            str[0]();
            str[1]();
            str[2]();
        }

N::Test8::Test8()
:str("Test 8 struct passed\n"){
printf("%s", str.c_str());
        }

Test1::Test1()
{
s = "Test";
        i = 1;
    }

void Test1::m1()
{
printf("%s %d passed\n", s.c_str(), i);
    }

int Test1::m2(int k)
{
printf("%s %d circular ptr reference passed\n", s.c_str(), k);

        return 0;
    }

void Test1::m3(int k, std::string&& str)
{
printf("%s %d %s\n", s.c_str(), k, str.c_str());
    }

Test2::Test2()
{
f = 2;
        v.push_back(new Test1());
    }

void Test2::m1()
{
m2();
    }

void Test2::m2()
{
v[0]->m2(2);
    }

Test3::Test3(int a)
:val(a){
}

void Test3::m1()
{
printf("Test %d one line ctor and ctor arg and initializer list passed\n", val); }

Test4::Test4(int a)
:Test3(4), val2(a){
printf("Test %d inheritance passed\n", val2);
    }

NamespaceTest5::Test5::Test5()
{
otherm(false);
            printf("Test 5 namespace passed\n");
        }

bool NamespaceTest5::Test5::otherm(bool b)
{
return !b;
        }

NamespaceTest6::O::P::Test6::Test6()
:a(6){
printf("Test %d nested namespace passed\n",get_a()); }

int NamespaceTest6::O::P::Test6::get_a()
{
return a;
                }

