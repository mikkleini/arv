using CalcBase.Tokens;
using CalcBase;
using System.Diagnostics;
using System.Numerics;
using CalcBase.Units;

namespace CalcBaseTest
{
    public class DevelopmentTests
    {
        public record class TestRecord
        {
            public required string Name { get; init; }
            public required string[] Items { get; init; }
            public required (string a, NumberType b)[] TupleItems { get; init; }
        }

        public record class TestParentRecord
        {
            public required TestRecord Rec { get; init; }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRationals()
        {
            BigRational x;

            x = BigRational.Pow((int)3, (int)3);
            Debug.WriteLine(x.ToString());

            x = BigRational.Pow(new BigRational(3), new BigRational(3));
            Debug.WriteLine(x.ToString());

            x = 10.125M;
            NumberType y = NumberType.NumDen(x, out NumberType.Integer z);
            Debug.WriteLine($"Den: {x} -> {y}/{z}");

            Assert.Pass();
        }

        [Test]
        public void TestRecordEquality()
        {
            TestRecord a = new()
            {
                Name = "A",
                Items = ["1", "2", "3"],
                TupleItems = [("x", 3.21M)]
            };

            TestRecord b = new()
            {
                Name = "B",
                Items = ["4", "5", "6"],
                TupleItems = [("x", 441.2M)]
            };            

            TestRecord[] recs = [a, b];
            TestParentRecord[] parentRecs = recs.Select(r => new TestParentRecord() { Rec = r }).ToArray();
            Assert.That(parentRecs[0].Rec, Is.EqualTo(a));

            (TestRecord r, string n)[] tuples = recs.Select(r => (r, r.Name)).ToArray();            
            Assert.That(tuples[0].r, Is.EqualTo(a));

            SIBaseUnit ua = new("UA", ["u"], Factory.Length, [("x", "y", 10)]);
            SIBaseUnit ub = new("UB", ["u"], Factory.Length, [("x", "y", 20)]);
            IUnit[] units = [ua, ub];
            Assert.That(units[0], Is.EqualTo(ua));

            
            SIBaseUnit s2 = Factory.Second;
            Assert.True(ReferenceEquals(Factory.Second, Factory.Second));
            Assert.True(ReferenceEquals(s2, Factory.Second));

            (IUnit unit, string x)[] mix = Factory.Units.Select(u => (u, u.Name)).ToArray();
            Assert.True(ReferenceEquals(mix[0].unit, s2));
            Assert.True(ReferenceEquals(mix[0].unit, Factory.Second));
            Assert.That(mix[0].unit, Is.EqualTo(Factory.Second));
        }
    }
}
