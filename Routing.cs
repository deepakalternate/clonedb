using Microsoft.AspNetCore.Builder;

namespace CloneDB
{
    public static class Routing
    {
        /// <summary>
        /// Extension method to define MVC routes for controllers to clean up Startup.cs
        /// </summary>
        /// <param name="app"></param>
        public static void DefineRoutes(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "addmovie",
                    template: "{controller=Movie}/{action=AddMovies}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Movie}/{action=Index}"
                );
            });
        }
    }
}