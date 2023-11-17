using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using PolkadotNET.SCALE.Types;

namespace PolkadotNET.SCALE;

public static class CollectionEncoding
{
    public static byte[] Encode(this string value)
    {
        var asBytes = Encoding.UTF8.GetBytes(value);
        var lenCompact = new CompactInt32(value.Length).Encode();

        return lenCompact.Concat(asBytes).ToArray();
    }
    
    public static string DecodeString(this byte[] encoded)
    {
        var readLen = encoded.DecodeCompactInt32(out var compact);
        return Encoding.UTF8.GetString(encoded, readLen, compact.Value);
    }
}

public static class StructEncoding
{
    private static byte[] Encode<T>(T value) where T: new()
    {
        var buffer = new List<byte>();
        var type = typeof(T);
        foreach (var propertyInfo in type.GetProperties())
        {
            var propertyValue = propertyInfo.GetValue(value);
            if (propertyValue == null)
                throw new Exception("cannot encode NULL value. use the Option struct instead");

            if (propertyInfo.PropertyType == typeof(bool))
                buffer.AddRange(((bool)propertyValue).Encode());
            if (propertyInfo.PropertyType == typeof(byte))
                buffer.AddRange(((byte)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactByte))
                buffer.AddRange(((CompactByte)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(sbyte))
                buffer.AddRange(((sbyte)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactSignedByte))
                buffer.AddRange(((CompactSignedByte)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(short))
                buffer.AddRange(((short)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactInt16))
                buffer.AddRange(((CompactInt16)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(ushort))
                buffer.AddRange(((ushort)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactUInt16))
                buffer.AddRange(((CompactUInt16)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(int))
                buffer.AddRange(((int)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactInt64))
                buffer.AddRange(((CompactInt64)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(uint))
                buffer.AddRange(((uint)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactUInt32))
                buffer.AddRange(((CompactUInt32)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(long))
                buffer.AddRange(((long)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactInt64))
                buffer.AddRange(((CompactInt64)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(ulong))
                buffer.AddRange(((ulong)propertyValue).Encode());
            else if (propertyInfo.PropertyType == typeof(CompactUInt64))
                buffer.AddRange(((CompactUInt64)propertyValue).Encode());
            else if (propertyInfo.PropertyType.IsEnum)
                buffer.Add(((Enum)propertyValue).Encode());
            else
                throw new Exception("Unknown Property Type");
        }
        
        return buffer.ToArray();
    }
}