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
 * 
 * ***************************************************************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;


[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "SLOPE")]
public struct SLOPE
{
    /// <summary>
    /// Number of rows
    /// </summary>
    public SqlInt16 N { get; set; }
    /// <summary>
    /// Sxy = Sum(x*y) from 1 to N
    /// </summary>
    private SqlDouble Sxy { get; set; }
    /// <summary>
    /// Sxx = Sum(x*x) from 1 to N
    /// </summary>
    public SqlDouble Sxx { get; set; }
    /// <summary>
    /// Sx = Sum(x) from 1 to N
    /// </summary>
    public SqlDouble Sx { get; set; }
    /// <summary>
    /// Sy = Sum(y) from 1 to N
    /// </summary>
    public SqlDouble Sy { get; set; }
    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init()
    {
        N = 0;
        Sxy = SqlDouble.Zero;
        Sxx = SqlDouble.Zero;
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
            Sx += x;
            Sy += y;
        }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(SLOPE group)
    {
        if (
            group.N == 0 || 
            group.Sxy == SqlDouble.Zero || group.Sxx == SqlDouble.Zero || 
            group.Sx == SqlDouble.Zero || group.Sy == SqlDouble.Zero)
        {/* if ANY is NULL, then do nothing */}
        else
        {
            N += group.N;
            Sxy += group.Sxy;
            Sxx += group.Sxx;
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
        return 
            (N == 0 || Sxy == SqlDouble.Zero || Sxx == SqlDouble.Zero || 
            Sx == SqlDouble.Zero || Sy == SqlDouble.Zero) ?
            SqlDouble.Null : (N * Sxy - Sx * Sy) / (N * Sxx - Sx * Sx);
    }
}

