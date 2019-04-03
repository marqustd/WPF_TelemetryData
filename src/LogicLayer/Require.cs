using System;

namespace LogicLayer
{
    public static class Require
    {
        public static void NotNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotEmpty(object[] objs, string paramName)
        {
            NotNull(objs, paramName);

            if (objs.Length == 0)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotEmpty(string obj, string paramName)
        {
            NotNull(obj, paramName);

            if (string.IsNullOrWhiteSpace(obj))
            {
                throw new ArgumentNullException();
            }
        }
    }
}