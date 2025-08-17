using PureDI;
using UnitTestForTrello.Controllers;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

namespace UnitTestForTrello.Routers
{
    public static class RouteConfig
    {
        public static Router Create(Func<IServiceScope> scopeFactory) // scopeFactory = factory to create a per-request IServiceScope
        {
            var r = new Router(scopeFactory); // r = router

            MapBoardRoutes(r);
            MapCardRoutes(r);
            MapMemberRoutes(r);
            MapWorkspaceRoutes(r);
            MapUserRoutes(r);

            return r;
        }

        private static void MapBoardRoutes(Router r) // r = router
        {
            r.Map(HttpMethod.GET, "/boards/starred/{userId?}", (rv, sp) => // rv = route values, sp = service provider
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetStarredBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map(HttpMethod.GET, "/boards/recent/{userId?}", (rv, sp) => // rv = route values, sp = service provider
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetRecentlyBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map(HttpMethod.GET, "/workspaces/{workspaceId?}/users/{userId?}/boards/member", (rv, sp) =>
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!;
                var ws = int.Parse(rv["workspaceId"]);
                var u = int.Parse(rv["userId"]); // có thể đến từ path HOẶC params query
                return c.GetBoardsWhereUserIsMemberInWorkspace(u, ws);
            });


            r.Map(HttpMethod.GET, "/workspaces/{workspaceId?}/users/{userId?}/boards/owner", (rv, sp) =>
            {
                var c = (BoardController)sp.GetService(typeof(BoardController))!; // c = controller
                return c.GetBoardsWhereUserIsOwnerInWorkspace(
                    int.Parse(rv["userId"]),           // userId from path
                    int.Parse(rv["workspaceId"]));     // workspaceId from path
            });
        }
        private static void MapCardRoutes(Router r)
        {
            r.Map(HttpMethod.GET, "/cards/{cardId?}/detail", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCardDetailByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/boards/{boardId?}/cards/detail", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCardDetailsByBoardId(int.Parse(rv["boardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/labels", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCardLabelsByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/comments/reactions", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCardCommentsAndReactionsCountByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/activities", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetActivitiesByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/custom-fields", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCustomFieldsByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/custom-field-values", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetCustomFieldValuesByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(HttpMethod.GET, "/cards/{cardId?}/attachments", (rv, sp) =>
            {
                var c = (CardController)sp.GetService(typeof(CardController))!;
                return c.GetAttachmentsByCardId(int.Parse(rv["cardId"]));
            });
        }
        private static void MapMemberRoutes(Router r)
        {
            // Lấy tất cả member trong một board
            r.Map(HttpMethod.GET, "/boards/{boardId?}/members", (rv, sp) =>
            {
                var c = (MemberController)sp.GetService(typeof(MemberController))!;
                return c.GetMembersByBoardId(int.Parse(rv["boardId"]));
            });

            // Lấy member theo card
            r.Map(HttpMethod.GET, "/cards/{cardId?}/members", (rv, sp) =>
            {
                var c = (MemberController)sp.GetService(typeof(MemberController))!;
                return c.GetMembersByCardId(int.Parse(rv["cardId"]));
            });

            // Lấy danh sách selectable members cho card (để assign)
            r.Map(HttpMethod.GET, "/cards/{cardId?}/members/selectable", (rv, sp) =>
            {
                var c = (MemberController)sp.GetService(typeof(MemberController))!;
                return c.GetSelectableMembersByCardId(int.Parse(rv["cardId"]));
            });
        }
        private static void MapWorkspaceRoutes(Router r)
        {
            // 1) Workspaces by user
            r.Map(HttpMethod.GET, "/users/{userId?}/workspaces", (rv, sp) =>
            {
                var c = (WorkspaceController)sp.GetService(typeof(WorkspaceController))!;
                return c.GetWorkspacesByUserId(int.Parse(rv["userId"]));
            });

            // 2) Workspace types
            r.Map(HttpMethod.GET, "/workspaces/types", (rv, sp) =>
            {
                var c = (WorkspaceController)sp.GetService(typeof(WorkspaceController))!;
                return c.GetWorkspaceTypes();
            });

            // 3) Workspace detail
            r.Map(HttpMethod.GET, "/workspaces/{workspaceId?}/detail", (rv, sp) =>
            {
                var c = (WorkspaceController)sp.GetService(typeof(WorkspaceController))!;
                return c.GetWorkspaceDetailById(int.Parse(rv["workspaceId"]));
            });
        }
        private static void MapUserRoutes(Router r)
        {
            // Lấy user theo email (email có thể đến từ path hoặc query)
            // Ví dụ: /users/by-email/james@abc.com  hoặc  /users/by-email?email=james@abc.com
            r.Map(HttpMethod.GET, "/users/{email?}", (rv, sp) =>
            {
                var c = (UserController)sp.GetService(typeof(UserController))!;
                var email = rv["email"]; // router của bạn đã merge path + query
                return c.GetUserByEmail(email);
            });
        }
    }
}
