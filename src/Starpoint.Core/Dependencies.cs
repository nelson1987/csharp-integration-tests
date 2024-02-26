using Microsoft.Extensions.DependencyInjection;

namespace Starpoint.Core
{
    public static class Dependencies
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IInclusaoTransferenciaHandler, InclusaoTransferenciaHandler>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
            return services;
        }
    }
}