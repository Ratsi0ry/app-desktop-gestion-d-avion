using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gestion_avion.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compagnie",
                columns: table => new
                {
                    id_compagnie = table.Column<string>(type: "text", nullable: false),
                    nom_compagnie = table.Column<string>(type: "text", nullable: false),
                    tel_compagnie = table.Column<string>(type: "text", nullable: false),
                    email_compagnie = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compagnie", x => x.id_compagnie);
                });

            migrationBuilder.CreateTable(
                name: "Date_vol",
                columns: table => new
                {
                    date_depart = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_vol", x => x.date_depart);
                });

            migrationBuilder.CreateTable(
                name: "Passager",
                columns: table => new
                {
                    passeport = table.Column<string>(type: "text", nullable: false),
                    nom_passager = table.Column<string>(type: "text", nullable: false),
                    prenom_passager = table.Column<string>(type: "text", nullable: false),
                    tel_passager = table.Column<string>(type: "text", nullable: false),
                    categorie_passager = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passager", x => x.passeport);
                });

            migrationBuilder.CreateTable(
                name: "Pilote",
                columns: table => new
                {
                    id_pilote = table.Column<string>(type: "text", nullable: false),
                    nom_pilote = table.Column<string>(type: "text", nullable: false),
                    prenom_pilote = table.Column<string>(type: "text", nullable: false),
                    tel_pilote = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilote", x => x.id_pilote);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    numero_place = table.Column<string>(type: "text", nullable: false),
                    classe_siege = table.Column<string>(type: "text", nullable: false),
                    occupee = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.numero_place);
                });

            migrationBuilder.CreateTable(
                name: "Statut_avion",
                columns: table => new
                {
                    code_statut = table.Column<string>(type: "text", nullable: false),
                    libelle_statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statut_avion", x => x.code_statut);
                });

            migrationBuilder.CreateTable(
                name: "Trajet",
                columns: table => new
                {
                    id_trajet = table.Column<string>(type: "text", nullable: false),
                    lieu_depart = table.Column<string>(type: "text", nullable: false),
                    destination = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trajet", x => x.id_trajet);
                });

            migrationBuilder.CreateTable(
                name: "Avion",
                columns: table => new
                {
                    id_avion = table.Column<string>(type: "text", nullable: false),
                    nom_avion = table.Column<string>(type: "text", nullable: false),
                    fk_id_compagnie = table.Column<string>(type: "text", nullable: false),
                    Placenumero_place = table.Column<string>(type: "text", nullable: true),
                    Statut_avioncode_statut = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avion", x => x.id_avion);
                    table.ForeignKey(
                        name: "FK_Avion_Place_Placenumero_place",
                        column: x => x.Placenumero_place,
                        principalTable: "Place",
                        principalColumn: "numero_place");
                    table.ForeignKey(
                        name: "FK_Avion_Statut_avion_Statut_avioncode_statut",
                        column: x => x.Statut_avioncode_statut,
                        principalTable: "Statut_avion",
                        principalColumn: "code_statut");
                    table.ForeignKey(
                        name: "fk_avion_compagnie",
                        column: x => x.fk_id_compagnie,
                        principalTable: "Compagnie",
                        principalColumn: "id_compagnie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repartir",
                columns: table => new
                {
                    fk_id_compagnie = table.Column<string>(type: "text", nullable: false),
                    fk_id_trajet = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repartir", x => new { x.fk_id_compagnie, x.fk_id_trajet });
                    table.ForeignKey(
                        name: "fk_repartir_compagnie",
                        column: x => x.fk_id_compagnie,
                        principalTable: "Compagnie",
                        principalColumn: "id_compagnie",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_repartir_trajet",
                        column: x => x.fk_id_trajet,
                        principalTable: "Trajet",
                        principalColumn: "id_trajet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caracteriser",
                columns: table => new
                {
                    fk_code_statut = table.Column<string>(type: "text", nullable: false),
                    fk_id_avion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caracteriser", x => new { x.fk_code_statut, x.fk_id_avion });
                    table.ForeignKey(
                        name: "fk_caracteriser_avion",
                        column: x => x.fk_id_avion,
                        principalTable: "Avion",
                        principalColumn: "id_avion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_caracteriser_statut_avion",
                        column: x => x.fk_code_statut,
                        principalTable: "Statut_avion",
                        principalColumn: "code_statut",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posseder",
                columns: table => new
                {
                    fk_numero_place = table.Column<string>(type: "text", nullable: false),
                    fk_id_avion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posseder", x => new { x.fk_id_avion, x.fk_numero_place });
                    table.ForeignKey(
                        name: "fk_posseder_avion",
                        column: x => x.fk_id_avion,
                        principalTable: "Avion",
                        principalColumn: "id_avion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posseder_place",
                        column: x => x.fk_numero_place,
                        principalTable: "Place",
                        principalColumn: "numero_place",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vol",
                columns: table => new
                {
                    id_vol = table.Column<string>(type: "text", nullable: false),
                    status_vol = table.Column<string>(type: "text", nullable: false),
                    fk_date_depart = table.Column<string>(type: "text", nullable: false),
                    fk_id_trajet = table.Column<string>(type: "text", nullable: false),
                    fk_id_avion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vol", x => x.id_vol);
                    table.ForeignKey(
                        name: "fk_vol_avion",
                        column: x => x.fk_id_avion,
                        principalTable: "Avion",
                        principalColumn: "id_avion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vol_date_vol",
                        column: x => x.fk_date_depart,
                        principalTable: "Date_vol",
                        principalColumn: "date_depart",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vol_trajet",
                        column: x => x.fk_id_trajet,
                        principalTable: "Trajet",
                        principalColumn: "id_trajet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Affecter",
                columns: table => new
                {
                    fk_id_vol = table.Column<string>(type: "text", nullable: false),
                    fk_id_pilote = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affecter", x => new { x.fk_id_pilote, x.fk_id_vol });
                    table.ForeignKey(
                        name: "fk_affecter_pilote",
                        column: x => x.fk_id_pilote,
                        principalTable: "Pilote",
                        principalColumn: "id_pilote",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_affecter_vol",
                        column: x => x.fk_id_vol,
                        principalTable: "Vol",
                        principalColumn: "id_vol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Billet",
                columns: table => new
                {
                    numero_billet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_passeport = table.Column<string>(type: "text", nullable: false),
                    fk_id_vol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billet", x => x.numero_billet);
                    table.ForeignKey(
                        name: "fk_billet_passager",
                        column: x => x.fk_passeport,
                        principalTable: "Passager",
                        principalColumn: "passeport",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_billet_vol",
                        column: x => x.fk_id_vol,
                        principalTable: "Vol",
                        principalColumn: "id_vol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    id_reservation = table.Column<string>(type: "text", nullable: false),
                    date_reservation = table.Column<string>(type: "text", nullable: false),
                    valide = table.Column<int>(type: "integer", nullable: false),
                    fk_numero_place = table.Column<string>(type: "text", nullable: false),
                    fk_passeport = table.Column<string>(type: "text", nullable: false),
                    fk_id_vol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.id_reservation);
                    table.ForeignKey(
                        name: "fk_reservation_passager",
                        column: x => x.fk_passeport,
                        principalTable: "Passager",
                        principalColumn: "passeport",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reservation_place",
                        column: x => x.fk_numero_place,
                        principalTable: "Place",
                        principalColumn: "numero_place",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reservation_vol",
                        column: x => x.fk_id_vol,
                        principalTable: "Vol",
                        principalColumn: "id_vol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affecter_fk_id_vol",
                table: "Affecter",
                column: "fk_id_vol");

            migrationBuilder.CreateIndex(
                name: "IX_Avion_fk_id_compagnie",
                table: "Avion",
                column: "fk_id_compagnie");

            migrationBuilder.CreateIndex(
                name: "IX_Avion_Placenumero_place",
                table: "Avion",
                column: "Placenumero_place");

            migrationBuilder.CreateIndex(
                name: "IX_Avion_Statut_avioncode_statut",
                table: "Avion",
                column: "Statut_avioncode_statut");

            migrationBuilder.CreateIndex(
                name: "IX_Billet_fk_id_vol",
                table: "Billet",
                column: "fk_id_vol");

            migrationBuilder.CreateIndex(
                name: "IX_Billet_fk_passeport",
                table: "Billet",
                column: "fk_passeport");

            migrationBuilder.CreateIndex(
                name: "IX_Caracteriser_fk_id_avion",
                table: "Caracteriser",
                column: "fk_id_avion");

            migrationBuilder.CreateIndex(
                name: "IX_Posseder_fk_numero_place",
                table: "Posseder",
                column: "fk_numero_place");

            migrationBuilder.CreateIndex(
                name: "IX_Repartir_fk_id_trajet",
                table: "Repartir",
                column: "fk_id_trajet");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_fk_id_vol",
                table: "Reservation",
                column: "fk_id_vol");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_fk_numero_place",
                table: "Reservation",
                column: "fk_numero_place");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_fk_passeport",
                table: "Reservation",
                column: "fk_passeport");

            migrationBuilder.CreateIndex(
                name: "IX_Vol_fk_date_depart",
                table: "Vol",
                column: "fk_date_depart");

            migrationBuilder.CreateIndex(
                name: "IX_Vol_fk_id_avion",
                table: "Vol",
                column: "fk_id_avion");

            migrationBuilder.CreateIndex(
                name: "IX_Vol_fk_id_trajet",
                table: "Vol",
                column: "fk_id_trajet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affecter");

            migrationBuilder.DropTable(
                name: "Billet");

            migrationBuilder.DropTable(
                name: "Caracteriser");

            migrationBuilder.DropTable(
                name: "Posseder");

            migrationBuilder.DropTable(
                name: "Repartir");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Pilote");

            migrationBuilder.DropTable(
                name: "Passager");

            migrationBuilder.DropTable(
                name: "Vol");

            migrationBuilder.DropTable(
                name: "Avion");

            migrationBuilder.DropTable(
                name: "Date_vol");

            migrationBuilder.DropTable(
                name: "Trajet");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Statut_avion");

            migrationBuilder.DropTable(
                name: "Compagnie");
        }
    }
}
