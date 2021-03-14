/***************************************************************************************************************************************************************************************
 *  @author: German Rafael Gomez Urbina
 *  @email: grgomezu@gmail.com
 *  
 *  SQL Server user defined aggregate functions of the 
 *  Linear Least-Squares Regression functions
 *  
 *  Based on:
 *  
 *  S. C. Chapra, Applied Numerical Methods with MATLAB for engineers and Scientists,
 *  2nd Edition, McGraw Hill, 2008.
 *  
 *  Also based on Oracle's linear regression functions:
 *  
 *  https://docs.oracle.com/cd/B19306_01/server.102/b14200/functions132.htm
 *  
 * Using:  
 * https://www.codeproject.com/articles/170061/custom-aggregates-in-sql-server
 * https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration-database-objects-user-defined-functions/clr-user-defined-aggregates-requirements?view=sql-server-ver15
 * 
 * MIT License:
 * 
 * Copyright 2021 German Gomez Urbina
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * ***************************************************************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
/// <summary>
/// Least-Squares Linear Regression coefficient of determination
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_R2")]
public struct LSREGR_R2
{
    /// <summary>
    /// Number of rows
    /// </summary>
    private SqlInt64 N { get; set; }
    /// <summary>
    /// Sxy = Sum(x*y) from 1 to N
    /// </summary>
    private SqlDouble Sxy { get; set; }
    /// <summary>
    /// Sxx = Sum(x*x) from 1 to N
    /// </summary>
    private SqlDouble Sxx { get; set; }
    /// <summary>
    /// Sx = Sum(x) from 1 to N
    /// </summary>
    private SqlDouble Sx { get; set; }
    /// <summary>
    /// Sy = Sum(y) from 1 to N
    /// </summary>
    private SqlDouble Sy { get; set; }
    /// <summary>
    /// Syy = Sum(y*y) from 1 to N
    /// </summary>
    private SqlDouble Syy { get; set; }

    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init()
    {
        N = 0;
        Sxy = SqlDouble.Zero;
        Sxx = SqlDouble.Zero;
        Syy = SqlDouble.Zero;
        Sx = SqlDouble.Zero;
        Sy = SqlDouble.Zero;
    }
    /// <summary>
    /// Accumulation of the values being passed in
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Accumulate(SqlDouble x, SqlDouble y)
    {
        if (x.IsNull || y.IsNull)
        {/* do nothing */}
        else
        {
            N += 1;
            Sxy += x * y;
            Sxx += x * x;
            Syy += y * y;
            Sx += x;
            Sy += y;
        }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(LSREGR_R2 group)
    {
        if (
            group.N == 0 ||
            group.Sxy == SqlDouble.Zero || group.Sxx == SqlDouble.Zero ||
            group.Sx == SqlDouble.Zero  || group.Sy == SqlDouble.Zero  ||
            group.Syy == SqlDouble.Zero)
        {/* if ANY is NULL, then do nothing */}
        else
        {
            N += group.N;
            Sxy += group.Sxy;
            Sxx += group.Sxx;
            Syy += group.Syy;
            Sx += group.Sx;
            Sy += group.Sy;
        }
    }
    /// <summary>
    /// Completes the aggregate computation and returns the result.
    /// </summary>
    /// <returns>The result of the aggregation</returns>
    public SqlDouble Terminate()
    {
        SqlDouble slope = (N == 0 || Sxy == SqlDouble.Zero || Sxx == SqlDouble.Zero ||
            Sx == SqlDouble.Zero || Sy == SqlDouble.Zero) ?
            SqlDouble.Null : (N * Sxy - Sx * Sy) / (N * Sxx - Sx * Sx);

        SqlDouble mean_y = (N == 0 ? SqlDouble.Null : Sy / N);
        SqlDouble mean_x = (N == 0 ? SqlDouble.Null : Sx / N);

        SqlDouble intercept = (slope == SqlDouble.Null || mean_x == SqlDouble.Null ||
            mean_y == SqlDouble.Null ? SqlDouble.Null : mean_y - slope * mean_x);

        if (intercept == SqlDouble.Null || slope == SqlDouble.Null ) //|| List_y.Count == 0 || List_x.Count == 0)
        {
            return SqlDouble.Null;
        } 
        else
        {
            SqlDouble sr = Syy - (2 * intercept * Sy) + (2 * intercept * slope * Sx) - (2 * slope * Sxy) + (slope * slope * Sxx) + (intercept * intercept * N);
            SqlDouble st = Syy - (2 * mean_y * Sy) + (mean_y * mean_y * N);

            return (st == SqlDouble.Null || sr == SqlDouble.Null || st == SqlDouble.Zero) ? 
                SqlDouble.Null : (st - sr) / st;
        }
    }
}