using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber;

public class AlgrorithmOlemskogo
{

    #region Variables and Sets
    /// <summary>
    /// Размерность заданной матрицы
    /// </summary>
    /// 
    private bool IsWrite { get; set; } = false;
    private int Size { get; init; }
    public int V { get; set; } = 0;
    private int[,] Matrix { get; init; }

    public int VCount = 0;
    public int CountA = 0;
    public int CountB = 0;
    public int CountC = 0;

    /// <summary>
    /// Множество индексов
    /// </summary>
    private SetInt I { get; set; } = new();

    /// <summary>
    /// Мощность множества, образованного парой G[j][s][0]
    /// </summary>
    private Dictionary<Pair, int> AlphaPower = new();

    private int LastBeta = new();

    /// <summary>
    /// Множество, показывающее симметричность раположения нулей относительно диагоналей
    /// </summary>
    private List<List<Dictionary<Pair, SetInt>>> D { get; set; } = new();

    /// <summary>
    /// Опорное множество, состоящие из еще не рассмотренных вершин на шаге js 
    /// </summary>
    private List<List<SetInt>> W { get; set; } = new();

    /// <summary>
    /// Опорное множество, состоящее из уже рассмотренных вершин на шаге js
    /// </summary>
    private List<List<SetInt>> F { get; set; } = new();

    /// <summary>
    /// Множество, состоящее из пар (q, r), таких, что пересечение Wjs и Djs не пустое. 
    /// Множество упорядоченно по мощностям пересечений Wjs и Djs
    /// </summary>
    private List<List<Set<Pair>>> G { get; set; } = new();

    /// <summary>
    /// Множество, выполняющее функцию памяти об использовании элементов Gjs на ветви j и уровне s
    /// </summary>
    private List<List<Set<Pair>>> Q { get; set; } = new();

    /// <summary>
    /// Узловой элемент, выбранный для j-ой ветви и s-го уровня
    /// </summary>
    private List<List<SetInt>> Alpha { get; set; } = new();

    /// <summary>
    /// Множество перспективных элементов, для обратного хода
    /// </summary>
    private List<List<SetInt>> Psi { get; set; } = new();


    private SetInt B { get; set; } = new();

    /// <summary>
    /// Множество неперспективных элементов для ветви j и уровня s
    /// </summary>
    private List<List<Set<Pair>>> Z { get; set; } = new();
    private List<List<int>> Ro { get; set; } = new();

    /// <summary>
    /// Минимально возможное разбиение вершин на цвета
    /// </summary>
    private List<SetInt> J { get; set; } = new();

    /// <summary>
    /// Текущая ветвь
    /// </summary>
    private int jey = 0;
    private int Jey { 
        get
        {
            if (jey < 0)
                jey = 0;
            return jey;
        }
        set
        {
            //j = value;
            if (value < 0)
                jey = 0;
            else jey = value;
        } 
    }

    /// <summary>
    /// Уровень ветви j
    /// </summary>
    private int s = 0;
    private int Ss {
        get
        {
            if (s < 0)
                s = 0;
            return s;
        }
        set
        {
            s = value;
            if (s < 0)
                s = 0;
        }
    }

    /// <summary>
    /// Число пройденных уровней по ветви j
    /// </summary>
    public List<int> R { get; set; } = new();

    #endregion

    /// <summary>
    /// Конструктор. Принимает в себя квадратную (0,1)-матрицу - матрицу смежности графа
    /// </summary>
    /// <param name="matrix"></param>
    public AlgrorithmOlemskogo(int[,] matrix, bool isWrite = false)
    {
        IsWrite= isWrite;
        Jey = 0;
        Ss = 0;
        Matrix = matrix;
        Size = matrix.GetLength(0);

        //Заполняем множество индексов
        for (int i = 0; i < Size; i++)
            I.Add(i);

        V = Size;
    }
    public void Start() =>
         PointOne();

    private void InitializeF()
    {
        while (F.Count <= Jey)
            F.Add(new());

        while (F[Jey].Count <= Ss)
            F[Jey].Add(new());
    }

    //Инициализируем Q[j][s]
    private void InitializeQ()
    {
        //Если для текущей ветви не созданно множество Q[j], то добавляем новое
        if (Q.Count == Jey)
            Q.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Q[j][s], то добавляем новое множество
        if (Q[Jey].Count == Ss)
            Q[Jey].Add(new());
    }

    /// <summary>
    /// Вычисляем W[j][s]
    /// </summary>
    //TODO: Переделать на вычисление по индексам
    private void CalculateW()
    {
        //Если для текущей ветви не созданно множество Wj, то добавляем новое
        if (W.Count <= Jey)
            W.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Wjs, то добавляем новое множество
        if (W[Jey].Count <= Ss)
            W[Jey].Add(new());


        W[Jey][Ss].Clear();

        var result = I;

        //Вычисляем Wjs как разность между элементами J (т.е. множеством текущих раскрасок)
        //и множестом всех индексов
        //foreach(var temp in J)
        //{
        //    temp.Sort(0, temp.Count-1);
        //    result = SetInt.Difference(result, temp);
        //}

        W[Jey][Ss] = result;

        if (IsWrite)
        {
            Console.WriteLine($"W[{Jey+1}][{Ss+1}]");
            foreach (var t in W[Jey][Ss])
                Console.Write(t+1 + " ");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Вычисляем W[j][s+1]
    /// </summary>
    private void NextSCalculateW()
    {
        var d = D[Jey][Ss];
        W[Jey].Add(new());
        Pair pair = new(Alpha[Jey][Ss][0], Alpha[Jey][Ss][1]);

        //Убираем Alpha[j][s] из пересечения W[j][s] и D(j, s)
        SetInt result = SetInt.Difference(
            SetInt.Intersection(W[Jey][Ss], d[pair]),
            Alpha[Jey][Ss]);

        //Записываем результат в W[j][s+1]
        W[Jey][Ss + 1] = result;
        FillD(Jey, Ss+1);
        if (IsWrite)
        {
            Console.WriteLine($"W[{Jey+1}][{Ss+2}]");
            foreach (var t in W[Jey][Ss+1])
                Console.Write(t+1 + " ");
            Console.WriteLine();
        }
    }
    private void CalculateG()
    {
        //Если для текущей ветви не созданно множество Gj, то добавляем новое
        if (G.Count <= Jey)
            G.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Gjs, то добавляем новое множество
        if (G[Jey].Count <= Ss)
            G[Jey].Add(new());

        //Очищаем множество Gjs
        G[Jey][Ss].Clear();

        //Вычисляем подходящие пары (q, r), перебирая пересечения элементов множества D с элементами множества W[j][s]
        foreach (var d in D[Jey][Ss])
        {
            //SetInt set = new();
            ////Добавляем во временное множество пару (q, r)
            //set.Add(d.Key.First);
            //set.Add(d.Key.Second);
            ////Проверяем, что пара элементов (q, r) содержится в множестве доступных значений W[j][s]
            //var temp = SetInt.Intersection(set, W[j][s]);
            //if (temp.Count == 2)
            //{
            //    //Пересечение элемента D и W[j][s]
            //    var setTwo = SetInt.Intersection(W[j][s], d.Value);
            //    if (setTwo.Count > 0)
            //    {
            //        //Добавляем в Gjs пару (q, r) и мощность пересечений D и Wjs
            //        Pair pair = new(d.Key.First, d.Key.Second, setTwo.Count);
            //        G[j][s].Add(pair);
            //    }
            //}

            Pair pair = new(d.Key.First, d.Key.Second, d.Value.Count);
            G[Jey][Ss].Add(pair);
        }

        //Сортируем Gjs по мощностям
        G[Jey][Ss].Sort(0, G[Jey][Ss].Count - 1);
        if (IsWrite)
        {
            Console.WriteLine($"G[{Jey+1}][{Ss+1}]");
            int countG = 1;
            for (int i = 0; i < G[Jey][Ss].Count; i++)
            {
                Console.WriteLine($"{countG++}. ({G[Jey][Ss].Array[i].First+1}, {G[Jey][Ss].Array[i].Second+1}) |\t {G[Jey][Ss].Array[i].Power}");
            }
            Console.WriteLine();
        }
    }

    //Добавляем Alpha[j][s]
    private void CalculateAlpha(Pair pair)
    {
        //Если для текущей ветви не созданно множество Alpha[j], то добавляем новое
        if (Alpha.Count <= Jey)
            Alpha.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Alpha[j][s], то добавляем новое множество
        if (Alpha[Jey].Count <= Ss)
            Alpha[Jey].Add(new());

        Alpha[Jey][Ss].Clear();
        //Записываем в Alpha[j][s] индексы пары максимальной мощности и G[j][s] без Q[j][s]
        Alpha[Jey][Ss].Add(pair.First);
        Alpha[Jey][Ss].Add(pair.Second);
        //AlphaPower.Add(new(pair.First, pair.Second), pair.Power);
    }
    private void CalculatePsi(int r)
    {
        //Если для текущей ветви не созданно множество Psi[j], то добавляем новое
        while (Psi.Count <= Jey)
            Psi.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Psi[j][s], то добавляем новое множество
        while (Psi[Jey].Count <= r)
            Psi[Jey].Add(new());

        SetInt set = new();
        //Вычисялем Psi[j][s] как J[j] без Всех Alpha[j]
        for (int i = 0; i < r; i++)
            set = SetInt.Union(set, Alpha[Jey][i]);

        //set.Sort(0, set.Count - 1);

        //Psi[Jey][r] = SetInt.Difference(J[Jey], set);
        //Psi[Jey][r].Sort(0, Psi[Jey][r].Count - 1);

        int countPsi = 1;
        if (IsWrite)
        {
            Console.WriteLine($"Psi[{Jey+1}][{r+1}]");
            for (int i = 0; i < Psi[Jey][r].Count; i++)
            {
                var p = Psi[Jey][r][i];
                Console.WriteLine($"{countPsi++}.  {p + 1}");
                //var f = w[1][2];
            }
            //V = Size;
            Console.WriteLine();
        }
    }
    private void CalculateZ(int r)
    {
        //Если для текущей ветви не созданно множество Z[j], то добавляем новое
        while (Z.Count <= Jey)
            Z.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Z[j][s], то добавляем новое множество
        while (Z[Jey].Count <= r)
            Z[Jey].Add(new());

        //Элементы множества G, не рассмотренные на шаге [j][s]
        //CalculateG();
        //InitializeQ();

        var t = Set<Pair>.Difference(G[Jey][r], Q[Jey][r], false);

        for (int i = 0; i < t.Count; i++)
            t[i] = new(t[i].First, t[i].Second);

        t.Sort(0, t.Count - 1);

        var temp = Set<Pair>
            .Intersection(t, GeneratePairSet(r));
        //Z[j][r] = temp;
        if (r != 0)
            Z[Jey][r] = temp;

        else
        {
            Set<Pair> result = new();
            var d = D[Jey][r];

            for (int i = 0; i < temp.Count; i++)
                if (SetInt.Difference(Psi[Jey][r], d[temp[i]]).Count == 0
                    && SetInt.Difference(d[temp[i]], Psi[Jey][r]).Count == 0)
                    result.Add(temp[i]);


            Z[Jey][r] = result;
        }

        int countZ = 1;
        if (IsWrite)
        {
            Console.WriteLine($"Z[{Jey+1}][{r+1}]");
            for (int i = 0; i < Z[Jey][r].Count; i++)
            {
                var p = Z[Jey][r][i];
                Console.WriteLine($"{countZ++}.  ({p.First + 1}, {p.Second+1}) ");
                //var f = w[1][2];
            }
            //V = Size;
            Console.WriteLine('\t');
        }
    }

    //Вычисляем B, как объединение B и Psi[0][0]
    private void CalculateB() =>
        B = SetInt.Union(B, Psi[0][0]);

    private int CalculateRo(int j, int s)
    {
        var t = Set<Pair>.Difference(G[j][s], Q[j][s], false);

        var pair = new Pair(Alpha[j][s][0], Alpha[j][s][1]);
        if (t.Count > 0)
            return D[j][s][pair].Count;
        else
            return 1;
        //if (Ro.Count <= j)
        //    Ro.Add(new());

        //Set<Pair> set = Set<Pair>.Difference(G[0][0], Q[0][0]);

        //if (Ro[0].Count <= s)
        //    if (set.Count > 0)
        //        Ro[0].Add(res);
        //    else Ro[0].Add(1);
    }
    private bool CheckA()
    {
        var ro = CalculateRo(Jey, 0);
        var powerW = W[Jey][0].Count;
        var res = Jey + (double)powerW / ro;
        if (res < V)
            return false;

        return true;
        //if
        //var mod = powerW % ro;
        //var div = powerW / ro;
        //if (mod == 0 && div == V || mod != 0 && div == V - 1)
        //    return true;
        //else return false;
    }
    private bool CheckAWithReadyRo()
    {
        var ro = 1;
        var powerW = W[Jey][0].Count;
        var res = Jey + (double)powerW / ro;
        if (res < V)
            return false;

        return true;
        //if
        //var mod = powerW % ro;
        //var div = powerW / ro;
        //if (mod == 0 && div == V || mod != 0 && div == V - 1)
        //    return true;
        //else return false;
    }
    private bool CheckB()
    {
        //return true;
        var ro = CalculateRo(0, Ss);
        if (2 * Ss + ro > (Size / V))
            return true;

        CountB++;
        var t = false;
        return t;
    }
    private bool CheckBWithReadyRo()
    {
        //return true;
        var ro = 1;
        if (2 * Ss + ro > (Size / V))
            return true;

        CountB++;
        var t = false;
        return t;
    }
    private bool CheckC()
    {
        //return true;
        var ro = CalculateRo(Jey, Ss);
        if (2 * Ss + ro == W[Jey][0].Count)
            return true;

        CountC++;
        return false;
    }
    private bool CheckCWithReadyRo()
    {
        //return true;
        var ro = 1;
        if (2 * Ss + ro == W[Jey][0].Count)
            return true;

        CountC++;
        return false;
    }

    /// <summary>
    /// Множество пар (q, r) таких, что (q, r) принадлежит Psi[j][s] x Psi[j][s]
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    private Set<Pair> GeneratePairSet(int r)
    {
        Set<Pair> result = new();

        for (int i = 0; i < Psi[Jey][r].Count - 1; i++)
        {
            for (int k = i + 1; k < Psi[Jey][r].Count; k++)
            {
                result.Add(new(Psi[Jey][r][i], Psi[Jey][r][k]));
            }
        }
        result.Sort(0, result.Count - 1);
        return result;
    }

    /// <summary>
    /// 1. Вычисляем W[j][1]
    /// 2. Инициализируем F[j][1]
    /// 3. Переход к шагу 2
    /// </summary>
    private void PointOne()
    {
        CalculateW();
        FillD(Jey, Ss);
        InitializeF();
        PointTwo();
    }

    /// <summary>
    /// 1. Проверяем мощность W[j][s]
    /// 2. Идем к пункте 3, 5 или 8
    /// </summary>
    private void PointTwo()
    {
        //Проверяем количество доступных индексов
        if (W[Jey][Ss].Count == 0)
        {
            //Если находимся на первом уровне, переходим к шагу 8 иначе к шагу 5
            if (Ss == 0)
                PointEight();
            else
                PointFive(true);
        }
        //Переходим на шаг 3
        else PointThree();
    }

    /// <summary>
    /// 1. Формируем G[j][s]
    /// 2. Формируем Q[j][s]
    /// </summary>
    private void PointThree()
    {
        //Вычисляем Gjs
        CalculateG();
        //Инициализируем Q
        InitializeQ();

        //Переходим на шаг 4
        PointFour();
    }

    /// <summary>
    /// I. G[j][s]/Q[j][s] != 0
    ///     1. Находим Alpha[j][s] из доступных эл-ов 
    ///     2. Строим W[j][s+1] 
    ///     3. Строим F[j][s+1]
    ///     4. Добавляем Alpha[j][s] в Q[j][s]
    ///     5. Переходим на пункт 2
    /// II. G[j][s]/Q[j][s] = 0, W[j][s]/F[j][s] != 0
    ///     1. Beta = W[j][s]/F[j][s]
    ///     2. Добавляем в F[j][s] Beta
    ///     3. Идем к пункте 5
    /// III. G[j][s]/Q[j][s] = 0, W[j][s]/F[j][s] = 0
    ///     1. s--
    ///     2. Идем к пункту 4
    /// </summary>
    private void PointFour()
    {
        //Вычисляем разность между Qjs и Gjs, т.е. количество не доступных пар из Gjs
        Set<Pair> set = Set<Pair>.Difference(
            G[Jey][Ss],
            Q[Jey][Ss],
            false);

        if (set.Count > 0)
        {
            set.Sort(0, set.Count-1);
            //Находим Alpha[j][s]
            CalculateAlpha(set[0]);
            if (Ss == 0)
                if (CheckA())
                {
                    //Ss = R[Jey];
                    Jey--;
                    RS();
                    PointFour();
                    return;
                }
            if (Jey == V - 2 && !CheckC())
            {
                VCount++;
                Jey--;
                //Ss = R[Jey];
                RS();
                PointFour();
                return;
            }

            //CalculateRo(set[0].Power);
            //Добавляем Alpha[j][s] в Q[j][s]
            if (Jey == 0 && !CheckB())
            //if(false)
            {
                VCount++;
                Ss -= 1;
                PointFour();
                return;
            }

            Q[Jey][Ss] = Set<Pair>.Union(Q[Jey][Ss], set[0]);
            //Убираем Alpha[j][s] из W[j][s+1]
            NextSCalculateW();
            //Переходим на уровень s+1 ветви j
            VCount++;
            Ss++;
            PointTwo();

            
        }
        else
        {
            InitializeF();
            SetInt beta = new();
            if (F[Jey][Ss].Count > 0)
                beta = SetInt.Difference(W[Jey][Ss], F[Jey][Ss]);
            else
                beta = W[Jey][Ss];

            if (beta.Count > 0)
            {
                F[Jey][Ss] = SetInt.Union(F[Jey][Ss], beta[0]);
                LastBeta = beta[0];
                if (Ss == 0)
                {
                    //CalculateAlpha(new(beta[0], beta[1]));
                    if (Jey != 0 && CheckAWithReadyRo())
                    {
                        
                        Jey--;
                        //Ss = R[Jey];
                        RS();
                        PointFour();
                        return;
                    }
                }
                if (Jey == 0 && !CheckBWithReadyRo()) 
                {
                    Ss -= 1;
                    PointFour();
                    return;
                }
                if (Jey == V - 2 && !CheckCWithReadyRo())
                {
                    Jey--;
                    //Ss = R[Jey];
                    RS();
                    PointFour();
                    return;
                }

                PointFive(false);
            }
            else
            {
                //Jey--;
                //RS();
                //PointFour();
                //PointFour();
                //if (Ss == 0)
                //{
                //    var t = new SetInt();
                //    for (var i = 0; i <= Jey; i++)
                //    {
                //        var temp = J[i];
                //        t = SetInt.Union(temp, t);
                //    }
                //    if (t.Count == Size)
                //    {
                //        Jey+=1;
                //        PointEight(); 
                //    }
                //}
                Ss -= 1;
                PointFour();
                //if (J[Jey].Count/2 == 1)
                //{
                //    Ss = J[Jey].Count/2;
                //}
                //else 
                //    Ss = J[Jey].Count/2 - 1;
                //if (Ss == 0)
                //{
                //    Console.WriteLine($"ANSWER - {V}");
                //    return;
                //}
                //VCount++;
                //Ss--;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isEven"></param>
    private void PointFive(bool isEven)
    {
        if (R.Count <= Jey)
            R.Add(Ss);

        if (J.Count <= Jey)
            J.Add(new());

        J[Jey].Clear();
        //Добавляем в J все Alpha полученные на j-ой ветви
        for (int i = 0; i < Ss; i++)
        {
            J[Jey].Add(Alpha[Jey][i][0]);
            J[Jey].Add(Alpha[Jey][i][1]);
        }

        //Если количество элементво, образующий j-ый нульдиагональный блок нечетно, то R[j] - 
        if (!isEven)
        {
            //R[j] = s - 1;
            //var beta = SetInt.Difference(W[j][s], F[j][s]);
            J[Jey] = SetInt.Union(J[Jey], LastBeta);
            R[Jey] = J[Jey].Count / 2;
        }
        else R[Jey] = J[Jey].Count / 2 - 1;
        //else R[j] = s;

        PointSix();
    }
    private void PointSix()
    {
        var tempR = J[Jey].Count/2-1; 
        while (tempR >= 0)
        {
            //Вычисляем Psi[j] и Z[j] для 0 <= s < r
            CalculatePsi(tempR);
            CalculateZ(tempR);

            //Добавляем неперспективные элементы в множество уже использованных для этого уровня элементов
            //при построении j−го нульдиагональ-ного блока
            Q[Jey][tempR] = Set<Pair>.Union(Q[Jey][tempR], Z[Jey][tempR]);

            tempR--;
        }

        CalculateB();

        PointSeven();
    }
    private void PointSeven()
    {
        //Q[j][0] = new();
        //F[j][0] = new();

        VCount++;
        Jey++;
        Ss = 0;

        PointOne();
    }
    private void PointEight()
    {
        VCount++;
        Jey -= 1;
        V = V < Jey+1 ? V : Jey+1;

        //var t = CheckA();
        if (false)
        {
            Ss = R[Jey];
            PointFour();
        }
        else
        {
            var ro = CalculateRo(0, 0);
            //Console.WriteLine(ro);
            var g = W[0][0].Count % ro;
            //Console.WriteLine(g);

            //var g = W[0][0].Count % Ro[0][0];
            
            int y = W[0][0].Count / ro;
            //Console.WriteLine(y);
            var t = SetInt.Difference(W[0][0], F[0][0]);
            if (g == 0 && y == V || g != 0 && y == V - 1 || t.Count <= 0)
            {
                Console.WriteLine($"ANSWER - {V}");
                return;
            }
            else
            {
                //Ss = R[Jey];
                RS();
                PointFour();
            }
        }
    }

    /// <summary>
    /// Вычисляем q-ое горизнотальное структурное множество (такие индексы q-ой строки, что Aqr = 0) 
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    private SetInt H(int q)
    {
        SetInt result = new();

        for (int r = 0; r < Size; r++)
        {
            if (Matrix[q, r] == 0) result.Add(r);
        }

        return result;
    }

    /// <summary>
    /// Вычисляем q-ое вертикальное структурное множество (такие индексы q-ого столбца, что Aqr = 0) 
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    private SetInt U(int r)
    {
        SetInt result = new();

        for (int q = 0; q < Size; q++)
        {
            if (Matrix[q, r] == 0) result.Add(q);
        }

        return result;
    }

    /// <summary>
    /// Заполняем множество D(q, r)
    /// </summary>
    private void FillD(int ji, int s)
    {
        //Если для текущей ветви не созданно множество Dj, то добавляем новое
        if (D.Count <= ji)
            D.Add(new());

        //Если для текущего уровня j-ой ветви не созданно Djs, то добавляем новое множество
        if (D[ji].Count <= s)
            D[ji].Add(new());

        //Очищаем множество Djs
        D[ji][s].Clear();
        var w = W[Jey][s];
        var d = new Dictionary<Pair, SetInt>();
        for (int i = 0; i < Size - 1; i++)
        {
            for (int j = i + 1; j < Size; j++)
            {
                //var t = W;
                //var t1 = W[ji];
                //var t2 = W[ji][s];
                //if (!(W[ji][s].Contains(i) && W[ji][s].Contains(j)))
                //{
                //    continue;
                //}
                var set = SetInt
                    .Intersection(
                        SetInt.Intersection(H(i), H(j)),
                        SetInt.Intersection(U(i), U(j)));

                set = SetInt.Intersection(set, w);

                var count = set.Count;

                //if (set != null
                //    && set.Find(i, 0, count - 1) >= 0
                //    && set.Find(j, 0, count - 1) >= 0)
                //{

                //    d.Add(new Pair(i, j), set);
                //}
            }
        }
        D[ji][s] = d;
        int countD = 1;
        if (IsWrite)
        {
            Console.WriteLine($"D[{ji+1}][{s+1}]");
            foreach (var p in d)
            {
                Console.Write($"{countD++}. ({p.Key.First + 1}, {p.Key.Second + 1}) |\t");
                Console.Write(p.Value.Count + "|\t");
                for (int i = 0; i < p.Value.Count; i++)
                {
                    Console.Write(p.Value[i] + 1 + "|\t");
                }
                Console.WriteLine();
                //var f = w[1][2];
            }
            //V = Size;
            Console.WriteLine();
        }
    }
    private Dictionary<Pair, SetInt> GetCurrentD(int i, int j)
        => D[i][j];

    private void RS()
    {
        
        Ss = J[Jey].Count/2-1;

        return;
    }
}
