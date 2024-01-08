using FluentMigrator;

namespace Identity.DataAccess.Providers.Dapper.Migrations;

[Profile("Development")]
[Migration(20230924)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("nickname").AsCustom("varchar(256)").Unique().NotNullable()
            .WithColumn("image_uri").AsCustom("text")
            .WithColumn("role_id").AsGuid().NotNullable();

        Create.Index("ix_user_id").OnTable("users").OnColumn("id").Ascending()
            .WithOptions().NonClustered();

        Create.Table("roles")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("name").AsCustom("varchar(20)").Unique().NotNullable();

        Create.Index("ix_user_role_id").OnTable("user_roles").OnColumn("id").Ascending()
            .WithOptions().NonClustered();

        Create.Table("user_credentials")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("email").AsCustom("text").Unique().NotNullable()
            .WithColumn("phone").AsCustom("varchar(16)").Nullable()
            .WithColumn("user_id").AsGuid().NotNullable();

        Create.Table("user_sensitive_datas")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("password_hash").AsCustom("text").NotNullable()
            .WithColumn("password_salt").AsCustom("text").NotNullable()
            .WithColumn("user_id").AsGuid().NotNullable();

        Create.Table("user_infos")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("creation_time").AsDateTime().WithDefaultValue(DateTime.Now.ToString())
            .WithColumn("description").AsCustom("varchar(256)").Nullable()
            .WithColumn("locale").AsCustom("varchar(50)").WithDefaultValue("Earth")
            .WithColumn("user_ud").AsGuid().NotNullable();


        Create.ForeignKey("fk_identity_users_roles_role_id")
            .FromTable("users").ForeignColumn("role_id")
            .ToTable("roles").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        Create.ForeignKey("fk_identity_user_credentials_users_user_id")
            .FromTable("user_credentials").ForeignColumn("user_id")
            .ToTable("users").InSchema("public").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        Create.ForeignKey("fk_identity_user_sensitive_datas_users_user_id")
            .FromTable("user_sensitive_datas").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("roles");
        Delete.Table("user_infos");
        Delete.Table("user_credentials");
        Delete.Table("user_sensitive_datas");
    }
}