using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;
using Nop.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class CourseBuilder : NopEntityBuilder<Course>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Course.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Course.ProductId))
                    .AsInt32().NotNullable().ForeignKey<Product>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Course.Name))
                    .AsString(500).NotNullable()
                .WithColumn(nameof(Course.LimitedToStores))
                    .AsBoolean().NotNullable()
                .WithColumn(nameof(Course.Deleted))
                    .AsBoolean().NotNullable()
                .WithColumn(nameof(Course.InstructorId))
                    .AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Course.InstructorGuid))
                    .AsGuid().NotNullable()
                 .WithColumn(nameof(Course.CreatedOnUtc))
                        .AsDateTime().NotNullable()
                 .WithColumn(nameof(Course.UpdatedOnUtc))
                        .AsDateTime().NotNullable();
        }
    }
}