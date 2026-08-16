BEGIN;

-- Nettoyage préalable si vous aviez déjà inséré des données de test
TRUNCATE TABLE 
    public."Affecter",
    public."Billet",
    public."Caracteriser",
    public."Posseder",
    public."Repartir",
    public."Reservation",
    public."Vol",
    public."Avion",
    public."Statut_avion",
    public."Place",
    public."Pilote",
    public."Passager",
    public."Date_vol",
    public."Trajet",
    public."Compagnie"
CASCADE;

-- ==========================================
-- 1. COMPAGNIES AÉRIENNES (5)
-- ==========================================
INSERT INTO public."Compagnie" (id_compagnie, nom_compagnie, tel_compagnie, email_compagnie) VALUES
('CMP01', 'Air France', '+33141567800', 'contact@airfrance.fr'),
('CMP02', 'Emirates', '+97142864000', 'support@emirates.com'),
('CMP03', 'Lufthansa', '+496986799799', 'info@lufthansa.de'),
('CMP04', 'Delta Air Lines', '+18002211212', 'support@delta.com'),
('CMP05', 'Qatar Airways', '+97440226000', 'support@qatarairways.com');

-- ==========================================
-- 2. TRAJETS (8)
-- ==========================================
INSERT INTO public."Trajet" (id_trajet, lieu_depart, destination) VALUES
('TRJ01', 'Paris (CDG)', 'New York (JFK)'),
('TRJ02', 'Paris (CDG)', 'Tokyo (HND)'),
('TRJ03', 'Francfort (FRA)', 'Dubaï (DXB)'),
('TRJ04', 'Dubaï (DXB)', 'Sydney (SYD)'),
('TRJ05', 'Atlanta (ATL)', 'Paris (CDG)'),
('TRJ06', 'Doha (DOH)', 'Londres (LHR)'),
('TRJ07', 'Tokyo (HND)', 'Los Angeles (LAX)'),
('TRJ08', 'Paris (CDG)', 'Rome (FCO)');

-- ==========================================
-- 3. DATES DE VOL (8)
-- ==========================================
INSERT INTO public."Date_vol" (date_depart) VALUES
('2026-09-01 08:30:00'),
('2026-09-01 14:15:00'),
('2026-09-01 22:00:00'),
('2026-09-02 06:45:00'),
('2026-09-02 11:30:00'),
('2026-09-02 20:45:00'),
('2026-09-03 10:00:00'),
('2026-09-03 18:20:00');

-- ==========================================
-- 4. PASSAGERS (15)
-- ==========================================
INSERT INTO public."Passager" (passeport, nom_passager, prenom_passager, tel_passager, categorie_passager) VALUES
('PASS-FR-001', 'Dupont', 'Jean', '+33612345678', 'Adulte'),
('PASS-FR-002', 'Martin', 'Sophie', '+33687654321', 'VIP'),
('PASS-US-003', 'Smith', 'John', '+12025550143', 'Adulte'),
('PASS-DE-004', 'Müller', 'Hans', '+49151234567', 'Enfant'),
('PASS-FR-005', 'Moreau', 'Lucas', '+33611223344', 'Adulte'),
('PASS-AE-006', 'Al-Mansoor', 'Tariq', '+971501234567', 'VIP'),
('PASS-US-007', 'Johnson', 'Emily', '+13125550199', 'Adulte'),
('PASS-JP-008', 'Takahashi', 'Kenji', '+819012345678', 'Adulte'),
('PASS-FR-009', 'Bernard', 'Chloe', '+33655443322', 'Etudiant'),
('PASS-GB-010', 'Williams', 'Oliver', '+447911123456', 'Adulte'),
('PASS-DE-011', 'Schneider', 'Emma', '+491609876543', 'Adulte'),
('PASS-US-012', 'Brown', 'Michael', '+14155552671', 'Senior'),
('PASS-QA-013', 'Al-Thani', 'Noora', '+97455123456', 'VIP'),
('PASS-FR-014', 'Petit', 'Antoine', '+33699887766', 'Enfant'),
('PASS-IT-015', 'Rossi', 'Giulia', '+393331234567', 'Adulte');

-- ==========================================
-- 5. PILOTES (8)
-- ==========================================
INSERT INTO public."Pilote" (id_pilote, nom_pilote, prenom_pilote, tel_pilote) VALUES
('PIL01', 'Tardy', 'Michel', '+33600112233'),
('PIL02', 'Alves', 'Carlos', '+33644556677'),
('PIL03', 'Bernard', 'Claire', '+33688990011'),
('PIL04', 'O’Connor', 'Liam', '+12025550188'),
('PIL05', 'Weber', 'Thomas', '+491701234567'),
('PIL06', 'Sato', 'Hiroshi', '+818098765432'),
('PIL07', 'Dubois', 'Pierre', '+33677889900'),
('PIL08', 'Al-Hassan', 'Youssef', '+971559988776');

-- ==========================================
-- 6. PLACES (12)
-- ==========================================
INSERT INTO public."Place" (numero_place, classe_siege, occupee) VALUES
('1A', 'Premiere', 1),
('1B', 'Premiere', 0),
('2A', 'Premiere', 1),
('10A', 'Business', 1),
('10B', 'Business', 1),
('11A', 'Business', 0),
('20A', 'Economique', 1),
('20B', 'Economique', 1),
('20C', 'Economique', 1),
('21A', 'Economique', 0),
('21B', 'Economique', 1),
('22A', 'Economique', 1);

-- ==========================================
-- 7. STATUTS D'AVION (4)
-- ==========================================
INSERT INTO public."Statut_avion" (code_statut, libelle_statut) VALUES
('STAT-AU-SOL', 'Au sol (Prêt)'),
('STAT-EN-VOL', 'En vol'),
('STAT-MAINT', 'En maintenance'),
('STAT-INSP', 'Inspection de routine');

-- ==========================================
-- 8. AVIONS (7)
-- ==========================================
INSERT INTO public."Avion" (id_avion, nom_avion, fk_id_compagnie, "Placenumero_place", "Statut_avioncode_statut") VALUES
('AV01', 'Boeing 777-300ER', 'CMP01', '1A', 'STAT-AU-SOL'),
('AV02', 'Airbus A380-800', 'CMP02', '10A', 'STAT-EN-VOL'),
('AV03', 'Airbus A350-900', 'CMP03', '20A', 'STAT-MAINT'),
('AV04', 'Boeing 787-9', 'CMP04', '1B', 'STAT-AU-SOL'),
('AV05', 'Airbus A350-1000', 'CMP05', '2A', 'STAT-EN-VOL'),
('AV06', 'Airbus A320neo', 'CMP01', '21B', 'STAT-INSP'),
('AV07', 'Boeing 777X', 'CMP02', '10B', 'STAT-AU-SOL');

-- ==========================================
-- 9. VOLS (8)
-- ==========================================
INSERT INTO public."Vol" (id_vol, fk_date_depart, fk_id_trajet, fk_id_avion) VALUES
('VOL101', '2026-09-01 08:30:00', 'TRJ01', 'AV01'),
('VOL102', '2026-09-01 14:15:00', 'TRJ02', 'AV01'),
('VOL201', '2026-09-01 22:00:00', 'TRJ03', 'AV03'),
('VOL202', '2026-09-02 06:45:00', 'TRJ04', 'AV02'),
('VOL301', '2026-09-02 11:30:00', 'TRJ05', 'AV04'),
('VOL302', '2026-09-02 20:45:00', 'TRJ06', 'AV05'),
('VOL401', '2026-09-03 10:00:00', 'TRJ07', 'AV07'),
('VOL402', '2026-09-03 18:20:00', 'TRJ08', 'AV06');

-- ==========================================
-- 10. BILLETS (12)
-- ==========================================
INSERT INTO public."Billet" (numero_billet, fk_passeport, fk_id_vol) VALUES
(10001, 'PASS-FR-001', 'VOL101'),
(10002, 'PASS-FR-002', 'VOL101'),
(10003, 'PASS-US-003', 'VOL101'),
(10004, 'PASS-DE-004', 'VOL201'),
(10005, 'PASS-FR-005', 'VOL102'),
(10006, 'PASS-AE-006', 'VOL202'),
(10007, 'PASS-US-007', 'VOL301'),
(10008, 'PASS-JP-008', 'VOL401'),
(10009, 'PASS-FR-009', 'VOL402'),
(10010, 'PASS-GB-010', 'VOL302'),
(10011, 'PASS-QA-013', 'VOL302'),
(10012, 'PASS-IT-015', 'VOL402');

-- ==========================================
-- 11. RÉSERVATIONS (12)
-- ==========================================
INSERT INTO public."Reservation" (id_reservation, date_reservation, valide, fk_numero_place, fk_passeport, fk_id_vol) VALUES
('RES001', '2026-08-10', 1, '1A', 'PASS-FR-001', 'VOL101'),
('RES002', '2026-08-11', 1, '1B', 'PASS-FR-002', 'VOL101'),
('RES003', '2026-08-12', 1, '20A', 'PASS-US-003', 'VOL101'),
('RES004', '2026-08-14', 1, '20B', 'PASS-DE-004', 'VOL201'),
('RES005', '2026-08-15', 0, '10A', 'PASS-FR-005', 'VOL102'),
('RES006', '2026-08-16', 1, '2A', 'PASS-AE-006', 'VOL202'),
('RES007', '2026-08-18', 1, '10B', 'PASS-US-007', 'VOL301'),
('RES008', '2026-08-19', 1, '20C', 'PASS-JP-008', 'VOL401'),
('RES009', '2026-08-20', 1, '21B', 'PASS-FR-009', 'VOL402'),
('RES010', '2026-08-21', 1, '11A', 'PASS-GB-010', 'VOL302'),
('RES011', '2026-08-22', 1, '1A', 'PASS-QA-013', 'VOL302'),
('RES012', '2026-08-23', 1, '22A', 'PASS-IT-015', 'VOL402');

-- ==========================================
-- 12. TABLES D'ASSOCIATION (N-N)
-- ==========================================

-- Table Affecter (Affectation de plusieurs pilotes par vol)
INSERT INTO public."Affecter" (fk_id_vol, fk_id_pilote) VALUES
('VOL101', 'PIL01'),
('VOL101', 'PIL02'),
('VOL102', 'PIL01'),
('VOL201', 'PIL05'),
('VOL202', 'PIL08'),
('VOL301', 'PIL04'),
('VOL302', 'PIL03'),
('VOL401', 'PIL06'),
('VOL402', 'PIL07');

-- Table Posseder (Répartition des sièges dans les avions)
INSERT INTO public."Posseder" (fk_numero_place, fk_id_avion) VALUES
('1A', 'AV01'),
('1B', 'AV01'),
('10A', 'AV01'),
('20A', 'AV01'),
('20B', 'AV01'),
('2A', 'AV02'),
('10B', 'AV02'),
('11A', 'AV02'),
('20C', 'AV03'),
('21A', 'AV04'),
('21B', 'AV05'),
('22A', 'AV06');

-- Table Caracteriser (Historique des statuts des avions)
INSERT INTO public."Caracteriser" (fk_code_statut, fk_id_avion) VALUES
('STAT-AU-SOL', 'AV01'),
('STAT-EN-VOL', 'AV02'),
('STAT-MAINT', 'AV03'),
('STAT-AU-SOL', 'AV04'),
('STAT-EN-VOL', 'AV05'),
('STAT-INSP', 'AV06'),
('STAT-AU-SOL', 'AV07');

-- Table Repartir (Trajets desservis par les compagnies)
INSERT INTO public."Repartir" (fk_id_compagnie, fk_id_trajet) VALUES
('CMP01', 'TRJ01'),
('CMP01', 'TRJ02'),
('CMP01', 'TRJ08'),
('CMP02', 'TRJ03'),
('CMP02', 'TRJ04'),
('CMP03', 'TRJ03'),
('CMP04', 'TRJ01'),
('CMP04', 'TRJ05'),
('CMP05', 'TRJ06');

COMMIT;