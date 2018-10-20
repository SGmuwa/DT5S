#pragma once

#ifndef _INC_STRING
#include <string.h>
#endif // !_INC_STRING

// Структура обозначает массив символов. Количество символов и указатель менять запрещено.
typedef struct {
	char * str; // Указатель на первый символ строки.
	size_t length; // Количество доступных символов.
} string;

// Структура обозначает массив двухбайтных символов. Количество символов и указатель менять запрещено.
typedef struct {
	wchar_t * str; // Указатель на первый символ строки.
	size_t length; // Количество доступных символов.
} wstring;

string String_intilizalStringMalloc(size_t length)
{
	string output = {
		malloc(sizeof(char)*length),
		length
	};
	return output;
}

wstring String_intilizalWstringMalloc(size_t length)
{
	wstring output = {
		malloc(sizeof(wchar_t)*length),
		length
	};
	return output;
}

void String_destructorFree(string str)
{
	if(str.str != NULL)
		free(str.str);
}