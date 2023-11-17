namespace PolkadotNET.SCALE.Tests;

public class EnumTests
{
    enum Foo : byte
    {
        First = 1,
        Second = 2,
        Third = 3
    }

    [Test]
    public void Encode()
    {
        var encoded = Foo.Second.Encode();
        Assert.That(encoded, Is.EqualTo((byte)2));
    }

    [Test]
    public void Decode()
    {
        var encoded = (byte)0x03;
        var decoded = encoded.DecodeEnum<Foo>();

        Assert.That(decoded, Is.EqualTo(Foo.Third));
    }

    enum Bar : ushort
    {
        Something = 1
    }

    [Test]
    public void ThrowIfNotByteSized()
    {
        Assert.Throws<ScaleEncodingException>(() => Bar.Something.Encode());
        Assert.Throws<ScaleEncodingException>(() => ((byte)0x03).DecodeEnum<Bar>());
    }
}