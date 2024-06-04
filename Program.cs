using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    internal class Program
    {
        public class SimplexMethod
        {
            private double[,] tableau;
            private int rows, cols;
            private List<int> basis;

            public SimplexMethod(double[,] tableau)
            {
                this.tableau = tableau;
                this.rows = tableau.GetLength(0);
                this.cols = tableau.GetLength(1);
                this.basis = new List<int>();

                for (int i = 0; i < rows - 1; i++)
                {
                    basis.Add(cols - rows + i);
                }
            }

            public void Solve()
            {
                while (true)
                {
                    int pivotColumn = SelectPivotColumn();
                    if (pivotColumn == -1) break;

                    int pivotRow = SelectPivotRow(pivotColumn);
                    if (pivotRow == -1) throw new InvalidOperationException("The problem is unbounded.");

                    Pivot(pivotRow, pivotColumn);
                }

                DisplaySolution();
            }

            private int SelectPivotColumn()
            {
                int pivotColumn = -1;
                double maxValue = 0;

                for (int j = 0; j < cols - 1; j++)
                {
                    if (tableau[rows - 1, j] > maxValue)
                    {
                        maxValue = tableau[rows - 1, j];
                        pivotColumn = j;
                    }
                }

                return pivotColumn;
            }

            private int SelectPivotRow(int pivotColumn)
            {
                int pivotRow = -1;
                double minRatio = double.PositiveInfinity;

                for (int i = 0; i < rows - 1; i++)
                {
                    if (tableau[i, pivotColumn] > 0)
                    {
                        double ratio = tableau[i, cols - 1] / tableau[i, pivotColumn];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            pivotRow = i;
                        }
                    }
                }

                return pivotRow;
            }

            private void Pivot(int pivotRow, int pivotColumn)
            {
                double pivotValue = tableau[pivotRow, pivotColumn];

                for (int j = 0; j < cols; j++)
                {
                    tableau[pivotRow, j] /= pivotValue;
                }

                for (int i = 0; i < rows; i++)
                {
                    if (i != pivotRow)
                    {
                        double multiplier = tableau[i, pivotColumn];
                        for (int j = 0; j < cols; j++)
                        {
                            tableau[i, j] -= multiplier * tableau[pivotRow, j];
                        }
                    }
                }

                basis[pivotRow] = pivotColumn;
            }

            private void DisplaySolution()
            {
                double[] solution = new double[cols - 1];

                for (int i = 0; i < basis.Count; i++)
                {
                    if (basis[i] < cols - 1)
                    {
                        solution[basis[i]] = tableau[i, cols - 1];
                    }
                }

                Console.WriteLine("Solution:");
                for (int i = 0; i < solution.Length; i++)
                {
                    Console.WriteLine($"x{i + 1} = {solution[i]}");
                }

                Console.WriteLine($"Optimal value: {tableau[rows - 1, cols - 1]}");
            }

            public static void Main(string[] args)
            {
                double[,] tableau = {
            { 2, 1, 1, 0, 0, 14 },
            { 4, 2, 0, 1, 0, 28 },
            { 2, 5, 0, 0, 1, 30 },
            { -1, -2, 0, 0, 0, 0 }
        };

                SimplexMethod simplex = new SimplexMethod(tableau);
                simplex.Solve();
                Console.Read();
            }
        }
    }
}
