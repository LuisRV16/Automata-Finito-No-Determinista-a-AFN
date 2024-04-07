using System.Reflection;
using System.Runtime.ConstrainedExecution;

class Program
{
    static int puroNumero(string q)
    {
        return int.Parse(q.Replace("q", ""));
    }

    static void Main(string[] args)
    {
        string[] q = { "q0", "q1", "q2", "q3" };
        string[] alfabeto = { "0", "1" };
        string[] f = {"q3"};
        string q0 = q[0];

        int filas = q.Length;
        int columnas = alfabeto.Length;

        HashSet<string>[,] tran = new HashSet<string>[filas, columnas];

        for (int i = 0; i < filas; i++)
            for (int j = 0; j < columnas; j++)
                tran[i, j] = new HashSet<string>();  // Se inicializa cada elemento del arreglo

        // Llenado de la tabla de transicion de estados
        //     |-----------------------|------------------------|
        //     |        0              |            1           |
        //     |-----------------------|------------------------|
        tran[0, 0].Add(q[0]); tran[0, 1].Add(q[0]);
        tran[0, 0].Add(q[2]); tran[0, 1].Add(q[3]);
        //     |-----------------------|------------------------|
        tran[1, 0].Add(q[1]); tran[1, 1].Add(q[0]);
        tran[1, 0].Add(q[2]); tran[1, 1].Add(q[1]);
        tran[1, 1].Add(q[2]);
        //     |-----------------------|------------------------|
        tran[2, 0].Add(q[1]); tran[2, 1].Add(q[0]);
        tran[2, 1].Add(q[1]);
        //     |-----------------------|------------------------|
        tran[3, 0].Add(q[0]); tran[3, 1].Add(q[3]);
        tran[3, 0].Add(q[2]);
        //     |-----------------------|------------------------|


        Console.WriteLine("Q = {" + string.Join(", ", q) + "}");
        Console.WriteLine("Σ = {" + string.Join(", ", alfabeto) + "}");
        Console.WriteLine("q0 = " + q0);
        Console.WriteLine("F = {" + string.Join(", ", f) + "}\n");

        Console.WriteLine("Matriz de transicion de estados");
        for (int i = 0; i < filas; i++)
        {
            string s1 = "{" + string.Join(", ", tran[i, 0]) + "}";
            string s2 = "{" + string.Join(", ", tran[i, 1]) + "}";
            Console.WriteLine(string.Format("{0,-10} {1} {2,-10}", s1, "|", s2));
        }

        HashSet<string> q0P = new HashSet<string>();
        q0P.Add(q0);

        Console.WriteLine("\nq0' = [" + q0P.Last() + "]");

        List<HashSet<string>> d = new List<HashSet<string>>();
        d.Add(q0P);
        List<HashSet<string>> a = new List<HashSet<string>>();
        List<HashSet<string>> b = new List<HashSet<string>>();

        HashSet<string> dPeroEnString = new HashSet<string>();
        dPeroEnString.Add("[" + q0 + "]");

        for (int i = 0; i < d.Count; i++)
        {
            string[] tempA = d[i].ToArray();

            for (int j = 0; j < alfabeto.Length; j++)
            {

                HashSet<string> resultado = new HashSet<string>();

                for (int k = 0; k < tempA.Length; k++)
                {
                    int estado = puroNumero(tempA[k]);

                    resultado.UnionWith(tran[estado, j]);

                }

                List<string> resultadoOrdenado = resultado.OrderBy(estado => estado).ToList();

                if (dPeroEnString.Add("[" + string.Join(", ", resultadoOrdenado) + "]"))
                    d.Add(resultado);

                if (j == 0)
                    a.Add(resultado);
                else
                    b.Add(resultado);

                Console.WriteLine("d'([" + string.Join(", ", tempA) + "], " + j + ") = [" + string.Join(", ", resultado) + "]");

            }
        }

        HashSet<string>[] estados = d.ToArray();
        HashSet<string>[] a0 = a.ToArray();
        HashSet<string>[] b1 = b.ToArray();

        string[,] afn = new string[d.Count, 3];

        Console.WriteLine();
        for (int i = 0; i < estados.Length; i++)
            Console.WriteLine(string.Format("{0,-20} {1} {2,-20} {3} {4, -20}",
            "[" + string.Join(", ", estados[i]) + "]",
            "|",
            "[" + string.Join(", ", a0[i]) + "]",
            "|",
            "[" + string.Join(", ", b1[i]) + "]"));

        // Crear un HashMap (Dictionary<TKey, TValue>)
        Dictionary<string, int> mapa = new Dictionary<string, int>();

        for (int i = 0; i < estados.Length; i++)
            mapa["[" + string.Join(", ", estados[i]) + "]"] = i;

        string estadosRenombrados = "Q' = {";

        for (int i = 0; i < estados.Length; i++)
            if (i < estados.Length - 1)
                estadosRenombrados += "r" + mapa["[" + string.Join(", ", estados[i]) + "]"] + ", ";
            else
                estadosRenombrados += "r" + mapa["[" + string.Join(", ", estados[i]) + "]"] + "}";

        string estadosFinales = "F = {";

        HashSet<string> estadosF = new HashSet<string>();

        for (int i = 0; i < f.Length; i++)
            for (int j = 0; j < estados.Length; j++)
                if (estados[j].Contains(f[i])) 
                    estadosF.Add("r"+mapa["[" + string.Join(", ", estados[j]) + "]"]);

        Console.WriteLine("\n"+estadosRenombrados);
        Console.WriteLine("Σ = {" + string.Join(", ", alfabeto) + "}");
        Console.WriteLine("q0' = r" + mapa[dPeroEnString.First()]);
        Console.WriteLine(estadosFinales + string.Join(", ", estadosF) + "}");
        Console.WriteLine();
        for (int i = 0; i < estados.Length; i++)
            Console.WriteLine(string.Format("{0,-3} {1} {2,-3} {3} {4, -3}",
            "r" + mapa["[" + string.Join(", ", estados[i]) + "]"],
            "|",
            "r" + mapa["[" + string.Join(", ", a0[i]) + "]"],
            "|",
            "r" + mapa["[" + string.Join(", ", b1[i]) + "]"]));
    }
}