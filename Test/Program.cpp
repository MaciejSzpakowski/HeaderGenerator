#include "Header.h"

int main()
{
    printf("Tests: 8\n");

    Test1 test;
    test.m1();
    Test2 test2;
    test.m2(2);
    Test3 test3(3);
    test3.m1();
    Test4 test4(4);
    NamespaceTest5::Test5 test5;
    NamespaceTest6::O::P::Test6 test6;
    N::Test7 test7;
    N::Test8 test8;
}