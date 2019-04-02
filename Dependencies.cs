using CloneDB.BAL;
using CloneDB.DAL;
using CloneDB.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CloneDB
{
    public static class Dependencies
    {
        /// <summary>
        /// Extension method to inject all dependencies to make Startup.cs cleaner
        /// </summary>
        /// <param name="services"></param>
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IDbFactory, DbFactory>();
            services.AddSingleton<IMoviesRepository, MoviesRepository>();
            services.AddSingleton<IPeopleRepository, PeopleRepository>();
            services.AddSingleton<IMovies, Movies>();
            services.AddSingleton<IPeople, People>();
            services.AddSingleton<IImageWriter, ImageWriter>();
        }
    }
}