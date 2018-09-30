// Реализовать программу, которая реализует субоптимизацию

#include "UserInterface.h"
#include <stdlib.h>

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

// Символизирует строку таблицы: указатель на начало параметров экземпляра.
typedef struct {
	doubleFlag * columns;
} Pareto_strValues;

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	string * titles; // Указатель на заголовки таблицы.
	Pareto_strValues * lines; // Представляет собой строку с именем экземпляра и его значениями.
	size_t countLines; // Количество экземпляров в таблице.
	size_t countColumns; // Количество критериев экземпляров в таблице.
} Pareto_strValueTable;

// Отвечает на вопрос, кто по Парето лучше?
// first - первый представитель.
// second - второй представитель.
// size_t countColumns - количество характеристик представителей.
// Возвращает: 1, если первый лучше второго. 0 - если нельяз сравнить. 2 - второй лучше первого. 255 - ошибка.
byte Pareto_isFirstBetter(Pareto_strValues first, Pareto_strValues second, size_t countColumns)
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

	size_t countFind = 0; // Количество найденных

	for (size_t i = input.countLines; --i != ~(size_t)0;)
		if (loosers[i] != ~(size_t)0)
			countFind++;

	// Выборка проигравших. TODO

	Pareto_strValueTable out;

	size_t countStr = 0; // Количество необходимых символов.
	for(size_t i = input.countColumns; --i != ~(size_t)0;)
		input.titles

	if(Pareto_intilizalTableMalloc(&out, countFind, input.countColumns, ) != 0)

	


	free(loosers);
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
			}
			free(table.lines);
			return 2;
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

