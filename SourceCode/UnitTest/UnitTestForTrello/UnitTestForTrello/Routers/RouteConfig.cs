using PureDI;
using UnitTestForTrello.Controllers;

namespace UnitTestForTrello.Routers
{
    public static class RouteConfig
    {
        public static Router Create(Func<IServiceScope> scopeFactory) // scopeFactory = factory to create a per-request IServiceScope
        {
            var r = new Router(scopeFactory); // r = router

            MapBoardRoutes(r);

            return r;
        }

        private static void MapBoardRoutes(Router r) // r = router
        {
            r.Map("GET", "/boards/starred/{userId}", (rv, sp) => // rv = route values, sp = service provider
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetStarredBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map("GET", "/boards/recent/{userId}", (rv, sp) => // rv = route values, sp = service provider
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetRecentlyBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map("GET", "/workspaces/{workspaceId}/users/{userId}/boards/member", (rv, sp) =>
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetBoardsWhereUserIsMemberInWorkspace(
                    int.Parse(rv["userId"]),           // userId from path
                    int.Parse(rv["workspaceId"]));     // workspaceId from path
            });

            r.Map("GET", "/workspaces/{workspaceId}/users/{userId}/boards/owner", (rv, sp) =>
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetBoardsWhereUserIsOwnerInWorkspace(
                    int.Parse(rv["userId"]),           // userId from path
                    int.Parse(rv["workspaceId"]));     // workspaceId from path
            });
        }
    }
}
