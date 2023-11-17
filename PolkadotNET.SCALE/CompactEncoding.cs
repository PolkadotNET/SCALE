using System.Numerics;
using PolkadotNET.SCALE.Types;

namespace PolkadotNET.SCALE;

public static class CompactEncoding
{
    private const int SingleByteMode = 0b00;
    private const int TwoByteMode = 0b01;
    private const int FourByteMode = 0b10;
    private const int MultiByteMode = 0b11;

    private const int SingleByteMaxRange = 0b0011_1111;
    private const int TwoByteMaxRange = 0b0011_1111_1111_1111;
    private const int FourByteMaxRange = 0b0011_1111_1111_1111_1111_1111_1111_1111;
    
    public static byte[] Encode(this CompactSignedByte value) => EncodeCompact((byte)value.Value);
    public static byte[] Encode(this CompactByte value) => EncodeCompact(value.Value);
    public static byte[] Encode(this CompactUInt16 value) => EncodeCompact(value.Value);
    public static byte[] Encode(this CompactInt16 value) => EncodeCompact((ushort)value.Value);
    public static byte[] Encode(this CompactUInt32 value) => EncodeCompact(value.Value);
    public static byte[] Encode(this CompactInt32 value) => EncodeCompact((uint)value.Value);
    public static byte[] Encode(this CompactUInt64 value) => EncodeCompact(value.Value);
    public static byte[] Encode(this CompactInt64 value) => EncodeCompact((ulong)value.Value);
    
    private static byte[] EncodeCompact(this byte value) => value switch
    {
        <= SingleByteMaxRange => ((byte)((value << 2) | 0b00)).Encode(),
        _ => ((ushort)((value << 2) | 0b01)).Encode(),
    };
    private static byte[] EncodeCompact(this ushort value) => value switch
    {
        <= SingleByteMaxRange => ((byte)((value << 2) | 0b00)).Encode(),
        <= TwoByteMaxRange => ((ushort)((value << 2) | 0b01)).Encode(),
        _ => ((uint)((value << 2) | 0b10)).Encode(),
    };
    private static byte[] EncodeCompact(this uint value) =>
        value switch
        {
            <= SingleByteMaxRange => ((byte)((value << 2) | 0b00)).Encode(),
            <= TwoByteMaxRange => ((ushort)((value << 2) | 0b01)).Encode(),
            <= FourByteMaxRange => ((uint)((value << 2) | 0b10)).Encode(),
            _ => new byte[] { 0b11 }.Concat(value.Encode()).ToArray()
        };
    private static byte[] EncodeCompact(this ulong value) =>
        value switch
        {
            <= SingleByteMaxRange => ((byte)((value << 2) | 0b00)).Encode(),
            <= TwoByteMaxRange => ((ushort)((value << 2) | 0b01)).Encode(),
            <= FourByteMaxRange => ((uint)((value << 2) | 0b10)).Encode(),
            _ => EncodeMultiByte(value)
        };
    private static byte[] EncodeMultiByte(this ulong value)
    {
        var leadingZeros = BitOperations.LeadingZeroCount(value);
        var requiredBytes = 8 - leadingZeros / 8;
        var buffer = new byte[requiredBytes + 1];
        buffer[0] = (byte)(0b11 + ((requiredBytes - 4) << 2));
        for (var i = 0; i < requiredBytes; i++)
        {
            buffer[i + 1] = (byte)value;
            value >>= 8;
        }

        return buffer;
    }
    
    public static int DecodeCompactSByte(this byte[] encoded, out CompactSignedByte value)
    {
        var len = DecodeCompact(encoded, out byte unsignedValue);
        value = new CompactSignedByte((sbyte)unsignedValue);
        return len;
    }
    
    public static int DecodeCompactByte(this byte[] encoded, out CompactByte value)
    {
        var len = DecodeCompact(encoded, out byte unsignedValue);
        value = new CompactByte(unsignedValue);
        return len;
    }
    
    private static int DecodeCompact(this byte[] encoded, out byte value)
    {
        var mode = encoded[0] & 0b11;
        var len = 0;

        switch (mode)
        {
            case SingleByteMode:
                value = (byte)(encoded[0] >> 2);
                len = 1;
                break;

            case TwoByteMode:
                value = Convert.ToByte(encoded.DecodeUInt16() >> 2);
                len = 2;
                break;

            default:
                throw new InvalidCastException("invalid mode for byte");
        }

        return len;
    }
    
    public static int DecodeCompactInt16(this byte[] encoded, out CompactInt16 value)
    {
        var len = DecodeCompact(encoded, out ushort unsignedValue);
        value = new CompactInt16((short)unsignedValue);
        return len;
    }
    
    public static int DecodeCompactUInt16(this byte[] encoded, out CompactUInt16 value)
    {
        var len = DecodeCompact(encoded, out ushort unsignedValue);
        value = new CompactUInt16(unsignedValue);
        return len;
    }
    
    private static int DecodeCompact(this byte[] encoded, out ushort value)
    {
        var mode = encoded[0] & 0b11;
        var len = 0;

        switch (mode)
        {
            case SingleByteMode:
                value = (ushort)(encoded[0] >> 2);
                len = 1;
                break;

            case TwoByteMode:
                value = (ushort)(encoded.DecodeUInt16() >> 2);
                len = 2;
                break;

            case FourByteMode:
                value = Convert.ToUInt16(encoded.DecodeUInt32() >> 2);
                len = 4;
                break;

            default:
                throw new InvalidCastException("invalid mode for ushort");
        }

        return len;
    }
    
    public static int DecodeCompactInt32(this byte[] encoded, out CompactInt32 value)
    {
        var len = DecodeCompact(encoded, out uint unsignedValue);
        value = new CompactInt32((int)unsignedValue);
        return len;
    }
    
    public static int DecodeCompactUInt32(this byte[] encoded, out CompactUInt32 value)
    {
        var len = DecodeCompact(encoded, out uint unsignedValue);
        value = new CompactUInt32(unsignedValue);
        return len;
    }
    
    private static int DecodeCompact(this byte[] encoded, out uint value)
    {
        var mode = encoded[0] & 0b11;
        var len = 0;

        switch (mode)
        {
            case SingleByteMode:
                value = (uint)(encoded[0] >> 2);
                len = 1;
                break;

            case TwoByteMode:
                value = (uint)(encoded.DecodeUInt16() >> 2);
                len = 2;
                break;

            case FourByteMode:
                value = (uint)(encoded.DecodeUInt32() >> 2);
                len = 4;
                break;

            case MultiByteMode:
                value = Convert.ToUInt32(encoded[1..].DecodeUInt32());
                len = 5;
                break;

            default:
                throw new InvalidCastException("invalid mode for ushort");
        }

        return len;
    }
    
    public static int DecodeCompactInt64(this byte[] encoded, out CompactInt64 value)
    {
        var len = DecodeCompact(encoded, out ulong unsignedValue);
        value = new CompactInt64((long)unsignedValue);
        return len;
    }
    
    public static int DecodeCompactUInt64(this byte[] encoded, out CompactUInt64 value)
    {
        var len = DecodeCompact(encoded, out ulong unsignedValue);
        value = new CompactUInt64(unsignedValue);
        return len;
    }
    
    private static int DecodeCompact(this byte[] encoded, out ulong value)
    {
        var mode = encoded[0] & 0b11;
        var len = 0;

        switch (mode)
        {
            case SingleByteMode:
                value = (ulong)(encoded[0] >> 2);
                len = 1;
                break;

            case TwoByteMode:
                value = (ulong)(encoded.DecodeUInt16() >> 2);
                len = 2;
                break;

            case FourByteMode:
                value = (ulong)(encoded.DecodeUInt32() >> 2);
                len = 4;
                break;

            case MultiByteMode:
                len = DecodeMultiByte(encoded, out value);
                break;

            default:
                throw new InvalidCastException("invalid mode for ushort");
        }

        return len;
    }
    private static int DecodeMultiByte(this byte[] encoded, out ulong value)
    {
        var requiredBytes = (encoded[0] >> 2) + 4;
        value = 0;
        for (var i = requiredBytes; i > 0; i--)
        {
            value <<= 8;
            value += encoded[i];
        }

        return requiredBytes + 1;
    }
}