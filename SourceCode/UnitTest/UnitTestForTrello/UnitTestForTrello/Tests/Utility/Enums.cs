namespace UnitTestForTrello.Tests.Utility
{
    public enum BoardStatus
    {
        ACTIVE
    }

    public enum StaredBoardsStatus
    {
        ACTIVE = 1,
        INACTIVE = 0
    }

    public enum OwnerType
    {
        WORKSPACE,
        BOARD,
        CARD,
        USER
    }
    public enum DataType
    {
        DROPDOWN
    }
    public enum RequestMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}