# Projet-A1S2
## Jeu de la vie + Etude de transmission COVID-19

Rapport Jeu de la Vie

### I.	GESTION DES FONCTIONNALITES PROPOSEES

Le programme rendu est un Jeu de la Vie fonctionnel comprenant les trois étapes demandés dans le sujet. La page d’accueil qui s’affiche à l’exécution du programme explique qui a conçu ce jeu et quelles versions l’utilisateur trouvera ici. 
Le menu de sélection demandé apparaît ensuite en offrant ses six propositions pour les différents modes de jeu (une seule population, deux populations, modélisation du COVID-19).
Le programme comporte également un menu de paramétrage afin de personnaliser l’expérience et de faire varier les paramètres de générations ou bien les taux de contamination dans le cas du COVID par exemple.

En ce qui concerne le calcul de l’état futur de la grille, il est réalisé dans les cas sans COVID, par la méthode PrédictionDeProchaineGen. Une double boucle for permet alors de parcourir chaque case de la matrice. Pour chaque case, on teste via la méthode TestVoisinageRang, son voisinage de rang un, c’est-à-dire que l’on teste le nombre de cellule vivante sur les huit cases qui entoure la case testée (en prenant en compte l’aspect torique de la grille grâce aux modulos). 
La valeur ainsi renvoyée permet d’appliquer les règles qui déterminent si une cellule doit naître, vivre ou mourir. Ces règles étant elles codées via des conditions imbriquées dans la méthode PrédictionDeProchaineGen.
Dans le cas ou on a deux populations, la méthode TestVoisinageRang, renvoie le nombre de cellule vivante de la population 1 au rang 1, le nombre de cellule vivante de la population 2 toujours au rang 1, et ces valeurs pour le rang 2. Les quatre valeurs sont distinctes pour permettre l’application des règles spécifiques à l’étape n°2.
Une fois ce comptage effectué, la méthode définie quelles cellules vont mourir et lesquelles vont naître. 
Le problème est d’appliquer ces changements sans pour autant ajouter immédiatement ces cellules ce qui fausserait alors le voisinage des cellules alentours.
Pour faire face à cette difficulté, la méthode ne change pas directement les cellules en mortes ou vivantes, il définie deux états pour chaque population : va vivre et va mourir. 
Les cellules qui vont mourir sont prises en compte dans le calcul des cellules déjà vivante puisqu’elles le sont pour cette génération et ne mourront qu’à la suivante. Tandis que les cellules qui vont vivre ne sont-elles pas prises en compte.
Une autre méthode : PassageDeGénération, permet quant à elle d’appliquer définitivement les changements en changeant l’état des cellules qui doivent naître en cellules vivantes et celles qui doivent mourir en cellules mortes.
Dans le cas du COVID-19, nous avons choisi de ne pas appliquer les règles de vie ou de mort de notre population, en effet nous considérons ainsi que la seule chose qui puisse faire mourir les cellules est le COVID-19, cela permet de simuler l’impact réel de la maladie.
Toutes les cellules vivantes le sont donc lors de la génération initiale de la matrice. Aucune cellule ne naîtra au cours de l’étude. On a donc une population stable et en vase clos pour commencer notre étude.
Ici, le passage de génération est particulier : il ne s’agit plus de détecter les cas de sur/sous-population et les cas où une cellule doit naître.
Ce qui nous intéresse ce n’est pas de savoir le nombre de cellules vivantes aux alentours. Il faut connaître le nombre de cellules saines et non immunisées et leurs coordonnées afin de potentiellement les infecter. 
Le R0 du Sars-Cov-2 est de 2,5. Nous avons choisi de ne pas offrir la possibilité de modifier ce paramètre, car nous étudions ici un virus donné. Son taux de reproduction est donc fixe (tant que le virus ne mute pas). Les taux de passage d’un stade à l’autre quant à eux dépendent de la population et de le résistance de son système immunitaire. Ces taux de passage mais aussi ceux de confinement sont par conséquent paramétrable dans le menu des paramètres. Ils ne s’affichent cependant que lorsque le choix du jeu est l’un des deux « Jeu de la Vie avec COVID-19 ».
Pour le passage de génération sur le COVID-19, nous avons donc créer trois nouvelles méthodes qui se rapprochent dans leur structure et leur fonctionnement des méthodes pour les jeux classiques. DetectionPouInfectionCovid correspond ainsi à TestVoisinageRang, mais là ou la dernière se contente de renvoyer le nombre de cellule vivante aux alentours, la première renvoie le nombre de cellules saines et leurs coordonnées, ce qui permet de potentiellement les infecter.
ContaminationCovid correspond quant à elle à PredictionDeProchaineGen, mais comporte par différence avec cette dernière un système d’infection de proche en proche sur la base du R0.
Enfin, PassageDeGenCovid s’apparente à PassageDeGénération, mais elle est plus complexe. Elle définie en fonction des taux d’aggravation si une cellule doit passer à un stade plus grave ou bien si on doit lui enlever une génération à attendre avant de devenir saine et immunisée. Cette attente est contrôlée via une matrice chronologie de taille identique à la matrice initiale. Elle enregistre le nombre de génération restantes à une cellule avant d’être guérie.

Si nous avons choisi de scinder cette évolution en plusieurs méthodes, c’est pour des raisons de lisibilité du code, mais également pour faciliter les tests et le débogage.




### II.	UTILITES ET FONCTIONNEMENT DES METHODES

```c#
static int[] TestVoisinageRang(int[,] grille, int posX, int posY) :
```
Il s’agit comme expliqué plus haut de la méthode qui compte le nombre de cellule vivantes de la population 1 et 2 au rang 1 et 2. Elle renvoie un tableau d’entier de taille 4, qui correspond aux quatre valeurs qu’elle teste.
Pour effectuer ces tests, on initialise une double boucle à la position en X et en Y moins 1 ou 2 en fonction du rang souhaité, par rapport à la cellule testée.
Quatre compteurs se charge d’enregistrer les nombres souhaités.

```c#
static void PredictionDeProchaineGen(int[,] grille, int choixdujeu) :
```
Cette méthode applique les règlements pour les modes de jeu à une ou deux populations.
Les règlements sont appliqués à l’aide de la méthode susmentionnée et de conditions imbriquées sur le nombre de cellules vivantes d’une population aux alentours.
Cette méthode ne renvoie rien, car elle fait évoluer la matrice grille avec des valeurs signifiant : va vivre ou va mourir.

```c#
static int[] DetectionPourInfectionCovid(int[,] grille, int posX, int posY) :
```
Cette méthode détecte les cellules saines et non immunisées et enregistre leur coordonnées, ainsi que leur nombre. Son fonctionnement est analogue à la première méthode, mais son fonctionnement est différent en ce sens qu’il enregistre également les positions en X et en Y de ces cellules.
La méthode renvoie un tableau de taille 17, correspondant aux coordonnées en X et en Y de huit cellules (nombre maximal de cellules saines et infectables possibles au rang 1) plus leur nombre.

```c#
static void ContaminationCovid(int[,] grille, int choixdujeu, double[] parametrage) : 
```
Cette méthode permet la propagation du virus via le R0. Elle s’appuie pour fonctionner, sur la méthode précédente. Elle se sert d’un générateur aléatoire pour déterminer quelles cellules infecter au-delà de trois cellules potentiellement infectables. 
Elle ne renvoie rien et ce contente de modifier la matrice grille avec une valeur va être infectée.

```c#
static int PassageDeGenCovid(int[,] grille, int[,] chronologie, double[] parametrage) : 
```
Cette méthode applique les changements initiés par la méthode précédente, incrémente de 5 la matrice chronologie des cellules infectée si elles passent à un nouveau stade, la décrémente si le stade reste inchangé. Elle s’occupe également de déterminer via un générateur aléatoire et en fonction des taux définis, si le stade d’une cellule infectée doit évoluer vers un stade plus grave. Elle change les matrice grille et chronologie.
Elle renvoie un entier correspondant au nombre de morts du virus.

```c#
static void PassageDeGeneration(int[,] grille) : 
```
Cette méthode applique les changements temporaires définies par la méthode PrédictionDeProchaineGen.
Elle ne renvoie rien et modifie simplement la matrice grille.

```c#
static bool[] NonNullNonVide(int[,] grille) :
```
Cette méthode consiste en un test de conformité sur la matrice. Elle permet de s’assurer que l’affichage console fonctionnera et explique la source de l’erreur si celle-ci provient de la matrice. Nous avons choisi de conserver cette méthode de débogage dans le code final.

```c#
static int[,] GenererMatriceAleatoire(double[] parametrage, int choixdujeu) :
```
Cette méthode s’occupe de la génération initiale de la matrice grille. C’est elle qui assure son remplissage de manière aléatoire. Cet aléa est cependant pondéré par le taux de remplissage initial défini.
Elle utilise pour cela un générateur aléatoire.
Elle renvoie la matrice grille générée.

```c#
static int Menu() : 
```
Cette méthode affiche le menu initial qui permet de choisir le jeu. Elle renvoie un entier correspondant au numéro de la version choisie. Cet entier est primordial pour le fonctionnement du programme car il détermine l’entrée ou non dans de nombreuses conditions.

```c#
static double[] Paramètres(int choixdujeu) : 
```
Cette méthode permet d’initialiser les paramètres avec les valeurs souhaitées par l’utilisateur. Ces valeurs interviendront dans de nombreuses autres méthodes du programme. Pour ne pas ennuyer l’utilisateur, les valeurs ont toutes un réglage par défaut.
Elle renvoie un tableau de double de taille 13, où chaque case du tableau correspond à un paramètre.

```c#
static int[] TaillePopulation(int[,] grille, int choixdujeu) : 
```
C’est la méthode qui est chargé de toutes les opérations de comptage sur les populations présentent dans la matrice grille, à l’exception de celles des morts car cela lui serait impossible puisque les morts ont lieu avant et qu’ils sont indistingables des cellules jamais nées.
Elle renvoie un tableau d’entier de taille 8, où chaque case correspond à un compteur nécessaire pour une version du programme.

```c#
static void LeJeuDeLaVie(int[,] grille, int choixdujeu, double[] parametrage) : 
```
La méthode principale : elle permet au jeu de se dérouler, générations après générations au moyen d’une boucle. Elle se charge de l’appel des méthodes qui permettent de passer les générations en fonction du mode de jeu choisi, ainsi que des menus. Elle s’occupe également d’appeler ou de réaliser les affichages nécessaires.
Elle ne renvoie rien.

```c#
static void AffichageConsole(int[,] grille) : 
```
Affiche la matrice grille en fonction de la symbologie propre à chaque mode. Elle ne renvoie rien. 

### III.	ETUDE DE LA PROPAGATION DU SARS-COV-2 (COVID-19)

Cette étude n’appelle pas à des conclusions ferme et définitive. Elle est imparfaite au vu de la simplicité des paramètres traités par le programme qui l’a permis. 

Pour cette étude, on considère que tous les cellules infectée le sont au stade 0. On considère que le taux de confinement efficace des personnes saines et non immunisée est de 75%, que le taux de confinement efficace des malades est de 90%.

Dans le cas sans confinement, après réalisation de plusieurs tests, on obtient une moyenne de mort de 61%. Cette mortalité est très élevée pour le coronavirus. Cela montre que les taux par défaut de passage d’un stade à un autre son surestimés. 

Dans le cas avec confinement, on obtient une mortalité bien moindre de 37% en moyenne. De plus outre cette baisse de plus de 20% de la mortalité on remarque qu’au niveau géographique le virus de diffuse moins et est bien mieux contenu. On remarque également que le confinement est utile s’il dure au moins 15 générations puisqu’il permet d’arriver dans la plupart des cas à une génération 20 avec 1 ou 2 malades sur les environs 210 de départ. Cela représente alors un pourcentage de malade compris entre 0,5 et 1% de la population totale ce que l’on peut estimer satisfaisant.

On peut donc en conclure que le confinement à un impact réel sur la propagation du virus en la diminuant drastiquement même si la mortalité reste élevée. On peut cependant remarquer si l’on se place à l’échelle nationale qu’un pour cent de personnes contaminé représente plus de 600 000 personne. Donc le confinement doit être le plus stricte possible (ce qui renforce son taux d’efficacité) pour être le plus efficace possible.

