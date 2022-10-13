namespace LearnFast.Services.Mapping.PropertyMatcher
{
    using System.Reflection;

    using LearnFast.Services.Mapping.PropertyCopier;

    public class PropertyCopier<TParent, TChild>
        where TParent : class
        where TChild : class
    {
        public static void CopyPropertiesFrom(TParent parent, TChild child)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = child.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                var isNotCopy = fromProperty.GetCustomAttribute(typeof(NotCopyAttribute)) != null;

                if (isNotCopy)
                {
                    continue;
                }

                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name &&
                        fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(child, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
    }
}
