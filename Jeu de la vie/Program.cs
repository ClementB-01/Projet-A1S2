
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsilvGui;
namespace Jeu_de_la_vie
{
    class Program
    {
        // Ce programme est un Jeu de la Vie tel qu'imaginé par J.H. Conway
        // Il contient trois modes de jeu différent : un premier avec une seule population, un deuxième avec deux et un troisième qui modélise une épidemie de Sars-Cov-2 (COVID-19)
        // Dans les deux premiers mode, il est possible de visualiser les étapes futures, là où dans le troisième on peut introduire la notion de confinement
        // Les chiffres et résultats d'une épidemie de Sars-Cov-2 ne sont que des simulations très imparfaites et on serait bien incapable d'en tirer une quelconque conclusion
        // Voici les différentes significations des couleurs dans le Gui et des symboles de la console:
        //
        // 1) Jeu de la Vie avec une seule population : - # (noir) = cellule vivante
        //                                              - . (gris) = cellule morte
        //                                              - - (vert) = cellule à naître
        //                                              - * (rouge)= cellule à mourir
        //
        // 2) Jeu de la Vie avec deux populations : - # (noir)  = cellule vivante   (Population 1)
        //                                          - - (vert)  = cellule à naître  (Population 1)
        //                                          - * (rouge )= cellule à mourir  (Population 1)
        //                                          - + (bleu)  = cellule vivante   (Population 2)
        //                                          - ! (jaune) = cellule à naître  (Population 2)                                      Ce code est laissé ici pour faciliter votre correction
        //                                          - ? (orange)= cellule à mourir  (Population 2)
        //                                          - . (gris) = cellule morte
        //
        // 3) Jeu de la Vie COVID-19 : - + (bleu)  = cellule saine non immunisée
        //                             - . (gris)  = cellule morte ou n'ayant jamais vécu   (stade 4)
        //                             - - (vert)  = cellule saine immunisée
        //                             - ! (jaune) = cellule Asymptomatique                 (stade 0)
        //                             - ? (orange)= cellule Légers symptômes               (stade 1)
        //                             - * (rouge )= cellule Symptômes grippaux             (stade 2)
        //                             - # (noir)  = cellule Problèmes respiratoires graves (stade 3)        


        static int[] TestVoisinageRang(int[,] grille, int posX, int posY) // Méthode testant le voisinage d'une cellule au rangs 1 et 2
        {
            int[] nbnearcell = { 0, 0, 0, 0 };
            for (int x = posX - 1; x < posX + 2; x++)
            {
                for (int y = posY - 1; y < posY + 2; y++)
                {
                    if (y != posY | x != posX)
                    {
                        if (grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 1 | grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 2) //On utilise les modulo pour avoir une grille torique
                        {
                            nbnearcell[0]++;
                        }
                        if (grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 4 | grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 6)
                        {
                            nbnearcell[1]++;
                        }
                    }
                }
            }
            //Console.WriteLine("Coordonées : " + posX + " " + posY + "  " + "Nb near cell = " + nbnearcell[0]);
            //Console.WriteLine("Coordonées : " + posX + " " + posY + "  " + "Nb near cell = " + nbnearcell[1]);
            for (int x = posX - 2; x <= posX + 2; x++)
            {
                for (int y = posY - 2; y <= posY + 2; y++)
                {
                    if (grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 1 || grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 2)
                    {
                        nbnearcell[2]++;
                    }
                    else if (grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 4 || grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 6)
                    {
                        nbnearcell[3]++;
                    }
                }
            }
            return nbnearcell;
        }
        static void PredictionDeProchaineGen(int[,] grille, int choixdujeu) // Applique les règles du jeu en fonction du voisinage 
        {
            int[] nbnearcell;
            for (int posX = 0; posX < grille.GetLength(0); posX++)
            {
                for (int posY = 0; posY < grille.GetLength(1); posY++)
                {
                    nbnearcell = TestVoisinageRang(grille, posX, posY);
                    if (grille[posX, posY] == 1) // Règles R1b et R2B pour population 1
                    {
                        if (nbnearcell[0] < 2 || nbnearcell[0] > 3)
                        {
                            grille[posX, posY] = 3;
                        }
                    }
                    if (grille[posX, posY] == 4) // Règles R1b et R2B pour population 2
                    {
                        if (nbnearcell[1] < 2 || nbnearcell[1] > 3)
                        {
                            grille[posX, posY] = 6;
                        }
                    }
                    else
                    {
                        if (nbnearcell[0] == 3 && nbnearcell[1] != 3) // Règle R3b pour population 1
                        {
                            grille[posX, posY] = 2;
                        }
                        if (nbnearcell[1] == 3 && nbnearcell[0] != 3) // Règle R3b pour population 2
                        {
                            grille[posX, posY] = 5;
                        }
                        else if (nbnearcell[0] == 3 && nbnearcell[1] == 3)  // Règle R4b
                        {
                            //Console.WriteLine("Coucou c'est moi la troisième règle ou la R4B");
                            if (nbnearcell[2] > nbnearcell[3])
                            {
                                grille[posX, posY] = 2;
                            }
                            if (nbnearcell[3] > nbnearcell[2])
                            {
                                grille[posX, posY] = 5;
                            }
                            else if (nbnearcell[2] == nbnearcell[3])
                            {
                                //Console.WriteLine("Je suis dans le elsif");
                                int[] sommedespopulations = TaillePopulation(grille, choixdujeu);
                                if (sommedespopulations[1] > sommedespopulations[2])
                                {
                                    grille[posX, posY] = 2;
                                }
                                if (sommedespopulations[2] > sommedespopulations[1])
                                {
                                    grille[posX, posY] = 5;
                                }
                            }
                        }
                    }

                }
            }
        }
        static int[] DetectionPourInfectionCovid(int[,] grille, int posX, int posY) // Cette méthode détecte les cas sains et non immunisés au rang 1 d'une cellule infectée et retourne leurs coordonées
        {
            int[] position = { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            int compteurTours = 0;
            for (int x = posX - 1; x < posX + 2; x++)
            {
                for (int y = posY - 1; y < posY + 2; y++)
                {
                    if (y != posY | x != posX)
                    {
                        compteurTours++;
                        if (grille[(x + grille.GetLength(0)) % grille.GetLength(0), (y + grille.GetLength(1)) % grille.GetLength(1)] == 4) //On ne cherche à détecter que les cellules saines et non immunisées autour d'une cellule infectée
                        {
                            position[0]++;
                            position[(compteurTours * 2) - 1] = (x + grille.GetLength(0)) % grille.GetLength(0);
                            position[compteurTours * 2] = (y + grille.GetLength(1)) % grille.GetLength(1);
                        }
                    }
                }
            }
            return position;
        }
        static void ContaminationCovid(int[,] grille, int choixdujeu, double[] parametrage) // Cette méthode se charge d'infecter en moyenne 2,5 cellules au contact de rang 1 d'une cellule infectée
        {
            Random valeur = new Random();
            int r0;
            int[] position;
            for (int posX = 0; posX < grille.GetLength(0); posX++)
            {
                for (int posY = 0; posY < grille.GetLength(1); posY++)
                {
                    if (choixdujeu == 5)
                    {
                        if (grille[posX, posY] == 5 | grille[posX, posY] == 6 | grille[posX, posY] == 3 | grille[posX, posY] == 1) // On a besoin de détecter des cellules saines et non immunisée, uniquement si une cellule malade est là pour les infectées
                        {
                            //Console.WriteLine("Coucou");
                            position = DetectionPourInfectionCovid(grille, posX, posY);
                            if (position[0] == 1 || position[0] == 2 || position[0] == 3)
                            {
                                //Console.WriteLine(posX + " " + posY + " " + position[0]);
                                for (int i = 0; i < (position[0] * 2) - 1; i += 2)
                                {
                                    if (position[i + 1] != -1)
                                    {
                                        grille[position[i + 1], position[i + 2]] = -5;
                                        //Console.WriteLine(position[i + 1] + " " + position[i + 2]);
                                    }
                                }
                            }
                            else
                            {
                                //Console.WriteLine(posX + " " + posY + " " + position[0]);
                                for (int i = 0; i < (position[0] * 2) - 1; i += 2)
                                {
                                    r0 = valeur.Next(0, 11);
                                    if (r0 < (3 / Convert.ToDouble(position[0])) * 10)
                                    {
                                        if (position[i + 1] != -1)
                                        {
                                            grille[position[i + 1], position[i + 2]] = -5;
                                            //Console.WriteLine(position[i + 1] + " " + position[i + 2]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (choixdujeu == 6)
                    {
                        if (grille[posX, posY] == 5 || grille[posX, posY] == 6 || grille[posX, posY] == 3 || grille[posX, posY] == 1) // On a besoin de détecter des cellules saines et non immunisée, uniquement si une cellule malade est là pour les infectées
                        {
                            position = DetectionPourInfectionCovid(grille, posX, posY);
                            r0 = valeur.Next(0, 101);
                            if (position[0] == 1 || position[0] == 2 || position[0] == 3)
                            {
                                for (int i = 0; i < (position[0] * 2) - 1; i += 2)
                                {
                                    if (position[i + 1] != -1)
                                    {
                                        if (r0 < ((1 - parametrage[10]) * (1 - parametrage[11])) * 100)
                                        {
                                            grille[(position[i + 1]), (position[i + 2])] = -5;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < (position[0] * 2) - 1; i += 2)
                                {
                                    if (position[i + 1] != -1)
                                    {
                                        r0 = valeur.Next(0, 11);
                                        if (r0 > (3 / Convert.ToDouble(position[0])) / 10)
                                        {
                                            r0 = valeur.Next(0, 11);
                                            if (r0 > (parametrage[10] * parametrage[11]) * 10)
                                            {
                                                grille[(position[i + 1]), (position[i + 2])] = -5;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        static int PassageDeGenCovid(int[,] grille, int[,] chronologie, double[] parametrage) // Cette méthode assure l'aggravation et l'amélioration de l'état de la population déjà malade d'une génération à l'autre
        {
            Random valeur = new Random();
            int poidsstatistiques;
            int nbmorts = 0;

            for (int posX = 0; posX < grille.GetLength(0); posX++)
            {
                for (int posY = 0; posY < grille.GetLength(1); posY++)
                {
                    if (grille[posX, posY] != 0)
                    {
                        poidsstatistiques = valeur.Next(0, 11);
                        if (grille[posX, posY] == 1)
                        {
                            if (poidsstatistiques <= Convert.ToInt32(parametrage[9] * 10))
                            {
                                chronologie[posX, posY]--;
                                if (chronologie[posX, posY] == 0)
                                {
                                    grille[posX, posY] = 2;
                                }
                            }
                            else
                            {
                                grille[posX, posY] = 0;
                                nbmorts++;
                            }
                        }
                        else if (grille[posX, posY] == 3)
                        {
                            if (poidsstatistiques <= Convert.ToInt32(parametrage[8] * 10))
                            {
                                chronologie[posX, posY]--;
                                if (chronologie[posX, posY] == 0)
                                {
                                    grille[posX, posY] = 2;
                                }
                            }
                            else
                            {
                                grille[posX, posY] = 1;
                                chronologie[posX, posY] += 5;
                            }
                        }
                        else if (grille[posX, posY] == 6)
                        {
                            if (poidsstatistiques <= Convert.ToInt32(parametrage[7] * 10))
                            {
                                chronologie[posX, posY]--;
                                if (chronologie[posX, posY] == 0)
                                {
                                    grille[posX, posY] = 2;
                                }
                            }
                            else
                            {
                                grille[posX, posY] = 3;
                                chronologie[posX, posY] += 5;
                            }
                        }
                        else if (grille[posX, posY] == 5)
                        {
                            if (poidsstatistiques <= Convert.ToInt32(parametrage[6] * 10))
                            {
                                chronologie[posX, posY]--;
                                if (chronologie[posX, posY] == 0)
                                {
                                    grille[posX, posY] = 2;
                                }
                            }
                            else
                            {
                                grille[posX, posY] = 6;
                                chronologie[posX, posY] += 5;
                            }
                        }
                        else if (grille[posX, posY] == -5) // Cas de la primo-infection
                        {
                            grille[posX, posY] = 5;
                            chronologie[posX, posY] = 5;
                        }
                    }
                }
            }
            //Console.WriteLine(nbmorts);
            return nbmorts;
        }
        static void PassageDeGeneration(int[,] grille)  // S'occupe d'appliquer les passages de générations
        {
            for (int posX = 0; posX < grille.GetLength(0); posX++)
            {
                for (int posY = 0; posY < grille.GetLength(1); posY++)
                {
                    if (grille[posX, posY] == 2)
                    {
                        grille[posX, posY] = 1;
                    }
                    if (grille[posX, posY] == 5)
                    {
                        grille[posX, posY] = 4;
                    }
                    if (grille[posX, posY] == 3 || grille[posX, posY] == 6)
                    {
                        grille[posX, posY] = 0;
                    }
                }
            }
        }
        static bool[] NonNullNonVide(int[,] grille)
        {
            bool nul = false;
            bool vide = false;
            if (grille == null)
            {
                nul = true;
                Console.WriteLine("La matrice n'a pas d'allocation mémoire");
            }
            else if (grille.GetLength(0) * grille.GetLength(1) == 0)
            {
                vide = true;
                Console.WriteLine("La matrice est vide");
            }
            bool[] table = new bool[2];
            table[0] = nul;
            table[1] = vide;
            return table;
        } // Test de conformité sur la matrice       
        static int[,] GenererMatriceAleatoire(double[] parametrage, int choixdujeu) // Remplis la matrice initiale de manière totalement aléatoire
        {
            Random valeur = new Random();
            int[,] grille = new int[Convert.ToInt32(parametrage[1]), Convert.ToInt32(parametrage[2])];
            int cell;
            int population;
            for (int posX = 0; posX < grille.GetLength(0); posX++)
            {
                for (int posY = 0; posY < grille.GetLength(1); posY++)
                {
                    cell = valeur.Next(0, 11);
                    if (cell <= Convert.ToInt32(parametrage[3] * 10))
                    {
                        if (choixdujeu == 1 || choixdujeu == 2)
                        {
                            grille[posX, posY] = 1;
                        }
                        if (choixdujeu == 3 || choixdujeu == 4) // Permet de générer deux populations distinctes dont de manière équiprobable (50% de chances)
                        {
                            population = valeur.Next(0, 11);
                            if (population <= 5)
                            {
                                grille[posX, posY] = 4;
                            }
                            if (population > 5)
                            {
                                grille[posX, posY] = 1;
                            }
                        }
                        if (choixdujeu == 5 || choixdujeu == 6)
                        {
                            population = valeur.Next(0, 11);
                            if (population < Convert.ToInt32(parametrage[5] * 10))
                            {
                                grille[posX, posY] = 5;
                            }
                            else
                            {
                                grille[posX, posY] = 4;
                            }
                        }
                    }
                }
            }
            return grille;
        }
        static int Menu() // Permet au joueur de choisir le jeu désiré
        {
            int choixdujeu = 0;
            while (choixdujeu != 1 && choixdujeu != 2 && choixdujeu != 3 && choixdujeu != 4 && choixdujeu != 5 && choixdujeu != 6)
            {
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                 Choisissez votre manière de jouer                                    ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("||       1. Jeu de la vie classique sans visualisation intermédiaire des états futurs                               ||");
                Console.WriteLine("||       2. Jeu de la vie classique avec visualisation intermédiaire des états futurs (à naître et à mourir)        ||");
                Console.WriteLine("||       3. Jeu de la vie variante (deux populations) sans visualisation des états futurs               <- NEW !    ||");
                Console.WriteLine("||       4. Jeu de la vie variante (deux populations) avec visualisation des états futurs               <- NEW !    ||");
                Console.WriteLine("||       5. Jeu de la vie variante du COVID-19                                                          <- NEW !    ||");
                Console.WriteLine("||       6. Jeu de la vie variante du COVID-19 & Confinement                                            <- NEW !    ||");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("-> ?   ");
                choixdujeu = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }

            return choixdujeu;
        }
        static double[] Paramètres(int choixdujeu)  // Initialise tous les paramètres et permet au joueur de paramètrer sa partie
        {
            int parametre = 0;
            int iteration = 20;
            int[] dimensiongrille = { 20, 20 };
            double tauxdegeneration = 0.5;
            double automatisation = 0;
            double tauxdecontamination = 0.1;
            double t1 = 0.6;
            double t2 = 0.2;
            double t3 = 0.1;
            double t4 = 0.02;
            double t5 = 0.75;
            double t6 = 0.90;
            double tempsConfinement = 15;
            double[] parametrage = new double[13];
            int menuLogique = 5;
            if (choixdujeu == 5) { menuLogique = 6; }
            if (choixdujeu == 6) { menuLogique = 7; }


            while (parametre != menuLogique)
            {
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                            Choisissez de modifier vos paramètres                                     ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("||       1. Nombre d'itérations du programme                                                  [Par défaut = 20]     ||");
                Console.WriteLine("||       2. Dimension de la matrice                                                           [Par défaut = 20, 20] ||");
                Console.WriteLine("||       3. Taux de génération des populations (compris entre 0,1 et 0,9)                     [Par défaut = 0,5]    ||");
                Console.WriteLine("||       4. Passage manuel ou automatique des générations                                     [Par défaut = Man]    ||");
                if (choixdujeu == 5 || choixdujeu == 6) { Console.WriteLine("||       5. Taux de contamination initial                                                     [Par défaut = 0,1]    ||"); }
                if (choixdujeu == 6) { Console.WriteLine("||       6. Taux d'évolutions et paramètrage du confinement                                                         ||"); }
                Console.WriteLine("||       " + menuLogique + ". Sortir de la modification et lancer le jeu                                                              ||");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("-> ?   ");
                parametre = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                if (parametre == 1 && choixdujeu == 1)  // Cas du paramétrage du nombre d'itérations (forcément supérieur à 0)
                {
                    iteration = 0;
                    while (iteration <= 0)
                    {
                        Console.WriteLine("Nombre d'itérations?");
                        Console.WriteLine("-> ?   ");
                        iteration = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                    }
                }
                if (parametre == 2)  // Cas du paramétrage des dimensions de la matrice (forcément supérieure ou égale à 2x2)
                {
                    dimensiongrille[0] = 0;
                    dimensiongrille[1] = 0;

                    while (dimensiongrille[0] < 2)
                    {
                        Console.WriteLine("Hauteur ?  (Minimum de 2 cases) ");
                        Console.WriteLine("-> ?   ");
                        dimensiongrille[0] = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                    }
                    while (dimensiongrille[1] < 2)
                    {
                        Console.WriteLine("Longueur ? (Minimum de 2 cases) ");
                        Console.WriteLine("-> ?   ");
                        dimensiongrille[1] = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                    }
                }
                if (parametre == 3) // Cas du paramétrage du taux de génération de cellule vivante dans la matrice (compris entre 0.1 et 0.9 inclus)
                {
                    tauxdegeneration = 0;
                    while (tauxdegeneration < 0.1 || tauxdegeneration > 0.9)
                    {
                        Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                        Console.WriteLine("-> ?   ");
                        tauxdegeneration = Convert.ToDouble(Console.ReadLine());
                        Console.Clear();
                    }
                }
                if (parametre == 4) // Cas du paramétrage de l'automatisation ou non du passage de génération
                {
                    automatisation = 2;
                    while (automatisation != 0 && automatisation != 1)
                    {
                        Console.WriteLine("Manuel -> 0    ;    Automatique -> 1");
                        Console.WriteLine("-> ?   ");
                        automatisation = Convert.ToDouble(Console.ReadLine());
                        Console.Clear();
                    }
                }
                if (parametre == 5 && (choixdujeu == 5 || choixdujeu == 6)) // Cas du paramétrage du taux de contamination initial des cellules par le COVID-19
                {
                    tauxdecontamination = 0;
                    while (tauxdecontamination < 0.1 || tauxdecontamination > 0.9)
                    {
                        Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                        Console.WriteLine("-> ?   ");
                        tauxdecontamination = Convert.ToDouble(Console.ReadLine());
                        Console.Clear();
                    }
                }
                if (parametre == 6 && (choixdujeu == 6))
                {
                    int choixdutaux = 0;
                    while (choixdutaux != 8)
                    {
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                       Choisissez de modifier les taux de transition                                  ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("||       1. Taux de passage du stade 0 au stade 1                                           [Par défaut = 60 %]     ||");
                        Console.WriteLine("||       2. Taux de passage du stade 1 au stade 2                                           [Par défaut = 20 %]     ||");
                        Console.WriteLine("||       3. Taux de passage du stade 2 au stade 3                                           [Par défaut = 10 %]     ||");
                        Console.WriteLine("||       4. Taux de passage du stade 3 au stade 4                                           [Par défaut = 2  %]     ||");
                        Console.WriteLine("||       5. Taux de confinement des personnes saines                                        [Par défaut = 75 %]     ||");
                        Console.WriteLine("||       6. Taux de confinement des personnes infectées                                     [Par défaut = 90 %]     ||");
                        Console.WriteLine("||       7. Nombres de générations avant déconfinement                                      [Par défaut = 15  ]     ||");
                        Console.WriteLine("||       8. Sortir de la modification des taux et retour aux paramètres généraux                                    ||");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("-> ?   ");
                        choixdutaux = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        if (choixdutaux == 1)
                        {
                            t1 = 0;
                            while (t1 < 0.01 || t1 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t1 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 2)
                        {
                            t2 = 0;
                            while (t2 < 0.01 || t2 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t2 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 3)
                        {
                            t3 = 0;
                            while (t3 < 0.01 || t3 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t3 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 4)
                        {
                            t4 = 0;
                            while (t4 < 0.01 || t4 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t4 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 5)
                        {
                            t5 = 0;
                            while (t5 < 0.01 || t5 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t4 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 6)
                        {
                            t6 = 0;
                            while (t6 < 0.01 || t6 > 1)
                            {
                                Console.WriteLine("Merci de renseigner le taux sous la forme 0,x ");
                                Console.WriteLine("-> ?   ");
                                t4 = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                        if (choixdutaux == 7)
                        {
                            tempsConfinement = 0;
                            while (tempsConfinement <= 0)
                            {
                                Console.WriteLine("Merci de renseigner la durée du confinement : ");
                                Console.WriteLine("-> ?   ");
                                tempsConfinement = Convert.ToDouble(Console.ReadLine());
                                Console.Clear();
                            }
                        }
                    }
                }
            }

            parametrage[0] = iteration;
            parametrage[1] = dimensiongrille[0];
            parametrage[2] = dimensiongrille[1];
            parametrage[3] = tauxdegeneration;
            parametrage[4] = automatisation;
            parametrage[5] = tauxdecontamination;
            parametrage[6] = t1;
            parametrage[7] = t2;
            parametrage[8] = t3;
            parametrage[9] = t4;
            parametrage[10] = t5;
            parametrage[11] = t6;
            parametrage[12] = tempsConfinement;
            return parametrage;
        }
        static int[] TaillePopulation(int[,] grille, int choixdujeu) // Gestion des compteurs de population
        {
            int[] sommedespopulations = { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if (choixdujeu == 5 || choixdujeu == 6)
                    {
                        if (grille[i, j] == 5) { sommedespopulations[3]++; }
                        if (grille[i, j] == 6) { sommedespopulations[4]++; }
                        if (grille[i, j] == 3) { sommedespopulations[5]++; }
                        if (grille[i, j] == 1) { sommedespopulations[6]++; }
                        if (grille[i, j] == 2) { sommedespopulations[7]++; }
                    }
                    else
                    {
                        if (grille[i, j] == 1)
                        {
                            sommedespopulations[1]++;
                        }
                        if (grille[i, j] == 4)
                        {
                            sommedespopulations[2]++;
                        }
                    }
                    if (grille[i, j] != 0)
                    {
                        sommedespopulations[0]++;
                    }
                }
            }

            return sommedespopulations;
        }
        static void LeJeuDeLaVie(int[,] grille, int choixdujeu, double[] parametrage) // Méthode exécutant le jeu à proprement parler
        {
            char continuer;
            int[] sommedespopulations;
            int[,] grilleanalyse = grille; // Création de la matrice d'analyse de la grille
            int nbmorts = 0;
            int[,] chronologie = new int[grille.GetLength(0), grille.GetLength(1)];
            if (choixdujeu == 1 || choixdujeu == 2) // Affiche les règles de couleurs pour le mode 1
            {
                Console.WriteLine("    Voici les différentes significations des couleurs dans le Gui et des symboles de la console :                         ");
                Console.WriteLine("                                                                                                                          ");
                Console.WriteLine("                  1) Jeu de la Vie avec une seule population : - # (noir) = cellule vivante                               ");
                Console.WriteLine("                                                               - . (gris) = cellule morte                                 ");
                Console.WriteLine("                                                               - - (vert) = cellule à naître                              ");
                Console.WriteLine("                                                               - * (rouge)= cellule à mourir                              ");
            }
            if (choixdujeu == 3 || choixdujeu == 4) // Affiche les règles de couleurs pour le mode 2
            {
                Console.WriteLine("    Voici les différentes significations des couleurs dans le Gui et des symboles de la console :                         ");
                Console.WriteLine("                  2) Jeu de la Vie avec deux populations : - # (noir)  = cellule vivante   (Population 1)                 ");
                Console.WriteLine("                                                           - - (vert)  = cellule à naître  (Population 1)                 ");
                Console.WriteLine("                                                           - * (rouge )= cellule à mourir  (Population 1)                 ");
                Console.WriteLine("                                                           - + (bleu)  = cellule vivante   (Population 2)                 ");
                Console.WriteLine("                                                           - ! (jaune) = cellule à naître  (Population 2)                 ");
                Console.WriteLine("                                                           - ? (orange)= cellule à mourir  (Population 2)                 ");
                Console.WriteLine("                                                           - . (gris) = cellule morte                                     ");
            }
            if (choixdujeu == 5 || choixdujeu == 6) // Affiche les règles de couleurs pour le mode 3
            {
                Console.WriteLine("    Voici les différentes significations des couleurs dans le Gui et des symboles de la console :                         ");
                Console.WriteLine("                  3) Jeu de la Vie COVID-19 : - + (bleu)  = cellule saine non immunisée                                   ");
                Console.WriteLine("                                              - - (vert)  = cellule saine immunisée                                       ");
                Console.WriteLine("                                              - . (gris)  = cellule morte ou n'ayant jamais vécu   (stade 4)              ");
                Console.WriteLine("                                              - ! (jaune) = cellule Asymptomatique                 (stade 0)              ");
                Console.WriteLine("                                              - ? (orange)= cellule Légers symptômes               (stade 1)              ");
                Console.WriteLine("                                              - * (rouge )= cellule Symptômes grippaux             (stade 2)              ");
                Console.WriteLine("                                              - # (noir)  = cellule Problèmes respiratoires graves (stade 3)              ");
            }
            System.Threading.Thread.Sleep(4000);
            Console.Clear();
            Fenetre gui = new Fenetre(grille, 15, 0, 0, "Jeu de la vie");

            for (int compteurdegeneration = 0; compteurdegeneration < Convert.ToInt32(parametrage[0]); compteurdegeneration++) // Boucle permettant la répétition des générations jusqu'au nombre défini
            {
                continuer = 's';
                for (int i = 0; i < grille.GetLength(0); i++)
                {
                    for (int j = 0; j < grille.GetLength(1); j++)
                    {
                        grille[i, j] = grilleanalyse[i, j];
                    }
                }

                if (choixdujeu == 1 || choixdujeu == 2 || choixdujeu == 3 || choixdujeu == 4) // Applique les changements dans le cas des modes 1 & 2
                {
                    PassageDeGeneration(grille);
                }

                sommedespopulations = TaillePopulation(grille, choixdujeu);
                AffichageConsole(grille);
                gui.RafraichirTout();
                Console.WriteLine();
                Console.WriteLine("Génération n° : " + (compteurdegeneration + 1) + "    Taille totale de la population : " + sommedespopulations[0]);

                if (choixdujeu == 5 || choixdujeu == 6) // Applique les changements dans le cas du mode 3
                {
                    ContaminationCovid(grille, choixdujeu, parametrage);
                    nbmorts += PassageDeGenCovid(grille, chronologie, parametrage);
                }

                if (choixdujeu == 1 || choixdujeu == 2 || choixdujeu == 3 || choixdujeu == 4)
                {
                    if (choixdujeu == 3 || choixdujeu == 4) // Affichage de la taille des populations requises pour le mode 2
                    {
                        Console.WriteLine("Taille de la population n°1 : " + sommedespopulations[1] + "  " + "Taille de la population n°2 : " + sommedespopulations[2]);
                    }
                    PredictionDeProchaineGen(grille, choixdujeu);
                    if (choixdujeu == 2 || choixdujeu == 4)  // Affichage de la visualisation des états futurs
                    {
                        Console.WriteLine("Appuyer sur Entrée pour continuer et afficher la prévision");
                        while (Console.ReadKey().Key != ConsoleKey.Enter) { } // Permet une mise en pause pour regarder le précédent affichage
                        AffichageConsole(grille);
                        gui.RafraichirTout();
                        Console.WriteLine("Prévision pour génération n° : " + (compteurdegeneration + 2));
                    }
                }
                else // Donc si choixdujeu = 5 ou 6, c'est à dire dans le cas du COVID-19, on affiche alors les textes correspondant
                {
                    Console.WriteLine("Nombre de personnes (stade 0) : " + sommedespopulations[3] + "    " + "Nombre de personnes (stade 1) : " + sommedespopulations[4]);
                    Console.WriteLine("Nombre de personnes (stade 2) : " + sommedespopulations[5] + "    " + "Nombre de personnes (stade 3) : " + sommedespopulations[6]);
                    Console.WriteLine("Nombre de personnes immunisées : " + sommedespopulations[7] + "   " + "Nombre de morts totaux du virus : " + nbmorts);
                }

                if (parametrage[4] == 0) // Condition permettant l'avancée manuelle si celle-ci est sélectionnée dans les réglages
                {
                    while (continuer != 'y')  // Permet de vérifier que la grille est correcte et d'avancer pas à pas
                    {
                        string nonvide;
                        Console.WriteLine(" Voulez-vous continuer ? (y/n)  :  ");
                        Console.WriteLine("-> ?   ");
                        nonvide = Console.ReadLine();
                        if (nonvide != "") // Evite les erreurs si l'utilisateur appuie malencontreusement sur Enter...
                        {
                            continuer = Convert.ToChar(nonvide);
                        }
                        if (continuer == 'n') // Permet d'arrêter le programme si on ne souhaites pas aller au bout...
                        {
                            compteurdegeneration = Convert.ToInt32(parametrage[0]);
                            continuer = 'y';
                        }
                    }
                }
                else if (parametrage[4] == 1) { System.Threading.Thread.Sleep(1000); }
                if ((choixdujeu == 3 || choixdujeu == 4)) // Permet de détecter une stabilisation de la grille
                {
                    bool stabilise = false;
                    for (int i = 0; i < grille.GetLength(0); i++)
                    {
                        for (int j = 0; j < grille.GetLength(1); j++)
                        {
                            if (grille[i, j] != grilleanalyse[i, j]) { stabilise = true; }                            
                        }
                    }
                    if (sommedespopulations[0] == 0 | stabilise)
                    {
                        compteurdegeneration = Convert.ToInt32(parametrage[0]);
                        AffichageConsole(grille);
                        AffichageConsole(grilleanalyse);
                        Console.WriteLine(" Grille stabilisée : fin du jeu !");
                    }
                    
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Partie achevée ! Merci d'avoir jouer !"); // Affichage de fin de partie
        }
        [System.STAThreadAttribute()]
        static void Main(string[] args)
        {
            int[,] grille; /*= new int[5, 5]= { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };*/
            bool quitter = false; // Permet de proposer de relancer une partie
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                                                                                                                          ");
            Console.WriteLine("                                      Bonjour et Bienvenue sur le Jeu de la Vie                                           ");
            Console.WriteLine("                                      Conçu et développé par Hadrien & Clément                                            ");
            Console.WriteLine("                                                                                                                          ");
            Console.WriteLine("                                                                                                                          ");
            Console.WriteLine(" Ce programme est un Jeu de la Vie tel qu'imaginé par J.H. Conway                                                         ");
            Console.WriteLine(" Il contient trois modes de jeu différent :                                                                               ");
            Console.WriteLine(" Un premier avec une population, un deuxième avec deux et un troisième modélisant une épidemie de Sars-Cov-2 (COVID-19)   ");
            Console.WriteLine(" Dans les deux premiers modes, il est possible de visualiser les étapes futures,                                          ");
            Console.WriteLine(" là où dans le troisième on peut introduire la notion de confinement.                                                     ");
            Console.WriteLine(" Les chiffres et résultats d'une épidemie de Sars-Cov-2 ne sont que des simulations très imparfaites                      ");
            Console.WriteLine(" et on serait bien incapable d'en tirer une quelconque conclusion.                                                        ");
            Console.WriteLine("                                                                                                                          ");
            Console.WriteLine("                                                                                                                          ");
            Console.WriteLine(" Appuyer sur une touche pour continuer...                                                                                 ");
            Console.ReadKey();

            Console.Clear();
            while (quitter == false)
            {
                int choixdujeu = Menu();
                double[] parametrage = Paramètres(choixdujeu);
                grille = GenererMatriceAleatoire(parametrage, choixdujeu);
                LeJeuDeLaVie(grille, choixdujeu, parametrage);
                Console.WriteLine("Souhaitez-vous recommencer une partie (1) où bien quitter l'application (2) ? ");
                Console.WriteLine("-> ?   ");
                int quit = Convert.ToInt32(Console.ReadLine());
                if (quit == 2) { quitter = true; }
                else if (quit != 1)
                {
                    Console.WriteLine("On avait dit 1 ou 2... Tant pis on ferme !");
                    quitter = true;
                }
                Console.Clear();
            }
            Console.WriteLine("Appuyer sur n'importe quelle touche pour quitter...");
            Console.WriteLine("Nous vous souhaitons le meilleur et une bonne journée !");
            Console.ReadKey();
        }
        static void AffichageConsole(int[,] grille)
        {
            bool[] table = NonNullNonVide(grille);
            if (!table[0] && !table[1])
            {
                int dim0 = grille.GetLength(0);
                int dim1 = grille.GetLength(1);
                for (int x = 0; x < dim0; x++)
                {
                    string affichage = "";
                    for (int y = 0; y < dim1; y++)
                    {
                        if (grille[x, y] == 0)
                        {
                            affichage += "." + "  ";
                        }
                        if (grille[x, y] == 1)
                        {
                            affichage += "#" + "  ";
                        }
                        if (grille[x, y] == 2)
                        {
                            affichage += "*" + "  ";
                        }
                        if (grille[x, y] == 3)
                        {
                            affichage += "-" + "  ";
                        }
                        if (grille[x, y] == 4)
                        {
                            affichage += "+" + "  ";
                        }
                        if (grille[x, y] == 6)
                        {
                            affichage += "?" + "  ";
                        }
                        if (grille[x, y] == 5)
                        {
                            affichage += "!" + "  ";
                        }
                    }
                    Console.WriteLine(affichage);
                }
            }
        } //  Permet un affichage des caractères dans la console        
    }
} // Programme conçu et développé par H.M. et Clément B
  // <a rel="license" href="http://creativecommons.org/licenses/by-nc-sa/4.0/"><img alt="Licence Creative Commons" style="border-width:0" src="https://i.creativecommons.org/l/by-nc-sa/4.0/80x15.png" /></a><br />Ce(tte) œuvre est mise à disposition selon les termes de la <a rel="license" href="http://creativecommons.org/licenses/by-nc-sa/4.0/">Licence Creative Commons Attribution - Pas d’Utilisation Commerciale - Partage dans les Mêmes Conditions 4.0 International</a>.
