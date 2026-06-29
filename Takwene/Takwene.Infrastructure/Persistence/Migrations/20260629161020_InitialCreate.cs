using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Takwene.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dsps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dsps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: false),
                    Isrc = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Genre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackDistributions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    DspId = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackDistributions_Dsps_DspId",
                        column: x => x.DspId,
                        principalTable: "Dsps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackDistributions_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Country", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Saudi Arabia", "layla@example.com", "Layla Hassan" },
                    { 2, "United States", "marcus@example.com", "Marcus Lee" },
                    { 3, "United Kingdom", "nadia@example.com", "Nadia Petrova" }
                });

            migrationBuilder.InsertData(
                table: "Dsps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Spotify" },
                    { 2, "Apple Music" },
                    { 3, "YouTube" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "ArtistId", "Genre", "Isrc", "ReleaseDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Pop", "SAA112500001", new DateOnly(2025, 2, 1), "Distributed", "Desert Dawn" },
                    { 2, 2, "Hip-Hop", "USRC17600002", new DateOnly(2024, 11, 20), "Submitted", "Night Drive" },
                    { 3, 3, "Electronic", "GBA112500003", new DateOnly(2025, 1, 10), "Distributed", "Echoes" },
                    { 4, 1, "R&B", "SAA112500004", new DateOnly(2025, 3, 5), "Draft", "Golden Hour" },
                    { 5, 2, "Rock", "USRC17600005", new DateOnly(2024, 9, 15), "Distributed", "City Lights" },
                    { 6, 1, "Electronic", "SAA112500006", new DateOnly(2025, 2, 18), "Submitted", "Mirage" },
                    { 7, 2, "Pop", "USRC17600007", new DateOnly(2025, 4, 1), "Draft", "Reckless" },
                    { 8, 3, "Indie", "GBA112500008", new DateOnly(2024, 12, 25), "Distributed", "Aurora" },
                    { 9, 1, "Afrobeat", "SAA112500009", new DateOnly(2025, 3, 22), "Submitted", "Sandstorm" }
                });

            migrationBuilder.InsertData(
                table: "TrackDistributions",
                columns: new[] { "Id", "DspId", "Status", "SubmittedAt", "TrackId" },
                values: new object[,]
                {
                    { 1, 1, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, 2, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 3, 3, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, 1, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 5, 3, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 6, 1, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 5 },
                    { 7, 2, "Rejected", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 5 },
                    { 8, 2, "Live", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 8 },
                    { 9, 1, "Pending", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 10, 3, "Pending", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 6 },
                    { 11, 1, "Pending", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 9 },
                    { 12, 2, "Pending", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dsps_Name",
                table: "Dsps",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackDistributions_DspId",
                table: "TrackDistributions",
                column: "DspId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackDistributions_TrackId_DspId",
                table: "TrackDistributions",
                columns: new[] { "TrackId", "DspId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ArtistId",
                table: "Tracks",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Isrc",
                table: "Tracks",
                column: "Isrc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackDistributions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Dsps");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
