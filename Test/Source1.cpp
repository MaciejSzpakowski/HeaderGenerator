#include "Header.h"
#include <string>
#include <cstdio>

class A
{
private:
    B* b;
    std::string s;
    int i;
public:
    A() 
    {
        s = "string";
        i = 7;
    }

    void m1() 
    {
        printf("%s %d\n", s.c_str(), i);
    }
};