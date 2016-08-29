#include "Header.h"

namespace N
{
    class Test7
    {
    private:
        class Nest
        {
        public:
            std::string str;
            void operator()()
            {
                printf("%s", str.c_str());
            }
        };

        Nest str[3];
    public:
        Test7()
        {
            str[0].str = "Test 7 ";
            str[1].str = "nested class and opeartor ";
            str[2].str = "passed\n";

            str[0]();
            str[1]();
            str[2]();
        }
    };

    struct Test8
    {
        std::string str;

        Test8():str("Test 8 struct passed\n")
        {
            printf("%s", str.c_str());
        }
    };

    struct Num
    {
        int i;
    };
}

namespace M
{
    struct TestStruct
    {
        int j;
    };
}