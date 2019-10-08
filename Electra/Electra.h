#pragma once

#ifndef _INC_STDLIB
#include <stdlib.h>
#endif // !_INC_STDLIB
#include "String.h"
#include "UserInterface.h"



#pragma region Критерии

extern struct Electra_key;

typedef struct {
	struct Electra_key * keys;	// Ключи.
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
} Electra_criterion; // Один критерий Электра

// Функция инцилизации критериев таблицы Электра.
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
				String_destructorFree(&out[i].name);
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
				String_destructorFree(&out[i].name);
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
int Electra_addKeyMalloc(Electra_scale * edit, Electra_key newKey)
{
	if (edit == NULL)
		return 1;
	if (edit->keys == NULL)
	{
		edit->keys = (Electra_key*)malloc(1 * sizeof(Electra_key));
		edit->length = 1;
		if (edit->keys == NULL)
		{
			edit->length = 0;
			return 2;
		}
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

// Освобождает память от всех ресурсов, связанной со шкалой.
// scale - Указатель на шкалу, которую надо освободить.
// Возвращает: код ошибки.
int Electra_scaleFree(Electra_scale * scale)
{
	if (scale == NULL)
		return 1;
	if (scale->keys == NULL)
	{
		scale->length = 0;
		return 0;
	}
	for (size_t i = scale->length; --i != ~(size_t)0;)
	{
		String_destructorFree(&scale->keys[i].name);
		scale->keys[i].value = 0;
	}
	free(scale->keys);
	*scale = { NULL, 0 };
	return 0;
}

#pragma endregion

#pragma region Экземпляры (строки) - структуры

typedef struct Electra_key {
	string name;	// Именное значение ключа.
	double value;	// Численное значение ключа.
} Electra_key;

// Символизирует строку таблицы: название экземпляра и указатель на начало его параметров.
typedef struct {
	string name;						// Имя экземпляра.
	const Electra_key ** columns;		// Массив указателей на значения, символизирующие из себя имя значения и его численное представление.
	const size_t * countColumns;	// Указатель на Electra_strValueTable.countColumns (количество критериев). 
} Electra_strValues;

#pragma endregion

#pragma region Таблица

// Структура, которая хранит в себе указатель на начало списка экземпляров, количество строк и количество столбцов.
typedef struct {
	Electra_criterion * const criteria;	// Указатель на критерии таблицы.
	size_t const countColumns;			// Количество критериев экземпляров в таблице.

	Electra_strValues * lines;			// Представляет собой строку с именем экземпляра и его значениями.
	size_t countLines;					// Количество экземпляров в таблице.
} Electra_strValueTable;

// Создаёт экземпляр в оперативной памяти для таблицы Электра.
// output - указатель, куда надо поместить результат.
// countColumns - количество критериев.
// countCharsCriterion - количество символов, выделяемые для критериев и ключей
// int * err - указатель, куда поместить код ошибки. В случае ошибки память очищается, а output не изменяется.
// 1 - Не хватило памяти для первичной инцилизации.
// 2 - Не верны входные данные.
// 3 - Ошибка в Electra_intilizalCriterionsMalloc
// Возвращает: экземпляр таблицы Электра.
Electra_strValueTable Electra_intilizalTableMalloc(size_t countColumns, size_t countCharsCriterion, int * errOut)
{
	Electra_strValueTable out = {
		(Electra_criterion*)malloc(sizeof(Electra_criterion)*countColumns),
		countColumns,
		(Electra_strValues*)malloc(sizeof(Electra_strValues) * 0),
		0
	};
	if (out.criteria == NULL)
	{
		if (out.criteria != NULL)
			free(out.criteria);
		if (out.lines != NULL)
			free(out.lines);
		if (errOut != NULL)
			*errOut = 1;
		return { 0 };
	}
	int err = Electra_intilizalCriterionsMalloc(out.criteria, out.countColumns, countCharsCriterion);
	if (err != 0)
	{
		free(out.lines);
		free(out.criteria);
		if (errOut != NULL)
			*errOut = 3;
		return { 0 };
	}
	return out;
}

// Добавление строки в таблицу. 
int Electra_addMalloc(Electra_strValueTable * edit, string name, Electra_key ** const columns)
{
	if (edit == NULL)
		return 1;
	if (edit->lines == NULL)
	{
		edit->lines = (Electra_strValues*)malloc(1 * sizeof(Electra_strValues));
		edit->countLines = 1;
		if (edit->lines == NULL)
		{
			edit->countLines = 0;
			return 2;
		}
	}
	else
	{
		Electra_strValues * newLines = (Electra_strValues *)realloc(edit->lines, (edit->countLines + 1) * sizeof(Electra_strValues));
		if (newLines == NULL)
			return 2;
		edit->lines = newLines;
		edit->countLines++;
	}
	edit->lines[edit->countLines - 1] = { name, columns, &edit->countColumns };
}

// Освобождает оперативную память от критериев и строк таблицы edit. 
// Стоит отметить, что 
// Electra_strValueTable * edit - входящая таблица.
// Возвращает: код ошибки.
int Electra_free(Electra_strValueTable * edit)
{
	if (edit == NULL)
		return 0;
	if (edit->lines != NULL)
	{
		for (size_t i = edit->countLines; --i != ~(size_t)0;)
		{
			if (edit->lines[i].columns != NULL)
				free(edit->lines[i].columns);
			String_destructorFree(&edit->lines[i].name);
		}
		free(edit->lines);
	}
	edit->countLines = 0;
	if (edit->criteria != NULL)
	{
		for (size_t i = edit->countColumns; --i != ~(size_t)0;)
		{
			String_destructorFree(&edit->criteria[i].name);
			edit->criteria[i].isToMax = 0;
			edit->criteria[i].weight = 0;
			Electra_scaleFree(&edit->criteria[i].scale_keys);
		}
		free(edit->criteria);
	}
	return 0;
}

#pragma region Консольный интерфейс таблицы

// Возвращает: код ошибки.
// 1 - Не хватило памяти для массива критериев.
// 2 - Не удалось добавить строку в таблицу.
int Electra_UAddMalloc(Electra_strValueTable * edit)
{
	// Нужна память на список колонок.
	Electra_key ** columns = (Electra_key **)malloc(edit->countColumns * sizeof(Electra_key*));
	if (columns == NULL)
		return 1;

	// Нужно назвать экземпляр.
	string lineName = { NULL, 0 };
	{
		char nameBuffer[1024 * 10];
		do {
			lineName.length = UserInterface_GetStr("name for new line: ", nameBuffer, sizeof(nameBuffer));
			lineName.str = (char*)malloc(lineName.length * sizeof(char));
		} while (lineName.str == NULL);
	} // clear nameBuffer.

	// Нужно присвоить колонкам значения.
	for (size_t i = edit->countColumns; --i != ~(size_t)0; )
	{ // Нужно задать значение каждой колонке.
		printf(edit->criteria[i].isToMax ? "max\n" : "min\n"); // Вывести направление.
		printf("criterion: %s, %lf\n", edit->criteria[i].name.str, edit->criteria[i].weight); // Вывести название криетрия и его вес.
		for (size_t j = edit->criteria[i].scale_keys.length; --j != ~(size_t)0; )
		{ // Вывести обозначение каждого критерия.
			printf("[key %llu] v: %lf, n: %s\n", (long long unsigned)j, edit->criteria[i].scale_keys.keys[j].value, edit->criteria[i].scale_keys.keys[j].name);
		}
		// Спросим каждый критерий.
		size_t select = UserInterface_GetUnsignedLongLongIntLimit("Select key: ", 0, edit->criteria[i].scale_keys.length - 1);
		columns[i] = &edit->criteria[i].scale_keys.keys[select]; // Говорим, что именно этот критерий - наш.
	}

	if (Electra_addMalloc(edit, lineName, columns) != 0)
	{
		free(columns);
		String_destructorFree(&lineName);
		return 2;
	}
	return 0;
}

Electra_strValueTable Electra_getDefault() {
	int err;
	Electra_strValueTable out = Electra_intilizalTableMalloc(5, 13, &err);
	if (err != 0)
		return out;

	out.criteria[0] = { String_CopyFromCharMalloc("The weight"), 4, {NULL, 0}, 0 };
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Obesity"), 250 }); // Ожирение
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Very heavy"), 150 }); // Очень тяжёлая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Heavy"), 100 }); // Тяжёлая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("A bit heavy"), 95 }); // Тяжёленькая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Normal weight"), 75 }); // Нормальный вес
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Almost light"), 65 }); // Лёгенькая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Lightweight"), 60 }); // Лёгкая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Too light weight"), 50 }); // Слишком лёгкая
	Electra_addKeyMalloc(&out.criteria[0].scale_keys, { String_CopyFromCharMalloc("Health hazard"), 45 }); // Угроза здоровью
	out.criteria[1] = { String_CopyFromCharMalloc("Growth"), 1, {NULL, 0}, 0 };
	Electra_addKeyMalloc(&out.criteria[1].scale_keys, { String_CopyFromCharMalloc("Tall"), 200 }); // Высокая
	Electra_addKeyMalloc(&out.criteria[1].scale_keys, { String_CopyFromCharMalloc("Middle"), 170 }); // Средняя
	Electra_addKeyMalloc(&out.criteria[1].scale_keys, { String_CopyFromCharMalloc("Little"), 150 }); // Маленькая
	Electra_addKeyMalloc(&out.criteria[1].scale_keys, { String_CopyFromCharMalloc("Very little"), 100 }); // Очень маленькая
	out.criteria[2] = { String_CopyFromCharMalloc("Diseases"), 5, {NULL, 0}, 0 };
	Electra_addKeyMalloc(&out.criteria[2].scale_keys, { String_CopyFromCharMalloc("Infectious"), 5 }); // 5 болезней
	Electra_addKeyMalloc(&out.criteria[2].scale_keys, { String_CopyFromCharMalloc("Chronic diseases"), 3 }); // 3 болезней
	Electra_addKeyMalloc(&out.criteria[2].scale_keys, { String_CopyFromCharMalloc("Healthy"), 1 }); // Вполне в норме
	out.criteria[3] = { String_CopyFromCharMalloc("Salary"), 2, {NULL, 0}, 1 };
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("May buy an apartment"), 1000000 }); // Может сама накопить на квартиру
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("Buys equipment"), 70000 }); // Покупает технику
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("Saves on equipment"), 50000 }); // Экономит на технику
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("Eats full"), 30000 }); // Не отказывает себе в питании
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("Saves on food"), 15000 }); // Экономит на еду
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("Lives at the expense of others"), 5000 }); // Живёт за чужой счёт
	Electra_addKeyMalloc(&out.criteria[3].scale_keys, { String_CopyFromCharMalloc("No income"), 0 }); // Нет заработка
	out.criteria[4] = { String_CopyFromCharMalloc("Languages"), 3, {NULL, 0}, 1 };
	Electra_addKeyMalloc(&out.criteria[4].scale_keys, { String_CopyFromCharMalloc("Linguist"), 5 }); // Лингвистка.
	Electra_addKeyMalloc(&out.criteria[4].scale_keys, { String_CopyFromCharMalloc("Language expert"), 3 }); // Знаток.
	Electra_addKeyMalloc(&out.criteria[4].scale_keys, { String_CopyFromCharMalloc("Knows two languages"), 2 }); // Знает два языка.
	Electra_addKeyMalloc(&out.criteria[4].scale_keys, { String_CopyFromCharMalloc("Knows only your language"), 1 }); // Знает только свой язык.
	// TODO
}

#pragma endregion

#pragma endregion