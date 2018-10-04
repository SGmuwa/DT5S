// Реализовать программу, которая реализует субоптимизацию

#include "UserInterface.h"
#include <stdlib.h>
#include <string.h>

typedef unsigned char byte;

// Представляет из себя тип данных, который хранит в себе double значение и флаг.
typedef struct {
	double numerical; // Численное значение.
} doubleFlag;

// Структура обозначает массив строк.
typedef struct {
	char * str; // Указатель на первый символ строки.
	size_t length; // Количество доступных символов.
} string;

// Символизирует строку таблицы: название экземпляра и указатель на начало его параметров.
typedef struct {
	string name;
	double * columns;
} Pareto_strValues;

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	string * titles;			// Указатель на заголовки таблицы.
	Pareto_strValues * lines;	// Представляет собой строку с именем экземпляра и его значениями.
	byte * flags;				// Указатель на массив флагов. Если 0 - значит, лучше отрицательное значение. Если 1 - лучше положительные значения.
	size_t countLines;			// Количество экземпляров в таблице.
	size_t countColumns;		// Количество критериев экземпляров в таблице.
} Pareto_strValueTable;

int Pareto_write(const Pareto_strValueTable input)
{
	
	for (size_t i = 0; i < input.countColumns; i++)
	{
		printf("%s;\t", input.titles[i].str);
	}
	printf("\n");
	for (size_t i = 0; i < input.countLines; i++)
	{
		printf("%s;\t", input.lines[i].name.str);
		for (size_t j = 0; j < input.countColumns; j++)
		{
			printf("%f;\t", input.lines[i].columns[j]);
		}
		printf("\n");
	}
}

// Удаление определённых линий.
// const Pareto_strValueTable input - Источник строк.
// const size_t * deleteIds - Список индентификаторов, которые должны быть удалены. Повторы разрешены. Значения в массиве ~(size_t)0 будут игнорироваться.
// size_t lengthIds - Количество идентификаторов.
Pareto_strValueTable Pareto_deleteLinesMalloc(const Pareto_strValueTable input, const size_t * deleteIds, size_t lengthIds)
{
	if (deleteIds == NULL || lengthIds == 0)
		return (Pareto_strValueTable) { NULL, NULL, NULL, 0, 0 }; // Не хватило памяти.


	size_t * ids = (size_t*)malloc(lengthIds * sizeof(*ids));

	if (ids == NULL)
		return (Pareto_strValueTable) { NULL, NULL, NULL, 0, 0 }; // Не хватило памяти.

	memcpy_s(ids, lengthIds * sizeof(*ids), deleteIds, lengthIds * sizeof(*deleteIds));

	// Удаление одинаковых чисел из ids.
	for (size_t i = lengthIds; --i != 0;)
		if (ids[i] == ~(size_t)0)
			continue;
		else for (size_t j = i; --j != ~(size_t)0;)
			if (ids[i] == ids[j])
				ids[i] = ~(size_t)0;

	size_t countids = 0; // Количество найденных

	for (size_t i = lengthIds; --i != ~(size_t)0;)
		if (ids[i] != ~(size_t)0)
			countids++;

	Pareto_strValueTable out =
	{
		NULL, // Указатель на заголовки таблицы.
		NULL, // Представляет собой строку с именем экземпляра и его значениями.
		NULL, // Указатель на массив флагов. Если 0 - значит, лучше отрицательное значение. Если 1 - лучше положительные значения.
		0,	  // Количество экземпляров в таблице.
		0	  // Количество критериев экземпляров в таблице.
	};

	size_t countMaybeWinners = input.countLines - countids;
	size_t * maybeWinners = (size_t *)malloc(countMaybeWinners * sizeof(size_t));

	if (maybeWinners == NULL)
	{
		return out;
	}

	for (size_t i = 0, iMay = 0; i < input.countLines; i++)
	{
		size_t j = lengthIds;
		while (--j != ~(size_t)0)
			if (ids[j] == i) // Лузер найден!
			{
				// Надо не дать лузеру дойти до входа!
				break;
			}
		if (j == ~(size_t)0)
			maybeWinners[iMay++] = i;
	}

	free(ids);
	ids = NULL;

	// Копирование заголовков.

	size_t maxCountStr = input.titles[0].length; // Количество необходимых символов.
	size_t countStr = input.titles[0].length; // Сумма всех символов.

	for (size_t i = input.countColumns; --i != 0;)
	{
		countStr += input.titles[i].length;
		if (maxCountStr < input.titles[i].length)
			maxCountStr = input.titles[i].length;
	}

	if (Pareto_intilizalTableMalloc(&out, countMaybeWinners, input.countColumns, maxCountStr) != 0)
	{
		free(maybeWinners);
		return out;
	}

	for (size_t i = out.countColumns; --i != ~(size_t)0;)
		memcpy_s(out.titles[i].str, out.titles[i].length, input.titles[i].str, input.titles[i].length);

	memcpy_s(out.flags, out.countColumns * sizeof(*out.flags), input.flags, input.countColumns * sizeof(*input.flags));

	// Сформировать список из победителей или нейтралов.

	for (size_t i = out.countLines; --i != ~(size_t)0;)
	{
		for (size_t j = out.countColumns; --j != ~(size_t)0;)
			out.lines[i].columns[j] = input.lines[maybeWinners[i]].columns[j];
		memcpy_s(out.lines[i].name.str, out.lines[i].name.length, input.lines[maybeWinners[i]].name.str, input.lines[maybeWinners[i]].name.length);
	}
	free(maybeWinners);
	return out;
}

// Отвечает на вопрос, кто по Парето лучше?
// size_t indexFirst - первый представитель.
// size_t indexSecond - второй представитель.
// Pareto_strValueTable table - данные из таблицы.
// Возвращает: 1, если первый лучше второго. 0 - если нельяз сравнить. 2 - второй лучше первого. 255 - ошибка.
byte Pareto_isFirstBetter(size_t indexFirst, size_t indexSecond, const Pareto_strValueTable table)
{
	size_t better[3] = {
		0, // ничья
		0, // первый лучше
		0  // второй лучше
	};
	for (size_t i = table.countColumns; --i != ~(size_t)0; )
	{
		if (table.lines[indexFirst].columns[i] == table.lines[indexSecond].columns[i])
			better[0]++; // Если равны, то ничья.

		else if (table.flags[i]) // Стримится к положительным числам.
		{
			if (table.lines[indexFirst].columns[i] > table.lines[indexSecond].columns[i])
				better[1]++;
			else//if(first.columns[i].numerical <  second.columns[i].numerical)
				better[2]++;
		}
		else // Стримится к отрицательным числам.
		{
			if (table.lines[indexFirst].columns[i] < table.lines[indexSecond].columns[i])
				better[1]++;
			else//if(first.columns[i].numerical >  second.columns[i].numerical)
				better[2]++;
		}

		if (better[1] != 0 && better[2] != 0)
			return 0;
	}
	if (better[1] == 0)
		return 2;
	else if (better[2] == 0)
		return 1;
	else
		return 0;
}

// Реализовать программу, которая ищет множество Парето
Pareto_strValueTable Pareto_findMalloc(const Pareto_strValueTable input)
{
	// Поиск проигравших.

	size_t * loosers = (size_t *) malloc(sizeof(size_t) * input.countLines); // лист лузеров.
	if (loosers == NULL)
		return (Pareto_strValueTable){ NULL, NULL, NULL, 0, 0 }; // Не хватило памяти.
	for (size_t i = input.countLines; --i != ~(size_t)0;)
		loosers[i] = ~(size_t)0;
	size_t loo_idx = 0; // идентификатор листа лузеров
	byte change;
	for (size_t x = input.countLines; --x != 0;)
		for (size_t y = x - 1; --y != ~(size_t)0;)
		{
			change = Pareto_isFirstBetter(x, y, input);
			if (change == 255)
			{
				free(loosers);
				return (Pareto_strValueTable) { NULL, NULL, NULL, 0, 0 };
			}
			if (change != 0)
				loosers[loo_idx++] = change == 1 ? y : x;
		}

	Pareto_strValueTable out = Pareto_deleteLinesMalloc(input, loosers, loo_idx);

	free(loosers);

	return out;
}

// Реализует субоптимизацию Парето.
// const Pareto_strValueTable input - Данные.
// size_t idMain - Идентификатор главного критерия.
// double border - Крайнее значение, которое может сществовать.
// Возвращает новый экземпляр таблицы без элементов, которые не подходят границе border по критерию idMain.
Pareto_strValueTable Pareto_optiMalloc(const Pareto_strValueTable input, size_t idMain, double border)
{

	if(idMain >= input.countColumns || input.lines == NULL || input.flags == NULL)
		return (Pareto_strValueTable) { NULL, NULL, NULL, 0, 0 };


	size_t * loosers = (size_t *)malloc(sizeof(size_t) * input.countLines); // лист лузеров.
	if (loosers == NULL)
		return (Pareto_strValueTable) { NULL, NULL, NULL, 0, 0 }; // Не хватило памяти.
	for (size_t i = input.countLines; --i != ~(size_t)0;)
		loosers[i] = ~(size_t)0;
	size_t loo_idx = 0; // идентификатор листа лузеров


	for (size_t i = input.countLines; --i != ~(size_t)0;)
		if ((input.lines[i].columns == NULL) ||
			(input.flags[idMain] && input.lines[i].columns[idMain] < border) ||
			(!input.flags[idMain] && input.lines[i].columns[idMain] > border))
			loosers[loo_idx++] = i;

	Pareto_strValueTable out = Pareto_deleteLinesMalloc(input, loosers, loo_idx);
	free(loosers);
	return out;
}


// Освобождает из памяти таблицу.
// Pareto_strValueTable * input - Указатель на входящую таблицу, которую необходимо освободить.
// Возвращает: код ошибки.
// 1 - строки не найдены.
// 2 - получен input NULL.
// -n - программа сработала хорошо, но были найдены указатели NULL в количестве n.
int Pareto_destructorTableFree(Pareto_strValueTable * input)
{
	size_t flag = 0;
	if (input == NULL)
	{
		return 2;
	}
	if (input->titles != NULL)
	{
		for (size_t i = input->countColumns; --i != ~(size_t)0;)
		{
			if (input->titles[i].str != NULL)
				free(input->titles[i].str);
			else flag++;
		}
		free(input->titles);
	}
	else flag++;
	if (input->lines == NULL)
		return 1;
	if (input->flags != NULL)
		free(input->flags);
	else flag++;
	for (size_t i = 0; i < input->countLines; i++)
	{
		if (input->lines[i].columns != NULL)
		{
			free(input->lines[i].columns);
		}
		else flag++;
		if (input->lines[i].name.str != NULL)
		{
			free(input->lines[i].name.str);
		}
		else flag++;
	}
	free(input->lines);
	input->countColumns = 0;
	input->countLines = 0;
	input->lines = NULL;
	input->titles = NULL;
	input->flags = NULL;
	return flag > (int)flag ? 1 << ((sizeof(int)*8) - 1) : -(int)flag; // Защита от переполнения.
}

// Создание в памяти таблицы Парето
int Pareto_intilizalTableMalloc(Pareto_strValueTable * out, size_t countLines, size_t countColumns, size_t countChars)
{
	Pareto_strValueTable table = {
		malloc(sizeof(*table.titles) * countColumns),
		malloc(sizeof(*table.lines) * countLines), // Указатель на экземпляры
		malloc(sizeof(*table.flags) * countColumns), // Флаги таблицы
		countLines, // Количество экземпляров
		countColumns // Критерии
	};
	if (table.lines == NULL || table.titles == NULL || table.flags == NULL)
	{
		if (table.lines != NULL) free(table.lines);
		if (table.titles != NULL) free(table.titles);
		if (table.flags != NULL) free(table.flags);
		return 1;
	}
	for (size_t i = 0; i < table.countColumns; i++)
	{
		table.titles[i].str = (char *) malloc(sizeof(*table.titles->str)*countChars);
		table.titles[i].length = countChars;

		if(table.titles[i].str == NULL)
		{
			table.titles[i--].length = 0; 
			for(; i != ~(size_t)0; i--)
			{
				free(table.titles[i].str);
			}
			free(table.titles);
			free(table.lines);
			free(table.flags);
			return 4;
		}
	}
	for (size_t i = 0; i < table.countLines; i++)
	{
		table.lines[i].columns = (double *) malloc(sizeof(*table.lines->columns) * table.countColumns);
		table.lines[i].name.str = (char *) malloc(sizeof(*table.lines->name.str) * countChars);
		table.lines[i].name.length = countChars;

		for (size_t j = table.countColumns; --j != ~(size_t)0;)
		{
			table.lines[i].columns[j] = 0;
		}
		if(table.lines[i].name.length > 0)
			table.lines[i].name.str[0] = 0;


		if (table.lines[i].columns == NULL)
		{
			//printf("malloc error [lines[%lu].columns]\n", (unsigned long)i);

			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].name.str);
			}
			free(table.lines);
			free(table.titles);
			free(table.flags);
			return 2;
		}
		if (table.lines[i].name.str == NULL)
		{
			//printf("malloc error [lines[%ul].name.str]\n", (unsigned long)i);
			free(table.lines[i].columns);
			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].name.str);
			}
			free(table.lines);
			free(table.flags);
			free(table.titles);
			return 3;
		}
	}

	for (size_t j = table.countColumns; --j != ~(size_t)0;)
	{
		table.flags[j] = 0;
		if(table.titles[j].length > 0)
			table.titles[j].str[0] = 0;
	}

	*out = table;
	return 0;
}

// Создаёт матрицу сравнений по-умолчанию.
// strValueTable * output - Указатель, куда записать результат.
// Возвращает: код ошибки.
// 1 - не хватило памяти при создании строк таблицы. Память очищается.
// 2 - не хватило памяти для создания. Память очищается.
// 3 - не хватило памяти для создания поля имени. Память очищается.
int Pareto_intilizalDefaultTableMalloc(Pareto_strValueTable * output)
{
	int err = Pareto_intilizalTableMalloc(output, 10, 5, 32);
	if (err != 0) return err;
	
	memcpy(output->titles[0].str, "The weight -", output->titles[0].length);
	memcpy(output->titles[1].str, "Growth -    ", output->titles[1].length);
	memcpy(output->titles[2].str, "Diseases -  ", output->titles[2].length);
	memcpy(output->titles[3].str, "Salary +    ", output->titles[3].length);
	memcpy(output->titles[4].str, "Languages + ", output->titles[4].length);

	output->flags[0] = 0;
	output->flags[1] = 0;
	output->flags[2] = 0;
	output->flags[3] = 1;
	output->flags[4] = 1;

	memcpy(output->lines[0].name.str, "Alyona ", output->lines[0].name.length);
	memcpy(output->lines[1].name.str, "Elena  ", output->lines[1].name.length);
	memcpy(output->lines[2].name.str, "My cat ", output->lines[2].name.length);
	memcpy(output->lines[3].name.str, "Peter  ", output->lines[3].name.length);
	memcpy(output->lines[4].name.str, "Irina  ", output->lines[4].name.length);
	memcpy(output->lines[5].name.str, "Mama   ", output->lines[5].name.length);
	memcpy(output->lines[6].name.str, "Natasha", output->lines[6].name.length);
	memcpy(output->lines[7].name.str, "Galina ", output->lines[7].name.length);
	memcpy(output->lines[8].name.str, "Olga   ", output->lines[8].name.length);
	memcpy(output->lines[9].name.str, "Zinaida", output->lines[9].name.length);

	

	memcpy_s(output->lines[0].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 120.0, 175.0, 3.0, 15000.0, 1.0 }, sizeof(double[5]));
	memcpy_s(output->lines[1].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 70.0, 160.0, 2.0, 4000.0, 2.0 }, sizeof(double[5]));
	memcpy_s(output->lines[2].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 4.0, 20.0, 1.0, -2500.0, 0.0 }, sizeof(double[5]));
	memcpy_s(output->lines[3].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 85.0, 185.0, 1.0, 70000.0, 2.0 }, sizeof(double[5]));
	memcpy_s(output->lines[4].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 75.0, 172.0, 2.0, 50000.0, 2.0 }, sizeof(double[5]));
	memcpy_s(output->lines[5].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 85.0, 170.0, 4.0, 40000.0, 2.0 }, sizeof(double[5]));
	memcpy_s(output->lines[6].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 60.0, 180.0, 2.0, 30000.0, 1.0 }, sizeof(double[5]));
	memcpy_s(output->lines[7].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 75.0, 152.0, 1.0, 65000.0, 2.0 }, sizeof(double[5]));
	memcpy_s(output->lines[8].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 80.0, 151.0, 3.0, 75000.0, 1.0 }, sizeof(double[5]));
	memcpy_s(output->lines[9].columns, sizeof(*output->lines->columns)*output->countColumns, (double[5]) { 74.0, 174.0, 2.0, 23000.0, 2.0 }, sizeof(double[5]));

	return 0;
}

void Pareto_intilizalTableMalloc_free_test(void)
{
	printf("Pareto intilizalTableMalloc and free\tStart test...\n");
	Pareto_strValueTable tst;
	if (Pareto_intilizalTableMalloc(&tst, 5, 6, 10) != 0)
	{
		printf("error to create table.\n");
		return;
	}

	// Тестирование значений таблицы

	if (tst.countColumns != 6)
		printf("error tst.countColumns\n");
	if (tst.countLines != 5)
		printf("error tst.countLines\n");

	// Тестирование пустых указателей таблицы

	if (tst.lines == NULL)
	{
		printf("error: lines NULL\n");
		return;
	}
	if (tst.titles == NULL)
	{
		printf("error: tst.titles NULL\n");
		return;
	}

	// Тестирование экземпляров таблицы

	for (size_t i = tst.countLines; --i != ~(size_t)0u; )
	{
		// Тестирование имён экхемпляров

		if (tst.lines[i].name.length != 10)
		{
			printf("error tst.lines[i].name.length != 10\n");
			return;
		}
		for (size_t ch = 0; ch < 10; ch++)
			tst.lines[i].name.str[ch] = '0' + (char)ch;
		for (size_t ch = 0; ch < 10; ch++)
			if (tst.lines[i].name.str[ch] != '0' + (char)ch)
				printf("error tst.lines[i].name.str[ch] != '0' + (char)ch\n");

		// Тестирование значений экземпляра

		if (tst.lines[i].columns == NULL)
		{
			printf("error tst.lines[i].columns == NULL\n");
			return;
		}

		for (size_t j = tst.countColumns; --j != ~(size_t)0;)
		{
			tst.lines[i].columns[j] = 123;
			if(tst.lines[i].columns[j] != 123)
				printf("error tst.lines[i].columns[j] != 123");
		}
	}

	// Тестирование заголовков таблицы
	for (size_t i = tst.countColumns; --i != ~(size_t)0;)
	{
		if (tst.titles[i].length != 10)
		{
			printf("error tst.titles[i].length != 10\n");
			return;
		}
		for (size_t ch = 0; ch < 10; ch++)
			tst.titles[i].str[ch] = '0' + (char)ch;
		for (size_t ch = 0; ch < 10; ch++)
			if (tst.titles[i].str[ch] != '0' + (char)ch)
				printf("error tst.titles[i].str[ch] != '0' + (char)ch\n");
	}

	Pareto_destructorTableFree(&tst);

	if (tst.countColumns != 0)
		printf("error tst.countColumns != 0");
	if (tst.countLines != 0)
		printf("error tst.countLines != 0");
	if (tst.lines != NULL)
		printf("error tst.lines != NULL");
	if (tst.titles != NULL)
		printf("error tst.titles != NULL");

	printf("Pareto intilizalTableMalloc and free\tFinish test...\n");
}

void Pareto_manyintilizalTableMalloc_free_test(void)
{
	printf("Start many Pareto test...\n");
	Pareto_strValueTable a;
	for (size_t i = 0; i < 1000; i++) {
		Pareto_intilizalTableMalloc(&a, 100, 100, 100);
		Pareto_destructorTableFree(&a);
	}
	printf("Finish manny Pareto test.\n");
}

string Pareto_getListNamesMalloc(const Pareto_strValueTable input)
{
	string out = {NULL, 0};
	for (size_t i = input.countLines; --i != ~(size_t)0;)
		out.length += input.lines[i].name.length;
	out.str = malloc(out.length * sizeof(*out.str));
	if (out.str == NULL)
		return out;
	out.str[0] = 0;
	for(size_t i = 0; i < input.countLines; i++)
		sprintf_s(out.str, out.length, "%s%llu)%s\n", out.str, out.length, (unsigned long long)i, input.lines[i].name.str, input.lines[i].name.length);
	
	return out;
}

string Pareto_getListTitlesMalloc(const Pareto_strValueTable input)
{
	string out = { NULL, 0 };
	for (size_t i = input.countColumns; --i != ~(size_t)0;)
		out.length += input.titles[i].length;
	out.str = malloc(out.length * sizeof(*out.str));
	if (out.str == NULL)
		return out;
	out.str[0] = 0;
	size_t chars = 0;
	for (size_t i = 0; i < input.countColumns; i++)
		chars += sprintf_s(out.str + chars, out.length - chars, "%llu)%s\n", (unsigned long long)i, input.titles[i].str);

	// Оптимизация по освобождению ненужной памяти.
	string outSmall = { malloc((chars + 1) * sizeof(out.str)), chars + 1 };
	memcpy_s(outSmall.str, outSmall.length, out.str, chars + 1);
	free(out.str); out.length = 0; out.str = NULL;

	return outSmall;
}

void manyMallocFree_test(void)
{
	printf("Start many malloc test...\n");
	void * a;
	for (size_t i = 0; i < 100000; i++) {
		a = malloc(1024*4);
		free(a);
	}
	printf("Finish many free test.\n");
}

void Pareto_find_test(void)
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

	memcpy_s(tst.lines[0].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 900.0, 20.0, 60.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[1].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 500.0, 30.0, 20.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[2].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 700.0, 36.0, 40.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[3].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 800.0, 40.0, 50.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[4].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 400.0, 60.0, 15.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[5].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 600.0, 30.0, 10.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[6].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 900.0, 35.0, 60.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[7].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 600.0, 24.0, 10.0 }, sizeof(double[3]));
	memcpy_s(tst.lines[8].columns, sizeof(*tst.lines->columns)*tst.countColumns, (double[3]) { 650.0, 35.0, 40.0 }, sizeof(double[3]));

	Pareto_write(tst);
	Pareto_strValueTable tst2 = Pareto_findMalloc(tst);
	Pareto_write(tst2);

	Pareto_destructorTableFree(&tst);
	Pareto_destructorTableFree(&tst2);

	printf("Pereto find test end...\n");
}

void z1_test(void)
{
	printf("z1\tStart test...\n");
	Pareto_intilizalTableMalloc_free_test();
	manyMallocFree_test();
	Pareto_manyintilizalTableMalloc_free_test();
	Pareto_find_test();
	printf("z1\tFinish test...\n");
}

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
	printf("main?\n");
	size_t chekByUser = ~(size_t)0;
	if (table2.countColumns < (unsigned char)~(unsigned char)0)
		chekByUser = UserInterface_GetChek(titles.str, (unsigned char)table2.countColumns, stdin, stdout);
	else
		chekByUser = UserInterface_GetUnsignedLongLongIntLimit(titles.str, 0, table2.countColumns - 1);
	
	free(titles.str); titles.str = NULL; titles.length = 0;

	Pareto_strValueTable table3 = Pareto_optiMalloc(table2, chekByUser, UserInterface_GetFloat("border = "));
	Pareto_destructorTableFree(&table2);
	Pareto_write(table3);
	Pareto_destructorTableFree(&table3);

}

void main()
{
	z1_test();
	z1_interface();

	UserInterface_Pause("Press any key...\n");

}

