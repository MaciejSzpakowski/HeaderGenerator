#include "Header.h"

namespace NamespaceTest5
{
    class Test5
    {
    public:
        Test5()
        {
            otherm(false);
            printf("Test 5 namespace passed\n");
        }

        bool otherm(bool b)
        {
            return !b;
        }
    };
}

namespace NamespaceTest6
{
    namespace O
    {
        namespace P
        {
            class Test6
            {
            private:
                int a;
            public:
                Test6() : a(6) { printf("Test %d nested namespace passed\n",get_a()); }

                int get_a()
                {
                    return a;
                }
            };
        }
    }
}