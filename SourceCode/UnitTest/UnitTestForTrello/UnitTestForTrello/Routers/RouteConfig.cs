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
            MapTemplateRoutes(r);
            MapCollectionRoutes(r);
            MapStickerRoutes(r);
            MapNotificationRoutes(r);
            MapSettingRoutes(r);

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
                var c = ReflectionFactory.Get<BoardController>(); // c = controller
                return c.GetRecentBoards(int.Parse(rv["userId"])); // userId from path
            });

            r.Map(RequestMethod.GET, "/boards", rv =>
            {
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
            // Card detail by cardId
            r.Map(RequestMethod.GET, "/cards/detail", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardDetailByCardId(int.Parse(rv["cardId"]));
            });

            // Card details by boardId
            r.Map(RequestMethod.GET, "/cards/detail/by-board", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardDetailsByBoardId(int.Parse(rv["boardId"]));
            });

            // Labels
            r.Map(RequestMethod.GET, "/cards/labels", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardLabelsByCardId(int.Parse(rv["cardId"]));
            });

            // Comments + Reactions count
            r.Map(RequestMethod.GET, "/cards/comments/reactions", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCardCommentsAndReactionsCountByCardId(int.Parse(rv["cardId"]));
            });

            // Activities
            r.Map(RequestMethod.GET, "/cards/activities", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetActivitiesByCardId(int.Parse(rv["cardId"]));
            });

            // Custom fields (definitions)
            r.Map(RequestMethod.GET, "/cards/custom-fields", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCustomFieldsByCardId(int.Parse(rv["cardId"]));
            });

            // Custom field values
            r.Map(RequestMethod.GET, "/cards/custom-field-values", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetCustomFieldValuesByCardId(int.Parse(rv["cardId"]));
            });

            // Attachments
            r.Map(RequestMethod.GET, "/cards/attachments", rv =>
            {
                var c = ReflectionFactory.Get<CardController>();
                return c.GetAttachmentsByCardId(int.Parse(rv["cardId"]));
            });
        }

        private static void MapMemberRoutes(Router r)
        {
            // Lấy tất cả member trong một workspace
            r.Map(RequestMethod.GET, "/members/by-workspace", rv =>
            {
                var c = ReflectionFactory.Get<MemberController>();
                return c.GetMembersByWorkspaceId(int.Parse(rv["workspaceId"]));
            });

            // Lấy tất cả member trong một board
            r.Map(RequestMethod.GET, "/members/by-board", rv =>
            {
                var c = ReflectionFactory.Get<MemberController>();
                return c.GetMembersByBoardId(int.Parse(rv["boardId"]));
            });

            // Lấy member theo card
            r.Map(RequestMethod.GET, "/members/by-card", rv =>
            {
                var c = ReflectionFactory.Get<MemberController>();
                return c.GetMembersByCardId(int.Parse(rv["cardId"]));
            });

            // Lấy danh sách selectable members cho card (để assign)
            r.Map(RequestMethod.GET, "/members/selectable", rv =>
            {
                var c = ReflectionFactory.Get<MemberController>();
                return c.GetSelectableMembersByCardId(int.Parse(rv["cardId"]));
            });

            // Lấy tất cả role-permissions
            r.Map(RequestMethod.GET, "/rolepermissions", rv =>
            {
                var c = ReflectionFactory.Get<MemberController>();
                return c.GetRolePermissions();
            });
        }

        private static void MapWorkspaceRoutes(Router r)
        {
            // 1) Workspaces by user
            r.Map(RequestMethod.GET, "/workspaces/by-user", rv =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();
                return c.GetWorkspacesByUserId(int.Parse(rv["userId"]));
            });

            // 2) Workspace types
            r.Map(RequestMethod.GET, "/workspaces/types", rv =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();
                return c.GetWorkspaceTypes();
            });

            // 3) Workspace detail
            r.Map(RequestMethod.GET, "/workspaces/detail", rv =>
            {
                var c = ReflectionFactory.Get<WorkspaceController>();
                return c.GetWorkspaceDetailById(int.Parse(rv["workspaceId"]));
            });
        }

        private static void MapUserRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/users/by-email", rv =>
            {
                var c = ReflectionFactory.Get<UserController>();
                var email = rv["email"]; // lấy từ query string ?email=...

                return c.GetUserByEmail(email);
            });
        }
        private static void MapTemplateRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/template-categories", rv =>
            {
                var c = ReflectionFactory.Get<TemplateController>();
                return c.GetAllCategories();
            });

            r.Map(RequestMethod.GET, "/templates/by-category", rv =>
            {
                var c = ReflectionFactory.Get<TemplateController>();
                return c.GetTemplatesByCategory(int.Parse(rv["categoryId"]));
            });

            r.Map(RequestMethod.GET, "/templates/detail", rv =>
            {
                var c = ReflectionFactory.Get<TemplateController>();
                return c.GetTemplateDetail(int.Parse(rv["templateId"]));
            });
        }
        private static void MapCollectionRoutes(Router r)
        {
            // Show board and the collection it belongs to in a specific workspace
            r.Map(RequestMethod.GET, "/boards/with-collections", (rv) =>
            {
                var c = ReflectionFactory.Get<CollectionController>();

                return c.GetBoardsWithCollectionsInWorkspace(int.Parse(rv["workspaceId"]));
            });

            // Show all collections in a specific workspace
            r.Map(RequestMethod.GET, "/collections/by-workspace", rv =>
            {
                var c = ReflectionFactory.Get<CollectionController>();

                return c.GetCollectionsByWorkspace(int.Parse(rv["workspaceId"]));
            });
        }
        private static void MapStickerRoutes(Router r)
        {
            // Non-custom stickers (DisplayValue != 'Custom Stickers')
            r.Map(RequestMethod.GET, "/stickers/non-custom", rv =>
            {
                var c = ReflectionFactory.Get<StickerController>();
                return c.GetNonCustomStickers();
            });

            // Custom stickers của một user cụ thể
            r.Map(RequestMethod.GET, "/stickers/custom", rv =>
            {
                var c = ReflectionFactory.Get<StickerController>();
                return c.GetCustomStickersByUser(int.Parse(rv["userId"]));
            });
        }

        private static void MapNotificationRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/notifications", rv =>
            {
                var c = ReflectionFactory.Get<NotificationController>();
                return c.GetNotificationByUser(
                    int.Parse(rv["userId"]),
                    bool.Parse(rv["isRead"])
                );
            });
        }

        private static void MapSettingRoutes(Router r)
        {
            r.Map(RequestMethod.GET, "/settings/values", rv =>
            {
                var c = ReflectionFactory.Get<SettingController>();

                var ownerId = int.Parse(rv["ownerId"]);
                var ownerTypeStr = rv["ownerType"];   // WORKSPACE, BOARD, USER...

                return c.GetValuesByOwnerType(ParseOwnerType(ownerTypeStr), ownerId);
            });

            r.Map(RequestMethod.GET, "/settings/options", rv =>
            {
                var c = ReflectionFactory.Get<SettingController>();

                var ownerTypeStr = rv["ownerType"];
                var enumOwnerType = Enum.Parse<OwnerType>(ownerTypeStr);

                return c.GetOptionsByOwnerType(ParseOwnerType(ownerTypeStr));
            });

            OwnerType ParseOwnerType(string ownerTypeStr)
            {
                if (!Enum.TryParse(ownerTypeStr, out OwnerType ownerType))
                    throw new ArgumentException($"Invalid ownerType: '{ownerTypeStr}'. Allowed: WORKSPACE, BOARD, USER.");

                return ownerType;
            }
        }

    }
}
