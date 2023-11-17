using System.Runtime.Intrinsics;

namespace PolkadotNET.SCALE.Types;

public record CompactByte(byte Value)
{
    public static implicit operator byte(CompactByte v1) => v1.Value;
    public static explicit operator CompactByte(byte v1) => new(v1);
}

public record CompactSignedByte(sbyte Value)
{
    public static implicit operator sbyte(CompactSignedByte v1) => v1.Value;
    public static explicit operator CompactSignedByte(sbyte v1) => new(v1);
}

public record CompactUInt16(ushort Value)
{
    public static implicit operator ushort(CompactUInt16 v1) => v1.Value;
    public static explicit operator CompactUInt16(ushort v1) => new(v1);
}

public record CompactInt16(short Value)
{
    public static implicit operator short(CompactInt16 v1) => v1.Value;
    public static explicit operator CompactInt16(short v1) => new(v1);
}

public record CompactUInt32(uint Value)
{
    public static implicit operator uint(CompactUInt32 v1) => v1.Value;
    public static explicit operator CompactUInt32(uint v1) => new(v1);
}

public record CompactInt32(int Value)
{
    public static implicit operator int(CompactInt32 v1) => v1.Value;
    public static explicit operator CompactInt32(int v1) => new(v1);
}

public record CompactUInt64(ulong Value)
{
    public static implicit operator ulong(CompactUInt64 v1) => v1.Value;
    public static explicit operator CompactUInt64(ulong v1) => new(v1);
}

public record CompactInt64(long Value)
{
    public static implicit operator long(CompactInt64 v1) => v1.Value;
    public static explicit operator CompactInt64(long v1) => new(v1);
}