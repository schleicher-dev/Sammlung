using System;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Sammlung.Functional;

namespace Fixtures.Sammlung.Functional
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void Maybe_ImplicitAssignment_ReferenceType()
        {
            Maybe<string> noValue = null;
            Maybe<string> implicitDefaultValue = default;
            Maybe<string> explicitDefaultValue = default(string);
            Maybe<string> value = "ABC";
            var constructedValue = Maybe.From("ABC");
            
            Assert.That(Maybe<string>.Nil.HasValue, Is.False);
            Assert.That(noValue.HasValue, Is.False);
            Assert.That(implicitDefaultValue.HasValue, Is.False);
            Assert.That(explicitDefaultValue.HasValue, Is.False);
            Assert.That(noValue.GetOrDefault(() => "XYZ"), Is.EqualTo("XYZ"));
            Assert.That(implicitDefaultValue.GetOrDefault(() => "XYZ"), Is.EqualTo("XYZ"));
            Assert.That(explicitDefaultValue.GetOrDefault(() => "XYZ"), Is.EqualTo("XYZ"));
            
            Assert.That(noValue.TryGetValue(out _), Is.False);
            Assert.That(implicitDefaultValue.TryGetValue(out _), Is.False);
            Assert.That(explicitDefaultValue.TryGetValue(out _), Is.False);
            
            Assert.That(value.HasValue, Is.True);
            Assert.That(value.GetOrDefault(), Is.EqualTo("ABC"));
            Assert.That(value.TryGetValue(out var xValue), Is.True);
            Assert.That(xValue, Is.EqualTo("ABC"));

            Assert.That(constructedValue.GetOrDefault(), Is.EqualTo("ABC"));
            Assert.That(constructedValue.GetOrDefault(() => "XYZ"), Is.EqualTo("ABC"));
            Assert.That(constructedValue.TryGetValue(out var xConstructedValue), Is.True);
            Assert.That(xConstructedValue, Is.EqualTo("ABC"));
        }

        [Test]
        public void Maybe_Nil_CallsAlternative()
        {
            // Arrange
            Maybe<string> noValue = default(string);
            var valueFunction = Substitute.For<Action<string>>();
            var alternativeFunction = Substitute.For<Action>();
            
            // Act
            noValue.InvokeOrDefault(valueFunction, alternativeFunction);

            // Assert
            valueFunction.DidNotReceive();
            alternativeFunction.Received(1);
        }

        [Test]
        public void Maybe_Not_Nil_CallsValue()
        {
            // Arrange
            Maybe<string> value = "ABC";
            var valueFunction = Substitute.For<Action<string>>();
            var alternativeFunction = Substitute.For<Action>();
            
            // Act
            value.InvokeOrDefault(valueFunction, alternativeFunction);

            // Assert
            valueFunction.Received(1).Invoke(Arg.Is("ABC"));
            alternativeFunction.DidNotReceive();
        }

        [Test]
        public void Maybe_WithValue_Map_StringToTheirLength()
        {
            Maybe<string> strValue = "ABC";
            var intValue = strValue.Map(v => v.Length);
            
            Assert.That(intValue.HasValue, Is.True);
            Assert.That(intValue.GetOrDefault(), Is.EqualTo(3));
        }

        [Test]
        public void Maybe_WithValue_Bind_StringToTheirLength()
        {
            Maybe<string> strValue = "ABC";
            var intValue = strValue.Bind(v => Maybe.From(v.Length));
            
            Assert.That(intValue.HasValue, Is.True);
            Assert.That(intValue.GetOrDefault(), Is.EqualTo(3));
        }

        [Test]
        public void Maybe_Nil_Map_StringToTheirLength()
        {
            Maybe<string> strValue = default(string);
            var intValue = strValue.Map(v => v.Length);
            
            Assert.That(intValue.HasValue, Is.False);
            Assert.That(intValue.GetOrDefault(int.MaxValue), Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void Maybe_Nil_Bind_StringToTheirLength()
        {
            Maybe<string> strValue = default(string);
            var intValue = strValue.Bind(v => Maybe.From(v.Length));
            
            Assert.That(intValue.HasValue, Is.False);
            Assert.That(intValue.GetOrDefault(int.MaxValue), Is.EqualTo(int.MaxValue));
        }
    }
}