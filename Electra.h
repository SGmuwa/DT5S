#pragma once

#ifndef _INC_STDLIB
#include <stdlib.h>
#endif // !_INC_STDLIB
#include "String.h"

// Символизирует строку таблицы: название экземпляра и указатель на начало его параметров.
typedef struct {
	string name;						// Имя экземпляра.
	double * columns;					// Значения по критериям экземпляра.
	size_t const * const countColumns;	// Указатель на Electra_strValueTable.countColumns. 
} Electra_strValues;

typedef struct {
	string name;	// Именное значение ключа.
	double value;	// Численное значение ключа.
} Electra_key;

typedef struct {
	Electra_key * keys;	// Ключи.
	size_t length;		// Количество ключей.
} Electra_scale; // Шкала Электра

typedef struct {
	// Название критерия.
	string name;
	// Численный вес критерия.
	double weight;
	// Свойства шкалы.
	Electra_scale scale_keys;
	// Истина, если считается чем больше значение по этому критерию, тем лучше.
	unsigned char isToMax;
} Electra_criterion;

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	Electra_criterion * criteria;	// Указатель на критерии таблицы.
	size_t countColumns;			// Количество критериев экземпляров в таблице.

	Electra_strValues * lines;		// Представляет собой строку с именем экземпляра и его значениями.
	size_t countLines;				// Количество экземпляров в таблице.
} Electra_strValueTable;

int Electra_intilizalCriterions(Electra_criterion * out, size_t countColumns, size_t countCharsCriterion)
{
	if (out == NULL)
		return 1;
	for (size_t i = countColumns; --i != ~(size_t)0; )
	{
		out[i].isToMax = -1;
		out[i].name = String_intilizalStringMalloc(countCharsCriterion);
		if (out[i].name.str == NULL)
		{
			while (++i != countColumns)
			{
				out[i].
			}
		}
	}
}

// Создаёт экземпляр в оперативной памяти для таблицы Электра.
// output - указатель, куда надо поместить результат.
// countLines - количество экземпляров.
// countColumns - количество критериев.
// countCharsCriterion - количество символов, выделяемые для критериев и ключей
// countCharsValues - количество символов, выделяемые для экземпляров.
// Возвращает: код ошибки. В случае ошибки память очищается, а output не изменяется.
// 1 - Не хватило памяти для первичной инцилизации.
// 2 - Не верны входные данные.
int Electra_intilizalTableMalloc(Electra_strValueTable * output, size_t countLines, size_t countColumns, size_t countCharsCriterion, size_t countCharsValues)
{
	if (output == NULL)
		return 2;
	Electra_strValueTable out = {
		(Electra_criterion*)malloc(sizeof(Electra_criterion)*countColumns),
		countColumns,
		(Electra_strValues*)malloc(sizeof(Electra_strValues)*countLines),
		countLines
	};
	if (out.criteria == NULL || out.lines == NULL)
	{
		if (out.criteria != NULL)
			free(out.criteria);
		if (out.lines != NULL)
			free(out.lines);
		return 1;
	}
}