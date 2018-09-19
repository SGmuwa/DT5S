// Реализовать программу, которая реализует субоптимизацию

#include "UserInterface.h"

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
	string str;
	doubleFlag * columns;
} strValues;

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	string * title; // Указатель на заголовки таблицы.
	strValues * lines; // Представляет собой строку с именем экземпляра и его значениями.
	size_t countLines; // Количество экземпляров в таблице.
	size_t countColumns; // Количество критериев экземпляров в таблице.
} strValueTable;

// Отвечает на вопрос, кто по Парето лучше?
// first - первый представитель.
// second - второй представитель.
// Возвращает: 1, если первый лучше второго. 0 - если второй лучше первого. 2 - если нельзя сравнить.
byte pareto_isFirstBetter(strValues first, strValues second)
{

}

// Реализовать программу, которая ищет множество Парето
strValueTable pareto_find(strValueTable input)
{

}

// Освобождает из памяти таблицу.
// strValueTable input - входящая таблица, которую необходимо освободить.
// Возвращает: код ошибки.
// 1 - строки не найдены.
// -n - программа сработала хорошо, но были найдены указатели NULL в количестве n.
int destructorTableFree(strValueTable input)
{
	size_t flag = 0;
	if (input.lines == NULL)
		return 1;
	for (size_t i = 0; i < input.countLines; i++)
	{
		if (input.lines[i].columns != NULL)
		{
			free(input.lines[i].columns);
			flag++;
		}
		if (input.lines[i].str.str != NULL)
		{
			flag++;
			free(input.lines[i].str.str);
		}
	}
	return flag > (int)flag ? 1 << (sizeof(int)*8) - 1 : -(int)flag; // Защита от переполнения.
}

int intilizalTableMalloc(size_t countLines, size_t countColumns, size_t countChars)
{
	strValueTable table = {
		(string *) malloc(sizeof(string) * countColumns)
		(strValues *) malloc(sizeof(strValues) * countLines), // Указатель на экземпляры
		countLines, // Количество экземпляров
		countColumns // Критерии
	};
	if (table.lines == NULL || table.titles == NULL)
	{
		printf("malloc error [lines].\n");
		return 1;
	}
	for (size_t i = 0; i < table.countLines; i++)
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
		}
	}
	for (size_t i = 0; i < table.countLines; i++)
	{
		table.lines[i].columns = (doubleFlag*)malloc(sizeof(doubleFlag) * table.countColumns);
		if (table.lines[i].columns == NULL)
		{
			printf("malloc error [lines[%d].columns]\n", i);

			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].str.str);
			}
			free(table.lines);
			return 2;
		}
		table.lines[i].str.str = (char *)malloc(sizeof(char) * countChars);
		if (table.lines[i].str.str == NULL)
		{
			printf("malloc error [lines[%d].str.str]\n", i);
			free(table.lines[i].columns);
			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].str.str);
			}
			free(table.lines);
			return 3;
		}
	}
	return 0;
}

// Создаёт матрицу сравнений по-умолчанию.
// strValueTable * output - Указатель, куда записать результат.
// Возвращает: код ошибки.
// 1 - не хватило памяти при создании строк таблицы. Память очищается.
// 2 - не хватило памяти для создания. Память очищается.
// 3 - не хватило памяти для создания поля имени. Память очищается.
int intilizalDefaultTableMalloc(strValueTable * output)
{
	strValueTable table = {
		(strValues *)malloc(sizeof(strValues) * 10), // Указатель на экземпляры
		10, // Количество экземпляров
		5 // Критерии
	};
	if (table.lines == NULL)
	{
		printf("malloc error [lines].\n");
		return 1;
	}
	for (size_t i = 0; i < table.countLines; i++)
	{
		table.lines[i].columns = (doubleFlag*)malloc(sizeof(doubleFlag) * table.countColumns);
		if (table.lines[i].columns == NULL)
		{
			printf("malloc error [lines[%d].columns]\n", i);

			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].str.str);
			}
			free(table.lines);
			return 2;
		}
		table.lines[i].str.str = (char *)malloc(sizeof(char) * 10);
		if (table.lines[i].str.str == NULL)
		{
			printf("malloc error [lines[%d].str.str]\n", i);
			free(table.lines[i].columns);
			for (size_t ii = i - 1; ii != ~(size_t)0; ii--)
			{
				free(table.lines[ii].columns);
				free(table.lines[ii].str.str);
			}
			free(table.lines);
			return 3;
		}
	}
}

void intilizalTableMalloc_free_test(void)
{
	printf("intilizalTableMalloc and free\tStart test...\n");

	printf("intilizalTableMalloc and free\tFinish test...\n");
}

void z1_test(void)
{
	printf("z1\tStart test...\n");
	
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

