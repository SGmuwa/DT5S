#include "stdafx.h"
#include "CppUnitTest.h"
#include "..\Example_C.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;


namespace Tests
{

	TEST_CLASS(CppWithC)
	{
	public:

		TEST_METHOD(firstTest)
		{
			// TODO: Разместите здесь код своего теста
			Assert::IsTrue(0 == 0, L"Zero is not zero");
		}

		TEST_METHOD(NeedCppInHeader)
		{
			Assert::IsTrue(Example_C_isCpp == myF_getCpp_ewijwioggowgogjw(), L"Not C++ language!");
		}

		TEST_METHOD(NeedCInHeader)
		{
			Assert::IsTrue(Example_C_isC == myF_getC_ewijwioggowgogjw(), L"Not C language!");
		}
	};
}