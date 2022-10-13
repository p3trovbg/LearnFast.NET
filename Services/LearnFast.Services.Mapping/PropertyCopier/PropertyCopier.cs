namespace LearnFast.Services.Mapping.PropertyMatcher
{
    public class Matcher<TParent, TChild>
        where TParent : class
        where TChild : class
    {
        public static void CopyPropertiesFrom(TParent parent, TChild child)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = child.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name &&
                        fromProperty.PropertyType == toProperty.PropertyType &&
                        fromProperty.Name != "Owner")
                    {
                        toProperty.SetValue(child, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
    }
}
