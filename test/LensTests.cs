using System;
using System.Collections.Immutable;
using Control.Lens;
using Xunit;

namespace ControlTest
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; }
        public int Age { get; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override bool Equals(object obj)
            => Equals(obj as Person);

        public bool Equals(Person p)
            => p != null
                && Name == p.Name
                && Age == p.Age;

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Age);
        }

        public Person With(string name = null, int? age = null)
            => new Person(
                name ?? Name,
                age == null ? Age : age.Value);

        public override string ToString()
            => $"Name: {Name}, Age: {Age}";
    }

    public class LensTests
    {
        [Fact]
        public void Given_List_Of_Persons_And_Composed_Lens_Then_List_Updated()
        {
            var persons = ImmutableArray.Create(
                new Person("John", 33),
                new Person("Ada", 25));

            var name = new Lens<Person, string>(
                p => p.Name,
                n => p => p.With(name: n));
            var pos = Create.Pos<Person>(0); 

            var sut = pos.Compose(name).Set("Oliver");
            var actual = sut(persons);

            var expected = ImmutableArray.Create(
                new Person("Oliver", 33),
                new Person("Ada", 25));

            Assert.Equal(expected, actual);
        }
    }
}
