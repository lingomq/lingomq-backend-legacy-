using FluentMigrator;
using System.Data;

namespace Finances.Api.Migrations;

[Profile("Development")]
[Migration(20231210)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("email").AsCustom("varchar(256)").NotNullable()
            .WithColumn("phone").AsCustom("varchar(15)").NotNullable();

        Create.Table("finances")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("name").AsCustom("varchar(50)").NotNullable()
            .WithColumn("description").AsCustom("varchar(256)").NotNullable()
            .WithColumn("amount").AsDecimal().NotNullable();

        Create.Table("user_finances")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("finance_id").AsGuid().NotNullable()
            .WithColumn("creation_date").AsDateTime().Nullable()
            .WithColumn("end_subsscription_date").AsDateTime().Nullable();

        Create.ForeignKey("FK_Finances_UserFinances_FinanceId")
            .FromTable("user_finances").ForeignColumn("finance_id")
            .ToTable("finances").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        Create.ForeignKey("FK_Finances_UserFinances_UserId")
            .FromTable("user_finances").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("finances");
        Delete.Table("user_finances");
    }
}