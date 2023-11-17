namespace PolkadotNET.SCALE.Tests;

public class StringTests
{
    [Test]
    public void Encode()
    {
        const string text = "Hello World";
        var encoded = text.Encode();

        Assert.That(encoded, Is.EqualTo(new byte[] { 44, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }));
    }

    [Test]
    public void Decode()
    {
        var encoded = new byte[]
        {
            44, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100
        };

        var text = encoded.DecodeString();
        Assert.That(text, Is.EqualTo("Hello World"));
    }

    [Test]
    public void InvalidUtf8ThrowsException()
    {
    }
}