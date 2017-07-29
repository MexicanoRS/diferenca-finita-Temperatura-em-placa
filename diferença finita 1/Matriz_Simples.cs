﻿using System;

namespace diferença_finita_1
{
    partial class Program
    {
        /// <summary>
        /// Esta Classe define alguns tipos mais básicos de matrizes lineares
        /// </summary>
        public class Matriz_Simples : IMatriz_Simples
        {
            /// <summary>
            /// Controi as Mtrizes simples A[N,N] e B[N] e X[N].
            /// </summary>
            /// <param name="Número_de_Variáveis">Númeor de Variáveis da Matiriz</param>
            /// <param name="Dimensão_X">Ainda não implementado</param>
            public Matriz_Simples(int Número_de_Variáveis, ref Pontos Matriz_Fonte, decimal Dimensão_X = 1)
            {
                if (Número_de_Variáveis <= 0) throw new ArgumentOutOfRangeException("Número_de_Variáveis", Número_de_Variáveis, "Insira um númeor de Variáveis Acima de 0.");
                if (Dimensão_X != 1) throw new NotImplementedException("Ainda Não implementei para dimenões maiores de X[].");
                número_de_Variáveis = Número_de_Variáveis;
                matriz_Problema = Matriz_Fonte;
                a = new double[Número_de_Variáveis, Número_de_Variáveis];
                b = new double[Número_de_Variáveis];
                if (Dimensão_X == 1)
                {
                    x = new double[Número_de_Variáveis];
                }
            }

            private Pontos matriz_Problema;

            public Pontos Matriz_Problema
            {
                get { return matriz_Problema; }
                set { matriz_Problema = value; }
            }



            private double[,] a;
            /// <summary>
            /// Matriz de Coeficientes
            /// </summary>
            public double[,] A
            {
                get { return a; }
                set { a = value; }
            }

            private double[] x;
            /// <summary>
            /// Matriz de Incógnitas
            /// </summary>
            public double[] X
            {
                get { return x; }
                set { x = value; }
            }

            private double[] b;
            /// <summary>
            /// Matriz Solução
            /// </summary>
            public double[] B
            {
                get { return b; }
                set { b = value; }
            }

            private int número_de_Variáveis;
            /// <summary>
            /// Número de Variáveis da minha matriz
            /// </summary>
            public int Número_de_Variáveis
            {
                get { return número_de_Variáveis; }
            }

            /// <summary>
            /// Verifica se a matriz de Cálculo tem solução e se converge para ela.
            /// </summary>
            public void Verificar_critério_de_Sassenfeld()
            {
                int N = número_de_Variáveis;
                double[] Sassen = new double[N];

                // Instancia um array com todos os valores pré-setados em 1
                int contador = 0;
                foreach (double valor in Sassen)
                {
                    Sassen[contador] = 1;
                    contador++;
                }
                for (int i = 0; i < N; i++)
                {
                    double Novo_Sassen = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (i != j)
                        {
                            Novo_Sassen += Sassen[j] * Math.Abs(A[i, j]);
                        }
                    }
                    Sassen[i] = Novo_Sassen / Math.Abs(A[i, i]);
                }
                Console.Write("\n Para a Matriz de Cálculo convergir para a solução,\n o vetor de Sassenfeld tem que ter todos os valores menores que 1");
                Mostrar_Matriz(ref Sassen, "\n\n O vetor de Sassenfeld que foi encontrado é: \n\n");
                contador = 0;
                bool flag = false;
                foreach (double valor in Sassen)
                {
                    if (Sassen[contador] >= 1)
                    {
                        flag = true;
                    }
                    contador++;
                }
                if (flag)
                {
                    Console.Write("\n A Matriz Não Passou no critério de Sassenfeld\n\n");
                }
                else
                {
                    Console.Write("\n OK. A Matriz Passou no critério de Sassenfeld\n\n");
                }
                Console.Write("\n Tecle calquer tecla para continuar:\n");
                Console.ReadKey();
            }


            public void Solucionar_matriz(double Precisão=2,int Núm_MaxInterações = 10000)
            {
                // determina o número de variáveis da matriz de cálculo 
                int N = Número_de_Variáveis;
                int contador = 0;
                foreach (double valor in x)
                {
                    X[contador] = 1;
                    contador++;
                }

                double[] Erro = new double[Número_de_Variáveis];
                bool Para_interação = true;
                double ValordePrecisão = 1 / Math.Pow(10,Precisão+1);
                int k = 0;
                while (k < Núm_MaxInterações)
                {
                    for (int i = 0; i < Número_de_Variáveis; i++)
                    {
                        Erro[i] = X[i]; 
                        X[i] = (B[i] - Somat(i) + A[i, i] * X[i]) / A[i, i];
                        Erro[i] = Math.Abs(Erro[i]-X[i]);
                    }
                    Para_interação = true;
                    for (int i=0;i < Número_de_Variáveis; i++)
                    {
                        if (Erro[i] > ValordePrecisão)
                        {
                            Para_interação = false;
                            break;
                        }
                    }
                    if (Para_interação) break;
                    k++;
                }

                if (k <= Núm_MaxInterações)
                {
                    Console.Write( "\n\n A respostas com todos os valores do vetor X icónitas com {0} casas de precisão foi encontrada em {1} interações \n\n", Precisão, k);
                }
                else
                {
                    Console.Write("\n\n A respostas com todos os valores do vetor X icónitas com {0} casas de precisão Não foi encontrada em {1} interações \n\n", Precisão, k);
                }
                int Contador = 0;
                for (int i = 0; i < matriz_Problema.Linha.Length; i++)
                {
                    for (int j = 0; j < matriz_Problema.Linha[i].Coluna.Length; j++)
                    {
                        if (matriz_Problema.Linha[i].Coluna[j].nome >= 0)
                        {
                            matriz_Problema.Linha[i].Coluna[j].valor = X[Contador];
                            Contador++;
                        }
                    }
                }
                Mostrar_Matriz(ref matriz_Problema, "\n\n A mantiz final foi: \n\n\n\n");
            }

            private double Somat(int i)
            {

                double soma = 0;
                for (int j = 0; j < Número_de_Variáveis; j++)
                {
                    soma += A[i, j] * X[j];
                }
                return soma;

            }

        }
    }
}

