// Реализовать программу, которая реализует субоптимизацию

#include "UserInterface.h"
#include "Pareto.h"

// Тестирование метода Парето
void z1_test(void)
{
	printf("z1\tStart test...\n");
	Pareto_intilizalTableMalloc_free_test();
	manyMallocFree_test();
	Pareto_manyintilizalTableMalloc_free_test();
	Pareto_find_test();
	printf("z1\tFinish test...\n");
}

// Интерфейс работы с Парето
void z1_interface(void)
{
	Pareto_strValueTable table;
	if (Pareto_intilizalDefaultTableMalloc(&table) != 0)
	{
		printf("error to create table.\n");
		return;
	}
	printf("\n------------------------------\n");
	Pareto_write(table);
	printf("\n------------------------------\n");
	Pareto_strValueTable table2 = Pareto_findMalloc(table);
	Pareto_destructorTableFree(&table);
	Pareto_write(table2);

	string titles = Pareto_getListTitlesMalloc(table2);
	do {
		printf("main?\n");
		size_t chekByUser = ~(size_t)0;
		if (table2.countColumns < (unsigned char)~(unsigned char)0)
			chekByUser = UserInterface_GetChek(titles.str, (unsigned char)table2.countColumns, stdin, stdout);
		else
			chekByUser = UserInterface_GetUnsignedLongLongIntLimit(titles.str, 0, table2.countColumns - 1);

		Pareto_strValueTable table3 = Pareto_optiMalloc(table2, chekByUser, UserInterface_GetFloat("border = "));
		Pareto_destructorTableFree(&table2);
		Pareto_write(table3);
		table2 = table3;
	} while (UserInterface_GetChek("Continue?\n0) No\n1) Yes\n", 1));
	Pareto_destructorTableFree(&table2);
	free(titles.str); titles.str = NULL; titles.length = 0;
}

// Тестирование метода Электро
void z2_test(void)
{

}

// Интерфейс метода Электро
void z2_interface(void)
{

}

void run(void)
{
	z1_test();
	switch (UserInterface_GetChek("0. Pareto\n1. el\n", 1))
	{
	case 0:
		z1_interface();
		break;
	case 1:
		z2_interface();
		break;
	}
	
	

	UserInterface_Pause("Press any key...\n");

}

