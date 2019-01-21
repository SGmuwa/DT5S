#pragma once

#include "Pareto.h"

int Pareto_intilizalTableMalloc_free_test(void)
{
	return 0;
}

int Pareto_manyintilizalTableMalloc_free_test(void)
{
	printf("Start many Pareto test...\n");
	Pareto_strValueTable a;
	for (size_t i = 0; i < 1000; i++) {
		Pareto_intilizalTableMalloc(&a, 100, 100, 100);
		Pareto_destructorTableFree(&a);
	}
	printf("Finish manny Pareto test.\n");
	return 0;
}

int manyMallocFree_test(void)
{
	printf("Start many malloc test...\n");
	void * a;
	for (size_t i = 0; i < 100000; i++) {
		a = malloc(1024 * 4);
		free(a);
	}
	printf("Finish many free test.\n");
	return 0;
}

int Pareto_find_test(void)
{
	printf("Pareto find test start...\n");
	Pareto_strValueTable tst; // test
	if (Pareto_intilizalTableMalloc(&tst, 9, 3, 30) != 0)
	{
		printf("Pareto malloc error in Pareto_findMalloc.");
		return;
	}

	memcpy(tst.titles[0].str, "Salary (rub) +", tst.titles[0].length);
	memcpy(tst.titles[1].str, "Duration of vacation (days) +", tst.titles[1].length);
	memcpy(tst.titles[2].str, "Travel time (minutes) -", tst.titles[2].length);

	for (size_t i = 0; i < 9; i++)
	{
		tst.lines[i].name.str[0] = '1' + i;
		tst.lines[i].name.str[1] = 0;
	}

	tst.flags[0] = 1;
	tst.flags[1] = 1;

	tst.flags[2] = 0;

	double colums[3];
	colums[0] = 900.0; colums[1] = 20.0; colums[2] = 60.0;
	memcpy_s(tst.lines[0].columns, sizeof(*tst.lines->columns)*tst.countColumns, colums, sizeof(double[3]));
	
	colums[0] = 500.0; colums[1] = 30.0; colums[2] = 20.0;
	memcpy_s(tst.lines[1].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 700.0; colums[1] = 36.0; colums[2] = 40.0;
	memcpy_s(tst.lines[2].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 800.0; colums[1] = 40.0; colums[2] = 50.0;
	memcpy_s(tst.lines[3].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 400.0; colums[1] = 60.0; colums[2] = 15.0;
	memcpy_s(tst.lines[4].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 600.0; colums[1] = 30.0; colums[2] = 10.0;
	memcpy_s(tst.lines[5].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 900.0; colums[1] = 35.0; colums[2] = 60.0;
	memcpy_s(tst.lines[6].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 600.0; colums[1] = 24.0; colums[2] = 10.0;
	memcpy_s(tst.lines[7].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));
	
	colums[0] = 650.0; colums[1] = 35.0; colums[2] = 40.0;
	memcpy_s(tst.lines[8].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[]) {  }, sizeof(double[3]));

	Pareto_write(tst);
	Pareto_strValueTable tst2 = Pareto_findMalloc(tst);
	Pareto_write(tst2);

	Pareto_destructorTableFree(&tst);
	Pareto_destructorTableFree(&tst2);

	printf("Pereto find test end...\n");
	return 0;
}

// Запуск всех тестов файла Pareto.
int Pareto_runTests() {
	int error = 0;
	if ((error = Pareto_intilizalTableMalloc_free_test()) != 0)
		return error;
	else if ((error = Pareto_manyintilizalTableMalloc_free_test()) != 0)
		return error;
	else if ((error = manyMallocFree_test()) != 0)
		return error;
	else
		return Pareto_find_test();
}
