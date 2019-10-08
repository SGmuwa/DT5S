#include "..\minctest\minctest.h"
#include "..\Pareto\ParetoTest.h"


void main()
{
	minctest_reset();
	minctest_run("Pareto", Pareto_test_run);
	minctest_results();
	printf_s("end.\n");
}