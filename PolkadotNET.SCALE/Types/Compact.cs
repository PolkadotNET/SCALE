using System.Runtime.Intrinsics;

namespace PolkadotNET.SCALE.Types;

public record CompactByte(byte Value)
{
    public static implicit operator byte(CompactByte v1) => v1.Value;
    public static explicit operator CompactByte(byte v1) => new(v1);
}
public record CompactSignedByte(sbyte Value);
public record CompactUInt16(ushort Value);
public record CompactInt16(short Value);
public record CompactUInt32(uint Value);
public record CompactInt32(int Value);
public record CompactUInt64(ulong Value);
public record CompactInt64(long Value);