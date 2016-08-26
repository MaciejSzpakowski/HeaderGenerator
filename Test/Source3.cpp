#include "Header.h"

class C
{
public:
    C(int a) :val(a)  {   }
protected:
    int val;
};

class C1 : public C
{
private:
    int val2;
public:
    C1(int a) : C(a * 2), val2(a)
    {
    }
};