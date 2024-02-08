using Application.Constants;

namespace WebApi.Auth.Policies
{
    public static class MainPoliciesInjecttion
    {
        public static IServiceCollection AddMainPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(MainPolicies.PolicyAdmins, option => option.RequireRole(SystemRoles.Admin));
                options.AddPolicy(MainPolicies.PolicyOperators, option => option.RequireRole(SystemRoles.Operator));
                options.AddPolicy(MainPolicies.PolicyEverythingAllowed, option => 
                    option.RequireRole(SystemRoles.Operator, SystemRoles.Admin));
            });
            return services;
        }
    }
}
