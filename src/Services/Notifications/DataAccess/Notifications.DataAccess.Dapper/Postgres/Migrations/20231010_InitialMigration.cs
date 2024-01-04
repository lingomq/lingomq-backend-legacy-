using System.Data;
using FluentMigrator;

namespace Notifications.DataAccess.Dapper.Postgres.Migrations;
[Profile("Development")]
[Migration( 20231010)]
public class InitialMigration : Migration {
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("email").AsCustom("varchar(256)").NotNullable()
            .WithColumn("phone").AsCustom("varchar(15)").NotNullable();

        Create.Table("notification_types")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("name").AsCustom("varchar(15)").NotNullable();

        Create.Table("notifications")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("title").AsCustom("varchar(50)").NotNullable()
            .WithColumn("content").AsCustom("varchar(255)").Nullable()
            .WithColumn("notification_type_id").AsGuid().NotNullable();

        Create.Table("user_notifications")
            .WithColumn("id").AsGuid().PrimaryKey().Unique().NotNullable()
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("notification_id").AsGuid().NotNullable()
            .WithColumn("date_of_receipt").AsDateTime().NotNullable()
            .WithColumn("is_readed").AsBoolean().Nullable();
        
        Create.ForeignKey("FK_Notifications_Notifications_NotificationsId")
            .FromTable("user_notifications").ForeignColumn("notification_id")
            .ToTable("notifications").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);
        
        Create.ForeignKey("FK_Notifications_UserNotifications_UserId")
            .FromTable("user_notifications").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);
        
        Create.ForeignKey("FK_Notifications_NotificationTypes_NotificationTypeId ")
            .FromTable("notifications").ForeignColumn("notification_type_id")
            .ToTable("notification_types").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("notification_types");
        Delete.Table("notifications");
        Delete.Table("user_notifications");
    }
}