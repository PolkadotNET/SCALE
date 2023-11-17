namespace PolkadotNET.SCALE;

public class ScaleEncodingException : Exception
{
    public ScaleEncodingException(string message) : base(message)
    {
    }
}

public static class IntegerEncoding
{
    // Simple
    // Bool
    // Enum

    // Integer
    public static byte[] Encode(this bool value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this byte value) => new[] { value };
    public static byte[] Encode(this sbyte value) => new[] { (byte)value };
    public static byte[] Encode(this ushort value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this short value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this uint value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this int value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this ulong value) => BitConverter.GetBytes(value);
    public static byte[] Encode(this long value) => BitConverter.GetBytes(value);

    public static byte Encode<TEnum>(this TEnum value) where TEnum : Enum
    {
        var valueType = typeof(TEnum).GetField("value__");
        if (valueType == null || valueType.FieldType != typeof(byte))
            throw new ScaleEncodingException("Enum must be of byte size");

        return (byte)(object)value;
    }

    // Integer
    public static bool DecodeBoolean(this byte value)
        => value switch
        {
            0x00 => false,
            0x01 => true,
            _ => throw new ScaleEncodingException("value must be either 0x01 or 0x00")
        };

    public static byte DecodeByte(this byte value) => value;
    public static sbyte DecodeSignedByte(this byte value) => (sbyte)value;
    public static ushort DecodeUInt16(this byte[] value) => BitConverter.ToUInt16(value);
    public static short DecodeInt16(this byte[] value) => BitConverter.ToInt16(value);
    public static uint DecodeUInt32(this byte[] value) => BitConverter.ToUInt32(value);
    public static int DecodeInt32(this byte[] value) => BitConverter.ToInt32(value);
    public static ulong DecodeUInt64(this byte[] value) => BitConverter.ToUInt64(value);
    public static long DecodeInt64(this byte[] value) => BitConverter.ToInt64(value);

    public static T DecodeEnum<T>(this byte encoded) where T : Enum
    {
        // ensure target enum is byte-sized
        var valueType = typeof(T).GetField("value__");
        if (valueType == null || valueType.FieldType != typeof(byte))
            throw new ScaleEncodingException("Enum must be of byte size");

        return (T)(object)encoded;
    }
}