using FakeStockProxy.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FakeStockProxy.Core.Entities;

public class FsStockRequest : Entity
{
    public string? Hashsum { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Expand { get; set; }
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public string? OrderDirection { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    public FsStock? FsStock { get; set; }

    #region System.Object overrides

    public override int GetHashCode()
    {
        return Hashsum!.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;
        if (ReferenceEquals(this, obj)) return true;

        return Equals((FsStockRequest)obj);
    }

    public bool Equals(FsStockRequest fsStockRequestDto)
    {
        return fsStockRequestDto.Hashsum == Hashsum;
    }

    public static bool operator ==(FsStockRequest a, FsStockRequest b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        return a.Hashsum == b.Hashsum;
    }

    public static bool operator !=(FsStockRequest a, FsStockRequest b)
    {
        return !(a == b);
    }

    #endregion
}
