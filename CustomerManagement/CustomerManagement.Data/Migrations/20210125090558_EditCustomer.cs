using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerManagement.Data.Migrations
{
    public partial class EditCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Email",
                "Customers",
                "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                "IX_Customers_Email",
                "Customers",
                "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Customers_PhoneNumber",
                "Customers",
                "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_Customers_Email",
                "Customers");

            migrationBuilder.DropIndex(
                "IX_Customers_PhoneNumber",
                "Customers");

            migrationBuilder.AlterColumn<string>(
                "Email",
                "Customers",
                "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}