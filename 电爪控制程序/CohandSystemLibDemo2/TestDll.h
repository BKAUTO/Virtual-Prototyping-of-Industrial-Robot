#pragma once
#ifndef _TEST_DLL_H_
#define _TEST_DLL_H_
#endif

#if defined (EXPORTBUILD)
# define _DLLExport __declspec (dllexport)
# else
# define _DLLExport __declspec (dllimport)
#endif

extern "C" int _DLLExport main();

_DLLExport class TestDll
{
public:
	TestDll(void);
	~TestDll(void);
};