﻿using System;
using System.Collections.Generic;
using Atlas.Domain.Categories;

namespace Atlas.Domain.Sites
{
    public class Site
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string PublicTheme { get; private set; }
        public string PublicCss { get; private set; }
        public string AdminTheme { get; private set; }
        public string AdminCss { get; private set; }
        public string Language { get; private set; }

        public virtual ICollection<Category> Categories { get; set; }

        public Site()
        {

        }

        public Site(string name, string title)
        {
            New(Guid.NewGuid(), name, title);
        }

        public Site(Guid id, string name, string title)
        {
            New(id, name, title);
        }

        private void New(Guid id, string name, string title)
        {
            Id = id;
            Name = name;
            Title = title;
            PublicTheme = "Default";
            PublicCss = "public.css";
            AdminTheme = "Default";
            AdminCss = "admin.css";
            Language = "en";
        }

        public void UpdateDetails(string title, string theme, string css, string language)
        {
            Title = title;
            PublicTheme = theme;
            PublicCss = css;
            Language = language;
        }
    }
}
