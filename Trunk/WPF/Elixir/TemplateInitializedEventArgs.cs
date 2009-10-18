using System;
using System.Windows;

namespace Elixir
{
    public class  TemplateInitializedEventArgs : EventArgs
    {
        public TemplateInitializedEventArgs(string template, FrameworkElement templateContent)
        {
            this.Template = template;
            this.TemplateContent = templateContent;
        }

        public string Template { get; private set; }
        public FrameworkElement TemplateContent { get; private set; }
    }
}