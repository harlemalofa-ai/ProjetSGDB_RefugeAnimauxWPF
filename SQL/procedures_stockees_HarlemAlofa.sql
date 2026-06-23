-- ============================================================
-- SQL FINAL MIS A JOUR - REFUGE ANIMAUX WPF
-- Harlem Alofa
-- A executer dans pgAdmin sur la base : refuge_animaux
-- ============================================================

-- ============================================================
-- NETTOYAGE DES FONCTIONS
-- ============================================================

DROP FUNCTION IF EXISTS get_animaux();
DROP FUNCTION IF EXISTS get_contacts();
DROP FUNCTION IF EXISTS get_adoptions();
DROP FUNCTION IF EXISTS get_familles_accueil();
DROP FUNCTION IF EXISTS get_vaccins();
DROP FUNCTION IF EXISTS get_vaccinations();
DROP FUNCTION IF EXISTS get_couleurs();
DROP FUNCTION IF EXISTS get_roles();
DROP FUNCTION IF EXISTS get_compatibilites();
DROP FUNCTION IF EXISTS get_animaux_presents();
DROP FUNCTION IF EXISTS get_tableau_bord();
DROP FUNCTION IF EXISTS verifier_coherence_refuge();
DROP FUNCTION IF EXISTS get_etat_animal(varchar);

DROP FUNCTION IF EXISTS get_fiche_animal_couleurs(text);
DROP FUNCTION IF EXISTS get_fiche_animal_compatibilites(text);
DROP FUNCTION IF EXISTS get_fiche_animal_vaccinations(text);
DROP FUNCTION IF EXISTS get_fiche_animal_historique(text);
DROP FUNCTION IF EXISTS get_fiche_animal_adoptions(text);
DROP FUNCTION IF EXISTS get_fiche_animal_familles(text);

DROP FUNCTION IF EXISTS get_fiche_contact_roles(integer);
DROP FUNCTION IF EXISTS get_fiche_contact_adoptions(integer);
DROP FUNCTION IF EXISTS get_fiche_contact_familles(integer);
DROP FUNCTION IF EXISTS get_fiche_contact_entrees(integer);
DROP FUNCTION IF EXISTS get_fiche_contact_sorties(integer);

-- ============================================================
-- NETTOYAGE DES PROCEDURES
-- ============================================================

DROP PROCEDURE IF EXISTS ajouter_contact(integer, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar);
DROP PROCEDURE IF EXISTS ajouter_contact(integer, text, text, text, text, text, text, text, text, text);

DROP PROCEDURE IF EXISTS modifier_contact(integer, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar);
DROP PROCEDURE IF EXISTS modifier_contact(integer, text, text, text, text, text, text, text, text, text);

DROP PROCEDURE IF EXISTS ajouter_animal_avec_entree(varchar, varchar, varchar, varchar, text, text, boolean, date, varchar, integer, date);
DROP PROCEDURE IF EXISTS ajouter_animal_avec_entree(text, text, text, text, text, text, boolean, timestamp without time zone, text, integer, timestamp without time zone);

DROP PROCEDURE IF EXISTS ajouter_adoption(varchar, integer, date, varchar);
DROP PROCEDURE IF EXISTS ajouter_adoption(text, integer, timestamp without time zone, text);

DROP PROCEDURE IF EXISTS modifier_statut_adoption(varchar, integer, date, varchar);
DROP PROCEDURE IF EXISTS modifier_statut_adoption(text, integer, timestamp without time zone, text);

DROP PROCEDURE IF EXISTS ajouter_famille_accueil(varchar, integer, date);
DROP PROCEDURE IF EXISTS ajouter_famille_accueil(text, integer, timestamp without time zone);

DROP PROCEDURE IF EXISTS cloturer_famille_accueil(varchar, integer, date);
DROP PROCEDURE IF EXISTS cloturer_famille_accueil(text, integer, timestamp without time zone);

DROP PROCEDURE IF EXISTS ajouter_vaccin(integer, varchar);
DROP PROCEDURE IF EXISTS ajouter_vaccin(integer, text);

DROP PROCEDURE IF EXISTS ajouter_vaccination(varchar, integer, date);
DROP PROCEDURE IF EXISTS ajouter_vaccination(text, integer, timestamp without time zone);

DROP PROCEDURE IF EXISTS ajouter_couleur(integer, varchar);
DROP PROCEDURE IF EXISTS ajouter_couleur(integer, text);

DROP PROCEDURE IF EXISTS ajouter_role(integer, varchar);
DROP PROCEDURE IF EXISTS ajouter_role(integer, text);

DROP PROCEDURE IF EXISTS ajouter_couleur_animal(varchar, integer);
DROP PROCEDURE IF EXISTS ajouter_couleur_animal(text, integer);

DROP PROCEDURE IF EXISTS ajouter_role_contact(integer, integer);

DROP PROCEDURE IF EXISTS ajouter_compatibilite_animal(varchar, integer, varchar, text);
DROP PROCEDURE IF EXISTS ajouter_compatibilite_animal(text, integer, text, text);

-- ============================================================
-- ETAT ACTUEL D'UN ANIMAL
-- ============================================================

CREATE OR REPLACE FUNCTION get_etat_animal(p_animal varchar)
RETURNS text
AS $$
DECLARE
    v_date_deces date;
    v_derniere_entree date;
    v_derniere_sortie date;
    v_raison_sortie text;
    v_adoptions_acceptees integer;
    v_familles_actives integer;
BEGIN
    SELECT a.date_deces
    INTO v_date_deces
    FROM animal a
    WHERE a.identifiant = p_animal;

    IF NOT FOUND THEN
        RETURN 'inconnu';
    END IF;

    IF v_date_deces IS NOT NULL THEN
        RETURN 'decede';
    END IF;

    SELECT COUNT(*)
    INTO v_adoptions_acceptees
    FROM adoption ad
    WHERE ad.ani_identifiant = p_animal
    AND ad.statut = 'acceptee';

    IF v_adoptions_acceptees > 0 THEN
        RETURN 'adopte';
    END IF;

    SELECT COUNT(*)
    INTO v_familles_actives
    FROM famille_accueil fa
    WHERE fa.fa_ani_identifiant = p_animal
    AND fa.date_fin IS NULL;

    IF v_familles_actives > 0 THEN
        RETURN 'famille_accueil';
    END IF;

    SELECT MAX(ae.date_entree)
    INTO v_derniere_entree
    FROM ani_entree ae
    WHERE ae.ani_identifiant = p_animal;

    IF v_derniere_entree IS NULL THEN
        RETURN 'sans_entree';
    END IF;

    SELECT MAX(s.date_sortie)
    INTO v_derniere_sortie
    FROM ani_sortie s
    WHERE s.ani_identifiant = p_animal;

    IF v_derniere_sortie IS NULL THEN
        RETURN 'au_refuge';
    END IF;

    SELECT s.raison
    INTO v_raison_sortie
    FROM ani_sortie s
    WHERE s.ani_identifiant = p_animal
    ORDER BY s.date_sortie DESC
    LIMIT 1;

    IF v_derniere_entree >= v_derniere_sortie THEN
        RETURN 'au_refuge';
    END IF;

    IF v_raison_sortie = 'retour_proprietaire' THEN
        RETURN 'retour_proprietaire';
    ELSIF v_raison_sortie = 'deces_animal' THEN
        RETURN 'decede';
    ELSIF v_raison_sortie = 'famille_accueil' THEN
        RETURN 'famille_accueil';
    ELSIF v_raison_sortie = 'adoption' THEN
        RETURN 'adopte';
    END IF;

    RETURN 'sorti';
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- FONCTIONS D'AFFICHAGE PRINCIPALES
-- ============================================================

CREATE OR REPLACE FUNCTION get_animaux()
RETURNS TABLE (
    identifiant varchar,
    nom varchar,
    type varchar,
    sexe varchar,
    particularites text,
    date_deces date,
    description text,
    date_sterilisation date,
    sterilise boolean,
    date_naissance date,
    etat_actuel text
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.identifiant::varchar,
        a.nom::varchar,
        a.type::varchar,
        a.sexe::varchar,
        a.particularites::text,
        a.date_deces,
        a.description::text,
        a.date_sterilisation,
        a.sterilise,
        a.date_naissance,
        get_etat_animal(a.identifiant)::text
    FROM animal a
    ORDER BY a.nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_contacts()
RETURNS TABLE (
    contact_identifiant integer,
    nom varchar,
    prenom varchar,
    rue varchar,
    cp varchar,
    localite varchar,
    registre_national varchar,
    gsm varchar,
    telephone varchar,
    email varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        c.contact_identifiant,
        c.nom::varchar,
        c.prenom::varchar,
        c.rue::varchar,
        c.cp::varchar,
        c.localite::varchar,
        c.registre_national::varchar,
        c.gsm::varchar,
        c.telephone::varchar,
        c.email::varchar
    FROM contact c
    ORDER BY c.nom, c.prenom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_adoptions()
RETURNS TABLE (
    animal varchar,
    contact integer,
    date_demande date,
    statut varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        ad.ani_identifiant::varchar,
        ad.adop_contact,
        ad.date_demande,
        ad.statut::varchar
    FROM adoption ad
    ORDER BY ad.date_demande DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_familles_accueil()
RETURNS TABLE (
    animal varchar,
    contact integer,
    date_debut date,
    date_fin date
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        fa.fa_ani_identifiant::varchar,
        fa.fa_contact,
        fa.date_debut,
        fa.date_fin
    FROM famille_accueil fa
    ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_vaccins()
RETURNS TABLE (
    identifiant integer,
    nom varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        v.identifiant,
        v.nom::varchar
    FROM vaccin v
    ORDER BY v.nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_vaccinations()
RETURNS TABLE (
    animal varchar,
    vaccin integer,
    nom_vaccin varchar,
    date_vaccination date
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        va.vac_animal::varchar,
        va.id_vaccin,
        v.nom::varchar,
        va.vaccination_date
    FROM vaccination va
    JOIN vaccin v ON v.identifiant = va.id_vaccin
    ORDER BY va.vaccination_date DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_couleurs()
RETURNS TABLE (
    identifiant integer,
    nom varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        c.col_identifiant,
        c.nom_couleur::varchar
    FROM couleur c
    ORDER BY c.nom_couleur;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_roles()
RETURNS TABLE (
    identifiant integer,
    nom varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        r.rol_identifiant,
        r.rol_nom::varchar
    FROM role r
    ORDER BY r.rol_nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_compatibilites()
RETURNS TABLE (
    identifiant integer,
    type varchar
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        comp.identifiant,
        comp.type::varchar
    FROM compatibilite comp
    ORDER BY comp.type;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_animaux_presents()
RETURNS TABLE (
    identifiant varchar,
    nom varchar,
    type varchar,
    sexe varchar,
    etat_actuel text
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.identifiant::varchar,
        a.nom::varchar,
        a.type::varchar,
        a.sexe::varchar,
        get_etat_animal(a.identifiant)::text
    FROM animal a
    WHERE get_etat_animal(a.identifiant) = 'au_refuge'
    ORDER BY a.nom;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- DASHBOARD ET DIAGNOSTIC
-- ============================================================

CREATE OR REPLACE FUNCTION get_tableau_bord()
RETURNS TABLE (
    element text,
    valeur integer
)
AS $$
BEGIN
    RETURN QUERY
    SELECT 'Animaux total'::text, COUNT(*)::integer FROM animal a
    UNION ALL
    SELECT 'Animaux au refuge'::text, COUNT(*)::integer FROM animal a WHERE get_etat_animal(a.identifiant) = 'au_refuge'
    UNION ALL
    SELECT 'Animaux adoptés'::text, COUNT(*)::integer FROM animal a WHERE get_etat_animal(a.identifiant) = 'adopte'
    UNION ALL
    SELECT 'Animaux en famille'::text, COUNT(*)::integer FROM animal a WHERE get_etat_animal(a.identifiant) = 'famille_accueil'
    UNION ALL
    SELECT 'Animaux décédés'::text, COUNT(*)::integer FROM animal a WHERE get_etat_animal(a.identifiant) = 'decede'
    UNION ALL
    SELECT 'Contacts'::text, COUNT(*)::integer FROM contact c
    UNION ALL
    SELECT 'Adoptions'::text, COUNT(*)::integer FROM adoption ad
    UNION ALL
    SELECT 'Familles accueil'::text, COUNT(*)::integer FROM famille_accueil fa
    UNION ALL
    SELECT 'Vaccinations'::text, COUNT(*)::integer FROM vaccination va;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION verifier_coherence_refuge()
RETURNS TABLE (
    probleme text
)
AS $$
BEGIN
    RETURN QUERY
    SELECT ('Animal avec plusieurs familles d''accueil actives : ' || fa.fa_ani_identifiant)::text
    FROM famille_accueil fa
    WHERE fa.date_fin IS NULL
    GROUP BY fa.fa_ani_identifiant
    HAVING COUNT(*) > 1

    UNION ALL

    SELECT ('Animal avec plusieurs adoptions acceptées : ' || ad.ani_identifiant)::text
    FROM adoption ad
    WHERE ad.statut = 'acceptee'
    GROUP BY ad.ani_identifiant
    HAVING COUNT(*) > 1

    UNION ALL

    SELECT ('Sortie sans entrée : ' || s.ani_identifiant)::text
    FROM ani_sortie s
    WHERE NOT EXISTS (
        SELECT 1
        FROM ani_entree e
        WHERE e.ani_identifiant = s.ani_identifiant
    )

    UNION ALL

    SELECT ('Date de décès avant naissance : ' || a.identifiant)::text
    FROM animal a
    WHERE a.date_deces IS NOT NULL
    AND a.date_deces < a.date_naissance;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- PROCEDURES : CONTACTS
-- ============================================================

CREATE OR REPLACE PROCEDURE ajouter_contact(
    p_contact_identifiant integer,
    p_nom text,
    p_prenom text,
    p_rue text,
    p_cp text,
    p_localite text,
    p_registre_national text,
    p_gsm text,
    p_telephone text,
    p_email text
)
AS $$
BEGIN
    INSERT INTO contact (
        contact_identifiant, nom, prenom, rue, cp, localite,
        registre_national, gsm, telephone, email
    )
    VALUES (
        p_contact_identifiant, p_nom, p_prenom, p_rue, p_cp, p_localite,
        p_registre_national, NULLIF(p_gsm, ''), NULLIF(p_telephone, ''), NULLIF(p_email, '')
    );
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE modifier_contact(
    p_contact_identifiant integer,
    p_nom text,
    p_prenom text,
    p_rue text,
    p_cp text,
    p_localite text,
    p_registre_national text,
    p_gsm text,
    p_telephone text,
    p_email text
)
AS $$
BEGIN
    UPDATE contact
    SET
        nom = p_nom,
        prenom = p_prenom,
        rue = p_rue,
        cp = p_cp,
        localite = p_localite,
        registre_national = p_registre_national,
        gsm = NULLIF(p_gsm, ''),
        telephone = NULLIF(p_telephone, ''),
        email = NULLIF(p_email, '')
    WHERE contact_identifiant = p_contact_identifiant;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- PROCEDURES : ANIMAUX / ADOPTIONS / FAMILLES / VACCINS
-- ============================================================

CREATE OR REPLACE PROCEDURE ajouter_animal_avec_entree(
    p_identifiant text,
    p_nom text,
    p_type text,
    p_sexe text,
    p_particularites text,
    p_description text,
    p_sterilise boolean,
    p_date_naissance timestamp without time zone,
    p_raison_entree text,
    p_contact_entree integer,
    p_date_entree timestamp without time zone
)
AS $$
BEGIN
    INSERT INTO animal (
        identifiant, nom, type, sexe, particularites, date_deces,
        description, date_sterilisation, sterilise, date_naissance
    )
    VALUES (
        p_identifiant, p_nom, p_type, p_sexe, NULLIF(p_particularites, ''),
        NULL, NULLIF(p_description, ''), NULL, p_sterilise, p_date_naissance::date
    );

    INSERT INTO ani_entree (ani_identifiant, date_entree, raison, entree_contact)
    VALUES (p_identifiant, p_date_entree::date, p_raison_entree, p_contact_entree);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_adoption(
    p_animal text,
    p_contact integer,
    p_date_demande timestamp without time zone,
    p_statut text
)
AS $$
BEGIN
    IF get_etat_animal(p_animal::varchar) <> 'au_refuge' THEN
        RAISE EXCEPTION 'Animal non disponible pour adoption. Etat actuel : %', get_etat_animal(p_animal::varchar);
    END IF;

    INSERT INTO adoption (ani_identifiant, adop_contact, date_demande, statut)
    VALUES (p_animal, p_contact, p_date_demande::date, p_statut);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE modifier_statut_adoption(
    p_animal text,
    p_contact integer,
    p_date_demande timestamp without time zone,
    p_statut text
)
AS $$
BEGIN
    IF p_statut = 'acceptee' AND get_etat_animal(p_animal::varchar) <> 'au_refuge' THEN
        RAISE EXCEPTION 'Animal non disponible pour adoption acceptée. Etat actuel : %', get_etat_animal(p_animal::varchar);
    END IF;

    UPDATE adoption
    SET statut = p_statut
    WHERE ani_identifiant = p_animal
    AND adop_contact = p_contact
    AND date_demande = p_date_demande::date;

    IF p_statut = 'acceptee' THEN
        INSERT INTO ani_sortie (ani_identifiant, date_sortie, raison, sortie_contact)
        VALUES (p_animal, CURRENT_DATE, 'adoption', p_contact);
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_famille_accueil(
    p_animal text,
    p_contact integer,
    p_date_debut timestamp without time zone
)
AS $$
BEGIN
    IF get_etat_animal(p_animal::varchar) <> 'au_refuge' THEN
        RAISE EXCEPTION 'Animal non disponible pour famille accueil. Etat actuel : %', get_etat_animal(p_animal::varchar);
    END IF;

    INSERT INTO famille_accueil (fa_ani_identifiant, fa_contact, date_debut, date_fin)
    VALUES (p_animal, p_contact, p_date_debut::date, NULL);

    INSERT INTO ani_sortie (ani_identifiant, date_sortie, raison, sortie_contact)
    VALUES (p_animal, p_date_debut::date, 'famille_accueil', p_contact);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE cloturer_famille_accueil(
    p_animal text,
    p_contact_retour integer,
    p_date_fin timestamp without time zone
)
AS $$
BEGIN
    UPDATE famille_accueil fa
    SET date_fin = p_date_fin::date
    WHERE fa.fa_ani_identifiant = p_animal
    AND fa.date_fin IS NULL;

    IF NOT EXISTS (
        SELECT 1
        FROM ani_entree ae
        WHERE ae.ani_identifiant = p_animal
        AND ae.date_entree = p_date_fin::date
    ) THEN
        INSERT INTO ani_entree (ani_identifiant, date_entree, raison, entree_contact)
        VALUES (p_animal, p_date_fin::date, 'retour_adoption', p_contact_retour);
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_vaccin(p_identifiant integer, p_nom text)
AS $$
BEGIN
    INSERT INTO vaccin (identifiant, nom)
    VALUES (p_identifiant, p_nom);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_vaccination(
    p_animal text,
    p_vaccin integer,
    p_date_vaccination timestamp without time zone
)
AS $$
BEGIN
    IF get_etat_animal(p_animal::varchar) IN ('decede', 'retour_proprietaire') THEN
        RAISE EXCEPTION 'Animal non disponible pour vaccination. Etat actuel : %', get_etat_animal(p_animal::varchar);
    END IF;

    INSERT INTO vaccination (vac_animal, id_vaccin, vaccination_date)
    VALUES (p_animal, p_vaccin, p_date_vaccination::date);
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- PROCEDURES : REFERENTIELS ET ASSOCIATIONS
-- ============================================================

CREATE OR REPLACE PROCEDURE ajouter_couleur(p_identifiant integer, p_nom text)
AS $$
BEGIN
    INSERT INTO couleur (col_identifiant, nom_couleur)
    VALUES (p_identifiant, p_nom);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_role(p_identifiant integer, p_nom text)
AS $$
BEGIN
    INSERT INTO role (rol_identifiant, rol_nom)
    VALUES (p_identifiant, p_nom);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_couleur_animal(p_animal text, p_couleur integer)
AS $$
BEGIN
    INSERT INTO animal_couleur (ani_identifiant, col_identifiant)
    VALUES (p_animal, p_couleur);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_role_contact(p_contact integer, p_role integer)
AS $$
BEGIN
    INSERT INTO personne_role (pers_identifiant, rol_identifiant)
    VALUES (p_contact, p_role);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ajouter_compatibilite_animal(
    p_animal text,
    p_compatibilite integer,
    p_valeur text,
    p_description text
)
AS $$
BEGIN
    INSERT INTO ani_compatibilite (ani_identifiant, comp_identifiant, valeur, description)
    VALUES (p_animal, p_compatibilite, p_valeur, NULLIF(p_description, ''));
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- FICHE ANIMAL COMPLETE
-- ============================================================

CREATE OR REPLACE FUNCTION get_fiche_animal_couleurs(p_animal text)
RETURNS TABLE (couleur_id integer, couleur varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT c.col_identifiant, c.nom_couleur::varchar
    FROM animal_couleur ac
    JOIN couleur c ON c.col_identifiant = ac.col_identifiant
    WHERE ac.ani_identifiant = p_animal
    ORDER BY c.nom_couleur;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_animal_compatibilites(p_animal text)
RETURNS TABLE (compatibilite_id integer, type varchar, valeur varchar, description text)
AS $$
BEGIN
    RETURN QUERY
    SELECT comp.identifiant, comp.type::varchar, ac.valeur::varchar, ac.description::text
    FROM ani_compatibilite ac
    JOIN compatibilite comp ON comp.identifiant = ac.comp_identifiant
    WHERE ac.ani_identifiant = p_animal
    ORDER BY comp.type;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_animal_vaccinations(p_animal text)
RETURNS TABLE (vaccin_id integer, vaccin varchar, date_vaccination date)
AS $$
BEGIN
    RETURN QUERY
    SELECT v.identifiant, v.nom::varchar, va.vaccination_date
    FROM vaccination va
    JOIN vaccin v ON v.identifiant = va.id_vaccin
    WHERE va.vac_animal = p_animal
    ORDER BY va.vaccination_date DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_animal_historique(p_animal text)
RETURNS TABLE (type_evenement text, date_evenement date, raison text, contact integer)
AS $$
BEGIN
    RETURN QUERY
    SELECT h.type_evenement, h.date_evenement, h.raison, h.contact
    FROM (
        SELECT 'Entrée'::text AS type_evenement, ae.date_entree AS date_evenement, ae.raison::text AS raison, ae.entree_contact AS contact
        FROM ani_entree ae
        WHERE ae.ani_identifiant = p_animal
        UNION ALL
        SELECT 'Sortie'::text AS type_evenement, s.date_sortie AS date_evenement, s.raison::text AS raison, s.sortie_contact AS contact
        FROM ani_sortie s
        WHERE s.ani_identifiant = p_animal
    ) h
    ORDER BY h.date_evenement DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_animal_adoptions(p_animal text)
RETURNS TABLE (contact integer, date_demande date, statut varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT ad.adop_contact, ad.date_demande, ad.statut::varchar
    FROM adoption ad
    WHERE ad.ani_identifiant = p_animal
    ORDER BY ad.date_demande DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_animal_familles(p_animal text)
RETURNS TABLE (contact integer, date_debut date, date_fin date)
AS $$
BEGIN
    RETURN QUERY
    SELECT fa.fa_contact, fa.date_debut, fa.date_fin
    FROM famille_accueil fa
    WHERE fa.fa_ani_identifiant = p_animal
    ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- FICHE CONTACT COMPLETE
-- ============================================================

CREATE OR REPLACE FUNCTION get_fiche_contact_roles(p_contact integer)
RETURNS TABLE (role_id integer, role_nom varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT r.rol_identifiant, r.rol_nom::varchar
    FROM personne_role pr
    JOIN role r ON r.rol_identifiant = pr.rol_identifiant
    WHERE pr.pers_identifiant = p_contact
    ORDER BY r.rol_nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_contact_adoptions(p_contact integer)
RETURNS TABLE (animal varchar, date_demande date, statut varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT ad.ani_identifiant::varchar, ad.date_demande, ad.statut::varchar
    FROM adoption ad
    WHERE ad.adop_contact = p_contact
    ORDER BY ad.date_demande DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_contact_familles(p_contact integer)
RETURNS TABLE (animal varchar, date_debut date, date_fin date)
AS $$
BEGIN
    RETURN QUERY
    SELECT fa.fa_ani_identifiant::varchar, fa.date_debut, fa.date_fin
    FROM famille_accueil fa
    WHERE fa.fa_contact = p_contact
    ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_contact_entrees(p_contact integer)
RETURNS TABLE (animal varchar, date_entree date, raison varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT ae.ani_identifiant::varchar, ae.date_entree, ae.raison::varchar
    FROM ani_entree ae
    WHERE ae.entree_contact = p_contact
    ORDER BY ae.date_entree DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_fiche_contact_sorties(p_contact integer)
RETURNS TABLE (animal varchar, date_sortie date, raison varchar)
AS $$
BEGIN
    RETURN QUERY
    SELECT s.ani_identifiant::varchar, s.date_sortie, s.raison::varchar
    FROM ani_sortie s
    WHERE s.sortie_contact = p_contact
    ORDER BY s.date_sortie DESC;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- TESTS RAPIDES A EXECUTER APRES CREATION
-- ============================================================

-- SELECT * FROM get_tableau_bord();
-- SELECT * FROM get_animaux();
-- SELECT * FROM get_contacts();
-- SELECT * FROM get_adoptions();
-- SELECT * FROM get_familles_accueil();
-- SELECT * FROM get_vaccins();
-- SELECT * FROM get_vaccinations();
-- SELECT * FROM get_couleurs();
-- SELECT * FROM get_roles();
-- SELECT * FROM get_compatibilites();
-- SELECT * FROM get_animaux_presents();
-- SELECT * FROM verifier_coherence_refuge();
-- SELECT * FROM get_fiche_animal_historique('26060100912');
-- SELECT * FROM get_fiche_contact_roles(9911);
-- SELECT * FROM get_fiche_contact_adoptions(9911);
