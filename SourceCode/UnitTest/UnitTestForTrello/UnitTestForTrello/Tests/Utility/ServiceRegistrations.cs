using PureDI;
using UnitTestForTrello.Controllers;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Tests.Utility
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddBoardModule(this IServiceCollection s)
            => s.AddScoped<IBoardRepository, BoardRepository>()
                .AddScoped<IBoardService, BoardService>()
                .AddTransient<BoardController>();

        public static IServiceCollection AddCardModule(this IServiceCollection s)
            => s.AddScoped<ICardRepository, CardRepository>()
                .AddScoped<ICardService, CardService>()
                .AddTransient<CardController>();

        public static IServiceCollection AddMemberModule(this IServiceCollection s)
            => s.AddScoped<IMemberRepository, MemberRepository>()
                .AddScoped<IMemberService, MemberService>()
                .AddTransient<MemberController>();


        public static IServiceCollection AddWorkspaceModule(this IServiceCollection s)
            => s.AddScoped<IWorkspaceRepository, WorkspaceRepository>()
                .AddScoped<IWorkspaceService, WorkspaceService>()
                .AddTransient<WorkspaceController>();


        public static IServiceCollection AddUserModule(this IServiceCollection s)
            => s.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>()
                .AddTransient<UserController>();
    }
}
