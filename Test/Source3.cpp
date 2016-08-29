#include "Header.h"

class Test3
{
public:
    Test3(int a) :val(a) {  }
    void m1() { printf("Test %d one line ctor and ctor arg and initializer list passed\n", val); }
protected:
    int val;
};

class Test4 : public Test3
{
private:
    int val2;
public:
    Test4(int a) : Test3(4), val2(a)
    {
        printf("Test %d inheritance passed\n", val2);
    }
};