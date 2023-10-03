using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Api.Migrations
{
    [Profile("Development")]
    [Migration(20230924)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("email").AsCustom("varchar(256)").Unique().NotNullable()
                .WithColumn("phone").AsCustom("varchar(15)")
                .WithColumn("password_hash").AsCustom("text").NotNullable()
                .WithColumn("password_salt").AsCustom("text").NotNullable();

            Create.Index("ix_user_email").OnTable("users").OnColumn("email").Ascending()
                .WithOptions().NonClustered();

            Create.Table("user_roles")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(20)").Unique().NotNullable();

            Create.Index("ix_user_role_name").OnTable("user_roles").OnColumn("name").Ascending()
                .WithOptions().NonClustered();

            Create.Table("user_infos")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("nickname").AsCustom("varchar(100)").Unique().NotNullable()
                .WithColumn("image_uri").AsCustom("text").Nullable()
                .WithDefaultValue("static/images/default.png")
                .WithColumn("additional").AsCustom("varchar(256)").Nullable()
                .WithColumn("role_id").AsGuid().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("creational_date").AsDateTime().WithDefaultValue(DateTime.Now.ToString())
                .WithColumn("is_removed").AsBoolean().Nullable().WithDefaultValue(false);

            Create.Table("user_links")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("user_info_id").AsGuid().NotNullable()
                .WithColumn("link_id").AsGuid().NotNullable()
                .WithColumn("href").AsCustom("varchar(256)").NotNullable();

            Create.Table("link_types")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(256)").NotNullable()
                .WithColumn("short_link").AsCustom("varchar(265)").NotNullable();

            Create.Table("user_statistics")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("total_words").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("total_hours").AsFloat().NotNullable().WithDefaultValue(0)
                .WithColumn("visit_streak").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("avg_words").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("last_update_at").AsDateTime().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable();


            Create.ForeignKey("FK_Identity_UserInfos_RoleId")
                .FromTable("user_infos").ForeignColumn("role_id")
                .ToTable("user_roles").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.ForeignKey("FK_Identity_UserInfos_UserId")
                .FromTable("user_infos").ForeignColumn("user_id")
                .ToTable("users").InSchema("public").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.ForeignKey("FK_Identity_UserLinks_UserInfoId")
                .FromTable("user_links").ForeignColumn("user_info_id")
                .ToTable("user_infos").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.ForeignKey("FK_Identity_UserLinks_LinkId")
                .FromTable("user_links").ForeignColumn("link_id")
                .ToTable("link_types").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.ForeignKey("FK_Identity_UserStatistics_UserId")
                .FromTable("user_statistics").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        }
        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("user_roles");
            Delete.Table("user_infos");
            Delete.Table("user_links");
            Delete.Table("link_types");
            Delete.Table("user_statistics");
        }
    }
}
