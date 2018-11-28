#include "stdafx.h"
#include "CppUnitTest.h"
#include "..\Pareto.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;


namespace Tests
{

	TEST_CLASS(Pareto)
	{
	public:
		
		TEST_METHOD(MyFirstTestInVS)
		{
			// TODO: Разместите здесь код своего теста
			Assert::IsTrue(0 == 0, L"Zero is zero");
			
		}

		TEST_METHOD(Pareto_init)
		{
			printf("Pareto intilizalTableMalloc and free\tStart test...");
			Pareto_strValueTable tst;
			Assert::IsTrue(Pareto_intilizalTableMalloc(&tst, 5, 6, 10) == 0, L"error to create table.");

			// Тестирование значений таблицы

			Assert::IsTrue(tst.countColumns == 6, L"error tst.countColumns");
			Assert::IsTrue(tst.countLines == 5, L"error tst.countLines");

			// Тестирование пустых указателей таблицы

			Assert::IsNotNull(tst.lines, L"error: lines NULL");
			Assert::IsNotNull(tst.titles, L"error: tst.titles NULL");

			// Тестирование экземпляров таблицы

			for (size_t i = tst.countLines; --i != ~(size_t)0u; )
			{
				// Тестирование имён экхемпляров

				Assert::IsTrue(tst.lines[i].name.length == 10, L"error tst.lines[i].name.length != 10");

				for (size_t ch = 0; ch < 10; ch++)
					tst.lines[i].name.str[ch] = '0' + (char)ch;
				for (size_t ch = 0; ch < 10; ch++)
					Assert::IsTrue(tst.lines[i].name.str[ch] == '0' + (char)ch, L"error tst.lines[i].name.str[ch] != '0' + (char)ch");

				// Тестирование значений экземпляра

				Assert::IsNotNull(tst.lines[i].columns, L"error tst.lines[i].columns == NULL");

				for (size_t j = tst.countColumns; --j != ~(size_t)0;)
				{
					tst.lines[i].columns[j] = 123;
					Assert::IsTrue(tst.lines[i].columns[j] == 123, L"error tst.lines[i].columns[j] != 123");
				}
			}

			// Тестирование заголовков таблицы
			for (size_t i = tst.countColumns; --i != ~(size_t)0;)
			{
				Assert::IsTrue(tst.titles[i].length == 10, L"error tst.titles[i].length != 10");
				for (size_t ch = 0; ch < 10; ch++)
					tst.titles[i].str[ch] = '0' + (char)ch;
				for (size_t ch = 0; ch < 10; ch++)
					Assert::IsTrue(tst.titles[i].str[ch] == '0' + (char)ch, L"error tst.titles[i].str[ch] != '0' + (char)ch");
			}

			Pareto_destructorTableFree(&tst);

			Assert::IsTrue(tst.countColumns == 0, L"error tst.countColumns != 0");
			Assert::IsTrue(tst.countLines == 0, L"error tst.countLines != 0");
			Assert::IsNull(tst.lines, L"error tst.lines != NULL");
			Assert::IsNull(tst.titles, L"error tst.titles != NULL");
		}
	};
}