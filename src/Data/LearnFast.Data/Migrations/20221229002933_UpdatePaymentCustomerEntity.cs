using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class UpdatePaymentCustomerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerIdentifier",
                table: "PaymentCustomers",
                newName: "RoutingNumber");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "PaymentCustomers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "PaymentCustomers");

            migrationBuilder.RenameColumn(
                name: "RoutingNumber",
                table: "PaymentCustomers",
                newName: "CustomerIdentifier");
        }
    }
}
