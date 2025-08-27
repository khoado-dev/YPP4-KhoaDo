using System.Net;
using UnitTestForTrello.CustomDI;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Services;
using UnitTestForTrello.Services.IServices;
using UnitTestForTrello.Tests;
namespace UnitTestForTrello
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var prefixes = new[] { "http://localhost:5001/" };
            var listener = new HttpListener();

            // --- Repositories ---
            ReflectionFactory.Register<IBoardRepository, BoardRepository>();
            ReflectionFactory.Register<ICardRepository, CardRepository>();
            ReflectionFactory.Register<ICollectionRepository, CollectionRepository>();
            ReflectionFactory.Register<IMemberRepository, MemberRepository>();
            ReflectionFactory.Register<INotificationRepository, NotificationRepository>();
            ReflectionFactory.Register<ISettingRepository, SettingRepository>();
            ReflectionFactory.Register<IStickerRepository, StickerRepository>();
            ReflectionFactory.Register<ITemplateRepository, TemplateRepository>();
            ReflectionFactory.Register<IUserRepository, UserRepository>();
            ReflectionFactory.Register<IWorkspaceRepository, WorkspaceRepository>();

            // --- Services ---
            ReflectionFactory.Register<IBoardService, BoardService>();
            ReflectionFactory.Register<ICardService, CardService>();
            ReflectionFactory.Register<ICollectionService, CollectionService>();
            ReflectionFactory.Register<IMemberService, MemberService>();
            ReflectionFactory.Register<INotificationService, NotificationService>();
            ReflectionFactory.Register<ISettingService, SettingService>();
            ReflectionFactory.Register<IStickerService, StickerService>();
            ReflectionFactory.Register<ITemplateService, TemplateService>();
            ReflectionFactory.Register<IUserService, UserService>();
            ReflectionFactory.Register<IWorkspaceService, WorkspaceService>();

            foreach (var p in prefixes)
            {
                listener.Prefixes.Add(p);
            }
            listener.Start();
            Console.WriteLine($"API listening on {string.Join(", ", prefixes)}");

            TestStartup.AssemblyInit();
            var router = TestStartup.Router;

            while (true)
            {
                var ctx = await listener.GetContextAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (ctx.Request.HttpMethod == "OPTIONS")
                        {
                            HttpResponseAdapter.WriteCors(ctx.Response);
                            ctx.Response.StatusCode = 204;
                            ctx.Response.Close();
                            return;
                        }

                        var reqDto = HttpRequestAdapter.From(ctx.Request);
                        var resDto = router?.Handle(reqDto);

                        await HttpResponseAdapter.WriteJson(ctx.Response, resDto!);
                    }
                    catch (Exception ex)
                    {
                        await HttpResponseAdapter.WriteProblem(ctx.Response, 500, "ServerError", ex.Message);
                    }
                    finally
                    {
                        ctx.Response.Close();
                    }
                });
            }
        }
    }
}