# Refuge Animaux — Projet de développement SGDB

Projet réalisé par **Harlem Alofa** dans le cadre du cours de projet de développement / système de gestion de base de données.

Ce projet a pour objectif de gérer les informations principales d’un refuge pour animaux : animaux, contacts, adoptions, familles d’accueil, vaccinations, couleurs, rôles, compatibilités, entrées et sorties.

---

## 1. Technologies utilisées

- **C#**
- **.NET**
- **Application Console**
- **WPF**
- **MVVM**
- **PostgreSQL**
- **Npgsql**
- **SQL / PLpgSQL**
- **GitHub**

---

## 2. Organisation générale du projet

Le projet contient deux parties principales :

```text
RefugeAnimaux              -> application Console
RefugeAnimauxWPF           -> application graphique WPF
SQL                        -> scripts SQL / procédures stockées
```

La première partie est une application Console organisée en couches.  
La deuxième partie est une application WPF utilisant une organisation MVVM.

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

2. Exécuter le script de création des tables si la base n’existe pas encore.

3. Exécuter le fichier des procédures stockées :

```text
procedures_stockees_HarlemAlofa.sql
```

ou, selon le nom conservé dans le projet :

```text
procedures_stockees_wpf_FINAL_MIS_A_JOUR.sql
```

Ce fichier contient les fonctions et procédures nécessaires à l’application WPF.

---

## 5. Connexion à PostgreSQL

La connexion à la base de données est définie dans la classe :

```text
RefugeAnimauxWPF/coucheAccesBD/AccesBD.cs
```

Exemple de chaîne de connexion :

```csharp
private readonly string chaineConnexion =
    "Host=localhost;" +
    "Database=refuge_animaux;" +
    "Username=postgres;" +
    "Password=Jesus2001";
```

Le mot de passe doit être adapté selon la configuration PostgreSQL de la machine utilisée.

---

## 6. Partie Console

La partie Console sert à manipuler les données du refuge via un menu textuel.

Elle contient une organisation en couches :

```text
couche métier
couche accès aux données
couche présentation
```

### Fonctionnalités principales Console

- Ajouter un animal
- Ajouter un contact
- Consulter les animaux
- Consulter les contacts
- Gérer les entrées et sorties
- Gérer les adoptions
- Gérer les familles d’accueil
- Afficher des statistiques
- Afficher un tableau de bord
- Vérifier la cohérence des données
- Supprimer intelligemment certaines données si elles ne sont pas liées

---

## 7. Partie WPF

La partie WPF est une application graphique permettant de consulter et manipuler les données du refuge plus facilement.

Elle utilise le principe **MVVM** :

```text
Vue          -> MainWindow.xaml
Vue-modèle   -> MainVueModele.cs
Modèle       -> classes métier
Accès BD     -> AccesBD.cs
Commandes    -> CommandeRelais.cs
```

---

## 8. Fonctionnalités principales WPF

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

- Liste des animaux
- Recherche d’un animal
- Ajout d’un animal avec son entrée au refuge
- Affichage de l’état actuel de l’animal
- Fiche complète animal

La fiche animal affiche :

```text
identité
date de naissance
stérilisation
description
particularités
couleurs
compatibilités
vaccinations
historique des entrées et sorties
adoptions liées
familles d’accueil liées
```

### Gestion des contacts

- Liste des contacts
- Recherche d’un contact
- Ajout d’un contact
- Modification d’un contact
- Fiche complète contact

La fiche contact affiche :

```text
identité
rôles
adoptions liées
familles d’accueil liées
entrées d’animaux liées
sorties d’animaux liées
```

### Gestion des adoptions

- Ajouter une demande d’adoption
- Modifier le statut d’une adoption
- Lorsqu’une adoption est acceptée, une sortie est créée pour l’animal

### Gestion des familles d’accueil

- Ajouter une famille d’accueil
- Clôturer une famille d’accueil
- Lors de la clôture, une nouvelle entrée au refuge est créée si nécessaire

### Gestion des vaccins

- Ajouter un vaccin
- Ajouter une vaccination à un animal
- Consulter les vaccinations

### Gestion des couleurs, rôles et compatibilités

- Ajouter une couleur
- Associer une couleur à un animal
- Ajouter un rôle
- Associer un rôle à un contact
- Associer une compatibilité à un animal

### Diagnostic

- Vérification de certaines incohérences possibles dans la base de données
- Exemple : animal avec plusieurs familles d’accueil actives, adoption acceptée en double, sortie sans entrée, etc.

---

## 9. Procédures stockées et fonctions SQL

L’application WPF utilise des procédures stockées PostgreSQL pour les actions de modification.

Exemples :

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

Elle utilise aussi plusieurs fonctions d’affichage :

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

Fonctions ajoutées pour les fiches détaillées :

```text
get_fiche_animal_couleurs
get_fiche_animal_compatibilites
get_fiche_animal_vaccinations
get_fiche_animal_historique
get_fiche_animal_adoptions
get_fiche_animal_familles

get_fiche_contact_roles
get_fiche_contact_adoptions
get_fiche_contact_familles
get_fiche_contact_entrees
get_fiche_contact_sorties
```

---

## 10. Exemples de tests

### Ajouter un contact

```text
Identifiant : 9911
Nom : Martin
Prénom : Clara
Rue : Rue des Lilas 12
CP : 1000
Localité : Bruxelles
Registre national : 99110100011
GSM : 0491000011
Téléphone : 021110011
Email : clara.martin@test.be
```

### Ajouter un animal

```text
Identifiant : 26060100911
Nom : Moka
Type : chat
Sexe : M
Date naissance : 01/01/2024
Stérilisé : non
Raison entrée : abandon
Contact entrée : 9911
Description : Chat calme ajouté depuis la WPF.
Particularités : Aime les endroits calmes.
```

### Ajouter une adoption

```text
Animal : 26060100911
Contact : 9911
Statut : demande
```

Puis modifier le statut en :

```text
acceptee
```

### Ajouter une famille d’accueil

```text
Animal : 26060100912
Contact : 9911
Date début : aujourd’hui
```

Puis clôturer avec une date de fin après la date de début.

---

## 11. Points importants

- Les DataGrid sont principalement en lecture seule.
- Les modifications se font via les formulaires et les boutons.
- Les règles métier importantes sont contrôlées par la base de données.
- Les procédures stockées contiennent les actions SQL principales.
- Certaines valeurs sont contrôlées par des contraintes dans la base de données, par exemple les rôles autorisés.

---

## 12. Limites connues

- La WPF utilise un grand `MainVueModele.cs` centralisé. Une amélioration future serait de diviser ce fichier en plusieurs ViewModels.
- Le mot de passe PostgreSQL est écrit dans la chaîne de connexion et doit être adapté selon l’ordinateur utilisé.
- Certaines suppressions avancées ne sont pas encore disponibles dans la WPF.
- L’interface pourrait encore être améliorée visuellement.

---

## 13. Améliorations possibles

- Ajouter des suppressions intelligentes dans la WPF
- Ajouter une page de paramètres pour la connexion PostgreSQL
- Améliorer le design de l’interface
- Séparer les ViewModels par fonctionnalité
- Ajouter un export CSV
- Ajouter des messages d’erreur plus détaillés pour l’utilisateur
- Ajouter des tests unitaires

---

## 14. Lancement de l’application

### Lancer la partie Console

Ouvrir la solution dans Visual Studio, sélectionner le projet Console et lancer avec :

```text
F5
```

### Lancer la partie WPF

Ouvrir le projet WPF dans Visual Studio, vérifier la chaîne de connexion PostgreSQL, puis lancer avec :

```text
F5
```

---

## 15. Résumé

Ce projet permet de gérer les données principales d’un refuge pour animaux avec deux interfaces :

```text
une application Console
une application WPF
```

La partie WPF s’appuie sur PostgreSQL et des procédures stockées pour réaliser les principales opérations métier. Les fiches animal et contact permettent de retrouver plus facilement les informations importantes liées à chaque entité.
