using FluentMigrator;

namespace Topics.Api.Migrations
{
    [Profile("Development")]
    [Migration(20231007)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("email").AsCustom("varchar(256)").NotNullable()
                .WithColumn("phone").AsCustom("varchar(15)").NotNullable();

            Create.Table("languages")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(15)").NotNullable();

            Create.Table("topic_statistics_types")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(50)").NotNullable();

            Create.Table("topic_levels")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(20)").NotNullable();

            Create.Table("topics")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("title").AsCustom("varchar(50)").NotNullable()
                .WithColumn("content").AsCustom("text").NotNullable()
                .WithColumn("icon").AsCustom("text").Nullable()
                .WithColumn("creational_date").AsDateTime().NotNullable()
                .WithColumn("language_id").AsGuid().NotNullable()
                .WithColumn("topic_level_id").AsGuid().NotNullable();

            Create.Table("topic_statistics")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("topic_id").AsGuid().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("topic_statistics_type_id").AsGuid().NotNullable()
                .WithColumn("statistics_date").AsDateTime().Nullable();

            Create.ForeignKey("FK_Topics_Topics_TopicLevelId")
                .FromTable("topics").ForeignColumn("topic_level_id")
                .ToTable("topic_levels").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Topics_Topics_LanguageId")
                .FromTable("topics").ForeignColumn("language_id")
                .ToTable("languages").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Topics_TopicStatistics_TopicId")
                .FromTable("topic_statistics").ForeignColumn("topic_id")
                .ToTable("topics").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Topics_TopicStatistics_UserId")
                .FromTable("topic_statistics").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Topics_TopicStatistics_StatisticsId")
                .FromTable("topic_statistics").ForeignColumn("topic_statistics_type_id")
                .ToTable("topic_statistics_types").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("languages");
            Delete.Table("topic_levels");
            Delete.Table("topics");
            Delete.Table("topic_statistics");
        }
    }
}
