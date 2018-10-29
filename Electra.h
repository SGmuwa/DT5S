#pragma once

#ifndef _INC_STDLIB
#include <stdlib.h>
#endif // !_INC_STDLIB
#include "String.h"
#include "UserInterface.h"

// Символизирует строку таблицы: название экземпляра и указатель на начало его параметров.
typedef struct {
	string name;						// Имя экземпляра.
	Electra_key ** columns;				// Массив указателей на значения, символизирующие из себя имя значения и его численное представление.
	size_t const * const countColumns;	// Указатель на Electra_strValueTable.countColumns (количество критериев). 
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

// Функция инцилизации скритериев таблицы Электра.
// Electra_criterion * out - Указатель на критерии Электра, доступные для записи.
// size_t countColumns - Количество критериев.
// size_t countCharsCriterion - Количество символов для названий критериев.
// Возвращает код ошибки. В случае возникновения ошибки память освобождается обратно.
// 1 - Аргумент out равен NULL.
// 2 - Не хватило памяти на выделение места для строки названия.
int Electra_intilizalCriterionsMalloc(Electra_criterion * out, size_t countColumns, size_t countCharsCriterion)
{
	if (out == NULL)
		return 1;
	for (size_t i = countColumns; --i != ~(size_t)0; )
	{
		out[i].isToMax = -1;
		out[i].weight = (double)UserInterface_getNaNFloat();
		out[i].name = String_intilizalStringMalloc(countCharsCriterion);
		if (out[i].name.str == NULL)
		{
			while (++i != countColumns)
			{
				String_destructorFree(out[i].name);
			}
			return 2;
		}
	}
	for (size_t i = countColumns; --i != ~(size_t)0; )
	{
		out[i].scale_keys.keys = (Electra_key*)malloc(0 * sizeof(Electra_key));
		out[i].scale_keys.length = 0;
		/*if (out[i].scale_keys.keys == NULL)
		{
			while (++i != countColumns)
			{
				free(out[i].scale_keys.keys);
			}
			i = -1;
			while (++i != countColumns)
			{
				String_destructorFree(out[i].name);
			}
			return 3;
		}*/
	}
	return 0;
}

// Добавляет к существующей шкале ключ.
// Electra_scale * edit - Указатель на шкалу, к которой необходимо добавить ключ.
// Electra_key newKey - Ключ, который необходимо добавить к шкале.
// Возвращает код ошибки.
// 1 - Отправлен edit NULL.
// 2 - Неудалось выделить дополнительную память.
int Electra_addKey(Electra_scale * edit, Electra_key newKey)
{
	if (edit == NULL)
		return 1;
	if (edit->keys == NULL)
	{
		edit->keys = (Electra_key*)malloc(1 * sizeof(Electra_key));
		edit->length = 1;
		if (edit->keys == NULL)
			return 2;
	}
	else
	{
		Electra_key * newKeys = (Electra_key *)realloc(edit->keys, (edit->length + 1) * sizeof(Electra_key));
		if (newKeys == NULL)
			return 2;
		edit->keys = newKeys;
		edit->length++;
	}
	edit->keys[edit->length - 1] = newKey;
	return 0;
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
	// TODO: доделать.
}