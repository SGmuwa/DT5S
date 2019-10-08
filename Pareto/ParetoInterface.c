#include "Pareto.h"
#include "..\UserInterface-CLanguage\UserInterface.h"

void main(void)
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

	string_t titles = Pareto_getListTitlesMalloc(table2);
	do {
		printf("main?\n");
		size_t chekByUser = ~(size_t)0;
		if (table2.countColumns < (unsigned char)~(unsigned char)0)
			chekByUser = UserInterface_GetChek(titles.first, (unsigned char)table2.countColumns);
		else
			chekByUser = (size_t)UserInterface_GetUnsignedLongLongIntLimit(titles.first, 0, table2.countColumns - 1);

		Pareto_strValueTable table3 = Pareto_optiMalloc(table2, chekByUser, UserInterface_GetFloat("граница = "));
		Pareto_destructorTableFree(&table2);
		Pareto_write(table3);
		table2 = table3;
	} while (UserInterface_GetChek("Продолжить?\n0) Выйти\n1) Выбрать ещё\n", 1));
	Pareto_destructorTableFree(&table2);
	free(titles.first); titles.first = NULL; titles.length = 0;
}