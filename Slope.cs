﻿/***************************************************************************************************************************************************************************************
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

/// <summary>
/// Least-Squares Linear Regressions Slope
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_SLOPE")]
public struct LSREGR_SLOPE
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
    public void Merge(LSREGR_SLOPE group)
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

/// <summary>
/// Least-Squares Linear Regressions Y-Intercept
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_INTERCEPT")]
public struct LSREGR_INTERCEPT
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
    public void Merge(LSREGR_INTERCEPT group)
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
        SqlDouble slope = (N == 0 || Sxy == SqlDouble.Zero || Sxx == SqlDouble.Zero ||
            Sx == SqlDouble.Zero || Sy == SqlDouble.Zero) ?
            SqlDouble.Null : (N * Sxy - Sx * Sy) / (N * Sxx - Sx * Sx);

        SqlDouble mean_y = (N == 0 ? SqlDouble.Null : Sy / N);
        SqlDouble mean_x = (N == 0 ? SqlDouble.Null : Sx / N);

        return (slope == SqlDouble.Null || mean_x == SqlDouble.Null ||
            mean_y == SqlDouble.Null ? SqlDouble.Null : mean_y - slope * mean_x);
    }
}

/// <summary>
/// Least-Squares Linear Regressions Row Count
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_COUNT")]
public struct LSREGR_COUNT
{
    /// <summary>
    /// Number of rows
    /// </summary>
    private SqlInt64 N { get; set; }
    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init() { N = 0; }
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
        { N += 1; }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(LSREGR_COUNT group)
    {
        if (group.N == 0)
        {/* if ANY is NULL, then do nothing */}
        else
        { N += group.N; }
    }
    /// <summary>
    /// Completes the aggregate computation and returns the result.
    /// </summary>
    /// <returns>The result of the aggregation</returns>
    public SqlDouble Terminate() { return N; }
}

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

    private List<SqlDouble> List_y;
    private List<SqlDouble> List_x;

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

        List_y = new List<SqlDouble>();
        List_x = new List<SqlDouble>();
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

            List_x.Add(x);
            List_y.Add(y);
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
            group.Sx == SqlDouble.Zero  || group.Sy == SqlDouble.Zero ||
            group.List_x.Count == 0     || group.List_y.Count == 0)
        {/* if ANY is NULL, then do nothing */}
        else
        {
            N += group.N;
            Sxy += group.Sxy;
            Sxx += group.Sxx;
            Sx += group.Sx;
            Sy += group.Sy;

            List_x.AddRange(group.List_x);
            List_y.AddRange(group.List_y);
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

        SqlDouble sr = SqlDouble.Null;
        SqlDouble st = SqlDouble.Null;
        if (intercept == SqlDouble.Null || List_y.Count == 0 || List_x.Count == 0)
        {
            return SqlDouble.Null;
        } 
        else
        {
            for (int i = 0; i < N.Value; ++i)
            {
                sr += (List_y[i] - intercept - (slope * List_x[i])) * 
                      (List_y[i] - intercept - (slope * List_x[i]));
                st += (List_y[i] - mean_y) * (List_y[i] - mean_y);
            }

            return (st == SqlDouble.Null || sr == SqlDouble.Null) ? 
                SqlDouble.Null : (st - sr) / st;
        }
    }
}

/// <summary>
/// Least-Squares Linear Regressions x-average
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_AVGX")]
public struct LSREGR_AVGX
{
    /// <summary>
    /// Number of rows
    /// </summary>
    private SqlInt64 N { get; set; }
    /// <summary>
    /// Sx = Sum(x) from 1 to N
    /// </summary>
    private SqlDouble Sx { get; set; }
    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init()
    {
        N = 0;
        Sx = SqlDouble.Zero;
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
            Sx += x;
        }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(LSREGR_AVGX group)
    {
        if (
            group.N == 0 ||
            group.Sx == SqlDouble.Zero)
        {/* if ANY is NULL, then do nothing */}
        else
        {
            N += group.N;
            Sx += group.Sx;
        }
    }
    /// <summary>
    /// Completes the aggregate computation and returns the result.
    /// </summary>
    /// <returns>The result of the aggregation</returns>
    public SqlDouble Terminate() {  return (N == 0 ? SqlDouble.Null : Sx / N); }
}

/// <summary>
/// Least-Squares Linear Regressions y-average
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_AVGY")]
public struct LSREGR_AVGY
{
    /// <summary>
    /// Number of rows
    /// </summary>
    private SqlInt64 N { get; set; }
    /// <summary>
    /// Sy = Sum(y) from 1 to N
    /// </summary>
    private SqlDouble Sy { get; set; }
    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init()
    {
        N = 0;
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
            Sy += y;
        }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(LSREGR_AVGY group)
    {
        if (
            group.N == 0 ||
            group.Sy == SqlDouble.Zero)
        {/* if ANY is NULL, then do nothing */}
        else
        {
            N += group.N;
            Sy += group.Sy;
        }
    }
    /// <summary>
    /// Completes the aggregate computation and returns the result.
    /// </summary>
    /// <returns>The result of the aggregation</returns>
    public SqlDouble Terminate() { return (N == 0 ? SqlDouble.Null : Sy / N); }
}

/// <summary>
/// Least-Squares Linear Regressions Slope
/// </summary>
[System.Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Microsoft.SqlServer.Server.Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "LSREGR_SXX")]
public struct LSREGR_SXX
{
    /// <summary>
    /// Sxx = Sum(x*x) from 1 to N
    /// </summary>
    private SqlDouble Sxx { get; set; }
    /// <summary>
    /// Function for query processor to intialize the computation 
    /// of the aggregation.
    /// </summary>
    public void Init()
    {
        Sxx = SqlDouble.Zero;
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
        { Sxx += x * x; }
    }
    /// <summary>
    /// Merge another instance of the aggregate class with current
    /// instance.
    /// </summary>
    /// <param name="group"></param>
    public void Merge(LSREGR_SXX group)
    {
        if (group.Sxx == SqlDouble.Zero)
        {/* if ANY is NULL, then do nothing */}
        else
        { Sxx += group.Sxx; }
    }
    /// <summary>
    /// Completes the aggregate computation and returns the result.
    /// </summary>
    /// <returns>The result of the aggregation</returns>
    public SqlDouble Terminate()
    {
        return Sxx;
    }
}