namespace PolkadotNET.SCALE.Tests;

public class IntegerTests
{
    [Test]
    public void OneByte()
    {
        Assert.That(((byte)200).Encode(), Is.EqualTo(new[] { (byte)0xC8 }));
        Assert.That(((sbyte)-100).Encode(), Is.EqualTo(new[] { (byte)0x9C }));

        Assert.That(((byte)0xC8).DecodeByte(), Is.EqualTo(200));
        Assert.That(((byte)0x9C).DecodeSignedByte(), Is.EqualTo(-100));
    }

    [Test]
    public void TwoByte()
    {
        Assert.That(((ushort)4500).Encode(), Is.EqualTo(new byte[] { 0x94, 0x11 }));
        Assert.That(((short)-1230).Encode(), Is.EqualTo(new byte[] { 0x32, 0xFB }));

        Assert.That(new byte[] { 0x94, 0x11 }.DecodeUInt16(), Is.EqualTo(4500));
        Assert.That(new byte[] { 0x32, 0xFB }.DecodeInt16(), Is.EqualTo(-1230));
    }

    [Test]
    public void FourByte()
    {
        Assert.That(((uint)205000).Encode(), Is.EqualTo(new byte[] { 0xC8, 0x20, 0x03, 0x00 }));
        Assert.That(((int)-111230).Encode(), Is.EqualTo(new byte[] { 0x82, 0x4D, 0xFE, 0xFF }));

        Assert.That(new byte[] { 0xC8, 0x20, 0x03, 0x00 }.DecodeUInt32(), Is.EqualTo(205000));
        Assert.That(new byte[] { 0x82, 0x4D, 0xFE, 0xFF }.DecodeInt32(), Is.EqualTo(-111230));
    }

    [Test]
    public void EightByte()
    {
        Assert.That(((ulong)2050001234).Encode(),
            Is.EqualTo(new byte[] { 0x52, 0x89, 0x30, 0x7A, 0x00, 0x00, 0x00, 0x00 }));
        Assert.That(((long)-111236120).Encode(),
            Is.EqualTo(new byte[] { 0xE8, 0xAB, 0x5E, 0xF9, 0xFF, 0xFF, 0xFF, 0xFF }));

        Assert.That(new byte[] { 0x52, 0x89, 0x30, 0x7A, 0x00, 0x00, 0x00, 0x00 }.DecodeUInt64(),
            Is.EqualTo(2050001234));
        Assert.That(new byte[] { 0xE8, 0xAB, 0x5E, 0xF9, 0xFF, 0xFF, 0xFF, 0xFF }.DecodeInt64(),
            Is.EqualTo(-111236120));
    }

    [Test]
    public void Bool()
    {
        Assert.That(true.Encode(), Is.EqualTo(new byte[] { 0x01 }));
        Assert.That(false.Encode(), Is.EqualTo(new byte[] { 0x00 }));

        Assert.That(((byte)0x01).DecodeBoolean(), Is.EqualTo(true));
        Assert.That(((byte)0x00).DecodeBoolean(), Is.EqualTo(false));

        Assert.Throws<ScaleEncodingException>(() => ((byte)0x02).DecodeBoolean());
    }

    enum ValidEnumeration : byte
    {
        First = 1,
        Second = 2,
        Third = 3
    }

    enum InvalidEnumeration : ushort
    {
        Foo = 123
    }

    [Test]
    public void Enum()
    {
        Assert.That(ValidEnumeration.Second.Encode(), Is.EqualTo((byte)2));
        
        Assert.That(((byte)2).DecodeEnum<ValidEnumeration>(), Is.EqualTo(ValidEnumeration.Second));
        Assert.Throws<ScaleEncodingException>(() => InvalidEnumeration.Foo.Encode());
        Assert.Throws<ScaleEncodingException>(() => ((byte)2).DecodeEnum<InvalidEnumeration>());
    }
}