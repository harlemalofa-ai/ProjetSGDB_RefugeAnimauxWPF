# Refuge Animaux — Projet de développement SGBD

Projet réalisé par **Harlem Alofa** dans le cadre du cours de projet de développement / système de gestion de base de données.

Ce projet a pour objectif de gérer les informations principales d’un refuge pour animaux : animaux, contacts, adoptions, familles d’accueil, vaccinations, couleurs, rôles, compatibilités, entrées et sorties.

---

## 1. Technologies utilisées

- **C#**
- **.NET / WPF**
- **XAML**
- **MVVM**
- **PostgreSQL**
- **Npgsql**
- **SQL / PLpgSQL**
- **Git / GitHub**

---

## 2. Organisation générale du projet

Le projet contient principalement :

```text
RefugeAnimauxWPF           -> application graphique WPF
SQL                        -> scripts SQL / procédures stockées
classesMetier              -> modèle métier
coucheAccesBD              -> accès PostgreSQL
coucheVueModele            -> ViewModel
commandes                  -> commandes MVVM
```

La partie Console du projet est conservée dans un dépôt séparé : `ProjetSGDB_RefugeAnimaux_Console`.

---

## 3. Base de données

La base de données utilisée s’appelle :

```text
refuge_animaux
```

Elle contient notamment les tables suivantes :

```text
animal
contact
couleur
animal_couleur
compatibilite
ani_compatibilite
vaccin
vaccination
ani_entree
ani_sortie
adoption
famille_accueil
role
personne_role
```

---

## 4. Installation de la base de données

Dans PostgreSQL / pgAdmin :

1. Créer une base de données nommée :

```sql
CREATE DATABASE refuge_animaux;
```

2. Exécuter le script de création des tables.
3. Exécuter le fichier des procédures stockées présent dans le dossier `SQL`.

---

## 5. Configuration de la connexion PostgreSQL

La connexion est gérée dans `coucheAccesBD/AccesBD.cs`.

**Aucun mot de passe n’est stocké dans le dépôt.**

La façon la plus simple consiste à définir la variable d’environnement suivante :

```text
REFUGE_ANIMAUX_DB
```

Exemple :

```text
Host=localhost;Database=refuge_animaux;Username=postgres;Password=VOTRE_MOT_DE_PASSE
```

Il est également possible d’utiliser les variables séparées suivantes :

```text
REFUGE_DB_HOST
REFUGE_DB_NAME
REFUGE_DB_USER
REFUGE_DB_PASSWORD
```

Par défaut, l’hôte, le nom de la base et l’utilisateur valent respectivement `localhost`, `refuge_animaux` et `postgres`. La variable `REFUGE_DB_PASSWORD` est obligatoire si `REFUGE_ANIMAUX_DB` n’est pas définie.

---

## 6. Architecture WPF / MVVM

L’application graphique suit une organisation MVVM :

```text
Vue          -> MainWindow.xaml
Vue-modèle   -> MainVueModele.cs
Modèle       -> classes métier
Accès BD     -> AccesBD.cs
Commandes    -> CommandeRelais.cs
```

---

## 7. Fonctionnalités principales

### Tableau de bord

- Nombre total d’animaux
- Nombre d’animaux au refuge
- Nombre d’animaux adoptés
- Nombre d’animaux en famille d’accueil
- Nombre de contacts
- Nombre d’adoptions
- Nombre de familles d’accueil
- Nombre de vaccinations

### Gestion des animaux

- Liste et recherche
- Ajout d’un animal avec son entrée au refuge
- Affichage de l’état actuel
- Fiche détaillée avec couleurs, compatibilités, vaccinations et historique

### Gestion des contacts

- Liste et recherche
- Ajout et modification
- Fiche détaillée avec rôles, adoptions, familles d’accueil, entrées et sorties

### Adoptions et familles d’accueil

- Ajouter une demande d’adoption
- Modifier son statut
- Créer automatiquement une sortie lors d’une adoption acceptée
- Ajouter et clôturer une famille d’accueil

### Vaccins et données associées

- Ajouter un vaccin
- Ajouter une vaccination
- Gérer couleurs, rôles et compatibilités

### Diagnostic

- Vérification de certaines incohérences possibles dans la base de données
- Exemples : plusieurs familles d’accueil actives, adoption acceptée en double, sortie sans entrée cohérente

---

## 8. Procédures stockées et fonctions SQL

L’application s’appuie sur des procédures stockées PostgreSQL pour les actions de modification, par exemple :

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

Elle utilise aussi des fonctions d’affichage et de diagnostic, notamment :

```text
get_animaux
get_contacts
get_adoptions
get_familles_accueil
get_vaccins
get_vaccinations
get_couleurs
get_roles
get_compatibilites
get_animaux_presents
get_tableau_bord
verifier_coherence_refuge
```

---

## 9. Points techniques

- Requêtes paramétrées avec Npgsql
- Séparation du modèle, de la vue, de la vue-modèle et de l’accès aux données
- Logique métier importante gérée par PostgreSQL via procédures, fonctions et contraintes
- DataGrid principalement en lecture seule ; modifications via formulaires et commandes
- Configuration sensible externalisée via variables d’environnement

---

## 10. Limites et améliorations possibles

- Séparer le grand `MainVueModele.cs` en plusieurs ViewModels spécialisés
- Ajouter davantage de tests unitaires
- Ajouter certaines suppressions avancées dans l’interface WPF
- Améliorer le design de l’interface
- Ajouter un export CSV
- Ajouter des messages d’erreur plus détaillés

---

## 11. Lancement

1. Configurer PostgreSQL et les variables d’environnement.
2. Ouvrir le projet dans Visual Studio.
3. Lancer l’application avec `F5`.

---

## Résumé

Ce projet combine **C#/.NET, WPF, MVVM, PostgreSQL, Npgsql, SQL et PL/pgSQL** dans une application métier complète de gestion de refuge animalier. Il met notamment en pratique l’architecture logicielle, la modélisation de données, l’accès à une base relationnelle, les procédures stockées et la conception d’une interface graphique.
