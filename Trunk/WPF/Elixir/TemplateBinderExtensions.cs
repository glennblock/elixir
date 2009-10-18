using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Linq.Expressions;

namespace Elixir
{
    public static class TemplateBinderExtensions
    {
        public static IDependencyPropertyBinderDefinition GetPropertyBinderDefinition(this ITemplateBinder templateBinder, string viewElementName, DependencyProperty dependencyProperty)
        {
            return templateBinder.BinderDefinitions
                .OfType<IDependencyPropertyBinderDefinition>()
                .Where(b => b.Property == dependencyProperty)
                .SingleOrDefault();
        }

    }
}
