using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back.Migrations
{
    /// <inheritdoc />
    public partial class firstessai03 : Migration
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
                name: "Modele_avion",
                columns: table => new
                {
                    code_modele = table.Column<string>(type: "text", nullable: false),
                    libelle_modele = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modele_avion", x => x.code_modele);
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
                    numero_place = table.Column<string>(type: "text", nullable: false)
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
                name: "Classe",
                columns: table => new
                {
                    code_classe = table.Column<string>(type: "text", nullable: false),
                    libelle_classe = table.Column<string>(type: "text", nullable: false),
                    Placenumero_place = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classe", x => x.code_classe);
                    table.ForeignKey(
                        name: "FK_Classe_Place_Placenumero_place",
                        column: x => x.Placenumero_place,
                        principalTable: "Place",
                        principalColumn: "numero_place",
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
                    fk_passeport = table.Column<string>(type: "text", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Avion",
                columns: table => new
                {
                    id_avion = table.Column<string>(type: "text", nullable: false),
                    nom_avion = table.Column<string>(type: "text", nullable: false),
                    fk_code_modele = table.Column<string>(type: "text", nullable: false),
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
                    table.ForeignKey(
                        name: "fk_avion_modele",
                        column: x => x.fk_code_modele,
                        principalTable: "Modele_avion",
                        principalColumn: "code_modele",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClasseModele_avion",
                columns: table => new
                {
                    Classescode_classe = table.Column<string>(type: "text", nullable: false),
                    Modele_Avionscode_modele = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasseModele_avion", x => new { x.Classescode_classe, x.Modele_Avionscode_modele });
                    table.ForeignKey(
                        name: "FK_ClasseModele_avion_Classe_Classescode_classe",
                        column: x => x.Classescode_classe,
                        principalTable: "Classe",
                        principalColumn: "code_classe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClasseModele_avion_Modele_avion_Modele_Avionscode_modele",
                        column: x => x.Modele_Avionscode_modele,
                        principalTable: "Modele_avion",
                        principalColumn: "code_modele",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diviser",
                columns: table => new
                {
                    fk_code_modele = table.Column<string>(type: "text", nullable: false),
                    fk_code_classe = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diviser", x => new { x.fk_code_modele, x.fk_code_classe });
                    table.ForeignKey(
                        name: "fk_diviser_classe",
                        column: x => x.fk_code_classe,
                        principalTable: "Classe",
                        principalColumn: "code_classe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_diviser_modele_avion",
                        column: x => x.fk_code_modele,
                        principalTable: "Modele_avion",
                        principalColumn: "code_modele",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regrouper",
                columns: table => new
                {
                    fk_code_classe = table.Column<string>(type: "text", nullable: false),
                    fk_numero_place = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regrouper", x => new { x.fk_code_classe, x.fk_numero_place });
                    table.ForeignKey(
                        name: "fk_regrouper_classe",
                        column: x => x.fk_code_classe,
                        principalTable: "Classe",
                        principalColumn: "code_classe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_regrouper_place",
                        column: x => x.fk_code_classe,
                        principalTable: "Place",
                        principalColumn: "numero_place",
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
                    date_depart = table.Column<string>(type: "text", nullable: false),
                    date_arrivee = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Affecter_fk_id_vol",
                table: "Affecter",
                column: "fk_id_vol");

            migrationBuilder.CreateIndex(
                name: "IX_Avion_fk_code_modele",
                table: "Avion",
                column: "fk_code_modele");

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
                name: "IX_Classe_Placenumero_place",
                table: "Classe",
                column: "Placenumero_place");

            migrationBuilder.CreateIndex(
                name: "IX_ClasseModele_avion_Modele_Avionscode_modele",
                table: "ClasseModele_avion",
                column: "Modele_Avionscode_modele");

            migrationBuilder.CreateIndex(
                name: "IX_Diviser_fk_code_classe",
                table: "Diviser",
                column: "fk_code_classe");

            migrationBuilder.CreateIndex(
                name: "IX_Posseder_fk_numero_place",
                table: "Posseder",
                column: "fk_numero_place");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_fk_numero_place",
                table: "Reservation",
                column: "fk_numero_place");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_fk_passeport",
                table: "Reservation",
                column: "fk_passeport");

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
                name: "ClasseModele_avion");

            migrationBuilder.DropTable(
                name: "Diviser");

            migrationBuilder.DropTable(
                name: "Posseder");

            migrationBuilder.DropTable(
                name: "Regrouper");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Pilote");

            migrationBuilder.DropTable(
                name: "Vol");

            migrationBuilder.DropTable(
                name: "Classe");

            migrationBuilder.DropTable(
                name: "Passager");

            migrationBuilder.DropTable(
                name: "Avion");

            migrationBuilder.DropTable(
                name: "Trajet");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Statut_avion");

            migrationBuilder.DropTable(
                name: "Compagnie");

            migrationBuilder.DropTable(
                name: "Modele_avion");
        }
    }
}
