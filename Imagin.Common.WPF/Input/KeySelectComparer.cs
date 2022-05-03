namespace Imagin.Common.Input
{
    public class DefaultKeySelectComparer : IKeySelectComparer
    {
        public bool Compare(object input, string query) => input?.ToString().ToLower().StartsWith(query.ToLower()) == true;
    }
}