﻿namespace Castle.MonoRail.Tests.Serializers.Form
{
    using Castle.MonoRail.Serialization;
    using NUnit.Framework;

    [TestFixture]
    public class FormBasedSerializerTests
    {
        [Test]
        public void Deserialize_WhenEmptyInput_JustInstantiateModel()
        {
            var ctx = new HttpContextStub();
            
            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new StubModelMetadataProvider(null));
            
            Assert.IsNotNull(model);
        }

        [Test]
        public void Deserialize_WithDepth0StringInput_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[name]"] = "hammett";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new StubModelMetadataProvider(null));

            Assert.AreEqual("hammett", model.Name);
        }

        [Test]
        public void Deserialize_WithDepth0Int32Input_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[age]"] = "32";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new StubModelMetadataProvider(null));

            Assert.AreEqual(32, model.Age);
        }

        [Test]
        public void Deserialize_WithDepth1StringInput_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[address][city]"] = "kirkland";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new StubModelMetadataProvider(null));

            Assert.IsNotNull(model.Address);
            Assert.AreEqual("kirkland", model.Address.City);
        }

        class Address
        {
            public string City { get; set; }
        }

        class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
        }
    }
}
