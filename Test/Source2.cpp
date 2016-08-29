#include "Header.h"
#include <vector>
#include <cstdio>

class Test2
{
private:
    std::vector<Test1*> v;
    float f;
public:
    Test2()
    {
        f = 2;
        v.push_back(new Test1());
    }

    void m1() 
    {
        m2();
    }

    void m2()     
    {
        v[0]->m2(2);
    }
};