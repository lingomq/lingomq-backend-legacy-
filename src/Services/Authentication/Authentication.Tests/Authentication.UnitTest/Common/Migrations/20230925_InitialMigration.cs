using FluentMigrator;

namespace Authentication.UnitTest.Common.Migrations
{
    [Profile("Test")]
    [Migration(20230925)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable().Identity()
                .WithColumn("email").AsCustom("varchar(256)").Unique().NotNullable()
                .WithColumn("phone").AsCustom("varchar(15)").Unique()
                .WithColumn("password_hash").AsCustom("text").NotNullable()
                .WithColumn("password_salt").AsCustom("text").NotNullable();

            Create.Index("ix_user_email").OnTable("users").OnColumn("email").Ascending()
                .WithOptions().NonClustered();

            Create.Table("user_roles")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable().Identity()
                .WithColumn("name").AsCustom("varchar(20)").Unique().NotNullable();

            Create.Index("ix_user_role_name").OnTable("user_roles").OnColumn("name").Ascending()
                .WithOptions().NonClustered();

            Create.Table("user_infos")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable().Identity()
                .WithColumn("nickname").AsCustom("varchar(100)").Unique().NotNullable()
                .WithColumn("image_uri").AsCustom("text")
                .WithDefaultValue("static/images/default.png")
                .WithColumn("additional").AsCustom("varchar(256)")
                .WithColumn("role_id").AsGuid().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("creational_date").AsDateTime().WithDefaultValue(DateTime.Now.ToString())
                .WithColumn("is_removed").AsBoolean().WithDefaultValue(false);

            Create.ForeignKey("FK_Identity_UserInfos_RoleId")
                .FromTable("user_infos").ForeignColumn("role_id")
                .ToTable("user_roles").PrimaryColumn("id");
            Create.ForeignKey("FK_Identity_UserInfos_UserId")
                .FromTable("user_infos").ForeignColumn("user_id")
                .ToTable("users").InSchema("public").PrimaryColumn("id");
        }
        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("user_roles");
            Delete.Table("user_infos");
        }
    }
}
