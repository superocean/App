using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XPGroup.Models;

namespace XPGroup.Areas.Admin.ViewModels
{
    public class AttributeView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<AttributeOptionView> Attributes { get; set; }
    }

    public class AttributeOptionView
    {
        public int AttributeOptionId { get; set; }

        public string Value { get; set; }
    }

    public class AttributeSaved
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}