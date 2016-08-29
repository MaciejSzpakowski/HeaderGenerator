#include "Header.h"
#include <string>
#include <cstdio>

namespace N
{
    class Test9
    {
    private:
        Test1 t1;
        Num num;
        M::TestStruct ts;
    public:
        Test9() :num({ 9 })
        {
            t1.m3(num.i, "back reference passed\n");
        }
    };
}

class Test1
{
private:
    Test2* t;
    std::string s;
    int i;
public:
    Test1()
    {
        s = "Test";
        i = 1;
    }

    void m1() 
    {
        printf("%s %d passed\n", s.c_str(), i);
    }

    int m2(int k)
    {
        printf("%s %d circular ptr reference passed\n", s.c_str(), k);

        return 0;
    }

    void m3(int k, std::string&& str)
    {
        printf("%s %d %s\n", s.c_str(), k, str.c_str());
    }
};