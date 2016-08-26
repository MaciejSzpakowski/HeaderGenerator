#include "Header.h"

namespace N
{
    class Hip
    {
    public:
        bool Test(bool b)
        {
            return !b;
        }
    };
}

namespace M
{
    namespace L
    {
        namespace K
        {
            class FF
            {
            private:
                int a;
            public:
                FF() : a(6) {}

                int get_a()
                {
                    return a;
                }
            };
        }
    }
}