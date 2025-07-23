using System.Text.RegularExpressions;

namespace AssetRipper.Checksum.Tests;

public partial class ReversalTests
{
	[GeneratedRegex("[H-Wh-w]{6}[HJLN]")]
	private static partial Regex ReverseOutputRegex { get; }

	[Test]
	public void TestReversalSymmetryAndRegexMatching()
	{
		for (int i = 0; i < 50; i++)
		{
			uint hash = RandomHash;
			string reversed = Crc32Algorithm.Reverse(hash);
			using (Assert.EnterMultipleScope())
			{
				Assert.That(Crc32Algorithm.HashAscii(reversed), Is.EqualTo(hash));
				Assert.That(ReverseOutputRegex.IsMatch(reversed));
			}
		}
	}

	[Test]
	public void ReversalLengthIsSeven()
	{
		string reversed = Crc32Algorithm.Reverse(RandomHash);
		Assert.That(reversed, Has.Length.EqualTo(7));
	}

	[TestCase("Prefix_")]
	public void PrefixedReversal_Ascii(string prefix)
	{
		uint hash = RandomHash;
		string reversed = Crc32Algorithm.ReverseAscii(hash, prefix);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(Crc32Algorithm.HashAscii(reversed), Is.EqualTo(hash));
			Assert.That(reversed, Does.StartWith(prefix));
			Assert.That(reversed, Has.Length.EqualTo(7 + prefix.Length));
		}
	}

	[TestCase("Prefix_")]
	[TestCase("\u60A8\u597D", Description = "Hello in Chinese")]
	public void PrefixedReversal_UTF8(string prefix)
	{
		uint hash = RandomHash;
		string reversed = Crc32Algorithm.ReverseUTF8(hash, prefix);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(Crc32Algorithm.HashUTF8(reversed), Is.EqualTo(hash));
			Assert.That(reversed, Does.StartWith(prefix));
			Assert.That(reversed, Has.Length.EqualTo(7 + prefix.Length));
		}
	}

	private static uint RandomHash => TestContext.CurrentContext.Random.NextUInt();
}
