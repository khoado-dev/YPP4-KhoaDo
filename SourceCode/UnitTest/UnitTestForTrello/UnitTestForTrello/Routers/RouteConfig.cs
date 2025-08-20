using UnitTestForTrello.Controllers;
using UnitTestForTrello.CustomDI;
using UnitTestForTrello.Tests;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Routers
{
    public static class RouteConfig
    {
        public static Router Create() // scopeFactory = factory to create a per-request IServiceScope
        {
            var r = new Router(); // r = router

            MapBoardRoutes(r);
            MapCardRoutes(r);
            MapMemberRoutes(r);
            MapWorkspaceRoutes(r);
            MapUserRoutes(r);

            return r;
        }

        private static void MapBoardRoutes(Router r) // r = router
        {
            r.Map(RequestMethod.GET, "/boards/starred", (rv) => // rv = route values = service provider
            {
                var c = ReflectionFactory.Get<BoardController>(); // c = controller
                return c.GetStarredBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map(RequestMethod.GET, "/boards/recent", (rv) => // rv = route values = service provider
            {
                var c =ReflectionFactory.Get<BoardController>(); // c = controller
                return c.GetRecentBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map(RequestMethod.GET, "/boards", rv => {
                var c = ReflectionFactory.Get<BoardController>();
                var ws = int.Parse(rv["workspaceId"]);
                var u = int.Parse(rv["userId"]);
                var mem = rv.TryGetValue("membership", out var m) ? m : "member"; //default to "member"
                return mem.Equals("owner", StringComparison.OrdinalIgnoreCase)
                    ? c.GetBoardsAsOwner(u, ws) // if membership is owner, get boards as owner
                    : c.GetBoardsAsMember(u, ws); // otherwise, get boards as member
            });
        }
        private static void MapCardRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/cards/{cardId}/detail", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardDetailByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/boards/{boardId}/cards/detail", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardDetailsByBoardId(int.Parse(rv["boardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/labels", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardLabelsByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/comments/reactions", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();

                return c.GetCardCommentsAndReactionsCountByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/activities", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();

                return c.GetActivitiesByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/custom-fields", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();

                return c.GetCustomFieldsByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/custom-field-values", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();

                return c.GetCustomFieldValuesByCardId(int.Parse(rv["cardId"]));
            });

            r.Map(RequestMethod.GET, "/cards/{cardId}/attachments", (rv) =>
            {
                var c = ReflectionFactory.Get<CardController>();

                return c.GetAttachmentsByCardId(int.Parse(rv["cardId"]));
            });
        }
        private static void MapMemberRoutes(Router r)
        {
            // Lấy tất cả member trong một workspace
            r.Map(RequestMethod.GET, "/workspaces/{workspaceId}/members", (rv) =>
            {
                var c = ReflectionFactory.Get<MemberController>();

                return c.GetMembersByWorkspaceId(int.Parse(rv["workspaceId"]));
            });
            // Lấy tất cả member trong một board
            r.Map(RequestMethod.GET, "/boards/{boardId}/members", (rv) =>
            {
                var c = ReflectionFactory.Get<MemberController>();

                return c.GetMembersByBoardId(int.Parse(rv["boardId"]));
            });

            // Lấy member theo card
            r.Map(RequestMethod.GET, "/cards/{cardId}/members", 
                (rv) =>
            {
                var c = ReflectionFactory.Get<MemberController>();

                return c.GetMembersByCardId(int.Parse(rv["cardId"]));
            });

            // Lấy danh sách selectable members cho card (để assign)
            r.Map(RequestMethod.GET, "/cards/{cardId}/members/selectable", (rv) =>
            {
                var c = ReflectionFactory.Get<MemberController>();

                return c.GetSelectableMembersByCardId(int.Parse(rv["cardId"]));
            });

            // Lấy tất cả role-permissions
            r.Map(RequestMethod.GET, "/rolepermissions", (rv) =>
            {
                var c = ReflectionFactory.Get<MemberController>();

                return c.GetRolePermissions();
            });

        }
        private static void MapWorkspaceRoutes(Router r)
        {
            // 1) Workspaces by user
            r.Map(RequestMethod.GET, "/users/{userId}/workspaces", (rv) =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();

                return c.GetWorkspacesByUserId(int.Parse(rv["userId"]));
            });

            // 2) Workspace types
            r.Map(RequestMethod.GET, "/workspaces/types", (rv) =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();

                return c.GetWorkspaceTypes();
            });

            // 3) Workspace detail
            r.Map(RequestMethod.GET, "/workspaces/{workspaceId}/detail", (rv) =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();

                return c.GetWorkspaceDetailById(int.Parse(rv["workspaceId"]));
            });
        }
        private static void MapUserRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/users/{email}", (rv) =>
            {
                var c = ReflectionFactory.Get<UserController>();
                var email = rv["email"]; // router của bạn đã merge path + query

                return c.GetUserByEmail(email);
            });
        }
    }
}
