namespace Shared.Constants.Extensions;

public static class ObjectExtensions
{
    public static T GetPropertyValue<T>(this object obj, string name)
    {
        try
        {
            var property = obj.GetType().GetProperty(name);
            if (property is null) return default(T);

            var result = (T)property.GetValue(obj, null);
            return result;
        }
        catch (Exception)
        {
            //ignored
            return default(T);
        }
    }
}