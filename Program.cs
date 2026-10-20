namespace QuickSortPerformance
{
    using System;
    using System.Collections.Generic;
     

        class SortStats
        {
            public long Comparisons;
            public long RecursiveCalls;
        }

        class Program
        {
            static void Main()
            {
                Activity1();
                Activity2();
                Activity3();                
            }

            // ====================================================
            // ACTIVITY 1: QUICKSORT THE FOOTBALL LEAGUE
            // ====================================================

            static Club[] CreateClubs()
            {
                return new Club[]
                {
                new Club("Sheffield United", 12, 5, 14),
                new Club("Leeds United", 12, 8, 16),
                new Club("Sunderland", 9, 3, 11),
                new Club("Burnley", 12, 8, 19),
                new Club("Bristol City", 9, 3, 13),
                new Club("Norwich City", 15, 7, 18),
                new Club("Coventry City", 12, 8, 16),
                new Club("Derby County", 6, -4, 8),
                new Club("Hull City", 9, 1, 10),
                new Club("Watford", 15, 4, 17)
                };
            }

            // Positive means a ranks ahead of b.
            // Negative means b ranks ahead of a.
            // Zero means all ranking criteria are equal.
            static int CompareClubs(Club a, Club b)
            {
                if (a.Points > b.Points) return 1;
                if (a.Points < b.Points) return -1;

                if (a.GoalDifference > b.GoalDifference) return 1;
                if (a.GoalDifference < b.GoalDifference) return -1;

                if (a.GoalsScored > b.GoalsScored) return 1;
                if (a.GoalsScored < b.GoalsScored) return -1;

                return 0;
            }

            static void Activity1()
            {
                Console.WriteLine("\nACTIVITY 1: QUICKSORT LEAGUE TABLE");

                Club[] clubs = CreateClubs();
                SortStats stats = new SortStats();

                QuickSortClubs(clubs, 0, clubs.Length - 1, true, stats);

                DisplayLeague(clubs);

                Console.WriteLine("Comparisons: " + stats.Comparisons);
                Console.WriteLine("Recursive calls: " + stats.RecursiveCalls);
                Console.WriteLine("First place: " + clubs[0].Name);
                Console.WriteLine("Last place: " + clubs[clubs.Length - 1].Name);
            }

            static void DisplayLeague(Club[] clubs)
            {
                Console.WriteLine("{0,-20} {1,7} {2,10} {3,8}","Club", "Points", "GoalDiff", "Goals");
                foreach (Club c in clubs)
                   Console.WriteLine("{0,-20} {1,7} {2,10} {3,8}", c.Name, c.Points, c.GoalDifference, c.GoalsScored);                
            }

            static void Swap<T>(T[] a, int i, int j)
            {
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }

            // First-element or last-element pivot.
            // Recursively sorts the smaller partition to limit
            // stack depth on sorted and reverse-sorted inputs.
            static void QuickSortClubs(Club[] a, int low, int high,bool firstPivot, SortStats stats)
            {
                stats.RecursiveCalls++;

                while (low < high)
                {
                    int pivotIndex = PartitionClubs(a, low, high, firstPivot, stats);

                    int leftSize = pivotIndex - low;
                    int rightSize = high - pivotIndex;

                    if (leftSize < rightSize)
                    {
                        QuickSortClubs(a, low, pivotIndex - 1,firstPivot, stats);
                        low = pivotIndex + 1;
                    }
                    else
                    {
                        QuickSortClubs(a, pivotIndex + 1, high, firstPivot, stats);
                        high = pivotIndex - 1;
                    }
                }
            }

            // Places clubs in descending ranking order.
            static int PartitionClubs(Club[] a, int low, int high, bool firstPivot, SortStats stats)
            {
                int pivotPosition = firstPivot ? low : high;
                Swap(a, pivotPosition, high);

                Club pivot = a[high];
                int boundary = low;

                for (int j = low; j < high; j++)
                {
                    stats.Comparisons++;

                    if (CompareClubs(a[j], pivot) > 0)
                    {
                        Swap(a, boundary, j);
                        boundary++;
                    }
                }

                Swap(a, boundary, high);
                return boundary;
            }

            // ====================================================
            // ACTIVITY 2: CHANGE PIVOT STRATEGY
            // ====================================================

            static void Activity2()
            {
                Console.WriteLine("\nACTIVITY 2: PIVOT STRATEGIES");

                foreach (bool firstPivot in new bool[] { true, false })
                {
                    Club[] clubs = CreateClubs();
                    SortStats stats = new SortStats();

                    QuickSortClubs(clubs, 0, clubs.Length - 1, firstPivot, stats);

                    Console.WriteLine((firstPivot ? "First" : "Last") + " element pivot: " + stats.Comparisons + " comparisons, " +
                        stats.RecursiveCalls + " recursive calls");
                }

                Console.WriteLine("Both strategies produce a valid ranking, but " + "their counts can differ.");
            }

            // ====================================================
            // ACTIVITY 3: PERFORMANCE INVESTIGATION
            // ====================================================

            static void Activity3()
            {
                Console.WriteLine("\nACTIVITY 3: PERFORMANCE TESTS");

                int[] sizes = { 100, 1000, 5000 };
                Random random = new Random(42);

                Console.WriteLine("{0,7} {1,-16} {2,-14} {3,14} {4,16}","Size", "Dataset", "Algorithm","Comparisons", "Recursive calls");

                foreach (int size in sizes)
                {
                    int[] randomData = new int[size];

                    for (int i = 0; i < size; i++)
                        randomData[i] = random.Next(0, size * 10);

                    int[] sortedData = (int[])randomData.Clone();
                    Array.Sort(sortedData);

                    int[] reverseData = (int[])sortedData.Clone();
                    Array.Reverse(reverseData);

                    int[] duplicateData = new int[size];

                    for (int i = 0; i < size; i++)
                        duplicateData[i] = random.Next(0, 5);

                    TestDataset(size, "Random", randomData);
                    TestDataset(size, "Sorted", sortedData);
                    TestDataset(size, "Reverse", reverseData);
                    TestDataset(size, "Duplicates", duplicateData);
                }
            }

            static void TestDataset(int size, string name, int[] source)
            {
                // Each algorithm receives its own copy.
                int[] mergeData = (int[])source.Clone();
                SortStats mergeStats = new SortStats();

                MergeSort(mergeData, 0, mergeData.Length - 1, mergeStats);

                PrintStats(size, name, "Merge Sort", mergeStats);

                foreach (bool firstPivot in new bool[] { true, false })
                {
                    int[] quickData = (int[])source.Clone();
                    SortStats quickStats = new SortStats();

                    QuickSortInts(quickData, 0, quickData.Length - 1, firstPivot, quickStats);

                    PrintStats(size, name, firstPivot ? "Quick first" : "Quick last", quickStats);

                    if (!IsSorted(quickData))
                        Console.WriteLine("ERROR: Quicksort failed.");
                }

                if (!IsSorted(mergeData))
                    Console.WriteLine("ERROR: Merge Sort failed.");
            }

            static void PrintStats(int size, string dataset,string algorithm, SortStats stats)
            {
                Console.WriteLine("{0,7} {1,-16} {2,-14} {3,14} {4,16}",size, dataset, algorithm,stats.Comparisons, stats.RecursiveCalls);
            }

            // Merge Sort for integer performance testing.
            static void MergeSort(int[] a, int left, int right, SortStats stats)
            {
                stats.RecursiveCalls++;

                if (left >= right)
                    return;

                int middle = left + (right - left) / 2;

                MergeSort(a, left, middle, stats);
                MergeSort(a, middle + 1, right, stats);

                Merge(a, left, middle, right, stats);
            }

            static void Merge(int[] a, int left, int middle,int right, SortStats stats)
            {
                int[] temp = new int[right - left + 1];

                int i = left;
                int j = middle + 1;
                int k = 0;

                while (i <= middle && j <= right)
                {
                    stats.Comparisons++;

                    // Select left first when values are equal.
                    if (a[i] <= a[j])
                        temp[k++] = a[i++];
                    else
                        temp[k++] = a[j++];
                }

                while (i <= middle)
                    temp[k++] = a[i++];

                while (j <= right)
                    temp[k++] = a[j++];

                for (int p = 0; p < temp.Length; p++)
                    a[left + p] = temp[p];
            }

            static void QuickSortInts(
                int[] a, int low, int high,
                bool firstPivot, SortStats stats)
            {
                stats.RecursiveCalls++;

                while (low < high)
                {
                    int pivot = PartitionInts(a, low, high, firstPivot, stats);

                    int leftSize = pivot - low;
                    int rightSize = high - pivot;

                    if (leftSize < rightSize)
                    {
                        QuickSortInts(
                            a, low, pivot - 1,
                            firstPivot, stats);

                        low = pivot + 1;
                    }
                    else
                    {
                        QuickSortInts(
                            a, pivot + 1, high,
                            firstPivot, stats);

                        high = pivot - 1;
                    }
                }
            }

            static int PartitionInts(int[] a, int low, int high, bool firstPivot, SortStats stats)
            {
                int pivotPosition = firstPivot ? low : high;
                Swap(a, pivotPosition, high);

                int pivot = a[high];
                int boundary = low;

                for (int j = low; j < high; j++)
                {
                    stats.Comparisons++;

                    if (a[j] < pivot)
                    {
                        Swap(a, boundary, j);
                        boundary++;
                    }
                }

                Swap(a, boundary, high);
                return boundary;
            }

            static bool IsSorted(int[] a)
            {
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i - 1] > a[i])
                        return false;
                }

                return true;
            }
        }
}