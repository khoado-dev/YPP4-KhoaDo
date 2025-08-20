using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories
{
    public class TemplateRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;

        public TemplateRepository()
        {
            _con = TestStartup.Conn!;
            _cache = TestStartup.Cache;
        }

        public IEnumerable<TemplateCategoryDTO>? GetAllCategories()
        {
            const string sql = @"
            SELECT 
              tpc.Id TemplateCategoryId, 
              tpc.IconUrl, 
              tpc.DisplayValue 
            FROM 
              TemplateCategory tpc;
            ";
            return _con.Query<TemplateCategoryDTO>(sql);
            
        }

        public IEnumerable<TemplateByCategoryDTO>? GetTemplatesByCategory(int categoryId)
        {
            const string sql = @"
            SELECT 
              tpl.Id TemplateId, 
              tpl.Title TemplateTitle, 
              tpl.TemplateDescription, 
              tpl.Viewed, 
              tpl.Copied, 
              tpl.UpdatedAt, 
              tpc.Id TemplateCategoryId, 
              tpc.DisplayValue TemplateCategory 
            FROM 
              Template tpl 
              JOIN TemplateCategory tpc ON tpc.Id = tpl.CategoryId 
            WHERE 
              tpc.Id = @TemplateCategoryId 
            ORDER BY 
              tpl.UpdatedAt DESC;
            ";
            return _con.Query<TemplateByCategoryDTO>(sql, new
            {
                TemplateCategoryId = categoryId
            });
        }

        public TemplateDetailDTO? GetTemplateDetail(int templateId)
        {
            const string sql = @"
            SELECT 
              tpl.Id TemplateId, 
              us.PictureUrl AS user_picture, 
              tpl.TemplateDescription AS template_description, 
              tpl.Title AS template_title, 
              us.Username, 
              tpl.Copied AS copied_number, 
              tpl.Viewed AS viewed_number, 
              tpl.BoardId 
            FROM 
              Template tpl 
              JOIN [Users] us ON us.Id = tpl.CreatedBy 
            WHERE 
              tpl.Id = @TemplateId;
            ";
            return _con.QueryFirstOrDefault<TemplateDetailDTO>(sql, new
            {
                TemplateId = templateId
            });
        }
    }
}