# Lost Echoes

***Lost Echoes*** est un jeu narratif en réalité virtuelle, avec des aspects de réflexion, énigmes et *escape game*.

Il s'agit d'un projet réalisé par 5 étudiants d'[IMT Atlantique](https://www.imt-atlantique.fr/fr), dans le cadre du projet commun de la TAF **IHM** (interaction humain-machine et réalité virtuelle).

---

## Table des matières

- [Lost Echoes](#lost-echoes)
  - [Table des matières](#table-des-matières)
  - [Présentation générale](#présentation-générale)
    - [Principe du jeu](#principe-du-jeu)
    - [Public cible](#public-cible)
    - [Avertissement important](#avertissement-important)
  - [Pré-requis](#pré-requis)
    - [Casque VR](#casque-vr)
    - [Ordinateur avec Unity](#ordinateur-avec-unity)
  - [Installation et déploiement](#installation-et-déploiement)
    - [1. Télécharger les fichiers](#1-télécharger-les-fichiers)
    - [2. Ouvrir le projet avec Unity](#2-ouvrir-le-projet-avec-unity)
    - [3. Déployer et téléverser l'application](#3-déployer-et-téléverser-lapplication)
    - [4. Lancer le jeu dans le casque](#4-lancer-le-jeu-dans-le-casque)
  - [Utilisation de l'application](#utilisation-de-lapplication)
    - [Configuration du casque](#configuration-du-casque)
    - [Début de la partie](#début-de-la-partie)
    - [Commandes](#commandes)
    - [Progression](#progression)
    - [Performances](#performances)
    - [Audio et sons](#audio-et-sons)
    - [Accessibilité](#accessibilité)
    - [Fin du jeu](#fin-du-jeu)
  - [Crédits](#crédits)
    - [Équipe](#équipe)
    - [Écriture de l'histoire (*storyboarding* et *screenplay*)](#écriture-de-lhistoire-storyboarding-et-screenplay)
    - [Conception de l'environnement et décoration visuelle](#conception-de-lenvironnement-et-décoration-visuelle)
    - [Coordination de la jouabilité et programmation pour la VR](#coordination-de-la-jouabilité-et-programmation-pour-la-vr)
    - [Conception et développement de l'architecture logicielle](#conception-et-développement-de-larchitecture-logicielle)
    - [Conception et implémentation des casse-têtes](#conception-et-implémentation-des-casse-têtes)
    - [Voix-off](#voix-off)
    - [Tests et débogage](#tests-et-débogage)
    - [Remerciement spéciaux](#remerciement-spéciaux)

## Présentation générale

### Principe du jeu

L'enjeu social ici traité est celui du **harcèlement scolaire**. Le jeu retrace l'expérience personnelle d'Alex, une jeune personne victime de violences de la part de ses collègues de classe. Le joueur incarne cette personne et évolue dans sa chambre à coucher.

Recherchez des informations, trouvez des objets importants, et résolvez des énigmes pour arriver au bout de cette courte expérience, qui se veut aussi ludique que touchante, mais également instructive !

### Public cible

Cette expérience s'adresse à tous types de joueurs ! L'application a pour ambition d'être accessible et donc aussi à la portée d'élèves de collège ou de lycée.

Le jeu est proposé en **langue française** uniquement.

### Avertissement important

Attention toutefois, si vous êtes sensible au **mal du simulateur** (*cybersickness*), il n'est pas recommandé de jouer longuement. Si vous vous sentez malade pendant la partie, il vous est vivement conseillé de prendre des pauses régulières (il est possible de quitter la partie et de la reprendre plus tard, avec votre progression sauvegardée).

## Pré-requis

### Casque VR

Pour jouer à *Lost Echoes*, il vous est nécessaire disposer d'un **casque de réalité virtuelle**, idéalement de type *standalone* (avec de l'espace de stockage et des capteurs de suivi/accéléromètres embarqués). Il vous faudra également jouer avec des **manettes** de VR, qui accompagnent la plupart des casques vendus sur le marché (le jeu ne se joue pas aux seules mains).

> Le jeu a été initialement développé, testé et pensé pour le ***Meta Quest 2*** (anciennement *Oculus Quest 2*), s'agissant du modèle mis à disposition par l'école et fourni sur site dans le cadre des activités scolaires.

Dans le cas d'un casque classique (avec stockage embarqué, tel que le *Quest 2*), vous devez avoir au moins **300 Mo** d'espace libre disponible.

### Ordinateur avec Unity

Ce logiciel étant au stade expérimental dans le cadre d'un projet scolaire, vous aurez besoin du moteur de conception de jeux **Unity** (en version 2021.3.9 de préférence, avec composants les permettant le lancement sous Android).

Il vous faut un **ordinateur** (Windows, Mac ou Linux) suffisamment moderne (la plupart devraient convenir) pour ouvrir le projet sous Unity et le téléverser vers le casque. De plus, un fichier `.apk` sera produit et ajouté à votre machine à l'issue du build. Vous devriez ensuite pouvoir le déployer sur d'autres dispositifs compatibles Android.

## Installation et déploiement

### 1. Télécharger les fichiers

Pour télécharger les données de l'application vers votre machine, vous pourrez soit **cloner ce dépôt** via la commande `git clone`, soit **télécharger l'archive** compressée du code source.

Stockez ensuite le dossier racine à un emplacement de votre choix (et dont vous disposez des droits d'accès).

### 2. Ouvrir le projet avec Unity

Dans Unity Hub, sélectionnez **Add** (ou Add project from disk), puis naviguez vers l'emplacement des fichiers téléchargés et sélectionnez le **dossier** racine.

Ouvrez ensuite le projet avec l'éditeur.

### 3. Déployer et téléverser l'application

Une fois dans l'éditeur Unity, sélectionnez **File > Build Settings**, ou utilisez la combinaison de touches `Ctrl` + `Maj` + `B`.

Assurez-vous ensuite que **Android** est sélectionné dans la boîte *Platform*. Si ce n'est pas le cas, sélectionnez Android puis cliquez sur **Switch Platform**.

Branchez ensuite votre casque à l'ordinateur via une connexion USB.

Vous pouvez à présent sélectionner l'option **Build and Run** en bas de la fenêtre de dialoguue. Vous aurez sûrement à choisir un emplacement sur votre machine, dans lequel stocker le fichier `.apk`, et un nom à lui donner.

> Il est souvent conseillé de créer un sous-dossier `Builds` dans le répertoire du projet, et d'y effectuer le déploiement (vous pouvez choisir le nom que vous voulez, par exemple `LostEchoes.apk`).

Le déploiement en lui-même se lancera alors. Attention, cette opération est susceptible de durer plusieurs minutes !

Une fois ceci fait, **il ne sera plus nécessaire de refaire le déploiement du jeu pour ce casque**. L'application sera stockée directement sur disque et pourra être relancée sans ordinateur, à moins que vous ne la désinstalliez/supprimiez du casque (en admettant qu'il y ait du stockage embarqué).

**Attention :** Il est (fortement) possible qu'Unity vous indique que le casque n'est pas branché alors qu'il l'est pourtant, la première fois. Si vous avez vérifié la connexion et que ce n'est pas un problème physique/externe, cela sera souvent dû au **débogage USB** n'ayant pas été autorisé depuis cet ordinateur. Pour y remédier, mettez simplement le casque puis sélectionnez le bouton autorisant le débogage USB lorsque l'invite apparaît. Vous devrez peut-être faire différemment selon le modèle ou essayer de débrancher/rebrancher le casque pour forcer le menu à apparaître. Si vous maîtrisez l'ordinateur d'un point de vue sécurité, vous pouvez choisir *Toujours autoriser depuis cet ordinateur* pour réduire le nombre de fois où vous aurez besoin d'autoriser le débogage USB. Vous devrez répéter l'opération pour chacun de vos casques si vous en avez plusieurs.

### 4. Lancer le jeu dans le casque

> *Cette section supposera que votre casque est un **Meta Quest 2**. La manipulation exacte pourra varier selon les modèles et les marques.*

Si vous portiez déjà le casque lors du déploiement, ou si vous l'avez mis peu de temps après (quelques secondes), le jeu s'est normalement déjà lancé et le menu s'affiche correctement.

*Quelques fois (cela arrive notamment après le déploiement initial si vous ne mettez pas le casque à temps), le dispositif pensera que le jeu tourne mais rien ne s'affichera. Relancez le jeu en le quittant (bouton Meta/Oculus sur la manette droite), puis en suivant les instructions données au paragraphe suivant.*

**Dans les autres cas**, y compris pour lancer le jeu plus tard, il vous faudra en général aller lancer l'application manuellement depuis la liste. Pour ce faire, invoquez le menu (bouton Meta/Oculus sur la manette droite), puis choisissez le menu des applications (icône ronde avec 9 carrés, sur la droite de la barre d'options). Si vous ne voyez pas le jeu dans la liste, ce qui sera probable, vous devrez afficher les **sources inconnues** en sélectionnant la barre de recherche puis en passant sur la catégorie *Sources inconnues*.

Cliquez ensuite sur l'application intitulée `LostEchoes` dans la liste pour la lancer. Vous devriez alors voir le menu principal du jeu, avec le titre écrit en haut.

## Utilisation de l'application

### Configuration du casque

Il est fortement recommandé de jouer avec une **limite stationnaire** configurée dans le casque (c'est-à-dire sans bouger de là où vous vous trouvez). Il est recommandé de jouer **assis**. Ce sera plus confortable visuellement mais également physiquement, votre session pouvant prendre plusieurs minutes.

> Si toutefois vous observez des problèmes d'affichage une fois en jeu en position assise, vous pourrez essayer de vous tenir *debout* (mais toujours en limite stationnaire) si cela paraît plus naturel au niveau de la hauteur. Cela est dû au réglage du **niveau du sol dans le casque**, que vous pouvez également reparamétrer, ce qui est conseillé pour rester assis (le mettre *moins bas*).

Durant la partie, pour éviter au maximum les soucis d'affichage, essayez de *tourner votre tête **le moins possible***. Vous pouvez le faire pour observer vos alentours, bien entendu, mais si vous souhaitez *changer de direction de façon pérenne*, il vaudrait mieux tourner à l'aide du **stick droit** (cf. [commandes](#commandes)).

Assurez-vous aussi de disposer de **suffisamment d'espace dégagé** autour de vous. Placez donc une chaise en plein milieu d'une pièce ou d'un espace vide, loin de tables et autres éléments ou personnes. *Faites toujours preuve de prudence quant à ce qui vous entoure dans le monde réel lorsque vous jouez en VR !*

### Début de la partie

Depuis le menu principal, sélectionnez **Nouvelle partie** pour entrer dans le jeu en créant une nouvelle sauvegarde (vous vous retrouverez alors dans la chambre d'Alex). Vous pouvez également consulter les commandes en jeu grâce au bouton associé depuis le menu.

### Commandes

Une fois dans le jeu, utilisez le **stick analogique gauche** pour vous déplacer et le **stick droit** (ou simplement votre tête) pour vous tourner. Utilisez le **bouton d'options** sur la manette gauche pour afficher le *menu de pause*, vous permettant de consulter les commandes, contrôler la sourdine ou encore quitter la partie.

Les sélections dans les menus et autres parties du jeu faisant intervenir des pointeurs laser s'effectuent classiquement avec les **gâchettes**. Par ailleurs, la sélection ou saisie d'objets en jeu (avec les mains affichées) s'effectue avec le **bouton de saisie** présent sur le côté de la tige de la plupart des manettes de VR.

### Progression

Ceci est un jeu de réflexion, avec de la recherche et des éngimes. N'hésitez donc pas à regarder partout !

Les objets importants de l'histoire s'accompagneront d'un **contour** très visible. Le rouge/orange indique un objet nouveau, nécessaire pour avancer.

À partir du moment où vous avez commencé à progresser dans l'histoire (vous avez ouvert le téléphone posé sur le bureau au moins une fois), vos données seront sauvegardées et accessibles à tout moment depuis le menu principal via le bouton **Continuer**, y compris si vous quittez le jeu entièrement.

### Performances

Le jeu étant assez gourmant au vu du nombre de modèles (peu de temps ayant pu être dédié à l'optimisation), il sera susceptible d'avoir une latence en fréquence de rafraîchissement (nombre d'images par seconde) assez gênante, surtout sur les anciens modèles de casques VR tout-en-un.

Le jeu devrait rester globalement jouable sur *Meta Quest 2*, mais vous pouvez y remédier légèrement avec les quelques pistes suivantes :

- Dans Unity, allez **Edit > Project settings...** ; depuis cette fenêtre, si vous naviguez vers le menu **Quality**, vous devrez voir différentes configurations possibles. Choisissez notamment High et Medium (cette dernière étant utilisée par Android, pour usage sur le casque), et désactivez en priorité l'**anti-crénelage** en passant **Anti Aliasing** à **Disabled** depuis le menu déroulant.
- Toujours dans Unity, si votre machine le permet, vous pouvez également double cliquer sur **Bedroom** (fichier situé dans **Scenes** dans l'explorateur des *Assets*, en bas de la fenêtre de l'éditeur). Depuis cet endroit, dépliez `XR Origin` dans la hiérarchie située à gauche, puis `Camera Offset` et enfin cliquez sur `Main Camera`. Vous devriez pouvoir essayer de désactiver l'option Anti-aliasing (à passer le mode sur *No Anti-aliasing*) sans le composant **Post-process Layer**, mais également de décocher la case *Corner Outlines* dans le composant **Outline Effect (Script)**. Sauvegardez ensuite avec `Ctrl` + `S`, puis refaites un déploiement avec *Build and Run* (`Ctrl` + `B`).

Si les performances demeurent un problème, vous devrez peut-être utiliser une technique de VR utilisant un casque en mode attaché où le jeu tourne votre l'ordinateur.

### Audio et sons

Le jeu comprend divers effets sonores, musiques et voix. En utilisant le menu principal (ou le menu en jeu), il est possible de mettre le son en **sourdine**.

> Il est toutefois recommandé de jouer *avec* le son. Sur **Oculus Quest 2**, vous devez régler le son du casque sur au moins 4 ou 5 points, pour pouvoir entendre les divers audios.

### Accessibilité

Il y a beaucoup de déplacements à effectuer, surtout en début de jeu. Prenez donc garde à ce que cela ne vous gêne pas ou ne vous rende pas malade.

> N'hésitez pas à quitter le jeu à tout moment et à reprendre votre partie plus tard si vous sentez une fatigue, des nausées ou vertiges. Il est toujours bon de faire des pauses régulièrement en jouant !

Par ailleurs, si vous bloquez sur une énigme et que vous ne souhaitez pas rester coincé, le téléphone fournit parfois quelques pistes ! Lors d'un casse-tête dédié, vous pouvez également appeler le menu pour le réinitialiser voire le passer pour avancer malgré tout.

### Fin du jeu

*Lost Echoes* est une expérience prévue pour durer **5 à 10 minutes**. Cela pourra varier selon l'expérience du joueur et des pauses qu'il ou elle prend, mais également la difficulté à réaliser certaines tâches ou encore à résoudre certains casse-têtes si vous ne les passez pas.

Le jeu se termine sur un message de prévention vous rappelant la bonne démarche à adopter en cas de harcèlement scolaire ou autre. N'hésitez pas à en parler si vous pensez observer ces phénomènes autour de vous et ne le prenez pas à la légère. Cherchez à aider au mieux les victimes de harcèlement qui ne se rendent pas toujours compte de la gravité de ce qui leur arrive. Mieux vaut un faux positif qu'un faux négatif !

Merci beaucoup d'avoir joué à notre jeu !

## Crédits

Ce logiciel a été réalisé dans le cadre d'un projet d'école d'ingénieurs, par 5 étudiants d'IMT Atlantique Brest.

### Équipe

- Lucas BUTERY
- Anass LASRY
- Mohamed Taher MAALEJ
- Maxime PRADILLON
- Selma SABRI

### Écriture de l'histoire (*storyboarding* et *screenplay*)

- Selma SABRI

### Conception de l'environnement et décoration visuelle

- Lucas BUTERY
- Mohamed Taher MAALEJ
- Selma SABRI

### Coordination de la jouabilité et programmation pour la VR

- Lucas BUTERY
- Maxime PRADILLON

### Conception et développement de l'architecture logicielle

- Anass LASRY
- Maxime PRADILLON

### Conception et implémentation des casse-têtes

- Mohamed Taher MAALEJ
- Maxime PRADILLON

### Voix-off

- Selma SABRI

### Tests et débogage

- Maxime PRADILLON

### Remerciement spéciaux

- Cédric FLEURY, Charlotte HOAREAU, Étienne PEILLARD (accompagnants durant le projet et encardants pour l'enseignement)
- Denis SANS (graphisme de quelques textures)
- Sacha, Imane et Mathias de la classe IHM, Nao et Julien (testeurs du jeu)
- Créateurs respectifs des ressources utilisés depuis *Sketchfab* et l'*Asset Store* d'Unity
