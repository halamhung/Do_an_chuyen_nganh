using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    DIS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIS_PER = table.Column<double>(type: "float", nullable: true),
                    DIS_NAME = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DIS_DATE_START = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DIS_DATE_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.DIS_ID);
                });

            migrationBuilder.CreateTable(
                name: "endLocations",
                columns: table => new
                {
                    END_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    END_LOCATION_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endLocations", x => x.END_LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "startLocations",
                columns: table => new
                {
                    START_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    START_LOCATION_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_startLocations", x => x.START_LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "tourTypes",
                columns: table => new
                {
                    TOUR_TYPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOUR_TYPE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourTypes", x => x.TOUR_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    HomeAdress = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NewCitizenIdentification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OldCitizenIdentification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfInssuance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tours",
                columns: table => new
                {
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOUR_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE = table.Column<int>(type: "int", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REMAINING = table.Column<int>(type: "int", nullable: true),
                    PRICE_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PRICE_NOT_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SURCHARGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CANCLE_CHANGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DIS_ID = table.Column<long>(type: "bigint", nullable: true),
                    START_LOCATION_ID = table.Column<long>(type: "bigint", nullable: true),
                    END_LOCATION_ID = table.Column<long>(type: "bigint", nullable: true),
                    TOUR_TYPE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tours", x => x.TOUR_ID);
                    table.ForeignKey(
                        name: "FK_tours_discounts_DIS_ID",
                        column: x => x.DIS_ID,
                        principalTable: "discounts",
                        principalColumn: "DIS_ID");
                    table.ForeignKey(
                        name: "FK_tours_endLocations_END_LOCATION_ID",
                        column: x => x.END_LOCATION_ID,
                        principalTable: "endLocations",
                        principalColumn: "END_LOCATION_ID");
                    table.ForeignKey(
                        name: "FK_tours_startLocations_START_LOCATION_ID",
                        column: x => x.START_LOCATION_ID,
                        principalTable: "startLocations",
                        principalColumn: "START_LOCATION_ID");
                    table.ForeignKey(
                        name: "FK_tours_tourTypes_TOUR_TYPE_ID",
                        column: x => x.TOUR_TYPE_ID,
                        principalTable: "tourTypes",
                        principalColumn: "TOUR_TYPE_ID");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NUM_ADULTS = table.Column<int>(type: "int", nullable: true),
                    NUM_TODDLERS = table.Column<int>(type: "int", nullable: true),
                    NUM_KIDS = table.Column<int>(type: "int", nullable: true),
                    PRICE_ADULTS = table.Column<double>(type: "float", nullable: true),
                    PRICE_TODDLERS = table.Column<double>(type: "float", nullable: true),
                    PRICE_KIDS = table.Column<double>(type: "float", nullable: true),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.BOOKING_ID);
                    table.ForeignKey(
                        name: "FK_bookings_Users_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateTable(
                name: "tourImages",
                columns: table => new
                {
                    IMAGE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMAGE_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DAY_NUMBER = table.Column<int>(type: "int", nullable: true),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourImages", x => x.IMAGE_ID);
                    table.ForeignKey(
                        name: "FK_tourImages_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => new { x.PostID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_PostCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategory_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PAYMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AMOUNT = table.Column<int>(type: "int", nullable: true),
                    TOTAL_PRICE = table.Column<double>(type: "float", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PAYMENT_ID);
                    table.ForeignKey(
                        name: "FK_payments_bookings_BOOKING_ID",
                        column: x => x.BOOKING_ID,
                        principalTable: "bookings",
                        principalColumn: "BOOKING_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_TOUR_ID",
                table: "bookings",
                column: "TOUR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_USER_ID",
                table: "bookings",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_endLocations_END_LOCATION_NAME",
                table: "endLocations",
                column: "END_LOCATION_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_BOOKING_ID",
                table: "payments",
                column: "BOOKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorId",
                table: "Post",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Slug",
                table: "Post",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_CategoryID",
                table: "PostCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_startLocations_START_LOCATION_NAME",
                table: "startLocations",
                column: "START_LOCATION_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tourImages_TOUR_ID",
                table: "tourImages",
                column: "TOUR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_DIS_ID",
                table: "tours",
                column: "DIS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_END_LOCATION_ID",
                table: "tours",
                column: "END_LOCATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_START_LOCATION_ID",
                table: "tours",
                column: "START_LOCATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_TOUR_TYPE_ID",
                table: "tours",
                column: "TOUR_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tourTypes_TOUR_TYPE_NAME",
                table: "tourTypes",
                column: "TOUR_TYPE_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NewCitizenIdentification",
                table: "Users",
                column: "NewCitizenIdentification",
                unique: true,
                filter: "[NewCitizenIdentification] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OldCitizenIdentification",
                table: "Users",
                column: "OldCitizenIdentification",
                unique: true,
                filter: "[OldCitizenIdentification] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "tourImages");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "tours");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "endLocations");

            migrationBuilder.DropTable(
                name: "startLocations");

            migrationBuilder.DropTable(
                name: "tourTypes");
        }
    }
}
