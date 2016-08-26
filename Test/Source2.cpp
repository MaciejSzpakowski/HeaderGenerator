#include "Header.h"
#include <vector>
#include <cstdio>

class B
{
private:
    std::vector<A*> v;
    float f;
public:
    B() 
    {
        f = 1.1;
        v.push_back(new A());
    }

    void m1() 
    {
        v[0]->m1();
    }

    void m2()     
    {
        printf("%f\n", f);
    }
};