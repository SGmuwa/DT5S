// Реализовать программу, которая реализует субоптимизацию

#include "UserInterface.h"
#include <stdlib.h>
#include <string.h>

typedef unsigned char byte;

// Представляет из себя тип данных, который хранит в себе double значение и флаг.
typedef struct {
	double numerical; // Численное значение.
	byte flag; // Если 0 - значит, лучше отрицательное значение. Если 1 - лучше положительные значения.
} doubleFlag;

// Структура обозначает массив строк.
typedef struct {
	char * str; // Указатель на первый символ строки.
	size_t length; // Количество доступных символов.
} string;

// Символизирует строку таблицы: название экземпляра и указатель на начало его параметров.
typedef struct {
	string name;
	doubleFlag * columns;
} Pareto_strValues;

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	string * titles;			// Указатель на заголовки таблицы.
	Pareto_strValues * lines;	// Представляет собой строку с именем экземпляра и его значениями.
	size_t countLines;			// Количество экземпляров в таблице.
	size_t countColumns;		// Количество критериев экземпляров в таблице.
} Pareto_strValueTable;

int Pareto_write(const Pareto_strValueTable input)
{
	for (size_t i = 0; i < input.countColumns; i++)
		printf("%s\t|", input.titles[i]);
	printf("\n");
	for (size_t i = 0; i < input.countLines; i++)
	{
		for (size_t j = 0; j < input.countColumns; j++)
		{
			printf("%f\t|", input.lines[i].columns[j].numerical);
		}
		printf("\n");
	}
}

// Отвечает на вопрос, кто по Парето лучше?
// first - первый представитель.
// second - второй представитель.
// size_t countColumns - количество характеристик представителей.
// Возвращает: 1, если первый лучше второго. 0 - если нельяз сравнить. 2 - второй лучше первого. 255 - ошибка.
byte Pareto_isFirstBetter(const Pareto_strValues first, const Pareto_strValues second, size_t countColumns)
{
	size_t better[3] = {
		0, // ничья
		0, // первый лучше
		0  // второй лучше
	};
	for (size_t i = countColumns - 1; i != ~(size_t)0; --i)
	{
		if (first.columns[i].flag != first.columns[i].flag)
			return -1;

		if (first.columns[i].numerical == second.columns[i].numerical)
			better[0]++; // Если равны, то ничья.

		else if (first.columns[i].flag) // Стримится к положительным числам.
		{
			if (first.columns[i].numerical > second.columns[i].numerical)
				better[1]++;
			else//if(first.columns[i].numerical <  second.columns[i].numerical)
				better[2]++;
		}
		else // Стримится к отрицательным числам.
		{
			if (first.columns[i].numerical < second.columns[i].numerical)
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
Pareto_strValueTable Pareto_find(Pareto_strValueTable input)
{
	// Поиск проигравших.

	size_t * loosers = (size_t *) malloc(sizeof(size_t) * input.countLines); // лист лузеров.
	if (loosers == NULL)
		return (Pareto_strValueTable){ NULL, NULL, 0, 0 }; // Не хватило памяти.
	for (size_t i = input.countLines; --i != ~(size_t)0;)
		loosers[i] = ~(size_t)0;
	size_t loo_idx = 0; // идентификатор листа лузеров
	byte change;
	for (size_t x = input.countLines; --x != 0;)
		for (size_t y = x - 1; y != ~(size_t)0;)
		{
			change = Pareto_isFirstBetter(input.lines[x], input.lines[y], input.countColumns);
			if (change == 255)
			{
				free(loosers);
				return (Pareto_strValueTable) { NULL, NULL, 0, 0 };
			}
			if (change != 0)
				loosers[loo_idx++] = change == 1 ? y : x;
		}

	// Удаление одинаковых чисел из loosers.
	for (size_t i = input.countLines; --i != 0;)
		if (loosers[i] == ~(size_t)0)
			continue;
		else for (size_t j = i; --j != ~(size_t)0;)
			if (loosers[i] == loosers[j])
				loosers[i] = ~(size_t)0;

	size_t countLoosers = 0; // Количество найденных

	for (size_t i = input.countLines; --i != ~(size_t)0;)
		if (loosers[i] != ~(size_t)0)
			countLoosers++;

	Pareto_strValueTable out =
	{
		NULL, // Указатель на заголовки таблицы.
		NULL, // Представляет собой строку с именем экземпляра и его значениями.
		0,	  // Количество экземпляров в таблице.
		0	  // Количество критериев экземпляров в таблице.
	};

	size_t countMaybeWinners = input.countLines - countLoosers;
	size_t * maybeWinners = (size_t *) malloc(countMaybeWinners * sizeof(size_t));

	if (maybeWinners == NULL)
	{
		free(loosers);
		return out;
	}

	for (size_t i = input.countLines, iMay = 0; --i != ~(size_t)0;)
	{
		size_t j = input.countLines;
		while(--j != ~(size_t)0)
			if (loosers[j] == i) // Лузер найден!
			{
				// Надо не дать лузеру дойти до входа!
				break;
			}
		if (j == ~(size_t)0)
			maybeWinners[iMay++] = i;
	}

	free(loosers);

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

	for(size_t i = out.countColumns; --i != ~(size_t)0;)
		memcpy_s(out.titles + i, maxCountStr, input.titles + i, input.titles[i].length);
	
	// Выборка: оставить тех, кто выиграл.

	for (size_t i = out.countLines; --i != ~(size_t)0;)
	{
		for (size_t j = out.countColumns; --j != ~(size_t)0;)
			out.lines[i].columns[j] = input.lines[maybeWinners[i]].columns[j];
		memcpy_s(out.lines[i].name.str, out.lines[i].name.length, input.lines[maybeWinners[i]].name.str, input.lines[maybeWinners[i]].name.length);
	}
	free(maybeWinners);
	return out;
}

// Освобождает из памяти таблицу.
// strValueTable input - входящая таблица, которую необходимо освободить.
// Возвращает: код ошибки.
// 1 - строки не найдены.
// 2 - отправлен NULL.
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
		}
		free(input->titles);
	}
	if (input->lines == NULL)
		return 1;
	for (size_t i = 0; i < input->countLines; i++)
	{
		if (input->lines[i].columns != NULL)
		{
			free(input->lines[i].columns);
			flag++;
		}
		if (input->lines[i].name.str != NULL)
		{
			flag++;
			free(input->lines[i].name.str);
		}
	}
	free(input->lines);
	input->countColumns = 0;
	input->countLines = 0;
	input->lines = NULL;
	input->titles = NULL;
	return flag > (int)flag ? 1 << ((sizeof(int)*8) - 1) : -(int)flag; // Защита от переполнения.
}

// Создание в памяти таблицы Парето
int Pareto_intilizalTableMalloc(Pareto_strValueTable * out, size_t countLines, size_t countColumns, size_t countChars)
{
	Pareto_strValueTable table = {
		(string *) malloc(sizeof(string) * countColumns),
		(Pareto_strValues *) malloc(sizeof(Pareto_strValues) * countLines), // Указатель на экземпляры
		countLines, // Количество экземпляров
		countColumns // Критерии
	};
	if (table.lines == NULL || table.titles == NULL)
	{
		printf("malloc error [lines].\n");
		return 1;
	}
	for (size_t i = 0; i < table.countColumns; i++)
	{
		table.titles[i].str = (char*) malloc(sizeof(char)*countChars);
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
			return 4;
		}
	}
	for (size_t i = 0; i < table.countLines; i++)
	{
		table.lines[i].columns = (doubleFlag*)malloc(sizeof(doubleFlag) * table.countColumns);
		if (table.lines[i].columns == NULL)
		{
			printf("malloc error [lines[%lu].columns]\n", (unsigned long)i);

			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].name.str);
			}
			free(table.lines);
			return 2;
		}
		table.lines[i].name.str = (char *)malloc(sizeof(char) * countChars);
		table.lines[i].name.length = countChars;
		if (table.lines[i].name.str == NULL)
		{
			printf("malloc error [lines[%ul].name.str]\n", (unsigned long)i);
			free(table.lines[i].columns);
			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].name.str);
			}
			free(table.lines);
			return 3;
		}
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
	return Pareto_intilizalTableMalloc(output, 10, 5, 128);
}

void Pareto_intilizalTableMalloc_free_test(void)
{
	printf("Pareto intilizalTableMalloc and free\tStart test...\n");
	Pareto_strValueTable testing;
	if (Pareto_intilizalTableMalloc(&testing, 5, 6, 10) != 0)
	{
		printf("error to create table.\n");
		return;
	}

	// Тестирование значений таблицы

	if (testing.countColumns != 6)
		printf("error testing.countColumns\n");
	if (testing.countLines != 5)
		printf("error testing.countLines\n");

	// Тестирование пустых указателей таблицы

	if (testing.lines == NULL)
	{
		printf("error: lines NULL\n");
		return;
	}
	if (testing.titles == NULL)
	{
		printf("error: testing.titles NULL\n");
		return;
	}

	// Тестирование экземпляров таблицы

	for (size_t i = testing.countLines; --i != ~(size_t)0u; )
	{
		// Тестирование имён экхемпляров

		if (testing.lines[i].name.length != 10)
		{
			printf("error testing.lines[i].name.length != 10\n");
			return;
		}
		for (size_t ch = 0; ch < 10; ch++)
			testing.lines[i].name.str[ch] = '0' + (char)ch;
		for (size_t ch = 0; ch < 10; ch++)
			if (testing.lines[i].name.str[ch] != '0' + (char)ch)
				printf("error testing.lines[i].name.str[ch] != '0' + (char)ch\n");

		// Тестирование значений экземпляра

		if (testing.lines[i].columns == NULL)
		{
			printf("error testing.lines[i].columns == NULL\n");
			return;
		}

		for (size_t j = testing.countColumns; --j != ~(size_t)0;)
		{
			testing.lines[i].columns[j].flag = 0;
			testing.lines[i].columns[j].numerical = 123;
			if (testing.lines[i].columns[j].flag != 0)
				printf("error testing.lines[i].columns[j].flag != 0");
			if(testing.lines[i].columns[j].numerical != 123)
				printf("error testing.lines[i].columns[j].numerical != 123");
		}
	}

	// Тестирование заголовков таблицы
	for (size_t i = testing.countColumns; --i != ~(size_t)0;)
	{
		if (testing.titles[i].length != 10)
		{
			printf("error testing.titles[i].length != 10\n");
			return;
		}
		for (size_t ch = 0; ch < 10; ch++)
			testing.titles[i].str[ch] = '0' + (char)ch;
		for (size_t ch = 0; ch < 10; ch++)
			if (testing.titles[i].str[ch] != '0' + (char)ch)
				printf("error testing.titles[i].str[ch] != '0' + (char)ch\n");
	}

	Pareto_destructorTableFree(&testing);

	if (testing.countColumns != 0)
		printf("error testing.countColumns != 0");
	if (testing.countLines != 0)
		printf("error testing.countLines != 0");
	if (testing.lines != NULL)
		printf("error testing.lines != NULL");
	if (testing.titles != NULL)
		printf("error testing.titles != NULL");

	printf("Pareto intilizalTableMalloc and free\tFinish test...\n");
}

void Pareto_manyintilizalTableMalloc_free_test(void)
{
	printf("Start many Pareto test...\n");
	Pareto_strValueTable a;
	for (size_t i = 0; i < 10000; i++) {
		Pareto_intilizalTableMalloc(&a, 100, 100, 100);
		Pareto_destructorTableFree(&a);
	}
	printf("Finish manny Pareto test.\n");
}

void manyMallocFree_test(void)
{
	printf("Start many malloc test...\n");
	void * a;
	for (size_t i = 0; i < 4000000; i++) {
		a = malloc(1024*4);
		free(a);
	}
	printf("Finish many free test.\n");
}

void Pareto_find_test(void)
{
	printf("Pareto find test start...\n");
	Pareto_strValueTable testing;
	if (Pareto_intilizalTableMalloc(&testing, lines, columns, chars) != 0)
	{
		printf("Pareto malloc error in Pareto_find.");
		return;
	}
	printf("Pereto find test end...\n");
}

void z1_test(void)
{
	printf("z1\tStart test...\n");
	Pareto_intilizalTableMalloc_free_test();
	manyMallocFree_test();
	Pareto_manyintilizalTableMalloc_free_test();
	printf("z1\tFinish test...\n");
}

void z1_interface(void)
{

}

void main()
{
	z1_test();
	z1_interface();

	UserInterface_Pause("Press any key...\n");

}

