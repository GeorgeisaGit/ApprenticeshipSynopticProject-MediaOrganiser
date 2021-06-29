namespace MediaOrganiser.Config
{
    /// <summary>
    /// This enum provides a restricted list of sorting operations that a consumer can choose from.
    /// </summary>
    public enum Sort
    {
        //We are assigning numbers to the values to ensure any additions do not affect consumers referring to enum
        // by number as opposed to value.
        NameAsc = 0,
        NameDesc = 1,
        DateAsc = 2,
        DateDesc = 3
    }
}