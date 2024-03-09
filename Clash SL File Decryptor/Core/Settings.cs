using System;
using System.Reflection;

namespace CSFD.Core
{
    class Settings
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();

        public static bool? ScVersion = true;
        public static string GetVersion()
        {
            return assembly.GetName().Version.ToString();
        }
        public static string GetCopyright()
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            // Get all custom attributes of the assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            // Check if the copyright attribute exists
            if (attributes.Length > 0)
            {
                AssemblyCopyrightAttribute copyrightAttribute = (AssemblyCopyrightAttribute)attributes[0];
                return copyrightAttribute.Copyright;
            }
            else
            {
                return "";
            }
        }
    }
}
