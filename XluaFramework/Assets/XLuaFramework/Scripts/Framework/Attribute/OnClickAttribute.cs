using System;

namespace XLuaFramework
{
    [AttributeUsage(
    AttributeTargets.Method,
    AllowMultiple = false)]
    public class OnClickAttribute : System.Attribute
    {
        private string view;
        public OnClickAttribute(string view)
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