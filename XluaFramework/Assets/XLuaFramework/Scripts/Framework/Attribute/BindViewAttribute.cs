using System;

namespace XLuaFramework
{
    [AttributeUsage(
    AttributeTargets.Field,
    AllowMultiple = false)]
    public class BindViewAttribute : System.Attribute
    {
        private string view;
        public BindViewAttribute(string view)
        {
            this.view = view;
        }

        public string View
        {
            get
            {
                return view;
            }
        }
    }
}