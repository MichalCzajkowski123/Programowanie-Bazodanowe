using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Wiecej_Poprawek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Products_ProductID",
                table: "OrderPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductGroups_ProductGroupID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_UserGroupID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserGroupID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductGroupID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserGroupID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductGroupID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductID1",
                table: "OrderPositions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupID",
                table: "Users",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GroupID",
                table: "Products",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPositions_ProductID1",
                table: "OrderPositions",
                column: "ProductID1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_Products_ProductID",
                table: "OrderPositions",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_Products_ProductID1",
                table: "OrderPositions",
                column: "ProductID1",
                principalTable: "Products",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductGroups_GroupID",
                table: "Products",
                column: "GroupID",
                principalTable: "ProductGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_GroupID",
                table: "Users",
                column: "GroupID",
                principalTable: "UserGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Products_ProductID",
                table: "OrderPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Products_ProductID1",
                table: "OrderPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductGroups_GroupID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_GroupID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_GroupID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_OrderPositions_ProductID1",
                table: "OrderPositions");

            migrationBuilder.DropColumn(
                name: "ProductID1",
                table: "OrderPositions");

            migrationBuilder.AddColumn<int>(
                name: "UserGroupID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupID",
                table: "Users",
                column: "UserGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupID",
                table: "Products",
                column: "ProductGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_Products_ProductID",
                table: "OrderPositions",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductGroups_ProductGroupID",
                table: "Products",
                column: "ProductGroupID",
                principalTable: "ProductGroups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_UserGroupID",
                table: "Users",
                column: "UserGroupID",
                principalTable: "UserGroups",
                principalColumn: "ID");
        }
    }
}
