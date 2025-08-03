using UnitTestForTrello.Models;
using UnitTestForTrello.Models.Interface;

namespace UnitTestForTrello.Services
{
    public class WorkspaceService
    {
        private IDbTemplate @object;

        public WorkspaceService(IDbTemplate @object)
        {
            this.@object = @object;
        }

        public int DeleteWorkspace(int id)
        {
            // SQL: DELETE FROM Workspaces WHERE Id = @id
            return @object.Update("DELETE FROM Workspaces WHERE Id = @p0", id);
        }

        public List<Workspace> GetAllWorkspaces()
        {
            // SQL: SELECT * FROM Workspaces
            return @object.Query(
                "SELECT Id, Name, Description, Type FROM Workspaces",
                reader => new Workspace
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Type = (WorkspaceTypeEnum)reader.GetInt32(reader.GetOrdinal("Type"))
                }
            );
        }

        public Workspace GetWorkspaceByName(string name)
        {
            // SQL: SELECT * FROM Workspaces WHERE Name = @name
            return @object.QueryForObject<Workspace>(
                "SELECT Id, Name, Description, Type FROM Workspaces WHERE Name = @p0",
                reader => new Workspace
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Type = (WorkspaceTypeEnum)reader.GetInt32(reader.GetOrdinal("Type"))
                },
                name
            );
        }

        public int UpdateWorkspace(int id, string name, string description, WorkspaceTypeEnum type)
        {
            // SQL: UPDATE Workspaces SET Name = @name, Description = @description, Type = @type WHERE Id = @id
            return @object.Update(
                "UPDATE Workspaces SET Name = @p0, Description = @p1, Type = @p2 WHERE Id = @p3",
                name, description, (int)type, id
            );
        }
    }
}