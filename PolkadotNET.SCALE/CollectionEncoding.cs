using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using PolkadotNET.SCALE.Types;

namespace PolkadotNET.SCALE;

public static class CollectionEncoding
{
    public static byte[] Encode(this string value) =>
        EncodeLength(value.ToArray()).Concat(Encoding.UTF8.GetBytes(value)).ToArray();

    public static byte[] Encode(this byte[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this bool[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this sbyte[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this short[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this ushort[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this int[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this uint[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this long[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    public static byte[] Encode(this ulong[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    
    public static byte[] Encode(this CompactByte[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactSignedByte[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactInt16[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactUInt16[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactInt32[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactUInt32[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactInt64[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();
    public static byte[] Encode(this CompactUInt64[] items) =>
        EncodeLength(items).Concat(items.SelectMany(x => x.Encode())).ToArray();

    private static byte[] EncodeLength(Array array)
        => new CompactInt32(array.Length).Encode();

    public static string DecodeString(this byte[] encoded)
        => Encoding.UTF8.GetString(encoded, encoded.DecodeCompactInt32(out var compact), compact.Value);

    public static byte[] DecodeByteCollection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(1).Select(c => c[0].DecodeByte()).ToArray();
    }

    public static sbyte[] DecodeSignedByteCollection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(1).Select(c => c[0].DecodeSignedByte()).ToArray();
    }

    public static short[] DecodeInt16Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(2).Select(c => c.DecodeInt16()).ToArray();
    }

    public static ushort[] DecodeUInt16Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(2).Select(c => c.DecodeUInt16()).ToArray();
    }

    public static int[] DecodeInt32Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(4).Select(c => c.DecodeInt32()).ToArray();
    }

    public static uint[] DecodeUInt32Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(4).Select(c => c.DecodeUInt32()).ToArray();
    }

    public static long[] DecodeInt64Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(8).Select(c => c.DecodeInt64()).ToArray();
    }

    public static ulong[] DecodeUInt64Collection(this byte[] encoded)
    {
        var len = encoded.DecodeCompactInt32(out var collectionLength);
        return encoded[len..(int)collectionLength].Chunk(8).Select(c => c.DecodeUInt64()).ToArray();
    }
}