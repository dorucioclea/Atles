﻿using System;
using Atlas.Domain.Sites;
using AutoFixture;
using NUnit.Framework;

namespace Atlas.Domain.Tests.Sites
{
    [TestFixture]
    public class SiteTests : TestFixtureBase
    {
        [Test]
        public void New()
        {
            const string name = "Default";
            const string title = "My Site";

            var sut = new Site(name, title);

            Assert.AreEqual(name, sut.Name, nameof(sut.Name));
            Assert.AreEqual(title, sut.Title, nameof(sut.Title));
            Assert.AreEqual("Default", sut.PublicTheme, nameof(sut.PublicTheme));
            Assert.AreEqual("public.css", sut.PublicCss, nameof(sut.PublicCss));
            Assert.AreEqual("Default", sut.AdminTheme, nameof(sut.AdminTheme));
            Assert.AreEqual("admin.css", sut.AdminCss, nameof(sut.AdminCss));
            Assert.AreEqual("en", sut.Language, nameof(sut.Language));
        }

        [Test]
        public void New_passing_id()
        {
            var id = Guid.NewGuid();
            const string name = "Default";
            const string title = "My Site";

            var sut = new Site(id, name, title);

            Assert.AreEqual(id, sut.Id, nameof(sut.Id));
            Assert.AreEqual(name, sut.Name, nameof(sut.Name));
            Assert.AreEqual(title, sut.Title, nameof(sut.Title));
            Assert.AreEqual("Default", sut.PublicTheme, nameof(sut.PublicTheme));
            Assert.AreEqual("public.css", sut.PublicCss, nameof(sut.PublicCss));
            Assert.AreEqual("Default", sut.AdminTheme, nameof(sut.AdminTheme));
            Assert.AreEqual("admin.css", sut.AdminCss, nameof(sut.AdminCss));
            Assert.AreEqual("en", sut.Language, nameof(sut.Language));
        }

        [Test]
        public void Update_details()
        {
            var sut = Fixture.Create<Site>();

            const string title = "New Title";
            const string theme = "New Theme";
            const string css = "New CSS";
            const string language = "it";

            sut.UpdateDetails(title, theme, css, language);

            Assert.AreEqual(title, sut.Title, nameof(sut.Title));
            Assert.AreEqual(theme, sut.PublicTheme, nameof(sut.PublicTheme));
            Assert.AreEqual(css, sut.PublicCss, nameof(sut.PublicCss));
            Assert.AreEqual(language, sut.Language, nameof(sut.Language));
        }
    }
}
