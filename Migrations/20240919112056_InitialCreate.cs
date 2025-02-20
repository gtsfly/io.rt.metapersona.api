using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace otel_advisor_webApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "def_experience",
                columns: table => new
                {
                    experience_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_def_experience", x => x.experience_id);
                });

            migrationBuilder.CreateTable(
                name: "def_user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_def_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "ref_location",
                columns: table => new
                {
                    location_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ref_location", x => x.location_id);
                });

            migrationBuilder.CreateTable(
                name: "rel_user_experience",
                columns: table => new
                {
                    user_preference_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    experience_id = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_user_experience", x => x.user_preference_id);
                    table.ForeignKey(
                        name: "FK_rel_user_experience_def_experience_experience_id",
                        column: x => x.experience_id,
                        principalTable: "def_experience",
                        principalColumn: "experience_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rel_user_experience_def_user_user_id",
                        column: x => x.user_id,
                        principalTable: "def_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "def_hotel",
                columns: table => new
                {
                    hotel_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_def_hotel", x => x.hotel_id);
                    table.ForeignKey(
                        name: "FK_def_hotel_ref_location_location_id",
                        column: x => x.location_id,
                        principalTable: "ref_location",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inf_reservation_offer",
                columns: table => new
                {
                    offer_key = table.Column<string>(type: "text", nullable: false),
                    check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    room_type = table.Column<string>(type: "text", nullable: false),
                    hotel_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_reservation_offer", x => x.offer_key);
                    table.ForeignKey(
                        name: "FK_inf_reservation_offer_def_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "def_hotel",
                        principalColumn: "hotel_id");
                });

            migrationBuilder.CreateTable(
                name: "inf_reservation_request",
                columns: table => new
                {
                    reservation_request_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    check_in_range_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_in_range_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    budget = table.Column<decimal>(type: "numeric", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    stay_duration = table.Column<int>(type: "integer", nullable: false),
                    exp_1 = table.Column<string>(type: "text", nullable: false),
                    exp_1_rating = table.Column<int>(type: "integer", nullable: false),
                    exp_2 = table.Column<string>(type: "text", nullable: false),
                    exp_2_rating = table.Column<int>(type: "integer", nullable: false),
                    exp_3 = table.Column<string>(type: "text", nullable: false),
                    exp_3_rating = table.Column<int>(type: "integer", nullable: false),
                    adult_num = table.Column<int>(type: "integer", nullable: false),
                    child_num = table.Column<int>(type: "integer", nullable: false),
                    children_ages = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hotel_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_reservation_request", x => x.reservation_request_id);
                    table.ForeignKey(
                        name: "FK_inf_reservation_request_def_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "def_hotel",
                        principalColumn: "hotel_id");
                    table.ForeignKey(
                        name: "FK_inf_reservation_request_def_user_user_id",
                        column: x => x.user_id,
                        principalTable: "def_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_hotel_experience",
                columns: table => new
                {
                    hotel_id = table.Column<int>(type: "integer", nullable: false),
                    experience_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_hotel_experience", x => new { x.hotel_id, x.experience_id });
                    table.ForeignKey(
                        name: "FK_rel_hotel_experience_def_experience_experience_id",
                        column: x => x.experience_id,
                        principalTable: "def_experience",
                        principalColumn: "experience_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rel_hotel_experience_def_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "def_hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inf_reservation_confirmed",
                columns: table => new
                {
                    confirmed_reservation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reservation_request_id = table.Column<int>(type: "integer", nullable: false),
                    hotel_id = table.Column<int>(type: "integer", nullable: false),
                    check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    room_type = table.Column<string>(type: "text", nullable: false),
                    confirm_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_reservation_confirmed", x => x.confirmed_reservation_id);
                    table.ForeignKey(
                        name: "FK_inf_reservation_confirmed_def_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "def_hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inf_reservation_confirmed_def_user_user_id",
                        column: x => x.user_id,
                        principalTable: "def_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inf_reservation_confirmed_inf_reservation_request_reservati~",
                        column: x => x.reservation_request_id,
                        principalTable: "inf_reservation_request",
                        principalColumn: "reservation_request_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inf_user_hotel_experience",
                columns: table => new
                {
                    user_hotel_experience_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    hotel_id = table.Column<int>(type: "integer", nullable: false),
                    reservation_request_id = table.Column<int>(type: "integer", nullable: false),
                    overall_rating = table.Column<int>(type: "integer", nullable: false),
                    experience_1 = table.Column<string>(type: "text", nullable: false),
                    experience_1_rating = table.Column<int>(type: "integer", nullable: false),
                    experience_2 = table.Column<string>(type: "text", nullable: false),
                    experience_2_rating = table.Column<int>(type: "integer", nullable: false),
                    experience_3 = table.Column<string>(type: "text", nullable: false),
                    experience_3_rating = table.Column<int>(type: "integer", nullable: false),
                    most_liked_experience = table.Column<string>(type: "text", nullable: false),
                    least_liked_experience = table.Column<string>(type: "text", nullable: false),
                    would_visit_again = table.Column<bool>(type: "boolean", nullable: false),
                    additional_comments = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_user_hotel_experience", x => x.user_hotel_experience_id);
                    table.ForeignKey(
                        name: "FK_inf_user_hotel_experience_def_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "def_hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inf_user_hotel_experience_def_user_user_id",
                        column: x => x.user_id,
                        principalTable: "def_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inf_user_hotel_experience_inf_reservation_confirmed_reserva~",
                        column: x => x.reservation_request_id,
                        principalTable: "inf_reservation_confirmed",
                        principalColumn: "confirmed_reservation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_def_hotel_location_id",
                table: "def_hotel",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_confirmed_hotel_id",
                table: "inf_reservation_confirmed",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_confirmed_reservation_request_id",
                table: "inf_reservation_confirmed",
                column: "reservation_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_confirmed_user_id",
                table: "inf_reservation_confirmed",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_offer_hotel_id",
                table: "inf_reservation_offer",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_request_hotel_id",
                table: "inf_reservation_request",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_reservation_request_user_id",
                table: "inf_reservation_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_user_hotel_experience_hotel_id",
                table: "inf_user_hotel_experience",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_user_hotel_experience_reservation_request_id",
                table: "inf_user_hotel_experience",
                column: "reservation_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_inf_user_hotel_experience_user_id",
                table: "inf_user_hotel_experience",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_rel_hotel_experience_experience_id",
                table: "rel_hotel_experience",
                column: "experience_id");

            migrationBuilder.CreateIndex(
                name: "IX_rel_user_experience_experience_id",
                table: "rel_user_experience",
                column: "experience_id");

            migrationBuilder.CreateIndex(
                name: "IX_rel_user_experience_user_id",
                table: "rel_user_experience",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inf_reservation_offer");

            migrationBuilder.DropTable(
                name: "inf_user_hotel_experience");

            migrationBuilder.DropTable(
                name: "rel_hotel_experience");

            migrationBuilder.DropTable(
                name: "rel_user_experience");

            migrationBuilder.DropTable(
                name: "inf_reservation_confirmed");

            migrationBuilder.DropTable(
                name: "def_experience");

            migrationBuilder.DropTable(
                name: "inf_reservation_request");

            migrationBuilder.DropTable(
                name: "def_hotel");

            migrationBuilder.DropTable(
                name: "def_user");

            migrationBuilder.DropTable(
                name: "ref_location");
        }
    }
}
