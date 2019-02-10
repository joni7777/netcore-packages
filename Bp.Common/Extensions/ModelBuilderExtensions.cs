using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bp.Common.Extensions
{
	public static class ModelBuilderExtensions
	{
		public static void ConvertTablesToSingularAndEverythingToSnakeCase(this ModelBuilder @this)
		{
			foreach (var entityType in @this.Model.GetEntityTypes())
			{
				entityType.Relational().TableName = entityType.DisplayName().ToSnakeCase();

				foreach (var property in entityType.GetProperties())
				{
					property.Relational().ColumnName = property.Name.ToSnakeCase();
				}
				
				foreach (var key in entityType.GetKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}
				
				foreach (var key in entityType.GetForeignKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}
				
				foreach (var index in entityType.GetIndexes())
				{
					index.Relational().Name = index.Relational().Name.ToSnakeCase();
				}
			}
		}
	}
}