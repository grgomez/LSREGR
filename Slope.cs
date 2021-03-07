using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

/* 
    * Based on
    * https://www.codeproject.com/articles/170061/custom-aggregates-in-sql-server
    */
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
    public SqlInt16 N { get; set; }
    private SqlDouble Sxy { get; set; }
    public SqlDouble Sxx { get; set; }
    public SqlDouble Sx { get; set; }
    public SqlDouble Sy { get; set; }
    public void Init()
    {
        N = 0;
        Sxy = SqlDouble.Zero;
        Sxx = SqlDouble.Zero;
        Sx = SqlDouble.Zero;
        Sy = SqlDouble.Zero;
    }
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
    public SqlDouble Terminate()
    {
        return 
            (N == 0 || Sxy == SqlDouble.Zero || Sxx == SqlDouble.Zero || 
            Sx == SqlDouble.Zero || Sy == SqlDouble.Zero) ?
            SqlDouble.Null : (N * Sxy - Sx * Sy) / (N * Sxx - Sx * Sx);
    }
}

