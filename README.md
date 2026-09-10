# Refuge Animaux — Application WPF / MVVM

Application graphique de gestion d’un refuge animalier réalisée dans le cadre d’un projet de développement SGBD.

Cette version complète la partie Console avec une interface **WPF en C#/.NET** organisée selon le pattern **MVVM** et connectée à une base **PostgreSQL** via **Npgsql**. Une partie importante des opérations métier repose également sur des **procédures et fonctions PL/pgSQL**.

> La version Console du projet est disponible dans le dépôt `ProjetSGDB_RefugeAnimaux_Console`.

## Fonctionnalités principales

### Tableau de bord
- nombre total d’animaux ;
- animaux présents au refuge ;
- animaux adoptés ;
- animaux en famille d’accueil ;
- contacts, adoptions, familles d’accueil et vaccinations.

### Gestion des animaux
- liste et recherche ;
- ajout d’un animal avec son entrée au refuge ;
- affichage de l’état actuel ;
- fiche détaillée avec couleurs, compatibilités, vaccinations et historique.

### Gestion des contacts
- liste et recherche ;
- ajout et modification ;
- fiche détaillée avec rôles, adoptions, familles d’accueil, entrées et sorties.

### Adoptions et familles d’accueil
- création d’une demande d’adoption ;
- modification du statut ;
- création d’une sortie lors d’une adoption acceptée ;
- ajout et clôture d’une famille d’accueil.

### Autres données
- vaccins et vaccinations ;
- couleurs ;
- rôles ;
- compatibilités ;
- diagnostic de cohérence.

## Stack technique

- C#
- .NET / WPF
- XAML
- MVVM
- PostgreSQL
- Npgsql
- SQL / PL/pgSQL
- Visual Studio
- Git / GitHub

## Architecture

```text
ProjetSGDB_RefugeAnimauxWPF/
├── MainWindow.xaml
├── classesMetier/
├── coucheAccesBD/
├── coucheVueModele/
├── commandes/
└── SQL/
```

Organisation MVVM :

```text
Vue          -> MainWindow.xaml
Vue-modèle   -> MainVueModele.cs
Modèle       -> classesMetier/
Accès BD     -> coucheAccesBD/AccesBD.cs
Commandes    -> commandes/CommandeRelais.cs
```

## Base de données

La base utilisée est :

```text
refuge_animaux
```

Elle contient notamment les entités liées aux animaux, contacts, vaccinations, adoptions, familles d’accueil, entrées/sorties, rôles, couleurs et compatibilités.

Les scripts SQL et procédures stockées sont conservés dans le dossier `SQL`.

## Configuration PostgreSQL

Aucun mot de passe n’est stocké dans le dépôt.

La connexion peut être fournie via :

```text
REFUGE_ANIMAUX_DB
```

ou avec :

```text
REFUGE_DB_HOST
REFUGE_DB_NAME
REFUGE_DB_USER
REFUGE_DB_PASSWORD
```

Exemple de chaîne complète :

```text
Host=localhost;Database=refuge_animaux;Username=postgres;Password=VOTRE_MOT_DE_PASSE
```

## Procédures et fonctions PostgreSQL

L’application s’appuie notamment sur des procédures pour :

```text
ajouter_contact
modifier_contact
ajouter_animal_avec_entree
ajouter_adoption
modifier_statut_adoption
ajouter_famille_accueil
cloturer_famille_accueil
ajouter_vaccin
ajouter_vaccination
ajouter_couleur
ajouter_role
ajouter_couleur_animal
ajouter_role_contact
ajouter_compatibilite_animal
```

Elle utilise aussi des fonctions d’affichage, de tableau de bord, de fiches détaillées et de diagnostic de cohérence.

## Points techniques mis en pratique

- développement desktop avec WPF/XAML ;
- architecture MVVM ;
- séparation modèle / vue / vue-modèle ;
- commandes MVVM ;
- accès PostgreSQL avec Npgsql ;
- requêtes paramétrées ;
- procédures stockées et fonctions PL/pgSQL ;
- contraintes et règles métier en base ;
- configuration sensible externalisée ;
- gestion d’une application métier avec plusieurs entités liées.

## Exécution

1. Configurer PostgreSQL et exécuter les scripts nécessaires du dossier `SQL`.
2. Définir les variables d’environnement de connexion.
3. Ouvrir le projet dans Visual Studio.
4. Lancer avec `F5`.

## Limites et améliorations possibles

- découper `MainVueModele.cs` en ViewModels spécialisés ;
- augmenter la couverture de tests ;
- ajouter certaines suppressions avancées ;
- améliorer encore l’interface ;
- ajouter un export CSV ;
- enrichir les messages d’erreur utilisateur.

## Contexte

Projet réalisé par **Harlem Kponve Alofa**, étudiant en **Bachelier en Informatique — orientation Développement d’applications**.

Ce dépôt met principalement en évidence des compétences en **C#, .NET, WPF, XAML, MVVM, PostgreSQL, SQL/PLpgSQL et architecture d’applications métier**.
