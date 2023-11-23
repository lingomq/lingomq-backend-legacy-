using FluentMigrator;

namespace Words.Api.Migrations
{
    [Profile("Development")]
    [Migration(20231004)]
    public class InitialMigration : Migration
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

            Create.Table("word_types")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("type_name").AsCustom("varchar(15)").NotNullable();

            Create.Table("user_words")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("word").AsCustom("varchar(256)").NotNullable()
                .WithColumn("translated").AsCustom("varchar(256)").Nullable()
                .WithColumn("language_id").AsGuid().NotNullable()
                .WithColumn("repeats").AsInt32().WithDefaultValue(0)
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("created_at").AsDateTime();

            Create.Table("user_word_types")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("user_word_id").AsGuid().NotNullable()
                .WithColumn("word_type_id").AsGuid().NotNullable();

            Create.Table("languages")
                .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
                .WithColumn("name").AsCustom("varchar(15)").NotNullable();

            Create.ForeignKey("FK_Words_UserWords_LanguageId")
                .FromTable("user_words").ForeignColumn("language_id")
                .ToTable("languages").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Words_UserWords_UserId ")
                .FromTable("user_words").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.ForeignKey("FK_Words_UserWordTypes_UserWordTypeId")
                .FromTable("user_word_types").ForeignColumn("word_type_id")
                .ToTable("word_types").PrimaryColumn("id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        }
        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("user_words");
            Delete.Table("word_types");
            Delete.Table("user_word_types");
            Delete.Table("languages");
        }
    }
}
