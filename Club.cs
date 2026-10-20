using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSortPerformance
{
    internal class Club
    {
        public string Name;
        public int Points;
        public int GoalDifference;
        public int GoalsScored;

        public Club(string name, int points, int difference, int goals)
        {
            Name = name;
            Points = points;
            GoalDifference = difference;
            GoalsScored = goals;
        }
    }
}
