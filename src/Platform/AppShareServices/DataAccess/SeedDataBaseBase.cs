namespace AppShareServices.DataAccess
{
    public abstract class SeedDataBaseBase
    {

        /// <summary>
        /// Should be override implement
        /// </summary>
        /// <returns></returns>
        public virtual async Task SeedAsync();
    }
}