using PolkadotNET.SCALE.Types;

namespace PolkadotNET.SCALE.Tests;

public class CompactTests
{
    [TestCase((byte)1, new byte[] { 0x04 }, 1 )]
    [TestCase((byte)64, new byte[] { 0x01, 0x01 }, 2 )]
    public void OneByte(byte plain, byte[] encoded, int readLen)
    {
        Assert.That(new CompactByte(plain).Encode(), Is.EqualTo(encoded));
        
        var len = encoded.DecodeCompactByte(out var decoded);
        Assert.That(len, Is.EqualTo(readLen));
        Assert.That(decoded.Value, Is.EqualTo(plain));
    }

    [TestCase((ushort)1, new byte[] { 0x04 }, 1 )]
    [TestCase((ushort)64, new byte[] { 0x01, 0x01 }, 2 )]
    [TestCase((ushort)16384, new byte[] { 0x02, 0x00, 0x01, 0x00 }, 4 )]
    public void TwoByte(ushort plain, byte[] encoded, int readLen)
    {
        Assert.That(new CompactUInt16(plain).Encode(), Is.EqualTo(encoded));
        
        var len = encoded.DecodeCompactUInt16(out var decoded);
        Assert.That(len, Is.EqualTo(readLen));
        Assert.That(decoded.Value, Is.EqualTo(plain));
    }
    
    [TestCase((uint)1, new byte[] { 0x04 }, 1 )]
    [TestCase((uint)64, new byte[] {  0x01, 0x01 }, 2 )]
    [TestCase((uint)16384, new byte[] {  0x02, 0x00, 0x01, 0x00 }, 4 )]
    [TestCase((uint)1073741824, new byte[] { 0x03, 0x00, 0x00, 0x00, 0x40 }, 5 )]
    public void FourByte(uint plain, byte[] encoded, int readLen)
    {
        Assert.That(new CompactUInt32(plain).Encode(), Is.EqualTo(encoded));
        
        var len = encoded.DecodeCompactUInt32(out var decoded);
        Assert.That(len, Is.EqualTo(readLen));
        Assert.That(decoded.Value, Is.EqualTo(plain));
    }
    [TestCase((ulong)1, new byte[] { 0x04 }, 1 )]
    [TestCase((ulong)64, new byte[] {  0x01, 0x01 }, 2 )]
    [TestCase((ulong)16384, new byte[] {  0x02, 0x00, 0x01, 0x00 }, 4 )]
    [TestCase((ulong)1073741824, new byte[] { 0x03, 0x00, 0x00, 0x00, 0x40 }, 5 )]
    [TestCase((ulong)(1L << 56), new byte[] {  0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, 9 )]
    public void EightByte(ulong plain, byte[] encoded, int readLen)
    {
        Assert.That(new CompactUInt64(plain).Encode(), Is.EqualTo(encoded));
        
        var len = encoded.DecodeCompactUInt64(out var decoded);
        Assert.That(len, Is.EqualTo(readLen));
        Assert.That(decoded.Value, Is.EqualTo(plain));
    }
}